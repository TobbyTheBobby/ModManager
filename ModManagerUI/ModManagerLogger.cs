using ModManager.LoggingSystem;
using System;
using ModManager;
using UnityEngine;

namespace ModManagerUI
{
    public class ModManagerLogger : Singleton<ModManagerLogger>, IModManagerLogger
    {
        public event EventHandler<LoggingEventEventArgs> LoggingEvent;
        
        public void LogInfo(string message)
        {
            Debug.Log(message);
        }
        
        public void LogWarning(string message)
        {
            Debug.LogWarning(message);
        }
        
        public void LogError(string message)
        {
            Debug.LogError(message);
        }
    }
}
