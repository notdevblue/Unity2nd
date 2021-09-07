using System;
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
    public ParticleSystem muzzleFlash; //�ѱ�ȭ�� ����Ʈ
    public ParticleSystem shellEjectEffect; // ź�� ���� ����Ʈ
    public float bulletLineEffectTime = 0.03f; //���η������� �׷����� �ð� 

    public LineRenderer bulletLineRenderer;
    public float damage = 25; //���� ������
    public float fireDistance = 50f; //���� ��Ÿ�
    public int magCapacity = 10; //źâ�� �뷮
    public int magAmmo; //���� ���� ���� źȯ
    public float timeBetFire = 0.12f; //�Ѿ� �߻� ����
    public float reloadTime = 1.0f; //������ �ð�
    public float lastFireTime; //���������� ���� �߻��� �ð�

    [Header("Audio clips")]
    public AudioClip reloadSound;        
    public AudioClip fireSound;
    public AudioClip fastReloadSound;

    private AudioSource audioSource;

    // bullet event
    public event Action<int> UpdateBullet;
    public event Action<int> UpdateMaxBullet;
    
    // reload event
    public event Action<float> ReloadEvent;
    private float currentReloadTime = 0;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        magAmmo = magCapacity;
        state = State.Ready;
        lastFireTime = 0;

        UpdateBullet    += x => {};
        UpdateMaxBullet += x => {};
        ReloadEvent     += x => {};

        UpdateBullet(magAmmo);
        UpdateMaxBullet(magCapacity);
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
        //총알 발사 사운드 재생
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
        UpdateBullet(magAmmo);
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

    public void FastReload()
    {
        audioSource.clip = fastReloadSound; // reload sound
        audioSource.Play();
        currentReloadTime = reloadTime;
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

    public IEnumerator ReloadRoutine()
    {
        state = State.Reloding;
        audioSource.clip = reloadSound;
        audioSource.Play();

        currentReloadTime = 0;
        while(currentReloadTime <= reloadTime)
        {
            yield return null;
            ReloadEvent(currentReloadTime / reloadTime); // current reload precentage
            currentReloadTime += Time.deltaTime;
        }
        
        ReloadEvent(0); // set statusbar to 0

        magAmmo = magCapacity;
        state = State.Ready;
        UpdateBullet(magAmmo);
        UIManager.instance.reloadSuccess = true;
    }
}

