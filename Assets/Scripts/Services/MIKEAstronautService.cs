using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MIKEAstronautService : MIKEService
{
    [SerializeField] private GameObject astronautPrefab;
    [SerializeField] private float interpolationSpeed = 5f;
    [SerializeField] private float walkingStartDistance = 0.15f;

    private LMCCAstronaut currAstronaut;
    private Vector3 currAstronautNewHeadPos, currAstronautNewLeftHandPos, currAstronautNewRightHandPos;
    private Quaternion currAstronautNewHeadRot, currAstronautNewLeftHandRot, currAstronautNewRightHandRot;


    // Start is called before the first frame update
    void Start()
    {
        Service = ServiceType.Astronaut;
        IsReliable = false;
        MIKEInputManager.Main.RegisterService(Service, this);
    }

    public override void ReceiveData(MIKEPacket packet)
    {
        Vector3 originalHeadPos = packet.ReadVector3();

        if (MIKEMap.Main.IsPositionOnMap(MIKEMap.Main.transform.TransformPoint(originalHeadPos), out RaycastHit hitInfo))
        {
            if (currAstronaut == null)
            {
                SpawnAstronaut();
            }

            Vector3 hitPointLocal = MIKEMap.Main.transform.InverseTransformPoint(hitInfo.point);

            currAstronautNewHeadPos = hitPointLocal;
            currAstronautNewHeadRot = packet.ReadQuaternion();

            currAstronautNewLeftHandPos = packet.ReadVector3() - originalHeadPos;
            currAstronautNewLeftHandRot = packet.ReadQuaternion();

            currAstronautNewRightHandPos = packet.ReadVector3() - originalHeadPos;
            currAstronautNewRightHandRot = packet.ReadQuaternion();
        }
        else
        {
            Debug.LogError("MIKEAstronautService: Astronaut position not on map");
            DestroyAstronaut();
        }
    }

    public void SpawnAstronaut()
    {
        currAstronaut = Instantiate(astronautPrefab, MIKEMap.Main.transform).GetComponentInChildren<LMCCAstronaut>();
    }

    public void DestroyAstronaut()
    {
        if (currAstronaut == null)
            return;
        Destroy(currAstronaut.gameObject);
        currAstronaut = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (currAstronaut != null)
        {
            currAstronaut.Head.localPosition = Vector3.Lerp(currAstronaut.Head.localPosition, currAstronautNewHeadPos, Time.deltaTime * interpolationSpeed);
            currAstronaut.Head.localRotation = Quaternion.Lerp(currAstronaut.Head.localRotation, currAstronautNewHeadRot, Time.deltaTime * interpolationSpeed);

            currAstronaut.LeftHand.localPosition = Vector3.Lerp(currAstronaut.LeftHand.localPosition, currAstronautNewLeftHandPos, Time.deltaTime * interpolationSpeed);
            currAstronaut.LeftHand.localRotation = Quaternion.Lerp(currAstronaut.LeftHand.localRotation, currAstronautNewLeftHandRot, Time.deltaTime * interpolationSpeed);

            currAstronaut.RightHand.localPosition = Vector3.Lerp(currAstronaut.RightHand.localPosition, currAstronautNewRightHandPos, Time.deltaTime * interpolationSpeed);
            currAstronaut.RightHand.localRotation = Quaternion.Lerp(currAstronaut.RightHand.localRotation, currAstronautNewRightHandRot, Time.deltaTime * interpolationSpeed);

            if (Vector3.Distance(currAstronaut.Head.localPosition, currAstronautNewHeadPos) > walkingStartDistance)
            {
                currAstronaut.Anim.SetBool("IsWalking", true);
            }
            else
            {
                currAstronaut.Anim.SetBool("IsWalking", false);
                currAstronaut.Head.localPosition = currAstronautNewHeadPos;
            }
        }
    }
}
