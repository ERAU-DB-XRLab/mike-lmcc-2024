using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MenuInput : InteractableComponent
{
    public delegate void InputEvent();
    public event InputEvent OnInputStart;

    public string InputText { get { return inputField.text; } set { inputField.text = value; } }
    [SerializeField] private TMP_InputField inputField;

    public override void Grab(HandInteract interact)
    {
        InputText = "";
        OnInputStart?.Invoke();
    }

    public override void Drop(HandInteract interact)
    {
        // Do nothing
    }

    public void AddNumber(string number)
    {
        inputField.text += number.ToString();
    }

    public void RemoveNumber()
    {
        if (inputField.text.Length > 0)
        {
            inputField.text = inputField.text.Substring(0, inputField.text.Length - 1);
        }
    }
}
