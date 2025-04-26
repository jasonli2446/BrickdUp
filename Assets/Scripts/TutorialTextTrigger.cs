using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    public GameObject tutorialText;

    private void Start()
    {
        if (tutorialText != null)
            tutorialText.SetActive(false); // Hide it initially
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && tutorialText != null)
        {
            tutorialText.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && tutorialText != null)
        {
            tutorialText.SetActive(false);
        }
    }
}

