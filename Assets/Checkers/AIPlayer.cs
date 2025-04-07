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
        yield return new WaitForSeconds(3.0f);
        EasyRandomMove();
        _checkerGame.isAiTurn = false;

    }
    void EasyRandomMove()
    {
        _darkPawns = GameObject.FindGameObjectsWithTag("DarkMarker");
        foreach (GameObject pawn in _darkPawns)
        {
            Vector3 pawnPos = pawn.transform.position;

            var possibleMoves = MarkerMovement.PossibleMoves(pawnPos);
            foreach (var move in possibleMoves)
            {
                var moveResult = MarkerMovement.Movement.ValidateMove(pawn, pawnPos, move);
                if (moveResult.IsValid)
                {
                    pawn.transform.position = moveResult.LandingPos;
                    if (moveResult.CapturedPawn != null)
                        Destroy(moveResult.CapturedPawn);
                    MarkersGenerator.instance.UpdatePawns(pawn, pawnPos, moveResult.LandingPos );
                    GameStateManager.instance.LastMove(pawn, pawnPos, moveResult.LandingPos);
                    if (_checkerGame.HasGameOver(_checkerGame.isAiTurn ? "WhiteMarker" : "DarkMarker"))
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
