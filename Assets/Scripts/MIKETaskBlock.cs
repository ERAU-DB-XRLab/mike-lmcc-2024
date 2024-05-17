using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MIKETaskBlock : MIKEExpandingBlock
{
    [SerializeField] private MenuButton expandButton;
    [SerializeField] private Transform hiddenParent;
    [Space]
    [SerializeField] private GameObject subTaskPrefab;
    [SerializeField] private Transform subTaskParent;

    private List<ProcedureStep> tasks = new List<ProcedureStep>();

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        startingHeight = 85;
    }

    public override void ToggleExpanded()
    {
        base.ToggleExpanded();
    }

    protected override IEnumerator Expand()
    {
        return base.Expand();
    }

    public void Initialize()
    {
        endingHeight = startingHeight + (tasks.Count * 35);
        hiddenParent.transform.localPosition = new Vector3(hiddenParent.transform.localPosition.x, hiddenParent.transform.localPosition.y + tasks.Count, hiddenParent.transform.localPosition.z);
        expandedBackground.rectTransform.sizeDelta = new Vector2(expandedBackground.rectTransform.sizeDelta.x, endingHeight * 0.82f);
        expandedFade.ReInitialize();

        if (tasks.Count > 0)
        {
            expandButton.gameObject.SetActive(true);
        }
        else
        {
            expandButton.gameObject.SetActive(false);
        }
    }

    public void AddTask(ProcedureStep step)
    {
        tasks.Add(step);
        if (step.IsSubStep)
        {
            TMP_Text subTaskText = Instantiate(subTaskPrefab, subTaskParent).GetComponent<TMP_Text>();
            subTaskText.text = step.step_number + ": " + step.description;
        }
        else if (step.IsSubSubStep)
        {
            TMP_Text subSubTaskText = Instantiate(subTaskPrefab, subTaskParent).GetComponent<TMP_Text>();
            subSubTaskText.text = "   " + step.step_number + ": " + step.description;
        }
        else
        {
            Debug.LogError("MIKETaskBlock: Task is not a sub or sub sub task!");
        }
    }
}
