using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFOV : MonoBehaviour
{
    public float viewRange = 15.0f;
    [Range(0, 360)]
    public float viewAngle = 120.0f;

    public LayerMask layerMask;
    private Transform playerTr;
    private int playerLayer; //플레이어가 속해있는 레이어

    //반지름 1인 원의 원주에 있는 점의 좌표를 구하는 함수
    public Vector3 CirclePoint(float angle)
    {
        angle += transform.eulerAngles.y;
        return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad),
                            0,
                            Mathf.Cos(angle * Mathf.Deg2Rad));
    }

    void Start()
    {
        playerTr = GameManager.instance.playerTR;
        playerLayer = LayerMask.NameToLayer("PLAYER");
        //플레이어 레이어의 번호를 알아온다.
    }

    //시야범위안에 플레이어가 있는가?
    public bool IsTracePlayer()
    {
        bool isTrace = false;
        Collider[] colls = Physics.OverlapSphere(
            transform.position, viewRange, 1 << playerLayer);
        if(colls.Length == 1){
            Vector3 dir = (playerTr.position - transform.position).normalized;
            
            if(Vector3.Angle(transform.forward, dir) < viewAngle * 0.5f){
                isTrace = true;
            }
        }
        return isTrace;
    }

    //플레이어와 적 사이에 아무것도 없이 플레이어가 보이는가?
    public bool IsViewPlayer()
    {   
        bool isView = false;
        RaycastHit hit;
        Vector3 dir = (playerTr.position - transform.position).normalized;
        if(Physics.Raycast(transform.position + new Vector3(0,0.5f, 0), dir, out hit, layerMask)){
            isView = (hit.collider.gameObject.CompareTag("PLAYER"));
        }
        return isView;
    }
}
