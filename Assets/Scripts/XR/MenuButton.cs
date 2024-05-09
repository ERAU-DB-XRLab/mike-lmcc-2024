using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuButton : ButtonComponent
{
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
        base.PointerEnter(interact);
        highlightImage.gameObject.SetActive(true);
        highlightImage.CrossFadeAlpha(0, 0, true);
        highlightImage.CrossFadeAlpha(1f, 1f, true);
    }

    public override void PointerExit(HandInteract interact)
    {
        base.PointerExit(interact);
        highlightImage.CrossFadeAlpha(0, 0.1f, true);
    }
}
