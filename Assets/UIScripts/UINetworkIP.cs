using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using dotenv.net;

public class UINetworkIP : MonoBehaviour
{
    private string SERVER_IP;
    [SerializeField] private TextMeshProUGUI ipAddress;
    [SerializeField] private Button connectButton;
    [SerializeField] private Button disconnectButton;

    private void Start()
    {
        DotEnv.Load();
        ipAddress.text = SERVER_IP;
        connectButton.onClick.AddListener(OnClickConnect);
        disconnectButton.onClick.AddListener(OnClickDisconnect);
        if (string.IsNullOrEmpty(SERVER_IP))
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
