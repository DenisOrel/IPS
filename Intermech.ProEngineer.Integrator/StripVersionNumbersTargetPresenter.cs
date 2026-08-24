// Decompiled with JetBrains decompiler
// Type: Intermech.ProEngineer.Integrator.StripVersionNumbersTargetPresenter
// Assembly: Intermech.ProEngineer.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 19987673-5EB5-4BB3-AE60-6A96614A14F3
// Assembly location: D:\IPS\Client\Intermech.ProEngineer.Integrator.dll

using Intermech.Mvp;
using System;

#nullable disable
namespace Intermech.ProEngineer.Integrator;

internal sealed class StripVersionNumbersTargetPresenter : Presenter<IStripVersionNumbserTargetView>
{
  private StripVersionNumbersTarget selectedTarget;

  public StripVersionNumbersTarget GetSelectedTarget() => this.selectedTarget;

  protected override void OnAttachView()
  {
    base.OnAttachView();
    this.View.OperationConfirmed += new EventHandler(this.OnOperationConfirmed);
  }

  protected override void OnDetachView()
  {
    base.OnDetachView();
    this.View.OperationConfirmed -= new EventHandler(this.OnOperationConfirmed);
  }

  private void OnOperationConfirmed(object sender, EventArgs e)
  {
    this.selectedTarget = this.View.GetSelectedTarget();
  }
}
