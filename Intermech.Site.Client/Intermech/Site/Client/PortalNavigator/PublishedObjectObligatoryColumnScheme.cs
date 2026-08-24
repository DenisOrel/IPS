// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.PortalNavigator.PublishedObjectObligatoryColumnScheme
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Interfaces;
using Intermech.Interfaces.WebPortal;
using Intermech.Kernel.Search;
using Intermech.Localization;
using Intermech.Navigator.DBObjects;
using Intermech.Navigator.Interfaces;
using System;
using System.Collections;
using System.Collections.Specialized;

#nullable disable
namespace Intermech.Site.Client.PortalNavigator;

internal class PublishedObjectObligatoryColumnScheme : INodeColumnScheme
{
  protected IDictionary transforms = (IDictionary) new HybridDictionary();

  public virtual string Name => LocalizationHolder.rm.GetString("Site.Client_37");

  public string ColumnIDToPersistName(object columnID)
  {
    switch (columnID)
    {
      case Guid _:
        return columnID.ToString();
      case ObligatoryObjectAttributes _:
        return ((int) columnID).ToString();
      default:
        return string.Empty;
    }
  }

  public object PersistNameToColumnID(string persistName)
  {
    if (persistName == string.Empty)
      return (object) null;
    return GuidHelper.IsGuid(persistName) ? (object) new Guid(persistName) : (object) (ObligatoryObjectAttributes) Convert.ToInt32(persistName);
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
    switch (columnID)
    {
      case Guid attrTypeGuid:
        IMSAttributeType attributeType = MetaDataHelper.GetAttributeType(attrTypeGuid);
        return attributeType == null ? (NodeColumn) null : (NodeColumn) new PublishNodeColumn(schemeGuid, columnID, Intermech.Navigator.DBObjects.Helper.ConvertType(attributeType.FieldType), attributeType.FieldType, attributeType.Name, sortOrder, sortIndex);
      case ObligatoryObjectAttributes _:
        return (NodeColumn) new PublishNodeColumn(schemeGuid, columnID, Intermech.Navigator.DBObjects.Helper.GetColumnType((ObligatoryObjectAttributes) columnID), Intermech.Navigator.DBObjects.Helper.GetColumnAttrType((ObligatoryObjectAttributes) columnID), ObligatoryObjectAttributesHelper.GetCaption((ObligatoryObjectAttributes) columnID), sortOrder, sortIndex);
      default:
        return (NodeColumn) null;
    }
  }

  public INodeColumnTransform GetDefaultTransform(object columnID)
  {
    lock (this.transforms)
    {
      this.CreateTransforms();
      return this.transforms.Contains(columnID) ? (INodeColumnTransform) this.transforms[columnID] : (INodeColumnTransform) null;
    }
  }

  protected virtual void CreateTransforms()
  {
    if (this.transforms.Count != 0)
      return;
    CaptionTransform captionTransform = new CaptionTransform();
    this.transforms.Add((object) -50, (object) captionTransform);
    this.transforms.Add((object) ObligatoryObjectAttributes.CAPTION, (object) captionTransform);
    this.transforms.Add((object) "F_CAPTION", (object) captionTransform);
    this.transforms.Add((object) ObligatoryObjectAttributes.F_OBJECT_TYPE, (object) new PortalObjectTypeNameTransform());
    SiteNameTransform siteNameTransform = new SiteNameTransform();
    this.transforms.Add((object) PortalConsts.attributeOwner, (object) siteNameTransform);
    this.transforms.Add((object) PortalConsts.attributeEnabledSites, (object) siteNameTransform);
    this.transforms.Add((object) PortalConsts.attributeCopyKeepers, (object) siteNameTransform);
    this.transforms.Add((object) PortalConsts.attributeParentSites, (object) siteNameTransform);
    this.transforms.Add((object) PortalConsts.attributeFirstPublishSite, (object) siteNameTransform);
    this.transforms.Add((object) PortalConsts.attributeCompositionOwner, (object) siteNameTransform);
    this.transforms.Add((object) PortalConsts.attributeCompositionParentSites, (object) siteNameTransform);
  }

  private NodeColumn CreateColumn(
    Guid schemeGuid,
    ObligatoryObjectAttributes columnID,
    NodeColumnSortOrder sortOrder,
    int sortIndex)
  {
    return (NodeColumn) new PublishNodeColumn(schemeGuid, (object) columnID, this.GetColumnType(columnID), this.GetColumnAttrType(columnID), ObligatoryObjectAttributesHelper.GetCaption(columnID), sortOrder, sortIndex);
  }

  private Type GetColumnType(ObligatoryObjectAttributes columnID) => Intermech.Navigator.DBObjects.Helper.GetColumnType(columnID);

  private FieldTypes GetColumnAttrType(ObligatoryObjectAttributes columnID)
  {
    return Intermech.Navigator.DBObjects.Helper.GetColumnAttrType(columnID);
  }
}
