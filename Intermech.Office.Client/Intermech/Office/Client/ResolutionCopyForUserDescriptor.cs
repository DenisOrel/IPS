// Decompiled with JetBrains decompiler
// Type: Intermech.Office.Client.ResolutionCopyForUserDescriptor
// Assembly: Intermech.Office.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 87EC380F-A344-4B99-B3CC-05ECB303FAD4
// Assembly location: D:\IPS\Client\Intermech.Office.Client.dll

using Intermech.Diagnostics;
using Intermech.Navigator.DBObjects;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Persistence;
using System;
using System.Drawing;

#nullable disable
namespace Intermech.Office.Client;

public class ResolutionCopyForUserDescriptor : 
  SimpleCustomObjectDescriptor,
  IDescriptor,
  INodeItems,
  IPersistable,
  ICloneable,
  IDescriptorElementStatuses,
  IContextAware
{
  [CanBeNull]
  private static Image _resolutionForUserIcon;
  public readonly long UserID;

  [NotNull]
  public static Image ResolutionForUserIcon
  {
    get
    {
      return OfficeImages.GetImage(ref ResolutionCopyForUserDescriptor._resolutionForUserIcon, "UserResolution.png");
    }
  }

  public ResolutionCopyForUserDescriptor(long resolutionID, long userID)
    : base(resolutionID, Holder.UserNamesCache.GetUserName(userID), mainIcon: ResolutionCopyForUserDescriptor.ResolutionForUserIcon)
  {
    this.UserID = userID;
  }

  [NotNull]
  public override INode GetNode(INodeID nodeID, params object[] args)
  {
    CustomObjectNode node = (CustomObjectNode) base.GetNode(nodeID, args);
    node.Options = NodeOptions.None;
    return (INode) node;
  }
}
