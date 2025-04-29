using System.IO;
using System.Text;
using System.Diagnostics;

public enum ELogLevel
{
    Log,
    Warning,
    Error
}

public class Logger
{
    private static StringBuilder stringBuilder = new StringBuilder(200);
    private static StackTrace stackTrace = new StackTrace();

    public static void Log(string message, ELogLevel level = ELogLevel.Log)
    {
        StackFrame frame = stackTrace.GetFrame(1);
        var method = frame.GetMethod();

        stringBuilder.Append($"{method.DeclaringType.Name}.{method.Name} ");
        stringBuilder.Append($"{message} \n");

        switch (level)
        {
            case ELogLevel.Log:
                UnityEngine.Debug.Log(stringBuilder.ToString());
                break;
            case ELogLevel.Warning:
                UnityEngine.Debug.LogWarning(stringBuilder.ToString());
                break;
            case ELogLevel.Error:
                UnityEngine.Debug.LogError(stringBuilder.ToString());
                break;
        }

        stringBuilder.Clear();
    }

    public static void FileLog(string message, ELogLevel level = ELogLevel.Log)
    {
        string todayDate = System.DateTime.Now.ToString("yyyyMMdd");
        string directoryPath = Path.Combine("Logs", "Z1Log");
        string filePath = Path.Combine(directoryPath, $"z1_log_{todayDate}.txt");

        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        StackFrame frame = stackTrace.GetFrame(1);
        var method = frame.GetMethod();

        stringBuilder.Append($"[{System.DateTime.Now}] ");
        stringBuilder.Append($"[{level}] ");
        stringBuilder.Append($"{method.DeclaringType.Name}.{method.Name} ");
        stringBuilder.Append($"({frame.GetFileLineNumber()}) ");
        stringBuilder.Append($"{message} \n");

        System.IO.File.AppendAllText(filePath, stringBuilder.ToString());

        stringBuilder.Clear();
    }
}