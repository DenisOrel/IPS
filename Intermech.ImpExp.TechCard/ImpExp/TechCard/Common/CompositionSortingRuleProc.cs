// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Common.CompositionSortingRuleProc
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.Interfaces;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.TechCard.Common;

internal class CompositionSortingRuleProc
{
  private static CompositionsAutosortRule _sortingRule;
  private static readonly Dictionary<int, Dictionary<int, CompositionSortingRuleProc.TechChildRelationType>> SortingRuleCache = new Dictionary<int, Dictionary<int, CompositionSortingRuleProc.TechChildRelationType>>();

  public static void LoadSortingRules(IUserSession session)
  {
    if (CompositionSortingRuleProc._sortingRule != null || session == null)
      return;
    CompositionSortingRuleProc._sortingRule = new CompositionsAutosortRule();
    IDBObject dbObject = session.GetObject(new Guid("cad00693-306c-11d8-b4e9-00304f19f545"), false);
    long num = dbObject != null ? dbObject.ObjectID : -1L;
    int attributeTypeId = MetaDataHelper.GetAttributeTypeID("cad00692-306c-11d8-b4e9-00304f19f545");
    IDBAttribute attributeById = session.GetObject(session.RoleID).GetAttributeByID(attributeTypeId);
    object objectID = attributeById != null ? attributeById.Value : (object) num;
    if (objectID != null && !objectID.Equals((object) DBNull.Value))
    {
      CompositionSortingRuleProc._sortingRule.Load(session, (long) objectID, false);
      if (CompositionSortingRuleProc._sortingRule.ParentObjectTypes.Count == 0)
        CompositionSortingRuleProc._sortingRule.Load(session, (long) objectID, false);
    }
    CompositionSortingRuleProc.SortingRuleCache.Clear();
    foreach (ParentObjectType parentObjectType in CompositionSortingRuleProc._sortingRule.ParentObjectTypes)
    {
      if (parentObjectType != null && parentObjectType.ChildRelationTypes != null && parentObjectType.ChildRelationTypes.Count != 0)
      {
        Dictionary<int, CompositionSortingRuleProc.TechChildRelationType> dictionary = new Dictionary<int, CompositionSortingRuleProc.TechChildRelationType>();
        CompositionSortingRuleProc.SortingRuleCache.Add(parentObjectType.ObjectTypeID, dictionary);
        for (int index = 0; index < parentObjectType.ChildRelationTypes.Count; ++index)
        {
          ChildRelationType childRelationType = parentObjectType.ChildRelationTypes[index];
          if (childRelationType != null)
            dictionary.Add(childRelationType.RelationTypeID, new CompositionSortingRuleProc.TechChildRelationType(childRelationType, index));
        }
      }
    }
  }

  public static CompositionSortingRuleProc.TechChildRelationType GetChildSortingRule(
    int projTypeId,
    int relTypeId,
    IUserSession session)
  {
    if (CompositionSortingRuleProc._sortingRule == null)
      CompositionSortingRuleProc.LoadSortingRules(session);
    Dictionary<int, CompositionSortingRuleProc.TechChildRelationType> dictionary;
    if (!CompositionSortingRuleProc.SortingRuleCache.TryGetValue(projTypeId, out dictionary))
      return (CompositionSortingRuleProc.TechChildRelationType) null;
    CompositionSortingRuleProc.TechChildRelationType childSortingRule;
    dictionary.TryGetValue(relTypeId, out childSortingRule);
    return childSortingRule;
  }

  internal class TechChildRelationType
  {
    private readonly ChildRelationType _childRelType;
    private readonly int _childRelTypeIdx;
    private readonly Dictionary<int, long> _childObjCache = new Dictionary<int, long>();

    public TechChildRelationType(ChildRelationType childRelType, int childRelTypeIdx)
    {
      this._childRelType = childRelType;
      this._childRelTypeIdx = childRelTypeIdx;
    }

    public ChildRelationType ChildRelType => this._childRelType;

    public Dictionary<int, long> ChildObjCache => this._childObjCache;

    public int ChildRelTypeIdx => this._childRelTypeIdx;
  }
}
