// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.PortalNavigator.PublishAttributesColumnScheme
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Interfaces.WebPortal;
using Intermech.Navigator.DBObjects;
using Intermech.Navigator.Interfaces;
using System;
using System.Collections;
using System.Collections.Specialized;

#nullable disable
namespace Intermech.Site.Client.PortalNavigator;

internal class PublishAttributesColumnScheme : INodeColumnScheme
{
  private IDictionary _transforms = (IDictionary) new HybridDictionary();

  public virtual string Name => string.Empty;

  public string ColumnIDToPersistName(object columnID)
  {
    return columnID is PortalAttributeType ? ((PortalAttributeType) columnID).ToString() : string.Empty;
  }

  public object PersistNameToColumnID(string persistName)
  {
    return persistName != string.Empty ? (object) new PortalAttributeType(persistName) : (object) null;
  }

  public NodeColumn CreateColumn(Guid schemeGuid, object columnID)
  {
    return this.CreateColumn(schemeGuid, columnID, NodeColumnSortOrder.None, -1);
  }

  public NodeColumn CreateColumn(
    Guid schemeGuid,
    object columnID,
    NodeColumnSortOrder sortOrder,
    int sortIndex)
  {
    if (!(columnID is PortalAttributeType))
      return (NodeColumn) null;
    PortalAttributeType portalAttributeType = (PortalAttributeType) columnID;
    return (NodeColumn) new PublishNodeColumn(schemeGuid, columnID, Intermech.Navigator.DBObjects.Helper.ConvertType(portalAttributeType.Type), portalAttributeType.Type, portalAttributeType.Name, sortOrder, sortIndex);
  }

  public virtual INodeColumnTransform GetDefaultTransform(object columnID)
  {
    lock (this._transforms)
    {
      if (this._transforms.Count == 0)
      {
        SiteNameTransform siteNameTransform = new SiteNameTransform();
        foreach (Guid siteCodeAttribute in PortalConsts.SiteCodeAttributes)
          this._transforms.Add((object) siteCodeAttribute, (object) siteNameTransform);
        this._transforms.Add((object) PortalConsts.attributePublishInComposition, (object) new BooleanTransform());
      }
      if (columnID is PortalAttributeType)
      {
        Guid key = new Guid(((PortalAttributeType) columnID).GUID);
        if (this._transforms.Contains((object) key))
          return (INodeColumnTransform) this._transforms[(object) key];
      }
      return (INodeColumnTransform) null;
    }
  }
}
