// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.TransferSettings`1
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.WebPortal;
using Intermech.PropertyEditors;
using System.ComponentModel;
using System.Diagnostics;

#nullable disable
namespace Intermech.Site.Client;

internal abstract class TransferSettings<TRulesService> : ITransferSettings where TRulesService : ITransferSettingsService
{
  protected bool inited;
  protected bool createDetailLog;

  private TRulesService GetService()
  {
    return (TRulesService) (ApplicationServices.Container.GetService(typeof (IMServerService)) as IMServerService).GetCustomService(typeof (TRulesService));
  }

  public void Apply()
  {
    TRulesService service = this.GetService();
    service.CreateDetailTaskLog = this.createDetailLog;
    this.OnApply(service);
  }

  public void Load()
  {
    TRulesService service = this.GetService();
    this.createDetailLog = service.CreateDetailTaskLog;
    this.OnLoad(service);
  }

  public abstract void OnApply(TRulesService service);

  public abstract void OnLoad(TRulesService service);

  protected void CheckInited()
  {
    if (this.inited)
      return;
    this.Load();
    this.inited = true;
  }

  public void OnCancel() => this.inited = false;

  [DisplayName("Подробный лог задачи")]
  [Description("Создавать для каждой задачи подробный лог-файл")]
  [TypeConverter(typeof (YesNoConverter))]
  public bool CreateDetailLog
  {
    [DebuggerStepThrough] get
    {
      this.CheckInited();
      return this.createDetailLog;
    }
    set => this.createDetailLog = value;
  }
}
