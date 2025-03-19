using TMPro;
using UnityEngine;

public class UIChat : MonoBehaviour
{
    [SerializeField] private TMP_InputField userInput;
    [SerializeField] private TMP_Text chatOutput;
    [SerializeField] private GameObject chatPanel;
    
    void Start()
    {
        chatPanel.SetActive(true);
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
        chatOutput.text += $"\nPlayer\n {userMessage}";
    }
}
