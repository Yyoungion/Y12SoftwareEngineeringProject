using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    public Animator animator;
    private Vector2 lastMoveDir = Vector2.down; 

    private void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        Vector2 moveDir = new Vector2(moveX, moveY).normalized;


        animator.SetFloat("Horizontal", moveX);
        animator.SetFloat("Vertical", moveY);
        animator.SetFloat("Speed", moveDir.sqrMagnitude);


        if (moveDir != Vector2.zero)
            lastMoveDir = moveDir;

        animator.SetFloat("LastHorizontal", lastMoveDir.x);
        animator.SetFloat("LastVertical", lastMoveDir.y);
    }
}
