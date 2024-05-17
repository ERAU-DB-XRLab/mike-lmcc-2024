using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public enum UIAType
{
    EMU1_POWER,
    EV1_WATER_SUPPLY,
    EV1_WATER_WASTE,
    EV1_OXYGEN,
    EMU2_POWER,
    EV2_WATER_SUPPLY,
    EV2_WATER_WASTE,
    EV2_OXYGEN,
    O2_VENT,
    DEPRESS_PUMP
}

public class MIKEProcedureManager : MonoBehaviour
{
    public delegate void ProcedureStepChanged(ProcedureStep step);
    public event ProcedureStepChanged OnStepChanged;

    public static MIKEProcedureManager Main { get; private set; }

    public Dictionary<string, ProcedureStep> Steps { get; private set; }
    public List<ProcedureStep> StepList { get; private set; }
    public ProcedureStep CurrentStep { get { return StepList[CurrentStepNum]; } }
    public int CurrentStepNum { get; private set; }
    public Dictionary<UIAType, UIAStatus> UIAStatuses { get; private set; }

    private JsonSerializerSettings settings = new JsonSerializerSettings
    {
        MissingMemberHandling = MissingMemberHandling.Ignore,
    };

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
        Steps = new Dictionary<string, ProcedureStep>();
        StepList = new List<ProcedureStep>();

        UIAStatuses = new Dictionary<UIAType, UIAStatus>()
        {
            { UIAType.EMU1_POWER, new UIAStatus(UIAType.EMU1_POWER, "ON", "OFF") },
            { UIAType.EV1_WATER_SUPPLY, new UIAStatus(UIAType.EV1_WATER_SUPPLY, "OPEN", "CLOSED") },
            { UIAType.EV1_WATER_WASTE, new UIAStatus(UIAType.EV1_WATER_WASTE, "OPEN", "CLOSED") },
            { UIAType.EV1_OXYGEN, new UIAStatus(UIAType.EV1_OXYGEN, "OPEN", "CLOSED") },
            { UIAType.EMU2_POWER, new UIAStatus(UIAType.EMU2_POWER, "ON", "OFF") },
            { UIAType.EV2_WATER_SUPPLY, new UIAStatus(UIAType.EV2_WATER_SUPPLY, "OPEN", "CLOSED") },
            { UIAType.EV2_WATER_WASTE, new UIAStatus(UIAType.EV2_WATER_WASTE, "OPEN", "CLOSED") },
            { UIAType.EV2_OXYGEN, new UIAStatus(UIAType.EV2_OXYGEN, "OPEN", "CLOSED") },
            { UIAType.O2_VENT, new UIAStatus(UIAType.O2_VENT, "OPEN", "CLOSED") },
            { UIAType.DEPRESS_PUMP, new UIAStatus(UIAType.DEPRESS_PUMP, "ON", "OFF") }
        };

        LoadProcedures();
        TSSManager.Main.OnUIAUpdated += UpdateUIA;
    }

    private void UpdateUIA(UIAData data)
    {
        UIAStatuses[UIAType.EMU1_POWER].Active = data.eva1_power;
        UIAStatuses[UIAType.EV1_WATER_SUPPLY].Active = data.eva1_water_supply;
        UIAStatuses[UIAType.EV1_WATER_WASTE].Active = data.eva1_water_waste;
        UIAStatuses[UIAType.EV1_OXYGEN].Active = data.eva1_oxy;
        UIAStatuses[UIAType.EMU2_POWER].Active = data.eva2_power;
        UIAStatuses[UIAType.EV2_WATER_SUPPLY].Active = data.eva2_water_supply;
        UIAStatuses[UIAType.EV2_WATER_WASTE].Active = data.eva2_water_waste;
        UIAStatuses[UIAType.EV2_OXYGEN].Active = data.eva2_oxy;
        UIAStatuses[UIAType.O2_VENT].Active = data.oxy_vent;
        UIAStatuses[UIAType.DEPRESS_PUMP].Active = data.depress;

        CheckForCompletedSteps();
    }

    private void CheckForCompletedSteps()
    {
        int currentAutoCompleteStepNum = CurrentStepNum;

        while (true)
        {
            if (currentAutoCompleteStepNum >= StepList.Count)
            {
                break;
            }

            if (StepList[currentAutoCompleteStepNum].autocomplete == null)
            {
                currentAutoCompleteStepNum++;
            }
            else
            {
                ProcedureStep step = StepList[currentAutoCompleteStepNum];
                UIAType type = (UIAType)Enum.Parse(typeof(UIAType), step.autocomplete.Split(',')[0].ToUpper());
                string status = step.autocomplete.Split(',')[1].ToUpper();
                if (UIAStatuses[type].GetActiveStatus() == status)
                {
                    currentAutoCompleteStepNum++;
                    CurrentStepNum = currentAutoCompleteStepNum;
                    OnStepChanged?.Invoke(CurrentStep);
                }

                break;
            }
        }
    }

    public void NextStep()
    {
        CurrentStepNum++;
        OnStepChanged?.Invoke(CurrentStep);
    }

    public void PreviousStep()
    {
        CurrentStepNum--;
        OnStepChanged?.Invoke(CurrentStep);
    }

    private void LoadProcedures()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("Data/Procedure");

        if (jsonFile != null)
        {
            ProcedureObject rootObject = JsonConvert.DeserializeObject<ProcedureObject>(jsonFile.text, settings);

            if (rootObject != null)
            {
                foreach (ProcedureStep step in rootObject.steps)
                {
                    Steps.Add(step.step_number, step);
                    StepList.Add(step);
                    if (step.sub_steps != null)
                    {
                        foreach (ProcedureStep subStep in step.sub_steps)
                        {
                            Steps.Add(subStep.step_number, subStep);
                            StepList.Add(subStep);
                            if (subStep.sub_steps != null)
                            {
                                foreach (ProcedureStep subSubStep in subStep.sub_steps)
                                {
                                    Steps.Add(subSubStep.step_number, subSubStep);
                                    StepList.Add(subSubStep);
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                Debug.LogError("MIKEProcedureManager: Failed to deserialize JSON into RootObject");
            }
        }
        else
        {
            Debug.LogError("MIKEProcedureManager: Failed to load JSON file");
        }
    }
}

public class UIAStatus
{
    public UIAType Type { get; private set; }
    public bool Active { get; set; }
    private string trueStatus;
    private string falseStatus;

    public UIAStatus(UIAType type, string trueStatus, string falseStatus)
    {
        Type = type;
        this.trueStatus = trueStatus;
        this.falseStatus = falseStatus;
    }

    public string GetActiveStatus()
    {
        return Active ? trueStatus : falseStatus;
    }

    public string GetInactiveStatus()
    {
        return Active ? falseStatus : trueStatus;
    }
}

[Serializable]
public class ProcedureObject
{
    public List<ProcedureStep> steps;
}

[Serializable]
public class ProcedureStep
{
    public string step_number;
    public string description;
    public List<ProcedureStep> sub_steps;
    public string autocomplete;

    [JsonIgnore]
    public bool HasSubSteps { get { return sub_steps != null; } }
    [JsonIgnore]
    public bool IsRootStep { get { return step_number.Split(".").Length == 1; } }
    [JsonIgnore]
    public bool IsSubStep { get { return step_number.Split(".").Length == 2; } }
    [JsonIgnore]
    public bool IsSubSubStep { get { return step_number.Split(".").Length == 3; } }
    [JsonIgnore]
    public bool Completed { get; set; }
}
