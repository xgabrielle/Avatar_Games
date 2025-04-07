using System;
using Unity.Mathematics;
using UnityEngine;

public class Player : MonoBehaviour
{
    private CheckerGame _checkerGame;
    private bool isMarker;
    private Vector3 targetPosition;
    private Vector3 currentMarkerPos;
    private GameObject currentMarker;
    private void Start()
    {
        _checkerGame = GetComponent<CheckerGame>();
    }

    internal void HandlePlayerTurn()
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

    void HandleClickOnMarker(RaycastHit hit)
    {
        currentMarkerPos = hit.collider.transform.position;
        currentMarker = hit.collider.gameObject;
        isMarker = true;
        MarkerMovement.Movement.GetSurroundings(currentMarker);
    }

    void HandleClickOnBoard(RaycastHit hit)
    {
        targetPosition = hit.collider.transform.position + new Vector3(0,0.6f,0);

        if (MarkerMovement.Movement.GetSurroundings(currentMarker))
        {
            if (MarkerMovement.Movement.Jump(currentMarker, currentMarkerPos,targetPosition))
            {
                currentMarker.transform.position = targetPosition;
                _checkerGame.isAiTurn = true;
                isMarker = false;
            }
                        
            else if (MarkerMovement.Movement.GetMarkerMove(currentMarker, currentMarkerPos, targetPosition))
            {
                currentMarker.transform.position = targetPosition;
                _checkerGame.isAiTurn = true;
                isMarker = false;
            }
        }
        else if (MarkerMovement.Movement.GetMarkerMove(currentMarker, currentMarkerPos, targetPosition))
        {
            currentMarker.transform.position = targetPosition;
            _checkerGame.isAiTurn = true;
            isMarker = false;
        }
        MarkersGenerator.instance.UpdatePawns(currentMarker, currentMarkerPos, targetPosition);
        GameStateManager.instance.LastMove(currentMarker, currentMarkerPos, targetPosition);
        if (_checkerGame.HasGameOver(_checkerGame.isAiTurn ? "WhiteMarker" : "DarkMarker"))
        {
            _checkerGame.isGameOver = true;
            Debug.Log("Game over for White: ");
        }
    }
}
