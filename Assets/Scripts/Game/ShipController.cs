using UnityEngine;

public class ShipController : MonoBehaviour, IControllable
{
    public Rigidbody rb;
    public float mass;

    [SerializeField] Vector3 initialVelocity;
    [SerializeField] float boosterStrength;
    [SerializeField] float rotationForce;
    [SerializeField] float rollForce;
    [SerializeField] PlayerController playerController;
    [SerializeField] SeatPlayer seatPlayer;


    void Start()
    {
        rb.velocity = initialVelocity;
        rb.mass = mass;
    }


    public void UpdateForce(Vector3 force)
    {
        rb.velocity += force / mass;
    }

    public void HandleInput(InputController input)
    {
        HandleRotation(input);
        Vector3 moveVelocity = transform.TransformDirection(input.GetSpaceShipMovement().normalized);
        HandleMovement(moveVelocity);

        if (input.InteractKey() && playerController.currentState == PlayerState.Seated)
        {
            seatPlayer.StandUp();
        }
    }

    void HandleMovement(Vector3 moveVelocity)
    {
        if (moveVelocity.magnitude > 0)
        {
            rb.AddForce(moveVelocity * boosterStrength, ForceMode.Force);
        }
    }

    void HandleRotation(InputController input)
    {
        float mouseX = input.GetMouseHorizontal();
        float mouseY = input.GetMouseVertical();

        // Yaw
        rb.AddTorque(transform.up * mouseX * rotationForce);

        // Pitch
        rb.AddTorque(transform.forward * mouseY * rotationForce);

        // Roll
        if (input.RollKey())
        {
            rb.AddTorque(transform.right * rollForce);
        }
        if (input.NegativeRollKey())
        {
            rb.AddTorque(-transform.right * rollForce);
        }

    }
}
