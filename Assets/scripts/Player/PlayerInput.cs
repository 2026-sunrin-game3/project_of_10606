using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    
    PlayerMovement movement;
    PlayerAnimator animator;
    Animator anime;
    BoxCollider2D boxCollider;
    PlayerBattle battle;
    AudioSource audioSource;
    public Vector2 axis;
    [SerializeField] AudioClip attackSfx;
    [SerializeField] AudioClip dashSfx;
    void Awake()
    {
        movement = GetComponent<PlayerMovement>();
        battle = GetComponent<PlayerBattle>();
        animator = GetComponent<PlayerAnimator>();
        audioSource = GetComponent<AudioSource>();
        boxCollider = GetComponent<BoxCollider2D>();
    }
    public void OnMove(InputValue value)
    {
        Vector2 axis_ = value.Get<Vector2>();
        axis = new Vector2(axis_.x, 0);
    }
    public bool HasAxis()
    {
        return axis.x != 0 || axis.y != 0;
    }
    public void OnJump()
    {
        if (movement.Jump())
            animator.Jump();
    }
    public void OnAttack()
    {
        
        battle.Attack();
        audioSource.PlayOneShot(attackSfx);
        animator.Play("entrybot_attack");
    }

    public void OnDash()
    {
        audioSource.PlayOneShot(dashSfx);
        battle.Dash((int)animator.direction);
    }

    public void OnSkill_1()
    {
        battle.Skill_1();
    }

    public void OnCrouch()
    {
        //anime.SetBool("isShift",true);
        //Debug.Log("앙기모");
        animator.Crouch(true);
    
    }
}