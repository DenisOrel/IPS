// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.PortalNavigator.PacketBinding
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Navigator.DB;
using Intermech.Navigator.Parts;
using System;

#nullable disable
namespace Intermech.Site.Client.PortalNavigator;

internal class PacketBinding(int typeID) : PortalTypesBinding(typeID)
{
  public override INodePart GetPart(IConditionsProvider conditionProvider)
  {
    return (INodePart) new PacketsPart((IServiceProvider) null, conditionProvider, this.typeID);
  }
}
