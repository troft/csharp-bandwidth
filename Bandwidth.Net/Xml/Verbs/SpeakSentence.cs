using System.Xml.Serialization;

namespace Bandwidth.Net.Xml.Verbs
{
    /// <summary>
    /// The SpeakSentence verb is used to convert any text into speak for the caller.
    /// </summary>
    /// <seealso href="http://ap.bandwidth.com/docs/xml/speaksentence/"/>
    public class SpeakSentence: IXmlSerializable, IVerb
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public SpeakSentence()
        {
            Gender = "female";
            Locale = "en_US";
        }

        /// <summary>
        /// The gender of the speaker
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// The accent of the speaker
        /// </summary>
        public string Locale { get; set; }

        /// <summary>
        /// The voice of the speaker, limited by gender
        /// </summary>
        public string Voice { get; set; }

       
        /// <summary>
        /// Sentence to speak
        /// </summary>
        public string Sentence { get; set; }

        System.Xml.Schema.XmlSchema IXmlSerializable.GetSchema()
        {
            return null;
        }

        void IXmlSerializable.ReadXml(System.Xml.XmlReader reader)
        {
            throw new System.NotImplementedException();
        }

        void IXmlSerializable.WriteXml(System.Xml.XmlWriter writer)
        {
            if (Gender != "female")
            {
                writer.WriteAttributeString("gender", Gender);
            }
            if (Locale != "en_US")
            {
                writer.WriteAttributeString("locale", Locale);
            }
            if (!string.IsNullOrEmpty(Voice))
            {
                writer.WriteAttributeString("voice", Voice);
            }
            writer.WriteString(Sentence);
        }
    }
}
