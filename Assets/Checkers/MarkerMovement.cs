using System.Collections.Generic;
using Unity.Mathematics.Geometry;
using Unity.VisualScripting;
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
        if (!DiagonalMove(startPos, targetPos) || IsSquareOccupied(targetPos))
        {
            Debug.Log("Not a possible move");
            return false;
        }

        string enemyTag = GetEnemyTag(pawn);

        return enemyTag switch
        {
            "WhiteMarker" => Mathf.Approximately(targetPos.z, startPos.z - 1),
            "DarkMarker" => Mathf.Approximately(targetPos.z,startPos.z + 1),
            _=> false
        };
    }
    
    private static bool DiagonalMove(Vector3 startPos, Vector3 targetPos)
    {
        float newPosX = Mathf.Abs(targetPos.x - startPos.x);
        float newPosZ = Mathf.Abs(targetPos.z - startPos.z);

        return Mathf.Approximately(newPosX, newPosZ);
    }

    internal static List<Vector3> PossibleMoves(Vector3 currentPlayerPos)
    {
        List<Vector3> validDir = new();
        Vector3 position = currentPlayerPos;
        
        Vector3[] directions =  new[]
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
                validDir.Add(dir);
        }
        return validDir;
    }
    internal bool GetSurroundings(GameObject pawn)
    {
        var pawnPos = pawn.transform.position;
        // game over for player/players
        foreach (Vector3 dir in PossibleMoves(pawnPos))
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

    
    
   internal string GetEnemyTag(GameObject pawn)
   {
       if (pawn != null)
       {
           string myTag = pawn.tag;
           string enemyTag = myTag == "DarkMarker" ? "WhiteMarker" : "DarkMarker";
           return enemyTag;
       }

       return null;
   }
   private static bool OutOfBounds(Vector3 pos) => pos.x > 7 || pos.x < 0 || pos.z > 7 || pos.z < 0;

}

