// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.SettingsSyncAttributes.DocumentsTypesFolder
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Client.Core;
using Intermech.Holders;
using Intermech.Interfaces;
using Intermech.PropertyEditors;
using System;
using System.Data;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Pdm.SettingsSyncAttributes;

public class DocumentsTypesFolder : CustomFolder
{
  public DocumentsTypesFolder(Guid aInstGuid, string aText, object aNodeParent)
    : base(aInstGuid, aText, aNodeParent, (object) -1)
  {
    if (Statics.IconSrv == null)
      return;
    this.node.ImageIndex = Statics.IconSrv.IndexOf(Statics.CategoryObjectTypes, 0);
    this.node.SelectedImageIndex = this.node.ImageIndex;
  }

  public override object GetServerObject(IUserSession session)
  {
    return (object) session.GetObjectTypeCollection((int) this.Id, CoreConsts.FilterRecords);
  }

  public override void LoadDataTable(bool reload)
  {
    this.dataTable = DataHolders.ObjectTypesHolder.LoadData((reload ? 1 : 0) != 0, (object) MetaDataHelper.GetObjectTypeID("cad00070-306c-11d8-b4e9-00304f19f545"));
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

  public override void ConstructPages(TabControl tabControl)
  {
    TabControlProcessor.AssignTabPages(tabControl, (object) TabPagesHolder.TabPages(this.instGuid).ListTabPage);
  }

  public override int ExportCategoryValue => 17;

  public override int ListCategoryValue => 17;
}
