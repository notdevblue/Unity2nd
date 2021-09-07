using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    private Gun gun;
    private Animator anim;
    private readonly int hashFire = Animator.StringToHash("fire");
    private readonly int hashReload = Animator.StringToHash("reload");

    //총알 발사 상태 변수들
    public bool isFire = false; //사격모드
    public bool isReload = false; //재장전 중인가?
    private float nextFire = 0.0f; //다음 사격가능시간
    private WaitForSeconds wsReload; //재장전 대기시간

    private Transform playerTR;
    private readonly float damping = 10.0f;

    void Start()
    {
        anim = GetComponent<Animator>();
        gun = GetComponentInChildren<Gun>();
        playerTR = GameManager.GetPlayer();
        wsReload = new WaitForSeconds(gun.reloadTime);
    }

    void Update()
    {
        if(isFire && !isReload)
        {
            Quaternion rot 
                = Quaternion.LookRotation(playerTR.position - transform.position);
            transform.rotation = Quaternion.Slerp(
                transform.rotation, rot, Time.deltaTime * damping);

            if(Time.time >= nextFire)
            {
                Fire();
                nextFire = Time.time + gun.timeBetFire
                             + Random.Range(0f, 0.4f); //나중에 수정
            }
        }
    }

    private void Fire()
    {
        anim.SetTrigger(hashFire);
        gun.Fire();
        if(gun.magAmmo <= 0)
        {
            gun.Reload();
            isReload = true;
            StartCoroutine(Reloading());//재장전 코루틴 시작
        }
    }

    IEnumerator Reloading()
    {
        anim.SetTrigger(hashReload);
        yield return wsReload;
        isReload = false;
    }
}
