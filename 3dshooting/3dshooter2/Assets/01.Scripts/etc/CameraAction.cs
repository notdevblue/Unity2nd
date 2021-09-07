using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine; //�ó׸ӽ� ����� ķ�ϰ�, ������Ʈ�� �ҷ��÷���

public class CameraAction : MonoBehaviour
{
    public static CameraAction instance { get; private set; }

    private CinemachineVirtualCamera followCam;
    private CinemachineBasicMultiChannelPerlin bPerlin;

    private bool isShake = false;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("�ټ��� ī�޶� �׼� ��ũ��Ʈ�� ������");
        }
        instance = this;
        followCam = GetComponent<CinemachineVirtualCamera>();
        bPerlin = followCam
                    .GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

    }

    public static void ShakeCam(float intensity, float time)
    {
        if (instance.isShake) return;
        
        instance.isShake = true;
        instance.StartCoroutine(instance.ShakeUpdate(intensity, time));
    }

    public IEnumerator ShakeUpdate(float intensity, float time)
    {
        bPerlin.m_AmplitudeGain = intensity;
        float t = 0;
        while (true)
        {
            yield return null;
            t += Time.deltaTime;
            if(t >= time)
            {
                break;
            }
            bPerlin.m_AmplitudeGain = Mathf.Lerp(intensity, 0, t / time);
        }
        bPerlin.m_AmplitudeGain = 0;
        isShake = false;
    }
}
