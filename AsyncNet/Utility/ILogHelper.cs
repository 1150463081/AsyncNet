using System;
using System.Collections.Generic;
using System.Text;

namespace AsyncNet
{
    public interface ILogHelper
    {
        void Log(string str, params object[] args);
        void ColorLog(LogColor color, string str, params object[] args);
        void LogWarn(string str, params object[] args);
        void LogError(string str, params object[] args);
    }
}
 