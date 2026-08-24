// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Workflow.PumpSchemeCategories
// Assembly: Intermech.ImpExp.Workflow, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 3E5C231D-9C58-4E51-9000-3F9F7E271790
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Workflow.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.DataWriter;
using Intermech.Workflow;
using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.Workflow;

[TaskDescription("Инициализация данных для перекачки групп шаблонов процессов", "Перекачка групп шаблонов процессов")]
[TaskType(PumperType.MetaData)]
public class PumpSchemeCategories : PumpClass
{
  protected WorkflowPlugin plugin;
  private CacheCategory _schemeCategoriesCache;
  private Dictionary<long, int> _parents = new Dictionary<long, int>();

  protected override Guid GUID => new Guid("{FC26574B-B328-4e81-8C50-660BE506E7C0}");

  public PumpSchemeCategories(WorkflowPlugin plugin)
    : base((PluginClass) plugin)
  {
    this.plugin = plugin;
  }

  protected CacheCategory SchemeCategoriesCache
  {
    get
    {
      if (this._schemeCategoriesCache == null)
        this._schemeCategoriesCache = PumpCache.Category[ImportingCategory.SchemeCategories];
      return this._schemeCategoriesCache;
    }
  }

  public override void Exam() => this.ExamCheckPoint("Проверка данных успешно завершена", 100);

  private void doPump()
  {
    try
    {
      this.PumpCheckPoint("Перекачка групп шаблонов процессов", 0);
      using (IDbCommand command = this.plugin.idb2.DbConnection.CreateCommand())
      {
        this.PumpCheckPoint("Определение количества групп шаблонов процессов для перекачки", 0);
        command.CommandText = "select count(*) from schemecategoriestable";
        int int32_1 = Convert.ToInt32(command.ExecuteScalar());
        command.CommandText = "select id, categoryname, incategory from schemecategoriestable order by id";
        IDataReader dataReader = command.ExecuteReader(CommandBehavior.SequentialAccess);
        try
        {
          int index = 1;
          string format = "Перекачка групп шаблонов процессов ({0} из {1})";
          IDataWriter idw = this.plugin.Idw;
          this._parents.Clear();
          while (dataReader.Read())
          {
            this.PumpCheckPoint(string.Format(format, (object) index, (object) int32_1), this.CalculatePercent(int32_1, index, 1, 99));
            try
            {
              int int32_2 = BasePumpHelper.ToInt32(dataReader[0]);
              string caption = dataReader.GetString(1);
              int int32_3 = BasePumpHelper.ToInt32(dataReader[2]);
              if (this.SchemeCategoriesCache.GetNewKey((object) int32_2) == 0L)
              {
                long num = idw.AddObject(wfConsts.SchemeCategoriesID, 0, caption);
                if (int32_3 != 0)
                  this._parents.Add(num, int32_3);
                this._schemeCategoriesCache.AddValue((object) int32_2, num);
              }
            }
            finally
            {
              ++index;
            }
          }
          foreach (KeyValuePair<long, int> parent in this._parents)
          {
            long newKey = this._schemeCategoriesCache.GetNewKey((object) parent.Value);
            if (newKey != 0L)
              idw.AddRelation(newKey, parent.Key, wfConsts.SimpleLinkTypeID);
          }
          this.PumpCheckPoint("Перекачка групп шаблонов процессов успешно завершена", 100);
        }
        finally
        {
          dataReader.Close();
          BlobHelper.Clear();
        }
      }
      this.PumpCheckPoint("Перекачка групп шаблонов процессов успешно завершена", 100);
    }
    catch (Exception ex)
    {
      BasePumpHelper.AppManager.AddErrorMessage(ex.Message);
    }
  }

  public override void Pump() => this.doPump();
}
