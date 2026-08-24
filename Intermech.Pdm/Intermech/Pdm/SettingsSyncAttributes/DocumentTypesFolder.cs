// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.SettingsSyncAttributes.DocumentTypesFolder
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Client.Core;
using Intermech.Holders;
using Intermech.Interfaces;
using Intermech.PropertyEditors;
using System;
using System.Data;

#nullable disable
namespace Intermech.Pdm.SettingsSyncAttributes;

public class DocumentTypesFolder : CustomFolder
{
  public DocumentTypesFolder(Guid aInstGuid, string aText, object aNodeParent, int id)
    : base(aInstGuid, aText, aNodeParent, (object) id)
  {
    if (Statics.IconSrv == null)
      return;
    this.node.ImageIndex = Statics.IconSrv.IndexOf(4, id);
    this.node.SelectedImageIndex = this.node.ImageIndex;
  }

  public override object GetServerObject(IUserSession session)
  {
    return (object) session.GetObjectType((int) this.Id);
  }

  public override void LoadDataTable(bool reload)
  {
    this.dataTable = DataHolders.ObjectTypesHolder.LoadData((reload ? 1 : 0) != 0, this.Id);
  }

  public override void PopulateCallback(bool reload)
  {
    ISelectorFilter treeView = this.Node.TreeView as ISelectorFilter;
    foreach (DataRow row in (InternalDataCollectionBase) this.dataTable.Rows)
    {
      if (treeView == null || treeView != null && treeView.IsInFilter(this.ListCategoryValue, (object) Convert.ToInt32(row["F_OBJECT_TYPE"])))
      {
        DocumentTypesFolder documentTypesFolder = new DocumentTypesFolder(this.instGuid, row["F_OBJ_TYPE_NAME"].ToString(), (object) this.Node, Convert.ToInt32(row["F_OBJECT_TYPE"]));
      }
    }
  }

  public override int Category => 17;
}
