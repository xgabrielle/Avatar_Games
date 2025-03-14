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
            foreach (var move in MarkerMovement.PossibleMoves(pawn.transform.position)) 
            {
                _checkerGame.targetPosition = move;
                _checkerGame.currentMarkerPos = pawn.transform.position;
                _checkerGame.currentMarker = pawn;
            }
            if (_markerMovement.GetSurroundings(_checkerGame.currentMarkerPos,_checkerGame.currentMarker))
            {
                if (_markerMovement.FreeJumpSpace(_checkerGame.currentMarkerPos, _markerMovement.colEnemyPos))
                {   
                    _markerMovement.Jump(_checkerGame.currentMarker);
                }

                else _markerMovement.GetMarkerMove(_checkerGame.currentMarker);
            }
            else _markerMovement.GetMarkerMove(_checkerGame.currentMarker);

            break;

            /*foreach (Vector3 move in MarkerMovement.PossibleMoves(pawn.transform.position))
            {
                Collider[] col = Physics.OverlapSphere(move, 0.1f);
                if (col.Length == 0 && move.x >= 0 && move.x < 8 && move.z >= 0 && move.z < 8)
                {
                    /*Debug.Log("pawnPos: " +pawn.transform.position);
                    Debug.Log("pawnTargetPos: "+ move);#1#
                    _checkerGame.markerPos = pawn.transform.position;
                    _checkerGame.targetPosition = move;
                    _checkerGame.marker = pawn;
                    _markerMovement.GetMarkerMove(_checkerGame.marker);
                    return;
                }
            }*/
        }

    }
    
}
