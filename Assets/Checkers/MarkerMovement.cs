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

    public class MoveResult
    {
        public bool IsValid { get; set; }
        public Vector3 LandingPos { get; set; }
        public GameObject CapturedPawn { get; set; }

        public MoveResult(bool isValid = false, Vector3 landingPos =default, GameObject capturedPawn=null)
        {
            IsValid = isValid;
            LandingPos = landingPos;
            CapturedPawn = capturedPawn;
        }
    }
    
    internal bool GetMarkerMove(GameObject pawn, Vector3 startPos, Vector3 targetPos)
    {
        if (!DiagonalMove(startPos, targetPos) || IsSquareOccupied(targetPos)) return false;

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

    internal static List<Vector3> PossibleMoves(GameObject currentPlayer)
    {
        List<Vector3> validDir = new();
        Vector3 dir1 = default, dir2 = default;
        Vector3 position = currentPlayer.transform.position;
        validDir.Clear();
        
        if (currentPlayer.CompareTag("WhiteMarker"))
        {
            // up
            dir1 = new Vector3(position.x + 1, position.y, position.z + 1); //R-U
            dir2 = new Vector3(position.x - 1, position.y, position.z + 1); // L-U
        }
        if (currentPlayer.CompareTag("DarkMarker"))
        {
            // down
            dir1 = new Vector3(position.x + 1, position.y, position.z - 1); // R-D
            dir2 = new Vector3(position.x - 1, position.y, position.z - 1); // L-D
        }
        
        if (!OutOfBounds(dir1)) validDir.Add(dir1);
        if (!OutOfBounds(dir2)) validDir.Add(dir2);
        return validDir;
    }
    private bool GetSurroundings(GameObject pawn)
    {
        var pawnPos = pawn.transform.position;
        foreach (Vector3 dir in PossibleMoves(pawn))
        {
            Collider[] hitColliders = new Collider[1];
            var size = Physics.OverlapSphereNonAlloc(dir, 0.1f, hitColliders);
            
            if (size == 0) continue;

            if (hitColliders[0].CompareTag(GetEnemyTag(pawn)))
                return true;
        }
        
        return false;
    }

    internal MoveResult ValidateMove(GameObject pawn, Vector3 start, Vector3 end)
    {
        if (GetSurroundings(pawn))
        {
            Vector3 midPos = (start + end) / 2;
            Collider[] middleColliders = Physics.OverlapSphere(midPos, 0.2f);

            foreach (var midCol in middleColliders)
            {
                if (midCol.CompareTag(GetEnemyTag(pawn)))
                {
                    if (FreeJumpSpace(start, end).Item1)
                    {
                        var jumpResult = FreeJumpSpace(start, end).Item2;
                        if (Jump(pawn, start, jumpResult)) return new MoveResult(true, jumpResult, midCol.gameObject);
                    }
                }
            } 
        }
        else if (GetMarkerMove(pawn, start, end))
                return new MoveResult(true, end);
        return new MoveResult();
    }

   internal bool Jump(GameObject pawn, Vector3 startPos, Vector3 targetPos)
   {
       if (!DiagonalMove(startPos, targetPos) || IsSquareOccupied(targetPos)) return false;
       
       var middlePos = (startPos + targetPos) * 0.5f;
       var enemyTag = GetEnemyTag(pawn);
      
       Collider[] middleColliders = new Collider[1];
       var size = Physics.OverlapSphereNonAlloc(middlePos, 0.1f, middleColliders);
       
       if (size == 0) return false;

       var midCol = middleColliders[0];
       
       if (!midCol.CompareTag(enemyTag)) return false;
       var jumpedMarker = midCol.gameObject;
       
       bool validJump = enemyTag == "WhiteMarker"
           ? Mathf.Approximately(targetPos.z, startPos.z - 2)
           : Mathf.Approximately(targetPos.z, startPos.z + 2);
       
       if (!validJump) return false;
          
       pawn.transform.position = targetPos;
       pawnDestroyed = jumpedMarker;
       Destroy(jumpedMarker);
       
       return true;
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
   
   private static (bool, Vector3) FreeJumpSpace(Vector3 markerPos, Vector3 movePos)
   {
       Vector3 landingPos = (movePos - markerPos) + movePos;
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

