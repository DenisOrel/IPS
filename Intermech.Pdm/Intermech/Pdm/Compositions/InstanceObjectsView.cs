// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.InstanceObjectsView
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Pdm;
using Intermech.Navigator.DBObjects;

#nullable disable
namespace Intermech.Pdm.Compositions;

public class InstanceObjectsView : ObjectsViewBase
{
  private static int _imageIndex = -1;

  public override string Caption => PDMPluginConsts.ListInstancesWindow;

  public override int ImageIndex
  {
    get
    {
      if (InstanceObjectsView._imageIndex >= 0)
        return InstanceObjectsView._imageIndex;
      InstanceObjectsView._imageIndex = (ServicesManager.GetService(typeof (INamedImageList)) as INamedImageList).ImageIndex("imgObjects.PDM");
      return InstanceObjectsView._imageIndex;
    }
  }
}
