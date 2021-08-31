using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : LivingEntity
{
    public float bloodEffectTime = 1.0f;
    
    private HealthBar hpBar = null;

    private void Start()
    {
        hpBar = GetComponentInChildren<HealthBar>();
        hpBar.SetFill(health, initHealth); // init
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
        if (dead) return;
        base.OnDamage(damage, hitPosition, hitNormal);

        StartCoroutine(ShowBloodEffect(hitPosition, hitNormal));
        CameraAction.ShakeCam(4.0f, 0.3f);
        hpBar.SetFill(health, initHealth);
    }

    public override void Die()
    {
        base.Die();
    }
}
