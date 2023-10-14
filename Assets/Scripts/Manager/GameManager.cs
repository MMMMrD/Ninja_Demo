using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameManager : Singleton<GameManager>
{
    [HideInInspector]public bool gameOver;  //判断游戏是否结束
    [HideInInspector]public PlayerController player;
    public List<GameOverReset> enemies = new List<GameOverReset>();
    public Action scenceEvent; 
    public Action ResetGameAction;
    private CinemachineVirtualCamera cinemachine;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    void Update() 
    {
        if(player != null)
        {
            gameOver = player.isDead;
        }
    }

    public void RegisterPlayer(PlayerController other)
    {
        player = other;
        cinemachine = FindObjectOfType<CinemachineVirtualCamera>();
        
        if(cinemachine != null)
        {
            cinemachine.Follow = player.transform;
        }
    }

    public void RegisterEnemies(GameOverReset gameOverReset)
    {
        enemies.Add(gameOverReset);
    }

    public void ResetGame()    //游戏结束时，重制各个敌人和Boss状态
    {
        foreach (GameOverReset item in enemies)
        {
            item.GameOverReset();
        }

        ResetGameAction?.Invoke();  //同时执行其他有关游戏结束的函数
    }
}
