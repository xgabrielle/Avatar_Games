using System;
using NUnit.Framework.Internal;
using Unity.VisualScripting;
using UnityEngine;


public class CheckerGame : MonoBehaviour
{
    private AIPlayer aiPlayer;
    private Player player;
    internal bool isAiTurn;
    internal bool isGameOver;
    internal string turn;
    
    internal CheckersMove lastMove = new();

    private void Start()
    {
        player = GetComponent<Player>();
        aiPlayer = GetComponent<AIPlayer>();
        
        isAiTurn = false;
        isGameOver = false;

    }
    
    private void Update()
    {
        if (!isGameOver)
        {
            if (!isAiTurn)
            {
                turn = "White";
                if (Input.GetMouseButtonDown(0)) 
                    player.HandlePlayerTurn();
                
            }
            else
            {
                turn = "Dark";
                aiPlayer.StartCoroutine(aiPlayer.GetAiMove());
                isAiTurn = false;
            }
        }
    }

    internal void LastMove(Vector3 start, Vector3 end, GameObject player)
    {
        lastMove = new ()
        {
            player = player.ToString(),
            from = new []{(int)start.x, (int)start.z},
            to = new [] {(int)end.x, (int)end.z} 
        };
        
    } 

}
