using UnityEngine;
using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class UIMenu : MonoBehaviour
{
   [SerializeField] private GameObject aiTypePanel;
    [SerializeField] private GameObject opponentPanel;
    
    [SerializeField] private GameObject connectToGame;
    [SerializeField] private GameObject waitForPlayer;
    [SerializeField] private List<Button> buttons;
    public static UIMenu instance { get; private set; }

    private void Start()
    {
        if (instance == null) instance = this;
        else Destroy(instance);
        foreach (var setButton in buttons)
        {
            setButton.onClick.AddListener(() => OnButtonClick(setButton.name));
        }
        
        opponentPanel.SetActive(true);
    }

    void OnButtonClick(string buttonName)
    {
        switch (buttonName)
        {
            case "AI" or "VS":
                GetOpponentType(buttonName);
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
                UIPersonality.instance.SetPersonality(Personality.Funny);
                break;
            case "Expert":
                UIPersonality.instance.SetPersonality(Personality.Expert);
                break;
        }
        
        
        aiTypePanel.SetActive(false);

        LoadGameScene();
    }

    void GetOpponentType(string button)
    {
        switch (button)
        {
            case "AI":
                GameManager.Instance.SetGameMode(GameMode.AI);
                aiTypePanel.SetActive(true);
                UIPlayerRole.instance.SetRole();
                break;
            case "VS":
                GameManager.Instance.SetGameMode(GameMode.LocalPlayer);
                connectToGame.SetActive(true);
                break;
        }
        
        opponentPanel.SetActive(false);
    }

    private void OnWaitForOpponent()
    {
        connectToGame.SetActive(false);
        waitForPlayer.SetActive(true);
    }

    internal void StartVSGame()
    {
        connectToGame.SetActive(false);
        waitForPlayer.SetActive(false);
        LoadGameScene();
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
    
    private void LoadGameScene()
    {
        SceneManager.LoadScene("Checkers");
    }

}
