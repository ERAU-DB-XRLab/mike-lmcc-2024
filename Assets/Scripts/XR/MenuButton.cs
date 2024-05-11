using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuButton : ButtonComponent
{
    public bool Pressed { get { return pressed; } }

    [SerializeField] private Image highlightImage;

    // Start is called before the first frame update
    void Start()
    {
        highlightImage.CrossFadeAlpha(0, 0, true);
    }

    void OnEnable()
    {
        highlightImage.gameObject.SetActive(false);
    }

    public override void PointerEnter(HandInteract interact)
    {
        if (onButtonCooldown)
        {
            return;
        }

        base.PointerEnter(interact);

        if (toggle)
        {
            if (pressed)
            {
                FadeIn();
            }
            else
            {
                FadeOut();
            }
        }
        else
        {
            FadeIn();
        }
    }

    public override void PointerExit(HandInteract interact)
    {
        base.PointerExit(interact);

        if (!toggle)
        {
            FadeOut();
        }
    }

    private void FadeIn()
    {
        highlightImage.gameObject.SetActive(true);
        highlightImage.CrossFadeAlpha(0, 0, true);
        highlightImage.CrossFadeAlpha(1f, 0.1f, true);
    }

    private void FadeOut()
    {
        highlightImage.CrossFadeAlpha(0, 0.1f, true);
    }
}
