using UnityEngine;
using TMPro;

public class GameOverUI : MonoBehaviour
{
    public static GameOverUI instance;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TMP_Text gameOverText;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public void UIGameOver(string message)
    {
        gameOverText.text = message;
        gameOverPanel.SetActive(true);
    }

    public void HideGameOver()
    {
        gameOverPanel.SetActive(false);
    }
} 