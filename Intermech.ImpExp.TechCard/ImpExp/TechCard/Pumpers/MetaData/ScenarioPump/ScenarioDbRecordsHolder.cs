// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.MetaData.ScenarioPump.ScenarioDbRecordsHolder
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.MetaData.ScenarioPump;

internal class ScenarioDbRecordsHolder
{
  public readonly List<ScenarioDbRecord.TC_SCRIPTS> Scripts;
  public readonly List<ScenarioDbRecord.TC_SCCELLS> ScrCells;
  public readonly List<ScenarioDbRecord.TC_SCNAMECOL> ScrNameCol;
  public readonly List<ScenarioDbRecord.TC_SCNAMEROW> ScrNameRow;
  public readonly Dictionary<int, List<ScenarioDbRecord.TC_SCRIPTS_XREF>> ScrScriptXRef;
  public readonly Dictionary<int, List<ScenarioDbRecord.TC_SCRIPTRAS>> ScrScenRas;
  public readonly Dictionary<int, List<ScenarioDbRecord.TC_SCRIPTRAS>> ScrCellRas;
  public readonly Dictionary<int, ScenarioDbRecord.TC_ZSCEN> zscen;

  public ScenarioDbRecordsHolder(
    Dictionary<int, ScenarioDbRecord.TC_ZSCEN> zScen,
    Dictionary<int, List<ScenarioDbRecord.TC_SCRIPTRAS>> scScenRas,
    Dictionary<int, List<ScenarioDbRecord.TC_SCRIPTRAS>> scCellRas,
    Dictionary<int, List<ScenarioDbRecord.TC_SCRIPTS_XREF>> scriptsRef,
    List<ScenarioDbRecord.TC_SCRIPTS> scripts,
    List<ScenarioDbRecord.TC_SCCELLS> scCells,
    List<ScenarioDbRecord.TC_SCNAMECOL> scNameCol,
    List<ScenarioDbRecord.TC_SCNAMEROW> scNameRow)
  {
    this.zscen = zScen;
    this.ScrScriptXRef = scriptsRef;
    this.Scripts = scripts;
    this.ScrCells = scCells;
    this.ScrNameCol = scNameCol;
    this.ScrNameRow = scNameRow;
    this.ScrScenRas = scScenRas;
    this.ScrCellRas = scCellRas;
  }

  public ScenarioDbRecord.TC_SCNAMECOL FindCol(int scen, int ncol)
  {
    return this.ScrNameCol == null ? (ScenarioDbRecord.TC_SCNAMECOL) null : this.ScrNameCol.FirstOrDefault<ScenarioDbRecord.TC_SCNAMECOL>((Func<ScenarioDbRecord.TC_SCNAMECOL, bool>) (namecol => namecol.Scen.Equals(scen) && namecol.NCol.Equals(ncol)));
  }

  public ScenarioDbRecord.TC_SCNAMEROW FindRow(int scen, int nrow)
  {
    return this.ScrNameRow == null ? (ScenarioDbRecord.TC_SCNAMEROW) null : this.ScrNameRow.FirstOrDefault<ScenarioDbRecord.TC_SCNAMEROW>((Func<ScenarioDbRecord.TC_SCNAMEROW, bool>) (namerow => namerow.Scen.Equals(scen) && namerow.NRow.Equals(nrow)));
  }

  public ScenarioDbRecord.TC_SCCELLS FindCell(int scen, int ncol, int rowKey)
  {
    return this.ScrCells == null ? (ScenarioDbRecord.TC_SCCELLS) null : this.ScrCells.FirstOrDefault<ScenarioDbRecord.TC_SCCELLS>((Func<ScenarioDbRecord.TC_SCCELLS, bool>) (cell => cell.Scen.Equals(scen) && cell.NCol.Equals(ncol) && cell.NRow.Equals(rowKey)));
  }
}
