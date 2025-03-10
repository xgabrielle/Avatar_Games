using System;
using Unity.Mathematics;
using UnityEngine;

public class MarkerMovement : MonoBehaviour
{
    private CheckerGame _checkerGame;
    private AvailableMoveChecker _availableMoveChecker;

    private void Start()
    {
        _checkerGame = GetComponent<CheckerGame>();
        _availableMoveChecker = GetComponent<AvailableMoveChecker>();
    }

    internal void DarkMarkerMove()
    {
        if (DiagonalMove(_checkerGame.markerPos, _checkerGame.targetPosition))
        {
            if (_checkerGame.targetPosition.z < _checkerGame.markerPos.z)
            { 
                NewMarkerPos(_checkerGame.targetPosition);
            }
        } else
            Debug.Log("Not a possible move");
        
    }
    
    internal void WhiteMarkerMove()
    {
        if (DiagonalMove(_checkerGame.markerPos, _checkerGame.targetPosition))
        {
            if (_checkerGame.targetPosition.z > _checkerGame.markerPos.z)
            { 
                NewMarkerPos(_checkerGame.targetPosition);
            }
            
        } else
            Debug.Log("Not a possible move");
    }
    
    internal void NewMarkerPos(Vector3 newMarkerPos)
    {
        _checkerGame.markerPos = newMarkerPos;
        _availableMoveChecker.triggerbox.transform.position = newMarkerPos + new Vector3(0,0.6f,1);
        _checkerGame.marker.transform.position = _checkerGame.markerPos + new Vector3(0,0.6f,0);
    }

    internal static bool DiagonalMove(Vector3 startPos, Vector3 targetPos)
    {
        float newPosX = Mathf.Abs(targetPos.x - startPos.x);
        float newPosZ = Mathf.Abs(targetPos.z - startPos.z);

        return Mathf.Approximately(newPosX, newPosZ);
        /*if (Mathf.Approximately(newPosX, 1) && Mathf.Approximately(newPosZ, 1))
            return Mathf.Approximately(newPosX, newPosZ);
        return false;*/

    }
}
