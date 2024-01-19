using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuiDuiDui
{
    public class CallClass
    {
        // CallClass var

        // to store the 3 numbers
        string callNum;
        // to store the name
        string callName;

        public CallClass()
        {
            // default ctor
        }

        // getters and setters
        public string CallNum { get => callNum; set => callNum = value; }
        public string CallName { get => callName; set => callName = value; }
    }
}
