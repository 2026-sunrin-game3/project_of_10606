using UnityEngine;
using UnityEngine.InputSystem;

public class GoUnderPlatform : MonoBehaviour
{
    BoxCollider2D boxCollider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.sKey.isPressed && Keyboard.current.spaceKey.isPressed)
        {
            boxCollider.enabled = false;
        }
        else boxCollider.enabled = true;
    }
    
}
