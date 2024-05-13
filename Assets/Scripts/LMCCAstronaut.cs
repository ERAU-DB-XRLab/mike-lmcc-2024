using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LMCCAstronaut : MonoBehaviour
{
    public Transform Head { get => head; }
    public Transform HeadHeight { get => headHeight; }
    public Transform LeftHand { get => leftHand; }
    public Transform RightHand { get => rightHand; }
    public Animator Anim { get => anim; }

    [SerializeField] private Transform head, headHeight, leftHand, rightHand;
    [SerializeField] private Animator anim;

    void OnAnimatorIK()
    {
        anim.SetIKPosition(AvatarIKGoal.LeftHand, leftHand.position);
        anim.SetIKPosition(AvatarIKGoal.RightHand, rightHand.position);
        anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1f);
        anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 1f);
    }
}
