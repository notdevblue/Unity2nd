using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : LivingEntity
{
    public float bloodEffectTime = 0.5f; //���� ����Ʈ ����ð�

    private HealthBar hpBar;

    private void Start()
    {
        hpBar = GetComponentInChildren<HealthBar>();
    }

    public override void OnDamage(float damage, Vector3 point, Vector3 normal)
    {
        if (death) return;
        base.OnDamage(damage, point, normal);        

        StartCoroutine(ShowBloodEffect(point, normal));
        CameraAction.ShakeCam(4, 0.3f);
        hpBar.SetFill(health, initHealth);
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
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!death)
        {
            IUseable item = other.GetComponent<IUseable>();

            if(item != null)
            {
                item.Use(gameObject);
            }
        }
    }

    public override void RestoreHealth(float value)
    {
        base.RestoreHealth(value);
        hpBar.SetFill(health, initHealth);
    }
}
