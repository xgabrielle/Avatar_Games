using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class EnvironmentLoader : MonoBehaviour
{
    public static Dictionary<string, string> Variables { get; private set; } = new();

    private void Awake()
    {
        string envPath = Path.Combine(Application.streamingAssetsPath, ".env");

        if (!File.Exists(envPath))
        {
            Debug.LogError($".env file not found at: {envPath}");
            return;
        }

        foreach (string line in File.ReadAllLines(envPath))
        {
            if (line.StartsWith("#") || !line.Contains("=")) continue;

            var parts = line.Split('=');
            if (parts.Length < 2) continue;

            string key = parts[0].Trim();
            string value = parts[1].Trim();

            Variables[key] = value;
        }
        
    }

    public static string Get(string key)
    {
        return Variables.TryGetValue(key, out string value) ? value : null;
    }
}