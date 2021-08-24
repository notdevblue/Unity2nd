using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public enum EnemyState
    {
        PATROL,
        TRACE,
        ATTACK,
        DIE
    }
    
    public EnemyState state = EnemyState.PATROL; //처음에는 패트롤 상태로 둔다.
    
    private Transform playerTr;
    //플레이어의 위치를 저장할 변수. 나중에는 게임매니저에서 가져온다.

    public float attackDist = 5.0f;
    public float traceDist = 10.0f;
    public float judgeDelay = 0.3f;

    public bool isDie = false;
    private WaitForSeconds ws;
    private MoveAgent moveAgent;

    private Animator anim;
    private readonly int hashMove = Animator.StringToHash("isMove");
    private readonly int hashSpeed = Animator.StringToHash("speed");

    private EnemyFOV fov;
    private EnemyShooter shooter;

    void Awake()
    {
        moveAgent = GetComponent<MoveAgent>();
        anim = GetComponent<Animator>();
        fov = GetComponent<EnemyFOV>();
        shooter = GetComponent<EnemyShooter>();
    }

    void Start()
    {
        playerTr = GameManager.instance.playerTR;
        ws = new WaitForSeconds(judgeDelay);//AI가 판단을 내리는 딜레이시간
    }

    void OnEnable()
    {
        StartCoroutine(CheckState());
        StartCoroutine(DoAction());
    }

    void Update()
    {
        anim.SetFloat(hashSpeed, moveAgent.speed);
    }

    IEnumerator CheckState()
    {
        while(!isDie){
            if(state == EnemyState.DIE)
                yield break; //코루틴 종료

            if(playerTr == null){
                yield return ws;
            }

            float dist = (playerTr.position - transform.position).sqrMagnitude;

            //공격사거리 내라면 공격            
            if(dist <= attackDist * attackDist){
                if(fov.IsViewPlayer() && fov.IsTracePlayer()){
                    state = EnemyState.ATTACK;
                }else if(fov.IsTracePlayer()){
                    state = EnemyState.TRACE;
                }
            }else if(fov.IsTracePlayer() && fov.IsViewPlayer()){
                state = EnemyState.TRACE;
            }else {
                state = EnemyState.PATROL;
            }
            yield return ws; //저지 시간만큼 딜레이
        }
        
    }

    IEnumerator DoAction()
    {
        while(!isDie){
            yield return ws;
            switch(state){
                case EnemyState.PATROL:
                    moveAgent.patrolling = true;
                    shooter.isFire = false;
                    anim.SetBool(hashMove, true);
                    break;
                case EnemyState.TRACE:
                    moveAgent.traceTarget = playerTr.position;
                    shooter.isFire = false;
                    anim.SetBool(hashMove, true);
                    break;
                case EnemyState.ATTACK:
                    moveAgent.Stop();
                    anim.SetBool(hashMove, false);
                    if(!shooter.isFire)
                    {
                        shooter.isFire = true;
                    }
                    break;
                case EnemyState.DIE:
                    moveAgent.Stop();
                    shooter.isFire = false;
                    break;
            }
        }
    }

    public void SetDead()
    {
        state = EnemyState.DIE;
    }
}
