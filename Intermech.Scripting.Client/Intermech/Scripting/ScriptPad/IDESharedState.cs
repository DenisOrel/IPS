// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.ScriptPad.IDESharedState
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using Intermech.Scripting.Common.DesignTime;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.Scripting.ScriptPad;

internal sealed class IDESharedState
{
  private readonly ICollection<LanguageSessionData> languageSessions;
  private readonly IDictionary<string, LanguageSessionData> languageSessionsByName;
  private ICollection<IDEPresenter> runningIDEs;
  private IDESettingsService settingsService;
  private IDESettings settings;

  public IDESharedState(IDESettingsService settingsService)
  {
    if (settingsService == null)
      throw new ArgumentNullException(nameof (settingsService));
    this.languageSessions = (ICollection<LanguageSessionData>) new LinkedList<LanguageSessionData>();
    this.languageSessionsByName = (IDictionary<string, LanguageSessionData>) new Dictionary<string, LanguageSessionData>();
    this.runningIDEs = (ICollection<IDEPresenter>) new HashSet<IDEPresenter>();
    this.settingsService = settingsService;
  }

  public void RegisterRunningIDE(IDEPresenter ide)
  {
    if (ide == null)
      throw new ArgumentNullException(nameof (ide));
    this.runningIDEs.Add(ide);
  }

  public void UnregisterRunningIDE(IDEPresenter ide)
  {
    if (ide == null)
      throw new ArgumentNullException(nameof (ide));
    if (!this.runningIDEs.Remove(ide) || this.runningIDEs.Count != 0)
      return;
    this.ReleaseResources();
  }

  private void ReleaseResources()
  {
    foreach (LanguageSessionData languageSession in (IEnumerable<LanguageSessionData>) this.languageSessions)
      languageSession.ShutdownSession();
    this.languageSessions.Clear();
    this.languageSessionsByName.Clear();
  }

  public LanguageSessionData GetOrCreateLanguageSessionData(IDEModel ideModel, string languageName)
  {
    if (ideModel == null)
      throw new ArgumentNullException(nameof (ideModel));
    if (languageName == null)
      throw new ArgumentNullException(nameof (languageName));
    LanguageSessionData languageSessionData;
    if (!this.languageSessionsByName.TryGetValue(languageName, out languageSessionData))
    {
      LanguageDescriptor byLanguageName = ideModel.LanguageRegistry.GetByLanguageName(languageName, true);
      languageSessionData = new LanguageSessionData(byLanguageName, byLanguageName.Services.GetSessionService().LoadSessionParameters((ISettingsContainer) this.settingsService));
      this.languageSessionsByName.Add(languageName, languageSessionData);
      this.languageSessions.Add(languageSessionData);
    }
    return languageSessionData;
  }

  public IDESettingsService SettingsService => this.settingsService;

  public IDESettings Settings
  {
    get
    {
      if (this.settings == null)
        this.settings = this.LoadSettings();
      return this.settings;
    }
    set
    {
      if (value == null)
        throw new ArgumentNullException(nameof (value));
      if (this.settings == value)
        return;
      this.settings = value;
      this.SaveSettings(value);
    }
  }

  private IDESettings LoadSettings()
  {
    IDESettings ideSettings = new IDESettings();
    ideSettings.FontFamily = this.settingsService.ReadString("TextEditor", "FontFamily", ideSettings.FontFamily);
    ideSettings.FontSize = this.settingsService.ReadInt32("TextEditor", "FontSize", ideSettings.FontSize);
    ideSettings.EnableCodeCompletion = this.settingsService.ReadBoolean("CodeCompletion", "Enabled", ideSettings.EnableCodeCompletion);
    ideSettings.XmlDocPathList = new List<string>((IEnumerable<string>) this.settingsService.ReadString("CodeCompletion", "XmlDocPathList", string.Empty).Split(new string[1]
    {
      ";"
    }, StringSplitOptions.RemoveEmptyEntries));
    return ideSettings;
  }

  private void SaveSettings(IDESettings settings)
  {
    this.settingsService.WriteString("TextEditor", "FontFamily", settings.FontFamily);
    this.settingsService.WriteInt32("TextEditor", "FontSize", settings.FontSize);
    this.settingsService.WriteBoolean("CodeCompletion", "Enabled", settings.EnableCodeCompletion);
    this.settingsService.WriteString("CodeCompletion", "XmlDocPathList", string.Join(";", (IEnumerable<string>) settings.XmlDocPathList));
    this.settingsService.Flush();
  }
}
