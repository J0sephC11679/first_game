using UnityEngine;

public class PlayerAttack : MonoBehaviour {

    public GameObject attackPoint;
    public float radius;
    public LayerMask enemies;
    public void attack()
    {
        Collider2D[] enemy = Physics2D.OverlapCircleAll(attackPoint.transform.position, radius, enemies);
    }

}
