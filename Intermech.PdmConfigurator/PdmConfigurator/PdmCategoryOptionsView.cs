// Decompiled with JetBrains decompiler
// Type: Intermech.PdmConfigurator.PdmCategoryOptionsView
// Assembly: Intermech.PdmConfigurator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: B5CB2E26-657B-4329-B46C-77AE46A32171
// Assembly location: D:\IPS\Client\Intermech.PdmConfigurator.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Navigator.DBObjects;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Views;
using System;
using System.Diagnostics;

#nullable disable
namespace Intermech.PdmConfigurator;

[ViewDescriptionProvider(typeof (PdmCategoryOptionsView.PdmCategoryOptionsViewDescriptionProvider))]
public sealed class PdmCategoryOptionsView : ObjectsViewBase
{
  private static int _imageIndex = -1;

  public override string Caption
  {
    [DebuggerStepThrough] get => PdmCategoryObjectNode.NodeName;
  }

  public override int OrderID
  {
    [DebuggerStepThrough] get => 0;
  }

  protected override int StateStreamCategoryID => Intermech.PdmConfigurator.PdmConfigurator.CategoryAllCategoryOptionsNode;

  public override string StateStreamPrefix => nameof (PdmCategoryOptionsView);

  public override int ImageIndex
  {
    get
    {
      if (PdmCategoryOptionsView._imageIndex >= 0)
        return PdmCategoryOptionsView._imageIndex;
      PdmCategoryOptionsView._imageIndex = (ServicesManager.GetService(typeof (INamedImageList)) as INamedImageList).ImageIndex("imgPdmConfigurator.Options");
      return PdmCategoryOptionsView._imageIndex;
    }
  }

  private sealed class PdmCategoryOptionsViewDescriptionProvider : BaseViewDescriptionProvider
  {
    public override ViewDescription DoGetViewDescription(
      ISelectedItems selectedItems,
      IServiceProvider serviceProvider)
    {
      if (!(serviceProvider.GetService(typeof (INamedImageList)) is INamedImageList service))
        service = ServicesManager.GetService(typeof (INamedImageList)) as INamedImageList;
      INamedImageList namedImageList = service;
      return new ViewDescription()
      {
        Caption = PdmCategoryObjectNode.NodeName,
        ImageIndex = namedImageList != null ? namedImageList.ImageIndex("imgPdmConfigurator.Options") : -1,
        OrderID = 0
      };
    }
  }
}
