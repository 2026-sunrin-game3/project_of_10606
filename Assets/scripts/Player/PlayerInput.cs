using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerInput : MonoBehaviour
{
    private PlayerMovement movement;

    public Vector2 axis;

    void Awake()
    {
        movement = GetComponent<PlayerMovement>();
    }
    public void OnMove(InputValue value)
    {
        Vector2 axis_ = value.Get<Vector2>();

        axis = new Vector2(axis_.x, 0);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public bool HasAxis()
    {
        return axis.x != 0 || axis.y != 0;
    }
    public void OnJump()
    {
        movement.Jump();
    }
}
