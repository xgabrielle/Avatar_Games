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
    private bool currentPlayer;

    private void Start()
    {
        _markerMovement = GetComponent<MarkerMovement>();
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
                if (hit.collider.CompareTag("DarkMarker"))
                {
                    currentPlayer = false;
                    markerPos = hit.collider.transform.position;
                    marker = hit.collider.gameObject;

                }
                
                else if (hit.collider.CompareTag("WhiteMarker"))
                {
                    currentPlayer = true;
                    markerPos = hit.collider.transform.position;
                    marker = hit.collider.gameObject;
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
                        if (_markerMovement.GetSurroundings(markerPos))
                        {
                            _markerMovement.Jump();
                        }
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
}
