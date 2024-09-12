using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraManager : MonoBehaviour
{
    public float zoomSpeed;
    public float minZoom;
    public float maxZoom;
    public float dragSpeed;

    private Camera cam;
    private Vector3 dragOrigin;
    private Transform target;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        CameraDrag();
        FollowTarget();

        CameraZoom();
        CheckObjectClick();
    }

    private void CameraZoom()
    {
        float scrollData = Input.GetAxis("Mouse ScrollWheel");
        float distance = scrollData * zoomSpeed;

        cam.transform.Translate(0, 0, distance);

        float currentZoom = cam.transform.position.z;
        cam.transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y, Mathf.Clamp(currentZoom, minZoom, maxZoom));
    }

    private void CameraDrag()
    {
        if (Input.GetMouseButtonDown(0))
        {
            dragOrigin = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam.transform.position.z));
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 currentPos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam.transform.position.z));
            Vector3 difference = dragOrigin - currentPos;
            cam.transform.position += difference;
        }
    }

    private void CheckObjectClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                target = hit.transform;
            }
            else
            {
                ClearTarget();
            }
        }
    }

    private void FollowTarget()
    {
        if (target != null)
        {
            Vector3 targetPosition = target.position;
            targetPosition.z = cam.transform.position.z;

            cam.transform.position = targetPosition;
        }
    }

    public void ClearTarget()
    {
        target = null;
    }
}
