namespace LogTest
{
    using LogTest.TimeProviders;
    using System;
    using System.Collections.Concurrent;
    using System.Threading;

    public partial class AsyncLog : ILog, IDisposable
    {
        private Thread _runThread;
        private ConcurrentQueue<LogLine> _linesQueue;

        private ILogWriter _writer; 

        private bool _exit;

        private bool _QuitWithFlush;

        private bool _isQueueOpen; 

        public AsyncLog(ILogWriter logWriter)
        {
            _QuitWithFlush = false;
            _isQueueOpen = true;
            _linesQueue = new ConcurrentQueue<LogLine>();

            this._writer = logWriter;

            this._runThread = new Thread(this.MainLoop);
            this._runThread.Start();
        }



        private void MainLoop()
        {
            while (_writer != null && !this._exit)
            {
                while (_linesQueue.Count > 0)
                {
                    if (!_linesQueue.TryDequeue(out var logLine))
                        continue;

                    if (this._exit && !this._QuitWithFlush)
                        break;
                    try
                    {
                        this._writer.Write(logLine);
                    }
                    catch(Exception e)
                    {
                    }

                    if (this._QuitWithFlush == true && this._linesQueue.Count == 0)
                        this._exit = true;

                    Thread.Sleep(50);
                }
            }
        }

     
        public void StopWithoutFlush()
        {
            this._exit = true;
            _isQueueOpen = false;
        }

        public void StopWithFlush()
        {
            this._QuitWithFlush = true;
            _isQueueOpen = false;
        }

        public void Write(string text)
        {
            try
            {
                if (_isQueueOpen)
                    this._linesQueue.Enqueue(new FileLogLine(TimeProvider.Current.UtcNow, text));
            }
            catch
            {
                //dont throw on error, continue
                Dispose();
            }

        }

        public void Dispose()
        {
            _writer.Dispose();
        }
    }
}