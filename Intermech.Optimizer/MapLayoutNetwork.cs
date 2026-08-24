// Decompiled with JetBrains decompiler
// Type: Intermech.Map.Layout.MapLayoutNetwork
// Assembly: Intermech.Optimizer, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: F2F8A027-9638-497B-A691-BEF61B30B332
// Assembly location: D:\IPS\Client\Intermech.Optimizer.dll
// XML documentation location: D:\IPS\Client\Intermech.Optimizer.xml

using System;
using System.Collections;

#nullable disable
namespace Intermech.Map.Layout;

[Serializable]
public class MapLayoutNetwork
{
  private Hashtable myMapGoObjToLink;
  private Hashtable myMapGoObjToNode;
  private ArrayList myNetworkLinks;
  private ArrayList myNetworkNodes;

  public MapLayoutNetwork()
  {
    this.myNetworkNodes = new ArrayList();
    this.myNetworkLinks = new ArrayList();
    this.myMapGoObjToNode = new Hashtable();
    this.myMapGoObjToLink = new Hashtable();
  }

  public MapLayoutNetwork(IMapCollection collection)
  {
    this.myNetworkNodes = new ArrayList();
    this.myNetworkLinks = new ArrayList();
    this.myMapGoObjToNode = new Hashtable();
    this.myMapGoObjToLink = new Hashtable();
    this.AddNodesAndLinksFromCollection(collection, true);
  }

  public MapLayoutNetworkLink AddLink(IMapLink ilink)
  {
    if (ilink == null)
      return (MapLayoutNetworkLink) null;
    MapLayoutNetworkLink link1 = this.FindLink(ilink.MapObject);
    if (link1 == null)
    {
      MapLayoutNetworkLink link2 = new MapLayoutNetworkLink();
      link2.MapObject = ilink.MapObject;
      if (ilink.FromNode != null && ilink.FromNode.MapObject is MapGroup mapObject1)
      {
        MapLayoutNetworkNode layoutNetworkNode = this.AddNode((MapObject) mapObject1);
        link2.FromNode = layoutNetworkNode;
      }
      if (ilink.ToNode != null && ilink.ToNode.MapObject is MapGroup mapObject2)
      {
        MapLayoutNetworkNode layoutNetworkNode = this.AddNode((MapObject) mapObject2);
        link2.ToNode = layoutNetworkNode;
      }
      this.AddLink(link2);
      return link2;
    }
    if (ilink.FromNode != null)
    {
      if (ilink.FromNode.MapObject is MapGroup mapObject3)
      {
        MapLayoutNetworkNode layoutNetworkNode = this.AddNode((MapObject) mapObject3);
        link1.FromNode = layoutNetworkNode;
      }
    }
    else
      link1.FromNode = (MapLayoutNetworkNode) null;
    if (ilink.ToNode != null)
    {
      if (!(ilink.ToNode.MapObject is MapGroup mapObject4))
        return link1;
      MapLayoutNetworkNode layoutNetworkNode = this.AddNode((MapObject) mapObject4);
      link1.ToNode = layoutNetworkNode;
      return link1;
    }
    link1.ToNode = (MapLayoutNetworkNode) null;
    return link1;
  }

  public void AddLink(MapLayoutNetworkLink link)
  {
    if (link == null)
      return;
    this.myNetworkLinks.Add((object) link);
    MapObject mapObject = link.MapObject;
    if (mapObject != null && this.FindLink(mapObject) == null)
      this.myMapGoObjToLink[(object) mapObject] = (object) link;
    link.ToNode?.AddSourceLink(link);
    link.FromNode?.AddDestinationLink(link);
    link.Network = this;
  }

  public MapLayoutNetworkNode AddNode(MapObject node)
  {
    if (node == null)
      return (MapLayoutNetworkNode) null;
    MapLayoutNetworkNode node1 = this.FindNode(node);
    if (node1 == null)
    {
      node1 = new MapLayoutNetworkNode();
      node1.MapObject = node;
      this.AddNode(node1);
    }
    return node1;
  }

