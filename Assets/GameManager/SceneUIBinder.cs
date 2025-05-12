using TMPro;
using UnityEngine;

public class SceneUIBinder : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerTurnText;

    private void Start()
    {
        if (TurnManager.instance != null)
        {
            TurnManager.instance.playerTurn = playerTurnText;
            TurnManager.instance.UIPlayerTurn();
        }
    }
}
