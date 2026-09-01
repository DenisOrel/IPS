// Decompiled with JetBrains decompiler
// Type: Intermech.Serialization.ClassicFormatters.SurrogateForCyclicalReference
// Assembly: Intermech.Serialization.Compatibility, Version=1.0.1.74, Culture=neutral, PublicKeyToken=null
// MVID: D3658D7B-7F63-413B-8D5F-ACD5662A960C
// Assembly location: D:\Projects\IPS Code\AltiumDesigner\IPSAddIn\Intermech.Serialization.Compatibility.dll

using System.Runtime.Serialization;

#nullable disable
namespace Intermech.Serialization.ClassicFormatters;

internal sealed class SurrogateForCyclicalReference : ISerializationSurrogate
{
  private readonly ISerializationSurrogate _innerSurrogate;

  internal SurrogateForCyclicalReference(ISerializationSurrogate innerSurrogate)
  {
    this._innerSurrogate = innerSurrogate;
  }

  public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
  {
    this._innerSurrogate.GetObjectData(obj, info, context);
  }

  public object SetObjectData(
    object obj,
    SerializationInfo info,
    StreamingContext context,
    ISurrogateSelector selector)
  {
    return this._innerSurrogate.SetObjectData(obj, info, context, selector);
  }
}
