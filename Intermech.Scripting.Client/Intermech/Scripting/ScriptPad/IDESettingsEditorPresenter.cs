// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.ScriptPad.IDESettingsEditorPresenter
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using Intermech.Mvp;
using Intermech.Runtime;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.Scripting.ScriptPad;

internal sealed class IDESettingsEditorPresenter : Presenter<IIDESettingsEditorView>
{
  private IDESettingsEditorModel model;

  public IDESettingsEditorPresenter(IDESettingsEditorModel model)
  {
    if (model == null)
      throw new ArgumentNullException(nameof (model));
    this.model = model.Settings != null ? model : throw PropertyExceptions.PropertyNotSetException((object) model, "Settings");
  }

  protected override void OnAttachView()
  {
    base.OnAttachView();
    this.model.ModifiedSettings = (IDESettings) null;
    this.View.FontFamily = this.model.Settings.FontFamily;
    this.View.FontSize = this.model.Settings.FontSize.ToString();
    this.View.EnableCodeCompletion = this.model.Settings.EnableCodeCompletion;
    this.View.XmlDocPathList = (ICollection<string>) this.model.Settings.XmlDocPathList;
    this.View.OperationConfirmed += new EventHandler(this.OnApplyChanges);
  }

  protected override void OnDetachView()
  {
    this.View.FontFamily = string.Empty;
    this.View.FontSize = string.Empty;
    this.View.EnableCodeCompletion = false;
    this.View.XmlDocPathList = (ICollection<string>) null;
    this.View.OperationConfirmed -= new EventHandler(this.OnApplyChanges);
    base.OnDetachView();
  }

  private void OnApplyChanges(object sender, EventArgs e)
  {
    IDESettings ideSettings = this.model.Settings.Clone();
    ideSettings.FontFamily = !string.IsNullOrEmpty(this.View.FontFamily) ? this.View.FontFamily : throw new ApplicationException("Не задано имя семейства шрифтов.");
    int result;
    if (!int.TryParse(this.View.FontSize, out result) || result <= 0)
      throw new ApplicationException("Задан некорректный размер шрифта.");
    ideSettings.FontSize = result;
    ideSettings.EnableCodeCompletion = this.View.EnableCodeCompletion;
    ideSettings.XmlDocPathList = new List<string>((IEnumerable<string>) this.View.XmlDocPathList);
    this.model.ModifiedSettings = ideSettings;
  }
}
