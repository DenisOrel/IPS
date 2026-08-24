// Decompiled with JetBrains decompiler
// Type: Intermech.Interfaces.FSS.IPSClientTimedOutException
// Assembly: Intermech.Interfaces.FSS, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 89C13FA3-8295-4BAF-985C-14C35172BA6B
// Assembly location: D:\IPS\Client\Intermech.Interfaces.FSS.dll
// XML documentation location: D:\IPS\Client\Intermech.Interfaces.FSS.xml

using System;
using System.Runtime.Serialization;

#nullable disable
namespace Intermech.Interfaces.FSS;

/// <summary>
/// Исключение "Клиент IPS был принудительно отключён по тайм-ауту"
/// </summary>
[Serializable]
public class IPSClientTimedOutException : GenericIPS_FSS_Exception
{
  public IPSClientTimedOutException()
  {
  }

  public IPSClientTimedOutException(string message)
    : base(message)
  {
  }

  public IPSClientTimedOutException(string message, Exception innerException)
    : base(message, innerException)
  {
  }

  protected IPSClientTimedOutException(SerializationInfo info, StreamingContext context)
    : base(info, context)
  {
  }
}
