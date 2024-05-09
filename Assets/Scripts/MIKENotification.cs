using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class MIKENotification : LMCCFadeBehavior
{

    [SerializeField] private TextMeshProUGUI headerText, contentText;

    public void SetText(string header, string content, Color c, float time)
    {

        headerText.SetText(header);
        contentText.SetText(content);

        headerText.color = c;

        Display(true);
        StartCoroutine(DelayedDeactivate(time));

    }

    public IEnumerator DelayedDeactivate(float delay)
    {
        yield return new WaitForSeconds(delay);
        Display(false, () => Destroy(gameObject));
    }

}
