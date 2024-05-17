using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LMCCErrorBlock : MonoBehaviour
{
    [SerializeField] private TMP_Text errorTitle;
    [SerializeField] private TMP_Text errorDescription;

    private ContentSizeFitter fitter;

    void Start()
    {
        fitter = GetComponentInParent<ContentSizeFitter>();
        fitter.enabled = false;
        fitter.enabled = true;
    }

    public void SetError(string title, string description)
    {
        errorTitle.text = title;
        errorDescription.text = description;
    }
}
