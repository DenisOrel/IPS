// Decompiled with JetBrains decompiler
// Type: Intermech.PdmConfigurator.PdmOptionsView
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

[ViewDescriptionProvider(typeof (PdmOptionsView.PdmOptionsViewDescriptionProvider))]
public sealed class PdmOptionsView : ObjectsViewBase
{
  private static int _imageIndex = -1;

  public override string Caption
  {
    [DebuggerStepThrough] get => PdmOptionObjectNode.NodeName;
  }

  public override int OrderID
  {
    [DebuggerStepThrough] get => 1;
  }

  protected override int StateStreamCategoryID => 1;

  public override string StateStreamPrefix => nameof (PdmOptionsView);

  public override int ImageIndex
  {
    get
    {
      if (PdmOptionsView._imageIndex >= 0)
        return PdmOptionsView._imageIndex;
      PdmOptionsView._imageIndex = (ServicesManager.GetService(typeof (INamedImageList)) as INamedImageList).ImageIndex("imgPdmConfigurator.Options");
      return PdmOptionsView._imageIndex;
    }
  }

  public override ContentType ViewContentType
  {
    get => ContentType.NonFolders;
    set
    {
    }
  }

  private sealed class PdmOptionsViewDescriptionProvider : BaseViewDescriptionProvider
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
        Caption = PdmOptionObjectNode.NodeName,
        ImageIndex = namedImageList != null ? namedImageList.ImageIndex("imgPdmConfigurator.Options") : -1,
        OrderID = 1
      };
    }
  }
}
