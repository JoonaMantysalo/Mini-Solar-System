using UnityEngine;

public class CelestialBody : MonoBehaviour
{
    public float mass;
    public Rigidbody rb;
    public Vector3 InitialVelocity;
    public Color color;
    new Renderer renderer;
    public Vector3 velocity { get; private set; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        velocity = InitialVelocity;
        rb.mass = mass;

        //SetMaterialAndColor();
    }

    //void Update()
    //{
    //    if (!Application.isPlaying)
    //    {
    //        SetMaterialAndColor();
    //    }
    //}

    //void SetMaterialAndColor()
    //{
    //    if (renderer == null)
    //    {
    //        renderer = GetComponent<Renderer>();
    //        renderer.sharedMaterial = new Material(renderer.sharedMaterial);
    //    }

    //    renderer.sharedMaterial.color = color;
    //}

    public void UpdateForce(Vector3 force)
    {
        UpdateVelocity(force / mass);
    }

    public void UpdatePosition(float timeStep)
    {
        rb.MovePosition(rb.position + velocity * timeStep);
    }

    private void UpdateVelocity(Vector3 acceleration)
    {
        velocity += acceleration;
    }
}
