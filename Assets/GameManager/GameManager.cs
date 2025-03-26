using System;
using UnityEngine;

public enum GameMode
{
    AI,
    LocalPlayer
}
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private GameMode currentGameMode;
    private void Start()
    {
        Instance = this;
    }

    internal string SetGame()
    { 
        string chatMode = null;
        if (GameMode.LocalPlayer == currentGameMode)
        {
            Debug.Log("Game mode = LocalPlayer");
            chatMode = "You are commenting on the on going game between the players.";
            // Set host or join
        }
        else if (GameMode.AI == currentGameMode)
        {
            Debug.Log("Game mode = AI");
            chatMode = "The game just made a move for you, you are the dark pawns. React as if you made this move and explain why you did it. You get happy if you win and sad if you lose.";
        }

        return chatMode;
    }
    
    
}
