using System;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using dotenv.net;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class UINetworkIP : MonoBehaviour
{
    private string _serverIP;
    [SerializeField] private TMP_InputField ipAddress;
    [SerializeField] private Button connectButton;
    [SerializeField] private Button disconnectButton;
    [SerializeField] private Button restartButton;
    
    private void Awake()
    {
        _serverIP = EnvironmentLoader.Get("SERVER_IP");
        Debug.Log("Loaded  ServerIP: " + _serverIP); 
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        Debug.Log("Loaded ServerIP: " + _serverIP); 
        ipAddress.text = _serverIP;
        connectButton.onClick.AddListener(OnClickConnect);
        disconnectButton.onClick.AddListener(OnClickDisconnect);
        restartButton.onClick.AddListener(OnRestart);
        
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
        disconnectButton.gameObject.SetActive(true);
    }

    private void OnClickDisconnect()
    {
        if (GameManager.Instance.IsLocalPlayerMode)
        {
            NetworkClient.Client.OnDisconnectClicked();
        }
        SceneManager.LoadScene("MenuScene");
    }

    private void OnRestart()
    {
        if (GameManager.Instance.IsLocalPlayerMode)
        {
            NetworkClient.Client.OnDisconnectClicked();
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    
    
}
