using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    public Rigidbody rb;
    public float mass;
    public Vector3 initialVelocity;

    public float gravityMultiplier = 7.5f;
    public float boosterStrength = 200f;
    public float rbVelocityMultiplier = 2.5f;

    float mouseSensitivity = 2.0f;
    float verticalRotation = 0.0f;
    float verticalRotationLimit = 90.0f;
    Camera playerCamera;

    Vector3 moveVelocity = Vector3.zero;
    bool boosterUp = false;
    bool boosterDown = false;

    void Start()
    {
        rb.velocity = initialVelocity * rbVelocityMultiplier;
        rb.mass = mass;
    }

    public void UpdateForce(Vector3 force)
    {
        rb.AddForce(force * gravityMultiplier, ForceMode.Force);

    }

    void Update()
    {
        MovementInputs();
    }

    void MovementInputs()
    {
        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        moveVelocity = transform.TransformDirection(input.normalized) * boosterStrength;


        //boosterUp = Input.GetKey(KeyCode.Space) ? true : false;
        //boosterDown = Input.GetKey(KeyCode.LeftControl) ? true : false;

        if (Input.GetKey(KeyCode.Space))
        {
            moveVelocity += transform.up * boosterStrength;
        }
        if (Input.GetKey(KeyCode.LeftControl))
        {
            moveVelocity += -transform.up * boosterStrength;
        }
    }

    private void FixedUpdate()
    {
        rb.AddForce(moveVelocity * rbVelocityMultiplier, ForceMode.Force);
    }
}
