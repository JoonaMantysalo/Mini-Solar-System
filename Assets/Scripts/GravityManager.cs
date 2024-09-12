using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GravityManager : MonoBehaviour
{
    public float timeStep;
    public int trajectorySteps;
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
            ApplyPlayerGravity(cb);
            ApplyShipGravity(cb);
            cb.UpdatePosition(timeStep);
        }
    }


    void ApplyGravity(CelestialBody body1, CelestialBody body2)
    {
        Rigidbody rb1 = body1.rb;
        Rigidbody rb2 = body2.rb;

        Vector3 direction = rb2.position - rb1.position;
        float distance = direction.magnitude;
        direction.Normalize();

        float forceMagnitude = GravitationalPull(body1.mass, body2.mass, distance);

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

        float forceMagnitude = GravitationalPull(player.mass, cb.mass, distance);

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

        float forceMagnitude = GravitationalPull(ship.mass, cb.mass, distance);

        Vector3 force = direction * forceMagnitude;
        ship.UpdateForce(force);
    }

    public float GravitationalPull(float mass1, float mass2, float distance)
    {
        return gravitationalConstant * mass1 * mass2 / (distance * distance);
    }
}
