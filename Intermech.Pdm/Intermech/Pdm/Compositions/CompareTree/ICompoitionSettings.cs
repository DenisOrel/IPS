// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.CompareTree.ICompoitionSettings
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Kernel.Search;
using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Intermech.Pdm.Compositions.CompareTree;

public interface ICompoitionSettings
{
  List<Tuple<int, int, List<int>>> ChildTypes { get; }

  List<int> GetChildTypes(int parentTypeID, int relationTypeID);

  List<int> GetObjectCompareAttributes(int objectTypeID);

  List<int> GetRelationCompareAttributes(int relationTypeID);

  List<int> GetIDObjectAttributes(int objectTypeID);

  List<int> GetIDRelationAttributes(int parentTypeID, int relationTypeID);

  List<Tuple<int, AttributeSourceTypes>> GetSortedAttributes(int parentTypeID);

  bool CheckExistsAttributes { get; set; }

  List<int> GetRelationTypes(int objectTypeID);

  ICompoitionSettings Clone();

  void Save(Stream stream);
}
