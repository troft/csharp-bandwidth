using Bandwidth.Net.Xml;
using Bandwidth.Net.Xml.Verbs;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bandwidth.Net.Tests
{
    [TestClass]
    public class XmlTests
    {
        [TestMethod]
        public void ResponseTest()
        {
            var response = new Response();
            Assert.AreEqual("<?xml version=\"1.0\" encoding=\"utf-8\"?>\n<Response />", response.ToXml());
        }

        [TestMethod]
        public void GatherTest()
        {
            var response = new Response();
            response.Add(new Gather{RequestUrl = "http://localhost"});
            Assert.AreEqual("<?xml version=\"1.0\" encoding=\"utf-8\"?>\n<Response>\n  <Gather requestUrl=\"http://localhost\" />\n</Response>", response.ToXml());
            response = new Response();
            response.Add(new Gather { RequestUrl = "http://localhost", Bargeable = false});
            Assert.AreEqual("<?xml version=\"1.0\" encoding=\"utf-8\"?>\n<Response>\n  <Gather requestUrl=\"http://localhost\" bargeable=\"false\" />\n</Response>", response.ToXml());
        }

        [TestMethod]
        public void HangupTest()
        {
            var response = new Response();
            response.Add(new Hangup());
            Assert.AreEqual("<?xml version=\"1.0\" encoding=\"utf-8\"?>\n<Response>\n  <Hangup />\n</Response>", response.ToXml());
        }

        [TestMethod]
        public void PauseTest()
        {
            var response = new Response();
            response.Add(new Pause{Duration = 10});
            Assert.AreEqual("<?xml version=\"1.0\" encoding=\"utf-8\"?>\n<Response>\n  <Pause duration=\"10\" />\n</Response>", response.ToXml());
        }

        
        [TestMethod]
        public void PlayAudioTest()
        {
            var response = new Response();
            response.Add(new PlayAudio{Digits = "0", Url = "http://localhost"});
            Assert.AreEqual("<?xml version=\"1.0\" encoding=\"utf-8\"?>\n<Response>\n  <PlayAudio digits=\"0\">http://localhost</PlayAudio>\n</Response>", response.ToXml());
        }

        [TestMethod]
        public void RecordTest()
        {
            var response = new Response();
            response.Add(new Record{Transcribe = true});
            Assert.AreEqual("<?xml version=\"1.0\" encoding=\"utf-8\"?>\n<Response>\n  <Record transcribe=\"true\" />\n</Response>", response.ToXml());
            response = new Response();
            response.Add(new Record { Transcribe = true, MaxDuration = 20});
            Assert.AreEqual("<?xml version=\"1.0\" encoding=\"utf-8\"?>\n<Response>\n  <Record maxDuration=\"20\" transcribe=\"true\" />\n</Response>", response.ToXml());
        }

        [TestMethod]
        public void RedirectTest()
        {
            var response = new Response();
            response.Add(new Redirect{RequestUrl = "http://localhost"});
            Assert.AreEqual("<?xml version=\"1.0\" encoding=\"utf-8\"?>\n<Response>\n  <Redirect requestUrl=\"http://localhost\" />\n</Response>", response.ToXml());
        }

        [TestMethod]
        public void RejectTest()
        {
            var response = new Response();
            response.Add(new Reject { Reason = "error" });
            Assert.AreEqual("<?xml version=\"1.0\" encoding=\"utf-8\"?>\n<Response>\n  <Reject reason=\"error\" />\n</Response>", response.ToXml());
        }

        [TestMethod]
        public void SpeakSentenceTest()
        {
            var response = new Response();
            response.Add(new SpeakSentence{ Sentence = "Hello" });
            Assert.AreEqual("<?xml version=\"1.0\" encoding=\"utf-8\"?>\n<Response>\n  <SpeakSentence>Hello</SpeakSentence>\n</Response>", response.ToXml());
            response = new Response();
            response.Add(new SpeakSentence { Sentence = "Hello", Gender = "male", Locale = "en", Voice = "test"});
            Assert.AreEqual("<?xml version=\"1.0\" encoding=\"utf-8\"?>\n<Response>\n  <SpeakSentence gender=\"male\" locale=\"en\" voice=\"test\">Hello</SpeakSentence>\n</Response>", response.ToXml());
        }

        [TestMethod]
        public void SendMessageTest()
        {
            var response = new Response();
            response.Add(new SendMessage{ From = "from", To = "to", RequestUrl = "url", Text = "Hello"});
            Assert.AreEqual("<?xml version=\"1.0\" encoding=\"utf-8\"?>\n<Response>\n  <SendMessage from=\"from\" to=\"to\" requestUrl=\"url\">Hello</SendMessage>\n</Response>", response.ToXml());

            response = new Response();
            response.Add(new SendMessage { From = "from", To = "to", RequestUrl = "url", Text = "Hello", RequestUrlTimeout = 10});
            Assert.AreEqual("<?xml version=\"1.0\" encoding=\"utf-8\"?>\n<Response>\n  <SendMessage from=\"from\" to=\"to\" requestUrl=\"url\" requestUrlTimeout=\"10\">Hello</SendMessage>\n</Response>", response.ToXml());
            
        }

        [TestMethod]
        public void TransferTest()
        {
            var response = new Response();
            response.Add(new Transfer{ TransferTo = "to", TransferCallerId = "id"});
            Assert.AreEqual("<?xml version=\"1.0\" encoding=\"utf-8\"?>\n<Response>\n  <Transfer transferTo=\"to\" transferCallerId=\"id\" />\n</Response>", response.ToXml());
            response = new Response();
            response.Add(new Transfer { TransferTo = "to", TransferCallerId = "id", SpeakSentence = new SpeakSentence { Sentence = "Hello" } });
            Assert.AreEqual("<?xml version=\"1.0\" encoding=\"utf-8\"?>\n<Response>\n  <Transfer transferTo=\"to\" transferCallerId=\"id\">\n    <SpeakSentence>Hello</SpeakSentence>\n  </Transfer>\n</Response>", response.ToXml());
        }

        [TestMethod]
        public void ComplexTest()
        {
            var response = new Response();
            response.Add(new Gather { RequestUrl = "http://localhost" });
            response.Add(new Hangup());
            Assert.AreEqual("<?xml version=\"1.0\" encoding=\"utf-8\"?>\n<Response>\n  <Gather requestUrl=\"http://localhost\" />\n  <Hangup />\n</Response>", response.ToXml());

            response = new Response(new Gather { RequestUrl = "http://localhost" }, new Hangup());
            Assert.AreEqual("<?xml version=\"1.0\" encoding=\"utf-8\"?>\n<Response>\n  <Gather requestUrl=\"http://localhost\" />\n  <Hangup />\n</Response>", response.ToXml());
        }
    }
}
