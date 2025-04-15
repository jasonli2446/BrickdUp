using UnityEngine;

public class PlayerAudioController : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip jumpSound;
    public AudioClip walkSound; // Add this for walking sound

    // Called by jump animation event
    public void PlayJumpSound()
    {
        if (jumpSound != null && audioSource != null)
            audioSource.PlayOneShot(jumpSound);
    }

    // Called by walk animation event
    public void PlayWalkSound()
    {
        if (walkSound != null && audioSource != null)
            audioSource.PlayOneShot(walkSound);
    }
}

