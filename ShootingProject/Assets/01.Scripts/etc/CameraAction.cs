using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine; // 시네미신 버츄얼 켐하고, 컴포넌트 불러오려고

public class CameraAction : MonoBehaviour
{
    static public CameraAction instance { get; set; }
    
    private CinemachineVirtualCamera followCam;
    private CinemachineBasicMultiChannelPerlin bPerlin;

    private bool isShake = false;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogWarning("다수의 CameraAction 스크립트가 실행되고 있습니다.");
        }
        instance = this;
        followCam = GetComponent<CinemachineVirtualCamera>();
        bPerlin = followCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    static public void ShakeCam(float intensity, float time)
    {
        if(instance.isShake) return;

        instance.isShake = true;
        instance.StartCoroutine(instance.ShakeUpdate(intensity, time));
    }

    public IEnumerator ShakeUpdate(float intensity, float time)
    {
        float t = time;
        float startedTime = Time.time;

        bPerlin.m_AmplitudeGain = intensity;

        while(true)
        {
            yield return null;

            t -= Time.deltaTime;
            // Time.deltaTime : 프레임 사이 시간

            if(t <= 0)
            {
                t = 0;
                break;
            }

            bPerlin.m_AmplitudeGain = Mathf.Lerp(intensity, 0, 1 - t / time); // 1 - t / time : 시간이 흐를수록 점점 0으로...

        }

        bPerlin.m_AmplitudeGain = 0;

        isShake = false;
    }
}
