using System;
using UnityEngine;
using UnityEngine.UI;

public class UIPersonality : MonoBehaviour
{
    [SerializeField] private GameObject startScreenUI;
    [SerializeField] private Button funnyButton;
    [SerializeField] private Button expertButton;
    
    private void Start()
    {
        UIChat.Instanse.chatPanel.SetActive(false);
        funnyButton.onClick.AddListener(() => SetAIPersonality("funny"));
        expertButton.onClick.AddListener(() => SetAIPersonality("expert"));
    }

    void SetAIPersonality(string button)
    {
        switch (button)
        {
            case "funny":
                ChatManager.Instance.SetPersonality(Personality.Funny);
                break;
            case "expert":
                ChatManager.Instance.SetPersonality(Personality.Expert);
                break;
        }
        UIChat.Instanse.chatPanel.SetActive(true);
        startScreenUI.SetActive(false);
        BoardGenerator.instance.BuildBoard();
        MarkersGenerator.instance.StartField();
        
    }

    
}
