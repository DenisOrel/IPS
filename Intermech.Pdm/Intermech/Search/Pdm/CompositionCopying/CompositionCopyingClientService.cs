// Decompiled with JetBrains decompiler
// Type: Intermech.Search.Pdm.CompositionCopying.CompositionCopyingClientService
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces.Client;
using Intermech.Search.Utilities;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.Search.Pdm.CompositionCopying;

public sealed class CompositionCopyingClientService : ICompositionCopyingClientService
{
  private ICurrentUserAndRole _currentUserAndRole;
  private IFiltrationService _filtrationService;
  private INavigatorClientService _navigatorClientService;

  public CompositionCopyingClientService(
    ICurrentUserAndRole currentUserAndRole,
    IFiltrationService filtrationService,
    INavigatorClientService navigatorClientService)
  {
    if (filtrationService == null)
      throw new ArgumentNullException(nameof (filtrationService));
    if (navigatorClientService == null)
      throw new ArgumentNullException(nameof (navigatorClientService));
    this._currentUserAndRole = currentUserAndRole;
    this._filtrationService = filtrationService;
    this._navigatorClientService = navigatorClientService;
  }

  public void CreateCompositionByPrototype(
    long objectVersionID,
    int[] allowableForCreateCopyObjectTypes,
    int[] relationTypes)
  {
    if (ObjectHelper.IsUnknownObjectID(objectVersionID))
      throw new ArgumentException();
    if (allowableForCreateCopyObjectTypes == null || allowableForCreateCopyObjectTypes.Length == 0 || ObjectTypeHelper.IsAnyUnknownObjectTypeID((IEnumerable<int>) allowableForCreateCopyObjectTypes))
      throw new ArgumentException();
    if (relationTypes == null || relationTypes.Length == 0 || RelationTypeHelper.IsAnyUnknownRelationTypeID((IEnumerable<int>) relationTypes))
      throw new ArgumentException();
    using (CompositionCopyingWizardForm copyingWizardForm = new CompositionCopyingWizardForm())
    {
      copyingWizardForm.Initialize(this._currentUserAndRole, this._filtrationService, this._navigatorClientService);
      copyingWizardForm.ObjectVersionID = objectVersionID;
      copyingWizardForm.AllowableForCreateCopyObjectTypes = allowableForCreateCopyObjectTypes;
      copyingWizardForm.RelationTypes = relationTypes;
      int num = (int) copyingWizardForm.ShowDialog();
    }
  }
}
