﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thrower : Enemy2D
{

    public ThrowerMissile missile;

    protected override void InitializeComponents()
    {
        attackAction = Attack;
    }

    void Update()
    {
        attackAction();
    }

    public override void Attack()
    {
        if (GetTarget() != null)
        {
            if (Vector2.Distance(transform.position, GetTarget().position) <= 10f)
            {
                missile.Fire();
                animator.SetTrigger("Throw");
                Destroy(transform.parent.gameObject, 0.5f);
            }
        }
    }

}
