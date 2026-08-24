// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Data.TechProcPump.Vtd.VtdPump
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.TechCard.Common;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump;
using Intermech.ImpExp.TechCard.Pumpers.Data.TechProcRoutePump;
using Intermech.Interfaces;
using System;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Data.TechProcPump.Vtd;

[TaskDescription("Инициализация данных для перекачки - Комплекты ведомостей", "Перекачка данных - Комплекты ведомостей")]
internal class VtdPump : PumpClass
{
  public VtdPump(PluginClass plugin)
    : base(plugin)
  {
    this.taskExam.Repumpble = true;
    this.taskPump.Repumpble = true;
  }

  protected override Guid GUID => new Guid("{F08AD7D3-3E30-42B6-8EE7-88B98D3D28A3}");

  public override void Pump()
  {
    string str1 = TechDataBuilder<PumpClass>.GetPumpModeCond("F_OBJ_KEY", -2);
    if (!string.IsNullOrEmpty(str1))
      str1 = " and " + str1;
    string str2 = $"select tpv.F_KEY from TP_VERSIONS tpv, TC_ARCDOCS ad, TC_OBJ2LINK objlnk where {$"ad.{"F_KIND"} = {Convert.ToInt32((object) TechDbConsts.TechcardTables.TC_ARC_DOCS.DocKind.Vedomost)} and "}ad.F_KEY = tpv.F_TCKEY and objlnk.F_KEY = tpv.F_KEY and {$"objlnk.{"F_OBJ_TYPE"} = {Convert.ToInt32((object) LinkedObjectType.TechProc)} "}{str1}";
    IDbCommand command = TechcardConsts.ConnectionManager.CreateCommand();
    command.CommandText = str2;
    ICache service = ApplicationServices.Container.GetService<ICache>();
    IImportingData cache = service.GetCache(ImportingCategory.VtdCache);
    try
    {
      using (IDataReader dataReader = command.ExecuteReader(TechcardConsts.ConnectionManager.CommandBehavior))
      {
        int ordinal = dataReader.GetOrdinal("F_KEY");
        while (dataReader.Read())
        {
          int int32 = dataReader.GetInt32(ordinal);
          if (cache.GetValue((object) int32) == null)
            cache.AddValue((object) int32, 0L);
        }
      }
    }
    finally
    {
      service.ReleaseCache(ImportingCategory.VtdCache);
    }
    base.Pump();
  }
}
