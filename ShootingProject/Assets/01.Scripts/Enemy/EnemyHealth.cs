using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : LivingEntity, IDamageable
{
    public float bloodEffectTime = 1.0f;
    private EnemyAI ai; // 적 사망처리 위해 가지고있어야함  

    private void Awake()
    {
        ai = GetComponent<EnemyAI>();
    }

    private IEnumerator ShowBloodEffect(Vector3 hitPos, Vector3 hitNormal)
    {
        GameObject effect = EffectManager.GetBloodEffect();
        Quaternion rot = Quaternion.LookRotation(hitNormal);
        effect.transform.position = hitPos;
        effect.transform.rotation = rot;
        effect.SetActive(true);
        yield return new WaitForSeconds(bloodEffectTime);
        effect.gameObject.SetActive(false);
    }

    public override void OnDamage(float damage, Vector3 hitPosition, Vector3 hitNormal)
    {
        base.OnDamage(damage, hitPosition, hitNormal);
        StartCoroutine(ShowBloodEffect(hitPosition, hitNormal));
    }

    public override void Die()
    {
        base.Die();
        ai.SetDead();
    }
}
