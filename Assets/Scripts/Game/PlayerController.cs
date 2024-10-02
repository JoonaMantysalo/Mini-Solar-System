using System;
using UnityEngine;

public class PlayerController : MonoBehaviour, IControllable
{
    public Rigidbody rb { get; private set; }
    public PlayerState currentState { get; private set; }
    public GameObject lookingAtinterActableObject { get; private set; }
    public Camera playerCamera { get; private set; }

    [SerializeField] float mass;
    [SerializeField] Vector3 initialVelocity;
    [SerializeField] ShipController shipController;
    [SerializeField] ShipDetection shipDetection;
    [SerializeField] WalkOnDoorDetection walkOnDoorDetection;
    [SerializeField] SeatPlayer seatPlayer;
    [SerializeField] GameObject seat;
    [SerializeField] float walkSpeed;
    [SerializeField] float runningSpeed;
    [SerializeField] float jumpForce;
    [SerializeField] float jetPackForce;
    [SerializeField] float stickToGroundForce;

    float mouseSensitivity = 1.5f;
    float verticalRotation = 0.0f;
    float verticalRotationLimit = 90.0f;
    float standUpSpeed = 5f;
    float rayCastMaxDistance = 3f;
    float timeStep;
    bool isGrounded = false;

    Rigidbody shipRb;
    CelestialBody currentPlanet;

    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        rb = GetComponent<Rigidbody>();

        timeStep = GameObject.FindGameObjectWithTag("GravityManager").GetComponent<GravityManager>().timeStep;

        playerCamera = Camera.main;
        shipRb = GameObject.FindGameObjectWithTag("SpaceShip").GetComponent<Rigidbody>();

        rb = GetComponent<Rigidbody>();
        rb.mass = mass;
        rb.velocity = initialVelocity * timeStep;

        SetState();
    }

    public void UpdateForce(Vector3 force)
    {
        rb.velocity += force / mass;
    }

    public void SetVelocity(Vector3 velocity)
    {
        rb.velocity = velocity;
    }
    void Update()
    {
        SetState();

        lookingAtinterActableObject = LookingAtInteractable();
    }

    void SetState()
    {
        if (seatPlayer.isSeated) currentState = PlayerState.Seated;
        else if (shipDetection.IsPlayerInside() || walkOnDoorDetection.OnDoor()) currentState = PlayerState.OnShip;
        else if (isGrounded) currentState = PlayerState.OnGround;
        else currentState = PlayerState.Flying;
    }

    GameObject LookingAtInteractable()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, rayCastMaxDistance))
        {
            if (hit.collider.gameObject.CompareTag("Interactable"))
            {
                return hit.collider.gameObject;
            }
        }
        return null;
    }

    public void HandleInput(InputController input)
    {
        HandleCamera(input.GetMouseHorizontal(), input.GetMouseVertical());
        HandleMovement(input);

        // 
        if (input.InteractKey() && lookingAtinterActableObject != null)
        {
            IInteractable interactable;
            // Try to get the interactable component of the object or it's parent object
            if (!lookingAtinterActableObject.TryGetComponent<IInteractable>(out interactable))
            {
                interactable = lookingAtinterActableObject.GetComponentInParent<IInteractable>();
            }
            if (interactable != null)
            {
                interactable.Interact();
            }
        }
    }

    void HandleCamera(float mouseX, float mouseY)
    {
        mouseX *= mouseSensitivity;
        mouseY *= mouseSensitivity;

        transform.Rotate(0, mouseX, 0);

        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -verticalRotationLimit, verticalRotationLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
    }

    void HandleMovement(InputController input)
    {
        if (currentState == PlayerState.Flying)
        {
            Vector3 spaceMoveVelocity = transform.TransformDirection(input.GetSpaceMovement().normalized);
            SpaceMovement(spaceMoveVelocity * jetPackForce);
        }
        else
        {
            Vector3 moveVelocity = transform.TransformDirection(input.GetMovement().normalized);
            moveVelocity *= input.Running() ? runningSpeed : walkSpeed;
            if (currentState == PlayerState.OnGround)
            {
                if (input.Jumping())
                {
                    Jump(moveVelocity);
                    isGrounded = false;
                }
                else
                {
                    GroundMovement(moveVelocity);
                    SetPlayerStandingUpOnPlanet();
                }

            }
            else if (currentState == PlayerState.OnShip)
            {
                MovementOnShip(moveVelocity);
            }
        }
        
    }

    void GroundMovement(Vector3 moveVelocity)
    {
        rb.velocity = currentPlanet.rb.velocity + moveVelocity;

        // Add downwards force to prevent the player from flying after running too fast
        rb.AddForce(-transform.up * stickToGroundForce, ForceMode.Impulse);
    }

    void Jump(Vector3 moveVelocity)
    {
        rb.velocity = currentPlanet.rb.velocity + moveVelocity + transform.up * jumpForce;
    }

    void MovementOnShip(Vector3 moveVelocity)
    {
        rb.velocity = shipRb.velocity + moveVelocity;
    }

    void SpaceMovement(Vector3 moveVelocity)
    {
        rb.AddForce(moveVelocity, ForceMode.Force);
    }

    void SetPlayerStandingUpOnPlanet()
    {
        // Rotate the player to stand upwards on the surface of a planet
        Vector3 directionToPlanet = (transform.position - currentPlanet.transform.position).normalized;
        Quaternion targetRotation = Quaternion.FromToRotation(transform.up, directionToPlanet) * transform.rotation;

        rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, standUpSpeed * Time.deltaTime);

        Vector3 localCameraEuler = playerCamera.transform.localRotation.eulerAngles;
        playerCamera.transform.localRotation = Quaternion.Euler(localCameraEuler.x, playerCamera.transform.localRotation.eulerAngles.y, 0);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("CelestialBody"))
        {
            isGrounded = true;
            currentPlanet = collision.gameObject.GetComponent<CelestialBody>();
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("CelestialBody"))
        {
            isGrounded = false;
        }
    }
}

public enum PlayerState
{
    OnGround,
    OnShip,
    Flying,
    Seated
}
