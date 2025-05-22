using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 30;  // Will be set by GunController when bullet is spawned

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }
    }
}
