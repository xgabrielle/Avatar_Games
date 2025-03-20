using UnityEngine;

public class MarkersGenerator : MonoBehaviour
{
    public static MarkersGenerator instanse { get; set; }
    [SerializeField] internal GameObject whitePieces;
    [SerializeField] internal GameObject redPieces;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        instanse = this;
    }

    internal void StartField()
    {
        for (int row = 0 ; row < 8; row++)
        {
            for (int column = 0; column < 3; column++)
            {
                if ((row + column) % 2 == 0)
                {
                    Instantiate(whitePieces, new Vector3(row, 0.6f, column), Quaternion.identity);
                }
                
            }
            
        }

        for (int row = 0; row < 8; row++)
        {
            for (int column = 5; column < 8; column++)
            {
                if ((row + column) % 2 == 0)
                { 
                    Instantiate(redPieces, new Vector3(row, 0.6f, column), Quaternion.identity);
                }
            }
        }
        
    }
}
