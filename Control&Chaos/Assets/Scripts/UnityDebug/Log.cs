using Conditional=System.Diagnostics.ConditionalAttribute;
using UnityEngine;


namespace Dmdrn.UnityDebug
{
    public static class Log
    {
        [Conditional("DMDRN_DEBUG")]
        public static void Message(string message)
        {
            Debug.Log(message);
        }


        [Conditional("DMDRN_DEBUG")]
        public static void MessageFormat(string format, params object[] args)
        {
            Debug.LogFormat(format, args);
        }


        [Conditional("DMDRN_DEBUG")]
        public static void Warning(string message)
        {
            Debug.LogWarning(message);
        }


        [Conditional("DMDRN_DEBUG")]
        public static void WarningFormat(string format, params object[] args)
        {
            Debug.LogWarningFormat(format, args);
        }


        [Conditional("DMDRN_DEBUG")]
        public static void Error(string message)
        {
            Debug.LogError(message);
        }


        [Conditional("DMDRN_DEBUG")]
        public static void ErrorFormat(string format, params object[] args)
        {
            Debug.LogErrorFormat(format, args);
        }


        [Conditional("DMDRN_DEBUG")]
        public static void Exception(System.Exception exception)
        {
            Debug.LogException(exception);
        }
    }
}
