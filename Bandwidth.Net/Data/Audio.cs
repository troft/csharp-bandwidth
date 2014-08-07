using System;

namespace Bandwidth.Net.Data
{
    public class Audio
    {
        public Uri FileUrl { get; set; }
        public string Sentence { get; set; }
        public Gender? Gender { get; set; }
        public string Locale { get; set; }
        public string Voice { get; set; }
        public bool? LoopEnabled { get; set; }
        public string Tag { get; set; }
    }
}