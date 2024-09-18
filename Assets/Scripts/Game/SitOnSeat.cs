//using UnityEngine;

//public class SitOnSeat : MonoBehaviour
//{
//    [SerializeField] Transform sittingPosition;
//    [SerializeField] Transform standUpPosition;
//    [SerializeField] GameObject player;

//    bool isSeated = false; 
//    Rigidbody playerRigidbody;
//    Collider playerCollider;
//    PlayerController playerController;
//    Camera playerCamera;
//    ShipController shipController;
//    Quaternion correctingRotation;

//    void Start()
//    {
//        playerRigidbody = player.GetComponent<Rigidbody>();
//        playerCollider = player.GetComponent<Collider>();
//        playerController = player.GetComponent<PlayerController>();
//        shipController = GetComponentInParent<ShipController>();
//        playerCamera = Camera.main;
//        correctingRotation = Quaternion.Euler(0, 90, 0);
//    }

//    void Update()
//    {
//        if (Input.GetMouseButtonDown(0))
//        {
//            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
//            RaycastHit hit;

//            if (Physics.Raycast(ray, out hit))
//            {
//                if (IsPartOfChair(hit.collider.gameObject) && !isSeated)
//                {
//                    SitPlayer();
//                }
//            }
//        }

//        if (isSeated && Input.GetKeyDown(KeyCode.F))
//        {
//            StandUp();
//        }
//    }

//    void SitPlayer()
//    {
//        playerRigidbody.isKinematic = true;

//        player.transform.position = sittingPosition.position;
//        player.transform.rotation = sittingPosition.rotation * correctingRotation;
//        player.transform.parent = sittingPosition;

//        playerCamera.transform.rotation = new Quaternion(0,0,0,0);

//        playerCollider.enabled = false;
//        isSeated = true;

//        playerManager.SwitchToController(shipController);
//    }

//    void StandUp()
//    {
//        player.transform.parent = null;
//        player.transform.position = standUpPosition.position;

//        isSeated = false;
//        playerRigidbody.isKinematic = false;
//        playerCollider.enabled = true;

//        playerManager.SwitchToController(playerController);
//    }

//    bool IsPartOfChair(GameObject clickedObject)
//    {
//        return clickedObject == gameObject || clickedObject.transform.IsChildOf(transform);
//    }

//    public bool IsSeated()
//    {
//        return isSeated;
//    }
//}
