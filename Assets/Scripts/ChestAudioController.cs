using UnityEngine;

public class ChestAudioController : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip chestOpenSound;

    public void PlayChestOpenSound()
    {
        Debug.Log("PlayChestOpenSound was called!");

        if (audioSource != null && chestOpenSound != null)
        {
            audioSource.PlayOneShot(chestOpenSound);
        }
        else
        {
            Debug.LogWarning("Missing AudioSource or AudioClip on ChestAudioController.");
        }
    }
}
