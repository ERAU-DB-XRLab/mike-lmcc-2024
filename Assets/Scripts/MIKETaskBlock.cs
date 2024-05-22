using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MIKETaskBlock : MIKEExpandingBlock
{
    [SerializeField] private TMP_Text taskText;
    [SerializeField] private MenuButton expandButton;
    [SerializeField] private Transform hiddenParent;
    [Space]
    [SerializeField] private GameObject subTaskPrefab;
    [SerializeField] private Transform subTaskParent;

    private List<ProcedureStep> subTasks = new List<ProcedureStep>();
    private List<TMP_Text> subTaskTexts = new List<TMP_Text>();

    private Color initialColor;

    public int StepNumber { get; set; }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    public override void ToggleExpanded()
    {
        base.ToggleExpanded();
    }

    protected override IEnumerator Expand()
    {
        return base.Expand();
    }

    public void Initialize(ProcedureStep step)
    {
        startingHeight = 85;
        taskText.text = step.step_number + ": " + step.description;
        initialColor = taskText.color;
        endingHeight = startingHeight + (subTasks.Count * 40);
        hiddenParent.transform.localPosition = new Vector3(hiddenParent.transform.localPosition.x, hiddenParent.transform.localPosition.y + subTasks.Count, hiddenParent.transform.localPosition.z);
        //expandedBackground.rectTransform.sizeDelta = new Vector2(expandedBackground.rectTransform.sizeDelta.x, endingHeight * 0.82f);
        Invoke("HideAfterDelay", 0.01f);

        if (subTasks.Count > 0)
        {
            expandButton.gameObject.SetActive(true);
        }
        else
        {
            expandButton.gameObject.SetActive(false);
        }
    }

    // Shits cringe i know but it works
    private void HideAfterDelay()
    {
        expandedFade.ReInitialize();
    }

    public void AddTask(ProcedureStep step)
    {
        subTasks.Add(step);
        if (step.IsSubStep)
        {
            TMP_Text subTaskText = Instantiate(subTaskPrefab, subTaskParent).GetComponent<TMP_Text>();
            subTaskText.text = step.step_number + ": " + step.description;
            subTaskTexts.Add(subTaskText);
        }
        else if (step.IsSubSubStep)
        {
            TMP_Text subSubTaskText = Instantiate(subTaskPrefab, subTaskParent).GetComponent<TMP_Text>();
            subSubTaskText.text = "   " + step.step_number + ": " + step.description;
            subTaskTexts.Add(subSubTaskText);
        }
        else
        {
            Debug.LogError("MIKETaskBlock: Task is not a sub or sub sub task!");
        }
    }

    public void ChangeStepNumber(int stepNum)
    {
        int diff = stepNum - StepNumber;

        if (diff < 0)
        {
            taskText.color = initialColor;
        }
        else if (diff >= subTaskTexts.Count)
        {
            taskText.color = MIKEResources.Main.PositiveNotificationColor;
        }
        else
        {
            taskText.color = MIKEResources.Main.WarningNotificationColor;
        }

        for (int i = 0; i < subTaskTexts.Count; i++)
        {
            subTaskTexts[i].color = i < diff ? MIKEResources.Main.PositiveNotificationColor : initialColor;
        }
    }
}
