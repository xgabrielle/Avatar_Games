using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

public class UIChat : MonoBehaviour
{
    //public static UIChat Instanse { get; private set; }  
    [SerializeField] private TMP_InputField userInput;
    [SerializeField] private TMP_Text chatOutput;
    
    [SerializeField] private ScrollRect scrollView;
    
    void Awake()
    {
        //Instanse = this;
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
    }
    
    void LateUpdate()
    {
        Canvas.ForceUpdateCanvases();
    }
    
}
