using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIButtonAudioController : MonoBehaviour
{
    public AudioSource audioSource;           // Reference to the AudioSource component
    public AudioClip buttonClickSound;        // The sound for button click
    public AudioClip buttonHoverSound;        // The sound for button hover

    void Start()
    {
        // If the AudioSource is not assigned in the Inspector, get it from the current object
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        Button button = GetComponent<Button>();
        if (button != null)
        {
            // Listen for button click
            button.onClick.AddListener(PlayButtonClickSound);
        }

        // If the button has an EventTrigger component, add the hover sound functionality
        EventTrigger eventTrigger = GetComponent<EventTrigger>();
        if (eventTrigger != null)
        {
            // Add hover sound event when mouse enters the button
            EventTrigger.Entry pointerEnterEntry = new EventTrigger.Entry();
            pointerEnterEntry.eventID = EventTriggerType.PointerEnter;
            pointerEnterEntry.callback.AddListener((data) => PlayButtonHoverSound());
            eventTrigger.triggers.Add(pointerEnterEntry);
        }
    }

    // Play the button click sound
    void PlayButtonClickSound()
    {
        if (audioSource != null && buttonClickSound != null)
        {
            audioSource.PlayOneShot(buttonClickSound);
        }
    }

    // Play the button hover sound
    void PlayButtonHoverSound()
    {
        if (audioSource != null && buttonHoverSound != null)
        {
            audioSource.PlayOneShot(buttonHoverSound);
        }
    }
}
