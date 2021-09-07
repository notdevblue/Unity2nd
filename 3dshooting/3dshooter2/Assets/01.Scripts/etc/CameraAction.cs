using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine; //시네머신 버츄얼 캠하고, 컴포넌트를 불러올려면

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
            Debug.LogError("다수의 카메라 액션 스크립트가 실행중");
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
