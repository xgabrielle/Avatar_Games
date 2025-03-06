using System;
using UnityEngine;


public class CheckerGame : MonoBehaviour
{
    private Vector3 targetPosition;
    private Vector3 markerPos;
    [SerializeField] private Material yellow;
    [SerializeField] private Material blue;
    [SerializeField] private Material originalColor;
    private GameObject marker;
    private GameObject previousMarker;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        

    }

    void AvailableMove(GameObject square)
    {
        
        if (DiagonalMove(markerPos, targetPosition))
        {
            Debug.Log($"TargetPos: {targetPosition}");
            Renderer chooseColor = square.GetComponent<Renderer>();
            chooseColor.material = yellow;

            if (targetPosition.z > markerPos.z)
            { 
                NewMarkerPos(targetPosition);
            }
            
        } else
            Debug.Log("Not a possible move");

      
    }

    void NewMarkerPos(Vector3 newMarkerPos)
    {
        markerPos = newMarkerPos;
        marker.transform.position = markerPos + new Vector3(0,0.5f,0);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("WhiteMarker"))
                {
                    markerPos = hit.collider.transform.position;
                    marker = hit.collider.gameObject;
                    PlayingMarker(marker);

                }
                
                if (hit.collider.CompareTag("BoardSquare"))
                {
                    targetPosition = hit.collider.transform.position;
                    AvailableMove(hit.collider.gameObject);
                    
                }
                
            }
            
        }
        
    }
    
    internal static bool DiagonalMove(Vector3 startPos, Vector3 targetPos)
    {
        float newPosX = Mathf.Abs(targetPos.x-startPos.x);
        float newPosZ = Math.Abs(targetPos.z-startPos.z);
        
        if (Mathf.Approximately(newPosX,1) && Mathf.Approximately(newPosZ,1)) 
            return Mathf.Approximately(newPosX, newPosZ);
        return false;

    }

    void PlayingMarker(GameObject marker)
    {
        Renderer chooseColor = this.marker.GetComponent<Renderer>();

        if (previousMarker != null && previousMarker != marker)
        {
            previousMarker.GetComponent<Renderer>().material = originalColor;
        }

        previousMarker = marker;
        chooseColor.material = blue;

    }

    void PlayerMove()
    {

    }

    void AIMove()
    {
        
    }

    void PossibleMoves()
    {
    }

    bool OutOfBounds(float x, float z) => (x < 0 && x > 7 && z < 0 && z > 7);
}
