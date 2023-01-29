using System;
using System.Text;

namespace LogTest
{
    internal class FileLogLine : LogLine
    {

        public FileLogLine(DateTime timestamp, string text = "")
        {
            Text = text;
            Timestamp = timestamp;
        }

        protected override string CreateLineText()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(Timestamp.ToString("yyyy-MM-dd HH:mm:ss:fff"));
            sb.Append("\t");
            sb.Append(Text);
            sb.Append("\t");
            sb.Append(Environment.NewLine);

            return sb.ToString();
        }



    }
}
