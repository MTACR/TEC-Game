﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thrower : MonoBehaviour
{

    public ThrowerMissile missile;

    private Animator animator;
    private Transform player;

    void Start()
    {
        animator = GetComponent<Animator>();
        player = FindObjectOfType<Controller>().transform;
    }

    void Update()
    {
        if (player != null)
        {
            if (Vector2.Distance(transform.position, player.position) <= 10f)
            {
                missile.Fire();
                animator.SetTrigger("Throw");
                Destroy(gameObject, 0.5f);
            }
        }
        else
        {
            player = FindObjectOfType<Controller>().transform;
        }
    }

}
