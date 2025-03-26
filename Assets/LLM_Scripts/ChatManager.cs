using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using dotenv.net;
using Newtonsoft.Json;
using Unity.VisualScripting;


public enum Personality
{
      Funny,
      Expert
}
public class ChatManager : MonoBehaviour
{
    private CheckerGame _checkerGame;
    public static ChatManager Instance { get; private set; }
    [SerializeField] private UIChat uiChat;

    private string apiKey;
    private string apiUrl = "https://api.openai.com/v1/chat/completions";
    string systemMessage;
    
    private Personality currentPersonality;

    private void Start()
    {
        Instance = this;
        DotEnv.Load();
        apiKey = Environment.GetEnvironmentVariable("API_KEY");

        if (string.IsNullOrEmpty(apiKey))
        {
            Debug.LogError("API Key not found! Make sure you have a .env file.");
        }
        
    }

    IEnumerator SendRequest(string userMessage, UIChat uiChat)
    {
        
        var gameState = GameStateManager.instance.GetBoardStateAsJSON(); 
    
        string toAI = $"{userMessage}\nGame State:\n{gameState}";
        var requestData = new
        {
            model = "gpt-4-turbo",
            messages = new Message[]
            {
                new Message {role = "system", content = SetGameContext() + "respond as short as possible." },
                new Message {role = "user", content = toAI}
            },
            max_tokens = 100 // length of AI response
        };
        
        string json = JsonConvert.SerializeObject(requestData);
        UnityWebRequest request = new UnityWebRequest(apiUrl, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", $"Bearer {apiKey}");
       
        yield return request.SendWebRequest();
        
        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Request is Successful!");
            string responseText = request.downloadHandler.text;
            ChatResponse chatResponse = JsonConvert.DeserializeObject<ChatResponse>(responseText);
            
            uiChat.AppendMessage($"\nAI: {chatResponse.choices[0].message?.content}");
            
        }
        else
        {
            Debug.LogError($"Error: {request.error}");
            Debug.LogError($"Response: {request.downloadHandler.text}"); 
        }
    }

    string SetGameContext()
    {
        return systemMessage + GameManager.Instance.SetGame();
    }

    public void SetPersonality(Personality newPersonality)
    {
        currentPersonality = newPersonality;
        switch (newPersonality)
        {
            case Personality.Funny:
                systemMessage = "You're a kind AI that makes a few small jokes during the game and want to get the other player to laugh.";
                Debug.Log("Funny AI");
                break;
            case Personality.Expert:
                systemMessage = "You are very good a checkers and will not hesitate to give your opinion on your components move. You start with an intimidating comment.";
                Debug.Log("Expert AI");
                break;
            
        }

        StartCoroutine(SendRequest(systemMessage, uiChat));
    }

    internal Coroutine GetAIMessage(string aiMessage)
    {
        return StartCoroutine(SendRequest(aiMessage, uiChat));
    }
    
    public void SendMessageToAI(string userMessage)
    {
        uiChat.AppendMessage($"\nPlayer: {userMessage}");
        StartCoroutine(SendRequest(userMessage, uiChat));

    }
}

public class ChatResponse
{
    public Choice[] choices; // array possible AI responses
}
public class Choice
{
    public Message message; // AI response
}
public class Message
{
    public string role; // who sent the message
    public string content; // the message
}

