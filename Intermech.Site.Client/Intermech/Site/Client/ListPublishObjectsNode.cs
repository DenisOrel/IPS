// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.ListPublishObjectsNode
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Navigator.DBObjects;
using Intermech.Navigator.Parts;
using System.Collections.Generic;

#nullable disable
namespace Intermech.Site.Client;

internal sealed class ListPublishObjectsNode(Dictionary<int, List<long>> objectIDs) : ObjectsDictNode(objectIDs, false)
{
  protected override List<PartSlot> CreateFolderSlots() => (List<PartSlot>) null;
}
