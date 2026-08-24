// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.BaseBackgroundReader
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Pdm;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Queries;
using System;
using System.Data;
using System.Diagnostics;
using System.Threading;

#nullable disable
namespace Intermech.Pdm.Compositions;

public class BaseBackgroundReader : IContextAware
{
  protected Guid selectGuid;
  protected DataTable queryResult;
  protected BackgroundState state;
  protected Thread thread;
  protected RuntimeSearchScheme scheme;
  public StateChanged StateChangedEvent;
  public RecordMapping Mapping;
  protected IServiceProvider services;

  public BaseBackgroundReader(IServiceProvider services)
  {
    this.services = services;
    this.selectGuid = Guid.NewGuid();
    this.state = BackgroundState.Empty;
  }

  public virtual IServiceProvider Services
  {
    [DebuggerStepThrough] get => this.services;
    set => this.services = value;
  }

  public BackgroundState State
  {
    get => this.state;
    set => this.state = value;
  }

  public DataTable QueryResult
  {
    get => this.queryResult;
    set => this.queryResult = value;
  }

  protected void ChangeState(BackgroundState state)
  {
    this.state = state;
    StateChanged stateChangedEvent = this.StateChangedEvent;
    if (stateChangedEvent == null)
      return;
    stateChangedEvent((object) this, new StateChangedEventArgs(state, 0));
  }

  protected void ChangeState(BackgroundState state, int percent)
  {
    this.state = state;
    StateChanged stateChangedEvent = this.StateChangedEvent;
    if (stateChangedEvent == null)
      return;
    stateChangedEvent((object) this, new StateChangedEventArgs(state, percent));
  }

  public void Cancel()
  {
    if (this.thread != null)
      this.thread.Abort();
    ((ICustomCompositionService) (ApplicationServices.Container.GetService(typeof (IMServerService)) as IMServerService).GetCustomService(typeof (ICompositionService))).CancelSelect(this.selectGuid);
    this.ChangeState(BackgroundState.Fill);
  }
}
