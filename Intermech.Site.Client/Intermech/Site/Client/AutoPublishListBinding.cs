// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.AutoPublishListBinding
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Kernel.Search;
using Intermech.Navigator.DB;
using Intermech.Navigator.Parts;
using Intermech.Navigator.Selections;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Intermech.Site.Client;

internal sealed class AutoPublishListBinding : ITopBinding, IBindingEx, IBinding
{
  private Dictionary<int, List<long>> _objectIDs;
  private AutoPublishListNode _node;
  private ConditionStructure[] _topConditions;

  public AutoPublishListBinding(AutoPublishListNode node, Dictionary<int, List<long>> objectIDs)
  {
    this._objectIDs = objectIDs;
    this._node = node;
  }

  public ConditionStructure[] TopConditions
  {
    get
    {
      if (this._topConditions == null)
      {
        List<ConditionStructure> conditionStructureList = new List<ConditionStructure>();
        conditionStructureList.AddRange((IEnumerable<ConditionStructure>) new ConditionStructure[3]
        {
          new ConditionStructure(Intermech.Navigator.Selections.Consts.KindSelectionAttrID, RelationalOperators.AttributeExists, (object) null, LogicalOperators.AND, 0, false),
          new ConditionStructure(Intermech.Navigator.Selections.Consts.KindSelectionAttrID, RelationalOperators.Equal, (object) 4, LogicalOperators.AND, 0, false),
          new ConditionStructure(MetaDataHelper.GetAttributeTypeID("cad00155-306c-11d8-b4e9-00304f19f545"), RelationalOperators.Equal, (object) false, LogicalOperators.AND, 0, false)
        });
        this._topConditions = conditionStructureList.ToArray();
      }
      return this._topConditions;
    }
  }

  public void BindSelection(long selObjectID)
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
      sessionKeeper.Session.GetObject(selObjectID).Attributes.FindByID(Intermech.Navigator.Selections.Consts.KindSelectionAttrID).Value = (object) 4;
  }

  public string GetCaption(int selTypeID) => Intermech.Navigator.DBObjectTypes.Helper.GetObjectTypeName(selTypeID);

  public object GetData(Type dataFormat) => (object) null;

  public BindingType BindingType => BindingType.Selections;

  public ConditionStructure[] GetConditions(long selObjectID)
  {
    if (this._objectIDs.Count > 0)
    {
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        ConditionStructure[] conditionStructures = ((ISelectionsService) ServicesManager.GetService(typeof (ISelectionsService))).GetConditionStructures((object) sessionKeeper.Session, selObjectID);
        ConditionStructure joinedCondition = new ConditionStructure(-2, RelationalOperators.In, (object) this._objectIDs.Values.SelectMany<List<long>, long>((Func<List<long>, IEnumerable<long>>) (x => (IEnumerable<long>) x)).ToArray<long>(), LogicalOperators.AND, 0, false);
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

  public INodePart GetPart(IConditionsProvider conditionProvider) => (INodePart) null;

  public string ViewCaption => "Найденные объекты";

  public List<PartSlot> CreateNonFolderSlots(IConditionsProvider conditionProvider)
  {
    return this._node.CreateNonFolderSlots(conditionProvider);
  }
}
