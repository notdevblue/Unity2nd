using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public static EffectManager instance;
    public GameObject bloodEffectPrefab;
    private List<GameObject> bloodEffectList = new List<GameObject>();

    void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("다수의 이펙트 매니저가 실행되고 있습니다.");
        }
        instance = this;

        for(int i = 0; i < 15; i++)
        {
            //여기서 이펙트를 미리 만들어서 리스트에 넣어둔다.
            GameObject effect = MakeBloodEffect();
            effect.SetActive(false);
            bloodEffectList.Add(effect);
        }
    }

    private GameObject MakeBloodEffect()
    {
        return Instantiate(
            bloodEffectPrefab, 
            transform.position, 
            Quaternion.identity, 
            transform);
    }

    public static GameObject GetBloodEffect()
    {
        // bloodEffectList에 있는 원소들중에서 
        //gameobject의 activeSelf 가 false인 애를 가져와야하지
        GameObject effect = instance.bloodEffectList.Find( x => !x.activeSelf );
        if(effect == null)
        {
            effect = instance.MakeBloodEffect();
            instance.bloodEffectList.Add(effect);
        }
        return effect;
    }
}
