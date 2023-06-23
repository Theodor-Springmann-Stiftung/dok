namespace MusenalmAPI;

public sealed class LogSink : IDisposable {

    private static readonly Lazy<LogSink> lazy = new Lazy<LogSink>(() => new LogSink());
    public static LogSink Instance { get { return lazy.Value; } }
    private static bool _writeToConsole = true;
    private static StreamWriter? _file = null;


    public void SetFile(string? filepath) {
        if (filepath != null) {
            if (!File.Exists(filepath))  {
                _file = File.CreateText(filepath);
            } else {
                _file = File.AppendText(filepath);
                _file.WriteLine();
            }
            _file.AutoFlush = true;
            _file.WriteLine(DateTime.Now + " " + "Log MusenalmDB");
        }
        else {
            if (_file != null) {
                _file.Flush();
                _file.Close();
                _file = null;
            }
        }
    }

    public void LogLine(string message) {
        if (_file != null) 
            _file.WriteLine(DateTime.Now + " " + message);
        if (_writeToConsole)
            Console.WriteLine(DateTime.Now + " " + message);
    }

    public void WriteToConsole(bool writeToConsole) {
        _writeToConsole = writeToConsole;
    }

    public void Close() {
        if (_file != null) {
            _file.Flush();
            _file.Close();
        }
        _writeToConsole = false;
    }

    public void Dispose() {
        Close();
    }
}