  public void AddNode(MapLayoutNetworkNode node)
  {
    if (node == null)
      return;
    this.myNetworkNodes.Add((object) node);
    MapObject mapObject = node.MapObject;
    if (mapObject != null)
      this.myMapGoObjToNode[(object) mapObject] = (object) node;
    node.Network = this;
  }

  public void AddNodesAndLinksFromCollection(IMapCollection collection, bool onlytruenodes)
  {
    foreach (MapObject mapObject in (IEnumerable) collection)
    {
      if (!(mapObject is IMapLink) && (!onlytruenodes || mapObject is IMapNode) && this.FindNode(mapObject) == null)
        this.AddNode(new MapLayoutNetworkNode()
        {
          Network = this,
          MapObject = mapObject
        });
    }
    foreach (MapObject mapObject1 in (IEnumerable) collection)
    {
      if (mapObject1 is IMapLink)
      {
        IMapPort fromPort = ((IMapLink) mapObject1).FromPort;
        IMapPort toPort = ((IMapLink) mapObject1).ToPort;
        if (fromPort != null && toPort != null && fromPort.MapObject != null && toPort.MapObject != null && this.FindLink(mapObject1) == null)
        {
          MapObject mapObject2 = fromPort.MapObject;
          while (mapObject2.Parent != null && this.FindNode(mapObject2) == null)
            mapObject2 = (MapObject) mapObject2.Parent;
          MapObject mapObject3 = toPort.MapObject;
          while (mapObject3.Parent != null && this.FindNode(mapObject3) == null)
            mapObject3 = (MapObject) mapObject3.Parent;
          if (mapObject2 != mapObject3)
          {
            MapLayoutNetworkNode node1 = this.FindNode(mapObject2);
            MapLayoutNetworkNode node2 = this.FindNode(mapObject3);
            if (node1 != null && node2 != null)
              this.LinkNodes(node1, node2, mapObject1);
          }
        }
      }
    }
  }

  public void CommitLinks()
  {
    foreach (MapLayoutNetworkLink link in this.Links)
      link.CommitPosition();
  }

  public void CommitNodes()
  {
    foreach (MapLayoutNetworkNode node in this.Nodes)
      node.CommitPosition();
  }

  public void CommitNodesAndLinks()
  {
    this.CommitNodes();
    this.CommitLinks();
  }

  public void DeleteArtificialNodes()
  {
    ArrayList arrayList = new ArrayList();
    foreach (MapLayoutNetworkNode node in this.Nodes)
    {
      if (node.MapObject == null)
        arrayList.Add((object) node);
    }
    foreach (MapLayoutNetworkNode node in arrayList)
      this.DeleteNode(node);
    arrayList.Clear();
    foreach (MapLayoutNetworkLink link in this.Links)
    {
      if (link.MapObject == null)
        arrayList.Add((object) link);
    }
    foreach (MapLayoutNetworkLink link in arrayList)
      this.DeleteLink(link);
  }

  public void DeleteLink(IMapLink ilink)
  {
    if (ilink == null)
      return;
    MapLayoutNetworkLink link = this.FindLink(ilink.MapObject);
    if (link == null)
      return;
    this.DeleteLink(link);
  }

  public void DeleteLink(MapLayoutNetworkLink link)
  {
    if (link == null)
      return;
    link.ToNode?.DeleteSourceLink(link);
    link.FromNode?.DeleteDestinationLink(link);
    int index = this.myNetworkLinks.IndexOf((object) link);
    if (index == -1)
      return;
    this.myNetworkLinks.RemoveAt(index);
    MapObject mapObject = link.MapObject;
    if (mapObject == null || this.FindLink(mapObject) != link)
      return;
    this.myMapGoObjToLink.Remove((object) mapObject);
  }

