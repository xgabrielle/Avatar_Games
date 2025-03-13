using System;
using Unity.VisualScripting;
using UnityEngine;


public class CheckerGame : MonoBehaviour
{
    internal Vector3 targetPosition;
    internal Vector3 markerPos;
    [SerializeField] private Material blue;
    [SerializeField] private Material originalColor;
    [SerializeField] private Material dark;
    internal GameObject marker;
    internal GameObject previousMarker;
    [SerializeField] private GameObject crown;
    private MarkerMovement _markerMovement;
    private bool isPiecePicked;
    private King _king;
    

    private void Start()
    {
        _markerMovement = GetComponent<MarkerMovement>();
        _king = FindObjectOfType<King>();

    }
    
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("DarkMarker") || hit.collider.CompareTag("WhiteMarker"))
                {
                    HandleClickOnMarker(hit);
                }
                
                else if (hit.collider.CompareTag("BoardSquare") && isPiecePicked)
                {
                    HandleClickOnBoard(hit);
                }
            }
        }
    }

    internal void HandleClickOnMarker(RaycastHit hit)
    {
        isPiecePicked = true;
        markerPos = hit.collider.transform.position;
        marker = hit.collider.gameObject;
        PlayingMarker(marker);
        _markerMovement.GetSurroundings(markerPos);
    }

    internal void HandleClickOnBoard(RaycastHit hit)
    {
        targetPosition = hit.collider.transform.position;

        if (_markerMovement.GetSurroundings(markerPos))
        {
            if (_markerMovement.FreeJumpSpace(markerPos, _markerMovement.colEnemyPos)) 
                _markerMovement.Jump();
                        
            else _markerMovement.GetMarkerMove();
        }
        else _markerMovement.GetMarkerMove();

        if (targetPosition.z > 6 || targetPosition.z < 1)
        {
            _king.SpawnCrown(marker,crown);
        }

        isPiecePicked = false;
    }

  
    void PlayingMarker(GameObject marker)
    {
        Renderer chooseColor = this.marker.GetComponent<Renderer>();

        if (previousMarker != null && previousMarker != marker)
        {
            if (previousMarker.CompareTag("WhiteMarker"))
                previousMarker.GetComponent<Renderer>().material = originalColor;
            else previousMarker.GetComponent<Renderer>().material = dark;
        }

        previousMarker = marker;
        chooseColor.material = blue;

    }
    
    internal string GetEnemyTag()
    {
        string myTag = marker.tag;
        string enemyTag = myTag == "DarkMarker" ? "WhiteMarker" : "DarkMarker";
        return enemyTag;
    }

}
