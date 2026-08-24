// Decompiled with JetBrains decompiler
// Type: Intermech.Interfaces.FSS.IPSClientNotLoggedException
// Assembly: Intermech.Interfaces.FSS, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 89C13FA3-8295-4BAF-985C-14C35172BA6B
// Assembly location: D:\IPS\Client\Intermech.Interfaces.FSS.dll
// XML documentation location: D:\IPS\Client\Intermech.Interfaces.FSS.xml

using System;
using System.Runtime.Serialization;

#nullable disable
namespace Intermech.Interfaces.FSS;

/// <summary>Исключение "Клиент IPS не подключён"</summary>
[Serializable]
public class IPSClientNotLoggedException : GenericIPS_FSS_Exception
{
  public IPSClientNotLoggedException()
  {
  }

  public IPSClientNotLoggedException(string message)
    : base(message)
  {
  }

  public IPSClientNotLoggedException(string message, Exception innerException)
    : base(message, innerException)
  {
  }

  protected IPSClientNotLoggedException(SerializationInfo info, StreamingContext context)
    : base(info, context)
  {
  }
}
