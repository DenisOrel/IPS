// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Common.DataManager.DataManager
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.TechCard.Common.TechCardSettings;
using Intermech.ImpExp.TechCard.Pumpers;
using Intermech.ImpExp.TechCard.Pumpers.Data.TechProcRoutePump;
using Intermech.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace Intermech.ImpExp.TechCard.Common.DataManager;

[TaskDescription("Подготовка к закачке данных TechCard", "Подготовка к закачке данных TechCard")]
internal class DataManager : PumpClass
{
  public const int CustomObjectType = 100;
  private readonly Guid _guid = new Guid("{AA796059-18D1-4f72-BD22-A4E3DF2CC1CC}");

  private void PrepareCheckData()
  {
    using (IDbCommand command = TechcardConsts.ConnectionManager.CreateCommand())
    {
      command.CommandText = "SELECT COUNT(*) FROM TP_PUMP_DATA";
      try
      {
        command.ExecuteReader();
      }
      catch (Exception ex)
      {
        string str = $"Ошибка при проверке таблицы {"TP_PUMP_DATA"}: {ex.Message}";
        this.plugin.appManager.AddWarningMessage(str);
        int num = (int) MessageBox.Show(str, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
      }
    }
  }

  private void PreparePumpData_Clear()
  {
    this.PumpCheckPoint("Удаление информации из вспомогательных таблиц", 0);
    using (IDbCommand command = TechcardConsts.ConnectionManager.CreateCommand())
    {
      command.CommandText = "DELETE FROM TP_PUMP_DATA";
      command.ExecuteNonQuery();
    }
    this.PumpCheckPoint("Удаление информации из вспомогательных таблиц успешно завершено", 30);
  }

  private void PreparePumpData_AppendThroughDocs()
  {
    int num = -2;
    using (IDbCommand command = TechcardConsts.ConnectionManager.CreateCommand())
    {
      string str1 = $"INSERT INTO TP_PUMP_DATA ( F_OBJ_TYPE, F_OBJ_KEY)   SELECT DISTINCT {(object) num}, A.F_TPKEY   FROM TP_OPER A    WHERE    A.F_TPKEY <> 0    AND A.F_DOCTCKEY IN    (SELECT PD.F_OBJ_KEY     FROM TP_PUMP_DATA PD WHERE PD.F_OBJ_TYPE = {(object) num})";
      command.CommandText = str1;
      command.ExecuteNonQuery();
      string str2 = string.Empty;
      switch (TechcardConsts.ConnectionManager.DataBaseType)
      {
        case "IntermechConnection.Oracle":
          str2 = "\r\n                                    INSERT INTO TP_PUMP_DATA \r\n                                      (F_OBJ_TYPE, F_OBJ_KEY)\r\n                                    SELECT       \r\n                                       -2,  A.F_TPKEY \r\n                                    FROM       \r\n                                        TP_OPER A\r\n                                    WHERE \r\n                                       A.F_TPKEY <> 0  \r\n                                       CONNECT BY PRIOR A.F_TPKEY = A.F_DOCTCKEY \r\n                                       \r\n                                       START WITH A.F_DOCTCKEY IN\r\n                                       (SELECT PD.F_OBJ_KEY FROM TP_PUMP_DATA PD WHERE PD.F_OBJ_TYPE = -2)\r\n                                   ";
          break;
        case "IntermechConnection.MsSQL":
          str2 = "\r\n                                    WITH  cte_links AS (\r\n                                        SELECT       \r\n                                    A.F_TPKEY \r\n                                        FROM       \r\n                                    TP_OPER A\r\n                                    WHERE \r\n                                    A.F_TPKEY <> 0  \r\n                                    AND A.F_DOCTCKEY IN\r\n                                    (SELECT PD.F_OBJ_KEY FROM TP_PUMP_DATA PD WHERE PD.F_OBJ_TYPE = -2)\r\n                                    UNION ALL\r\n                                    SELECT \r\n                                    B.F_TPKEY\r\n                                        FROM \r\n                                    TP_OPER B\r\n                                    INNER JOIN cte_links cte \r\n                                    ON B.F_DOCTCKEY = cte.F_TPKEY\r\n                                    WHERE\r\n                                    B.F_TPKEY <> 0  \r\n                                        )\r\n                                    INSERT INTO TP_PUMP_DATA \r\n                                      (F_OBJ_TYPE, F_OBJ_KEY)\r\n\r\n                                    SELECT \r\n                                    DISTINCT -2, F_TPKEY\r\n                                    FROM \r\n                                        cte_links\r\n                                   ";
          break;
        case "IntermechConnection.Interbase":
          str2 = "\r\n                                    INSERT INTO TP_PUMP_DATA \r\n                                      (F_OBJ_TYPE, F_OBJ_KEY)\r\n\r\n                                    WITH recursive cte_links (F_TPKEY) AS (\r\n                                        SELECT       \r\n                                            A.F_TPKEY \r\n                                        FROM       \r\n                                            TP_OPER A\r\n                                        WHERE \r\n                                           A.F_TPKEY <> 0  \r\n                                           AND A.F_DOCTCKEY IN\r\n                                           (SELECT PD.F_OBJ_KEY FROM TP_PUMP_DATA PD WHERE PD.F_OBJ_TYPE = -2)\r\n                                        UNION ALL\r\n                                        SELECT \r\n                                            B.F_TPKEY\r\n                                        FROM \r\n                                            TP_OPER B, cte_links\r\n                                        WHERE\r\n                                          B.F_DOCTCKEY = cte_links.F_TPKEY\r\n                                          AND B.F_TPKEY <> 0  \r\n                                    )\r\n                                    SELECT\r\n                                     DISTINCT -2, F_TPKEY\r\n                                    FROM \r\n                                        cte_links\r\n                                   ";
          break;
      }
      if (string.IsNullOrEmpty(str2))
        return;
      command.CommandText = str2;
      command.ExecuteNonQuery();
    }
  }

  private void PreparePumpData_ArchiveMode()
  {
    this.PumpCheckPoint("Сохранение списка документов", 31 /*0x1F*/);
    this.PumpCheckPoint("Сохранение списка документов", 31 /*0x1F*/);
    int num1 = -1;
    DataSet dataSet = new DataSet();
    try
    {
      string sqlText = " SELECT * FROM TP_PUMP_DATA WHERE F_OBJ_TYPE = " + (object) num1;
      IDbDataAdapter dataAdapter = TechcardConsts.Plugin.idb.GetDataAdapter(sqlText);
      dataAdapter.Fill(dataSet);
      DataTable table = dataSet.Tables.Count > 0 ? dataSet.Tables[0] : (DataTable) null;
      if (table != null)
      {
        int columnIndex1 = table.Columns.IndexOf("F_OBJ_TYPE");
        int columnIndex2 = table.Columns.IndexOf("F_OBJ_KEY");
        foreach (int num2 in TechSettingsHelper.PumpArchiveDocIDS)
        {
          DataRow row = table.NewRow();
          row[columnIndex1] = (object) num1;
          row[columnIndex2] = (object) num2;
          table.Rows.Add(row);
        }
      }
      dataAdapter.Update(dataSet);
    }
    catch (Exception ex)
    {
      TechcardConsts.Plugin.appManager.AddErrorMessage($"Ошибка сохранения списка документов: {ex.Message}");
      if (!(ex is OutOfMemoryException))
        return;
      throw;
    }
    using (IDbCommand command = TechcardConsts.ConnectionManager.CreateCommand())
    {
      string str = $"INSERT INTO TP_PUMP_DATA ( F_OBJ_TYPE, F_OBJ_KEY)   SELECT DISTINCT {(object) -2}, A.F_KEY   FROM TP_VERSIONS A   LEFT JOIN    TC_ARCDOCS B    ON    A.F_TCKEY = B.F_KEY    WHERE    B.F_DOCID IN    (SELECT PD.F_OBJ_KEY     FROM TP_PUMP_DATA PD WHERE PD.F_OBJ_TYPE = {(object) -1})";
      command.CommandText = str;
      command.ExecuteNonQuery();
    }
    this.PumpCheckPoint("Сохранение списка документов завершено", 50);
    this.PumpCheckPoint("Сохранение списка связей со сквозными ТП", 65);
    this.PreparePumpData_AppendThroughDocs();
    this.PumpCheckPoint("Сохранение списка изделий", 66);
    this.PreparePumpData_FillArticles();
    this.PumpCheckPoint("Подготовка информации в вспомогательных таблицах успешно завершена", 100);
  }

  private void PreparePumpData_TPMode()
  {
    this.PumpCheckPoint("Cохранение списка документов", 31 /*0x1F*/);
    int num = -2;
    DataSet dataSet = new DataSet();
    try
    {
      string sqlText = " SELECT * FROM TP_PUMP_DATA WHERE F_OBJ_TYPE = " + (object) num;
      IDbDataAdapter dataAdapter = TechcardConsts.Plugin.idb.GetDataAdapter(sqlText);
      dataAdapter.Fill(dataSet);
      DataTable table = dataSet.Tables.Count > 0 ? dataSet.Tables[0] : (DataTable) null;
      if (table != null)
      {
        int columnIndex1 = table.Columns.IndexOf("F_OBJ_TYPE");
        int columnIndex2 = table.Columns.IndexOf("F_OBJ_KEY");
        foreach (int pumpDoc in TechSettingsHelper.PumpDocList)
        {
          DataRow row = table.NewRow();
          row[columnIndex1] = (object) num;
          row[columnIndex2] = (object) pumpDoc;
          table.Rows.Add(row);
        }
      }
      dataAdapter.Update(dataSet);
    }
    catch (Exception ex)
    {
      TechcardConsts.Plugin.appManager.AddErrorMessage($"Ошибка сохранения списка документов: {ex.Message}");
      if (!(ex is OutOfMemoryException))
        return;
      throw;
    }
    this.PumpCheckPoint("Сохранение списка документов завершено", 50);
    this.PumpCheckPoint("Сохранение списка связей со сквозными ТП", 65);
    this.PreparePumpData_AppendThroughDocs();
    this.PumpCheckPoint("Сохранение списка изделий", 66);
    this.PreparePumpData_FillArticles();
    this.PumpCheckPoint("Подготовка информации в вспомогательных таблицах успешно завершена", 100);
  }

  private void PreparePumpData_ArtsFromSelectedObjsStructure(ObjStructExpander structExpander)
  {
    bool flag = structExpander is ProdZakStructExpander;
    string str = flag ? "производственных заказов" : "изделий";
    this.PumpCheckPoint("Получение списка зарегистрированных в Techcard изделий из состава выбранных " + str, 31 /*0x1F*/);
    List<ArtInfoLight> headObjsInfo = structExpander is ProdZakStructExpander ? TechSettingsHelper.PumpProdZakList : TechSettingsHelper.PumpArtList;
    List<ArtInfoLight> source = structExpander.CollectObjsFromObjsStructures(headObjsInfo);
    this.PumpCheckPoint("Получение списка всех зарегистрированных в TechCard изделий", 50);
    string sqlText1 = "select F_KEY,F_ID,F_VER from TC_ARCARTS ";
    bool workWithArtVersinTC = TechPumpData.Configs.WorkWithArtVers;
    if (workWithArtVersinTC)
      sqlText1 += ", F_VER";
    List<ArtInfoLight> regArts = new List<ArtInfoLight>();
    using (IDataReader dataReader = this.GetDataReader(sqlText1))
    {
      Dictionary<string, int> tableColumns = this.GetTableColumns(dataReader);
      int i1 = tableColumns["F_KEY"];
      int i2 = tableColumns["F_ID"];
      int i3 = tableColumns["F_VER"];
      while (dataReader.Read())
      {
        int int32Value = DataSetProcessor.GetInt32Value(dataReader[i2], 0);
        if (int32Value > 0)
        {
          ArtInfoLight artInfoLight = new ArtInfoLight(int32Value, DataSetProcessor.GetInt32Value(dataReader[i3], 0), -1, DataSetProcessor.GetInt32Value(dataReader[i1], 0));
          regArts.Add(artInfoLight);
        }
      }
    }
    this.PumpCheckPoint("Получение списка зарегистрированных в TechCard изделий из состава выбранных " + str, 60);
    Comparer<ArtInfoLight> comparer = Comparer<ArtInfoLight>.Create((Comparison<ArtInfoLight>) ((left, right) =>
    {
      int num = left.ArtId - right.ArtId;
      if (workWithArtVersinTC && num == 0)
        num = left.ArtVer - right.ArtVer;
      return num;
    }));
    regArts.Sort((IComparer<ArtInfoLight>) comparer);
    IEnumerable<ArtInfoLight> artInfoLights1 = source.Select(candidateArt => new
    {
      candidateArt = candidateArt,
      foundIdx = regArts.BinarySearch(candidateArt, (IComparer<ArtInfoLight>) comparer)
    }).Where(_param1 => _param1.foundIdx >= 0).Select(_param1 => regArts[_param1.foundIdx]);
    IEnumerable<ArtInfoLight> artInfoLights2 = flag ? TechSettingsHelper.PumpProdZakList.Select(candidateArt => new
    {
      candidateArt = candidateArt,
      foundIdx = regArts.BinarySearch(candidateArt, (IComparer<ArtInfoLight>) comparer)
    }).Where(_param1 => _param1.foundIdx >= 0).Select(_param1 => regArts[_param1.foundIdx]) : (IEnumerable<ArtInfoLight>) null;
    this.PumpCheckPoint("Сохранение списка зарегистрированных в TechCard изделий из состава выбранных " + str, 75);
    DataSet dataSet = new DataSet();
    try
    {
      int int32_1 = Convert.ToInt32((object) Intermech.ImpExp.TechCard.Common.DataManager.DataManager.ObjDataType.odtArtKey);
      int int32_2 = Convert.ToInt32((object) Intermech.ImpExp.TechCard.Common.DataManager.DataManager.ObjDataType.odtProdZakKey);
      string sqlText2 = "select * from TP_PUMP_DATA " + $"where {"F_OBJ_TYPE"} in ({int32_1}, {int32_2})";
      IDbDataAdapter dataAdapter = TechcardConsts.Plugin.idb.GetDataAdapter(sqlText2);
      dataAdapter.Fill(dataSet);
      DataTable table = dataSet.Tables.Count > 0 ? dataSet.Tables[0] : (DataTable) null;
      if (table != null)
      {
        int columnIndex1 = table.Columns.IndexOf("F_OBJ_TYPE");
        int columnIndex2 = table.Columns.IndexOf("F_OBJ_KEY");
        foreach (ArtInfoLight artInfoLight in artInfoLights1)
        {
          DataRow row = table.NewRow();
          row[columnIndex1] = (object) int32_1;
          row[columnIndex2] = (object) artInfoLight.ArtTCKey;
          table.Rows.Add(row);
        }
        if (artInfoLights2 != null)
        {
          foreach (ArtInfoLight artInfoLight in artInfoLights2)
          {
            DataRow row = table.NewRow();
            row[columnIndex1] = (object) int32_2;
            row[columnIndex2] = (object) artInfoLight.ArtTCKey;
            table.Rows.Add(row);
          }
        }
      }
      dataAdapter.Update(dataSet);
    }
    catch (Exception ex)
    {
      TechcardConsts.Plugin.appManager.AddErrorMessage($"Ошибка сохранения списка изделий из составов выбранных {str}: {ex.Message}");
      if (!(ex is OutOfMemoryException))
        return;
      throw;
    }
    this.PumpCheckPoint("Заполнение списка документов ТП по составам выбранных " + str, 90);
    this.PreparePumpData_FillTPsFromArts();
    this.PumpCheckPoint("Подготовка информации в вспомогательных таблицах успешно завершена", 100);
  }

  private void PreparePumpData_ArtList()
  {
    this.PreparePumpData_ArtsFromSelectedObjsStructure((ObjStructExpander) new ArtStructExpander());
  }

  private void PreparePumpData_ProdZakList()
  {
    this.PreparePumpData_ArtsFromSelectedObjsStructure((ObjStructExpander) new ProdZakStructExpander());
  }

  private void PreparePumpData_AllMode()
  {
    this.PumpCheckPoint("Подготовка информации в вспомогательных таблицах успешно завершена", 100);
  }

  private void PreparePumpData_FillArticles()
  {
    using (IDbCommand command = TechcardConsts.ConnectionManager.CreateCommand())
    {
      string str = $"INSERT INTO TP_PUMP_DATA ( F_OBJ_TYPE, F_OBJ_KEY)   SELECT DISTINCT {(object) -3}, A.F_ART_TCKEY   FROM TC_OBJ2LINK A    WHERE    F_OBJ_TYPE = {(object) 1} AND    F_OBJ_KEY IN    (SELECT PD.F_OBJ_KEY     FROM TP_PUMP_DATA PD  WHERE PD.F_OBJ_TYPE = {(object) -2})";
      command.CommandText = str;
      command.ExecuteNonQuery();
    }
  }

  private void PreparePumpData_FillTPsFromArts()
  {
    using (IDbCommand command = TechcardConsts.ConnectionManager.CreateCommand())
    {
      string str = $"insert into TP_PUMP_DATA (F_OBJ_TYPE, F_OBJ_KEY) {$"select distinct {Convert.ToInt32((object) Intermech.ImpExp.TechCard.Common.DataManager.DataManager.ObjDataType.odtDocKey)}, A.{"F_OBJ_KEY"} "}from TC_OBJ2LINK A where {$"{"F_OBJ_TYPE"} = {Convert.ToInt32((object) LinkedObjectType.TechProc)} and "}F_ART_TCKEY in (select PD.F_OBJ_KEY from TP_PUMP_DATA PD {$"where PD.{"F_OBJ_TYPE"} = {Convert.ToInt32((object) Intermech.ImpExp.TechCard.Common.DataManager.DataManager.ObjDataType.odtArtKey)}"})";
      command.CommandText = str;
      command.ExecuteNonQuery();
    }
  }

  private void PreparePumpData_FillZagot()
  {
    if (!TechSettingsHelper.PumpDataType.HasFlag((Enum) TechPumpDataType.Zagot))
      return;
    this.PreparePumpData_FillObj2Link(3);
  }

  private void PreparePumpData_FillRoutes()
  {
    if (!TechSettingsHelper.PumpDataType.HasFlag((Enum) TechPumpDataType.Route) || TechSettingsHelper.PumpMode == TechPumpMode.tpmAll)
      return;
    this.PreparePumpData_FillObj2Link(2);
    using (IDbCommand command = TechcardConsts.ConnectionManager.CreateCommand())
    {
      int num = 107;
      string str1 = $"INSERT INTO TP_PUMP_DATA ( F_OBJ_TYPE, F_OBJ_KEY)   SELECT DISTINCT {(object) num}, F_TMP_OBRABOTKI   FROM TC_NROUTES    WHERE    F_KEY IN    (SELECT PD.F_OBJ_KEY     FROM TP_PUMP_DATA PD WHERE PD.F_OBJ_TYPE = {(object) 2}) AND    F_TMP_OBRABOTKI <> 0";
      command.CommandText = str1;
      command.ExecuteNonQuery();
      string str2 = $"INSERT INTO TP_PUMP_DATA ( F_OBJ_TYPE, F_OBJ_KEY)   SELECT DISTINCT {(object) num}, F_TMP_SBORKI   FROM TC_NROUTES    WHERE    F_KEY IN    (SELECT PD.F_OBJ_KEY     FROM TP_PUMP_DATA PD WHERE PD.F_OBJ_TYPE = {(object) 2}) AND    F_TMP_SBORKI <> 0";
      command.CommandText = str2;
      command.ExecuteNonQuery();
      string str3 = $"INSERT INTO TP_PUMP_DATA ( F_OBJ_TYPE, F_OBJ_KEY)   SELECT DISTINCT {(object) 122}, F_KEY   FROM TC_NROUTE_STRINGS    WHERE    F_TEMPLATE_ID IN    (SELECT PD.F_OBJ_KEY     FROM TP_PUMP_DATA PD WHERE PD.F_OBJ_TYPE = {(object) num}) ";
      command.CommandText = str3;
      command.ExecuteNonQuery();
    }
  }

  private void PreparePumpData_FillMater()
  {
    if (!TechSettingsHelper.PumpDataType.HasFlag((Enum) TechPumpDataType.MatGroup))
      return;
    this.PreparePumpData_FillObj2Link(4);
  }

  private void PreparePumpData_FillObj2Link(int objectTypeId)
  {
    if (TechSettingsHelper.PumpMode == TechPumpMode.tpmAll)
      return;
    using (IDbCommand command = TechcardConsts.ConnectionManager.CreateCommand())
    {
      string str = $"INSERT INTO TP_PUMP_DATA ( F_OBJ_TYPE, F_OBJ_KEY)   SELECT DISTINCT F_OBJ_TYPE, F_OBJ_KEY   FROM TC_OBJ2LINK    WHERE    F_OBJ_TYPE = {(object) objectTypeId} AND    F_ART_TCKEY IN    (SELECT OL.F_OBJ_KEY     FROM TP_PUMP_DATA OL WHERE OL.F_OBJ_TYPE = {(object) -3})";
      command.CommandText = str;
      command.ExecuteNonQuery();
    }
  }

  public DataManager(PluginClass plugin)
    : base(plugin)
  {
    this.taskExam.Repumpble = true;
    this.taskPump.Repumpble = true;
    this.PrepareCheckData();
  }

  protected override Guid GUID => this._guid;

  public override void Exam() => this.ExamCheckPoint("Подготовка данных успешно завершена", 100);

  public override void Pump()
  {
    TechPumpMode pumpMode = TechSettingsHelper.PumpMode;
    string caption = EnumTypeHelper.GetCaption((Enum) pumpMode);
    TechcardConsts.Plugin.appManager.AddInfoMessage($"Начала подготовки данных в режиме : \"{caption}\"");
    this.PreparePumpData_Clear();
    switch (pumpMode)
    {
      case TechPumpMode.tpmArchive:
        this.PreparePumpData_ArchiveMode();
        break;
      case TechPumpMode.tpmTpList:
        this.PreparePumpData_TPMode();
        break;
      case TechPumpMode.tpmArtList:
        this.PreparePumpData_ArtList();
        break;
      case TechPumpMode.tpmProdZakList:
        this.PreparePumpData_ProdZakList();
        break;
      default:
        this.PreparePumpData_AllMode();
        break;
    }
    this.PreparePumpData_FillZagot();
    this.PreparePumpData_FillMater();
    this.PreparePumpData_FillRoutes();
  }

  public enum ObjDataType
  {
    odtProdZakKey = -4, // 0xFFFFFFFC
    odtArtKey = -3, // 0xFFFFFFFD
    odtDocKey = -2, // 0xFFFFFFFE
    odtDocID = -1, // 0xFFFFFFFF
  }
}
