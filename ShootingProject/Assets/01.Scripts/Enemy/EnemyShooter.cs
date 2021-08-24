using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    private Gun gun;
    private Animator anim;
    private readonly int hashFire = Animator.StringToHash("fire");
    private readonly int hashReload = Animator.StringToHash("reload");

    // 총 발사 상태를 알려주는 변수들
    public bool isFire = false;
    public bool isReload = false;
    private float nextFire = 0.0f;
    private WaitForSeconds wsReload;
    private Transform playerTr;
    private readonly float damping = 10.0f; // damping 수차가 높을 수록 빠르게 회전

    private void Start()
    {
        anim = GetComponent<Animator>();
        playerTr = GameManager.instance.playerTR;
        gun = GetComponentInChildren<Gun>();           // 총 스크립트 가져오는 부분
        wsReload = new WaitForSeconds(gun.reloadTime); // 총의 재장전 타임
    }

    void Update()
    {
        if (isFire && !isReload)
        {
            Quaternion rot = Quaternion.LookRotation(playerTr.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * damping);

            if (Time.time >= nextFire)
            {
                Fire();
                nextFire = Time.time + gun.timeBetFire + Random.Range(0.0f, 0.3f);
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
            StartCoroutine(Reloading());
        }
    }

    IEnumerator Reloading()
    {
        anim.SetTrigger(hashReload);
        yield return wsReload;
        isReload = false;
    }
}
