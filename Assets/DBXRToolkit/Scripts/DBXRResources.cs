using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DBXRResources : MonoBehaviour
{

    public static DBXRResources Main { get; private set; }

    [Header("Layer Info")]
    [SerializeField] private LayerMask interactLayerMask;
    [SerializeField] private LayerMask staticLayerMask;
    [SerializeField] private int interactLayer;
    [SerializeField] private int uiLayer;

    [Header("Misc")]
    [SerializeField] private float throwSpeedMultiplier = 1.5f;
    [SerializeField] private float interactRadius = 0.1f;

    ////////////////////////////////////////////////////////

    // Layer Info
    public LayerMask InteractLayerMask { get { return interactLayerMask; } }
    public LayerMask StaticLayerMask { get { return staticLayerMask; } }
    public int InteractLayer { get { return interactLayer; } }
    public int UILayer { get { return uiLayer; } }

    // Misc
    public float ThrowSpeedMultiplier { get { return throwSpeedMultiplier; } }
    public float InteractRadius { get { return interactRadius; } }

    ////////////////////////////////////////////////////////

    void Awake()
    {
        if (Main == null)
            Main = this;
        else
            Debug.Log("More than one JoeXRResources in the scene!");
    }

    public Vector3 GetDirectionFromTransform(Transform t, InteractionDirection dir)
    {
        switch (dir)
        {

            case InteractionDirection.POSITIVE_X:
                return t.right;

            case InteractionDirection.POSITIVE_Y:
                return t.up;

            case InteractionDirection.POSITIVE_Z:
                return t.forward;

            case InteractionDirection.NEGATIVE_X:
                return -t.right;

            case InteractionDirection.NEGATIVE_Y:
                return -t.up;

            case InteractionDirection.NEGATIVE_Z:
                return -t.forward;

        }

        return Vector3.zero;
    }

}

public enum InteractionDirection
{
    POSITIVE_X, POSITIVE_Y, POSITIVE_Z, NEGATIVE_X, NEGATIVE_Y, NEGATIVE_Z
}