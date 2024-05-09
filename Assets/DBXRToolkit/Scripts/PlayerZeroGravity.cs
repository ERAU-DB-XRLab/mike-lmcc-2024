using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerZeroGravity : Player
{

    [SerializeField] private HandControllerInteractZeroGravity leftHand, rightHand;

    private Camera mainCamera;
    private Vector3 leftStart, rightStart;
    private Vector3 vectorStart, axis;

    // Start is called before the first frame update
    new void Awake()
    {
        base.Awake();
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {

        if(leftHand.Holding && rightHand.Holding)
        {
            
            if(!leftHand.Rotating)
            {
                leftHand.Rotating = true;
                rightHand.Rotating = true;
                leftStart = leftHand.transform.parent.localPosition;
                rightStart = rightHand.transform.parent.localPosition;
                axis = mainCamera.transform.forward;
                vectorStart = leftStart - rightStart;
            }

            Vector3 newVector = leftHand.transform.parent.localPosition - rightHand.transform.parent.localPosition;
            transform.RotateAround(mainCamera.transform.position, axis, Vector3.SignedAngle(newVector, vectorStart, axis));
            vectorStart = newVector;

        } else
        {
            if (leftHand.Rotating)
            {
                leftHand.Rotating = false;
                rightHand.Rotating = false;
                leftHand.UpdateGrabPoints();
                rightHand.UpdateGrabPoints();
            }
        }

    }
}
