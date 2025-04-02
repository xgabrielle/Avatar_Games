using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Unity.VisualScripting;

[System.Serializable]
public class CheckersGameState
{
    public int[,] board;
    public string turn;
    public CheckersMove lastMove;
}
[System.Serializable]
public class CheckersMove
{
    public string player;
    public int[] from;
    public int[] to;
}

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager instance { get; private set; }
    CheckersGameState gameState = new ();
    private CheckersMove lastMove = new();
    private List<CheckersMove> moveHistory = new();
    
    private void Start()
    {
        instance = this;
    }

    public string GetBoardStateAsJSON (CheckersMove updateLastMove, string player)
    {
        int[,] board = gameState.board;
        if (board == null)
        {
            board = new int[8, 8];
            player = "White";
        }
        
        for (int x = 0; x < 8; x++)
        {
            for (int z = 0; z < 8; z++)
            {
                GameObject pawn = MarkersGenerator.instance.MarkerPos()[x, z];
                if (pawn != null)
                {
                    if (pawn.CompareTag("WhiteMarker")) board[x, z] = 1;
                    else if (pawn.CompareTag("DarkMarker")) board[x, z] = 2;
                }
                else board [x, z] = 0;
            }
        }
        
        gameState = new ()
        {
            lastMove = updateLastMove,
            turn = player,
            board = board,
        };
        string json = JsonConvert.SerializeObject(gameState, Formatting.Indented);
        //Debug.Log("JSON: "+json);
        return json;
    }
    
    internal void LastMove(Vector3 start, Vector3 end, GameObject player)
    {
        lastMove = new ()
        {
            player = player.CompareTag("WhiteMarker") ? "White" : "Dark",
            from = new []{(int)start.x, (int)start.z},
            to = new [] {(int)end.x, (int)end.z} 
        };
        moveHistory.Add(lastMove);
        
        gameState.board[(int)start.x, (int)start.z] = 0;
        gameState.board[(int)end.x, (int)end.z] = player.CompareTag("WhiteMarker") ? 1 : 2; 
        
        gameState.turn = gameState.turn == "White" ? "Dark" : "White";
        GetBoardStateAsJSON(gameState.lastMove, lastMove.player);
    }
    
    public string GetPlayer()
    {
        return gameState.turn;
    }

    public CheckersMove PreviousMove()
    {
        return lastMove;
    }
}
