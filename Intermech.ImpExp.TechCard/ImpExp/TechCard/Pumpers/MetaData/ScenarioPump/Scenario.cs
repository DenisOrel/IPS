// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.MetaData.ScenarioPump.Scenario
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.TechCard.Common;
using System;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.MetaData.ScenarioPump;

[Serializable]
internal class Scenario
{
  private readonly ScenarioProperty _property = new ScenarioProperty();
  public int key;
  public string Name;
  public ScenarioKind Kind;
  public ScenarioCell[,] Cells;
  public int ColCount;
  public int RowCount;

  public ScenarioProperty Property => this._property;

  public override string ToString()
  {
    if (!this.Name.Equals(string.Empty))
      return this.Name;
    string str = "Сценарий";
    Guid importingObjectType = ScenarioUtils.GetImportingObjectType(this);
    if (!importingObjectType.Equals(Guid.Empty))
    {
      try
      {
        if (TechcardConsts.Plugin.Imdi.ObjectTypes.ExistsByGuid(importingObjectType))
          str = TechcardConsts.Plugin.Imdi.ObjectTypes.GetByGuid(importingObjectType).ObjectName;
      }
      catch (Exception ex)
      {
        TechcardConsts.Plugin.appManager.AddWarningMessage($"Невозможно получить Наименование типа объекта {importingObjectType} по причине: {ex.Message}");
        if (ex is OutOfMemoryException)
          throw;
      }
    }
    return this.Name = str;
  }
}
