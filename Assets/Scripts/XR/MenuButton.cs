using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuButton : ButtonComponent
{
    [SerializeField] private Image highlightImage;

    private float highlightAlpha;

    // Start is called before the first frame update
    void Start()
    {
        highlightAlpha = highlightImage.color.a;
        highlightImage.CrossFadeAlpha(0, 0, true);
    }

    public override void PointerEntered(HandInteract interact)
    {
        base.PointerEntered(interact);
        highlightImage.CrossFadeAlpha(highlightAlpha, 0.1f, true);
    }

    public override void PointerExited(HandInteract interact)
    {
        base.PointerExited(interact);
        highlightImage.CrossFadeAlpha(0, 0.1f, true);
    }
}
