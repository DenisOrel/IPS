// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.CompareTree.ICompareTreeSettingsService
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces;
using Intermech.Kernel.Search;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.Pdm.Compositions.CompareTree;

public interface ICompareTreeSettingsService
{
  ICompoitionSettings GetCompoitionSettings(Guid ruleID);

  void SetCompoitionSettings(IDBObject ruleObject, ICompoitionSettings newSettings);

  List<int> GetRelationTypes(Guid ruleID, int objectTypeID);

  List<int> GetChildobjectTypeIDs(Guid ruleID, int parentTypeID, int relationTypeID);

  List<Tuple<int, AttributeSourceTypes>> GetSortedAttributes(Guid ruleID, int parentTypeID);

  List<int> GetIDObjectAttributes(Guid ruleID, int objectTypeID);

  List<int> GetIDRelationAttributes(Guid ruleID, int parentTypeID, int relationTypeID);

  List<int> GetRelationCompareAttributes(Guid ruleID, int relationTypeID);

  List<int> GetObjectCompareAttributes(Guid ruleID, int objectTypeID);

  bool CheckExistsAttributes(Guid ruleID);
}
