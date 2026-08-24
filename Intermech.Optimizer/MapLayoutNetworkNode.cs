// Decompiled with JetBrains decompiler
// Type: Intermech.Map.Layout.MapLayoutNetworkNode
// Assembly: Intermech.Optimizer, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: F2F8A027-9638-497B-A691-BEF61B30B332
// Assembly location: D:\IPS\Client\Intermech.Optimizer.dll
// XML documentation location: D:\IPS\Client\Intermech.Optimizer.xml

using System;
using System.Collections;
using System.Drawing;

#nullable disable
namespace Intermech.Map.Layout;

[Serializable]
public class MapLayoutNetworkNode : IMapNode, IMapGraphPart, IMapPort
{
  private PointF myCenter;
  private ArrayList myDestinationLinks;
  private int myFlags;
  private MapLayoutNetwork myMapLayoutNetwork;
  private MapObject myGoObject;
  private object myNodeData;
  private object myNodeUserData;
  private ArrayList mySourceLinks;

  public MapLayoutNetworkNode()
  {
    this.myNodeData = (object) null;
    this.myMapLayoutNetwork = (MapLayoutNetwork) null;
    this.myGoObject = (MapObject) null;
    this.myCenter = new PointF(0.0f, 0.0f);
    this.mySourceLinks = new ArrayList();
    this.myDestinationLinks = new ArrayList();
    this.myNodeUserData = (object) null;
    this.myFlags = 0;
  }

  public void AddDestinationLink(MapLayoutNetworkLink destinationLink)
  {
    if (this.myDestinationLinks.Contains((object) destinationLink))
      return;
    this.myDestinationLinks.Add((object) destinationLink);
  }

  public void AddSourceLink(MapLayoutNetworkLink sourceLink)
  {
    if (this.mySourceLinks.Contains((object) sourceLink))
      return;
    this.mySourceLinks.Add((object) sourceLink);
  }

  public virtual void CommitPosition()
  {
    if (this.myGoObject == null)
      return;
    if (this.myGoObject.SelectionObject != null)
      this.myGoObject.SelectionObject.Center = this.myCenter;
    else
      this.myGoObject.Center = this.myCenter;
  }

  public void DeleteDestinationLink(MapLayoutNetworkLink destinationLink)
  {
    int index = this.myDestinationLinks.IndexOf((object) destinationLink);
    if (index == -1)
      return;
    this.myDestinationLinks.RemoveAt(index);
  }

  public void DeleteSourceLink(MapLayoutNetworkLink sourceLink)
  {
    int index = this.mySourceLinks.IndexOf((object) sourceLink);
    if (index == -1)
      return;
    this.mySourceLinks.RemoveAt(index);
  }

  private MapLayoutLinkEnumerator GetLinkEnumerator(ArrayList a) => new MapLayoutLinkEnumerator(a);

  private MapLayoutNodeEnumerator GetNodeEnumerator(ArrayList a) => new MapLayoutNodeEnumerator(a);

  IEnumerable IMapNode.DestinationLinks
  {
    get => (IEnumerable) this.GetLinkEnumerator(this.myDestinationLinks);
  }

  IEnumerable IMapNode.Destinations => (IEnumerable) this.Destinations;

  IEnumerable IMapNode.Links => (IEnumerable) this.Links;

  IEnumerable IMapNode.Nodes => (IEnumerable) this.Nodes;

  IEnumerable IMapNode.Ports => (IEnumerable) null;

  IEnumerable IMapNode.SourceLinks => (IEnumerable) this.GetLinkEnumerator(this.mySourceLinks);

  IEnumerable IMapNode.Sources => (IEnumerable) this.Sources;

  void IMapPort.AddDestinationLink(IMapLink l)
  {
    if (!(l is MapLayoutNetworkLink layoutNetworkLink))
      return;
    this.myDestinationLinks.Add((object) layoutNetworkLink);
  }

  void IMapPort.AddSourceLink(IMapLink l)
  {
    if (!(l is MapLayoutNetworkLink layoutNetworkLink))
      return;
    this.mySourceLinks.Add((object) layoutNetworkLink);
  }

  bool IMapPort.CanLinkFrom() => true;

  bool IMapPort.CanLinkTo() => true;

  void IMapPort.ClearLinks()
  {
    foreach (MapLayoutNetworkLink sourceLink in this.SourceLinks)
      this.Network.DeleteLink(sourceLink);
    foreach (MapLayoutNetworkLink destinationLink in this.DestinationLinks)
      this.Network.DeleteLink(destinationLink);
  }

  bool IMapPort.ContainsLink(IMapLink l)
  {
    return this.mySourceLinks.Contains((object) l) || this.myDestinationLinks.Contains((object) l);
  }

  IMapLink[] IMapPort.CopyLinksArray()
  {
    IMapLink[] mapLinkArray = new IMapLink[this.myDestinationLinks.Count + this.mySourceLinks.Count];
    int count = this.mySourceLinks.Count;
    for (int index = 0; index < count; ++index)
      mapLinkArray[index] = (IMapLink) this.mySourceLinks[index];
    for (int index = 0; index < this.myDestinationLinks.Count; ++index)
      mapLinkArray[index + count] = (IMapLink) this.myDestinationLinks[index];
    return mapLinkArray;
  }

