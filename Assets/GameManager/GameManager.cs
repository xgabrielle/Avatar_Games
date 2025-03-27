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
            chatMode = "You are the dark markers and go second. Take in the move it just made and pretend like you made the move. You get happy if you win and sad if you lose.";
        }

        return chatMode;
    }
    
    
}
