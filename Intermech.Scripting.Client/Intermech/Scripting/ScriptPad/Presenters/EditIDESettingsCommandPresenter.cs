// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.ScriptPad.Presenters.EditIDESettingsCommandPresenter
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using Intermech.Mvp;
using System;

#nullable disable
namespace Intermech.Scripting.ScriptPad.Presenters;

internal sealed class EditIDESettingsCommandPresenter : IDEChildPresenter
{
  private IDESharedState ideSharedState;
  private Action<IDESettings, IDESettings> updateUIAction;

  public EditIDESettingsCommandPresenter(
    IDEPresenter idePresenter,
    IDESharedState ideSharedState,
    Action<IDESettings, IDESettings> updateUIAction = null)
    : base(idePresenter)
  {
    this.ideSharedState = ideSharedState != null ? ideSharedState : throw new ArgumentNullException(nameof (ideSharedState));
    this.updateUIAction = updateUIAction;
  }

  protected override void OnAttachView()
  {
    base.OnAttachView();
    this.IDEView.EditIDESettingsCommand.Click += new EventHandler(this.ProcessEditIDESettingsCommand);
    this.IDEView.EditIDESettingsCommand.Enabled = true;
  }

  protected override void OnDetachView(bool fullDetach)
  {
    this.IDEView.EditIDESettingsCommand.Click -= new EventHandler(this.ProcessEditIDESettingsCommand);
    if (fullDetach)
      this.ClearViewState();
    base.OnDetachView(fullDetach);
  }

  private void ClearViewState() => this.IDEView.EditIDESettingsCommand.Enabled = false;

  private void ProcessEditIDESettingsCommand(object sender, EventArgs e)
  {
    try
    {
      IDESettingsEditorModel model = new IDESettingsEditorModel();
      model.Settings = this.ideSharedState.Settings.Clone();
      MvpContext.ViewService.ShowModal((IPresenter) new IDESettingsEditorPresenter(model), (object) this.IDEView);
      if (model.ModifiedSettings == null)
        return;
      IDESettings settings = this.ideSharedState.Settings;
      this.ideSharedState.Settings = model.ModifiedSettings;
      if (this.updateUIAction == null)
        return;
      this.updateUIAction(settings, model.ModifiedSettings);
    }
    catch (Exception ex)
    {
      this.IDEPresenter.ShowUnhandledException(ex);
    }
  }
}
