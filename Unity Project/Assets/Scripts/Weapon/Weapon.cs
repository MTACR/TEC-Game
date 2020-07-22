﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    public Bullet bullet;
    public float fireRate = 1f;

    private float lastCooldown;

    void Start()
    {
        //if (transform.parent != null)
            //tag = transform.parent.tag;

        fireRate = 1 / fireRate;
    }

    public void Fire(Transform barrel)
    {
        if (lastCooldown <= Time.time)
        {
            lastCooldown = Time.time + fireRate;
            Instantiate(bullet, barrel.position, barrel.rotation).tag += tag;
        }
    }

    public bool CanFire()
    {
        return (lastCooldown <= Time.time) ? true : false;
    }

}
