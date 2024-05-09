using UnityEngine;
using UnityEngine.InputSystem;

public class HandControllerInteractZeroGravity : HandControllerInteract
{

    [Header("Zero Gravity")]
    [SerializeField] private bool enableJetpackInput;
    [SerializeField] private InputActionProperty jetpack;
    [SerializeField] private float jetpackSpeed;
    [SerializeField] private Velocity playerVel;

    private readonly float accel = 2f;
    private Rigidbody rb;
    private Camera mainCamera;

    private Vector3 holdPoint;
    private Vector3 playerPoint;

    public bool Holding { get; private set; }
    public bool Rotating { get; set; }

    new void Awake()
    {
        base.Awake();
        mainCamera = Camera.main;
        rb = GetComponentInParent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

        if (grab.action.WasPressedThisFrame())
        {
            
            if(CheckStatic())
            {
                UpdateGrabPoints();
                Holding = true;
            }
            
            InteractableComponent ic = GetNearestInteractable();
            if (ic != null)
            {
                currentInteractable = ic;
                anim.SetFistOverride(true);
                currentInteractable.Grabbed.Invoke(this);
            }
        }

        if (grab.action.WasReleasedThisFrame())
        {
            if (currentInteractable)
            {
                currentInteractable.Dropped.Invoke(this);
                currentInteractable = null;
                anim.SetFistOverride(false);
            }

            if(Holding)
            {
                Holding = false;
                rb.velocity = playerVel.GetVelocity();
            }
        }

        if (interact.action.WasPressedThisFrame())
        {
            if (currentInteractable)
            {
                currentInteractable.InteractStarted.Invoke(this);
            }
        }

        if (interact.action.WasReleasedThisFrame())
        {
            if (currentInteractable)
            {
                currentInteractable.InteractStopped.Invoke(this);
            }
        }

        if (handOverride)
        {
            ClampToOverride();
        }

        if(enableJetpackInput)
        {

            Vector2 jetpackInput = jetpack.action.ReadValue<Vector2>();
            rb.velocity = Vector3.MoveTowards(rb.velocity, ((mainCamera.transform.forward * jetpackInput.y) + (mainCamera.transform.right * jetpackInput.x)) * jetpackSpeed, accel * Time.deltaTime);

        }

        if(Holding && !Rotating)
        {
            // Translate
            Vector3 delta = transform.parent.localPosition - holdPoint;
            rb.transform.position = playerPoint - (rb.transform.forward * delta.z) - (rb.transform.right * delta.x) - (rb.transform.up * delta.y);
        }

    }

    public bool CheckStatic()
    {
        return Physics.CheckSphere(transform.position, 0.1f, DBXRResources.Main.StaticLayerMask);
    }

    public void UpdateGrabPoints()
    {
        holdPoint = transform.parent.localPosition;
        playerPoint = rb.transform.position;
    }

}
