// Decompiled with JetBrains decompiler
// Type: Intermech.Interfaces.FSS.SystemDirectoryException
// Assembly: Intermech.Interfaces.FSS, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 89C13FA3-8295-4BAF-985C-14C35172BA6B
// Assembly location: D:\IPS\Client\Intermech.Interfaces.FSS.dll
// XML documentation location: D:\IPS\Client\Intermech.Interfaces.FSS.xml

using System;
using System.Runtime.Serialization;

#nullable disable
namespace Intermech.Interfaces.FSS;

/// <summary>
/// Исключение "Попытка использовать системную папку в качестве файлового хранилища"
/// </summary>
[Serializable]
public class SystemDirectoryException : GenericIPS_FSS_Exception
{
  public SystemDirectoryException()
  {
  }

  public SystemDirectoryException(string message)
    : base(message)
  {
  }

  public SystemDirectoryException(string message, Exception innerException)
    : base(message, innerException)
  {
  }

  protected SystemDirectoryException(SerializationInfo info, StreamingContext context)
    : base(info, context)
  {
  }
}
