using System;
using UnityEngine;

public enum PlayerTurn { White, Dark }
public class TurnManager : MonoBehaviour
{
  public static TurnManager instance { get; set; }
  public PlayerTurn currentPlayer = PlayerTurn.White;

  private void Start()
  {
    if (instance == null) instance = this;
    else Destroy(gameObject);
  }

  public void SwitchTurn()
  {
    currentPlayer = currentPlayer == PlayerTurn.White ? PlayerTurn.Dark : PlayerTurn.White;
    Debug.Log($"[{DateTime.Now}] [TurnManager] Turn switched to: {currentPlayer}");
  }

  public bool IsMyTurn()
  {
    bool isMyTurn = (currentPlayer == PlayerTurn.White && RoleManager.Role == "White Markers") ||
                   (currentPlayer == PlayerTurn.Dark && RoleManager.Role == "Dark Markers");
    
    Debug.Log($"[{DateTime.Now}] [TurnManager] Is my turn: {isMyTurn} (Current: {currentPlayer}, My Role: {RoleManager.Role})");
    return isMyTurn;
  }
}
