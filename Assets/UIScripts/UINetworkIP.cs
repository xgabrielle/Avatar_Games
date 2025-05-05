using System;
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

    private void Start()
    {
        DotEnv.Load();
        _serverIP = Environment.GetEnvironmentVariable("SERVER_IP");
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
    }

    private void OnClickDisconnect()
    {
        NetworkClient.Client.OnDisconnectClicked();
    }
    
    
    
}
