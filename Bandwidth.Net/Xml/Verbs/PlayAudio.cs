using System.Xml.Serialization;

namespace Bandwidth.Net.Xml.Verbs
{
    /// <summary>
    /// The PlayAudio verb is used to play an audio file in the call.
    /// </summary>
    /// <seealso href="http://ap.bandwidth.com/docs/xml/playaudio/"/>
    public class PlayAudio: IXmlSerializable, IVerb
    {
        /// <summary>
        /// Allows you to play DTMF digits in the call
        /// </summary>
        public string Digits { get; set; }

        /// <summary>
        /// Url of media resourse to play
        /// </summary>
        public string Url { get; set; }

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
            if (!string.IsNullOrEmpty(Digits))
            {
                writer.WriteAttributeString("digits", Digits);
            }
            writer.WriteString(Url);
        }
    }
}
