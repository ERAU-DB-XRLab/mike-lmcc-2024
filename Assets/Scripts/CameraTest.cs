using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Net;
using System.Net.Sockets;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Experimental.Rendering;

public class CameraTest : MonoBehaviour
{

    public static CameraTest Main;

    private Socket sock;
    private WebCamTexture tex;
    private IPEndPoint endPoint;

    // Start is called before the first frame update
    void Start()
    {

        Main = this;

        tex = new WebCamTexture();


        sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram,
        ProtocolType.Udp);

        // ML: 10.33.165.186
        // COMP: 10.33.229.99
        //IPAddress serverAddr = IPAddress.Parse("10.33.229.99");
        IPAddress serverAddr = IPAddress.Parse("10.33.139.153");
        endPoint = new IPEndPoint(serverAddr, 7777);

    }

    public void SendData(byte[] data)
    {

        int deviceID = 1;
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

    // Update is called once per frame
    void Update()
    {

        if (tex.isPlaying)
        {

            Texture2D t = new Texture2D(640, 480);
            Debug.Log(tex.width + " " + tex.height);
            t.SetPixels(tex.GetPixels());
            List<byte> data = new List<byte>();
            data.Add(2);

            data.AddRange(t.EncodeToJPG(15).ToList<byte>());

            int totalLength = data.Count;
            int packetSize = 65000;
            int packetCount = (int)Mathf.Ceil((float)totalLength / (float)packetSize);
            Debug.Log("total length: " + totalLength + " expected packets: " + packetCount);
            int sent = 0;
            while (true)
            {
                if (data.Count < packetSize)
                {
                    sock.SendTo(data.ToArray(), endPoint);
                    sent++;
                    break;
                }
                else
                {
                    List<byte> rangeToAdd = data.GetRange(0, packetSize);
                    sock.SendTo(rangeToAdd.ToArray(), endPoint);
                    sent++;
                    data.RemoveRange(0, packetSize);
                }
            }

            //sock.SendTo(new byte[] { 0 }, endPoint);

            Debug.Log("Sent: " + sent);

        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            RawImage img = GetComponent<RawImage>();
            img.texture = tex;
            tex.Play();
        }

    }
}
