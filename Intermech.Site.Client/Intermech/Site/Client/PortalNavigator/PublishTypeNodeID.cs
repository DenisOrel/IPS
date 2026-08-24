// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.PortalNavigator.PublishTypeNodeID
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Navigator.Interfaces;
using System.Diagnostics;

#nullable disable
namespace Intermech.Site.Client.PortalNavigator;

public class PublishTypeNodeID : INodeID
{
  protected internal string caption;
  protected internal int id;
  private object cookie;

  public PublishTypeNodeID()
  {
  }

  public PublishTypeNodeID(int id, string name)
  {
    this.id = id;
    this.caption = name;
  }

  public int CategoryID
  {
    [DebuggerStepThrough] get => SiteClientConsts.CategoryPublishType;
  }

  public int TypeID
  {
    [DebuggerStepThrough] get => this.id;
  }

  public object Cookie
  {
    [DebuggerStepThrough] get => this.cookie;
    [DebuggerStepThrough] set => this.cookie = value;
  }

  public override bool Equals(object obj)
  {
    return obj is PublishTypeNodeID publishTypeNodeId && this.id == publishTypeNodeId.id;
  }

  public override int GetHashCode() => this.id.GetHashCode();
}
