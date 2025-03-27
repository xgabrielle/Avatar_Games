using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
[System.Serializable]
public class CheckersGameState
{
    public int[,] board;
    public string turn;
    public List<CheckersMove> lastMove;
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
    private CheckerGame _checkerGame;

    private void Start()
    {
        _checkerGame = GetComponent<CheckerGame>();
        instance = this;
    }

    public string GetBoardStateAsJSON (List<CheckersMove> updateLastMove, GameObject[,] pawns, string player)
    {
        int[,] board = new int[8,8];
        for (int x = 0; x < 8; x++)
        {
            for (int z = 0; z < 8; z++)
            {
                GameObject pawn = pawns[x, z];
                if (pawn != null)
                {
                    if (pawn.CompareTag("WhiteMarker")) board[7 - z, x] = 1;
                    else if (pawn.CompareTag("DarkMarker")) board[7 - z, x] = 2;
                    // add king later
                }
                else board [7 - z, x] = 0;
            }
            
        }
        // WhitePiece(Clone) (UnityEngine.GameObject)
        gameState = new ()
        {
            lastMove = updateLastMove,
            turn = (player == "White" ? "Dark" : "White"),
            board = board
        };
        Debug.Log("turn: " + gameState.turn);
        string json = JsonConvert.SerializeObject(gameState, Formatting.Indented);
        Debug.Log("JSON: "+json);
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
    }

    public List<CheckersMove> GetMoveHistory()
    {
        return moveHistory;
    }

    public string GetPlayer()
    {
        return gameState.turn;
    }
    
}
