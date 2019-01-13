
using System;
using System.IO;
using System.Reflection;
using System.Threading;

namespace FileLock
{
    public class Program
    {
        static private string[] _args;

        static private string lockFile = "./filelock.txt";

        static void Main(string[] args)
        {
            WriteLog($@"Start FileLock v {Assembly.GetExecutingAssembly().GetName().Version}");

            _args = args;

            if (_args.Length < 2)
            {
                WriteLog($@"Argument is missing. Expected -[lock|free] -[key]");
            }

            var action = _args[0].Normalize();
            var key = _args[1].Normalize();

            ChangeLock(action, key);
        }


        private static int iAmount = 1;
        private static int iAmountMax = 5;

        private static void ChangeLock(string action, string key)
        {
            try
            {
                if (iAmount > iAmountMax)
                {
                    WriteLog($@"try to do {action} on {lockFile}. iAmount={iAmount}, it is greater than {iAmountMax}. Exit. No lock-change is done");
                }

                action = action.NormalizeX();
                key = key.NormalizeX();
                var strLock = $@"LOCK BY {key}".NormalizeX();
                var strFree = "FREE".NormalizeX();

                var expectedContent = strFree;

                if (action == strFree)
                {
                    expectedContent = strLock;
                }
                else
                {
                    action = strLock;
                }

                WriteLog($@"try to do '{action}' on '{lockFile}'. Expected content is '{expectedContent}'. iAmount={iAmount}");

                using (var fs = new FileStream(lockFile, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                {
                    string content;
                    using (StreamReader reader = new StreamReader(fs))
                    {
                        content = reader.ReadToEnd();

                        content = content.NormalizeX();

                        if (content == expectedContent || content == strFree)
                        {
                            using (StreamWriter writer = new StreamWriter(fs))
                            {
                                fs.SetLength(0);
                                writer.Write(action);
                                WriteLog($@"Action '{action}' is OK");
                            }
                        }
                        else
                        {
                            throw new Exception($@"File '{lockFile}' has not expected content : '{content}'");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLog($@"Error: {ex.Message}");
                WriteLog($@"Wait 30 sec and repeat");
                Thread.Sleep(1000 * 30);
                iAmount++;
                ChangeLock(action, key);
            }

        }


        private static void WriteLog(string str)
        {
            Console.WriteLine(str);
            System.Diagnostics.Trace.TraceInformation(str);
        }
    }

    public static class Helper
    {
        public static string NormalizeX(this string value)
        {
            return value.Trim().ToUpper();
        }
    }
}
