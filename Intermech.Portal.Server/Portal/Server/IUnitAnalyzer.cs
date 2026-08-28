// Decompiled with JetBrains decompiler
// Type: Intermech.Portal.Server.IUnitAnalyzer
// Assembly: Intermech.Portal.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 814BABAA-794A-446D-BCF7-B9A0D67EFF42
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Portal.Server.dll

using Intermech.Interfaces;
using System;
using System.Collections.Generic;
using System.Xml;

#nullable disable
namespace Intermech.Portal.Server;

internal interface IUnitAnalyzer
{
  void Analysis(
    IDBObjectCollection publishObjects,
    List<Guid> importedObjects,
    PackAnalyzInfo packAnalyzInfo,
    Dictionary<Guid, int> partCounter);

  string SiteForUpdate { get; }

  bool AutoTransfer { get; }

  XmlNode RootNode { get; }
}
