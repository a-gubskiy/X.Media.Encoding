using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;

namespace X.Media.Encoding
{
    public class Encoder
    {
        public string FFmpeg2TheoraPath { get; set; }
        public string FFmpegPath { get; set; }
        public string HandBrakePath { get; set; }
        public string Outpoot { get; private set; }

        public Encoder()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var directory = Path.GetDirectoryName(assembly.Location);

            FFmpegPath = Path.Combine(directory, "ffmpeg.exe");
            FFmpeg2TheoraPath = Path.Combine(directory, "ffmpeg2theora.exe");
            HandBrakePath = Path.Combine(directory, "HandBrakeCLI.exe");

            if (!File.Exists(FFmpegPath))
            {
                File.WriteAllBytes(FFmpegPath, X.Media.Encoding.Properties.Resources.ffmpeg);
            }

            if (!File.Exists(FFmpeg2TheoraPath))
            {
                File.WriteAllBytes(FFmpeg2TheoraPath, X.Media.Encoding.Properties.Resources.ffmpeg2theora);
            }

            if (!File.Exists(HandBrakePath))
            {
                File.WriteAllBytes(HandBrakePath, X.Media.Encoding.Properties.Resources.HandBrakeCLI);
            }
            
        }

        public bool EncodeVideo(string inputFile, Format format, Quality quality, string outputFile)
        {
            try
            {
                var resolution = String.Empty;
                var videoBitRate = String.Empty;

                if (quality == Quality.Low)
                {
                    resolution = "320x180";
                    videoBitRate = "300k";
                }
                if (quality == Quality.Medium)
                {
                    resolution = "640x360";
                    videoBitRate = "600k";
                }
                if (quality == Quality.High)
                {
                    resolution = "1280x720";
                    videoBitRate = "1024k";
                }

                if (format == Format.Mp4)
                {
                    if (quality == Quality.Low)
                    {
                        var arguments = String.Format("--preset \"{2}\" --turbo --optimize --input {0} --output {1}",
                                                      inputFile, outputFile, "iPhone & iPod Touch");
                        Outpoot = RunProcess(HandBrakePath, arguments);
                    }


                    if (quality == Quality.Medium || quality == Quality.High)
                    {
                        var arguments = String.Format("-i {0} -ab 128k -ac 2 -vcodec libx264 -s {1} -crf 22 -threads 0 -b {3} -strict experimental  {2}", inputFile, resolution, outputFile, videoBitRate);
                        Outpoot = RunProcess(FFmpegPath, arguments);
                    }
                }

                if (format == Format.Ogg)
                {
                    var arguments = String.Format("--videoquality 5 --audioquality 1 --max_size {0} {1}", resolution,
                                                  inputFile);
                    Outpoot = RunProcess(FFmpeg2TheoraPath, arguments);

                    var ogvTempFile = Path.GetDirectoryName(inputFile) + "\\" +
                                      Path.GetFileNameWithoutExtension(inputFile) + ".ogv";
                    File.Copy(ogvTempFile, outputFile);
                    File.Delete(ogvTempFile);
                }

                if (format == Format.Webm)
                {
                    var arguments = String.Format("-i {0} -f webm -vcodec libvpx -acodec libvorbis -b {3} -r 25 -s {1} -ar 44100 -ab 128k -ac 2 -y {2}", inputFile, resolution, outputFile, videoBitRate);
                    Outpoot = RunProcess(FFmpegPath, arguments);
                }
            }
            catch (Exception ex)
            {
                Outpoot += "\n" + ex.Message;
            }

            return File.Exists(outputFile);
        }

        private static string RunProcess(string executablePath, string parameters)
        {
            //create a process info
            var processStartInfo = new ProcessStartInfo(executablePath, parameters);
            processStartInfo.UseShellExecute = false;
            processStartInfo.CreateNoWindow = false;
            processStartInfo.RedirectStandardOutput = false;
            processStartInfo.RedirectStandardError = false;


            //Create the output and streamreader to get the output
            var output = new StringBuilder();

            output.AppendFormat("Operation start at: {0} {1}\r\n", DateTime.Now.ToLongDateString(), DateTime.Now.ToLongTimeString());
            output.AppendFormat("{0} {1}\r\n", executablePath, parameters);

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
                        process.PriorityClass = ProcessPriorityClass.BelowNormal;
                    }
                    if (!process.HasExited)
                    {
                        process.ProcessorAffinity = (IntPtr)1;
                    }

                    process.WaitForExit();
                }
                catch
                {
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