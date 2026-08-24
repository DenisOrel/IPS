// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Imbase.PumpRebuildKeys
// Assembly: Intermech.ImpExp.Imbase, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 14B82A62-153A-4D0C-8A5E-F24874681A1E
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Imbase.dll

using Intermech.ImpExp.Interface;
using Intermech.Interfaces;
using Intermech.Interfaces.Imbase;
using System;
using System.Threading;

#nullable disable
namespace Intermech.ImpExp.Imbase;

[TaskDescription("'", "Преобразование ключей Imbase")]
[TaskType(PumperType.MetaData)]
internal sealed class PumpRebuildKeys(ImbasePlugin plugin) : PumpImbaseClass(plugin)
{
  public static Guid guid = new Guid("{28CE6F8C-2B63-4631-B72B-806313690C39}");

  protected override Guid GUID => PumpRebuildKeys.guid;

  public override void Pump()
  {
    string status = "Преобразование ключей Imbase";
    this.PumpCheckPoint(status, 1);
    IUserSession userSession = this.plugin.Idw.GetUserSession().Clone("ImpExpImbase.Pump");
    try
    {
      if (!(userSession.GetCustomService(typeof (IKeyConverter)) is IKeyConverter customService))
      {
        this.PumpCheckPoint("Серверная служба IKeyConverter модуля Imbase не найдена. Преобразование ключей Imbase не произведено.", 100);
      }
      else
      {
        customService.Start(userSession.SessionGUID);
        while (customService.State > 0)
        {
          this.PumpCheckPoint(status, customService.Value);
          Thread.Sleep(500);
        }
        if (customService.State != -2)
          return;
        this.PumpCheckPoint("Преобразование ключей Imbase успешно завершено.", 100);
      }
    }
    finally
    {
      userSession.Logout("ImpExpImbase.Pump");
    }
  }
}
