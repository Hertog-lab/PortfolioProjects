using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignalEmitter : MonoBehaviour
{
    public AudioSource audioSource;
    [SerializeField] AudioClip signal;
    float volumeModifier;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = 0;
    }

    void Update()
    {
        if (!InfoManager.instance._scannerIsActive)
            audioSource.Stop();
    }

    public void SetVolume(float angle)
    {
        if (!audioSource.isPlaying)
            audioSource.Play();

        float convertedAngle;
        convertedAngle = angle / 80;
        convertedAngle = Mathf.Clamp(convertedAngle, 0.01f, 1);
        audioSource.volume = 1 - convertedAngle;
    }
}
