using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Assemblies;
using UnityEngine.InputSystem;

public class PlayerAnimator : MonoBehaviour
{
    Animator animator;
    EntityStat stat;
    public float direction;
    void Start()
    {
        animator = GetComponent<Animator>();
        stat = GetComponent<EntityStat>();
    }
    void Update()
    {
        if (Keyboard.current.sKey.isPressed)
        {
            animator.SetBool("isShift", true);
        }
        else animator.SetBool("isShift", false);
    }

    public void SetMoving(bool val, Vector2 axis)
    {
        animator.SetBool("isMoving", val);
        Debug.Log(val);
        float moveRate = stat.GetResultValue("moveSpeed") / stat.GetBaseValue("moveSpeed");

        animator.SetFloat("moveSpeed", moveRate);

        if (val)
        {
            if (axis.x > 0)
                direction = 1;
            else if (axis.x < 0)
                direction = -1;

            transform.localScale = new Vector2(
                Mathf.Abs(transform.localScale.x) * direction,
                transform.localScale.y
            );
        }
    }

    public void Jump()
    {
        animator.SetTrigger("Jump");
    }

    public void Play(string id)
    {
        animator.Play(id);
    }

    public void Crouch(bool shift)
    {
        animator.SetBool("isShift", true); 
    }
}
