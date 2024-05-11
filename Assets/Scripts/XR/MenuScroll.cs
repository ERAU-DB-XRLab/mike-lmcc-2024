using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class MenuScroll : InteractableComponent
{
    [SerializeField] private Scrollbar scrollBar;
    [SerializeField] private float scrollSpeed = 1;
    [SerializeField] private bool inverted = false;

    private RectTransform scrollTransform;
    private Transform handTransform;
    private Vector3 startHandPosLocal;
    private float startScrollPos;
    private bool scrolling = false;

    new void Awake()
    {
        base.Awake();
        scrollTransform = gameObject.GetComponent<RectTransform>();
    }

    public override void Grab(HandInteract interact)
    {
        // Prevent two hands from scrolling at the same time
        if (scrolling)
            return;

        scrolling = true;
        handTransform = interact.GetHandTransform();
        startHandPosLocal = scrollBar.transform.InverseTransformPoint(handTransform.position);
        startScrollPos = scrollBar.value;
    }

    public override void Drop(HandInteract interact)
    {
        if (interact.GetHandTransform() == handTransform)
            scrolling = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (scrolling)
        {
            Vector3 handPosLocal = scrollBar.transform.InverseTransformPoint(handTransform.position);
            float relativeDistance = (handPosLocal.y - startHandPosLocal.y) * (inverted ? -1 : 1) / scrollTransform.rect.height;
            scrollBar.value = Mathf.Clamp(startScrollPos + relativeDistance * scrollSpeed, 0f, 1f);
        }
    }
}
