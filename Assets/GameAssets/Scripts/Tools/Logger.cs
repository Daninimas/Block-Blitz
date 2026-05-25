using UnityEngine;

namespace GameAssets.Scripts.Tools
{
    
    public static class Log
    {
        #region Trace
        
        private const string TraceColor = "aqua";
        
        /// <summary>
        /// A highlighted output to use when you are following the trace of an error.
        /// Only appears on editor and test builds.
        /// </summary>
        /// <param name="category">Displays more info about the context.</param>
        /// <param name="message">String or object to be converted to string representation for display.</param>
        public static void Trace(string category, object message)
        {
            Debug.Log(FormatMessageWithCategory(category, message, TraceColor));
        }
        
        /// <summary>
        /// A highlighted output to use when you are following the trace of an error.
        /// Only appears on editor and test builds.
        /// </summary>
        /// <param name="category">Displays more info about the context.</param>
        /// <param name="message">String or object to be converted to string representation for display.</param>
        public static void Trace(object message)
        {
            Debug.Log($"<color={TraceColor}> {message}</color>");
        }

        #endregion
        
        #region Info
        
        /// <summary>
        /// Outputs info about what is happening. Only appears on editor and test builds.
        /// </summary>
        /// <param name="category">Displays more info about the context.</param>
        /// <param name="message">String or object to be converted to string representation for display.</param>
        public static void Info(string category, object message)
        {
            Debug.Log(FormatMessageWithCategory(category, message));
        }

        #endregion

        #region Warning

        /// <summary>
        /// Warnings something that may be unexpected. Only appears on editor and test builds.
        /// </summary>
        /// <param name="category">Displays more info about the context.</param>
        /// <param name="message">String or object to be converted to string representation for display.</param>
        public static void Warning(string category, object message)
        {
            Debug.LogWarning(FormatMessageWithCategory(category, message));
        }

        #endregion

        #region Error

        /// <summary>
        /// Alerts of a fail.
        /// </summary>
        /// <param name="category">Displays more info about the context.</param>
        /// <param name="message">String or object to be converted to string representation for display.</param>
        public static void Error(string category, object message)
        {
            Debug.LogError(FormatMessageWithCategory(category, message));
        }

        #endregion


        #region Helpers
        
        private static string FormatMessageWithCategory(string category, object message, string color = "")
        {
            if (color == "")
                return $"<b>[{category}]</b> {message}";
            
            return $"<color={color}><b>[{category}]</b> {message}</color>";
        }

        #endregion
    }
}