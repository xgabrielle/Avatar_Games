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

    void HandleClickOnMarker(RaycastHit hit)
    {
        isPiecePicked = true;
        markerPos = hit.collider.transform.position;
        marker = hit.collider.gameObject;
        PlayingMarker(marker);
        _markerMovement.GetSurroundings(markerPos);
    }

    void HandleClickOnBoard(RaycastHit hit)
    {
        targetPosition = hit.collider.transform.position;

        if (_markerMovement.GetSurroundings(markerPos))
        {
            if (FreeJumpSpace(markerPos, _markerMovement.colEnemyPos)) 
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
    
    internal bool FreeJumpSpace(Vector3 markerPos, Vector3 enemyPos)
    {
        Vector3 landingPos = (enemyPos - markerPos) + enemyPos;
        Collider[] col = Physics.OverlapSphere(landingPos, 0.2f);
        if (col.Length > 1 || landingPos.x > 7 || landingPos.x < 0 || landingPos.z > 7 || landingPos.z < 0)
        {
            return false;
        }
        return true;
    }
    
    internal string GetEnemyTag()
    {
        string myTag = marker.tag;
        string enemyTag = myTag == "DarkMarker" ? "WhiteMarker" : "DarkMarker";
        return enemyTag;
    }

}
