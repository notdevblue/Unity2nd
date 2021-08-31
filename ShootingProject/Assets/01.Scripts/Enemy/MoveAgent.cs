using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveAgent : MonoBehaviour
{
    public Transform wayPointGroup;
    private List<Transform> wayPoints = new List<Transform>(); 
    //웨이포인트를 저장할 리스트

    public int nextIndex;
    private NavMeshAgent agent;

    private readonly float patrolSpeed = 1.5f;
    private readonly float traceSpeed = 4.0f;

    private bool _patrolling;
    public bool patrolling{
        get {return _patrolling;}
        set {
            _patrolling = value;
            if(_patrolling){
                agent.speed = patrolSpeed;
                MoveWayPoint();
            }
        }
    }

    private Vector3 _traceTarget;
    public Vector3 traceTarget  {
        get {return _traceTarget;}
        set {
            _traceTarget = value;
            agent.speed = traceSpeed;
            TraceTarget(_traceTarget);
        }
    }

    public float speed {
        get {
            return agent.velocity.magnitude;
        }
    }

    private void TraceTarget(Vector3 pos)
    {
        if(agent.isPathStale) return;
        agent.destination = pos;
        agent.isStopped = false;
    }

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false;

        agent.speed = patrolSpeed; //처음 패트롤 속도로
    }

    public void Stop()
    {
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        _patrolling = false;
    }

    void Start()
    {
        wayPointGroup = GameObject.Find("WaypointGroup").transform;

        wayPointGroup.GetComponentsInChildren<Transform>(wayPoints);
        wayPoints.RemoveAt(0); // 0은 부모 좌표
        MoveWayPoint(); //다음 웨이포인트로 이동한다.
    }

    private void MoveWayPoint()
    {
        //경로가 아직 설정되어 있지 않다면 true를 리턴한다.
        if(agent.isPathStale) return;
    
        agent.destination = wayPoints[nextIndex].position;

        agent.isStopped = false; //에이전트를 on해준다.
    }

    // Update is called once per frame
    void Update()
    {
        if(!_patrolling) return;
        
        if(agent.velocity.sqrMagnitude >= 0.04f && agent.remainingDistance <= 0.5f){
            nextIndex = (++nextIndex) % wayPoints.Count;
            MoveWayPoint();
        }
    }
}
