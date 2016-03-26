using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace X.Media.Encoding
{
    internal class ProcessManager
    {
        public static string RunProcess(string fileNmae, string arguments)
        {
            //create a process info
            var processStartInfo = new ProcessStartInfo
            {
                UseShellExecute = false,
                CreateNoWindow = false,
                RedirectStandardOutput = false,
                RedirectStandardError = false,
                FileName = fileNmae,
                Arguments = arguments
            };

            //Create the output and streamreader to get the output
            var output = new StringBuilder();

            output.AppendFormat("Operation start at: {0} {1}\r\n", DateTime.Now.ToLongDateString(), DateTime.Now.ToLongTimeString());
            output.AppendFormat("{0} {1}\r\n", fileNmae, arguments);

            StreamReader outputStreamReader = null;

            //try the process
            try
            {
                //run the process
                var process = new Process();
                process.StartInfo = processStartInfo;

                process.Start();

                try
                {
                    if (!process.HasExited)
                    {
                        process.PriorityClass = ProcessPriorityClass.Normal;
                    }
                    if (!process.HasExited)
                    {
                        process.ProcessorAffinity = (IntPtr)1;
                    }

                    process.WaitForExit();
                }
                catch (Exception ex)
                {
                    output.AppendLine(String.Format("Error: {0}", ex.Message));
                }

                if (processStartInfo.RedirectStandardOutput)
                {
                    //get the output
                    outputStreamReader = process.StandardError;

                    //now put it in a string
                    output.AppendLine(outputStreamReader.ReadToEnd());
                }

                process.Close();
            }
            catch (Exception ex)
            {
                output.AppendLine("\r\nError:");
                output.AppendLine(ex.Message);
                output.AppendLine("\r\n");
            }
            finally
            {
                //now, if we succeded, close out the streamreader
                if (outputStreamReader != null)
                {
                    outputStreamReader.Close();
                    outputStreamReader.Dispose();
                }
            }

            output.AppendFormat("Operation end at: {0} {1}\r\n", DateTime.Now.ToLongDateString(), DateTime.Now.ToLongTimeString());
            output.AppendLine("----------------------------------------------------------------------------------------------------------------");


            return output.ToString();
        }
    }
}
