using System;
using System.Collections;
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
        GameObject[] darkPawns = GameObject.FindGameObjectsWithTag("DarkMarker");
        foreach (GameObject pawn in darkPawns)
        {
            Vector3 pawnPos = pawn.transform.position;
            foreach (var move in MarkerMovement.PossibleMoves(pawn.transform.position)) 
            {
                if (_markerMovement.GetSurroundings(pawnPos,pawn))
                {
                    if (_markerMovement.FreeJumpSpace(pawnPos, move).Item1) 
                        _markerMovement.Jump(pawn, pawnPos, _markerMovement.FreeJumpSpace(pawnPos, _markerMovement.colEnemyPos).Item2);
                    else if (_markerMovement.GetMarkerMove(pawn, pawnPos, move))
                    {
                        pawn.transform.position = move;
                    }
                }
                else if (_markerMovement.GetMarkerMove(pawn, pawnPos, move))
                {
                    pawn.transform.position = move;
                }
            }

            break;
        }

    }
    
}
