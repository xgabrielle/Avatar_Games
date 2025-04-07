using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.VFX;

public class AIPlayer : MonoBehaviour
{
    private CheckerGame _checkerGame;

    private void Start()
    {
        _checkerGame = GetComponent<CheckerGame>();
    }

    internal IEnumerator GetAiMove()
    {
        yield return new WaitForSeconds(3.0f);
        EasyRandomMove();
        _checkerGame.isAiTurn = false;

    }
    void EasyRandomMove()
    {
        GameObject[] darkPawns = GameObject.FindGameObjectsWithTag("DarkMarker");
        foreach (GameObject pawn in darkPawns)
        {
            Vector3 pawnPos = pawn.transform.position;
            
            foreach (var move in MarkerMovement.PossibleMoves(pawnPos))
            {
                var moveResult = MarkerMovement.Movement.ValidateMove(pawn, pawnPos, move);
                if (moveResult.IsValid)
                {
                    pawn.transform.position = moveResult.LandingPos;
                    if (moveResult.CapturedPawn != null)
                        Destroy(moveResult.CapturedPawn);
                    MarkersGenerator.instance.UpdatePawns(pawn, pawnPos, moveResult.LandingPos );
                    GameStateManager.instance.LastMove(pawn, pawnPos, moveResult.LandingPos);
                    if (_checkerGame.HasGameOver(_checkerGame.isAiTurn ? "WhiteMarker" : "DarkMarker"))
                    { 
                        _checkerGame.isGameOver = true;
                        Debug.Log("Game over, AI lost (No valid moves)");
                        ChatManager.Instance.SendMessageToAI("");
                    }
                    return;
                }
            }
        }
    }
}
