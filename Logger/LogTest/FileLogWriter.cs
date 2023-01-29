using LogTest.TimeProviders;
using System;
using System.IO;

namespace LogTest
{
    public class FileLogWriter: IDisposable, ILogWriter
    {
        private StreamWriter _writer;
        private string _logLocation;
        private DateTime _curDate;


        public FileLogWriter(string logLocation)
        {
            _logLocation = logLocation;
            _curDate = TimeProvider.Current.UtcNow;

            try
            {
                if (!Directory.Exists(_logLocation))
                    Directory.CreateDirectory(_logLocation);

                _writer = File.AppendText(CreateLogFileName());
                _writer.AutoFlush = true;
                WriteHeader();
            }
            catch
            {

            }
        }

        private StreamWriter CreateLogStreamWriter()
        {
            var writer = File.AppendText(CreateLogFileName());
            writer.AutoFlush = true;
            return writer;
        }

        private void WriteHeader()
        {
            _writer.Write("Timestamp".PadRight(25, ' ') + "\t" + "Data".PadRight(15, ' ') + "\t" + Environment.NewLine);
        }

        public void Write(LogLine logLine)
        {
            if (TimeProvider.Current.UtcNow.Date != _curDate.Date)
            {
                _curDate = TimeProvider.Current.UtcNow;

                lock (_writer)
                {
                    this._writer.Dispose();

                    this._writer = CreateLogStreamWriter();
                    WriteHeader();
                }
            }
            _writer.Write(logLine.LineText());
        }

        private string CreateLogFileName()
        {
            return Path.Combine(_logLocation, "Log") + _curDate.ToString("yyyyMMdd HHmmss fff") + ".log";
        }

        public void Dispose()
        {
            _writer.Dispose();
        }
    }
}
