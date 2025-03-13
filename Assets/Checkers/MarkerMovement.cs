using System;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class MarkerMovement : MonoBehaviour
{
    private CheckerGame _checkerGame;
    internal Vector3 colEnemyPos;
    private King _king;
    private void Start()
    {
        _checkerGame = GetComponent<CheckerGame>();
        _king = FindObjectOfType<King>();
    }

    internal void GetMarkerMove(GameObject pawn)
    {
        if (DiagonalMove(_checkerGame.markerPos, _checkerGame.targetPosition))
        {
            if (_king.isKing)
                NewMarkerPos(_checkerGame.targetPosition);
            
            else if (_checkerGame.GetEnemyTag(pawn) == "WhiteMarker")
            {
                
                if (Mathf.Approximately(_checkerGame.targetPosition.z, _checkerGame.markerPos.z - 1)) 
                { 
                    NewMarkerPos(_checkerGame.targetPosition);
                }
             
            }
            else if (_checkerGame.GetEnemyTag(pawn) == "DarkMarker")
            {
                if (Mathf.Approximately(_checkerGame.targetPosition.z, _checkerGame.markerPos.z + 1))
                { 
                    Debug.Log("PlayerTargetPos: "+ _checkerGame.targetPosition);
                    NewMarkerPos(_checkerGame.targetPosition);
                }
            }
            
        }
        else Debug.Log("Not a possible move");
    }
    
    private void NewMarkerPos(Vector3 newMarkerPos)
    {
        _checkerGame.markerPos = newMarkerPos;
        _checkerGame.marker.transform.position = _checkerGame.markerPos;
    }

    private static bool DiagonalMove(Vector3 startPos, Vector3 targetPos)
    {
        float newPosX = Mathf.Abs(targetPos.x - startPos.x);
        float newPosZ = Mathf.Abs(targetPos.z - startPos.z);

        return Mathf.Approximately(newPosX, newPosZ);
    }

    internal static Vector3[] PossibleMoves(Vector3 currentPlayerPos)
    {
        Vector3 position = currentPlayerPos;
        Vector3[] directions = new[]
        {
            // up
            new Vector3(position.x + 1, position.y, position.z + 1), //R-U
            new Vector3(position.x - 1, position.y, position.z + 1), // L-U
            // down
            new Vector3(position.x + 1, position.y, position.z - 1), // R-D
            new Vector3(position.x - 1, position.y, position.z - 1)  // L-D
        };
        return directions;
    }
    internal bool GetSurroundings(Vector3 marker, GameObject pawn)
    {
        foreach (Vector3 dir in PossibleMoves(marker))
        {
            Collider[] hitColliders = Physics.OverlapSphere(dir, 0.1f);
            foreach (Collider colliders in hitColliders)
            {
                colEnemyPos = colliders.transform.position;
                Debug.Log(colliders.tag);
                if (colliders.CompareTag(_checkerGame.GetEnemyTag(pawn)))
                {
                    return true;
                }
            }
        }
        return false;
    }

   internal void Jump(GameObject pawn)
   {
       Vector3 middlePos = ((_checkerGame.markerPos + _checkerGame.targetPosition) / 2) + Vector3.up*0.5f;
       Collider[] middleColliders = Physics.OverlapSphere(middlePos, 0.1f);
       

       foreach (Collider midCol in middleColliders)
       {
           if (midCol.CompareTag(_checkerGame.GetEnemyTag(pawn)))
           { 
               GameObject jumpedMarker = midCol.gameObject;
               
               if (DiagonalMove(_checkerGame.markerPos, _checkerGame.targetPosition))
               {
                   if (_checkerGame.GetEnemyTag(pawn) == "WhiteMarker")
                   {
                       if (Mathf.Approximately(_checkerGame.targetPosition.z, _checkerGame.markerPos.z - 2))
                       { 
                           NewMarkerPos(_checkerGame.targetPosition);
                           Destroy(jumpedMarker);
                       }
                   }
                   else
                   {
                       if (Mathf.Approximately(_checkerGame.targetPosition.z, _checkerGame.markerPos.z + 2))
                       { 
                           NewMarkerPos(_checkerGame.targetPosition);
                           Destroy(jumpedMarker);
                       }
                   }
                   
               } else
                   Debug.Log("Not a possible move");
               break;
           }
       }
       
        
    }
   
   internal bool FreeJumpSpace(Vector3 markerPos, Vector3 enemyPos)
   {
       Vector3 landingPos = (enemyPos - markerPos) + enemyPos;
       Collider[] col = Physics.OverlapSphere(landingPos, 0.2f);
       if (col.Length > 1 || landingPos.x > 7 || landingPos.x < 0 || landingPos.z > 7 || landingPos.z < 0)
       {
           return false;
       }
       return true;
   }
   
   
   
}
