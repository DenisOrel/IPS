// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.MetaData.ScenarioPump.ScenarioBuilder
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using System;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.MetaData.ScenarioPump;

internal class ScenarioBuilder
{
  private readonly ScenarioDbRecordsHolder _recordsHolder;

  internal ScenarioBuilder(ScenarioDbRecordsHolder recordsHolder)
  {
    this._recordsHolder = recordsHolder != null ? recordsHolder : throw new ArgumentNullException(nameof (recordsHolder));
  }

  public Scenario Build(ScenarioDbRecord.TC_SCRIPTS script)
  {
    if (script == null)
      throw new ArgumentNullException(nameof (script));
    Scenario scenario = new Scenario()
    {
      key = script.Key,
      Name = script.Name,
      Kind = (ScenarioKind) script.Kind,
      ColCount = script.ColCount,
      RowCount = script.RowCount
    };
    int num = 0;
    switch (scenario.Kind)
    {
      case ScenarioKind.Rez:
        ++scenario.RowCount;
        break;
      case ScenarioKind.PerehConc:
        ++scenario.RowCount;
        break;
    }
    scenario.Cells = new ScenarioCell[scenario.ColCount, scenario.RowCount];
    if (scenario.RowCount == 0)
      return scenario;
    for (int ncol = 0; ncol < script.ColCount; ++ncol)
    {
      ScenarioDbRecord.TC_SCNAMECOL col = this._recordsHolder.FindCol(script.Key, ncol);
      scenario.Cells[ncol, 0] = col == null ? new ScenarioCell(string.Empty, CellValueType.Text) : new ScenarioCell(col.Name, CellValueType.Text, col.Width);
    }
    if (scenario.Kind == ScenarioKind.Rez)
    {
      string str = "ЧЕРН";
      if (TechPumpData.Entities.EntitiesList.ContainsKey("ЧЕРН"))
        str = TechPumpData.Entities.EntitiesList["ЧЕРН"].Name;
      scenario.Cells[0, 1] = new ScenarioCell(str, CellValueType.Text, scenario.Cells[0, 0].Width);
      scenario.Cells[1, 1] = new ScenarioCell("ЧЕРН", CellValueType.Code, scenario.Cells[1, 0].Width);
      ++num;
    }
    int width = scenario.Cells[0, 0].Width;
    for (int nrow = 1; nrow < script.RowCount; ++nrow)
    {
      ScenarioDbRecord.TC_SCNAMEROW row = this._recordsHolder.FindRow(script.Key, nrow);
      scenario.Cells[0, nrow + num] = row == null ? new ScenarioCell(string.Empty, CellValueType.Text, width) : new ScenarioCell(row.Name, CellValueType.Text, width);
    }
    for (int ncol = 1; ncol < script.ColCount; ++ncol)
    {
      for (int nrow = 1; nrow < script.RowCount; ++nrow)
      {
        ScenarioDbRecord.TC_SCNAMEROW row = this._recordsHolder.FindRow(script.Key, nrow);
        ScenarioDbRecord.TC_SCCELLS cell = row != null ? this._recordsHolder.FindCell(script.Key, ncol, row.Key) : (ScenarioDbRecord.TC_SCCELLS) null;
        if (cell != null)
        {
          ScenarioCell scenarioCell = new ScenarioCell(cell.Code, CellValueType.Code, scenario.Cells[ncol, 0].Width)
          {
            DefaultValue = cell.Default
          };
          if (this._recordsHolder.ScrCellRas.ContainsKey(cell.Key))
            scenarioCell.IsReCountButton = true;
          scenario.Cells[ncol, nrow + num] = scenarioCell;
        }
        else
          scenario.Cells[ncol, nrow + num] = new ScenarioCell(string.Empty, CellValueType.Text, scenario.Cells[ncol, 0].Width);
      }
    }
    if (scenario.Kind == ScenarioKind.PerehConc)
    {
      string str = "Тепр";
      if (TechPumpData.Entities.EntitiesList.ContainsKey("Тепр"))
        str = TechPumpData.Entities.EntitiesList["Тепр"].Name;
      scenario.Cells[0, scenario.RowCount - 1] = new ScenarioCell(str, CellValueType.Text, scenario.Cells[0, 0].Width);
      scenario.Cells[1, scenario.RowCount - 1] = new ScenarioCell("Тепр", CellValueType.Code, 400, 200)
      {
        Anchor = "Top, Left"
      };
    }
    scenario.Property.SlideId = script.SlideID;
    if (this._recordsHolder.ScrScriptXRef.ContainsKey(script.Key))
    {
      foreach (ScenarioDbRecord.TC_SCRIPTS_XREF tcScriptsXref in this._recordsHolder.ScrScriptXRef[script.Key])
      {
        scenario.Property.Catalog.CatalogId = tcScriptsXref.Catalog;
        scenario.Property.Catalog.Production = tcScriptsXref.Production;
        if (!scenario.Property.Catalog.FoldersId.Contains(tcScriptsXref.Level))
          scenario.Property.Catalog.FoldersId.Add(tcScriptsXref.Level);
      }
      if (scenario.Kind == ScenarioKind.Zagot && scenario.Property.Catalog.Production != 19)
        scenario.Kind = ScenarioKind.Unknown;
    }
    scenario.Property.IsReCountButton = true;
    if (this._recordsHolder.zscen.ContainsKey(scenario.key))
    {
      scenario.Property.VidDet = this._recordsHolder.zscen[scenario.key].VidDet;
      scenario.Property.VidZag = this._recordsHolder.zscen[scenario.key].ZagCode;
      string typeName = this._recordsHolder.zscen[scenario.key].TypeName;
      if (!typeName.Equals(string.Empty))
        scenario.Name = typeName;
      if (this._recordsHolder.zscen[scenario.key].Production != 19)
        scenario.Kind = ScenarioKind.Unknown;
    }
    return scenario;
  }
}
