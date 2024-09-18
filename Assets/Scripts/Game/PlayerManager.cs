//using UnityEngine;

//public class PlayerManager : MonoBehaviour
//{
//    IController activeController;
//    public PlayerState currentState { get; private set; }
//    public CelestialBody currentPlanet { get; private set; }

//    [SerializeField] GameObject spaceShip;
//    [SerializeField] GameObject seat;
//    SitOnSeat sitOnSeat;
//    ShipDetection shipDetection;
//    bool isGrounded;
//    public bool running { get; private set; }
//    public bool jumping;
//    Vector3 moveVelocity;

//    void Start()
//    {
//        activeController = GetComponent<PlayerController>();
//        currentState = PlayerState.Flying;
//        sitOnSeat = seat.GetComponent<SitOnSeat>();
//        shipDetection = spaceShip.GetComponent<ShipDetection>();
//        isGrounded = false;
//        moveVelocity = Vector3.zero;
//    }

//    void Update()
//    {
//        SetState();
//        HandleInput();

//        activeController.HandleCamera();
//    }

//    void FixedUpdate()
//    {
//        activeController.HandleMovement(moveVelocity);
//    }

//    void SetState()
//    {
//        if (sitOnSeat.IsSeated()) currentState = PlayerState.Seated;
//        else if (shipDetection.IsPlayerInside()) currentState = PlayerState.OnShip;
//        else if (isGrounded ) currentState = PlayerState.OnGround;
//        else currentState = PlayerState.Flying;
//    }

//    void HandleInput()
//    {
//        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

//        if (Input.GetKey(KeyCode.Space))
//        {
//            input.y += 1;
//            jumping = true;
//        }
//        else jumping = false;
//        if (Input.GetKey(KeyCode.LeftControl))
//        {
//            input.y += -1;
//        }
//        moveVelocity = transform.TransformDirection(input.normalized);

//        if (Input.GetKey(KeyCode.LeftShift)) running = true;
//        else running = false;
//    }

//    public void SwitchToController(IController newController)
//    {
//        activeController = newController;
//    }

    
//}

//public enum PlayerState
//{
//    OnGround,
//    OnShip,
//    Flying,
//    Seated
//}