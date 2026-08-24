// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.ProductionsPump.ProductionsPump
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.DataWriter;
using Intermech.ImpExp.TechCard.Common;
using Intermech.ImpExp.TechCard.Common.LoadCache;
using Intermech.ImpExp.TechCard.Pumpers;
using Intermech.Interfaces;
using Intermech.Kernel.Search;
using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.TechCard.ProductionsPump;

[TaskDescription("Инициализация данных для перекачки - Виды производства", "Перекачка данных - Виды производства")]
[TaskType(PumperType.MetaData)]
internal class ProductionsPump(PluginClass plugin) : PumpClass(plugin)
{
  private Guid _guid = new Guid("{0A9DA8C4-992F-41cb-AD04-2D6D5BFAFF32}");

  protected override Guid GUID => this._guid;

  public override void Exam()
  {
    this.ExamCheckPoint("Инициализация считывания видов производства", 0);
    int tableRecordsCount = this.GetTableRecordsCount("TC_PRODUCTIONS");
    IDataReader defaultDataReader = this.GetDefaultDataReader("TC_PRODUCTIONS");
    try
    {
      TechPumpData.Production.Productions.Clear();
      Intermech.ImpExp.TechCard.Production.ParseSchema(this.GetTableColumns(defaultDataReader));
      int index = 0;
      while (defaultDataReader.Read())
      {
        ProductInfo prodInfo = Intermech.ImpExp.TechCard.Production.Parse(defaultDataReader);
        if (prodInfo != null)
          TechPumpData.Production.Productions.Add(prodInfo.ProductionID, new IpsProductionObj(0L, prodInfo));
        ++index;
        if (index % 100 == 0)
          this.ExamCheckPoint($"Получение видов производства ({index} из {tableRecordsCount})", this.CalculatePercent(tableRecordsCount, index, 1, 99));
      }
      this.ExamCheckPoint("Получение видов производства завершено", 100);
      TechCache.WriteOneList(TechCache.CategoryList.ProductionsList, (object) TechPumpData.Production.Productions);
    }
    finally
    {
      defaultDataReader.Close();
    }
  }

  public override void Pump()
  {
    this.PumpCheckPoint("Инициализация закачки видов производства", 0);
    if (TechPumpData.Production.Productions == null || TechPumpData.Production.Productions.Count == 0)
      return;
    IImportedObjectList impList = this.plugin.Idw.CreateImportedObjectListWithStatistics(this.GUID);
    Dictionary<ObjectRecord, IpsProductionObj> obj2ProdObjList = new Dictionary<ObjectRecord, IpsProductionObj>();
    impList.AfterImportEvent += (AfterImportEventDelegate) ((_param1, _param2) =>
    {
      for (int index = 0; index < impList.Items.Count; ++index)
      {
        ObjectRecord key = impList.Items[index].Object;
        IpsProductionObj ipsProductionObj;
        if (obj2ProdObjList.TryGetValue(key, out ipsProductionObj) && key.Object_id != 0L && key.Object_id != -1L)
        {
          ipsProductionObj.ObjID = key.Object_id;
          obj2ProdObjList.Remove(key);
        }
      }
    });
    int id1 = this.plugin.Imdi.ObjectTypes.GetByGuid(TechcardConsts.TypeConsts.otProductionObjTypeGuid).ID;
    int id2 = this.plugin.Imdi.AttributeTypes.GetByGuid(TechcardConsts.TypeConsts.atNaimAttrTypeGuid).ID;
    int id3 = this.plugin.Imdi.AttributeTypes.GetByGuid(TechcardConsts.TypeConsts.atNaimAttrTypeGuid).ID;
    int id4 = this.plugin.Imdi.AttributeTypes.GetByGuid(TechcardConsts.TypeConsts.atShortNaimAttrTypeGuid).ID;
    this.PumpCheckPoint("Подготовка к закачке видов производства", 1);
    IDBObjectCollection objectCollection = this.plugin.Idw.GetUserSession().GetObjectCollection(id1);
    DBRecordSetParams paramSet = new DBRecordSetParams((ConditionStructure[]) null, new object[2]
    {
      (object) ObligatoryObjectAttributes.F_OBJECT_ID,
      (object) id4
    });
    string name = this.plugin.Imdi.AttributeTypes.GetByID(id4).Name;
    DataTable dataTable = objectCollection.Select(paramSet);
    foreach (IpsProductionObj ipsProductionObj in TechPumpData.Production.Productions.Values)
    {
      if (ipsProductionObj.ProdInfo != null)
      {
        DataRow[] dataRowArray = dataTable.Select($"[{name}] = '{ipsProductionObj.ProdInfo.Loc_Litera}'");
        if (dataRowArray.Length.Equals(0))
        {
          ObjectRecord key = impList.AddObject(id1, 0, ipsProductionObj.ProdInfo.Name);
          impList.AddAttributeStr(id2, ipsProductionObj.ProdInfo.Name);
          impList.AddAttributeStr(id4, ipsProductionObj.ProdInfo.Loc_Litera);
          impList.AddAttributeStr(id3, ipsProductionObj.ProdInfo.Name);
          AttributesHelper.AddObligatoryObjectAttributes(this.plugin.Imdi.UserSession, impList);
          obj2ProdObjList.Add(key, ipsProductionObj);
        }
        else
        {
          long int64 = Convert.ToInt64(dataRowArray[0][0]);
          ipsProductionObj.ObjID = int64;
        }
      }
    }
    this.PumpCheckPoint("Закачка видов производства", 50);
    impList.Import();
    this.PumpCheckPoint("Закачка видов производства завершена", 100);
    TechCache.WriteOneList(TechCache.CategoryList.ProductionsList, (object) TechPumpData.Production.Productions);
  }
}
