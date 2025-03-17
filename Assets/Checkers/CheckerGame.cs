using System;
using Unity.VisualScripting;
using UnityEngine;


public class CheckerGame : MonoBehaviour
{
    [SerializeField] private Material blue;
    [SerializeField] private Material originalColor;
    [SerializeField] private Material dark;
    [SerializeField] private GameObject crown;
    
    internal Vector3 targetPosition;
    internal Vector3 currentMarkerPos;
    internal GameObject currentMarker;
    internal GameObject previousMarker;
    internal bool isAiTurn;
    private bool isGameOver;
    private bool isMarker;
    private MarkerMovement _markerMovement;
    //private King _king;
    private AIPlayer _Ai;
    

    private void Start()
    {
        _markerMovement = GetComponent<MarkerMovement>();
        //_king = FindObjectOfType<King>();
        _Ai = GetComponent<AIPlayer>();
        isAiTurn = false;
        isGameOver = false;

    }
    
    private void Update()
    {
        if (!isGameOver)
        {
            if (!isAiTurn)
            {
                if (Input.GetMouseButtonDown(0)) 
                    HandlePlayerTurn();
                Debug.Log("Ai turn");
            }
            else
            {
                _Ai.StartCoroutine(_Ai.GetAiMove());
                Debug.Log("Player Turn");
                isAiTurn = false;
            }
            
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
                if (isMarker) HandleClickOnBoard(hit);
            }
        }
    }

    internal void HandleClickOnMarker(RaycastHit hit)
    {
        currentMarkerPos = hit.collider.transform.position;
        currentMarker = hit.collider.gameObject;
        isMarker = true;
        _markerMovement.GetSurroundings(currentMarkerPos, currentMarker);
    }

    internal void HandleClickOnBoard(RaycastHit hit)
    {
        targetPosition = hit.collider.transform.position + new Vector3(0,0.6f,0);

        if (_markerMovement.GetSurroundings(currentMarkerPos, currentMarker))
        {
            if (_markerMovement.FreeJumpSpace(currentMarkerPos, targetPosition).Item1) 
                _markerMovement.Jump(currentMarker,currentMarkerPos, _markerMovement.FreeJumpSpace(currentMarkerPos, targetPosition).Item2);
                        
            else if (_markerMovement.GetMarkerMove(currentMarker, currentMarkerPos, targetPosition))
            {
                currentMarker.transform.position = targetPosition;
                isAiTurn = true;
                isMarker = false;
            }
        }
        else if (_markerMovement.GetMarkerMove(currentMarker, currentMarkerPos, targetPosition))
        {
            currentMarker.transform.position = targetPosition;
            isAiTurn = true;
            isMarker = false;
        }

        /*if (targetPosition.z > 6 || targetPosition.z < 1)
        {
            _king.SpawnCrown(currentMarker,crown);
        }*/
        
    }

  
    /*void PlayingMarker(GameObject marker)
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

    }*/
    
    internal string GetEnemyTag(GameObject pawn)
    {
        string myTag = pawn.tag;
        string enemyTag = myTag == "DarkMarker" ? "WhiteMarker" : "DarkMarker";
        return enemyTag;
    }

}
