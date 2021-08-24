using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSensor : MonoBehaviour
{
    public float openTime = 2f;  //열려있는 시간
    public float openSpeed = 5f; //열리는 속도
    public Vector3 openDistance = new Vector3(4, 0, 0 );//x축 기준으로 움직이는 문

    private bool isOpen = false; //문이 열려있는가?
    private Vector3 originPoint; //원본 위치
    private Vector3 targetPoint; //이동할 위치

    private Coroutine co = null;

    void Start()
    {
        originPoint = transform.parent.position;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("PLAYER"))
        {
            if(isOpen && co != null) StopCoroutine(co);
            OpenDoor();
        }
    }

    private void OpenDoor()
    {
        if(isOpen) return;
        targetPoint = originPoint + openDistance; //타겟위치를 저장하고
        isOpen = true;
    }

    private void CloseDoor()
    {
        isOpen = false;
    }

    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("PLAYER"))
        {
            co = StartCoroutine("StayOpen");
        }
    }

    IEnumerator StayOpen()
    {
        yield return new WaitForSeconds(openTime);
        CloseDoor();
    }

    void Update()
    {
        if(isOpen){
            Vector3 nextPos = Vector3.Lerp(
                    transform.parent.position,
                    targetPoint,
                    Time.deltaTime * openSpeed);

            transform.parent.position = nextPos;
        }else{
            if( (transform.parent.position - originPoint).sqrMagnitude <= 0.01f ){
                return;
            }
            

            Vector3 nextPos = Vector3.Lerp(
                    transform.parent.position,
                    originPoint,
                    Time.deltaTime * openSpeed);

            transform.parent.position = nextPos;
        }
    }
}
