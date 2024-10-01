using UnityEngine;

public class SeatPlayer : MonoBehaviour
{
    [SerializeField] Transform sittingPosition;
    [SerializeField] Transform standUpPosition;
    [SerializeField] GameObject player;
    [SerializeField] ShipController shipController;
    public bool isSeated { get; private set; }

    Rigidbody playerRb;
    Collider playerCollider;
    PlayerController playerController;
    Camera playerCamera;

    Quaternion correctingRotation;


    void Start()
    {
        playerRb = player.GetComponent<Rigidbody>();
        playerCollider = player.GetComponent<Collider>();
        playerController = player.GetComponent<PlayerController>();
        playerCamera = playerController.playerCamera;

        correctingRotation = Quaternion.Euler(0, 90, 0);
    }

    public void SitPlayer()
    {
        playerRb.isKinematic = true;

        player.transform.position = sittingPosition.position;
        player.transform.rotation = sittingPosition.rotation * correctingRotation;
        player.transform.parent = sittingPosition;

        playerCamera.transform.rotation = new Quaternion(0, 0, 0, 0);

        playerCollider.enabled = false;
        isSeated = true;

        GameManager.Instance.SwitchControl(shipController);
    }

    public void StandUp()
    {
        player.transform.parent = null;
        player.transform.position = standUpPosition.position;

        isSeated = false;
        playerRb.isKinematic = false;
        playerCollider.enabled = true;

        GameManager.Instance.SwitchControl(playerController);
    }
}
