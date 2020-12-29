using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    private float WalkSpeed = 5f;
    public bool EnableInput = false;
    // private float lastMoveTime;
    // private float lerpDuration = .4f;
    Vector2Int moveVec = Vector2Int.zero;
    Rigidbody2D rb;
    Animator animator;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (EnableInput == true)
        {
            float inputX = Input.GetAxisRaw("Horizontal");
            float inputY = Input.GetAxisRaw("Vertical");
            Vector2 inputRawVec = new Vector2(inputX, inputY);
            if (inputX != moveVec.x || inputY != moveVec.y)
            {
                Debug.Log($"InputX: {inputX}");
                Debug.Log($"InputY: {inputY}");
            }
            moveVec = Vector2Int.zero;
            // Check x-axis input
            if (inputX > 0)
            {
                moveVec += Vector2Int.right;
            }
            else if (inputX < 0)
            {
                moveVec += Vector2Int.left;
            }

            // Check y-axis input
            if (inputY > 0)
            {
                moveVec += Vector2Int.up;
            }
            else if (inputY < 0)
            {
                moveVec += Vector2Int.down;
            }

            if (moveVec != Vector2Int.zero)
            {
                animator.SetFloat("Horizontal", inputX);
                animator.SetFloat("Vertical", inputY);
            }
            animator.SetFloat("MoveSpeed", moveVec.magnitude);
        }
    }
    private void FixedUpdate()
    {
        Vector3 moveDirection = new Vector3(moveVec.x, moveVec.y, 0).normalized;
        rb.velocity = moveDirection * WalkSpeed;
    }
}
