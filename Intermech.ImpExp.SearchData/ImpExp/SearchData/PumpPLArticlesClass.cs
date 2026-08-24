// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.SearchData.PumpPLArticlesClass
// Assembly: Intermech.ImpExp.SearchData, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 218D3933-9EC7-421F-AD43-19C3596D6EE8
// Assembly location: D:\IPS\Client\Intermech.ImpExp.SearchData.dll

using Intermech.ImpExp.Interface;
using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.SearchData;

[TaskDescription("Инициализация данных для производственных ведомостей", "Перекачка производственных ведомостей")]
internal sealed class PumpPLArticlesClass : PumpArticlesClass
{
  public PumpPLArticlesClass(SearchDataPlugin plugin)
    : base(plugin)
  {
    this.taskPump.Repumpble = true;
  }

  public override void Exam()
  {
    PluginSettings.PumpArtVersions = true;
    PluginSettings.PumpSysArtVersions = true;
    base.Exam();
  }

  protected override void BeforePump() => base.BeforePump();

  protected override Dictionary<int, string> WhereSections
  {
    get
    {
      return new Dictionary<int, string>()
      {
        {
          99999990,
          $" and a.section_id= {99999990}"
        }
      };
    }
  }
}
