using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Linq;
using System.Collections;
using System.Collections.Concurrent;
using System;

public enum DeliveryType
{
    Reliable,
    Unreliable
}

public class MIKEServerManager : MonoBehaviour
{
    public static MIKEServerManager Main { get; private set; }

    public bool Connected { get; private set; } = true;

    private Socket socket;
    public IPEndPoint EndPoint { get { return endPoint; } }
    private IPEndPoint endPoint;
    private byte[] buffer = new byte[65536];
    private Queue<byte[]> dataToReceive = new Queue<byte[]>();
    private bool tasksRunning;

    public int ReliableSendCount { get { return reliableSendCount; } }
    private const int reliableSendCount = 30;
    private const float reliableSendInterval = 0.1f;
    private int reliableCounter = 0;

    public string OtherIP { get { return otherIP; } set { otherIP = value; } }
    [SerializeField] private string otherIP;
    [SerializeField] private int sendPort = 7777;
    [SerializeField] private int receivePort = 7777;

    // Start UDP server
    void Awake()
    {
        if (Main == null)
            Main = this;
        else
            Destroy(this);

        StartServer();
        SetEndPoint(otherIP);

        // Receiving
        /*Task.Run(async () =>
        {
            using (var udpClient = new UdpClient(receivePort))
            {
                while (tasksRunning)
                {
                    var receivedResults = await udpClient.ReceiveAsync();
                    dataToReceive.Enqueue(receivedResults.Buffer);
                    Debug.Log("Length: " + receivedResults.Buffer.Length);
                }
            }
        });*/
    }

    public void StartServer()
    {
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        socket.ReceiveBufferSize = 65536 * 2;
        socket.Bind(new IPEndPoint(IPAddress.Any, receivePort));
        tasksRunning = true;
        ReceiveData();
        Debug.Log("Server started.");
    }

    public void SetEndPoint(string ip)
    {
        OtherIP = ip;
        endPoint = new IPEndPoint(IPAddress.Parse(otherIP), sendPort);
        Debug.Log("IP set to: " + OtherIP);
    }

    private async void ReceiveData()
    {
        //var recvEP = new IPEndPoint(IPAddress.Any, receivePort);
        var segBuffer = new ArraySegment<byte>(buffer);
        while (tasksRunning)
        {
            try
            {
                Debug.Log("Waiting for data...");

                var receivedResults = await socket.ReceiveAsync(segBuffer, SocketFlags.None);
                byte[] data = segBuffer.Slice(0, receivedResults).ToArray();
                dataToReceive.Enqueue(data);

                //Debug.Log("Length: " + data.Length);
            }
            catch (ObjectDisposedException)
            {
                Debug.Log("Socket closed.");
            }
            catch (Exception e)
            {
                Debug.LogError("MIKEServerManager error when receiving data: " + e.Message);
            }
        }
    }

    void OnDisable()
    {
        tasksRunning = false;
        socket.Close();
        Debug.Log("Server stopped.");
    }

    void Update()
    {
        //Debug.Log("Data Count: " + dataToReceive.Count);
        if (dataToReceive.Count > 0)
        {
            Debug.Log("Receiving data...");
            MIKEInputManager.Main.ReceiveInput(dataToReceive.TryDequeue(out byte[] data) ? data : null);

            if (dataToReceive.Count > 40)
            {
                dataToReceive.Clear();
            }
        }
    }

    public async void SendData(ServiceType type, MIKEPacket packet, DeliveryType deliveryType)
    {
        if (deliveryType == DeliveryType.Reliable)
            await SendDataReliably(type, packet);
        else
            await SendData(type, packet, true);
    }

    private async Task SendData(ServiceType type, MIKEPacket packet, bool insertServiceType)
    {
        try
        {
            if (insertServiceType)
                packet.InsertAtStart((byte)((int)type));
            if (endPoint == null)
            {
                Debug.LogError("No endpoint set.");
                return;
            }
            await socket.SendToAsync(packet.ByteArray, SocketFlags.None, endPoint);
        }
        catch (Exception e)
        {
            Debug.LogError("MIKEServerManager error when sending data: " + e.Message);
        }
    }

    // Sends the packet multiple times to make sure it is received, I know this is stupid but I simply don't give a shit
    private async Task SendDataReliably(ServiceType type, MIKEPacket packet)
    {
        reliableCounter++;
        packet.InsertAtStart(reliableCounter);
        packet.InsertAtStart((byte)((int)type));

        int count = 0;
        while (count < reliableSendCount)
        {
            await SendData(type, packet, false);
            await Task.Delay((int)(reliableSendInterval * 1000));
            count++;
        }
    }

    /*private IEnumerator SendDataCoroutine(ServiceType type, MIKEPacket packet)
    {
        int count = 0;
        while (count < reliableSendCount)
        {
            SendData(type, packet, false);
            count++;
            yield return new WaitForSeconds(reliableSendInterval);
        }
    }*/

    /*public void SendData(ServiceType type, byte[] data)
    {
        int serviceID = (int)type;
        int packetSize = 65000;
        int byteTotal = data.Length;

        List<byte> dataAsList = data.ToList();

        while (byteTotal > 0)
        {
            int count = byteTotal >= packetSize ? packetSize : byteTotal;

            List<byte> dataToSend = new List<byte>() { (byte)serviceID };
            dataToSend.AddRange(dataAsList.GetRange(0, count));
            dataAsList.RemoveRange(0, count);

            socket.SendTo(dataToSend.ToArray(), endPoint);

            byteTotal -= count;
        }
    }

    public void SendDataReliably(ServiceType type, byte[] data)
    {
        reliableCounter = (reliableCounter + 1) % 255;
        List<byte> reliableDataToSend = new List<byte>() { (byte)reliableCounter };
        reliableDataToSend.AddRange(data);
        StartCoroutine(SendDataCoroutine(type, reliableDataToSend.ToArray()));
    }

    // Sends the packet multiple times to make sure it is received, I know this is stupid but I simply don't give a shit
    private IEnumerator SendDataCoroutine(ServiceType type, byte[] data)
    {
        int count = 0;

        while (count < reliableSendCount)
        {
            SendData(type, data);
            count++;
            yield return new WaitForSeconds(reliableSendInterval);
        }
    }*/
}