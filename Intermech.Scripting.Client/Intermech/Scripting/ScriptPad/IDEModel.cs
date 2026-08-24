// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.ScriptPad.IDEModel
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using Intermech.Scripting.Common.DesignTime;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.Scripting.ScriptPad;

internal class IDEModel : FreezableObject, ICloneable
{
  private IDEMode mode;
  private IDESettingsService settingsService;
  private LanguageRegistry languageRegistry;
  private IScriptSystemService scriptSystem;
  private List<(ScriptProject, bool)> openAtStartup;

  public IDEModel()
  {
    this.mode = IDEMode.Normal;
    this.settingsService = (IDESettingsService) new InMemoryIDESettingsService();
    this.openAtStartup = new List<(ScriptProject, bool)>();
  }

  public IDEMode Mode
  {
    get => this.mode;
    set
    {
      this.RequireNotFrozenBeforePropertyChange(nameof (Mode));
      this.mode = value;
    }
  }

  public IDESettingsService SettingsService
  {
    get => this.settingsService;
    set
    {
      if (value == null)
        throw new ArgumentNullException(nameof (value));
      this.RequireNotFrozenBeforePropertyChange(nameof (SettingsService));
      this.settingsService = value;
    }
  }

  public LanguageRegistry LanguageRegistry
  {
    get => this.languageRegistry;
    set
    {
      this.RequireNotFrozenBeforePropertyChange(nameof (LanguageRegistry));
      this.languageRegistry = value;
    }
  }

  public IScriptSystemService ScriptSystem
  {
    get => this.scriptSystem;
    set
    {
      this.RequireNotFrozenBeforePropertyChange(nameof (ScriptSystem));
      this.scriptSystem = value;
    }
  }

  public ICollection<(ScriptProject, bool)> OpenAtStartup
  {
    get => (ICollection<(ScriptProject, bool)>) this.openAtStartup;
  }

  public IDEModel Clone()
  {
    IDEModel ideModel = new IDEModel();
    ideModel.mode = this.mode;
    ideModel.settingsService = this.settingsService;
    ideModel.languageRegistry = this.languageRegistry;
    ideModel.scriptSystem = this.scriptSystem;
    ideModel.openAtStartup.AddRange((IEnumerable<(ScriptProject, bool)>) this.openAtStartup);
    return ideModel;
  }

  object ICloneable.Clone() => (object) this.Clone();
}
