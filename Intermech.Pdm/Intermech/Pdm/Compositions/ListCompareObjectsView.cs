// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.ListCompareObjectsView
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Pdm;
using Intermech.Navigator.DBObjects;

#nullable disable
namespace Intermech.Pdm.Compositions;

internal class ListCompareObjectsView : ObjectsViewBase
{
  private static int _imageIndex = -1;

  public ListCompareObjectsView() => this._editingModeButtonItem.Visible = false;

  public override string Caption => PDMPluginConsts.ListCompareObjects;

  public override int ImageIndex
  {
    get
    {
      if (ListCompareObjectsView._imageIndex >= 0)
        return ListCompareObjectsView._imageIndex;
      ListCompareObjectsView._imageIndex = (ServicesManager.GetService(typeof (INamedImageList)) as INamedImageList).ImageIndex("imgCompCompare");
      return ListCompareObjectsView._imageIndex;
    }
  }
}