  IEnumerable IMapPort.DestinationLinks
  {
    get => (IEnumerable) this.GetLinkEnumerator(this.myDestinationLinks);
  }

  int IMapPort.DestinationLinksCount => this.myDestinationLinks.Count;

  IEnumerable IMapPort.Links
  {
    get
    {
      ArrayList a = new ArrayList();
      for (int index = 0; index < this.mySourceLinks.Count; ++index)
        a.Add(this.mySourceLinks[index]);
      for (int index = 0; index < this.myDestinationLinks.Count; ++index)
      {
        if (!a.Contains(this.myDestinationLinks[index]))
          a.Add(this.myDestinationLinks[index]);
      }
      return (IEnumerable) new MapLayoutLinkEnumerator(a);
    }
  }

  int IMapPort.LinksCount => this.myDestinationLinks.Count + this.mySourceLinks.Count;

  IMapNode IMapPort.Node => (IMapNode) this;

  IEnumerable IMapPort.SourceLinks => (IEnumerable) this.GetLinkEnumerator(this.mySourceLinks);

  int IMapPort.SourceLinksCount => this.mySourceLinks.Count;

  bool IMapPort.IsValidLink(IMapPort to) => true;

  void IMapPort.OnLinkChanged(
    IMapLink link,
    int subhint,
    int oldInt,
    object oldVal,
    RectangleF oldRect,
    int newInt,
    object newVal,
    RectangleF newRect)
  {
  }

  void IMapPort.RemoveLink(IMapLink l)
  {
    if (!(l is MapLayoutNetworkLink layoutNetworkLink))
      return;
    this.DeleteDestinationLink(layoutNetworkLink);
    this.DeleteSourceLink(layoutNetworkLink);
  }

  public virtual PointF Center
  {
    get => this.myCenter;
    set => this.myCenter = value;
  }

  public virtual MapLayoutLinkEnumerator DestinationLinks
  {
    get => this.GetLinkEnumerator(this.myDestinationLinks);
  }

  public ArrayList DestinationLinksList => this.myDestinationLinks;

  public virtual MapLayoutNodeEnumerator Destinations
  {
    get
    {
      ArrayList a = new ArrayList();
      foreach (MapLayoutNetworkLink destinationLink in this.myDestinationLinks)
      {
        MapLayoutNetworkNode toNode = destinationLink.ToNode;
        if (toNode != null && !a.Contains((object) toNode))
          a.Add((object) toNode);
      }
      return this.GetNodeEnumerator(a);
    }
  }

  public MapObject MapObject
  {
    get => this.myGoObject;
    set
    {
      this.myGoObject = value;
      if (this.myGoObject == null)
        return;
      if (this.myGoObject.SelectionObject != null)
        this.myCenter = this.myGoObject.SelectionObject.Center;
      else
        this.myCenter = this.myGoObject.Center;
    }
  }

  public virtual MapLayoutLinkEnumerator Links
  {
    get
    {
      ArrayList a = new ArrayList();
      foreach (MapLayoutNetworkLink sourceLink in this.mySourceLinks)
        a.Add((object) sourceLink);
      foreach (MapLayoutNetworkLink destinationLink in this.myDestinationLinks)
      {
        if (!a.Contains((object) destinationLink))
          a.Add((object) destinationLink);
      }
      return this.GetLinkEnumerator(a);
    }
  }

  public MapLayoutNetwork Network
  {
    get => this.myMapLayoutNetwork;
    set => this.myMapLayoutNetwork = value;
  }

  public object NodeData
  {
    get => this.myNodeData;
    set => this.myNodeData = value;
  }

  public virtual MapLayoutNodeEnumerator Nodes
  {
    get
    {
      ArrayList a = new ArrayList();
      foreach (MapLayoutNetworkLink sourceLink in this.mySourceLinks)
      {
        MapLayoutNetworkNode fromNode = sourceLink.FromNode;
        if (fromNode != null && !a.Contains((object) fromNode))
          a.Add((object) fromNode);
      }
      foreach (MapLayoutNetworkLink destinationLink in this.myDestinationLinks)
      {
        MapLayoutNetworkNode toNode = destinationLink.ToNode;
        if (toNode != null && !a.Contains((object) toNode))
          a.Add((object) toNode);
      }
      return this.GetNodeEnumerator(a);
    }
  }

  public virtual MapLayoutLinkEnumerator SourceLinks => this.GetLinkEnumerator(this.mySourceLinks);

  public ArrayList SourceLinksList => this.mySourceLinks;

  public virtual MapLayoutNodeEnumerator Sources
  {
    get
    {
      ArrayList a = new ArrayList();
      foreach (MapLayoutNetworkLink sourceLink in this.mySourceLinks)
      {
        MapLayoutNetworkNode fromNode = sourceLink.FromNode;
        if (fromNode != null && !a.Contains((object) fromNode))
          a.Add((object) fromNode);
      }
      return this.GetNodeEnumerator(a);
    }
  }

  public int UserFlags
  {
    get => this.myFlags;
    set => this.myFlags = value;
  }

  public object UserObject
  {
    get => this.myNodeUserData;
    set => this.myNodeUserData = value;
  }
}
