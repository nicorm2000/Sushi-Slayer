using UnityEngine;

public class EnemyAnimationsStateController : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private Animator animator;

    [Header("References")]
    [SerializeField] private AIChase aiChase;
    [SerializeField] private AIShooterChase aiShooterChase;
    [SerializeField] private HealthSystem healthSystem;

    private void OnEnable()
    {
        aiChase.onEnemyWalkChange += HandleEnemyMovementChange;
        aiShooterChase.onShooterEnemyWalkChange += HandleShooterEnemyMovement;
        healthSystem.onEnemyDeadChange += HandleEnemyDeadChange;
    }

    private void HandleEnemyMovementChange(bool isWalkin) 
    {
        animator.SetBool("IsWalking", isWalkin);
    }

    private void HandleShooterEnemyMovement(bool isWalkin) 
    {
        animator.SetBool("IsWalking", isWalkin);
    }

    private void HandleEnemyDeadChange(bool isDead) 
    {
        animator.SetBool("IsDead", isDead);
    }

    private void OnDisable()
    {
        aiChase.onEnemyWalkChange -= HandleEnemyMovementChange;
        aiShooterChase.onShooterEnemyWalkChange -= HandleShooterEnemyMovement;
        healthSystem.onEnemyDeadChange -= HandleEnemyDeadChange;
    }
}
