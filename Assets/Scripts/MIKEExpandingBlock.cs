using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MIKEExpandingBlock : MonoBehaviour
{
    [SerializeField] protected Image expandedBackground;
    [SerializeField] protected Image buttonIcon;
    [SerializeField] protected Sprite upArrow;
    [SerializeField] protected Sprite downArrow;
    public LMCCFadeBehavior ExpandedFade { get; private set; }
    [SerializeField] protected LMCCFadeBehavior expandedFade;

    protected ContentSizeFitter fitter;

    protected float startingHeight = 100;
    protected float endingHeight = 250;

    protected bool expanded = false;

    protected virtual void Awake()
    {
        fitter = GetComponentInParent<ContentSizeFitter>();
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        expandedBackground.CrossFadeAlpha(0f, 0f, false);
    }

    public virtual void ToggleExpanded()
    {
        SetExpanded(!expanded);
    }

    public virtual void SetExpanded(bool value)
    {
        if (value)
        {
            StopCoroutine(Contract());
            StartCoroutine(Expand());
            buttonIcon.sprite = upArrow;
        }
        else
        {
            StopCoroutine(Expand());
            StartCoroutine(Contract());
            buttonIcon.sprite = downArrow;
        }
    }

    protected virtual IEnumerator Expand()
    {
        RectTransform t = (RectTransform)transform;
        float width = t.sizeDelta.x;
        float timeToExpand = 0.3f;
        int stepCount = 20;

        for (int i = 0; i < stepCount; i++)
        {
            yield return new WaitForSeconds(timeToExpand / stepCount);
            float newHeight = Mathf.Lerp(startingHeight, endingHeight, i / (float)stepCount);
            t.sizeDelta = new Vector2(width, newHeight);
            fitter.enabled = false;
            fitter.enabled = true;
        }

        expandedFade.Display(true);
        expandedBackground.CrossFadeAlpha(1f, 0.1f, false);
        expanded = true;
    }

    protected virtual IEnumerator Contract()
    {
        expandedBackground.CrossFadeAlpha(0f, 0.1f, false);
        expandedFade.Display(false, () => expanded = false);
        yield return new WaitUntil(() => !expanded);

        RectTransform t = (RectTransform)transform;
        float width = t.sizeDelta.x;
        float timeToExpand = 0.3f;
        int stepCount = 20;

        for (int i = 0; i < stepCount; i++)
        {
            yield return new WaitForSeconds(timeToExpand / stepCount);
            float newHeight = Mathf.Lerp(endingHeight, startingHeight, i / (float)stepCount);
            t.sizeDelta = new Vector2(width, newHeight);
            fitter.enabled = false;
            fitter.enabled = true;
        }
    }
}
