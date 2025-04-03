using System;
using NUnit.Framework.Internal;
using Unity.VisualScripting;
using UnityEngine;


public class CheckerGame : MonoBehaviour
{
    private AIPlayer aiPlayer;
    private Player player;
    internal bool isAiTurn;
    internal bool isGameOver;
    internal string turn;

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
        {
            if (!isAiTurn)
            {
                turn = "White pawn";
                if (Input.GetMouseButtonDown(0)) 
                    player.HandlePlayerTurn();
            }
            else
            {
                turn = "Dark pawn";
                aiPlayer.StartCoroutine(aiPlayer.GetAiMove());
                isAiTurn = false;
            }
        }
        
    }

    public bool IsGameOver()
    {
        return isGameOver;
    }

    public bool HasGameOver(string player)
    {
        if (HasPawnsLeft(player)) return true;
        if (!HasValidMoves(player)) return true;

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

        foreach (var possibleMove in MarkerMovement.PossibleMoves(pawnPos))
        {
            if (MarkerMovement.Movement.GetMarkerMove(pawn, pawnPos, possibleMove)) return true;

            if (MarkerMovement.Movement.Jump(pawn, pawnPos, possibleMove)) return true;
        }
        
        return false;
    }
}
