using System;

namespace LogTest
{
    public interface ILogWriter : IDisposable
    {
        void Write(LogLine logLine);
    }
}
