using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Linq;
using System.Collections;

public enum ServiceType
{
    Message = 1,
}

public class MIKEServerManager : MonoBehaviour
{

    public static MIKEServerManager Main { get; private set; }

    // Sending
    private Socket sock;
    private IPEndPoint endPoint;


    // Receiving
    private Queue<byte[]> dataToReceive = new Queue<byte[]>();
    private bool tasksRunning = true;

    private const int reliableSendCount = 30;

    [SerializeField] private string otherIP;
    public string OtherIP { get { return otherIP; } set { otherIP = value; } }

    // Start TCP server
    void Awake()
    {

        Main = this;

        // Sending
        sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram,
        ProtocolType.Udp);

        IPAddress otherAddress = IPAddress.Parse(otherIP);
        endPoint = new IPEndPoint(otherAddress, 7777);

        // Receiving

        Task.Run(async () =>
        {
            using (var udpClient = new UdpClient(7777))
            {
                while (tasksRunning)
                {
                    //IPEndPoint object will allow us to read datagrams sent from any source.
                    var receivedResults = await udpClient.ReceiveAsync();
                    dataToReceive.Enqueue(receivedResults.Buffer);
                    Debug.Log(receivedResults.Buffer.Length);
                }
            }
        });

    }

    void OnDisable()
    {
        tasksRunning = false;
        Debug.Log("Server stopped.");
    }

    void Update()
    {

        if (dataToReceive.Count > 0)
        {
            MIKEInputManager.Main.ReceiveInput(dataToReceive.Dequeue());

            if (dataToReceive.Count > 40)
            {
                dataToReceive.Clear();
            }

        }

    }

    public void SendData(ServiceType type, byte[] data)
    {
        int deviceID = (int)type;
        int packetSize = 65000;
        int byteTotal = data.Length;

        List<byte> dataAsList = data.ToList();

        while (byteTotal > 0)
        {

            int count = byteTotal >= packetSize ? packetSize : byteTotal;

            List<byte> dataToSend = new List<byte>() { (byte)deviceID };
            dataToSend.AddRange(dataAsList.GetRange(0, count));
            dataAsList.RemoveRange(0, count);

            sock.SendTo(dataToSend.ToArray(), endPoint);
            Debug.Log(dataToSend.Count);

            byteTotal -= count;
        }
    }

    public void SendDataReliably(ServiceType type, byte[] data)
    {
        StartCoroutine(SendDataCoroutine(type, data));
    }

    private IEnumerator SendDataCoroutine(ServiceType type, byte[] data)
    {
        int count = 0;

        while (count < reliableSendCount)
        {
            SendData(type, data);
            count++;
            yield return new WaitForSeconds(0.1f);
        }
    }

}