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

            var avifil32Path = Path.Combine(directory, "avifil32.dll");

            CheckFile(FFmpegPath, X.Media.Encoding.Properties.Resources.ffmpeg);
            CheckFile(FFmpeg2TheoraPath, X.Media.Encoding.Properties.Resources.ffmpeg2theora);
            CheckFile(HandBrakePath, X.Media.Encoding.Properties.Resources.HandBrakeCLI);
            CheckFile(avifil32Path, X.Media.Encoding.Properties.Resources.avifil32);
        }

        private static void CheckFile(string path, byte[] file)
        {
            if (!File.Exists(path))
            {
                File.WriteAllBytes(path, file);
            }
        }

        public bool EncodeVideo(String inputFile, Format format, Quality quality, String outputFile, int autodBitRate = 128)
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

                if (quality == Quality.VeryHigh)
                {
                    resolution = "1920x1080";
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

                    if (quality == Quality.Medium || quality == Quality.High || quality == Quality.VeryHigh)
                    {
                        var arguments = String.Format("-i {0} -ab {4}k -ac 2 -vcodec libx264 -s {1} -crf 22 -threads 0 -b {3} -strict experimental -movflags +faststart {2}",
                            inputFile, resolution, outputFile, videoBitRate, autodBitRate);

                        Outpoot = RunProcess(FFmpegPath, arguments);
                    }
                }
                else if (format == Format.Ogg)
                {
                    var arguments = String.Format("--videoquality 5 --audioquality 1 --max_size {0} {1}", resolution, inputFile);

                    Outpoot = RunProcess(FFmpeg2TheoraPath, arguments);

                    var ogvTempFile = Path.GetDirectoryName(inputFile) + "\\" +
                                      Path.GetFileNameWithoutExtension(inputFile) + ".ogv";

                    File.Copy(ogvTempFile, outputFile);
                    File.Delete(ogvTempFile);
                }
                else if (format == Format.Webm)
                {
                    var arguments = String.Format("-i {0} -f webm -vcodec libvpx -acodec libvorbis -b {3} -r 25 -s {1} -ar 44100 -ab {4}k -ac 2 -y {2}", inputFile, resolution, outputFile, videoBitRate, autodBitRate);
                    Outpoot = RunProcess(FFmpegPath, arguments);
                }
            }
            catch (Exception ex)
            {
                Outpoot += "\n" + ex.Message;
            }

            return File.Exists(outputFile);
        }

        private static string RunProcess(string fileNmae, string arguments)
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
                        process.PriorityClass = ProcessPriorityClass.BelowNormal;
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

        public static string GetFormatExtension(Format format)
        {
            switch (format)
            {
                case Format.Webm: return ".webm";
                case Format.Ogg: return ".ogg";
                case Format.Mp4: return ".mp4";
                default: throw new Exception("Unknown video fomat");
            }
        }
    }
}
