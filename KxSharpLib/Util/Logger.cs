using System;
using System.Drawing;
using System.Windows.Forms;
using FastColoredTextBoxNS;

namespace KxSharpLib.Utility
{

    public class Logger
    {
        public FastColoredTextBox LoggingFCTB { get; set; }
        public TextStyle defaultStyle = new TextStyle(Brushes.Gray, null, FontStyle.Regular);
        public TextStyle infoStyle = new TextStyle(Brushes.YellowGreen, null, FontStyle.Regular);
        public TextStyle warningStyle = new TextStyle(Brushes.OrangeRed, null, FontStyle.Regular);
        public TextStyle errorStyle = new TextStyle(Brushes.Red, null, FontStyle.Regular);

        public Logger(FastColoredTextBox fctb) => LoggingFCTB = fctb;

        public void WriteInfo(string text)
        {
            Write(text, infoStyle);
        }

        public void WriteWarning(string text)
        {
            Write(text, warningStyle);
        }

        public void WriteError(string text)
        {
            Write(text, errorStyle);
        }

        public void Write(string text)
        {
            Write(text, defaultStyle);
        }

        public void Write(string text, Style style)
        {
            string dateTime = DateTime.Now.ToString("hh:mm:ss");
            LoggingFCTB.BeginUpdate();
            LoggingFCTB.Selection.BeginUpdate();

            var userSelection = LoggingFCTB.Selection.Clone();

            LoggingFCTB.TextSource.CurrentTB = LoggingFCTB;

            if (LoggingFCTB.Text.Length != 0)
                LoggingFCTB.AppendText(Environment.NewLine);

            LoggingFCTB.AppendText(string.Format("[{0} >> {1}]", dateTime, text), style);

            if (!userSelection.IsEmpty || userSelection.Start.iLine < LoggingFCTB.LinesCount - 2)
            {
                LoggingFCTB.Selection.Start = userSelection.Start;
                LoggingFCTB.Selection.End = userSelection.End;
            }
            else LoggingFCTB.GoEnd();

            LoggingFCTB.Selection.EndUpdate();
            LoggingFCTB.EndUpdate();
        }
    }




    public class LoggerRtb
    {
        public RichTextBox LoggingRTB { get; set; }
        public enum EType : byte
        {
            INFO,
            WARNING,
            ERROR,
        }
        public LoggerRtb(RichTextBox rtb) => LoggingRTB = rtb;
        public void Write(Color TextColor, string Text)
        {
            if (LoggingRTB.InvokeRequired)
            {
                LoggingRTB.BeginInvoke(new Action(delegate
                {
                    Write(TextColor, Text);
                }));
                return;
            }

            string nDateTime = DateTime.Now.ToString("[hh:mm:ss tt]  >>  ");
            LoggingRTB.SelectionStart = LoggingRTB.Text.Length;
            LoggingRTB.SelectionColor = TextColor;

            if (LoggingRTB.Lines.Length == 0)
            {
                LoggingRTB.AppendText(nDateTime + Text);
                LoggingRTB.ScrollToCaret();
                LoggingRTB.AppendText(Environment.NewLine);
            }
            else
            {
                LoggingRTB.AppendText(nDateTime + Text + Environment.NewLine);
                LoggingRTB.ScrollToCaret();
            }
        }
        public void Write(EType type, string text)
        {
            switch (type)
            {
                case EType.INFO:
                    Write(Color.Yellow, text);
                    break;
                case EType.WARNING:
                    Write(Color.Orange, text);
                    break;
                case EType.ERROR:
                    Write(Color.Red, text);
                    break;
                default:
                    Write(text);
                    break;
            }
        }
        public void Write(string Text) { Write(Color.Gray, Text); }
    }
}
