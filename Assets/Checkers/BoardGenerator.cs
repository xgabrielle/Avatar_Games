using Unity.Mathematics;
using UnityEngine;

public class BoardGenerator : MonoBehaviour
{
    public static BoardGenerator instance { get; private set; }
    [SerializeField] private GameObject fieldPrefab;
    [SerializeField] private Material blackField;
    [SerializeField] private Material redField;
    [SerializeField] private int boardSize;
    private GameObject squareSpace;
    
    void Start()
    {
        if (instance ==null ) instance = this;
        else Destroy(instance);
    }

    internal void BuildBoard()
    {
        for (int row = 0; row < boardSize; row++)
        {
            for (int column = 0; column < boardSize; column++)
            {
                squareSpace = Instantiate(fieldPrefab, new Vector3(row, 0, column), quaternion.identity);
                squareSpace.transform.parent = transform;

                Renderer chooseColor = squareSpace.GetComponent<Renderer>();
                if ((row + column) % 2 == 0)
                    chooseColor.material = blackField;
                else 
                    chooseColor.material = redField;
            }
        }
    }
}
