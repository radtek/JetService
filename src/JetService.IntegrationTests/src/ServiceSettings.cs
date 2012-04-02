using System;
using System.IO;
using System.Xml.Serialization;

namespace JetService.IntegrationTests
{
  [XmlRoot("jetservice")]
  public class ServiceSettings
  {
    [XmlElement("name")]
    public string Name { get; set; }

    [XmlElement("displayName")]
    public string DisplayName { get; set; }

    [XmlElement("description")]
    public string Description { get; set; }

    [XmlElement("execution")]
    public ExecutionElement Execution { get; set; }

    public ServiceSettings()
    {
      Execution = new ExecutionElement();
    }

    [XmlRoot("execution")]
    public class ExecutionElement
    {
      [XmlElement("workdir")]
      public string WorkDir { get; set; }

      [XmlElement("program")]
      public string Program { get; set; }

      [XmlElement("arguments")]
      public string Arguments { get; set; }
    }


    public static void Serialize(string file, ServiceSettings settings)
    {
      using(var wr = File.CreateText(file))
      {
        var f = new XmlSerializerFactory();
        var ser = f.CreateSerializer(typeof (ServiceSettings));
        if (ser == null)
          throw new Exception("Failed to get serializer");
        ser.Serialize(wr, settings);
      }
    }
  }
}