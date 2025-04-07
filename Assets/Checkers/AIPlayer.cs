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
        //bool validMove = false;
        //GameObject validPawn = null;
        //Vector3 validStartPos = default;
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
                        
                    /*if (MarkerMovement.Movement.FreeJumpSpace(pawnPos, move).Item1)
                        {
                            if (MarkerMovement.Movement.Jump(pawn, pawnPos, MarkerMovement.Movement.FreeJumpSpace(pawnPos, move).Item2))
                            {
                                pawn.transform.position = MarkerMovement.Movement.FreeJumpSpace(pawnPos, move).Item2;
                                validMove = true;
                                break;
                            }
                            if (MarkerMovement.Movement.GetMarkerMove(pawn, pawnPos, move)) // check if square is occupied
                            {
                                pawn.transform.position = move;
                                validMove = true;
                                break;
                            }
                        }
                        else if (MarkerMovement.Movement.GetMarkerMove(pawn, pawnPos, move)) // check if square is occupied
                        {
                            pawn.transform.position = move;
                            validMove = true;
                            break;
                        }*/
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
