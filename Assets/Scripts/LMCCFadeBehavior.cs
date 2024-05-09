using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LMCCFadeBehavior : MonoBehaviour
{
    [SerializeField] private bool startVisible = false;

    private List<MaskableGraphic> graphics;

    protected virtual void Awake()
    {
        graphics = new List<MaskableGraphic>();
        foreach (MaskableGraphic graphic in GetComponentsInChildren<MaskableGraphic>(false))
        {
            graphics.Add(graphic);

            if (!startVisible)
                graphic.CrossFadeAlpha(0f, 0f, true);
        }

    }

    public void Display(bool display, UnityAction callback = null)
    {
        if (display)
            StartCoroutine(Activate(callback));
        else
            StartCoroutine(Deactivate(callback));
    }

    private IEnumerator Fade(bool fadeIn)
    {
        foreach (MaskableGraphic graphic in graphics)
        {
            graphic.CrossFadeAlpha(fadeIn ? 1f : 0f, MIKEResources.Main.WidgetFadeTime, false);
        }

        yield return new WaitForSeconds(MIKEResources.Main.WidgetFadeTime);
    }

    private IEnumerator Activate(UnityAction callback = null)
    {
        yield return Fade(true);
        callback?.Invoke();
    }

    private IEnumerator Deactivate(UnityAction callback = null)
    {
        yield return Fade(false);
        callback?.Invoke();
    }
}
