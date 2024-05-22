using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MIKESettingsManager : MonoBehaviour
{
    public static MIKESettingsManager Main { get; private set; }

    void Awake()
    {
        if (Main == null)
            Main = this;
        else
            Destroy(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        ReadFromJSON();
    }

    private void ReadFromJSON()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("Data/Settings");
        if (jsonFile != null)
        {
            SettingsObject settings = JsonUtility.FromJson<SettingsObject>(jsonFile.text);
            if (settings != null)
            {
                // Update HUD
                MIKEServerManager.Main.SetEndPoint(settings.Other_IP);
                // Update TSS
                TSSManager.Main.Connect(settings.TSS_IP);
                // Update Rover
                ((RoverScreen)LMCCMenuSpawner.Main.Menus[(int)ScreenType.Rover].CurrentScreen).RoverCamUrl = settings.Rover_IP + ":5000";
                // Update EVA
                TSSManager.Main.CurrentEVA = settings.EVA == "1" ? EVA.EVA1 : EVA.EVA2;
            }
        }
    }

    public void SaveToJSON(string other_ip, string tss_ip, string rover_ip, string eva)
    {
        MIKEServerManager.Main.SetEndPoint(other_ip);
        TSSManager.Main.Connect(tss_ip);
        ((RoverScreen)LMCCMenuSpawner.Main.Menus[(int)ScreenType.Rover].CurrentScreen).RoverCamUrl = rover_ip + ":5000";
        TSSManager.Main.CurrentEVA = eva == "1" ? EVA.EVA1 : EVA.EVA2;
        SettingsObject settings = new SettingsObject()
        {
            Other_IP = MIKEServerManager.Main.EndPoint.Address.ToString(),
            TSS_IP = TSSManager.Main.Host,
            Rover_IP = ((RoverScreen)LMCCMenuSpawner.Main.Menus[(int)ScreenType.Rover].CurrentScreen).RoverCamUrl.Replace(":5000", ""),
            EVA = TSSManager.Main.CurrentEVA == EVA.EVA1 ? "1" : "2"
        };

        string json = JsonUtility.ToJson(settings);
        File.WriteAllText(Application.dataPath + "/Resources/Data/Settings.json", json);
    }
}

public class SettingsObject
{
    public string Other_IP;
    public string TSS_IP;
    public string Rover_IP;
    public string EVA;
}
