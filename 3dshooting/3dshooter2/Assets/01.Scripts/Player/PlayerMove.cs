using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotateSpeed = 6f;

    private PlayerInput playerInput;
    private CharacterController characterController;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        characterController = GetComponent<CharacterController>();
    }
    
    private void FixedUpdate()
    {
        Rotate();
        Move();
    }

    private void Move()
    {
        Transform cam = Camera.main.transform;

        Vector3 forward = Quaternion.Euler(new Vector3(-53.519f, 0, 0)) 
                            * cam.forward;
        Vector3 right = cam.right; //이거는 카메라의 오른쪽 벡터

        Vector3 dir = ( forward * playerInput.frontMove  
                        + right * playerInput.rightMove).normalized;
        dir.y = 0;
        characterController.Move(dir * moveSpeed * Time.deltaTime);
    }

    private void Rotate()
    {
        Vector3 target = playerInput.mousePos;
        target.y = 0;
        Vector3 v = target - transform.position;
        float degree = Mathf.Atan2(v.x, v.z) * Mathf.Rad2Deg;
        float rot = Mathf.LerpAngle(
                        transform.eulerAngles.y, 
                        degree, 
                        Time.deltaTime * rotateSpeed);
        transform.eulerAngles = new Vector3(0, rot, 0);
    }
}