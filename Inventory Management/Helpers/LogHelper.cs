using NLog;

namespace Inventory_Management.Helpers
{
    //Use of NLog library to log info/error to text file- Refer to NLog.config!
    //Singleton LogHelper class
    public class LogHelper : ILogHelper
    {

        private static LogHelper instance; //single instance of this class.
        private static Logger logger; //static variable to hold the single instnace of NLog logger

        //private constructor- singleton design pattern
        private LogHelper() { }

        //just to check if the LogHelper instance is present already. if not, create one
        public static LogHelper GetInstance()
        {
            if (instance == null) 
                instance = new LogHelper();
            return instance;
        }

        private Logger GetLogger(string _logger)
        {
            if (logger == null)
                logger = LogManager.GetLogger(_logger);
            return logger;
        }

        //to log Error messages
        public void Error(string message, string arg = null)
        {
            switch (arg)
            {
                case null:
                    GetLogger("inventoryLoggerRule").Error(message);
                    break;
                default:
                    GetLogger("inventoryLoggerRule").Error(message, arg);
                    break;
            }
        }

        //to log informational messages
        public void Info(string message, string arg = null)
        {
            switch (arg)
            {
                case null:
                    GetLogger("inventoryLoggerRule").Info(message);
                    break;
                default:
                    GetLogger("inventoryLoggerRule").Info(message, arg);
                    break;
            }
        }
    }
}