// Decompiled with JetBrains decompiler
// Type: Intermech.Serialization.Compatibility.Binary.NullHolderSurrogate
// Assembly: Intermech.Serialization.Compatibility, Version=1.0.1.74, Culture=neutral, PublicKeyToken=null
// MVID: D3658D7B-7F63-413B-8D5F-ACD5662A960C
// Assembly location: D:\Projects\IPS Code\AltiumDesigner\IPSAddIn\Intermech.Serialization.Compatibility.dll

using System;
using System.Runtime.Serialization;

#nullable disable
namespace Intermech.Serialization.Compatibility.Binary;

internal sealed class NullHolderSurrogate : ISerializationSurrogate
{
  public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
  {
    throw new NotSupportedException();
  }

  public object SetObjectData(
    object obj,
    SerializationInfo info,
    StreamingContext context,
    ISurrogateSelector selector)
  {
    return (object) null;
  }
}
