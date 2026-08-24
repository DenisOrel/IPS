// Decompiled with JetBrains decompiler
// Type: Intermech.Interfaces.FSS.IPSServerInfo
// Assembly: Intermech.Interfaces.FSS, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 89C13FA3-8295-4BAF-985C-14C35172BA6B
// Assembly location: D:\IPS\Client\Intermech.Interfaces.FSS.dll
// XML documentation location: D:\IPS\Client\Intermech.Interfaces.FSS.xml

using System;

#nullable disable
namespace Intermech.Interfaces.FSS;

/// <summary>
/// Информация о серверной службе защиты файловых хранилищ
/// </summary>
[Serializable]
public struct IPSServerInfo
{
  /// <summary>Версия сервера</summary>
  public string Version;
  /// <summary>Имя сервиса</summary>
  public string ServiceName;
}
