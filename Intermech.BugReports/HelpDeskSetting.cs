// Decompiled with JetBrains decompiler
// Type: Intermech.BugReports.HelpDeskSetting
// Assembly: Intermech.BugReports, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 16F80F46-2B9D-4747-9BFD-4CC209192F4E
// Assembly location: D:\IPS\Client\Intermech.BugReports.dll

using System.CodeDom.Compiler;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.CompilerServices;

#nullable disable
namespace Intermech.BugReports;

[CompilerGenerated]
[GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "11.0.0.0")]
internal sealed class HelpDeskSetting : ApplicationSettingsBase
{
  private static HelpDeskSetting defaultInstance = (HelpDeskSetting) SettingsBase.Synchronized((SettingsBase) new HelpDeskSetting());

  public static HelpDeskSetting Default => HelpDeskSetting.defaultInstance;

  [UserScopedSetting]
  [DebuggerNonUserCode]
  [DefaultSettingValue("")]
  public string UserName
  {
    get => (string) this[nameof (UserName)];
    set => this[nameof (UserName)] = (object) value;
  }

  [UserScopedSetting]
  [DebuggerNonUserCode]
  [DefaultSettingValue("")]
  public string Password
  {
    get => (string) this[nameof (Password)];
    set => this[nameof (Password)] = (object) value;
  }
}
