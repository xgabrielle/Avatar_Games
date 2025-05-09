using System;
using TMPro;
using UnityEngine;

public enum PlayerTurn { White, Dark }
public class TurnManager : MonoBehaviour
{
  public static TurnManager instance { get; set; }
  public PlayerTurn currentPlayer = PlayerTurn.White;
  [SerializeField] private TextMeshProUGUI playerTurn;

  private void Start()
  {
    if (instance == null) instance = this;
    else Destroy(instance);
  }

  public void SwitchTurn()
  {
    currentPlayer = currentPlayer == PlayerTurn.White ? PlayerTurn.Dark : PlayerTurn.White;
  }

  public bool IsMyTurn()
  {
    bool isMyTurn = (currentPlayer == PlayerTurn.White && RoleManager.Role == "White Markers") ||
                   (currentPlayer == PlayerTurn.Dark && RoleManager.Role == "Dark Markers");
    return isMyTurn;
  }

  internal void UIPlayerTurn()
  {
    string currentTurn = currentPlayer.ToString();
    playerTurn.text = $"{currentTurn} turn to move";
  }
}
