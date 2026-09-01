// Decompiled with JetBrains decompiler
// Type: IPSAddIn.Installer.AddInFolder
// Assembly: IPSAddIn.Installer, Version=8.0.3.1634, Culture=neutral, PublicKeyToken=null
// MVID: 0B42B756-5F54-4959-820D-851B2C3E0C84
// Assembly location: D:\Projects\IPS Code\AltiumDesigner\IPSAddIn.Installer.exe

using System;
using System.IO;

#nullable disable
namespace IPSAddIn.Installer;

internal class AddInFolder
{
  public string FolderPath { get; private set; }

  public AddInFolder(string folder)
  {
    this.FolderPath = !string.IsNullOrEmpty(folder) ? folder : throw new ArgumentNullException(nameof (folder));
  }

  public static AddInFolder Create(string altiumDataFolder, string foundFolder)
  {
    string str = foundFolder == string.Empty ? Path.Combine(altiumDataFolder, Consts.ExtensionsRootFolderName, Consts.IPSAddInFolderName) : foundFolder;
    if (Directory.Exists(str))
    {
      if (!(OutputQuestion<string>.AskUser($"Папка {str} не пустая, файлы будут перезаписаны! Продолжить установку? Да(Y)/Нет(N)", (OutputQuestion<string>.AnswerHandler) ((string answer, out string stringValue) =>
      {
        stringValue = answer.ToUpper();
        return stringValue == "Y" || stringValue == "N";
      })) == "Y"))
        throw new Exception("Пользователь прервал установку.");
      Directory.Delete(str, true);
      Directory.CreateDirectory(str);
    }
    else
      Directory.CreateDirectory(str);
    return new AddInFolder(str);
  }

  public void CopyFiles(string sourceDir, string targetDir)
  {
    if (!Directory.Exists(targetDir))
      Directory.CreateDirectory(targetDir);
    foreach (string file in Directory.GetFiles(sourceDir))
    {
      string fileName = Path.GetFileName(file);
      File.Copy(file, Path.Combine(targetDir, fileName), true);
    }
    foreach (string directory in Directory.GetDirectories(sourceDir))
    {
      string fileName = Path.GetFileName(directory);
      string targetDir1 = Path.Combine(targetDir, fileName);
      this.CopyFiles(directory, targetDir1);
    }
  }
}
