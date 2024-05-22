using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LMCCHandSync : MonoBehaviour
{
    [SerializeField] private Transform leftHand;
    [SerializeField] private Transform rightHand;
    [SerializeField] private Animator leftHandAnimator;
    [SerializeField] private Animator rightHandAnimator;
    [SerializeField] private float sendInterval = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SendYourTransformData());
    }

    private IEnumerator SendYourTransformData()
    {
        while (true)
        {
            var packet = new MIKEPacket();
            packet.Write(MIKEMap.Main.transform.InverseTransformPoint(leftHand.position));
            packet.Write(leftHand.rotation);
            packet.Write(leftHandAnimator.GetBool("GripDown"));
            packet.Write(leftHandAnimator.GetBool("TriggerDown"));

            packet.Write(MIKEMap.Main.transform.InverseTransformPoint(rightHand.position));
            packet.Write(rightHand.rotation);
            packet.Write(rightHandAnimator.GetBool("GripDown"));
            packet.Write(rightHandAnimator.GetBool("TriggerDown"));

            MIKEServerManager.Main.SendData(ServiceType.Hand, packet, DeliveryType.Unreliable);
            yield return new WaitForSeconds(sendInterval);
        }
    }
}
