using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIPersonality : MonoBehaviour
{
    [SerializeField] private GameObject aiTypePanel;
    [SerializeField] private GameObject opponentPanel;
    [SerializeField] private GameObject vsTypePanel;
    [SerializeField] private GameObject chatPanel;
    [SerializeField] private GameObject connectToGame;
    [SerializeField] private GameObject waitForPlayer;
    [SerializeField] private List<Button> buttons; 
    
    private void Start()
    {
        foreach (var setButton in buttons)
        {
            setButton.onClick.AddListener(() => OnButtonClick(setButton.name));
        }
        chatPanel.SetActive(false);
        opponentPanel.SetActive(true);
    }

    void OnButtonClick(string buttonName)
    {
        switch (buttonName)
        {
            case "AI" or "VS":
                GetOpponentType(buttonName);
                UIPlayerRole.instance.SetRole();
                break;
            case "Funny" or "Expert":
                SetAIPersonality(buttonName);
                break;
        }
    }
    void SetAIPersonality(string button)
    {
        switch (button)
        {
            case "Funny":
                ChatManager.Instance.SetPersonality(Personality.Funny);
                break;
            case "Expert":
                ChatManager.Instance.SetPersonality(Personality.Expert);
                break;
        }
        chatPanel.SetActive(true);
        aiTypePanel.SetActive(false);
        BoardGenerator.instance.BuildBoard();
        MarkersGenerator.instance.StartField();
    }

    void GetOpponentType(string button)
    {
        switch (button)
        {
            case "AI":
                GameManager.Instance.currentGameMode = GameMode.AI;
                GameManager.Instance.SetGame();
                aiTypePanel.SetActive(true);
                break;
            case "VS":
                GameManager.Instance.currentGameMode = GameMode.LocalPlayer;
                connectToGame.SetActive(true);
                break;
        }
        opponentPanel.SetActive(false);
    }

    private void OnWaitForOpponent()
    {
        connectToGame.SetActive(false);
        waitForPlayer.SetActive(true);
        Debug.Log("connect screen is deactivated, wait is active");
    }

    void StartVSGame()
    {
        connectToGame.SetActive(false);
        waitForPlayer.SetActive(false);
        Debug.Log("wait is deactivated");
        GameManager.Instance.SetGame();
        BoardGenerator.instance.BuildBoard();
        MarkersGenerator.instance.StartField();
    }
    private void OnEnable()
    {
        NetworkClient.WaitingForPlayer += OnWaitForOpponent;
        NetworkClient.OnPlayerConnect += StartVSGame;
    }

    private void OnDisable()
    {
        NetworkClient.WaitingForPlayer -= OnWaitForOpponent;
        NetworkClient.OnPlayerConnect -= StartVSGame;
    }
}
