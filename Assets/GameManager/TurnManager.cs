using System;
using UnityEngine;

public enum PlayerTurn {Player1, Player2 }
public class TurnManager : MonoBehaviour
{
  public static TurnManager instance { get; set; }
  public PlayerTurn currentPlayer = PlayerTurn.Player1;

  private void Start()
  {
    instance = this;
  }

  public void SwitchTurn()
  {
    currentPlayer = currentPlayer == PlayerTurn.Player1 ? PlayerTurn.Player2 : PlayerTurn.Player1;
  }
}
