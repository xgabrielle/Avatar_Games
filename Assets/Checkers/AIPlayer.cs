using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.VFX;

public class AIPlayer : MonoBehaviour
{
    private CheckerGame _checkerGame;
    private MarkerMovement _markerMovement;

    private void Start()
    {
        _checkerGame = GetComponent<CheckerGame>();
        _markerMovement = GetComponent<MarkerMovement>();
    }

    internal IEnumerator GetAiMove()
    {
        yield return new WaitForSeconds(2.0f);
        EasyRandomMove();
        _checkerGame.isAiTurn = false;

    }
    void EasyRandomMove()
    {
        bool validMove = false;
        GameObject[] darkPawns = GameObject.FindGameObjectsWithTag("DarkMarker");
        foreach (GameObject pawn in darkPawns)
        {
            if (darkPawns.Length < 1)
            {
                _checkerGame.isGameOver = true;
                Debug.Log("Game over, AI lost");
            }
            if (validMove) break;
            
            Vector3 pawnPos = pawn.transform.position;
            foreach (var move in MarkerMovement.PossibleMoves(pawnPos)) 
            {
                if (_markerMovement.FreeJumpSpace(pawnPos, move).Item1)
                {
                    if (_markerMovement.Jump(pawn, pawnPos, _markerMovement.FreeJumpSpace(pawnPos, move).Item2))
                    {
                        pawn.transform.position = _markerMovement.FreeJumpSpace(pawnPos, move).Item2;
                        validMove = true;
                        break;
                    }
                    if (_markerMovement.GetMarkerMove(pawn, pawnPos, move)) // check if square is occupied
                    {
                        pawn.transform.position = move;
                        validMove = true;
                        break;
                    }
                }
                else if (_markerMovement.GetMarkerMove(pawn, pawnPos, move)) // check if square is occupied
                {
                    pawn.transform.position = move;
                    validMove = true;
                    break;
                }
                
            }
        }
        
        if (!validMove)
        {
            _checkerGame.isGameOver = true;
            Debug.Log("Game over, AI lost (No valid moves)");
        }
        

    }
    
}
