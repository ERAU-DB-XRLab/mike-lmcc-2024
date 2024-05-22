using System;
using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;

public class ProcedureScreen : LMCCScreen
{
    [SerializeField] private GameObject taskTitle;
    [SerializeField] private GameObject taskPrefab;
    [SerializeField] private Transform taskParent;

    [SerializeField] private MenuButton nextButton;
    [SerializeField] private MenuButton previousButton;

    private List<MIKETaskBlock> taskBlocks = new List<MIKETaskBlock>();
    private List<GameObject> taskTitles = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        MIKEProcedureManager.Main.OnStepChanged += UpdateSteps;

        nextButton.ValueChanged.AddListener(NextStep);
        previousButton.ValueChanged.AddListener(PreviousStep);
    }

    void OnEnable()
    {
        DeleteAll();
        CreateTaskList();
        UpdateSteps(MIKEProcedureManager.Main.CurrentStepNum, MIKEProcedureManager.Main.CurrentStep);
    }

    public override void ScreenDeactivated()
    {
        DeleteAll();
    }

    private void UpdateSteps(int currentStepNum, ProcedureStep step)
    {
        foreach (MIKETaskBlock task in taskBlocks)
        {
            if (task != null)
            {
                task.ChangeStepNumber(currentStepNum);
            }
        }
    }

    private void CreateTaskList()
    {
        MIKETaskBlock currTask = null;
        ProcedureStep currStep = null;

        for (int i = 0; i < MIKEProcedureManager.Main.StepList.Count; i++)
        {
            ProcedureStep step = MIKEProcedureManager.Main.StepList[i];
            if (step.step_number == null)
            {
                Debug.Log("Bruh Title" + i);
                GameObject title = Instantiate(taskTitle, taskParent);
                title.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = step.description;
                taskBlocks.Add(null);
                taskTitles.Add(title);
                continue;
            }

            if (step.IsRootStep)
            {
                if (currTask != null)
                {
                    Debug.Log("Bruh Root" + i);
                    currTask.Initialize(currStep);
                }

                currTask = Instantiate(taskPrefab, taskParent).GetComponent<MIKETaskBlock>();
                currStep = step;
                taskBlocks.Add(currTask);
                currTask.StepNumber = i;
            }
            else
            {
                if (currTask == null)
                {
                    Debug.LogError("ProcedureScreen: First step is not a root step!");
                    return;
                }
                else
                {
                    currTask.AddTask(step);
                }
            }
        }

        if (currTask != null)
        {
            currTask.Initialize(currStep);
        }
    }

    // IDK why I have to do this, but it works. I think MenuButton calls this function twice and I don't remember why but don't care enough to fix it.
    public void NextStep(bool value)
    {
        if (value)
        {
            MIKEProcedureManager.Main.NextStep();
        }
    }

    // IDK why I have to do this, but it works. I think MenuButton calls this function twice and I don't remember why but don't care enough to fix it.
    public void PreviousStep(bool value)
    {
        if (value)
        {
            MIKEProcedureManager.Main.PreviousStep();
        }
    }

    private void DeleteAll()
    {
        foreach (MIKETaskBlock task in taskBlocks)
        {
            if (task != null)
            {
                Destroy(task.gameObject);
            }
        }
        taskBlocks.Clear();

        foreach (GameObject title in taskTitles)
        {
            Destroy(title);
        }
        taskTitles.Clear();
    }
}
