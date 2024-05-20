using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum MenuInputField
{
    HUD = 0,
    TSS = 1,
    ROVER = 2
}

public class SettingsScreen : LMCCScreen
{
    [SerializeField] private List<MenuInput> inputFields;
    [SerializeField] private List<MenuButton> numpadButtons;
    [SerializeField] private MenuButton submitButton;
    [SerializeField] private MenuButton evaButton;

    public MenuInput CurrentInput { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        foreach (MenuInput input in inputFields)
        {
            input.OnInputStart += () => CurrentInput = input;
        }

        numpadButtons[0].ValueChanged.AddListener((b) => { if (b) AddNumber("1"); });
        numpadButtons[1].ValueChanged.AddListener((b) => { if (b) AddNumber("2"); });
        numpadButtons[2].ValueChanged.AddListener((b) => { if (b) AddNumber("3"); });
        numpadButtons[3].ValueChanged.AddListener((b) => { if (b) AddNumber("4"); });
        numpadButtons[4].ValueChanged.AddListener((b) => { if (b) AddNumber("5"); });
        numpadButtons[5].ValueChanged.AddListener((b) => { if (b) AddNumber("6"); });
        numpadButtons[6].ValueChanged.AddListener((b) => { if (b) AddNumber("7"); });
        numpadButtons[7].ValueChanged.AddListener((b) => { if (b) AddNumber("8"); });
        numpadButtons[8].ValueChanged.AddListener((b) => { if (b) AddNumber("9"); });
        numpadButtons[9].ValueChanged.AddListener((b) => { if (b) RemoveNumber(); });
        numpadButtons[10].ValueChanged.AddListener((b) => { if (b) AddNumber("0"); });
        numpadButtons[11].ValueChanged.AddListener((b) => { if (b) AddNumber("."); });

        submitButton.ValueChanged.AddListener((b) => { if (b) UpdateAll(); });

        evaButton.ValueChanged.AddListener((b) => { if (b) SwitchEva(TSSManager.Main.CurrentEVA); });
    }

    void OnEnable()
    {
        CurrentInput = null;
        inputFields[(int)MenuInputField.HUD].InputText = MIKEServerManager.Main.OtherIP;
        inputFields[(int)MenuInputField.TSS].InputText = TSSManager.Main.Host;
        inputFields[(int)MenuInputField.ROVER].InputText = ((RoverScreen)LMCCMenuSpawner.Main.Menus[(int)ScreenType.Rover].CurrentScreen).RoverCamUrl.Replace(":5000", "");
    }

    public void SwitchEva(EVA eva)
    {
        if (eva == EVA.EVA1)
        {
            TSSManager.Main.CurrentEVA = EVA.EVA2;
            evaButton.GetComponentInChildren<TMP_Text>().text = "2";
        }
        else
        {
            TSSManager.Main.CurrentEVA = EVA.EVA1;
            evaButton.GetComponentInChildren<TMP_Text>().text = "1";
        }
    }

    public void AddNumber(string number)
    {
        if (CurrentInput != null)
        {
            CurrentInput.AddNumber(number);
        }
    }

    public void RemoveNumber()
    {
        if (CurrentInput != null)
        {
            CurrentInput.RemoveNumber();
        }
    }

    public void UpdateAll()
    {
        // Update HUD
        MIKEServerManager.Main.SetEndPoint(inputFields[(int)MenuInputField.HUD].InputText);
        // Update TSS
        TSSManager.Main.Connect(inputFields[(int)MenuInputField.TSS].InputText);
        // Update Rover
        ((RoverScreen)LMCCMenuSpawner.Main.Menus[(int)ScreenType.Rover].CurrentScreen).RoverCamUrl = inputFields[(int)MenuInputField.ROVER].InputText + ":5000";
    }
}
