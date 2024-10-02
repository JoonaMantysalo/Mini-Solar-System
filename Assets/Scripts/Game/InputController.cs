using UnityEngine;

public class InputController
{

    public float GetHorizontal()
    {
        return Input.GetAxis("Horizontal");
    }

    public float GetVertical()
    {
        return Input.GetAxis("Vertical");
    }

    public float GetMouseHorizontal()
    {
        return Input.GetAxis("Mouse X");
    }

    public float GetMouseVertical()
    {
        return Input.GetAxis("Mouse Y");
    }

    public Vector3 GetMovement()
    {
        return new Vector3(GetHorizontal(), 0, GetVertical());
    }

    public float GetVerticalMovement()
    {
        float input = 0;
        if (VerticalKey())          input += 1;
        if (NegativeVerticalKey())  input += -1;
        return input;
    }

    public Vector3 GetSpaceMovement()
    {
        Vector3 input = GetMovement();
        input.y = GetVerticalMovement();
        return input;
    }

    public Vector3 GetSpaceShipMovement()
    {
        return new Vector3(GetVertical(), GetVerticalMovement(), -GetHorizontal());
    }

    public bool Jumping()
    {
        return Input.GetKeyDown(KeyCode.Space);
    }

    public bool Running()
    {
        return Input.GetKey(KeyCode.LeftShift);
    }


    public bool VerticalKey()
    {
        return Input.GetKey(KeyCode.Space);
    }

    public bool NegativeVerticalKey()
    {
        return Input.GetKey(KeyCode.LeftControl);
    }

    public bool RollKey()
    {
        return Input.GetKey(KeyCode.Q);
    }

    public bool NegativeRollKey()
    {
        return Input.GetKey(KeyCode.E);
    }

    public bool InteractKey()
    {
        return Input.GetKeyDown(KeyCode.F) || Input.GetMouseButtonDown(0);
    }
}
