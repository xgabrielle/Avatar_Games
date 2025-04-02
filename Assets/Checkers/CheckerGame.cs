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
    
    //internal CheckersMove lastMove = new();

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
                turn = "White pawn";
                if (Input.GetMouseButtonDown(0)) 
                    player.HandlePlayerTurn();
                
            }
            else
            {
                turn = "Dark pawn";
                aiPlayer.StartCoroutine(aiPlayer.GetAiMove());
                
                isAiTurn = false;
            }
        }
    }

    public bool IsGameOver()
    {
        return isGameOver;
    }
}
