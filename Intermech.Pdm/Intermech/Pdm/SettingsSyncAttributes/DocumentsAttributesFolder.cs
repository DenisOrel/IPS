// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.SettingsSyncAttributes.DocumentsAttributesFolder
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Client.Core;
using Intermech.Interfaces;
using Intermech.PropertyEditors;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Intermech.Pdm.SettingsSyncAttributes;

public class DocumentsAttributesFolder : CustomFolder
{
  private List<int> attrIDList = new List<int>();

  public DocumentsAttributesFolder(Guid aInstGuid, string aText, object aNodeParent)
    : base(aInstGuid, aText, aNodeParent, (object) null)
  {
    if (Statics.IconSrv == null)
      return;
    this.node.ImageIndex = Statics.IconSrv.IndexOf(Statics.CategoryAttributes, 0);
    this.node.SelectedImageIndex = this.node.ImageIndex;
  }

  public override object GetServerObject(IUserSession session)
  {
    return (object) session.GetAttributesGroupCollection(0, CoreConsts.FilterRecords);
  }

  public override void LoadDataTable(bool reload)
  {
    this.attrIDList.Clear();
    foreach (IMSAttribute4 imsAttribute4 in MetaDataHelper.GetObjectTypeChildrenIDRecursive(new Guid("cad00070-306c-11d8-b4e9-00304f19f545")).Select<int, List<IMSAttribute4ObjectType>>(new Func<int, List<IMSAttribute4ObjectType>>(MetaDataHelper.GetAttribute4ObjectTypeList)).SelectMany<List<IMSAttribute4ObjectType>, IMSAttribute4ObjectType>((Func<List<IMSAttribute4ObjectType>, IEnumerable<IMSAttribute4ObjectType>>) (allAttr => allAttr.Where<IMSAttribute4ObjectType>((Func<IMSAttribute4ObjectType, bool>) (attr => !this.attrIDList.Contains(attr.AttributeID))))))
      this.attrIDList.Add(imsAttribute4.AttributeID);
    this.attrIDList.Add(-14);
  }

  public override void PopulateCallback(bool reload)
  {
    foreach (int attrId in this.attrIDList)
    {
      IMSAttributeType attributeType = MetaDataHelper.GetAttributeType(attrId);
      DocumentsAttributeFolder documentsAttributeFolder = new DocumentsAttributeFolder(this.instGuid, attributeType.Name, (object) this.Node, attributeType.FieldType, attrId);
    }
  }
}
