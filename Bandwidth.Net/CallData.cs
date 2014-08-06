namespace Bandwidth.Net
{
    public class CallData
    {
        public CallState State { get; set; }
        public string TransferTo { get; set; }
        public string CallbackUrl { get; set; }
        public string TransferCallerId { get; set; }
        public WhisperAudio WhisperAudio { get; set; }
        public bool? RecordingEnabled { get; set; }
    }

    public class WhisperAudio
    {
        public Gender? Gender { get; set; }
        public string Voice { get; set; }
        public string Sentence { get; set; }
        public string Locale { get; set; }
    }

    public enum Gender
    {
        Male,
        Female
    }

    public enum CallState
    {
        Active,
        Completed,
        Rejected,
        Transferring
    }
}