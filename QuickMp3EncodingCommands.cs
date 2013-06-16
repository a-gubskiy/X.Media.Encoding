namespace X.Media.Encoding
{
    /// <summary>
    /// Ready made encoding commands for FFmpeg
    /// Use when calling EncodeVideo commands as string encodingCommand
    /// Add remove as you like
    /// </summary>
    public struct QuickAudioEncodingCommands
    {
        public const string MP3128Kbps = "-y -ab 128k -ar 44100";
        public const string MP396Kbps = "-y -ab 96k -ar 44100";
        public const string MP364Kbps = "-y -ab 64k -ar 44100";
        public const string MP332Kbps = "-y -ab 32k -ar 44100";
    }
}