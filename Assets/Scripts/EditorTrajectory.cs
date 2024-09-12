using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Experimental.AI;

[ExecuteInEditMode]
public class EditorTrajectory : MonoBehaviour
{
    public CelestialBody relativeBody;
    public bool orbitRelativeToBody;
    public bool drawTrajectories;

    CelestialBodyEditor relativeBodyEditor;
    List<CelestialBodyEditor> celestialBodies;
    GravityManager manager;

    void Update()
    {
        if (manager == null)
        {
            manager = FindObjectOfType<GravityManager>();
        }
        if (!Application.isPlaying && manager != null && drawTrajectories)
        {
            UpdateTrajectory();
        }
    }

    private void UpdateTrajectory()
    {
        celestialBodies = new List<CelestialBodyEditor>();
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("CelestialBody"))
        {
            CelestialBody cb = obj.GetComponent<CelestialBody>();
            CelestialBodyEditor cbe = new CelestialBodyEditor(cb);
            celestialBodies.Add(cbe);
            if (cb == relativeBody) relativeBodyEditor = cbe;

        }
        if (relativeBody == null) relativeBodyEditor = null;

        int trajectorySteps = manager.trajectorySteps;

        for (int n = 0; n < trajectorySteps; n++)
        {
            for (int i = 0; i < celestialBodies.Count; i++)
            {
                for (int j = i + 1; j < celestialBodies.Count; j++)
                {
                    ApplyGravity(celestialBodies[i], celestialBodies[j], manager);
                }
            }
            foreach (CelestialBodyEditor cb in celestialBodies)
            {
                cb.UpdatePosition();
            }
        }
    }

    void OnRenderObject()
    {
        if (celestialBodies == null || celestialBodies.Count == 0 || !drawTrajectories)
            return;

        foreach (CelestialBodyEditor cb in celestialBodies)
        {
            if (cb.trajectoryPoints == null || cb.trajectoryPoints.Count < 2)
            {
                continue;
            }

            Handles.color = cb.GetColor();

            for (int i = 0; i < cb.trajectoryPoints.Count - 1; i++)
            {
                Vector3 drawPoint1 = cb.trajectoryPoints[i];
                Vector3 drawPoint2 = cb.trajectoryPoints[i + 1];
                if (orbitRelativeToBody && relativeBodyEditor != null)
                {
                    drawPoint1 -= relativeBodyEditor.trajectoryPoints[i];
                    drawPoint2 -= relativeBodyEditor.trajectoryPoints[i + 1];

                    // Draw the lines based on the current editor position of the relative object
                    drawPoint1 += relativeBody.transform.position;
                    drawPoint2 += relativeBody.transform.position;
                }
                Handles.DrawLine(drawPoint1, drawPoint2);
            }
        }
    }

    void ApplyGravity(CelestialBodyEditor body1, CelestialBodyEditor body2, GravityManager manager)
    {
        Vector3 direction = body1.position - body2.position;
        float distance = direction.magnitude;
        direction.Normalize();

        float forceMagnitude = manager.GravitationalPull(body1.mass, body2.mass, distance);

        Vector3 force = direction * forceMagnitude;
        Vector3 accelerationBody1 = force / body1.mass;
        Vector3 accelerationBody2 = -force / body2.mass;

        body1.UpdateVelocity(accelerationBody1);
        body2.UpdateVelocity(accelerationBody2);
    }
}
