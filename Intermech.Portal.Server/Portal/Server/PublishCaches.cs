// Decompiled with JetBrains decompiler
// Type: Intermech.Portal.Server.PublishCaches
// Assembly: Intermech.Portal.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 814BABAA-794A-446D-BCF7-B9A0D67EFF42
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Portal.Server.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.Portal.Server;

internal class PublishCaches
{
  public List<Tuple<Guid, long>> Relations;
  public List<long> Objects;
  public List<long> ObjectsWithLinks;
  public List<Tuple<Guid, long>> RelationsWithLinks;
  public Dictionary<Guid, long> ImportedObjectsIDs;

  public PublishCaches()
  {
    this.Relations = new List<Tuple<Guid, long>>();
    this.Objects = new List<long>();
    this.ImportedObjectsIDs = new Dictionary<Guid, long>();
    this.ObjectsWithLinks = new List<long>();
    this.RelationsWithLinks = new List<Tuple<Guid, long>>();
  }

  public void Destroy()
  {
    this.Relations = (List<Tuple<Guid, long>>) null;
    this.Objects = (List<long>) null;
    this.ImportedObjectsIDs = (Dictionary<Guid, long>) null;
    this.ObjectsWithLinks = (List<long>) null;
    this.RelationsWithLinks = (List<Tuple<Guid, long>>) null;
  }
}
