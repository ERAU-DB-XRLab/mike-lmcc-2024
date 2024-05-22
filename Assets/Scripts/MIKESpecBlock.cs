using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MIKESpecBlock : MIKEExpandingBlock
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

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        Invoke("HideAfterDelay", 0.01f);
    }

    // Shits cringe i know but it works
    private void HideAfterDelay()
    {
        expandedFade.ReInitialize();
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
}
