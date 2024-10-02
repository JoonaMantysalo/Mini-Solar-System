using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DoorBehavior : MonoBehaviour
{
    [SerializeField] HingeJoint hingeJoint;
    [SerializeField] float doorMoveSpeed;
    JointMotor motor;
    bool open = false;
    bool hasHitGround = false;
    Quaternion originalRotation;

    void Start()
    {
        motor = hingeJoint.motor;
        motor.force = 100000f;
        originalRotation = transform.rotation;
        LockDoor();
    }

    void Update()
    {
        if (open && hasHitGround)
        {
            motor.targetVelocity = 0;
            hingeJoint.motor = motor;
        }
        if (!open && IsAtOriginalState())
        {
            LockDoor();
        }
    }

    public void MoveDoor()
    {
        if (open)
        {
            CloseDoor();
            open = false;
        }
        else
        {
            OpenDoor();
            open = true;
        }
    }

    void OpenDoor()
    {
        motor.targetVelocity = doorMoveSpeed;
        hingeJoint.motor = motor;
        hingeJoint.useMotor = true;
    }

    void CloseDoor()
    {
        hasHitGround = false;
        motor.targetVelocity = -doorMoveSpeed;
        hingeJoint.motor = motor;
        hingeJoint.useMotor = true;
    }

    void LockDoor()
    {
        motor.targetVelocity = 0; // Prevent any movement
        hingeJoint.motor = motor;
        hingeJoint.useMotor = true;
    }

    bool IsAtOriginalState()
    {
        return Quaternion.Angle(transform.rotation, originalRotation) < 1f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("CelestialBody"))
        {
            hasHitGround = true;
        }
    }

}
