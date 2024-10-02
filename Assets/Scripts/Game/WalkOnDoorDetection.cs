using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkOnDoorDetection : MonoBehaviour
{
    bool playerOnDoor = false;

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerOnDoor = true;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerOnDoor = false;
        }
    }

    public bool OnDoor()
    {
        return playerOnDoor;
    }
}
