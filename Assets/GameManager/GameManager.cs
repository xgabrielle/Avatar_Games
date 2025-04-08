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
    internal GameMode currentGameMode;
    private GameMode playerType2;
    internal bool startGame;
    [SerializeField] internal GameObject playerTwo;
    private AIPlayer ai;
    private Player local;
    private void Start()
    {
        Instance = this;
        ai = playerTwo.GetComponent<AIPlayer>();
        local = playerTwo.GetComponent<Player>();
    }

    internal string SetGame()
    { 
        
        string chatMode = null;
        if (GameMode.LocalPlayer == currentGameMode)
        {
            chatMode = "You are commenting on the on going game between the players.";
            playerType2 = GameMode.LocalPlayer;
            ai.enabled = false;
            local.enabled = true;
        }
        else if (GameMode.AI == currentGameMode)
        {
            chatMode = "You are the dark markers and go second. Take in the move it just made and pretend like you made the move. You get happy if you win and sad if you lose.";
            playerType2 = GameMode.AI;
            ai.enabled = true;
            local.enabled = false;
        }

        startGame = true;
        return chatMode;
    }
    
    
}
