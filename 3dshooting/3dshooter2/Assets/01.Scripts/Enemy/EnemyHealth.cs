using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : LivingEntity
{
    public float bloodEffectTime = 0.5f; //혈흔 이펙트 재생시간

    private EnemyAI ai;

    void Awake()
    {
        ai = GetComponent<EnemyAI>();
    }

    public override void OnDamage(float damage, Vector3 point, Vector3 normal)
    {
        base.OnDamage(damage, point, normal);
        if (death) return;
        StartCoroutine(ShowBloodEffect(point, normal));
    }

    IEnumerator ShowBloodEffect(Vector3 position, Vector3 normal)
    {
        GameObject effect = EffectManager.GetBloodEffect();
        Quaternion rot = Quaternion.LookRotation(normal);
        effect.transform.position = position;
        effect.transform.rotation = rot;
        effect.SetActive(true);
        yield return new WaitForSeconds(bloodEffectTime);
        effect.gameObject.SetActive(false);
    }

    public override void Die()
    {
        base.Die();
        ai.SetDead(); //이거 아직 구현 안함.
    }
}
