using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public int attackDamage = 20;
    public LayerMask enemyLayers;

    [Header("Attack Cooldown")]
    public float attackRate = 2f;

    [Header("Sprites")]
    public Sprite idleSprite;
    public Sprite attackSprite;

    [Header("Attack Visual")]
    public float attackSpriteDuration = 0.15f;

    private float nextAttackTime = 0f;

    private SpriteRenderer spriteRenderer;
    private PlayerInputActions inputActions;

    private void Awake()
    {
        inputActions = new PlayerInputActions();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        inputActions.Enable();
        inputActions.Player.Attack.performed += OnAttack;
    }

    private void OnDisable()
    {
        inputActions.Player.Attack.performed -= OnAttack;
        inputActions.Disable();
    }

    private void OnAttack(InputAction.CallbackContext context)
    {
        if (Time.time < nextAttackTime)
            return;

        Attack();

        nextAttackTime = Time.time + 1f / attackRate;
    }

    private void Attack()
    {
        StartCoroutine(AttackAnimation());

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(
            attackPoint.position,
            attackRange,
            enemyLayers
        );

        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<EnemyHealth>()?.takeDamage(attackDamage);
        }
    }

    private IEnumerator AttackAnimation()
    {
        spriteRenderer.sprite = attackSprite;

        yield return new WaitForSeconds(attackSpriteDuration);

        spriteRenderer.sprite = idleSprite;
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}