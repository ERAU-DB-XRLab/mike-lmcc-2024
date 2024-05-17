using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcedureScreen : MonoBehaviour
{
    [SerializeField] private GameObject taskPrefab;
    [SerializeField] private Transform taskParent;

    private List<MIKETaskBlock> taskBlocks;

    // Start is called before the first frame update
    void Start()
    {
        taskBlocks = new List<MIKETaskBlock>();
        CreateTaskList();
        MIKEProcedureManager.Main.OnStepChanged += UpdateSteps;
    }

    private void UpdateSteps(ProcedureStep step)
    {
        // TODO
    }

    private void CreateTaskList()
    {
        MIKETaskBlock currTask = null;
        Debug.Log("ProcedureScreen: Creating task list, " + MIKEProcedureManager.Main.StepList.Count + " steps.");

        foreach (ProcedureStep step in MIKEProcedureManager.Main.StepList)
        {
            if (step.IsRootStep)
            {
                if (currTask != null)
                {
                    currTask.Initialize();
                }

                currTask = Instantiate(taskPrefab, taskParent).GetComponent<MIKETaskBlock>();
                taskBlocks.Add(currTask);
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
            currTask.Initialize();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
