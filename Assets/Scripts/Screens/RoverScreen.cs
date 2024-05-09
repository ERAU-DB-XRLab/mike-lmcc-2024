using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class RoverScreen : LMCCScreen
{
    [SerializeField] private string roverCamUrl;
    [SerializeField] private RawImage roverCamImage;
    [SerializeField] private float framesPerSecond = 30f;
    [Space]
    [SerializeField] private MIKEWidgetValue roverX;
    [SerializeField] private MIKEWidgetValue roverY;
    [SerializeField] private MIKEWidgetValue qrID;

    private Texture2D roverCamTexture;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetCameraFeed());
        TSSManager.Main.OnRoverUpdated += UpdateRover;
    }

    private void UpdateRover(RoverData data)
    {
        roverX.SetValue((float)data.posx, MIKEResources.Main.PositiveNotificationColor);
        roverY.SetValue((float)data.posy, MIKEResources.Main.PositiveNotificationColor);
        qrID.SetValue(data.qr_id, MIKEResources.Main.PositiveNotificationColor);
    }

    private IEnumerator GetCameraFeed()
    {
        while (true)
        {
            using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(roverCamUrl))
            {
                yield return www.SendWebRequest();

                if (www.result == UnityWebRequest.Result.Success)
                {
                    roverCamTexture = DownloadHandlerTexture.GetContent(www);
                    roverCamImage.texture = roverCamTexture;
                }
                else
                {
                    Debug.Log(www.error);
                }
            }

            yield return new WaitForSeconds(1f / framesPerSecond);
        }
    }
}
