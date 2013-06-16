namespace X.Media.Encoding
{
    public struct Resolution
    {
        /// <summary>
        /// 128x96
        /// </summary>
        public const string SQCIF = "sqcif";

        /// <summary>
        /// 176x144
        /// </summary>
        public const string QCIF = "qcif";

        /// <summary>
        /// 320x240
        /// </summary>
        public const string QVGA = "qvga";

        /// <summary>
        /// 352x288
        /// </summary>
        public const string CIF = "cif";

        /// <summary>
        /// 640x480
        /// </summary>
        public const string VGA = "vga";

        /// <summary>
        /// 
        /// </summary>
        public const string SVGA = "svga";
    }

    /// <summary>
    /// Ready made encoding commands for FFmpeg
    /// Use when calling EncodeVideo commands as string encodingCommand
    /// Add remove as you like
    /// </summary>
    public class QuickVideoEncodingCommands
    {
        //-b
        static string LQVideoBitrate = "256k";
        static string MQVideoBitrate = "512k";
        static string HQVideoBitrate = "756k";
        static string VHQVideoBitrate = "1024k";
       
        //-ab 
        static string LQAudioBitrate = "32k";
        static string MQAudioBitrate = "64k";
        static string HQAudioBitrate = "96k";
        static string VHQAudioBitrate = "128k";
        
        //-ar
        static string LQAudioSamplingFrequency = "22050";
        static string MQAudioSamplingFrequency = "44100";
        static string HQAudioSamplingFrequency = "44100";
       
        //flv
        public static string FLVLowQualityQCIF = string.Format("-y -b {0} -ab {1} -ar {2} -s {3} -f flv", LQVideoBitrate, LQAudioBitrate, LQAudioSamplingFrequency, Resolution.QVGA);
        public static string FLVMediumQualityCIF = string.Format("-y -b {0} -ab {1} -ar {2} -s {3} -f flv", MQVideoBitrate, MQAudioBitrate, MQAudioSamplingFrequency, Resolution.CIF);
        public static string FLVHighQualityVGA = string.Format("-y -b {0} -ab {1} -ar {2} -s {3} -f flv", HQVideoBitrate, HQAudioBitrate, HQAudioSamplingFrequency, Resolution.VGA);
        public static string FLVVeryHighQualitySVGA = string.Format("-y -b {0} -ab {1} -ar {2} -s {3} -f flv", VHQVideoBitrate, VHQAudioBitrate, HQAudioSamplingFrequency, Resolution.SVGA);

        public static string FLVLowQualityKeepOriginalSize = string.Format("-y -b {0} -ab {1} -ar {2} -f flv", LQVideoBitrate, LQAudioBitrate, LQAudioSamplingFrequency, Resolution.QVGA);
        public static string FLVMediumQualityKeepOriginalSize = string.Format("-y -b {0} -ab {1} -ar {2} -f flv", MQVideoBitrate, MQAudioBitrate, MQAudioSamplingFrequency, Resolution.CIF);
        public static string FLVHighQualityKeepOriginalSize = string.Format("-y -b {0} -ab {1} -ar {2} -f flv", HQVideoBitrate, HQAudioBitrate, HQAudioSamplingFrequency, Resolution.VGA);
        public static string FLVVeryHighQualityKeepOriginalSize = string.Format("-y -b {0} -ab {1} -ar {2} -f flv", VHQVideoBitrate, VHQAudioBitrate, HQAudioSamplingFrequency, Resolution.SVGA);

        //3gp
        public static string THREEGPLowQualitySQCIF = string.Format("-y -acodec aac -ac 1 -b {0} -ab {1} -ar {2} -s {3} -f 3gp", LQVideoBitrate, LQAudioBitrate, LQAudioSamplingFrequency, Resolution.SQCIF);
        public static string THREEGPMediumQualityQCIF = string.Format("-y -acodec aac -b {0} -ab {1} -ar {2} -s {3} -f 3gp", MQVideoBitrate, MQAudioBitrate, MQAudioSamplingFrequency, Resolution.QCIF);
        public static string THREEGPHighQualityCIF = string.Format("-y -acodec aac -b {0} -ab {1} -ar {2} -s {3} -f 3gp", VHQVideoBitrate, VHQAudioBitrate, HQAudioSamplingFrequency, Resolution.CIF);
        //mp4
        public static string MP4LowQualityKeepOriginalSize = string.Format("-y -b {0} -ab {1} -ar {2} -f mp4", LQVideoBitrate, LQAudioBitrate, LQAudioSamplingFrequency, Resolution.QVGA);
        public static string MP4MediumQualityKeepOriginalSize = string.Format("-y -b {0} -ab {1} -ar {2} -f mp4", MQVideoBitrate, MQAudioBitrate, MQAudioSamplingFrequency, Resolution.CIF);
        public static string MP4HighQualityKeepOriginalSize = string.Format("-y -b {0} -ab {1} -ar {2} -f mp4", HQVideoBitrate, HQAudioBitrate, HQAudioSamplingFrequency, Resolution.VGA);

        public static string MP4LowQualityQVGA = string.Format("-y -b {0} -ab {1} -ar {2} -s {3} -f mp4", LQVideoBitrate, LQAudioBitrate, LQAudioSamplingFrequency, Resolution.QVGA);
        public static string MP4MediumQualityCIF = string.Format("-y -b {0} -ab {1} -ar {2} -s {3} -f mp4", MQVideoBitrate, MQAudioBitrate, MQAudioSamplingFrequency, Resolution.CIF);
        public static string MP4HighQualityVGA = string.Format("-y -b {0} -ab {1} -ar {2} -s {3} -f mp4", HQVideoBitrate, HQAudioBitrate, HQAudioSamplingFrequency, Resolution.VGA);

        //WMV
        public static string WMVLowQualityQVGA = string.Format("-y -vcodec wmv2  -acodec wmav2 -b {0} -ab {1} -ar {2} -s {3}", LQVideoBitrate, LQAudioBitrate, LQAudioSamplingFrequency, Resolution.QVGA);
        public static string WMVMediumQualityCIF = string.Format("-y -vcodec wmv2  -acodec wmav2 -b {0} -ab {1} -ar {2} -s {3}", MQVideoBitrate, MQAudioBitrate, MQAudioSamplingFrequency, Resolution.CIF);
        public static string WMVHighQualityVGA = string.Format("-y -vcodec wmv2  -acodec wmav2 -b {0} -ab {1} -ar {2} -s {3}", HQVideoBitrate, HQAudioBitrate, HQAudioSamplingFrequency, Resolution.VGA);
        public static string WMVVeryHighQualitySVGA = string.Format("-y -vcodec wmv2  -acodec wmav2 -b {0} -ab {1} -ar {2} -s {3}", VHQVideoBitrate, VHQAudioBitrate, HQAudioSamplingFrequency, Resolution.SVGA);

        public static string WMVLowQualityKeepOriginalSize = string.Format("-y -vcodec wmv2  -acodec wmav2 -b {0} -ab {1} -ar {2}", LQVideoBitrate, LQAudioBitrate, LQAudioSamplingFrequency, Resolution.QVGA);
        public static string WMVMediumQualityKeepOriginalSize = string.Format("-y -vcodec wmv2  -acodec wmav2 -b {0} -ab {1} -ar {2}", MQVideoBitrate, MQAudioBitrate, MQAudioSamplingFrequency, Resolution.CIF);
        public static string WMVHighQualityKeepOriginalSize = string.Format("-y -vcodec wmv2  -acodec wmav2 -b {0} -ab {1} -ar {2}", HQVideoBitrate, HQAudioBitrate, HQAudioSamplingFrequency, Resolution.VGA);
        public static string WMVVeryHighQualityKeepOriginalSize = string.Format("-y -vcodec wmv2  -acodec wmav2 -b {0} -ab {1} -ar {2}", VHQVideoBitrate, VHQAudioBitrate, HQAudioSamplingFrequency, Resolution.SVGA);

        //"-i infile.avi -f webm output.webm"
        public static string OggMediumQualityKeepOriginalSize = string.Format("-f webm");
        public static string Mp4MediumQualityKeepOriginalSize = string.Format("");
        public static string WebmMediumQualityKeepOriginalSize = string.Format("");
    }
}