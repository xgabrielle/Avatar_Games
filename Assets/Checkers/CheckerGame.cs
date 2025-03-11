using System;
using UnityEngine;


public class CheckerGame : MonoBehaviour
{
    internal Vector3 targetPosition;
    internal Vector3 markerPos;
    
    [SerializeField] private Material blue;
    [SerializeField] private Material originalColor;
    internal GameObject marker;
    private GameObject previousMarker;
    private MarkerMovement _markerMovement;

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

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("DarkMarker"))
                {
                    markerPos = hit.collider.transform.position;
                    marker = hit.collider.gameObject;
                    _markerMovement.GetSurroundings(markerPos);

                }
                
                else if (hit.collider.CompareTag("WhiteMarker"))
                {
                    markerPos = hit.collider.transform.position;
                    marker = hit.collider.gameObject;
                    PlayingMarker(marker);
                    _markerMovement.GetSurroundings(markerPos);
                }
                
                if (hit.collider.CompareTag("BoardSquare"))
                {
                    targetPosition = hit.collider.transform.position;
                    if (_markerMovement.GetSurroundings(markerPos)) 
                        _markerMovement.Jump();
                    else _markerMovement.GetMarkerMove();
                    
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

    internal string GetEnemy()
    {
        string myTag = marker.tag;
        string enemyTag = myTag == "DarkMarker" ? "WhiteMarker" : "DarkMarker";
        return enemyTag;
    }

    void AIMove()
    {
        
    }
}
