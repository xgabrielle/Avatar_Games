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
    //private bool isPiecePicked;
    private King _king;
    private AIPlayer _Ai;
    internal bool isAiTurn;
    

    private void Start()
    {
        _markerMovement = GetComponent<MarkerMovement>();
        _king = FindObjectOfType<King>();
        _Ai = GetComponent<AIPlayer>();
        isAiTurn = false;

    }
    
    private void Update()
    {
        /*if (Input.GetMouseButtonDown(0)) 
            HandlePlayerTurn();*/
        
        if (!isAiTurn)
        {
            if (Input.GetMouseButtonDown(0)) 
                HandlePlayerTurn();
        }
        else
        {
            isAiTurn = false;
            _Ai.StartCoroutine(_Ai.GetAiMove());
        }
    }

    void HandlePlayerTurn()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.CompareTag("DarkMarker") || hit.collider.CompareTag("WhiteMarker"))
            {
                HandleClickOnMarker(hit);
            }
                
            else if (hit.collider.CompareTag("BoardSquare") /*&& isPiecePicked*/)
            {
                HandleClickOnBoard(hit);
                isAiTurn = true;
            }
        }
    }

    internal void HandleClickOnMarker(RaycastHit hit)
    {
        //isPiecePicked = true;
        markerPos = hit.collider.transform.position;
        marker = hit.collider.gameObject;
        PlayingMarker(marker);
        //_markerMovement.GetSurroundings(markerPos);
    }

    internal void HandleClickOnBoard(RaycastHit hit)
    {
        targetPosition = hit.collider.transform.position + new Vector3(0,0.6f,0);

        if (_markerMovement.GetSurroundings(markerPos, hit.collider.gameObject))
        {
            if (_markerMovement.FreeJumpSpace(markerPos, _markerMovement.colEnemyPos)) 
                _markerMovement.Jump(hit.collider.gameObject);
                        
            else _markerMovement.GetMarkerMove(hit.collider.gameObject);
        }
        else _markerMovement.GetMarkerMove(hit.collider.gameObject);

        if (targetPosition.z > 6 || targetPosition.z < 1)
        {
            _king.SpawnCrown(marker,crown);
        }

        //isPiecePicked = false;
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
    
    internal string GetEnemyTag(GameObject pawn)
    {
        if (marker.tag==null)
        {
            Debug.Log("Marker is null or tag?" +marker.tag + "tag: "+ tag + "marker: "+ marker);
        }
        string myTag = pawn.tag;
        string enemyTag = myTag == "DarkMarker" ? "WhiteMarker" : "DarkMarker";
        return enemyTag;
    }

}
