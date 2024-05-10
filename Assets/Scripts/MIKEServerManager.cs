using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Linq;
using System.Collections;

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

        // Receiving
        Task.Run(async () =>
        {
            using (var udpClient = new UdpClient(receivePort))
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
                dataToReceive.Clear(); // POTENTIAL ISSUE/RACE CONDITION HERE, IF DATA IS ENQUEUED IN THE TASK ABOVE AND THEN CLEARED HERE DATA WILL BE LOST. NEEDS TO BE TESTED
            }
        }
    }

    public void SendData(ServiceType type, byte[] data)
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
            yield return new WaitForSeconds(0.1f);
        }
    }
}