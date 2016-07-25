using System;
using NLog;

namespace ViewModel
{
    public class MessageDialogViewModel
    {
        public void WriteLog(string message, Exception ex)
        {
            Logger log = LogManager.GetLogger(string.Empty);
            log.Error(ex, message, null);
        }
    }
}