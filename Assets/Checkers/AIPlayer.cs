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
                    var result = _checkerGame.CheckGameOver();
                    if (result != CheckerGame.GameResult.None)
                    { 
                        _checkerGame.isGameOver = true;
                        _checkerGame.gameResult = result;
                        string message = result == CheckerGame.GameResult.WhiteWins ? "White Wins!" :
                                         result == CheckerGame.GameResult.DarkWins ? "Dark Wins!" :
                                         "It's a Tie!";
                        GameOverUI.instance.UIGameOver(message);
                        ChatManager.Instance.SendMessageToAI("");
                    }
                    return;
                }
            }
        }
    }
}
