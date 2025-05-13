using System.Collections;
using UnityEngine;

public class AIPlayer : MonoBehaviour
{
    private CheckerGame _checkerGame;
    private GameObject[] _darkPawns;

    private void Start()
    {
        _checkerGame = GetComponent<CheckerGame>();
    }

    internal IEnumerator GetAiMove()
    {
        yield return new WaitForSeconds(1.0f);
        EasyRandomMove();
        TurnManager.instance.SwitchTurn();
        _checkerGame.aiCoroutine = null;

    }
    void EasyRandomMove()
    {
        _darkPawns = GameObject.FindGameObjectsWithTag("DarkMarker");
        foreach (GameObject pawn in _darkPawns)
        {
            Vector3 pawnPos = pawn.transform.position;

            var possibleMoves = MarkerMovement.PossibleMoves(pawn);
            foreach (var move in possibleMoves)
            {
                var moveResult = MarkerMovement.movement.ValidateMove(pawn, pawnPos, move);
                if (moveResult.isValid)
                {
                    pawn.transform.position = moveResult.landingPos;
                    if (moveResult.capturedPawn != null)
                        ObjectPool.Instance.Return(moveResult.capturedPawn);
                    MarkersGenerator.instance.UpdatePawns(pawn, pawnPos, moveResult.landingPos );
                    GameStateManager.instance.LastMove(pawn, pawnPos, moveResult.landingPos);
                    if (_checkerGame.HasGameOver(_checkerGame.isGameOver ? "WhiteMarker" : "DarkMarker"))
                    { 
                        _checkerGame.isGameOver = true;
                        Debug.Log("Game over, AI lost (No valid moves)");
                        ChatManager.Instance.SendMessageToAI("");
                    }
                    return;
                }
            }
        }
    }
}
