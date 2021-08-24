using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State
{
    Ready,
    Empty,
    Reloding
}

public class Gun : MonoBehaviour
{
    public State state { get; private set; }
    public Transform firePosition;
    public ParticleSystem muzzleFlash; //?????? ?????
    public ParticleSystem shellEjectEffect; // ¼?? ???? ?????
    public float bulletLineEffectTime = 0.03f; //??????????? ??????? ??? 

    public LineRenderer bulletLineRenderer;
    public float damage = 25; //???? ??????
    public float fireDistance = 50f; //???? ?????
    public int magCapacity = 10; //¼â?? ??
    public int magAmmo; //???? ???? ???? ¼?
    public float timeBetFire = 0.12f; //??? ??? ????
    public float reloadTime = 1.0f; //?????? ???
    public float lastFireTime; //?????????? ???? ????? ???

    [Header("Audio clips")]
    public AudioClip reloadSound;        
    public AudioClip fireSound;

    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        magAmmo = magCapacity;
        state = State.Ready;
        lastFireTime = 0;
    }

    public void Fire()
    {
        if(state == State.Ready && Time.time >= lastFireTime + timeBetFire)
        {
            lastFireTime = Time.time;
            Shot();
        }
    }

    private void Shot()
    {
        //?? ?? ????????? ??????
        audioSource.clip = fireSound;
        audioSource.Play();

        RaycastHit hit;
        Vector3 hitPosition = Vector3.zero;
        if(Physics.Raycast(
                firePosition.position, 
                firePosition.forward, out hit, fireDistance))
        {
            IDamageable target = hit.transform.GetComponent<IDamageable>();
            if(target != null)
            {
                target.OnDamage(damage, hit.point, hit.normal);
            }
            hitPosition = hit.point;
        }else
        {
            hitPosition = firePosition.position
                            + firePosition.forward * fireDistance;
        }

        StartCoroutine(ShotEffect(hitPosition));
        magAmmo--;
        if(magAmmo <= 0)
        {
            state = State.Empty;
        }
    }

    private IEnumerator ShotEffect(Vector3 hitPosition)
    {
        muzzleFlash.Play();
        shellEjectEffect.Play();
        bulletLineRenderer.SetPosition(1,
            bulletLineRenderer.transform.InverseTransformPoint( hitPosition ));
        bulletLineRenderer.gameObject.SetActive(true);
        yield return new WaitForSeconds(bulletLineEffectTime);
        bulletLineRenderer.gameObject.SetActive(false);
    }

    public bool Reload()
    {
        if(state == State.Reloding || magAmmo >= magCapacity)
        {
            return false;
        }
        StartCoroutine(ReloadRoutine());
        return true;
    }

    public IEnumerator ReloadRoutine ()
    {
        state = State.Reloding;
        audioSource.clip = reloadSound;
        audioSource.Play();
        
        yield return new WaitForSeconds(reloadTime);
        magAmmo = magCapacity;
        state = State.Ready;
    }
}

