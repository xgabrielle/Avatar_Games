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
                if (_markerMovement.FreeJumpSpace(_checkerGame.currentMarkerPos, _markerMovement.colEnemyPos).Item1) 
                    _markerMovement.Jump(_checkerGame.currentMarker, _markerMovement.FreeJumpSpace(_checkerGame.currentMarkerPos, _markerMovement.colEnemyPos).Item2);

                else _markerMovement.GetMarkerMove(_checkerGame.currentMarker, _checkerGame.currentMarkerPos, _checkerGame.targetPosition);
            }
            else _markerMovement.GetMarkerMove(_checkerGame.currentMarker, _checkerGame.currentMarkerPos, _checkerGame.targetPosition);

            break;
        }

    }
    
}
