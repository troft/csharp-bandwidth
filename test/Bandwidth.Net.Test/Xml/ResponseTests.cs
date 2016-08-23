using System.Xml.Serialization;
using Bandwidth.Net.Xml;
using Xunit;

namespace Bandwidth.Net.Test.Xml
{
  public class ResponseTests
  {
    [Fact]
    public void TestToXml()
    {
      var response = new Response();
      Assert.Equal("<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<Response />", response.ToXml());
    }

    [Fact]
    public void TestToXml2()
    {
      var response = new Response(new TestItem
      {
        StringValue = "Test",
        Attribute1 = 10
      });
      Assert.Equal("<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<Response>\r\n  <TestItem Attribute1=\"10\">\r\n    <StringValue>Test</StringValue>\r\n  </TestItem>\r\n</Response>", response.ToXml());
    }

    [Fact]
    public void TestToXml3()
    {
      var response = new Response();
      response.Add(new TestItem
      {
        StringValue = "Test",
        Attribute1 = 10
      });
      Assert.Equal("<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<Response>\r\n  <TestItem Attribute1=\"10\">\r\n    <StringValue>Test</StringValue>\r\n  </TestItem>\r\n</Response>", response.ToXml());
    }

    [Fact]
    public void TestToXmlWithNested()
    {
      var response = new Response();
      response.Add(new TestItem
      {
        Attribute1 = 10,
        Nested = new TestItem { Attribute1 = 20}
      });
      Assert.Equal("<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<Response>\r\n  <TestItem Attribute1=\"10\">\r\n    <Nested Attribute1=\"20\" />\r\n  </TestItem>\r\n</Response>", response.ToXml());
    }

    public class TestItem : IVerb
    {
      public string StringValue { get; set; }
      
      [XmlAttribute]
      public int Attribute1 { get; set; }

      public TestItem Nested { get; set; }
    }
  }
}
