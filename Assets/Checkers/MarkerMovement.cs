using System.Collections.Generic;
using UnityEngine;

public class MarkerMovement : MonoBehaviour
{
    private CheckerGame _checkerGame;
    internal Vector3 colEnemyPos;
    //private King _king;
    private void Start()
    {
        _checkerGame = GetComponent<CheckerGame>();
        //_king = FindObjectOfType<King>();
    }

    internal bool GetMarkerMove(GameObject pawn, Vector3 startPos, Vector3 targetPos)
    {
        if (DiagonalMove(startPos, targetPos))
        {
            /*if (_king.isKing)
                pawn.transform.position = targetPos;*/

            if (_checkerGame.GetEnemyTag(pawn) == "WhiteMarker")
            {
                if (Mathf.Approximately(targetPos.z, startPos.z - 1) && IsSquareOccupied(targetPos))
                {
                    return true;
                }
            }
            else if (_checkerGame.GetEnemyTag(pawn) == "DarkMarker")
            {
                if (Mathf.Approximately(targetPos.z, startPos.z + 1) && IsSquareOccupied(targetPos))
                {
                    return true;
                }
            }
        }
        else Debug.Log("Not a possible move");

        return false;
    }
    
    private static bool DiagonalMove(Vector3 startPos, Vector3 targetPos)
    {
        float newPosX = Mathf.Abs(targetPos.x - startPos.x);
        float newPosZ = Mathf.Abs(targetPos.z - startPos.z);

        return Mathf.Approximately(newPosX, newPosZ);
    }

    internal static List<Vector3> PossibleMoves(Vector3 currentPlayerPos)
    {
        List<Vector3> vaildDir = new();
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
        foreach (var dir in directions)
        {
            if (!OutofBounds(dir))
                vaildDir.Add(dir);
        }
        return vaildDir;
    }
    internal bool GetSurroundings(Vector3 marker, GameObject pawn)
    {
        foreach (Vector3 dir in PossibleMoves(marker))
        {
            Collider[] hitColliders = Physics.OverlapSphere(dir, 0.1f);
            foreach (Collider colliders in hitColliders)
            {
                colEnemyPos = colliders.transform.position;
                if (colliders.CompareTag(_checkerGame.GetEnemyTag(pawn)))
                {
                    return true;
                }
            }
        }
        return false;
    }

   internal void Jump(GameObject pawn, Vector3 startPos, Vector3 targetPos)
   {
       Vector3 middlePos = ((startPos + targetPos) / 2);
       Collider[] middleColliders = Physics.OverlapSphere(middlePos, 0.1f);
       
       foreach (Collider midCol in middleColliders)
       {
           if (midCol.CompareTag(_checkerGame.GetEnemyTag(pawn)))
           { 
               GameObject jumpedMarker = midCol.gameObject;
               
               if (DiagonalMove(startPos, targetPos))
               {
                   if (_checkerGame.GetEnemyTag(pawn) == "WhiteMarker")
                   {
                       if (Mathf.Approximately(targetPos.z, startPos.z - 2))
                       { 
                           pawn.transform.position = targetPos;
                           Destroy(jumpedMarker);
                       }
                   }
                   else
                   {
                       if (Mathf.Approximately(targetPos.z, startPos.z + 2))
                       { 
                           pawn.transform.position = targetPos;
                           Destroy(jumpedMarker);
                       }
                   }
                   
               } else
                   Debug.Log("Not a possible move");
               break;
           }
       }
    }

   internal bool IsSquareOccupied(Vector3 targetPos)
   {
       Collider[] playerCollider = Physics.OverlapSphere(targetPos, 0.2f);
       if (playerCollider.Length > 1)
       {
           return false;
       }
       
       return true;
   }
   
   internal (bool, Vector3) FreeJumpSpace(Vector3 markerPos, Vector3 enemyPos)
   {
       Vector3 landingPos = (enemyPos - markerPos) + enemyPos;
       Collider[] col = Physics.OverlapSphere(landingPos, 0.2f);
       if (col.Length > 1 || OutofBounds(enemyPos))
       {
           return (false, landingPos);
       }
       return (true, landingPos);
   }

   internal static bool OutofBounds(Vector3 pos) => pos.x > 7 || pos.x < 0 || pos.z > 7 || pos.z < 0;

}

