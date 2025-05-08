using System;
using System.Collections;
using NUnit.Framework.Internal;
using Unity.VisualScripting;
using UnityEngine;


public class CheckerGame : MonoBehaviour
{
    private AIPlayer aiPlayer;
    private Player player;
    internal bool isGameOver;
    private bool waitForAI;
    [SerializeField] private GameObject white;
    internal Coroutine aiCoroutine;

    private void Start()
    {
        if (!GameManager.Instance.startGame)
        {
            Debug.LogWarning("Game not set up from MenuScene. Using fallback setup.");
            GameManager.Instance.SetGame();
        }
        
        BoardGenerator.instance.BuildBoard();
        MarkersGenerator.instance.StartField();
        player = GetComponent<Player>();
        aiPlayer = GetComponent<AIPlayer>();
        isGameOver = false;
    }
    
  
    private void Update()
    {
        if (!GameManager.Instance.startGame || isGameOver) return;

        switch (TurnManager.instance.currentPlayer)
        {
            case PlayerTurn.White:
                Player1();
                break;
            case PlayerTurn.Dark:
                Player2();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        TurnManager.instance.UIPlayerTurn();
    }

    void Player1()
    {
        if (!Input.GetMouseButtonDown(0)) return;
        
        player.HandlePlayerTurn(white);
        
    }

    void Player2()
    {
        if (GameMode.AI == GameManager.Instance.currentGameMode)
        {
            if (aiCoroutine == null)
                aiCoroutine = aiPlayer.StartCoroutine(aiPlayer.GetAiMove());
        }
        else
        {
            if (!Input.GetMouseButtonDown(0)) return;
            player.HandlePlayerTurn(GameManager.Instance.playerTwo);
        }
    }

    public bool IsGameOver()
    {
        return isGameOver;
    }

    public bool HasGameOver(string playerTag)
    {
        if (HasPawnsLeft(playerTag)) return true;
        if (!HasValidMoves(playerTag)) return true;

        return false;
    }

    bool HasPawnsLeft(string pawn)
    {
        GameObject[] pawns = GameObject.FindGameObjectsWithTag(pawn);
        return pawns.Length == 0;
    }
    

    bool HasValidMoves(string pawn)
    {
        GameObject[] pawns = GameObject.FindGameObjectsWithTag(pawn);
        foreach (var pawnTag in pawns)
        {
            if (ValidMovesLeft(pawnTag)) return true;
        }

        return false;
    }

    bool ValidMovesLeft(GameObject pawn)
    {
        var pawnPos = pawn.transform.position;

        foreach (var possibleMove in MarkerMovement.PossibleMoves(pawn))
        {
            if (MarkerMovement.Movement.GetMarkerMove(pawn, pawnPos, possibleMove)) return true;

            if (MarkerMovement.Movement.Jump(pawn, pawnPos, possibleMove)) return true;
        }
        
        return false;
    }
}
