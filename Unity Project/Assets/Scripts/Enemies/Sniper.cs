﻿using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class Sniper : Enemy2D
{
    private Vector3 yAxis;

    public Transform barrelF;
    public Transform barrelDU;
    public Transform barrelDD;

    void Start()
    {
        setTarget();
        SetAnimator();
        setWeapon();
        setEnemySpawner();
        attackAction = CanAttack;
        yAxis = new Vector3(0f, 1f, 0f);
    }

    void Update()
    {
        if (target) {
            attackAction(); 
        }
    }


    public override void Attack()
    {
        transform.rotation = (transform.position.x > target.position.x) ? Quaternion.identity : Quaternion.Euler(0f, 180f, 0f); // setShootingDir()

        float angle = Vector3.Angle(yAxis, target.position - transform.position); //getAimingAngle()
        animator.SetFloat("angle", angle);

        if (weapon.CanFire()) {

            if (angle > 0f && angle < 70f) //setBarrel
            {
                barrel = barrelDU;
            } else {
                if (angle > 70f && angle < 110f)
                {
                    barrel = barrelF;
                } else
                {
                    barrel = barrelDD;
                }
            }

            barrel.rotation = Quaternion.Euler(-angle - 90f, (target.position.x < transform.position.x) ? 90f : -90f, -90f);

            weapon.Fire(barrel);
        }
    }

}
