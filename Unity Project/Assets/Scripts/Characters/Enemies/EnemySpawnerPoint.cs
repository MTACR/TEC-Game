﻿using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class EnemySpawnerPoint : MonoBehaviour
{
    public Enemy2D enemy;
    public float spawnCooldown = 2;
    public int enemyLimit = 5;
    float nextSpawnTime;
    private ArrayList enemyList;
    private bool isAlive = true;

    void Start()
    {
        enemyList = new ArrayList(enemyLimit);
        nextSpawnTime = spawnCooldown;
    }

    void FixedUpdate()
    {
        nextSpawnTime -= Time.fixedDeltaTime;

        if (nextSpawnTime < 0f) 
        {
            Spawn();
        }
    }

    private void Spawn()
    {
        if ((enemyList.Count < enemyLimit) && isAlive && !GetComponent<Renderer>().IsVisibleFrom(Camera.main))
        {
            Enemy2D e = Instantiate(enemy, transform.position, quaternion.identity);
            e.SetOnDieListener(() => enemyList.Remove(e));
            enemyList.Add(e);
            nextSpawnTime = spawnCooldown;
        }
    }

    public void SetEnabled(bool enabled)
    {
        this.enabled = enabled;
    }

    public void Kill()
    {
        isAlive = false;
    }

}