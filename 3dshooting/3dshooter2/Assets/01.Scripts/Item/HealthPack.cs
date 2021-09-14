using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : MonoBehaviour, IUseable
{
    
    public int value = 10;
    public AudioClip useSound;
    private AudioSource audioSource;
    private BoxCollider boxCol;
    private Transform[] children;


    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        boxCol = GetComponent<BoxCollider>();
        children = GetComponentsInChildren<Transform>(); // 0번은 부모의 것도 있다.
    }
    
    public void Use(GameObject target)
    {
        LivingEntity living = target.GetComponent<LivingEntity>();
        if (living != null)
        {
            living.RestoreHealth(value);
            for (int i = 1; i < children.Length; ++i)
            {
                children[i].gameObject.SetActive(false);
            }
            audioSource.clip = useSound;
            audioSource.Play();
            Invoke(nameof(SetDisable), 1.0f);
        }
    }

    public void SetDisable()
    {
        gameObject.SetActive(false);
    }
}
