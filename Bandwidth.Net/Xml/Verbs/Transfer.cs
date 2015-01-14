using System.Xml.Serialization;

namespace Bandwidth.Net.Xml.Verbs
{
    /// <summary>
    /// The Transfer verb is used to transfer the call to another number.
    /// </summary>
    /// <seealso href="http://ap.bandwidth.com/docs/xml/transfer/"/>
    public class Transfer: IVerb
    {
        /// <summary>
        /// Defines the number the call will be transferred to 
        /// </summary>
        [XmlAttribute("transferTo")]
        public string TransferTo { get; set; }

        /// <summary>
        /// This is the caller id that will be used when the call is transferred.
        /// </summary>
        [XmlAttribute("transferCallerId")]
        public string TransferCallerId { get; set; }

        /// <summary>
        /// This will speak the text into the call before transferring it.
        /// </summary>
        public SpeakSentence SpeakSentence { get; set; }

    }
}
