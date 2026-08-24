// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.PortalNavigator.PublishNodeColumn
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Interfaces;
using Intermech.Interfaces.WebPortal;
using Intermech.Navigator.Interfaces;
using System;

#nullable disable
namespace Intermech.Site.Client.PortalNavigator;

[Serializable]
internal class PublishNodeColumn(
  Guid schemeGuid,
  object id,
  Type dataType,
  FieldTypes attrType,
  string caption,
  NodeColumnSortOrder sortOrder,
  int sortIndex) : NodeColumn(schemeGuid, id, dataType, attrType, caption, sortOrder, sortIndex)
{
  protected override IMSAttributeType GetAttributeType()
  {
    if (!(this.ID is PortalAttributeType))
      return base.GetAttributeType();
    PortalAttributeType id = (PortalAttributeType) this.ID;
    Guid attrTypeGuid = new Guid(id.GUID);
    IMSAttributeType attributeType = MetaDataHelper.GetAttributeType(attrTypeGuid);
    if (attributeType != null)
      return attributeType;
    return new IMSAttributeType()
    {
      AttributeGuid = attrTypeGuid,
      AttributeID = -10000,
      FieldType = id.Type,
      Name = id.Name
    };
  }
}
