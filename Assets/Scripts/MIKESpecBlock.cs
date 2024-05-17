using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MIKESpecBlock : MonoBehaviour
{
    public SpecData SpecData { get; set; }

    [SerializeField] private TMP_Text specName;
    [SerializeField] private TMP_Text specID;
    [Space]
    [SerializeField] private MIKEWidgetValue sio2Value;
    [SerializeField] private MIKEWidgetValue al2o3Value;
    [SerializeField] private MIKEWidgetValue mnoValue;
    [SerializeField] private MIKEWidgetValue caoValue;
    [SerializeField] private MIKEWidgetValue p2o3Value;
    [SerializeField] private MIKEWidgetValue tio2Value;
    [SerializeField] private MIKEWidgetValue feoValue;
    [SerializeField] private MIKEWidgetValue mgoValue;
    [SerializeField] private MIKEWidgetValue k2oValue;
    [SerializeField] private MIKEWidgetValue otherValue;
    [Space]
    [SerializeField] private Image expandedBackground;
    [SerializeField] private Image buttonIcon;
    [SerializeField] private Sprite upArrow;
    [SerializeField] private Sprite downArrow;

    private LMCCFadeBehavior expandedFade;
    private ContentSizeFitter fitter;

    private float startingHeight = 100, endingHeight = 250;
    private bool expanded = false;

    void Awake()
    {
        fitter = GetComponentInParent<ContentSizeFitter>();
    }

    // Start is called before the first frame update
    void Start()
    {
        expandedFade = GetComponentInChildren<LMCCFadeBehavior>();
        expandedBackground.CrossFadeAlpha(0f, 0f, false);
    }

    public void DisplaySpecData()
    {
        string[] modifiedName = SpecData.name.Split('_');
        modifiedName[0] = modifiedName[0].ToUpper();
        modifiedName[1] = modifiedName[1].ToUpper();
        specName.text = modifiedName[0] + " " + modifiedName[1];
        specID.text = "ID: " + SpecData.id.ToString();

        sio2Value.SetValue((float)SpecData.data.SiO2, MIKEResources.Main.PositiveNotificationColor);
        al2o3Value.SetValue((float)SpecData.data.Al2O3, MIKEResources.Main.PositiveNotificationColor);
        mnoValue.SetValue((float)SpecData.data.MnO, MIKEResources.Main.PositiveNotificationColor);
        caoValue.SetValue((float)SpecData.data.CaO, MIKEResources.Main.PositiveNotificationColor);
        p2o3Value.SetValue((float)SpecData.data.P2O3, MIKEResources.Main.PositiveNotificationColor);
        tio2Value.SetValue((float)SpecData.data.TiO2, MIKEResources.Main.PositiveNotificationColor);
        feoValue.SetValue((float)SpecData.data.FeO, MIKEResources.Main.PositiveNotificationColor);
        mgoValue.SetValue((float)SpecData.data.MgO, MIKEResources.Main.PositiveNotificationColor);
        k2oValue.SetValue((float)SpecData.data.K2O, MIKEResources.Main.PositiveNotificationColor);
        otherValue.SetValue((float)SpecData.data.other, MIKEResources.Main.PositiveNotificationColor);
    }

    public void ToggleExpanded()
    {
        SetExpanded(!expanded);
    }

    public void SetExpanded(bool value)
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

    private IEnumerator Expand()
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

    private IEnumerator Contract()
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
