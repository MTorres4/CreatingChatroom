using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatroom
{
    class Log : ILoggable
    {
        private List<string> messagelog = new List<string>();
        public List<string> MessageLog
        {
            get
            {
                return messagelog;
            }
            set
            {
                messagelog = value;
            }
        }
        public Log()
        {

        }
        public void WriteTo(string input)
        {
            MessageLog.Add(input);
        }
    }
}
