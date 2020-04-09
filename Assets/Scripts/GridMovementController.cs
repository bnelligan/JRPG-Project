using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMovementController : MonoBehaviour
{

    // How long does it take to move 1 space
    public float WalkSpeed = 1f;
    public float UpperMoveThreshold = 0.5f;
    public float LowerMoveThreshold = 0.05f;
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
        float moveDistance = Vector3.Distance(transform.position, targetPos);
        Vector3 moveDirection = targetPos - transform.position;
        if (moveDistance < LowerMoveThreshold)
        {
            transform.position = targetPos;
        }
        else if (moveDistance > UpperMoveThreshold)
        {
            transform.position += moveDirection * WalkSpeed * Time.fixedDeltaTime;
        }
        else
        {
            float lerpSpeed = Mathf.Lerp(WalkSpeed , WalkSpeed * 0.5f, moveDistance / UpperMoveThreshold + LowerMoveThreshold);
            transform.position += lerpSpeed * moveDirection * Time.fixedDeltaTime;
        }
    }
}
