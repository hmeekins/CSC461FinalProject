using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Transform playerRoot;
    public Transform headTransform;
    public float moveSpeed = 2.5f;

    void Update()
    {
        Vector2 input = GameFlowController.Instance.LeftHanded
            ? OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick)
            : OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
        if (GameFlowController.Instance.State == GameState.PlayRunning) {
            Vector3 forward = headTransform.forward;
            Vector3 right = headTransform.right;

            forward.y = 0f;
            right.y = 0f;

            forward.Normalize();
            right.Normalize();

            Vector3 move = forward * input.y + right * input.x;

            playerRoot.position += move * moveSpeed * Time.deltaTime;
        }
    }
}