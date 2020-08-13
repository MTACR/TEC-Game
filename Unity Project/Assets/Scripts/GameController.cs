﻿using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

    public Player2D player;
    public CinemachineVirtualCamera cam;
    public CinemachineConfiner confiner;
    public GameObject loading;
    public GameObject complete;
    public Volume damage;
    public static bool isPaused;
    public static bool canPause;

    private GameMenuButtons menu;
    private Lifebar lifebar;
    private Vector3 checkpoint;
    private CameraRange range;
    private int scene;
    private int lifes = 4;

    //private Dictionary<Type, int> pontuation;

    void Awake()
    {
        lifebar = FindObjectOfType<Lifebar>();
        range = FindObjectOfType<CameraRange>();
        menu = FindObjectOfType<GameMenuButtons>();
        menu.gameObject.SetActive(false);

        StartCoroutine(ILoadScene(4));
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (canPause)
            {
                if (isPaused)
                {
                    menu.Resume();
                }
                else
                {
                    menu.gameObject.SetActive(true);
                    menu.Pause();
                }
            }
        }
    }

    private IEnumerator ILoadScene(int s)
    {
        scene = s;
        checkpoint = new Vector3(-1, 5, 0);

        lifebar.gameObject.SetActive(false);
        complete.SetActive(false);
        loading.SetActive(true);

        AsyncOperation load = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);

        while (!load.isDone)
        {
            yield return null;
        }

        confiner.m_BoundingShape2D = GameObject.FindGameObjectWithTag("Grid").GetComponent<PolygonCollider2D>();

        /*pontuation = new Dictionary<Type, int>();

        foreach (Enemy2D e in FindObjectsOfType<Enemy2D>())
        {
            pontuation[e.GetType()] = 0;

            //TODO: não contar se inimigo morrer sozinho

            e.SetOnDieListener(() => pontuation[e.GetType()]++);
        }*/

        yield return new WaitForSecondsRealtime(1f);

        loading.SetActive(false);
        lifebar.gameObject.SetActive(true);
        range.SetEnabled(true);

        SetupPlayer();

        canPause = true;
    }

    private void SetupPlayer()
    {
        Player2D p = FindObjectOfType<Player2D>();

        if (p == null)
            p = Instantiate(player, checkpoint, Quaternion.identity, null);
        else
        {
            p.transform.position = checkpoint;
            p.EnableControls();
        }
            
        lifebar.SetPlayer(p);

        cam.Follow = p.transform;
        cam.LookAt = p.transform;
        cam.GetCinemachineComponent<CinemachineFramingTransposer>().m_XDamping = 4;

        p.SetOnDamageListener(() => damage.weight = (float)(Math.Exp(p.maxHP - p.GetHP()) / 100));

        p.SetOnDieListener(() =>
        {
            cam.GetCinemachineComponent<CinemachineFramingTransposer>().m_XDamping = 0;
            cam.Follow = null;
            cam.LookAt = null;
            

            lifes--;

            if (lifes == 0)
            {
                StartCoroutine(IGameOver());
            } 
            else
            {
                lifebar.SetExtraLifes(lifes);

                StartCoroutine(INewPlayer());

                StartCoroutine(IAnim());
            }
        });
    }

    private IEnumerator INewPlayer()
    {
        yield return new WaitForSecondsRealtime(2f);

        SetupPlayer();
    }

    private IEnumerator IGameOver()
    {
        canPause = false;

        yield return new WaitForSecondsRealtime(2.5f);

        foreach (GameObject o in FindObjectsOfType<GameObject>())
            Destroy(o);

        SceneManager.LoadScene("GameOver");
    }

    public void SetCheckpoint(Vector3 position)
    {
        checkpoint = position;
    }

    public void CompleteLevel(CinemachineVirtualCamera vcam)
    {
        StartCoroutine(ICompleteLevel(vcam));
    }

    private IEnumerator ICompleteLevel(CinemachineVirtualCamera vcam)
    {
        if (vcam != null)
            vcam.m_Priority = 100;

        canPause = false;

        range.SetEnabled(false);
        lifebar.gameObject.SetActive(false);

        Player2D p = FindObjectOfType<Player2D>();
        p.DisableControlsAndRun();

        foreach (Enemy2D e in FindObjectsOfType<Enemy2D>())
            e.SetEnabled(false);

        foreach (Bullet b in FindObjectsOfType<Bullet>())
            Destroy(b.gameObject);

        yield return new WaitForSecondsRealtime(0.5f);

        complete.SetActive(true);

        yield return new WaitForSecondsRealtime(3f);

        AsyncOperation load = SceneManager.UnloadSceneAsync(scene);

        while (!load.isDone)
        {
            yield return null;
        }

        //complete.SetActive(false);

        StartCoroutine(ILoadScene(scene + 1));
    }

    private IEnumerator IAnim()
    {
        yield return new WaitForSecondsRealtime(1f);

        while (damage.weight > 0)
        {
            damage.weight -= 0.05f;

            yield return new WaitForSecondsRealtime(0.01f);
        }
    }

    public void AddExtraLifes(int l)
    {
        int tmp = lifes + l;

        lifes = tmp <= 4 ? tmp : 4;

        lifebar.SetExtraLifes(lifes);
    }

}
