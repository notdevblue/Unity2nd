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
    private readonly int hashDie = Animator.StringToHash("die");
    private readonly int hashDieIndex = Animator.StringToHash("dieIndex");

    //적군의 시야를 위한 판별 코드
    private EnemyFOV fov;
    //총알발사를 위한 Shooter클래스
    private EnemyShooter shooter;

    void Awake()
    {
        moveAgent = GetComponent<MoveAgent>();
        anim = GetComponent<Animator>();
        // fov를 가져온다.
        fov = GetComponent<EnemyFOV>();
        shooter = GetComponent<EnemyShooter>();
    }

    void Start()
    {
        playerTr = GameManager.GetPlayer();
        ws = new WaitForSeconds(judgeDelay);//AI가 판단을 내리는 딜레이시간
    }

    void OnEnable()
    {
        isDie = false;
        state = EnemyState.PATROL;

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
                // 내 시야각안에 플레이어가 있으면서 보이기도 한다.
                if(fov.IsTracePlayer() && fov.IsViewPlayer() ){
                    state = EnemyState.ATTACK;
                }
                
            }else if(fov.IsTracePlayer()){
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
                    shooter.isFire = false;//공격중지
                    anim.SetBool(hashMove, true);
                    break;
                case EnemyState.TRACE:
                    moveAgent.traceTarget = playerTr.position;
                    shooter.isFire = false;//공격중지
                    anim.SetBool(hashMove, true);
                    break;
                case EnemyState.ATTACK:
                    moveAgent.Stop();
                    anim.SetBool(hashMove, false);
                    if(!shooter.isFire){
                        shooter.isFire = true;//공격시작
                    }
                    break;
                case EnemyState.DIE:
                    moveAgent.Stop();
                    shooter.isFire = false;
                    isDie = true;
                    anim.SetInteger(hashDieIndex, Random.Range(0, 3));
                    anim.SetTrigger(hashDie);
                    break;
            }
        }
    }
    // 적이 사망했을 때 개수를 줄이고 
    // 플레이어가 사망했을 때 적이 모두 사라지는 것, + 적 객체가 스폰도 안되야 한다.

    public void SetDead()
    {
        state = EnemyState.DIE;
        StartCoroutine(DeadProcess());
    }

    IEnumerator DeadProcess()
    {
        yield return new WaitForSeconds(5f);
        gameObject.SetActive(false);
    }
}
