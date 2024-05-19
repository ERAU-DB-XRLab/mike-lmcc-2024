using System;
using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;

public class ProcedureScreen : LMCCScreen
{
    [SerializeField] private GameObject taskPrefab;
    [SerializeField] private Transform taskParent;

    [SerializeField] private MenuButton nextButton;
    [SerializeField] private MenuButton previousButton;

    private List<MIKETaskBlock> taskBlocks = new List<MIKETaskBlock>();

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
        Debug.Log("ProcedureScreen: Updating steps");
        foreach (MIKETaskBlock task in taskBlocks)
        {
            task.ChangeStepNumber(currentStepNum);
        }
    }

    private void CreateTaskList()
    {
        MIKETaskBlock currTask = null;
        ProcedureStep currStep = null;

        for (int i = 0; i < MIKEProcedureManager.Main.StepList.Count; i++)
        {
            ProcedureStep step = MIKEProcedureManager.Main.StepList[i];
            if (step.IsRootStep)
            {
                if (currTask != null)
                {
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
            Destroy(task.gameObject);
        }
        taskBlocks.Clear();
    }
}
