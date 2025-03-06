using UnityEngine;
using System.Collections;
using Unity.Cinemachine;

public class CameraController : MonoBehaviour
{
    private CinemachineCamera cinemachineCamera;
    private CinemachineBasicMultiChannelPerlin perlinNoise;

    void Awake()
    {
        cinemachineCamera = GetComponent<CinemachineCamera>();
        if (cinemachineCamera != null)
        {
            perlinNoise = cinemachineCamera.GetComponent<CinemachineBasicMultiChannelPerlin>();
            ResetIntensity();
        }
        else
        {
            Debug.LogError("CinemachineVirtualCamera component is not found on the GameObject.");
        }
    }

    public void ShakeCamera(float intensity, float shakeTime)
    {
        if (perlinNoise != null)
        {
            perlinNoise.AmplitudeGain = intensity;
            StartCoroutine(waitTime(shakeTime));
        }
        else
        {
            Debug.LogError("CinemachineBasicMultiChannelPerlin component is not found on the CinemachineVirtualCamera.");
        }
    }

    IEnumerator waitTime(float time)
    {
        yield return new WaitForSeconds(time);
        ResetIntensity();
    }

    void ResetIntensity()
    {
        if (perlinNoise != null)
        {
            perlinNoise.AmplitudeGain = 0;
        }
    }
}
