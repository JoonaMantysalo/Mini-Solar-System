using System.Collections.Generic;
using UnityEngine;

public class GravityManager : MonoBehaviour
{
    public float timeStep;
    public float gravitationalConstant;
    List<CelestialBody> celestialBodies;
    PlayerController player;
    ShipController ship;

    private void Start()
    {
        celestialBodies = new List<CelestialBody>();
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("CelestialBody"))
        {
            celestialBodies.Add(obj.GetComponent<CelestialBody>());
        }
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        ship = GameObject.FindGameObjectWithTag("SpaceShip").GetComponent<ShipController>();
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < celestialBodies.Count; i++)
        {
            for (int j = i + 1; j < celestialBodies.Count; j++)
            {
                ApplyGravity(celestialBodies[i], celestialBodies[j]);
            }
        }

        foreach (CelestialBody cb in celestialBodies)
        {
            cb.UpdatePosition(timeStep);
            ApplyPlayerGravity(cb);
            ApplyShipGravity(cb);
        }
    }


    void ApplyGravity(CelestialBody body1, CelestialBody body2)
    {
        Rigidbody rb1 = body1.rb;
        Rigidbody rb2 = body2.rb;

        Vector3 direction = rb2.position - rb1.position;
        float distance = direction.magnitude;
        direction.Normalize();

        float forceMagnitude = GravitationalPull(rb1.mass, rb2.mass, distance);

        Vector3 force = direction * forceMagnitude;
        body1.UpdateForce(force * timeStep);
        body2.UpdateForce(-force * timeStep);
    }

    void ApplyPlayerGravity(CelestialBody cb)
    {
        Rigidbody rb1 = player.rb;
        Rigidbody rb2 = cb.rb;

        Vector3 direction = rb2.position - rb1.position;
        float distance = direction.magnitude;
        direction.Normalize();

        float forceMagnitude = GravitationalPull(rb1.mass, rb2.mass, distance);

        Vector3 force = direction * forceMagnitude;
        player.UpdateForce(force);
    }

    private void ApplyShipGravity(CelestialBody cb)
    {
        Rigidbody rb1 = ship.rb;
        Rigidbody rb2 = cb.rb;

        Vector3 direction = rb2.position - rb1.position;
        float distance = direction.magnitude;
        direction.Normalize();

        float forceMagnitude = GravitationalPull(rb1.mass, rb2.mass, distance);

        Vector3 force = direction * forceMagnitude;
        ship.UpdateForce(force);
    }

    public float GravitationalPull(float mass1, float mass2, float distance)
    {
        return gravitationalConstant * mass1 * mass2 / (distance * distance);
    }
}
