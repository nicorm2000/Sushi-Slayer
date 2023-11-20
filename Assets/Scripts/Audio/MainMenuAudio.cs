using UnityEngine;

public class MainMenuAudio : MonoBehaviour
{
    [Header("Audio Manager")]
    [SerializeField] AudioManager audioManager;
    [SerializeField] string intro;

    private void Start()
    {
        if (!AudioManager.mute)
        {
            audioManager.PlaySound(intro);
        }
    }
}