using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public enum Personality
{
    None = -1,
    Funny = 0,
    Expert = 1
}
public class UIPersonality : MonoBehaviour
{
    public static UIPersonality instance { get; set; }
    private Personality currentPersonality;
    string systemMessage;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(instance);

        DontDestroyOnLoad(gameObject); 
        
    }

    public void SetPersonality(Personality newPersonality)
    {
        currentPersonality = newPersonality;
        switch (newPersonality)
        {
            case Personality.Funny:
                systemMessage = "You're a kind AI that makes a few small jokes during the game and want to get the other player to laugh.";
                //Debug.Log("Funny AI");
                break;
            case Personality.Expert:
                systemMessage = "You are very good a checkers and will not hesitate to give your opinion on your components move. You start with an intimidating comment.";
                //Debug.Log("Expert AI");
                break;
        }
    }
    
    internal string SetGameContext()
    {
        return systemMessage + GameManager.Instance.SetGame();
    }
    
}
