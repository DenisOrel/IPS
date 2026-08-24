// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.CompareTree.ColumnsConfiguration
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Infralution.Controls.VirtualTree;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Configuration;
using System;

#nullable disable
namespace Intermech.Pdm.Compositions.CompareTree;

internal sealed class ColumnsConfiguration
{
  private IConfiguration _config;

  public ColumnsConfiguration(int compareTypeID)
  {
    if (!(ServicesManager.GetService(typeof (IConfigurationManager)) is IConfigurationManager service))
      return;
    string name = $"PDM_CompareTreeView_ColumnsConfiguration_{compareTypeID}";
    this._config = service.Open(name);
    if (this._config != null)
      return;
    this._config = service.Create(name);
  }

  private int DefaultColumnWidth(int attributeID)
  {
    if (attributeID < 0)
    {
      switch (attributeID)
      {
        case -50:
          return 350;
        case -6:
          return 20;
        case -2:
          return 75;
      }
    }
    else if (attributeID == ControlsHelper.AttributeChangesID)
      return 40;
    return 150;
  }

  public void SaveColumns(params Intermech.VirtualTreeView.VirtualTreeView[] views)
  {
    this._config.Clear();
    foreach (Intermech.VirtualTreeView.VirtualTreeView view in views)
    {
      int tag = (int) view.Tag;
      int num = 0;
      foreach (Column column in view.Columns)
      {
        string str = $"{num};{column.Width}";
        this._config.SetProperty(this.MakeColumnKey(Convert.ToInt32(column.Name), tag), str);
        ++num;
      }
    }
  }

  private string MakeColumnKey(int attributeID, int treeViewID) => $"col{treeViewID}{attributeID}";

  public int GetColumnWidth(int attributeID, int treeViewID)
  {
    string property = this._config.GetProperty(this.MakeColumnKey(attributeID, treeViewID));
    if (!string.IsNullOrEmpty(property))
    {
      string[] strArray = property.Split(';');
      if (strArray.Length > 1)
        return Convert.ToInt32(strArray[1]);
    }
    return this.DefaultColumnWidth(attributeID);
  }
}
