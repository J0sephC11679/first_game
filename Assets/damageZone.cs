using UnityEngine;

public class damageZone : MonoBehaviour
{

    public int damage = 100;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerHealth player = collision.GetComponent<PlayerHealth>();

        if (player != null)
        {
            player.takeDamage(damage);
        }
    }
}
