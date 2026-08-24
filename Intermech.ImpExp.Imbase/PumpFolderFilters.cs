// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Imbase.PumpFolderFilters
// Assembly: Intermech.ImpExp.Imbase, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 14B82A62-153A-4D0C-8A5E-F24874681A1E
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Imbase.dll

using Intermech.ImpExp.Interface;
using System;

#nullable disable
namespace Intermech.ImpExp.Imbase;

[TaskDescription("", "Перекачка данных фильтрации папок Imbase для Techcard")]
[TaskType(PumperType.MetaData)]
internal sealed class PumpFolderFilters(ImbasePlugin plugin) : PumpImbaseClass(plugin)
{
  public static Guid _guid = new Guid("{B8D4E3E8-02BF-40a5-804A-9AE4E1D78ECF}");

  protected override Guid GUID => PumpFolderFilters._guid;

  public override void Pump()
  {
    IFoldersFilter foldersFilterPumper = this.plugin.Idw.GetFoldersFilterPumper();
    foldersFilterPumper.OnMessage += new MessageEventHandler(this.filter_OnMessage);
    foldersFilterPumper.OnProgress += new ProgressEventHandler(this.filter_OnProgress);
    try
    {
      foldersFilterPumper.PumpTCLinks(this.plugin.idb.DbConnection);
      this.PumpCheckPoint("Перекачка данных фильтрации папок Imbase для Techcard успешно завершена", 100);
    }
    finally
    {
      if (foldersFilterPumper != null)
      {
        foldersFilterPumper.OnMessage -= new MessageEventHandler(this.filter_OnMessage);
        foldersFilterPumper.OnProgress -= new ProgressEventHandler(this.filter_OnProgress);
      }
    }
  }

  private void filter_OnProgress(object sender, ProgressInfo e)
  {
    this.PumpCheckPoint($"Перекачка данных фильтрации папок Imbase для Techcard ({e.Current} из {e.CountAll})", this.CalculatePercent(e.CountAll, e.Current, 1, 99));
  }

  private void filter_OnMessage(object sender, string message)
  {
    this.plugin.appManager.AddWarningMessage(message);
  }
}
