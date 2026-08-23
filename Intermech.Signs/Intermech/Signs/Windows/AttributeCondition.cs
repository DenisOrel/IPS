// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Windows.AttributeCondition
// Assembly: Intermech.Signs, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: A3C02709-D794-49CE-8C55-5624449406B7
// Assembly location: D:\IPS\Client\Intermech.Signs.dll
// XML documentation location: D:\IPS\Client\Intermech.Signs.xml

using Intermech.Interfaces;
using Intermech.Kernel.Search;
using System;

#nullable disable
namespace Intermech.Signs.Windows;

/// <summary>Базовый класс для классов-условий</summary>
internal abstract class AttributeCondition : IAttributeCondition
{
  protected int attributeID;

  public AttributeCondition(int attributeID) => this.attributeID = attributeID;

  public void SetConditionStructure(
    IUserSession session,
    ConditionStructure[] structures,
    ref bool signed)
  {
    this.SetConditionStructure(session, this.FindCondition(structures), ref signed);
  }

  private ConditionStructure FindCondition(ConditionStructure[] structures)
  {
    if (structures == null)
      return ConditionStructure.Empty;
    for (int index = 0; index < structures.Length; ++index)
    {
      ConditionStructure structure = structures[index];
      if (structure.Attribute != null && (structure.Attribute is int && (int) structure.Attribute == this.attributeID || structure.Attribute is Guid && MetaDataHelper.GetAttributeTypeID((Guid) structure.Attribute) == this.attributeID))
        return structures[index];
    }
    return ConditionStructure.Empty;
  }

  public abstract ConditionStructure GetConditionStricture();

  public abstract void Clear();

  protected abstract void SetConditionStructure(
    IUserSession session,
    ConditionStructure cs,
    ref bool signed);
}
