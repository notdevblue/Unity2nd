using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public static EffectManager instance = null;
    public GameObject bloodEffectPrefab = null;
    private List<GameObject> bloodEffectList = new List<GameObject>();

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("다수의 이펙트 매니저가 실행중입니다.");
        }
        instance = this;

        for (int i = 0; i < 15; ++i)
        {
            GameObject effect = MakeBloodEffect();
            effect.SetActive(false);
            bloodEffectList.Add(effect);
        }
    }

    public static GameObject GetBloodEffect()
    {
        GameObject effect = instance.bloodEffectList.Find(x => !x.activeSelf);
        if(effect == null)
        {
            effect = instance.MakeBloodEffect();
            instance.bloodEffectList.Add(effect);
        }
        return effect;
    }

    private GameObject MakeBloodEffect()
    {
        return Instantiate(bloodEffectPrefab, transform.position, Quaternion.identity, transform);
    }
}
