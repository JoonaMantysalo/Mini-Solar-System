using UnityEngine;

public class ShipController : MonoBehaviour, IControllable
{
    public Rigidbody rb;
    public float mass;

    [SerializeField] Vector3 initialVelocity;
    [SerializeField] float boosterStrength = 8000f;
    [SerializeField] float rotationForce = 2000f;
    [SerializeField] float rollForce = 100f;


    void Start()
    {
        rb.velocity = initialVelocity;
        rb.mass = mass;
    }


    public void UpdateForce(Vector3 force)
    {
        rb.velocity += force / mass;
    }

    public void SetVelocity(Vector3 velocity)
    {
        rb.velocity = velocity;
    }

    public void HandleInput(InputController input)
    {
        HandleRotation(input);
        Vector3 moveVelocity = transform.TransformDirection(input.GetSpaceShipMovement().normalized);
        HandleMovement(moveVelocity);
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
