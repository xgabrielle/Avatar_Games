using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPersonality : MonoBehaviour
{
    [SerializeField] private GameObject aiTypePanel;
    [SerializeField] private GameObject opponentPanel;
    [SerializeField] private GameObject vsTypePanel;
    [SerializeField] private GameObject chatPanel;
    
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
                GameManager.Instance.SetGame();
                aiTypePanel.SetActive(true);
                break;
            case "VS":
                GameManager.Instance.SetGame();
                vsTypePanel.SetActive(true);
                break;
        }
        opponentPanel.SetActive(false);
    }

    
}
