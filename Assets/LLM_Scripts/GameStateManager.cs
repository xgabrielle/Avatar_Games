using System;
using UnityEngine;
using Newtonsoft.Json;
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
    public static GameStateManager instance;
    CheckersGameState gameState = new ();
    private CheckerGame _checkerGame;

    private void Start()
    {
        _checkerGame = GetComponent<CheckerGame>();
        instance = this;
    }

    public string GetBoardStateAsJSON ()
    {
        int[,] board = new int[8,8];
        for (int x = 0; x < 8; x++)
        {
            for (int z = 0; z < 8; z++)
            {
                GameObject pawn = MarkersGenerator.instance.MarkerPos()[x, z];
                if (pawn != null)
                {
                    if (pawn.CompareTag("WhiteMarker")) board[7 - z, x] = 1;
                    else if (pawn.CompareTag("DarkMarker")) board[7 - z, x] = 2;
                    // add king later
                }
                else board [7 - z, x] = 0;
            }
            
        }
        
        gameState = new ()
        {
            lastMove = _checkerGame.lastMove,
            turn = _checkerGame.turn,
            board = board
        };
        
        string json = JsonConvert.SerializeObject(gameState, Formatting.Indented);
        Debug.Log("JSON: "+json);
        return json;
    }
    
}
