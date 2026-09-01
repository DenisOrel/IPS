// Decompiled with JetBrains decompiler
// Type: Intermech.Reflection.TypeIdentifiers.TypeNameParserException
// Assembly: Intermech.Serialization.Compatibility, Version=1.0.1.74, Culture=neutral, PublicKeyToken=null
// MVID: D3658D7B-7F63-413B-8D5F-ACD5662A960C
// Assembly location: D:\Projects\IPS Code\AltiumDesigner\IPSAddIn\Intermech.Serialization.Compatibility.dll

using System;
using System.Runtime.Serialization;

#nullable disable
namespace Intermech.Reflection.TypeIdentifiers;

[Serializable]
public class TypeNameParserException : Exception
{
  public TypeNameParserException()
  {
  }

  public TypeNameParserException(string message)
    : base(message)
  {
  }

  public TypeNameParserException(string message, Exception inner)
    : base(message, inner)
  {
  }

  protected TypeNameParserException(SerializationInfo info, StreamingContext context)
    : base(info, context)
  {
  }
}
