// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.TechProcPump.Common.ExperementPump
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.TechCard.Common;
using Intermech.Interfaces.Client;
using System;

#nullable disable
namespace Intermech.ImpExp.TechCard.TechProcPump.Common;

[TaskDescription("Инициализация Экспереремента", "Эксперемент")]
internal class ExperementPump(PluginClass plugin) : PumpClass(plugin)
{
  private Guid _guid = new Guid("{34E4A582-40DB-413c-90BE-3078187A36E9}");
  private long index;

  protected override Guid GUID => this._guid;

  public override void Exam() => base.Exam();

  private void UseChace()
  {
    Random random = new Random();
    Array values = Enum.GetValues(typeof (ImportingCategory));
    int index1 = random.Next(values.Length - 1);
    ImportingCategory category = (ImportingCategory) values.GetValue(index1);
    IImportingData importingData1;
    if (!(ServicesManager.GetService(typeof (ICache)) is ICache service))
      importingData1 = (IImportingData) null;
    else
      importingData1 = service.GetCache(category);
    IImportingData importingData2 = importingData1;
    try
    {
      for (int index2 = 0; index2 < random.Next(); ++index2)
      {
        importingData2.AddValue(category, (object) this.index++, (long) random.Next());
        importingData2.GetNewKey(category, (object) (this.index - 1L));
      }
    }
    catch (Exception ex)
    {
      TechcardConsts.Plugin.appManager.AddErrorMessage(ex.Message);
      if (!(ex is OutOfMemoryException))
        return;
      throw;
    }
    finally
    {
      service?.ReleaseCache(category);
    }
  }

  public override void Pump()
  {
    for (int index = 0; index < 1000; ++index)
      this.UseChace();
    base.Pump();
  }
}
