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
    private MarkerMovement.MoveResult moveResult;
    private void Start()
    {
        _checkerGame = GetComponent<CheckerGame>();
    }

    internal void HandlePlayerTurn(GameObject player)
    {
        if (GameManager.Instance.IsLocalPlayerMode)
            if (!TurnManager.instance.IsMyTurn()) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hit))
        {
            if (hit.collider.CompareTag(player.tag))
                HandleClickOnMarker(hit);
            else if (hit.collider.CompareTag("BoardSquare") && isMarker)
                HandleClickOnBoard(hit);
        }
    }

    void HandleClickOnMarker(RaycastHit hit)
    {
        currentMarkerPos = hit.collider.transform.position;
        currentMarker = hit.collider.gameObject;
        isMarker = true;
    }

    void HandleClickOnBoard(RaycastHit hit)
    {
        targetPosition = hit.collider.transform.position + new Vector3(0,0.6f,0);
        moveResult = MarkerMovement.movement.ValidateMove(currentMarker, currentMarkerPos, targetPosition);
        
        bool valid = moveResult.isValid || MarkerMovement.movement.Jump(currentMarker, currentMarkerPos, targetPosition);
        if (valid)
        {
            ValidMove();
            if (GameManager.Instance.IsLocalPlayerMode)
                NetworkClient.Client.SendMove(currentMarkerPos, targetPosition);
            if (GameManager.Instance.IsAIMode)
            {
                TurnManager.instance.SwitchTurn();
            }
        }
        // Do nothing if not valid: don't switch turn
    }

    void ValidMove()
    {
        currentMarker.transform.position = moveResult.landingPos; 
        isMarker = false;
        if (GameManager.Instance.IsAIMode)
        {
            MarkersGenerator.instance.UpdatePawns(currentMarker, currentMarkerPos, targetPosition);
        }
        GameStateManager.instance.LastMove(currentMarker, currentMarkerPos, targetPosition);

        var result = _checkerGame.CheckGameOver();
        if (result != CheckerGame.GameResult.None)
        {
            _checkerGame.isGameOver = true;
            _checkerGame.gameResult = result;
            string message = result == CheckerGame.GameResult.WhiteWins ? "White Wins!" :
                             result == CheckerGame.GameResult.DarkWins ? "Dark Wins!" :
                             "It's a Tie!";
            GameOverUI.instance.UIGameOver(message);
        }
    }
}
