using System;
using NUnit.Framework.Internal;
using Unity.VisualScripting;
using UnityEngine;


public class CheckerGame : MonoBehaviour
{
    private GameObject[,] board;
    private AIPlayer aiPlayer;
    private Player player;
    internal bool isAiTurn;
    internal bool isGameOver;
    internal string turn;

    internal CheckersMove lastMove = new();

    private void Start()
    {
        board =  MarkersGenerator.instance.MarkerPos();
        player = GetComponent<Player>();
        aiPlayer = GetComponent<AIPlayer>();
        
        isAiTurn = false;
        isGameOver = false;

    }
    
    private void Update()
    {
        if (!isGameOver)
        {
            // boardUpdate -> send update to OpenAI
            if (!isAiTurn)
            {
                if (Input.GetMouseButtonDown(0)) 
                    player.HandlePlayerTurn();
                Debug.Log("Ai turn");
                turn = "Dark";
                
            }
            else
            {
                aiPlayer.StartCoroutine(aiPlayer.GetAiMove());
                Debug.Log("Player Turn");
                isAiTurn = false;
                turn = "White";
            }
            
            GameStateManager.instance.GetBoardStateAsJSON(board, turn, lastMove);
            
        }
    }

    internal void LastMove(Vector3 start, Vector3 end, GameObject player)
    {
        lastMove = new ()
        {
            player = player.ToString(),
            from = new []{(int)start.x, (int)start.z}, // currentPos
            to = new [] {(int)end.x, (int)end.z}    // targetPos
        };
        
    } 

}
