using UnityEngine;

public class SlipperySurface : MonoBehaviour
{
    [SerializeField] private float speedMultiplier = 1.5f; // Speed multiplier when inside the slippery zone

    private AIChase aiChase;
    private AIShooterChase aiShooterChase;

    private void OnTriggerEnter(Collider other)
    {
        aiChase = other.GetComponent<AIChase>();
        aiShooterChase = other.GetComponent<AIShooterChase>();

        if (aiChase != null)
        {
            aiChase.chaseSpeed *= speedMultiplier;
        }
        else if (aiShooterChase != null)
        {
            aiShooterChase.chaseSpeed *= speedMultiplier;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        aiChase = other.GetComponent<AIChase>();
        aiShooterChase = other.GetComponent<AIShooterChase>();

        if (aiChase != null)
        {
            aiChase.chaseSpeed /= speedMultiplier;
        }
        else if (aiShooterChase != null)
        {
            aiShooterChase.chaseSpeed /= speedMultiplier;
        }
    }
}