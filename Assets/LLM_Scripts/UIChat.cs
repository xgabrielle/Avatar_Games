using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

public class UIChat : MonoBehaviour
{
    public static UIChat Instanse { get; set; }  
    [SerializeField] private TMP_InputField userInput;
    [SerializeField] private TMP_Text chatOutput;
    [SerializeField] internal GameObject chatPanel;
    [SerializeField] private ScrollRect scrollView;
    
    void Awake()
    {
        Instanse = this;
        //chatPanel.SetActive(true);
    }

    private void Start()
    {
        ChatManager.Instance.SetGameContext("The game has started! Let's play checkers. Try your best moves!");

    }

    public void SendUserMessage()
    {
        if (!string.IsNullOrWhiteSpace(userInput.text))
        { 
            string userMessage = userInput.text;
            userInput.text = "";
            ChatManager.Instance.SendMessageToAI(userMessage);
        }
    }

    public void AppendMessage(string userMessage)
    {
        Debug.Log("Appending message: " + userMessage);
        
        chatOutput.text += $"\n{userMessage}";
        scrollView.verticalNormalizedPosition = 0f;
    }
    
    void LateUpdate()
    {
        Canvas.ForceUpdateCanvases();
    }
    
}
