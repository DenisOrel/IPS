// Decompiled with JetBrains decompiler
// Type: Intermech.Search.Mbom.MbomClientHelper
// Assembly: Intermech.Mbom, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 13559C9A-4DBC-479B-BA71-AFEA0247DEC7
// Assembly location: D:\IPS\Client\Intermech.Mbom.dll
// XML documentation location: D:\IPS\Client\Intermech.Mbom.xml

using Intermech.Navigator.Controls;
using Intermech.Navigator.DBObjects;
using System;

#nullable disable
namespace Intermech.Search.Mbom;

public static class MbomClientHelper
{
  public static NodeID GetObjectNodeID(NavigatorTreeNode navigatorTreeNode)
  {
    return navigatorTreeNode != null ? navigatorTreeNode.NodeID as NodeID : throw new ArgumentNullException(nameof (navigatorTreeNode));
  }

  public static NodeID GetParentObjectNodeID(NavigatorTreeNode navigatorTreeNode)
  {
    if (navigatorTreeNode == null)
      throw new ArgumentNullException(nameof (navigatorTreeNode));
    return navigatorTreeNode.Parent == null ? (NodeID) null : MbomClientHelper.GetObjectNodeID(navigatorTreeNode.Parent);
  }
}
