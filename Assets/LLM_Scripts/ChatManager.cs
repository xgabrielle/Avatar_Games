using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public enum Personality
{
      Funny,
      Serious,
      Sarcastic,
      Expert
}
public class ChatManager : MonoBehaviour
{
    public static ChatManager Instance { get; set; }
   
    private string apiKey = Environment.GetEnvironmentVariable("API_KEY");
    private string apiUrl = "";

    private string systemMessage;
    private Personality currentPersonality;

    [SerializeField] private TMP_InputField userInput;
    [SerializeField] private TMP_Text chatOutput;
    [SerializeField] private GameObject chatPanel;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else Destroy(gameObject);
    }

    IEnumerator SendRequest(string userMessage)
    {
        var requestData = new
        {
            model = "gpt-4-turbo",
            messages = new[]
            {
                new {role = "system", content = systemMessage},
                new {role = "user", content = userMessage}
            },
            max_tokens = 50 // length of AI response
        };

        string json = JsonUtility.ToJson(requestData);
        UnityWebRequest request = new UnityWebRequest(apiUrl, "Post");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", $"Bearer {apiKey}");
        
        yield return request.SendWebRequest();
        
        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Request is Successful!");
        }
        else
        {
            Debug.LogError($"Error: {request.error}");
        }
    }

    public void SetGameContext(string newSystemMessage)
    {
        
    }

    public void SetPersonality(Personality newPersonality)
    {
        
    }

    public void SendMessageToAI(string userMessage)
    {
        
    }
}