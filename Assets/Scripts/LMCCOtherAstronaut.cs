using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LMCCOtherAstronaut : MonoBehaviour
{
    public Animator Anim { get => anim; }

    private Animator anim;

    // Start is called before the first frame update
    public void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }
}
