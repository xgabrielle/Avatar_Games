using System;
using System.Collections.Concurrent;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;

public class MainThreadDispatcher : MonoBehaviour
{
    private static MainThreadDispatcher instance;
    private static ConcurrentQueue<Action> _actions = new();

    public static void Run(Action action)
    {
        if (action == null) return;
        _actions.Enqueue(action);
    }

    private void Awake()
    {
        if (instance == null && instance == this) instance = this;
        else Destroy(gameObject);
    }

    private void Update()
    {
        while (_actions.TryDequeue(out var action))
        {
            action.Invoke();
        }
    }
}
