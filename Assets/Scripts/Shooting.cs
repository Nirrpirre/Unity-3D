using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GunController : MonoBehaviour
{
    public GameObject bulletPrefab; 
    public Transform firePoint; 
    public float bulletSpeed = 20f; 
    public float fireRate = 0.5f; 
    private float nextFireTime = 0f;
    public bool allowedToShoot = true;
    

    void Update()
    {

        if (!allowedToShoot) return;

        if (Input.GetKey(KeyCode.Mouse0) && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;


        }
    }

    void Shoot()
    {
        
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.velocity = firePoint.forward * bulletSpeed;
    }

    public void SetallowedToShoot(bool state)
    {
        allowedToShoot = state;
    }
}
