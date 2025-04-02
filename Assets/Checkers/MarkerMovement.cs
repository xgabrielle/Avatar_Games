using System.Collections.Generic;
using UnityEngine;

public class MarkerMovement : MonoBehaviour
{
    public static MarkerMovement Movement { get; private set; }
    private GameObject pawnDestroyed;
    private void Start()
    {
        Movement = this;
    }

    internal bool GetMarkerMove(GameObject pawn, Vector3 startPos, Vector3 targetPos)
    {
        if (DiagonalMove(startPos, targetPos))
        {
            if (GetEnemyTag(pawn) == "WhiteMarker")
            {
                if (Mathf.Approximately(targetPos.z, startPos.z - 1) && !IsSquareOccupied(targetPos))
                {
                    return true;
                }
            }
            else if (GetEnemyTag(pawn) == "DarkMarker")
            {
                if (Mathf.Approximately(targetPos.z, startPos.z + 1) && !IsSquareOccupied(targetPos))
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
            if (!OutOfBounds(dir))
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
                if (colliders.CompareTag(GetEnemyTag(pawn)))
                {
                    return true;
                }
            }
        }
        return false;
    }

   internal bool Jump(GameObject pawn, Vector3 startPos, Vector3 targetPos)
   {
       Vector3 middlePos = ((startPos + targetPos) / 2);
       Collider[] middleColliders = Physics.OverlapSphere(middlePos, 0.1f);
       
       foreach (Collider midCol in middleColliders)
       {
           if (midCol.CompareTag(GetEnemyTag(pawn)))
           { 
               GameObject jumpedMarker = midCol.gameObject;
               
               if (DiagonalMove(startPos, targetPos))
               {
                   if (GetEnemyTag(pawn) == "WhiteMarker")
                   {
                       if (Mathf.Approximately(targetPos.z, startPos.z - 2))
                       { 
                           pawn.transform.position = targetPos;
                           pawnDestroyed = jumpedMarker;
                           Destroy(jumpedMarker);
                           return true;
                       }
                   }
                   else
                   {
                       if (Mathf.Approximately(targetPos.z, startPos.z + 2))
                       { 
                           pawn.transform.position = targetPos;
                           pawnDestroyed = jumpedMarker; 
                           Destroy(jumpedMarker);
                           return true;
                       }
                   }
                   
               } else
                   Debug.Log("Not a possible move");
               break;
           }
       }

       return false;

   }

   public GameObject DestroyedPawn()
   {
       return pawnDestroyed;
   } 
   bool IsSquareOccupied(Vector3 targetPos)
   {
       Collider[] playerCollider = Physics.OverlapSphere(targetPos, 0.2f);
       if (playerCollider.Length > 1)
       {
           return true;
       }
       
       return false;
   }
   
   internal (bool, Vector3) FreeJumpSpace(Vector3 markerPos, Vector3 enemyPos)
   {
       Vector3 landingPos = (enemyPos - markerPos) + enemyPos;
       Collider[] col = Physics.OverlapSphere(landingPos, 0.2f);
       if (col.Length > 1 || OutOfBounds(landingPos))
       {
           return (false, landingPos);
       }
       return (true, landingPos);
   }

    
    
   string GetEnemyTag(GameObject pawn)
   {
       string myTag = pawn.tag;
       string enemyTag = myTag == "DarkMarker" ? "WhiteMarker" : "DarkMarker";
       return enemyTag;
   }
   private static bool OutOfBounds(Vector3 pos) => pos.x > 7 || pos.x < 0 || pos.z > 7 || pos.z < 0;

}

