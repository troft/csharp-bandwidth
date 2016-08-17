using Newtonsoft.Json;
using Xunit;

namespace Bandwidth.Net.Test
{
  public class StringBooleanConverterTests
  {

    [Fact]
    public void TestReadJson()
    {
      var item = JsonConvert.DeserializeObject<TestItem>("{\"Item1\":\"true\", \"Item2\":\"true\"}");
      Assert.True(item.Item1);
      Assert.True(item.Item2);
    }

    [Fact]
    public void TestReadJson2()
    {
      var item = JsonConvert.DeserializeObject<TestItem>("{\"Item1\":\"false\", \"Item2\":\"false\"}");
      Assert.False(item.Item1);
      Assert.False(item.Item2);
    }

    [Fact]
    public void TestReadJson3()
    {
      var item = JsonConvert.DeserializeObject<TestItem>("{}");
      Assert.False(item.Item1);
      Assert.Null(item.Item2);
    }

    [Fact]
    public void TestWriteJson()
    {
      var item = new TestItem
      {
        Item1 = true,
        Item2 = true
      };
      var json = JsonConvert.SerializeObject(item);
      Assert.Equal("{\"Item1\":\"true\",\"Item2\":\"true\"}", json);
    }

    [Fact]
    public void TestWriteJson2()
    {
      var item = new TestItem
      {
        Item1 = false,
        Item2 = false
      };
      var json = JsonConvert.SerializeObject(item);
      Assert.Equal("{\"Item1\":\"false\",\"Item2\":\"false\"}", json);
    }

    [Fact]
    public void TestWriteJson3()
    {
      var item = new TestItem();
      var json = JsonConvert.SerializeObject(item);
      Assert.Equal("{\"Item1\":\"false\",\"Item2\":null}", json);
    }


    public class TestItem
    {
      [JsonConverter(typeof (StringBooleanConverter))]
      public bool Item1 { get; set; }
      
      [JsonConverter(typeof (StringBooleanConverter))]
      public bool? Item2 { get; set; }
    }
  }
}
