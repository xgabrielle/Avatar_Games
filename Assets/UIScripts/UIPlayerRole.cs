using System;
using System.Security;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UIPlayerRole : MonoBehaviour
{
    public static UIPlayerRole instance { get; private set; }
    [SerializeField] private TextMeshProUGUI roleText;

    private void Start()
    {
        if (instance == null) instance = this;
        else Destroy(instance);
        
    }

    internal void SetRole()
    {
        if (GameMode.LocalPlayer == GameManager.Instance.currentGameMode)
        {
            string role = RoleManager.Role;
            roleText.text = $"You are:\n{role}";
        }
        else
        {
            roleText.text = "You are:\nPlaying with AI";
        }
    }
}
