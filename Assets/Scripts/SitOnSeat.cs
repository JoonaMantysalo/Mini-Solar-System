using UnityEngine;

public class SitOnSeat : MonoBehaviour
{
    [SerializeField] Transform sittingPosition;
    [SerializeField] GameObject player;

    bool isSeated = false; 
    Rigidbody playerRigidbody;
    Collider playerCollider;

    void Start()
    {
        playerRigidbody = player.GetComponent<Rigidbody>();
        playerCollider = player.GetComponent<Collider>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (IsPartOfChair(hit.collider.gameObject) && !isSeated)
                {
                    SitPlayer();
                }
            }
        }

        if (isSeated && Input.GetKeyDown(KeyCode.Space))
        {
            StandUp();
        }
    }

    private void SitPlayer()
    {
        playerRigidbody.isKinematic = true;

        player.transform.position = sittingPosition.position;
        player.transform.rotation = sittingPosition.rotation;
        player.transform.parent = sittingPosition;

        playerCollider.enabled = false;
        isSeated = true;
    }

    private void StandUp()
    {
        player.transform.parent = null;

        isSeated = false;
        playerRigidbody.isKinematic = false;
        playerCollider.enabled = true;
    }

    private bool IsPartOfChair(GameObject clickedObject)
    {
        return clickedObject == gameObject || clickedObject.transform.IsChildOf(transform);
    }

    public bool IsSeated()
    {
        return isSeated;
    }
}
