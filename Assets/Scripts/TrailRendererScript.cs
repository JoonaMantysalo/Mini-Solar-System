using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class TrailRendererScript : MonoBehaviour
{
    private Transform target;
    private float pointSpacing = 0.1f;

    private LineRenderer lineRenderer;
    private List<Vector3> points = new List<Vector3>();
    private Vector3 lastPoint;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        target = transform;

        lineRenderer.startColor = GetComponent<CelestialBody>().color;
        lineRenderer.endColor = GetComponent<CelestialBody>().color;

        points.Add(target.position);
        lastPoint = target.position;
        UpdateLineRenderer();
    }

    void Update()
    {
        if (target != null)
        {
            float distance = Vector3.Distance(target.position, lastPoint);
            if (distance >= pointSpacing)
            {
                points.Add(target.position);
                lastPoint = target.position;
                UpdateLineRenderer();
            }
        }
    }

    void UpdateLineRenderer()
    {
        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPositions(points.ToArray());
    }
}