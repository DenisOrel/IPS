// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.Projects.DiskScripts.OpenDiskScriptDialogService
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using Intermech.Collections;
using Intermech.IO;
using Intermech.Scripting.Common.DesignTime;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Scripting.Projects.DiskScripts;

internal sealed class OpenDiskScriptDialogService : DiskScriptDialogService
{
  private string scriptDirectory;
  private IScriptProjectRepository repository;
  private OpenFileDialog ofdOpenScript;

  public OpenDiskScriptDialogService(DiskScriptRepository repository, string scriptDirectory)
  {
    if (repository == null)
      throw new ArgumentNullException(nameof (repository));
    if (scriptDirectory == null)
      throw new ArgumentNullException(nameof (scriptDirectory));
    this.repository = (IScriptProjectRepository) repository;
    this.scriptDirectory = scriptDirectory;
    this.ofdOpenScript = new OpenFileDialog();
    this.ofdOpenScript.Title = "Выберите сценарий для открытия";
    this.ofdOpenScript.Filter = "Все файлы|*.*";
    this.ofdOpenScript.SupportMultiDottedExtensions = true;
    this.ofdOpenScript.InitialDirectory = this.scriptDirectory;
    this.ofdOpenScript.RestoreDirectory = true;
  }

  public ScriptProject TryOpenScript(ICollection<LanguageInfo> languageFilter)
  {
    this.ofdOpenScript.Filter = this.CalculateDialogFilters(languageFilter);
    this.ofdOpenScript.DefaultExt = languageFilter == null || languageFilter.Count == 0 ? string.Empty : languageFilter.First<LanguageInfo>().SourceExtension;
    this.ofdOpenScript.FileName = string.Empty;
    if (this.ofdOpenScript.ShowDialog() != DialogResult.OK)
      return (ScriptProject) null;
    string fileName = this.ofdOpenScript.FileName;
    string @extension = Path.GetExtension(fileName);
    CollectionUtils.Find<LanguageInfo>((IEnumerable<LanguageInfo>) languageFilter, (Predicate<LanguageInfo>) (x => PathUtils.IsSamePath(x.SourceExtension, @extension)));
    return this.repository.Get((object) new DiskScriptKey(fileName));
  }
}
