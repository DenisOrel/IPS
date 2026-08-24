// Decompiled with JetBrains decompiler
// Type: Intermech.Map.Layout.MapLayoutNetworkLink
// Assembly: Intermech.Optimizer, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: F2F8A027-9638-497B-A691-BEF61B30B332
// Assembly location: D:\IPS\Client\Intermech.Optimizer.dll
// XML documentation location: D:\IPS\Client\Intermech.Optimizer.xml

using System;
using System.Drawing;

#nullable disable
namespace Intermech.Map.Layout;

[Serializable]
public class MapLayoutNetworkLink : IMapLink, IMapGraphPart
{
  private int myFlags;
  private MapLayoutNetworkNode myFromNode;
  private MapLayoutNetwork myMapLayoutNetwork;
  private MapObject myGoObject;
  private object myLinkData;
  private object myLinkUserData;
  private MapLayoutNetworkNode myToNode;

  public MapLayoutNetworkLink()
  {
    this.myMapLayoutNetwork = (MapLayoutNetwork) null;
    this.myGoObject = (MapObject) null;
    this.myFromNode = (MapLayoutNetworkNode) null;
    this.myToNode = (MapLayoutNetworkNode) null;
    this.myLinkUserData = (object) null;
    this.myFlags = 0;
  }

  public virtual void CommitPosition()
  {
  }

  public MapLayoutNetworkNode GetOtherNode(IMapNode n)
  {
    MapLayoutNetworkNode layoutNetworkNode = n as MapLayoutNetworkNode;
    if (this.ToNode == layoutNetworkNode)
      return this.FromNode;
    return this.FromNode == layoutNetworkNode ? this.ToNode : (MapLayoutNetworkNode) null;
  }

  public MapLayoutNetworkNode GetOtherPort(IMapPort p)
  {
    MapLayoutNetworkNode layoutNetworkNode = p as MapLayoutNetworkNode;
    if (this.ToNode == layoutNetworkNode)
      return this.FromNode;
    return this.FromNode == layoutNetworkNode ? this.ToNode : (MapLayoutNetworkNode) null;
  }

  IMapNode IMapLink.FromNode => (IMapNode) this.myFromNode;

  IMapPort IMapLink.FromPort
  {
    get => (IMapPort) this.myFromNode;
    set => this.myFromNode = (MapLayoutNetworkNode) value;
  }

  IMapNode IMapLink.ToNode => (IMapNode) this.myToNode;

  IMapPort IMapLink.ToPort
  {
    get => (IMapPort) this.myToNode;
    set => this.myToNode = (MapLayoutNetworkNode) value;
  }

  IMapNode IMapLink.GetOtherNode(IMapNode n) => (IMapNode) this.GetOtherNode(n);

  IMapPort IMapLink.GetOtherPort(IMapPort p) => (IMapPort) this.GetOtherPort(p);

  void IMapLink.OnPortChanged(
    IMapPort port,
    int subhint,
    int oldI,
    object oldVal,
    RectangleF oldRect,
    int newI,
    object newVal,
    RectangleF newRect)
  {
  }

  void IMapLink.Unlink()
  {
    if (this.Network == null)
      return;
    this.Network.DeleteLink(this);
  }

  public virtual void OnPortChanged(
    IMapPort port,
    int subhint,
    int oldI,
    object oldVal,
    RectangleF oldRect,
    int newI,
    object newVal,
    RectangleF newRect)
  {
  }

  public void ReverseLink()
  {
    MapLayoutNetworkNode fromNode = this.myFromNode;
    this.myFromNode = this.myToNode;
    this.myToNode = fromNode;
  }

  public void Unlink()
  {
    if (this.Network == null)
      return;
    this.Network.DeleteLink(this);
  }

  public MapLayoutNetworkNode FromNode
  {
    get => this.myFromNode;
    set => this.myFromNode = value;
  }

  public MapLayoutNetworkNode FromPort
  {
    get => this.myFromNode;
    set => this.myFromNode = value;
  }

  public MapObject MapObject
  {
    get => this.myGoObject;
    set => this.myGoObject = value;
  }

  public object LinkData
  {
    get => this.myLinkData;
    set => this.myLinkData = value;
  }

  public MapLayoutNetwork Network
  {
    get => this.myMapLayoutNetwork;
    set => this.myMapLayoutNetwork = value;
  }

  public virtual MapStroke Stroke
  {
    get
    {
      if (this.myGoObject is MapStroke)
        return (MapStroke) this.myGoObject;
      return this.myGoObject is MapLabeledLink ? (MapStroke) ((MapLabeledLink) this.myGoObject).RealLink : (MapStroke) null;
    }
  }

  public MapLayoutNetworkNode ToNode
  {
    get => this.myToNode;
    set => this.myToNode = value;
  }

  public MapLayoutNetworkNode ToPort
  {
    get => this.myToNode;
    set => this.myToNode = value;
  }

  public int UserFlags
  {
    get => this.myFlags;
    set => this.myFlags = value;
  }

  public object UserObject
  {
    get => this.myLinkUserData;
    set => this.myLinkUserData = value;
  }
}
