using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMovementController : MonoBehaviour
{

    // How long does it take to move 1 space
    public float WalkSpeed = 1f;
    public float UpperMoveThreshold = 1.0f;
    public float LowerMoveThreshold = 0.25f;
    public bool EnableInput = true;
    Vector3 targetPos;
    TileManager tileManager;
    Rigidbody2D rb;
    private void Start()
    {
        targetPos = transform.position;
        rb = GetComponent<Rigidbody2D>();
        tileManager = FindObjectOfType<TileManager>();
    }
    void Update()
    {
        //if (transform.position == targetPos)
        //{
        //    moveStartPosition = transform.position;
        //    targetPos = transform.position + MoveVec;
        //    moveStartTime = Time.time;
        //}

        //float t = (Time.time - moveStartTime) / WalkSpeed;
        //if(t > 1f)
        //{
        //    t = 1f;
        //}
        //float posX = Mathf.Lerp(moveStartPosition.x, targetPos.x, t);
        //float posY = Mathf.Lerp(moveStartPosition.y, targetPos.y, t);
        //transform.position = new Vector3(posX, posY, transform.position.z);
    }

    private void FixedUpdate()
    {
        Vector2Int MoveVec = Vector2Int.zero;
        if (EnableInput == true)
        {
            float inputX = Input.GetAxisRaw("Horizontal");
            float inputY = Input.GetAxisRaw("Vertical");
            if (inputX > 0)
            {
                MoveVec += Vector2Int.right;
            }
            else if (inputX < 0)
            {
                MoveVec += Vector2Int.left;
            }

            if (inputY > 0)
            {
                MoveVec += Vector2Int.up;
            }
            else if (inputY < 0)
            {
                MoveVec += Vector2Int.down;
            }
        }

        targetPos = tileManager.FindMove(transform.position, MoveVec, 1);
        float moveDistance = Vector3.Distance(transform.position, targetPos);
        Vector3 moveDirection = targetPos - transform.position;
        moveDirection.Normalize();
        // Snapping to position is choppy af
        if (moveDistance < LowerMoveThreshold)
        {
            transform.position = targetPos;
        }
        else if (moveDistance > LowerMoveThreshold)
        {
            rb.velocity = moveDirection * WalkSpeed * Time.fixedDeltaTime;
            Debug.Log($"Full Speed! ({moveDirection})");
        }
        else
        {
            float lerpSpeed = Mathf.Lerp(WalkSpeed, 0f, moveDistance / LowerMoveThreshold);// moveDistance / UpperMoveThreshold + LowerMoveThreshold);
            Debug.Log($"Lerp Speed: {lerpSpeed} ({moveDistance} / {LowerMoveThreshold}");
            rb.velocity = lerpSpeed * moveDirection * Time.fixedDeltaTime;
        }
    }
}
