using Unity.Mathematics;
using UnityEngine;

public class BoardGenerator : MonoBehaviour
{
    [SerializeField] private GameObject fieldPrefab;
    [SerializeField] private Material blackField;
    [SerializeField] private Material redField;
    [SerializeField] private int boardSize = 8;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        BuildBoard();
    }

    void BuildBoard()
    {
        for (int row = 0; row < boardSize; row++)
        {
            for (int column = 0; column < boardSize; column++)
            {
                GameObject squareSpace = Instantiate(fieldPrefab, new Vector3(row, 0, column), quaternion.identity);
                squareSpace.transform.parent = transform;

                Renderer chooseColor = squareSpace.GetComponent<Renderer>();
                if ((row + column) % 2 == 0)
                    chooseColor.material = redField;
                else 
                    chooseColor.material = blackField;
            }
        }
    }
}
