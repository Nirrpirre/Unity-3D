using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public float health = 100f;
    private float maxHealth;

    [Header("Game Over UI")]
    public GameObject gameOverCanvas;

    [Header("Health Bar UI")]
    public Image healthBar;

    Rigidbody rb;
    GameObject[] enemyList;

    void Start()
    {
        maxHealth = health;
        rb = GetComponent<Rigidbody>();
        enemyList = GameObject.FindGameObjectsWithTag("Enemy");

        gameOverCanvas.SetActive(false);
        UpdateHealthBar();
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        health = Mathf.Clamp(health, 0, maxHealth);

        UpdateHealthBar();

        if (health <= 0)
        {
            EndGame();
        }
    }

    void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            float healthPercent = health / maxHealth;
            healthBar.rectTransform.localScale = new Vector3(healthPercent, 1f, 1f);
        }
    }

    private void EndGame()
    {
        gameOverCanvas.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        rb.constraints = RigidbodyConstraints.FreezeAll;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        GameObject playerCamera = GameObject.Find("Main Camera");
        if (playerCamera.TryGetComponent(out PlayerCam playerCam))
        {
            playerCam.SetCanLook(false);
        }

        GameObject gun = GameObject.Find("SciFiGunLightBlack");
        if (gun.TryGetComponent(out GunController gunController))
        {
            gunController.SetallowedToShoot(false);
        }

        foreach (GameObject enemy in enemyList)
        {
            if (enemy.TryGetComponent(out Enemy enemyScript))
            {
                enemyScript.SetEnemyState(false);
            }
        }
    }

    public float GetHealth()
    {
        return health;
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
