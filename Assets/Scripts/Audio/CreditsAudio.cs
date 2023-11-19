using UnityEngine;

public class CreditsAudio : MonoBehaviour
{
    [Header("Audio Manager")]
    [SerializeField] AudioManager audioManager;
    [SerializeField] string intro;

    private void Start()
    {
        audioManager.PlaySound(intro);
    }
}