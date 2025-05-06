using System;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using dotenv.net;
using Unity.VisualScripting;

public class UINetworkIP : MonoBehaviour
{
    private string _serverIP;
    [SerializeField] private TMP_InputField ipAddress;
    [SerializeField] private Button connectButton;
    [SerializeField] private Button disconnectButton;

    private void Awake()
    {
        #if UNITY_EDITOR
        // Works in Editor
        DotEnv.Load(); 
        _serverIP = Environment.GetEnvironmentVariable("SERVER_IP");
        #else
        // Works in Build
        string envPath = Path.Combine(Application.streamingAssetsPath, ".env");
        foreach (string line in File.ReadAllLines(envPath))
        {
            if (line.StartsWith("SERVER_IP"))
            {
                _serverIP = line.Split('=')[1].Trim();
                Debug.Log("SERVER CHECK: " +_serverIP);
                break;
            }
        }
        #endif

        Debug.Log("SERVER IP: " + _serverIP);
    }
    private void Start()
    {
        /*DotEnv.Load();
        _serverIP = Environment.GetEnvironmentVariable("SERVER_IP");*/
        ipAddress.text = _serverIP;
        connectButton.onClick.AddListener(OnClickConnect);
        disconnectButton.onClick.AddListener(OnClickDisconnect);
        if (string.IsNullOrEmpty(_serverIP))
        {
            Debug.LogError("SERVER_IP not found! Make sure you have a .env file.");
        }
    }

    private void OnClickConnect()
    {
        string ip = ipAddress.text.Trim();
        if (string.IsNullOrEmpty(ip))
        {
            Debug.Log("IP Field is empty");
        }
        NetworkClient.Client.StartMultiplayer(ip);
        UIPlayerRole.instance.SetRole();
    }

    private void OnClickDisconnect()
    {
        NetworkClient.Client.OnDisconnectClicked();
    }
    
    
    
}
