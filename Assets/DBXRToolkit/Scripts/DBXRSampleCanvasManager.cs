using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DBXRSampleCanvasManager : MonoBehaviour
{

    [SerializeField] private LeverComponent leverFree, leverBound;
    [SerializeField] private ButtonComponent buttonPush, buttonToggle;
    [SerializeField] private FlightStickComponent flightStickFree, flightStickBound;
    [SerializeField] private PushLeverComponent pushLeverBlack, pushLeverBlue, pushLeverRed;
    [SerializeField] private YokeComponent yoke;

    [Space]
    [Space]
    [SerializeField] private TMP_Text leverFreeText, leverBoundText, buttonPushText, buttonToggleText, flightStickFreeText, flightStickBoundText,
                                     pushLeverBlackText, pushLeverBlueText, pushLeverRedText, yokeTranslationText, yokeRotationText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        leverFreeText.SetText("Value: " + leverFree.GetValue().ToString("0.0"));
        leverBoundText.SetText("Value: " + leverBound.GetValue().ToString("0.0"));
        buttonPushText.SetText("Value: " + buttonPush.IsPressed());
        buttonToggleText.SetText("Value: " + buttonToggle.IsPressed());
        flightStickFreeText.SetText("Value: " + flightStickFree.GetValue().x.ToString("0.0") + ", " + flightStickFree.GetValue().y.ToString("0.0"));
        flightStickBoundText.SetText("Value: " + flightStickBound.GetValue().x.ToString("0.0") + ", " + flightStickBound.GetValue().y.ToString("0.0"));
        pushLeverBlackText.SetText(pushLeverBlack.GetValue().ToString("0.0"));
        pushLeverBlueText.SetText(pushLeverBlue.GetValue().ToString("0.0"));
        pushLeverRedText.SetText(pushLeverRed.GetValue().ToString("0.0"));
        yokeTranslationText.SetText("Translation: " + yoke.GetValue().x.ToString("0.0"));
        yokeRotationText.SetText("Rotation: " + yoke.GetValue().y.ToString("0.0"));
    }
}
