// Decompiled with JetBrains decompiler
// Type: Intermech.Serialization.ClassicFormatters.Binary.BinaryFormatterEventSource
// Assembly: Intermech.Serialization.Compatibility, Version=1.0.1.74, Culture=neutral, PublicKeyToken=null
// MVID: D3658D7B-7F63-413B-8D5F-ACD5662A960C
// Assembly location: D:\Projects\IPS Code\AltiumDesigner\IPSAddIn\Intermech.Serialization.Compatibility.dll

using System;
using System.Diagnostics.Tracing;

#nullable disable
namespace Intermech.Serialization.ClassicFormatters.Binary;

[EventSource(Name = "System.Runtime.Serialization.Formatters.Binary.BinaryFormatterEventSource")]
internal sealed class BinaryFormatterEventSource : EventSource
{
  private const int EventId_SerializationStart = 10;
  private const int EventId_SerializationStop = 11;
  private const int EventId_SerializingObject = 12;
  private const int EventId_DeserializationStart = 20;
  private const int EventId_DeserializationStop = 21;
  private const int EventId_DeserializingObject = 22;
  public static readonly BinaryFormatterEventSource Log = new BinaryFormatterEventSource();

  private BinaryFormatterEventSource()
  {
  }

  [Event(10, Opcode = EventOpcode.Start, Keywords = (EventKeywords) 1, Level = EventLevel.Informational, ActivityOptions = EventActivityOptions.Recursive)]
  public void SerializationStart()
  {
    if (!this.IsEnabled(EventLevel.Informational, (EventKeywords) 1))
      return;
    this.WriteEvent(10);
  }

  [Event(11, Opcode = EventOpcode.Stop, Keywords = (EventKeywords) 1, Level = EventLevel.Informational)]
  public void SerializationStop()
  {
    if (!this.IsEnabled(EventLevel.Informational, (EventKeywords) 1))
      return;
    this.WriteEvent(11);
  }

  [NonEvent]
  public void SerializingObject(Type type)
  {
    if (!this.IsEnabled(EventLevel.Informational, (EventKeywords) 1))
      return;
    this.SerializingObject(type.AssemblyQualifiedName);
  }

  [Event(12, Keywords = (EventKeywords) 1, Level = EventLevel.Informational)]
  private void SerializingObject(string typeName) => this.WriteEvent(12, typeName);

  [Event(20, Opcode = EventOpcode.Start, Keywords = (EventKeywords) 2, Level = EventLevel.Informational, ActivityOptions = EventActivityOptions.Recursive)]
  public void DeserializationStart()
  {
    if (!this.IsEnabled(EventLevel.Informational, (EventKeywords) 2))
      return;
    this.WriteEvent(20);
  }

  [Event(21, Opcode = EventOpcode.Stop, Keywords = (EventKeywords) 2, Level = EventLevel.Informational)]
  public void DeserializationStop()
  {
    if (!this.IsEnabled(EventLevel.Informational, (EventKeywords) 2))
      return;
    this.WriteEvent(21);
  }

  [NonEvent]
  public void DeserializingObject(Type type)
  {
    if (!this.IsEnabled(EventLevel.Informational, (EventKeywords) 2))
      return;
    this.DeserializingObject(type.AssemblyQualifiedName);
  }

  [Event(22, Keywords = (EventKeywords) 2, Level = EventLevel.Informational)]
  private void DeserializingObject(string typeName) => this.WriteEvent(22, typeName);

  public static class Keywords
  {
    public const EventKeywords Serialization = (EventKeywords) 1;
    public const EventKeywords Deserialization = (EventKeywords) 2;
  }
}