  public void DeleteNode(MapObject node)
  {
    if (node == null)
      return;
    MapLayoutNetworkNode node1 = this.FindNode(node);
    if (node1 == null)
      return;
    this.DeleteNode(node1);
  }

  public void DeleteNode(MapLayoutNetworkNode node)
  {
    if (node == null)
      return;
    int index1 = this.myNetworkNodes.IndexOf((object) node);
    if (index1 == -1)
      return;
    this.myNetworkNodes.RemoveAt(index1);
    MapObject mapObject = node.MapObject;
    if (mapObject != null)
      this.myMapGoObjToNode.Remove((object) mapObject);
    ArrayList sourceLinksList = node.SourceLinksList;
    for (int index2 = sourceLinksList.Count - 1; index2 >= 0; --index2)
      this.DeleteLink((MapLayoutNetworkLink) sourceLinksList[index2]);
    ArrayList destinationLinksList = node.DestinationLinksList;
    for (int index3 = destinationLinksList.Count - 1; index3 >= 0; --index3)
      this.DeleteLink((MapLayoutNetworkLink) destinationLinksList[index3]);
  }

  public void DeleteSelfLinks()
  {
    ArrayList arrayList = new ArrayList();
    foreach (MapLayoutNetworkLink link in this.Links)
    {
      if (link.FromNode == link.ToNode)
        arrayList.Add((object) link);
    }
    for (int index = 0; index < arrayList.Count; ++index)
      this.DeleteLink((MapLayoutNetworkLink) arrayList[index]);
  }

  public MapLayoutNetworkLink FindLink(MapObject obj)
  {
    return obj == null ? (MapLayoutNetworkLink) null : this.myMapGoObjToLink[(object) obj] as MapLayoutNetworkLink;
  }

  public MapLayoutNetworkNode FindNode(MapObject obj)
  {
    return obj == null ? (MapLayoutNetworkNode) null : this.myMapGoObjToNode[(object) obj] as MapLayoutNetworkNode;
  }

  public MapLayoutNetworkLink LinkNodes(
    MapLayoutNetworkNode fromNode,
    MapLayoutNetworkNode toNode,
    MapObject obj)
  {
    if (fromNode.Network != this || toNode.Network != this)
      return (MapLayoutNetworkLink) null;
    MapLayoutNetworkLink link = new MapLayoutNetworkLink();
    link.MapObject = obj;
    link.FromNode = fromNode;
    link.ToNode = toNode;
    this.AddLink(link);
    return link;
  }

  public void RemoveAllNodesAndLinks()
  {
    this.myNetworkNodes = new ArrayList();
    this.myNetworkLinks = new ArrayList();
    this.myMapGoObjToNode = new Hashtable();
    this.myMapGoObjToLink = new Hashtable();
  }

  public void ReverseLink(MapLayoutNetworkLink link)
  {
    MapLayoutNetworkNode fromNode = link.FromNode;
    MapLayoutNetworkNode toNode = link.ToNode;
    if (fromNode == null || toNode == null)
      return;
    fromNode.DeleteDestinationLink(link);
    toNode.DeleteSourceLink(link);
    link.ReverseLink();
    fromNode.AddSourceLink(link);
    toNode.AddDestinationLink(link);
  }

  protected Hashtable GoObjToLinkMap => this.myMapGoObjToLink;

  protected Hashtable GoObjToNodeMap => this.myMapGoObjToNode;

  public int LinkCount => this.myNetworkLinks.Count;

  public MapLayoutLinkEnumerator Links => new MapLayoutLinkEnumerator(this.myNetworkLinks);

  internal ArrayList LinksArray => this.myNetworkLinks;

  public int NodeCount => this.myNetworkNodes.Count;

  public MapLayoutNodeEnumerator Nodes => new MapLayoutNodeEnumerator(this.myNetworkNodes);

  internal ArrayList NodesArray => this.myNetworkNodes;
}
