// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.SettingsSyncAttributes.DocumentsAttributeFolder
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Client.Core;
using Intermech.Interfaces;
using Intermech.PropertyEditors;
using System;

#nullable disable
namespace Intermech.Pdm.SettingsSyncAttributes;

public class DocumentsAttributeFolder : CustomFolder
{
  public DocumentsAttributeFolder(
    Guid aInstGuid,
    string aText,
    object aNodeParent,
    FieldTypes aType,
    int attrID)
    : base(aInstGuid, aText, aNodeParent, (object) attrID)
  {
    if (Statics.IconSrv == null)
      return;
    int num = Statics.IconSrv.IndexOf(3, -1, (object) aType);
    this.node.ImageIndex = num;
    this.node.SelectedImageIndex = num;
  }

  public override object GetServerObject(IUserSession session)
  {
    return (object) session.GetAttributeType(Convert.ToInt32(this.Id));
  }
}
