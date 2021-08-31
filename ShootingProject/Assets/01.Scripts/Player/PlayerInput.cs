using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//code.gondr.net => 87번 강의 
public class PlayerInput : MonoBehaviour
{
    public string frontAxisName = "Vertical";
    public string rightAxisName = "Horizontal";
    public string fireButtonName = "Fire1";
    public string reloadButtonName = "Reload";

    public float frontMove {get; private set;}
    public float rightMove {get; private set;}
    public bool fire {get; private set;}
    public bool reload {get; private set;}
    public Vector3 mousePos {get; private set;}
    public LayerMask whatIsGround;

    void Update()
    {
        frontMove = Input.GetAxis(frontAxisName);
        rightMove = Input.GetAxis(rightAxisName);
        fire = Input.GetButtonDown(fireButtonName);
        reload = Input.GetButtonDown(reloadButtonName);

        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        //mousePosition은 스크린좌표로 나온다. 
        //따라서 스크린좌표로 레이를 쏘면 그 방향에 있는 월드좌표를 구할 수 있다.
        RaycastHit hit;
        float depth = Camera.main.farClipPlane; //f12적으면 유니티 함수들은 정의 파일을 볼 수 있다.
        if(Physics.Raycast(camRay, out hit, depth, whatIsGround)){
            mousePos = hit.point;
            // Debug.DrawRay(Camera.main.transform.position, 
            //             camRay.direction * depth,
            //             Color.red,
            //             0.5f);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(mousePos, 0.5f);
        //마우스 위치에 0.5반지름으로 원을 그려준다.
    }
}
