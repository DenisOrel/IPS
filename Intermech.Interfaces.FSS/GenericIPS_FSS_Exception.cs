// Decompiled with JetBrains decompiler
// Type: Intermech.Interfaces.FSS.GenericIPS_FSS_Exception
// Assembly: Intermech.Interfaces.FSS, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 89C13FA3-8295-4BAF-985C-14C35172BA6B
// Assembly location: D:\IPS\Client\Intermech.Interfaces.FSS.dll
// XML documentation location: D:\IPS\Client\Intermech.Interfaces.FSS.xml

using System;
using System.Runtime.Serialization;

#nullable disable
namespace Intermech.Interfaces.FSS;

/// <summary>
/// Общее исключение службы защиты файловых хранилищ IPS.FSS
/// </summary>
[Serializable]
public class GenericIPS_FSS_Exception : Exception
{
  public GenericIPS_FSS_Exception()
  {
  }

  public GenericIPS_FSS_Exception(string message)
    : base(message)
  {
  }

  public GenericIPS_FSS_Exception(string message, Exception innerException)
    : base(message, innerException)
  {
  }

  protected GenericIPS_FSS_Exception(SerializationInfo info, StreamingContext context)
    : base(info, context)
  {
  }
}
