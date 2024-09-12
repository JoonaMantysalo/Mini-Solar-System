using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody rb;
    public float mass;
    public Vector3 initialVelocity;

    public bool isGrounded = false;
    public bool isSeated = false;
    public bool onShip = false;
    float mouseSensitivity = 2.0f;

    float verticalRotation = 0.0f;
    float verticalRotationLimit = 90.0f;
    Camera playerCamera;
    CelestialBody currentPlanet;
    float standUpSpeed = 5f;
    float walkSpeed = 20f;
    float runningSpeed = 40f;
    public float jumpForce = 10f;
    bool jumping = false;
    public float jetPackForce = 80f;
    bool jetpackUp = false;
    bool jetpackDown = false;
    [SerializeField] float rbVelocityMultiplier = 2.5f;
    [SerializeField] float gravityMultiplier = 7.5f;
    float groundMovementMultiplier = 0.03f;
    float stickToGroundForce = 20f;
    float timeStep;
    Vector3 moveVelocity = Vector3.zero;
    [SerializeField] GameObject spaceShip;
    [SerializeField] GameObject seat;
    ShipController shipController;
    ShipDetection shipDetection;
    SitOnSeat sitOnSeat;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        timeStep = GameObject.FindGameObjectWithTag("GravityManager").GetComponent<GravityManager>().timeStep;
        playerCamera = Camera.main;

        shipController = spaceShip.GetComponent<ShipController>();
        shipDetection = spaceShip.GetComponent<ShipDetection>();
        sitOnSeat = seat.GetComponent<SitOnSeat>();

        rb = GetComponent<Rigidbody>();
        rb.mass = mass;
        rb.velocity = initialVelocity * rbVelocityMultiplier;

    }

    public void UpdateForce(Vector3 force)
    {
        if (!sitOnSeat.IsSeated())
        {
            rb.AddForce(force * gravityMultiplier, ForceMode.Force);
        }
    }

    void Update()
    {
        MovementInputs();
        HandleCamera();
    }

    void MovementInputs()
    {
        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        if (Input.GetKey(KeyCode.LeftShift))
        {
            moveVelocity = transform.TransformDirection(input.normalized) * runningSpeed;
        } 
        else
        {
            moveVelocity = transform.TransformDirection(input.normalized) * walkSpeed;
        }

        if (isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.Space)) jumping = true;
        }
        else
        {
            jetpackUp = Input.GetKey(KeyCode.Space) ? true : false;
            jetpackDown = Input.GetKey(KeyCode.LeftControl) ? true : false;
        }
    }

    void HandleCamera()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        //if (isGrounded)
        //{
            transform.Rotate(0, mouseX, 0);

            verticalRotation -= mouseY;
            verticalRotation = Mathf.Clamp(verticalRotation, -verticalRotationLimit, verticalRotationLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
        //}
        //else
        //{
        //    Quaternion yawRotation = Quaternion.Euler(0, mouseX, 0);

        //    // Create the rotation for the pitch (vertical) movement based on mouse Y
        //    Quaternion pitchRotation = Quaternion.Euler(-mouseY, 0, 0);

        //    // Combine the rotations: apply yaw first, then pitch
        //    transform.localRotation = transform.localRotation * yawRotation;
        //    transform.localRotation = transform.localRotation * pitchRotation;

        //    // Reset the camera rotation to follow the body
        //    playerCamera.transform.localRotation = Quaternion.identity;
        //}
    }

    void FixedUpdate()
    {
        if (isGrounded)
        {
            GroundMovement();
            SetPlayerStandingUpOnPlanet();
        }
        else if(shipDetection.IsPlayerInside() && !sitOnSeat.IsSeated())
        {
            GroundMovement();
        } 
        else
        {
            SpaceMovement();
        }
    }

    void GroundMovement()
    {
        if (jumping)
        {
            rb.velocity += (currentPlanet.velocity + moveVelocity * groundMovementMultiplier) * rbVelocityMultiplier;
            rb.AddForce((transform.up + currentPlanet.velocity) * jumpForce, ForceMode.Impulse);

            jumping = false;
        }
        else
        {
            rb.MovePosition(rb.position + (currentPlanet.velocity + moveVelocity * groundMovementMultiplier) * timeStep);

            rb.AddForce(-transform.up * stickToGroundForce, ForceMode.Impulse);

        }

    }

    void SpaceMovement()
    {
        rb.AddForce(moveVelocity * rbVelocityMultiplier, ForceMode.Force);

        if (jetpackUp)
        {
            rb.AddForce(transform.up * jetPackForce * rbVelocityMultiplier, ForceMode.Force);
        }
        if (jetpackDown)
        {
            rb.AddForce(-transform.up * jetPackForce * rbVelocityMultiplier, ForceMode.Force);
        }
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("CelestialBody"))
        {
            isGrounded = true;
            currentPlanet = collision.gameObject.GetComponent<CelestialBody>();
            rb.velocity = Vector3.zero;
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
