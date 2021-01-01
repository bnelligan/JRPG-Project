using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using UnityEngine.InputSystem;

public class GridMovementController : MonoBehaviour
{

    // How long does it take to move 1 space
    private float WalkSpeed = 5f;
    private float LowMoveThreshold = 0.3f;
    public bool EnableInput = true;
    private bool isMoving = false;
    private float lastMoveTime;
    private float lerpDuration = .4f;
    Vector3 targetPos;
    Vector3 currentTilePos;
    Vector2 inputVec;
    Vector2Int moveVec = Vector2Int.zero;
    TileManager tileManager;
    Rigidbody2D rb;
    Animator animator;
    AudioSource audio;

    // Audio
    [SerializeField]
    AudioClip walkingSFX;
    // PlayerInputManager inputManager;

    private void Start()
    {
        targetPos = transform.position;
        rb = GetComponent<Rigidbody2D>();
        tileManager = FindObjectOfType<TileManager>();
        animator = GetComponentInChildren<Animator>();
        audio = GetComponent<AudioSource>();
        // inputManager = GetComponent<PlayerInputManager>();
    }
    private void Update()
    {
        if (EnableInput == true)
        {
            float inputX = Input.GetAxisRaw("Horizontal");
            float inputY = Input.GetAxisRaw("Vertical");
            if(inputX != moveVec.x || inputY != moveVec.y)
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

            if (moveVec.magnitude > 0 && isMoving == false)
            {
                audio.clip = walkingSFX;
                audio.loop = true;
                audio.Play();
                isMoving = true;
            }
            else if(moveVec.magnitude == 0 && isMoving == true)
            {
                audio.Stop();
                isMoving = false;
            }

            if(moveVec != Vector2Int.zero)
            {
                animator.SetFloat("Horizontal", inputX);
                animator.SetFloat("Vertical", inputY);
            }
            animator.SetFloat("MoveSpeed", moveVec.magnitude);
        }

        //Debug.Log($"Move vector: " + MoveVec);[
        targetPos = tileManager.FindMove(transform.position, moveVec, 1);
        currentTilePos = tileManager.FindTilePos(transform.position);//.FindMove(transform.position, Vector2Int.zero, 0);
        
        //Debug.Log($"Target position: {targetPos}");
        float moveDistance = Vector3.Distance(transform.position, targetPos);
        Vector3 moveDirection = targetPos - transform.position;
        Vector3 moveVec3 = new Vector3(moveVec.x, moveVec.y, 0);
        moveDirection.Normalize();
        bool isOppositeMovement = Vector3.Dot(moveVec3, moveDirection) < 0;

        if (moveVec3 == Vector3.zero || moveDistance < LowMoveThreshold || isOppositeMovement )//|| targetPos == currentTilePos)
        {
            rb.velocity = Vector2.zero;
            // lastMoveTime -= moveDistance * Time.deltaTime; // Go back in time to speed up lerp
            float lerpAmount = (Time.time - lastMoveTime) / (lerpDuration);
            transform.position = Vector3.Lerp(transform.position, targetPos, lerpAmount);
            //rb.velocity = Vector2.zero;
            //transform.position = Vector3.Lerp(transform.position, targetPos, (Time.time - lastMoveTime) / lerpDuration);
        }
        else
        {
            lastMoveTime = Time.time;
            rb.velocity = moveDirection * WalkSpeed;
        }
    }

    private void FixedUpdate()
    {
        // Check for exit key
        //if(Input.GetKeyDown(KeyCode.Escape))
        //{
        //    Application.Quit();
        //}       
    }

    public void DisableMovement()
    {
        EnableInput = false;
    }

    public void EnableMovement()
    {
        EnableInput = true;
    }
}
