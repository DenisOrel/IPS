// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Data.TechRoutePump.TechRoute_Links.Routes2Tp.TechRoute2TpPump
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.TechCard.Common;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.ObjectRecords;
using Intermech.Interfaces.TechCard;
using System;
using System.Data;
using System.Windows.Forms;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Data.TechRoutePump.TechRoute_Links.Routes2Tp;

[TaskDescription("Инициализация данных для перекачки - Связь расцеховки с техпроцессом", "Перекачка данных - Связь расцеховки с техпроцессом")]
internal class TechRoute2TpPump(PluginClass plugin) : TechLinkBasePump(plugin)
{
  private readonly Guid _guid = new Guid("{94AB4B18-1FCB-4949-91F5-17697FAE1804}");

  protected override Guid GUID => this._guid;

  protected override void InitData()
  {
    this._relTypeGuid = TechCardConsts.RelTypes.TechRouteRelationGuid;
    this._categoryA = ImportingCategory.TechRoute;
    this._categoryB = ImportingCategory.TechProcessPump;
    this._fieldAName = "F_SOURCE_KEY";
    this._fieldBName = "F_TARGET_KEY";
    this._tableName = "TC_TECH_LINKS";
    this._recType = "Связь расцеховки с техпроцессом";
  }

  protected override ImportingCategory GetTechCategory() => ImportingCategory.LinksTechRoute2TpPump;

  protected override ImportingCategory GetTechUniqueCategory()
  {
    return ImportingCategory.LinksTechRoute2TpUniqueLinks;
  }

  protected override string GetUniqueRecordHash(TechObjectRecordBase record)
  {
    int int32_1 = Convert.ToInt32(record.Fields[this.GetFieldNameA(record)]);
    int int32_2 = Convert.ToInt32(record.Fields[this.GetFieldNameB(record)]);
    return $"{(object) this.GetNewKeyA(record, int32_1)}_{(object) this.GetNewKeyB(record, int32_2)}";
  }

  protected override TechObjectRecord GetTpObjRec() => (TechObjectRecord) new TechRoute2TpObject();

  protected override TechDataSource GetDataSource()
  {
    if (this._dataSource != null)
      return this._dataSource;
    TechDataBuilderSimple<TechPumpBase> dataBuilder = new TechDataBuilderSimple<TechPumpBase>((TechPumpBase) this);
    dataBuilder.PumpModeCondFunc = (Func<string, string, string>) ((condField, dopType) =>
    {
      string dataSource = string.Empty;
      string pumpModeCond1 = TechDataBuilder<PumpClass>.GetPumpModeCond(this._fieldAName, 2);
      if (pumpModeCond1 != string.Empty)
        dataSource = pumpModeCond1;
      string pumpModeCond2 = TechDataBuilder<PumpClass>.GetPumpModeCond(this._fieldBName, -2);
      if (pumpModeCond2 != string.Empty)
        dataSource = dataSource != string.Empty ? $"{dataSource} AND {pumpModeCond2}" : pumpModeCond2;
      return dataSource;
    });
    this._dataSource = new TechDataSource((ITechDataBuilder) dataBuilder);
    return this._dataSource;
  }

  protected override void CheckBaseRecords()
  {
  }

  public override void Exam()
  {
    bool flag;
    using (IDataReader customDataReader = this.GetCustomDataReader(string.Format(" SELECT \r\n                                  {0}, {1}, {2}\r\n                                FROM \r\n                                  {3} \r\n                                WHERE\r\n                                  {2} = {4} \r\n                                GROUP BY\r\n                                  {0}, {1}, {2}\r\n                                HAVING \r\n                                  COUNT(*) > 1 ", (object) "F_SOURCE_KEY", (object) "F_TARGET_KEY", (object) "F_LINK_TYPE", (object) "TC_TECH_LINKS", (object) Convert.ToInt32((object) TechDbConsts.TechcardTables.TC_TECH_LINKS.TTechLinkType.tltRouteToTP))))
      flag = customDataReader.Read();
    if (flag && MessageBox.Show($"В базе Imbase обнаружены не уникальные привязки РМ к ТП!{Environment.NewLine}Рекомендуется перед импортом запустить в TechCFG команду 'Администрирование'-'Корректировка базы'. Прервать импорт?", "Внимание", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
      Application.Exit();
    base.Exam();
  }

  public override void Pump() => base.Pump();
}
