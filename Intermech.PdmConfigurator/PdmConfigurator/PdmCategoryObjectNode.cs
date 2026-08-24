// Decompiled with JetBrains decompiler
// Type: Intermech.PdmConfigurator.PdmCategoryObjectNode
// Assembly: Intermech.PdmConfigurator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: B5CB2E26-657B-4329-B46C-77AE46A32171
// Assembly location: D:\IPS\Client\Intermech.PdmConfigurator.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Kernel.Search;
using Intermech.Localization;
using Intermech.Navigator;
using Intermech.Navigator.DBObjects;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Parts;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.PdmConfigurator;

public sealed class PdmCategoryObjectNode : ObjectNode
{
  public static readonly string NodeName = LocalizationHolder.rm.GetString("PdmConfigurator_19");
  private ConditionStructure[] _conditions;
  protected new AdvancedServiceContainer _services = new AdvancedServiceContainer();

  public PdmCategoryObjectNode(int objTypeID, long objID)
    : base(objTypeID, objID)
  {
    this._services.AddService(typeof (PdmCategoryObjectID), (object) new PdmCategoryObjectID(objID));
    this._conditions = new ConditionStructure[1]
    {
      new ConditionStructure(Intermech.Interfaces.PdmConfigurator.Consts.attributeCategoryLinkID, RelationalOperators.Equal, (object) this._objID, (object) null, LogicalOperators.NONE, 0, true, AttributeSourceTypes.Auto, ColumnContents.ID)
    };
  }

  public override IServiceProvider Services
  {
    get => (IServiceProvider) this._services;
    set => this._services.AdvancedProvider = value;
  }

  protected override List<PartSlot> CreateNonFolderSlots()
  {
    IViewState service = this.Services != null ? this.Services.GetService(typeof (IViewState)) as IViewState : (IViewState) null;
    if (service == null || (service.ViewState & ViewStateFlags.NodeInViews) == ViewStateFlags.None)
      return (List<PartSlot>) null;
    return new List<PartSlot>()
    {
      new PartSlot(new Guid("{7F78301F-D7BB-4E85-ADA5-DAB876BCF417}"), (INodePart) new ObjectsPart(Intermech.Interfaces.PdmConfigurator.Consts.objtypeOptionID, this._conditions, this.Services))
    };
  }

  public override NodeColumnCollection GetDefaultColumns(ContentType content)
  {
    IViewState service1 = this.Services != null ? this.Services.GetService(typeof (IViewState)) as IViewState : (IViewState) null;
    ViewStateFlags viewStateFlags = service1 != null ? service1.ViewState : ViewStateFlags.None;
    if ((viewStateFlags & ViewStateFlags.NodeInViews) != ViewStateFlags.NodeInViews && (viewStateFlags & ViewStateFlags.InParametersCard) != ViewStateFlags.InParametersCard)
      return Utils.CaptionColumnOnly(NodeColumnSortOrder.Ascending);
    NodeColumnCollection defaultColumns = new NodeColumnCollection();
    Guid columnSchemeGuid1 = Intermech.Navigator.Consts.ObjectObligatoryColumnSchemeGuid;
    Guid columnSchemeGuid2 = Intermech.Navigator.Consts.CurrentObjectColumnSchemeGuid;
    IColumnSchemes service2 = (IColumnSchemes) ServicesManager.GetService(typeof (IColumnSchemes));
    defaultColumns.Add(service2.CreateColumn(columnSchemeGuid1, (object) ObligatoryObjectAttributes.F_OBJECT_ID), 90);
    defaultColumns.Add(service2.CreateColumn(columnSchemeGuid2, (object) Intermech.Interfaces.PdmConfigurator.Consts.attributeOptionDataTypeID), 125);
    defaultColumns.Add(service2.CreateColumn(columnSchemeGuid2, (object) Intermech.Interfaces.PdmConfigurator.Consts.attributeOptionCodeID), 125);
    defaultColumns.Add(service2.CreateColumn(columnSchemeGuid1, (object) ObligatoryObjectAttributes.CAPTION, NodeColumnSortOrder.Ascending, 0), 400);
    return defaultColumns;
  }
}
