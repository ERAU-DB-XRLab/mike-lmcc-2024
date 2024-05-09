using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dissipate : MonoBehaviour
{

    [SerializeField] private float timeToDissipate;
    private Renderer[] renderers;


    // Start is called before the first frame update
    protected void Awake()
    {
        renderers = GetComponentsInChildren<Renderer>();
        StartDissipate(false);
    }

    public virtual void StartDissipate(bool visible)
    {
        foreach(Renderer r in renderers)
        {
            if(r.material.HasFloat("_DissipationAmount"))
            {
                StartCoroutine(Dissipation(visible, r.material));
            }
        }
    }

    protected IEnumerator Dissipation(bool b, Material m)
    {

        int stepCount = 50;

        for(int i = 0; i < stepCount; i++)
        {
            m.SetFloat("_DissipationAmount", Mathf.Lerp(b ? 0f : 1f, b ? 1f : 0f, i / ((float)stepCount - 1)));
            yield return new WaitForSeconds(timeToDissipate / stepCount);
        }

    }

}
