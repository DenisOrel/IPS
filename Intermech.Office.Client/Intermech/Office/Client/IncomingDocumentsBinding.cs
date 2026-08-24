// Decompiled with JetBrains decompiler
// Type: Intermech.Office.Client.IncomingDocumentsBinding
// Assembly: Intermech.Office.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 87EC380F-A344-4B99-B3CC-05ECB303FAD4
// Assembly location: D:\IPS\Client\Intermech.Office.Client.dll

using Intermech.Diagnostics;
using Intermech.Extensions;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Kernel.Search;
using Intermech.Navigator.DB;
using Intermech.Navigator.DBObjects;
using Intermech.Navigator.Parts;
using Intermech.Navigator.Selections;
using System;

#nullable disable
namespace Intermech.Office.Client;

internal sealed class IncomingDocumentsBinding : ITopBinding, IBinding, IBindingStateStream
{
  [CanBeNull]
  private readonly long[] _objectIDs;
  [CanBeNull]
  private readonly IServiceProvider _services;
  [CanBeNull]
  private ConditionStructure[] _topConditions;

  public IncomingDocumentsBinding([CanBeNull] IServiceProvider services, [CanBeNull] long[] objectIDs)
  {
    this._objectIDs = objectIDs;
    this._services = services;
  }

  [NotNull]
  public ConditionStructure[] TopConditions
  {
    get
    {
      if (this._topConditions == null)
        this._topConditions = ListFactory.Create<ConditionStructure>(new ConditionStructure(Intermech.Navigator.Selections.Consts.KindSelectionAttrID, RelationalOperators.AttributeExists, (object) null, LogicalOperators.AND, 0, false), new ConditionStructure(Intermech.Navigator.Selections.Consts.KindSelectionAttrID, RelationalOperators.Equal, (object) 3, LogicalOperators.AND, 0, false), new ConditionStructure(Intermech.Navigator.Selections.Consts.ObjectTypesAttrID, RelationalOperators.Equal, (object) "cad00070-306c-11d8-b4e9-00304f19f545", LogicalOperators.AND, 0, false), new ConditionStructure(MetaDataHelper.GetAttributeTypeID("cad00155-306c-11d8-b4e9-00304f19f545"), RelationalOperators.Equal, (object) false, LogicalOperators.AND, 0, false)).ToArray();
      return this._topConditions;
    }
  }

  public void BindSelection(long selObjectID)
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBObject iDbAttributable = sessionKeeper.Session.GetObject(selObjectID);
      iDbAttributable.AttributeByID(Intermech.Navigator.Selections.Consts.KindSelectionAttrID).Value = (object) 3;
      (iDbAttributable.GetAttributeByID(Intermech.Navigator.Selections.Consts.ObjectTypesAttrID) ?? iDbAttributable.Attributes.AddAttribute(Intermech.Navigator.Selections.Consts.ObjectTypesAttrID, false)).Value = (object) "cad00070-306c-11d8-b4e9-00304f19f545";
    }
  }

  public string GetCaption(int selTypeID) => Intermech.Navigator.DBObjectTypes.Helper.GetObjectTypeName(selTypeID);

  [CanBeNull]
  public object GetData(Type dataFormat) => (object) null;

  public BindingType BindingType => BindingType.Selections;

  public ConditionStructure[] GetConditions(long selObjectID)
  {
    if (this._objectIDs != null && this._objectIDs.Length != 0)
    {
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        ConditionStructure[] conditionStructures = ServicesManager.GetService<ISelectionsService>().GetConditionStructures((object) sessionKeeper.Session, selObjectID);
        ConditionStructure joinedCondition = new ConditionStructure(-2, RelationalOperators.In, (object) this._objectIDs, LogicalOperators.AND, 0, false);
        ConditionStructure[] conditions;
        if (conditionStructures == null)
          conditions = new ConditionStructure[1]
          {
            joinedCondition
          };
        else
          conditions = ConditionStructure.Join(joinedCondition, conditionStructures);
        return conditions;
      }
    }
    return new ConditionStructure[1]
    {
      new ConditionStructure(-2, RelationalOperators.Empty, (object) null, LogicalOperators.AND, 0, false)
    };
  }

  [NotNull]
  public INodePart GetPart(IConditionsProvider conditionProvider)
  {
    return (INodePart) new ObjectsPart(conditionProvider, this._services);
  }

  [NotNull]
  public string ViewCaption => "Найденные объекты";

  public int CategoryID => OfficeClientConsts.CategoryIncomingDocuments;

  public int CategoryType => 0;

  [NotNull]
  public string Prefix => string.Empty;
}
