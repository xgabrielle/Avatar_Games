using System;
using Unity.VisualScripting;
using UnityEngine;


public class CheckerGame : MonoBehaviour
{
    private AIPlayer aiPlayer;
    private Player player;
    
    internal bool isAiTurn;
    internal bool isGameOver;
    

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
            // boardUpdate -> send update to OpenAI
        {
            if (!isAiTurn)
            {
                if (Input.GetMouseButtonDown(0)) 
                    player.HandlePlayerTurn();
                Debug.Log("Ai turn");
            }
            else
            {
                aiPlayer.StartCoroutine(aiPlayer.GetAiMove());
                Debug.Log("Player Turn");
                isAiTurn = false;
            }
            
        }
    }
    

}
