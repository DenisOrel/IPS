// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.ContextComposition.ObjectContextCompositionNodeQuery
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Kernel.Search;
using Intermech.Navigator.DBObjects;
using Intermech.Navigator.Queries;

#nullable disable
namespace Intermech.Pdm.ContextComposition;

internal sealed class ObjectContextCompositionNodeQuery(
  INodeQuerySupport nodeQuerySupport,
  long objectVersionID,
  int objectTypeID,
  ConditionStructure[] conditions,
  int relationTypeID) : RelatedObjectsQuery(nodeQuerySupport, objectVersionID, objectTypeID, RelatedObjectsRole.Composition, relationTypeID, conditions)
{
}
