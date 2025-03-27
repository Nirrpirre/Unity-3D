using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bullet2 : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerHealth Player = collision.gameObject.GetComponent<PlayerHealth>();
            if (Player != null)
            {
                Player.TakeDamage(10);
            }
        }
    }
}
