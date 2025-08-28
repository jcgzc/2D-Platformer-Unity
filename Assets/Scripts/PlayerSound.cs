using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip footstepClip;
    public AudioClip jumpClip;
    public AudioClip landClip;

    public float footstepDuration = 0.7f; // how long to let the footstep play

    public void PlayFootstepSound()
    {
        StartCoroutine(PlayTrimmedSound(footstepClip, footstepDuration));
    }

    public void PlayJumpSound()
    {
        audioSource.PlayOneShot(jumpClip);
    }

    public void PlayLandSound()
    {
        audioSource.PlayOneShot(landClip);
    }

    private System.Collections.IEnumerator PlayTrimmedSound(AudioClip clip, float duration)
    {
        audioSource.clip = clip;
        audioSource.Play();
        yield return new WaitForSeconds(duration);
        audioSource.Stop();
    }
}


