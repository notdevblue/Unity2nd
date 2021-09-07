using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxHealth : MonoBehaviour,IDamageable
{
    public float hp = 50f;
    public GameObject effect;

    public void OnDamage(float damage, Vector3 point, Vector3 normal)
    {
        hp -= damage;

        GameObject e = Instantiate(effect, point, Quaternion.LookRotation(normal));
        Destroy(e, 1.0f);

        if (hp <= 0)
        {
            Die(); //»ç¸Á½Ã Die È£Ãâ
        }
    }

    private void Die()
    {
        Destroy(this.gameObject);
    }
}
