﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMovementController : MonoBehaviour
{

    // How long does it take to move 1 space
    [SerializeField]
    private float WalkSpeed = 1f;
    [SerializeField]
    private float LowMoveThreshold = 0.5f;
    public bool EnableInput = true;
    private float lastMoveTime;
    public float lerpDuration = 5;
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
            Debug.Log($"InputX: {inputX}");
            Debug.Log($"InputY: {inputY}");
            // Check x-axis input
            if (inputX > 0)
            {
                MoveVec += Vector2Int.right;
            }
            else if (inputX < 0)
            {
                MoveVec += Vector2Int.left;
            }

            // Check y-axis input
            if (inputY > 0)
            {
                MoveVec += Vector2Int.up;
            }
            else if (inputY < 0)
            {
                MoveVec += Vector2Int.down;
            }
        }

        //Debug.Log($"Move vector: " + MoveVec);
        targetPos = tileManager.FindMove(transform.position, MoveVec, 1);
        //Debug.Log($"Target position: {targetPos}");
        float moveDistance = Vector3.Distance(transform.position, targetPos);
        Vector3 moveDirection = targetPos - transform.position;
        Vector3 moveVec3 = new Vector3(MoveVec.x, MoveVec.y, 0);
        moveDirection.Normalize();
        bool isOppositeMovement = Vector3.Dot(moveVec3, moveDirection) < 0;

        if (moveVec3 != Vector3.zero || moveDistance < LowMoveThreshold || isOppositeMovement)
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
            rb.velocity = moveVec3 * WalkSpeed;
        }
    }
}
