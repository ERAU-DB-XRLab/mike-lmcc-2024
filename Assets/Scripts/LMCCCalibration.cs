using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LMCCCalibration : MonoBehaviour
{
    [SerializeField] private Transform uiOrigin;
    [SerializeField] private Transform leftHand;
    [SerializeField] private Transform rightHand;
    [SerializeField] private float uiUpOffset = 0.22f;
    [SerializeField] private float uiForwardOffset = 0.1f;
    [SerializeField] private float minStopDistance = 0.1f;
    [SerializeField] private float minStopTime = 5f;

    private Vector3 leftHandPrevPos;
    private Vector3 rightHandPrevPos;
    private float timer = 0f;
    private bool calibrating = false;

    // Start is called before the first frame update
    void Start()
    {
        // For DEGUGGING
        //Calibrate();
    }

    public void Calibrate()
    {
        leftHandPrevPos = leftHand.position;
        rightHandPrevPos = rightHand.position;
        timer = 0f;
        calibrating = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (calibrating)
        {
            Vector3 lrSegment = rightHand.position - leftHand.position;
            Vector3 lrCenter = leftHand.position + (lrSegment / 2);
            uiOrigin.position = lrCenter + Vector3.up * uiUpOffset + Vector3.Cross(lrSegment, Vector3.up).normalized * uiForwardOffset;
            uiOrigin.rotation = Quaternion.LookRotation(Vector3.Cross(lrSegment, Vector3.up));

            if (Vector3.Distance(leftHandPrevPos, leftHand.position) < minStopDistance && Vector3.Distance(rightHandPrevPos, rightHand.position) < minStopDistance)
            {
                timer += Time.deltaTime;
            }
            else
            {
                timer = 0f;
            }

            if (timer >= minStopTime)
            {
                calibrating = false;
                Debug.Log("Calibration complete");
                MIKENotificationManager.Main.SendNotification("NOTIFICATION", "Calibration complete!", MIKEResources.Main.PositiveNotificationColor, 5f);
            }

            leftHandPrevPos = leftHand.position;
            rightHandPrevPos = rightHand.position;
        }
    }
}
