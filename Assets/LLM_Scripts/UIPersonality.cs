using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UIPersonality : MonoBehaviour
{
    [SerializeField] private GameObject aiTypePanel;
    [SerializeField] private GameObject opponentPanel;
    [SerializeField] private GameObject chatPanel;
    [SerializeField] private List<Button> buttons; 
    
    private void Start()
    {
        foreach (var setButton in buttons)
        {
            setButton.onClick.AddListener(() => OnButtonClick(setButton.name));
        }
        chatPanel.SetActive(false);
        aiTypePanel.SetActive(true);
    }

    void OnButtonClick(string buttonName)
    {
        switch (buttonName)
        {
            // FIRST CHOOSE A GAME TO PLAY
            // SECOND WHO YOU'RE PLAYING WITH
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
                break;
            case "VS":
                break;
        }
        opponentPanel.SetActive(false);
        aiTypePanel.SetActive(true);
    }

    
}
