// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.PortalNavigator.PortalNodeID
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Navigator.Interfaces;
using System.Diagnostics;

#nullable disable
namespace Intermech.Site.Client.PortalNavigator;

public class PortalNodeID : INodeID
{
  protected internal string caption;
  private object cookie;

  public PortalNodeID()
  {
  }

  public PortalNodeID(string name) => this.caption = name;

  public int CategoryID
  {
    [DebuggerStepThrough] get => SiteClientConsts.CategoryPortal;
  }

  public int TypeID
  {
    [DebuggerStepThrough] get => 0;
  }

  public object Cookie
  {
    [DebuggerStepThrough] get => this.cookie;
    [DebuggerStepThrough] set => this.cookie = value;
  }

  public override int GetHashCode() => this.caption.GetHashCode();

  public override bool Equals(object obj)
  {
    return obj is PortalNodeID portalNodeId && portalNodeId.caption == this.caption;
  }
}
