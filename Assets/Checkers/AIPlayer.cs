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
        yield return new WaitForSeconds(1.0f);
        EasyRandomMove();
        _checkerGame.isAiTurn = false;

    }
    void EasyRandomMove()
    {
        GameObject[] darkPawns = GameObject.FindGameObjectsWithTag("DarkMarker");
        foreach (GameObject pawn in darkPawns)
        {
            /*if (_markerMovement.GetSurroundings(pawn.transform.position, pawn))
            {
                if (_markerMovement.FreeJumpSpace(pawn.transform.position, _markerMovement.colEnemyPos)) 
                    _markerMovement.Jump(pawn);
                        
                else _markerMovement.GetMarkerMove(pawn);
            }
            else _markerMovement.GetMarkerMove(pawn);*/
            
            foreach (Vector3 move in MarkerMovement.PossibleMoves(pawn.transform.position))
            {
                Collider[] col = Physics.OverlapSphere(move, 0.1f);
                if (col.Length == 0 && move.x >= 0 && move.x < 8 && move.z >= 0 && move.z < 8)
                {
                    Debug.Log("pawnPos: " +pawn.transform.position);
                    Debug.Log("pawnTargetPos: "+ move);
                    _checkerGame.markerPos = pawn.transform.position;
                    _checkerGame.targetPosition = move;
                    _markerMovement.GetMarkerMove(pawn);
                    return;
                }
            }
        }

    }
    
}
