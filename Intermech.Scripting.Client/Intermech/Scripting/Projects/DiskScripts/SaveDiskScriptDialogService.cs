// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.Projects.DiskScripts.SaveDiskScriptDialogService
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using Intermech.Scripting.Common.DesignTime;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Scripting.Projects.DiskScripts;

internal sealed class SaveDiskScriptDialogService : DiskScriptDialogService
{
  private string scriptDirectory;
  private SaveFileDialog sfdSaveScript;

  public SaveDiskScriptDialogService(string scriptDirectory)
  {
    this.scriptDirectory = scriptDirectory != null ? scriptDirectory : throw new ArgumentNullException(nameof (scriptDirectory));
    this.sfdSaveScript = new SaveFileDialog();
    this.sfdSaveScript.Title = "Куда сохранить сценарий?";
    this.sfdSaveScript.Filter = "Все файлы|*.*";
    this.sfdSaveScript.SupportMultiDottedExtensions = true;
    this.sfdSaveScript.InitialDirectory = this.scriptDirectory;
    this.sfdSaveScript.RestoreDirectory = true;
  }

  public string TrySelectFilePath(ScriptProject scriptProject)
  {
    LanguageInfo languageInfo = scriptProject != null ? scriptProject.LanguageInfo : throw new ArgumentNullException(nameof (scriptProject));
    this.sfdSaveScript.Filter = this.CalculateDialogFilters((ICollection<LanguageInfo>) new LanguageInfo[1]
    {
      languageInfo
    });
    this.sfdSaveScript.DefaultExt = languageInfo.SourceExtension;
    this.sfdSaveScript.FileName = string.Empty;
    return this.sfdSaveScript.ShowDialog() != DialogResult.OK ? (string) null : this.sfdSaveScript.FileName;
  }
}
