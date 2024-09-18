using UnityEngine;

public interface IController
{
    void UpdateForce(Vector3 force);
    void SetVelocity(Vector3 velocity);
    void HandleMovement(Vector3 moveVelocity);
    void HandleCamera();

}
