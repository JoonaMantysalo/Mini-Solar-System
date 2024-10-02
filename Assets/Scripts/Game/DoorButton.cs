using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorButton : MonoBehaviour, IInteractable
{
    [SerializeField] DoorBehavior door;

    public void Interact()
    {
        door.MoveDoor();
    }
}
