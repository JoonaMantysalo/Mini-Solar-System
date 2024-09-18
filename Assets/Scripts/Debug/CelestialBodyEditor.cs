using System.Collections.Generic;
using UnityEngine;

public class CelestialBodyEditor
{
    public Vector3 position;
    public Vector3 velocity;
    public float mass;
    public List<Vector3> trajectoryPoints;
    public List<Vector3> relativetrajectoryPoints;
    public CelestialBody body;

    public CelestialBodyEditor(CelestialBody body)
    {
        this.body = body;
        Rigidbody rb = body.rb;
        this.position = new Vector3(rb.position.x, -rb.position.y, rb.position.z);
        this.velocity = body.InitialVelocity;
        this.mass = rb.mass;
        trajectoryPoints = new List<Vector3>();
    }

    public void UpdateVelocity(Vector3 acceleration)
    {
        velocity += acceleration;
    }

    public void UpdatePosition(int trajectoryStepInterval)
    {
        position -= velocity * Time.fixedDeltaTime * trajectoryStepInterval;
        trajectoryPoints.Add(new Vector3(position.x, -position.y, position.z));
    }

    public Color GetColor()
    {
        return body.color;
    }
}
