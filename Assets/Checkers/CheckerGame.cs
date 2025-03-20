using System;
using Unity.VisualScripting;
using UnityEngine;


public class CheckerGame : MonoBehaviour
{
    [SerializeField] private GameObject crown;
    
    private Vector3 targetPosition;
    private Vector3 currentMarkerPos;
    private GameObject currentMarker;
    internal bool isAiTurn;
    internal bool isGameOver;
    private bool isMarker;
    private AIPlayer _Ai;
    

    private void Start()
    {
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
                
            else if (hit.collider.CompareTag("BoardSquare"))
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
        MarkerMovement.Movement.GetSurroundings(currentMarkerPos, currentMarker);
    }

    internal void HandleClickOnBoard(RaycastHit hit)
    {
        targetPosition = hit.collider.transform.position + new Vector3(0,0.6f,0);

        if (MarkerMovement.Movement.GetSurroundings(currentMarkerPos, currentMarker))
        {
            if (MarkerMovement.Movement.Jump(currentMarker, currentMarkerPos,targetPosition))
            {
                currentMarker.transform.position = targetPosition;
                isAiTurn = true;
                isMarker = false;
            }
                        
            else if (MarkerMovement.Movement.GetMarkerMove(currentMarker, currentMarkerPos, targetPosition))
            {
                currentMarker.transform.position = targetPosition;
                isAiTurn = true;
                isMarker = false;
            }
        }
        else if (MarkerMovement.Movement.GetMarkerMove(currentMarker, currentMarkerPos, targetPosition))
        {
            currentMarker.transform.position = targetPosition;
            isAiTurn = true;
            isMarker = false;
        }
        
        
    }
    
    
    internal string GetEnemyTag(GameObject pawn)
    {
        string myTag = pawn.tag;
        string enemyTag = myTag == "DarkMarker" ? "WhiteMarker" : "DarkMarker";
        return enemyTag;
    }

}
