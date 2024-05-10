using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverAbove : MonoBehaviour
{
    public bool IsBouncing { get => bounce; set => bounce = value; }

    [SerializeField] private float hoverHeight;
    [Header("Bounce Animation")]
    [SerializeField] private bool bounce;
    [SerializeField] private float bounceDist;
    [SerializeField] private float bounceFreq = 1;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = (transform.parent != null ? transform.parent.position : Vector3.zero) + (new Vector3(0, hoverHeight + (bounce ? bounceDist * Mathf.Sin(Time.realtimeSinceStartup * bounceFreq) : 0), 0));
    }
}
