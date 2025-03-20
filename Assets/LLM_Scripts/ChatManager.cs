using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using dotenv.net;
using Newtonsoft.Json;
 

public enum Personality
{
      Funny,
      Expert
}
public class ChatManager : MonoBehaviour
{
    public static ChatManager Instance { get; set; }
    [SerializeField] private UIChat uiChat;

    private string apiKey;
    private string apiUrl = "https://api.openai.com/v1/chat/completions";

    private string systemMessage;
    private Personality currentPersonality;

    /*private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else Destroy(gameObject);
    }*/

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
        var requestData = new
        {
            model = "gpt-4-turbo",
            messages = new Message[]
            {
                new Message {role = "system", content = systemMessage},
                new Message {role = "user", content = userMessage}
            },
            max_tokens = 50 // length of AI response
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
            Debug.Log("Full API Response: " + request.downloadHandler.text);
            
            ChatResponse chatResponse = JsonConvert.DeserializeObject<ChatResponse>(responseText);
            
            uiChat.AppendMessage($"AI: "+ chatResponse.choices[0].message?.content);
            
        }
        else
        {
            Debug.LogError($"Error: {request.error}");
            Debug.LogError($"Response: {request.downloadHandler.text}"); 
        }
    }

    public void SetGameContext(string newSystemMessage)
    {
        systemMessage = newSystemMessage;
    }

    public void SetPersonality(Personality newPersonality)
    {
        currentPersonality = newPersonality;
        switch (newPersonality)
        {
            case Personality.Funny:
                systemMessage = "You're a kind AI that makes a few small jokes during the game";
                Debug.Log("Funny AI");
                break;
            case Personality.Expert:
                systemMessage = "You are very good a checkers and will not hesitate to give your opinion on your components move.";
                Debug.Log("Expert AI");
                break;
            
        }
    }

    public void SendMessageToAI(string userMessage)
    {
        uiChat.AppendMessage($"Player: {userMessage}");
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

