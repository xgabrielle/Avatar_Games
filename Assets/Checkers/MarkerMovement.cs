using System;
using Unity.Mathematics;
using UnityEngine;

public class MarkerMovement : MonoBehaviour
{
    private CheckerGame _checkerGame;
    private void Start()
    {
        _checkerGame = GetComponent<CheckerGame>();
    }

    internal void DarkMarkerMove()
    {
        if (DiagonalMove(_checkerGame.markerPos, _checkerGame.targetPosition))
        {
            if (Mathf.Approximately(_checkerGame.targetPosition.z, _checkerGame.markerPos.z - 1))
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
            if (Mathf.Approximately(_checkerGame.targetPosition.z, _checkerGame.markerPos.z + 1))
            { 
                NewMarkerPos(_checkerGame.targetPosition);
            }
            
        } else
            Debug.Log("Not a possible move");
    }
    
    internal void NewMarkerPos(Vector3 newMarkerPos)
    {
        _checkerGame.markerPos = newMarkerPos;
        _checkerGame.marker.transform.position = _checkerGame.markerPos + new Vector3(0,0.6f,0);
    }

    internal static bool DiagonalMove(Vector3 startPos, Vector3 targetPos)
    {
        float newPosX = Mathf.Abs(targetPos.x - startPos.x);
        float newPosZ = Mathf.Abs(targetPos.z - startPos.z);

        return Mathf.Approximately(newPosX, newPosZ);
    }
    
    internal bool GetSurroundings(Vector3 marker)
    {
        Vector3 position = marker;
        Vector3[] directions = new[]
        {
            new Vector3(position.x + 1, position.y, position.z+1), //R-U
            new Vector3(position.x - 1, position.y, position.z+1), // L-U
            new Vector3(position.x + 1, position.y, position.z - 1), // R-D
            new Vector3(position.x - 1, position.y, position.z - 1)  // L-D
        };

        foreach (Vector3 dir in directions)
        {
            Collider[] hitColliders = Physics.OverlapSphere(dir, 0.1f);
            foreach (Collider colliders in hitColliders)
            {
                if (colliders.CompareTag(_checkerGame.GetEnemy()))
                { 
                    return true;
                }
            }
        }

        return false;
    }
    

   internal void Jump()
   {
       Vector3 middlePos = ((_checkerGame.markerPos + _checkerGame.targetPosition) / 2) + Vector3.up*0.5f;
       Collider[] middleColliders = Physics.OverlapSphere(middlePos, 0.1f);
       

       foreach (Collider midCol in middleColliders)
       {
           if (midCol.CompareTag(_checkerGame.GetEnemy()))
           { 
               GameObject jumpedMarker = midCol.gameObject;
               
               if (DiagonalMove(_checkerGame.markerPos, _checkerGame.targetPosition))
               {
                   if (_checkerGame.GetEnemy() == "WhiteMarker")
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
}
