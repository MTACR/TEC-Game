﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHack : Enemy
{

    protected NavMeshAgent agent;

    protected override void InitializeComponents()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    void FixedUpdate()
    {
        if (GetTarget() != null)
        {
            Vector2 lookDir = GetTarget().position - transform.position;
            rb.rotation = -Mathf.Atan2(lookDir.x, lookDir.y) * Mathf.Rad2Deg + 90f;

            agent.destination = GetTarget().position;
        }
    }

    public void Fire()
    {
        base.Fire(barrel);
    }
 
}
