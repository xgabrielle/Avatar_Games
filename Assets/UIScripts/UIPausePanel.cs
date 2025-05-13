using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIPausePanel : MonoBehaviour
{
    [SerializeField] private List<Button> buttons;
    [SerializeField] private GameObject pausePanel;
    private void Start()
    {
        foreach (var btn in buttons)
        {
            btn.onClick.AddListener(() => PausePanel(btn.name));
        }
    }

    void PausePanel(string button)
    {
        pausePanel.SetActive(true);
        switch (button)
        {
            case "Back":
                pausePanel.SetActive(false);
                UIMenu.instance.ActivateButtonByName("ExitButton");
                break;
            case "Restart":
                UINetworkIP.instance.OnRestart();
                break;
            case "Exit":
                if (GameManager.Instance.IsAIMode)
                    SceneManager.LoadScene("MenuScene");
                else
                {
                    UINetworkIP.instance.OnClickDisconnect();
                }
                UIMenu.instance.ResetGame();
                break;
        }
    }
  
}
