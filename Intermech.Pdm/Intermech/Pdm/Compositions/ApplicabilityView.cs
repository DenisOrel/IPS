// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.ApplicabilityView
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Pdm;
using Intermech.Localization;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Views;
using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace Intermech.Pdm.Compositions;

[ViewDescriptionProvider(typeof (ApplicabilityView.ApplicabilityViewDescriptionProvider))]
internal sealed class ApplicabilityView : ContainsViewBase
{
  private string _caption = LocalizationHolder.rm.GetString("Pdm_46");
  private int _imageIndex = -1;

  public ApplicabilityView()
    : base(ContainsMode.Applicability)
  {
    if (!(ServicesManager.GetService(typeof (INamedImageList)) is INamedImageList service))
      return;
    this._imageIndex = service.ImageIndex("imgEntersTo");
  }

  public override int ImageIndex => this._imageIndex;

  public override string Caption => this._caption;

  public override int OrderID => 26;

  protected override List<Guid> GetPossibleRelationTypes(IUserSession session, int objType)
  {
    List<Guid> relationTypes = new List<Guid>();
    DataTable applicabilitiesList = session.GetRelationsApplicabilityCollection().GetApplicabilitiesList(-1, objType, -1);
    this.FillApplicability(session, applicabilitiesList, ref relationTypes);
    return relationTypes;
  }

  private sealed class ApplicabilityViewDescriptionProvider : BaseViewDescriptionProvider
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
        Caption = LocalizationHolder.rm.GetString("Pdm_46"),
        ImageIndex = namedImageList.ImageIndex("imgEntersTo"),
        OrderID = 26
      };
    }
  }
}
