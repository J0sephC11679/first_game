using Unity.VisualScripting;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 1000;
    public int currentHealth;

    public HealthBar healthBar;

    [SerializeField] private GameObject GameOver;

    private bool isDead = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

        if (GameOver != null)
            GameOver.SetActive(false);
    }

    public void takeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;

        healthBar.SetHealth(currentHealth);

        if (currentHealth < 0)
        {
            gameOver();
        }
    }

    void gameOver()
    {
        isDead = true;

        if (GameOver != null)
            GameOver.SetActive(true);

        Time.timeScale = 0f;
    }
}
