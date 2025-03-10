using System;
using UnityEngine;


public class CheckerGame : MonoBehaviour
{
    internal Vector3 targetPosition;
    internal Vector3 markerPos;
    [SerializeField] private Material yellow;
    [SerializeField] private Material blue;
    [SerializeField] private Material originalColor;
    internal GameObject marker;
    private GameObject previousMarker;
    private MarkerMovement _markerMovement;
    private AvailableMoveChecker _availableMoveChecker;
    private bool currentPlayer;

    private void Start()
    {
        _markerMovement = GetComponent<MarkerMovement>();
        _availableMoveChecker = GetComponent<AvailableMoveChecker>();
    }
    
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            int layerMask = ~LayerMask.GetMask("Triggerbox");

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {
                Debug.Log("Inside raycast");
                if (hit.collider.CompareTag("DarkMarker"))
                {
                    currentPlayer = false;
                    markerPos = hit.collider.transform.position;
                    marker = hit.collider.gameObject;
                    _availableMoveChecker.ActiveTrigger(hit.collider.gameObject.transform.position);

                }
                
                else if (hit.collider.CompareTag("WhiteMarker"))
                {
                    currentPlayer = true;
                    markerPos = hit.collider.transform.position;
                    marker = hit.collider.gameObject;
                    _availableMoveChecker.ActiveTrigger(hit.collider.gameObject.transform.position);
                    PlayingMarker(marker);

                }
                
                
                if (hit.collider.CompareTag("BoardSquare"))
                {
                    targetPosition = hit.collider.transform.position;
                    if (currentPlayer == false)
                    {
                        _markerMovement.DarkMarkerMove();
                    }
                    else
                    { 
                        _markerMovement.WhiteMarkerMove();
                    }
                    
                }
                
            }
            
        }
        
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
