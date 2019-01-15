using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace SM64RAM
{
    public interface MessageStream
    {
        void AppendMessage(string Message, string type);
    }
    public class MessageBoxStream : MessageStream
    {
        public bool showWarnings = false;
        public void AppendMessage(string message, string type)
        {
            if (type == "Error")
                System.Windows.Forms.MessageBox.Show(message, type, MessageBoxButtons.OK, MessageBoxIcon.Error);
            else if (showWarnings || type != "Warning")
                System.Windows.Forms.MessageBox.Show(message, type);
        }
    }
    public class MessageHistoryStream : MessageStream
    {
        public class Message
        {
            string message, type;
            public Message(string message, string type) { this.message = message; this.type = type; }
        }
        public List<Message> messages = new List<Message>();
        public void AppendMessage(string message, string type) { messages.Add(new Message(message, type)); }
    }
}
