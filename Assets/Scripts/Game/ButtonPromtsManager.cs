using UnityEngine;

public class ButtonPromtsManager : MonoBehaviour
{
    [SerializeField] GameObject groundControlsUI;
    [SerializeField] GameObject flyingControlsUI;
    [SerializeField] GameObject shipControlsUI;
    [SerializeField] GameObject interactControlsUI;
    [SerializeField] PlayerController playerController;



    void Update()
    {
        SetCornerPromts();
        if (playerController.lookingAtinterActableObject != null)
        {
            interactControlsUI.SetActive(true);
        }
        else
        {
            interactControlsUI.SetActive(false);
        }
    }

    void SetCornerPromts()
    {
        if (playerController.currentState == PlayerState.Seated)
        {
            groundControlsUI.SetActive(false);
            flyingControlsUI.SetActive(false);
            shipControlsUI.SetActive(true);
        }
        else if (playerController.currentState == PlayerState.Flying)
        {
            groundControlsUI.SetActive(false);
            flyingControlsUI.SetActive(true);
            shipControlsUI.SetActive(false);
        }
        else
        {
            groundControlsUI.SetActive(true);
            flyingControlsUI.SetActive(false);
            shipControlsUI.SetActive(false);
        }
    }
}