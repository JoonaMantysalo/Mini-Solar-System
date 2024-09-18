using UnityEngine;

[ExecuteInEditMode]
public class CelestialBody : MonoBehaviour
{
    [SerializeField] float mass;
    public Vector3 InitialVelocity;
    public Rigidbody rb { get; private set; }

    public Vector3 velocity { get; private set; }
    public Color color;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        velocity = InitialVelocity;
        rb.mass = mass;
    }

    public void UpdateForce(Vector3 force)
    {
        UpdateVelocity(force / mass);
    }

    public void UpdatePosition(float timeStep)
    {
        rb.velocity = velocity * timeStep;
    }

    private void UpdateVelocity(Vector3 acceleration)
    {
        velocity += acceleration;
    }
}
