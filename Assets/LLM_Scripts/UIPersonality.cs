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
        funnyButton.onClick.AddListener(SetFunnyAI);
        expertButton.onClick.AddListener(SetExpertAI);
    }

    public void SetFunnyAI()
    {
        ChatManager.Instance.SetPersonality(Personality.Funny);
        UIChat.Instanse.chatPanel.SetActive(true);
        startScreenUI.SetActive(false);
        BoardGenerator.instanse.BuildBoard();
        MarkersGenerator.instanse.StartField();
        
       
    }

    public void SetExpertAI()
    {
        ChatManager.Instance.SetPersonality(Personality.Expert);
        UIChat.Instanse.chatPanel.SetActive(true);
        startScreenUI.SetActive(false);
        BoardGenerator.instanse.BuildBoard();
        MarkersGenerator.instanse.StartField();
    }
    
}
