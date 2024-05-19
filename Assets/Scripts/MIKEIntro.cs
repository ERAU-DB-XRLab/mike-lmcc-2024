using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MIKEIntro : MonoBehaviour
{
    public bool Complete { get; private set; } = false;

    [SerializeField] private List<GameObject> mainObjects;
    private Dissipate[] dissipate;

    public void Play()
    {
        mainObjects.ForEach(o => o.SetActive(false));
        dissipate = GetComponentsInChildren<Dissipate>();
        foreach (Dissipate d in dissipate)
        {
            d.StartDissipate(true, 3f);
        }
        StartCoroutine(ShowApp());
    }

    public IEnumerator ShowApp()
    {
        yield return new WaitForSeconds(3.5f);
        mainObjects.ForEach(o => o.SetActive(true));
        Complete = true;
    }

}
