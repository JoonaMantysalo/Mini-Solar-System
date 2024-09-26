using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public PlayerController player;
    public IControllable activeController;

    InputController inputController = new InputController();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        activeController = player;

    }

    void Update()
    {
        if (activeController != null)
        {
            activeController.HandleInput(inputController);
        }
    }

    public void SwitchControl(IControllable newController)
    {
        activeController = newController;
    }
}
