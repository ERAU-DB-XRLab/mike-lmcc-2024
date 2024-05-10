using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Linq;
using System.Collections;
using System.Collections.Concurrent;

public class MIKEServerManager : MonoBehaviour
{

    public static MIKEServerManager Main { get; private set; }

    // Sending
    private Socket sock;
    private IPEndPoint endPoint;

    // Receiving
    private UdpClient udpClient;
    private ConcurrentQueue<byte[]> dataToReceive = new ConcurrentQueue<byte[]>();
    private bool tasksRunning = true;

    private const int reliableSendCount = 1;
    private const float reliableSendInterval = 0.1f;
    private int reliableCounter = 0;

    public string OtherIP { get { return otherIP; } set { otherIP = value; } }
    [SerializeField] private string otherIP;
    [SerializeField] private int sendPort = 7777;
    [SerializeField] private int receivePort = 7777;

    // Start TCP server
    void Awake()
    {
        if (Main == null)
            Main = this;
        else
            Destroy(this);

        // Sending
        sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

        IPAddress otherAddress = IPAddress.Parse(otherIP);
        endPoint = new IPEndPoint(otherAddress, sendPort);
        udpClient = new UdpClient(receivePort);
        ReceiveData();

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

    private async void ReceiveData()
    {
        while (tasksRunning)
        {
            //Debug.Log("Waiting for data...");
            var receivedResults = await udpClient.ReceiveAsync();
            dataToReceive.Enqueue(receivedResults.Buffer);
            //await Task.Delay(100);
            //Debug.Log("Length: " + receivedResults.Buffer.Length);
        }
    }

    void OnDisable()
    {
        tasksRunning = false;
        Debug.Log("Server stopped.");
    }

    void Update()
    {
        Debug.Log("Data Count: " + dataToReceive.Count);
        if (dataToReceive.Count > 0)
        {
            MIKEInputManager.Main.ReceiveInput(dataToReceive.TryDequeue(out byte[] data) ? data : null);

            if (dataToReceive.Count > 40)
            {
                dataToReceive.Clear();
            }
        }
    }

    public void SendData(ServiceType type, MIKEPacket packet, bool insertServiceType = true)
    {
        if (insertServiceType)
            packet.InsertAtStart((byte)((int)type));
        sock.SendTo(packet.ByteArray, endPoint);

        //while (packet.UnreadLength > 0)
        //{
        //    int count = packet.UnreadLength >= maxPacketSize ? maxPacketSize : packet.UnreadLength;
        //    sock.SendTo(packet.ReadBytes(count), endPoint);
        //}
    }

    public void SendDataReliably(ServiceType type, MIKEPacket packet)
    {
        reliableCounter++;
        packet.InsertAtStart(reliableCounter);
        packet.InsertAtStart((byte)((int)type));
        StartCoroutine(SendDataCoroutine(type, packet));
    }

    // Sends the packet multiple times to make sure it is received, I know this is stupid but I simply don't give a shit
    private IEnumerator SendDataCoroutine(ServiceType type, MIKEPacket packet)
    {
        int count = 0;
        while (count < reliableSendCount)
        {
            SendData(type, packet, false);
            count++;
            yield return new WaitForSeconds(reliableSendInterval);
        }
    }

    /*public void SendData(ServiceType type, byte[] data)
    {
        int serviceID = (int)type;
        int byteTotal = data.Length;

        List<byte> dataAsList = data.ToList();

        while (byteTotal > 0)
        {
            int count = byteTotal >= maxPacketSize ? maxPacketSize : byteTotal;

            List<byte> dataToSend = new List<byte>() { (byte)serviceID };
            dataToSend.AddRange(dataAsList.GetRange(0, count));
            dataAsList.RemoveRange(0, count);

            sock.SendTo(dataToSend.ToArray(), endPoint);

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