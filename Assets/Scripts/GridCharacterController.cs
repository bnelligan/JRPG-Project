using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCharacterController : MonoBehaviour
{

    // How long does it take to move 1 space
    public float WalkSpeed = 0.5f;
    float moveStartTime;
    Vector3 moveStartPosition;
    Vector3 targetPos;
    public bool EnableInput = true;

    private void Start()
    {
        targetPos = transform.position;
        moveStartPosition = transform.position;
        EnableInput = true;
    }
    void Update()
    {
        if (EnableInput == true)
        {
            float inputX = Input.GetAxis("Horizontal");
            float inputY = Input.GetAxis("Vertical");

            Vector3 MoveVec = Vector3.zero;

            if (inputX > 0)
            {
                MoveVec += Vector3.right;
            }
            else if (inputX < 0)
            {
                MoveVec += Vector3.left;
            }
            else if (inputY > 0)
            {
                MoveVec += Vector3.up;
            }
            else if (inputY < 0)
            {
                MoveVec += Vector3.down;
            }


            if (transform.position == targetPos)
            {
                moveStartPosition = transform.position;
                targetPos = transform.position + MoveVec;
                moveStartTime = Time.time;
            }

            float t = (Time.time - moveStartTime) / WalkSpeed;
            //if(t > 1f)
            //{
            //    t = 1f;
            //}
            float posX = Mathf.Lerp(moveStartPosition.x, targetPos.x, t);
            float posY = Mathf.Lerp(moveStartPosition.y, targetPos.y, t);
            transform.position = new Vector3(posX, posY, transform.position.z);
        }
    }

    IEnumerator MoveToPosition()
    {
        yield return new WaitForEndOfFrame();
    }
}
