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

    internal void SetGame()
    {
        if (GameMode.LocalPlayer == currentGameMode)
        {
            Debug.Log("Game mode = LocalPlayer");
        }
        else if (GameMode.AI == currentGameMode)
        {
            Debug.Log("Game mode = AI");
        }
    }
    
    
}
