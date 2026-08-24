// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.TechRoutePump.TechRoute_Links.Routes2Zag.TechRoute2ZagPump
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.ObjectRecords;
using Intermech.ImpExp.TechCard.Pumpers.Data.TechRoutePump.TechRoute_Links.Routes2Zag;
using Intermech.ImpExp.TechCard.TechProcPump.Common;
using Intermech.Interfaces.TechCard;
using System;
using System.Data;
using System.Windows.Forms;

#nullable disable
namespace Intermech.ImpExp.TechCard.TechRoutePump.TechRoute_Links.Routes2Zag;

[TaskDescription("Инициализация данных для перекачки - Связь расцеховки с заготовкой", "Перекачка данных - Связь расцеховки с заготовкой")]
internal class TechRoute2ZagPump(PluginClass plugin) : TechLinkBasePump(plugin)
{
  private readonly Guid _guid = new Guid("{C953FB63-CFB0-42cf-9F51-8A45E4DFDA9A}");

  protected override Guid GUID => this._guid;

  protected override void InitData()
  {
    this._relTypeGuid = TechCardConsts.RelTypes.TechRouteRelationGuid;
    this._categoryA = ImportingCategory.TechRoute;
    this._categoryB = ImportingCategory.TechZagot;
    this._fieldAName = "F_ROUTE_ID";
    this._fieldBName = "F_ZAG_ID";
    this._tableName = "TC_NROUTE_TO_ZAG";
    this._recType = "Связь расцеховки с заготовкой";
  }

  protected override ImportingCategory GetTechCategory()
  {
    return ImportingCategory.LinksTechRoute2ZagPump;
  }

  protected override ImportingCategory GetTechUniqueCategory()
  {
    return ImportingCategory.LinksTechRoute2ZagUniqueLinks;
  }

  protected override string GetUniqueRecordHash(TechObjectRecordBase record)
  {
    int int32_1 = Convert.ToInt32(record.Fields[this.GetFieldNameA(record)]);
    int int32_2 = Convert.ToInt32(record.Fields[this.GetFieldNameB(record)]);
    return $"{(object) this.GetNewKeyA(record, int32_1)}_{(object) this.plugin.Imdi.ImportedObjects.GetID(this.GetNewKeyB(record, int32_2))}";
  }

  protected override TechObjectRecord GetTpObjRec() => (TechObjectRecord) new TechRoute2ZagObject();

  protected override TechDataSource GetDataSource()
  {
    if (this._dataSource != null)
      return this._dataSource;
    TechDataBuilderSimple<TechPumpBase> dataBuilder = new TechDataBuilderSimple<TechPumpBase>((TechPumpBase) this);
    dataBuilder.PumpModeCondFunc = (Func<string, string, string>) ((condField, dopType) =>
    {
      string dataSource = string.Empty;
      string pumpModeCond1 = TechDataBuilder<PumpClass>.GetPumpModeCond("F_ROUTE_ID", 2);
      if (pumpModeCond1 != string.Empty)
        dataSource = pumpModeCond1;
      string pumpModeCond2 = TechDataBuilder<PumpClass>.GetPumpModeCond("F_ZAG_ID", 3);
      if (pumpModeCond2 != string.Empty)
        dataSource = dataSource != string.Empty ? $"{dataSource} AND {pumpModeCond2}" : pumpModeCond2;
      return dataSource;
    });
    this._dataSource = new TechDataSource((ITechDataBuilder) dataBuilder);
    return this._dataSource;
  }

  public override void Exam()
  {
    bool flag;
    using (IDataReader customDataReader = this.GetCustomDataReader(string.Format(" SELECT \r\n                                  {0}, {1}\r\n                                FROM \r\n                                  {2} \r\n                                GROUP BY\r\n                                  {0}, {1}\r\n                                HAVING \r\n                                  COUNT(*) > 1 ", (object) "F_ROUTE_ID", (object) "F_ZAG_ID", (object) "TC_NROUTE_TO_ZAG", (object) "F_KEY", (object) "F_DOCID", (object) "F_VERSION")))
      flag = customDataReader.Read();
    if (flag && MessageBox.Show($"В базе Imbase обнаружены не уникальные привязки заготовок к расцеховкам!{Environment.NewLine}Рекомендуется перед импортом запустить в TechCFG команду 'Администрирование'-'Корректировка базы'. Прервать импорт ?", "Внимание", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
      Application.Exit();
    using (IDataReader customDataReader = this.GetCustomDataReader(string.Format(" SELECT F_KEY FROM TC_NROUTE_TO_ZAG LINKS     \r\n                        WHERE    \r\n                            NOT EXISTS(\r\n                            SELECT B.F_ART_TCKEY FROM \r\n                              TC_OBJ2LINK B,\r\n                              TC_OBJ2LINK A \r\n                            WHERE \r\n                                B.F_OBJ_KEY = LINKS.F_ROUTE_ID\r\n                                AND A.F_OBJ_KEY   = LINKS.F_ZAG_ID \r\n                                AND A.F_OBJ_TYPE  = 3 \r\n                                AND B.F_ART_TCKEY = A.F_ART_TCKEY\r\n                                AND B.F_OBJ_TYPE  = 2\r\n                                      )             \r\n                      ")))
      flag = customDataReader.Read();
    if (flag && MessageBox.Show($"В базе Imbase обнаружены привязки заготовок к расцеховкам, без привязок с изделиям !{Environment.NewLine}Рекомендуется перед импортом запустить в TechCFG команду 'Администрирование'-'Корректировка базы'. Прервать импорт ?", "Внимание", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
      Application.Exit();
    base.Exam();
  }

  public override void Pump() => base.Pump();

  public override bool CheckObjTypeOrParamType(string entCode, Guid attrGuid) => false;

  public override bool CheckObjLinkOrParamType(string entCode, Guid attrGuid) => false;

  public override void FillRecordParamsFixed(TechObjectRecord record, TechParamList paramList)
  {
  }

  protected override void CheckBaseRecords()
  {
  }
}
