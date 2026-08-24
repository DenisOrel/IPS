// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.ScriptPad.OpenScriptData
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using Intermech.Scripting.Common.DesignTime;
using System;
using System.Collections.Generic;
using System.Text;

#nullable disable
namespace Intermech.Scripting.ScriptPad;

internal sealed class OpenScriptData
{
  private ScriptProject project;
  private bool modified;

  public OpenScriptData(ScriptProject scriptProject, Encoding encoding, bool readOnlyMode)
  {
    if (scriptProject == null)
      throw new ArgumentNullException(nameof (scriptProject));
    if (encoding == null)
      throw new ArgumentNullException(nameof (encoding));
    this.project = scriptProject;
    this.LanguageInfo = scriptProject.LanguageInfo;
    this.Encoding = encoding;
    this.ReadOnlyMode = readOnlyMode;
    this.ContextMenuActions = new List<ITextEditorUIAction>();
    this.ChildPresenters = new List<IDEChildPresenter>();
  }

  public ScriptProject Project
  {
    get => this.project;
    internal set
    {
      if (value == null)
        throw new ArgumentNullException(nameof (value));
      if (value.LanguageInfo.Name != this.LanguageInfo.Name)
        throw new ArgumentException("При замене проекта сценария язык сценария должен совпадать.", nameof (value));
      if (this.project == value)
        return;
      this.project = value;
      if (this.ProjectChanged != null)
        this.ProjectChanged((object) this, EventArgs.Empty);
      this.Modified = true;
    }
  }

  public event EventHandler ProjectChanged;

  public LanguageInfo LanguageInfo { get; private set; }

  public Encoding Encoding { get; private set; }

  public bool ReadOnlyMode { get; private set; }

  public ITextEditorAction CommentSelectionAction { get; internal set; }

  public ITextEditorAction UncommentSelectionAction { get; internal set; }

  public ITextEditorAction FormatIndentsAction { get; internal set; }

  public List<ITextEditorUIAction> ContextMenuActions { get; private set; }

  public List<IDEChildPresenter> ChildPresenters { get; private set; }

  public bool Modified
  {
    get => this.modified;
    set
    {
      if (this.modified == value)
        return;
      this.modified = value;
      if (this.ModifiedChanged == null)
        return;
      this.ModifiedChanged((object) this, EventArgs.Empty);
    }
  }

  public event EventHandler ModifiedChanged;

  public IScriptWindow Window { get; set; }

  public string GetDisplayName()
  {
    IScriptDisplayBehavior displayBehavior = this.Project.Behaviors.GetDisplayBehavior(false);
    string displayName = displayBehavior != null ? displayBehavior.GetDisplayName() : this.Project.Name;
    if (string.IsNullOrEmpty(displayName))
      displayName = $"Новый сценарий {this.LanguageInfo.Name}";
    if (this.ReadOnlyMode)
      displayName = $"{displayName} (только чтение)";
    return displayName;
  }
}
