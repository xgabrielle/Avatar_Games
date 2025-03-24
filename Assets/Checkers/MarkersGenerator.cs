using UnityEngine;

public class MarkersGenerator : MonoBehaviour
{
    public static MarkersGenerator instance { get; set; }
    [SerializeField] internal GameObject whitePieces;
    [SerializeField] internal GameObject redPieces;
    private GameObject[,] pieces = new GameObject[8,8];
    void Start()
    {
        instance = this;
    }

    internal void StartField()
    {
        for (int row = 0 ; row < 8; row++)
        {
            for (int column = 0; column < 3; column++)
            {
                if ((row + column) % 2 == 0)
                {
                    GameObject piece = Instantiate(whitePieces, new Vector3(row, 0.6f, column), Quaternion.identity);
                    pieces[row, column] = piece;
                }
                
            }
            
        }

        for (int row = 0; row < 8; row++)
        {
            for (int column = 5; column < 8; column++)
            {
                if ((row + column) % 2 == 0)
                { 
                    GameObject piece = Instantiate(redPieces, new Vector3(row, 0.6f, column), Quaternion.identity);
                    pieces[row, column] = piece;
                }
            }
        }
        
    }

    internal GameObject[,] MarkerPos()
    {
        return pieces;
    }
}
