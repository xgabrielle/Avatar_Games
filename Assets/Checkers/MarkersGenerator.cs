using UnityEngine;

public class MarkersGenerator : MonoBehaviour
{
    public static MarkersGenerator instance { get; private set; }
    [SerializeField] internal GameObject whitePieces;
    [SerializeField] internal GameObject redPieces;
    private readonly GameObject[,] _pieces = new GameObject[8,8];
    private void Start()
    {
        instance = this;
    }

    internal void StartField()
    {
        for (var row = 0 ; row < 8; row++)
        {
            for (var column = 0; column < 3; column++)
            {
                if ((row + column) % 2 != 0) continue;
                GameObject piece = Instantiate(whitePieces, new Vector3(row, 0.6f, column), Quaternion.identity);
                _pieces[row, column] = piece;
            }
        }

        for (int row = 0; row < 8; row++)
        {
            for (int column = 5; column < 8; column++)
            {
                if ((row + column) % 2 != 0) continue;
                GameObject piece = Instantiate(redPieces, new Vector3(row, 0.6f, column), Quaternion.identity);
                _pieces[row, column] = piece;
            }
        }
    }

    public void UpdatePawns(GameObject pawn, Vector3 startPos, Vector3 endPos)
    {
        _pieces[(int)startPos.x, (int)startPos.z] = null;
        _pieces[(int)endPos.x, (int)endPos.z] = pawn;
        if (!MarkerMovement.Movement.Jump(pawn, startPos, endPos)) return;
        var destroyPawn = MarkerMovement.Movement.DestroyedPawn();
        if (destroyPawn != null)
        {
            _pieces[(int)destroyPawn.transform.position.x, (int)destroyPawn.transform.position.z] = null;
        }
    }
    internal GameObject[,] MarkerPos()
    {
        return _pieces;
    }
}
