// Decompiled with JetBrains decompiler
// Type: IPSAddIn.Installer.Program
// Assembly: IPSAddIn.Installer, Version=8.0.3.1634, Culture=neutral, PublicKeyToken=null
// MVID: 0B42B756-5F54-4959-820D-851B2C3E0C84
// Assembly location: D:\Projects\IPS Code\AltiumDesigner\IPSAddIn.Installer.exe

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

#nullable disable
namespace IPSAddIn.Installer;

internal class Program
{
  private Assembly _appAssembly;
  private string _appName;
  private string _appBaseDir;

  [STAThread]
  private static void Main(string[] args) => new Program(args).Run();

  public Program(string[] args)
  {
  }

  private void Run()
  {
    try
    {
      this.InitializeAppInfo();
      this.DisplayProgramBanner();
      this.DoWork();
    }
    catch (Exception ex)
    {
      Output.WriteError("Ошибка: " + ex.Message);
      Output.WriteError("Установка расширения невозможна!");
    }
    Output.WriteLine("Нажмите любую клавишу...");
    Console.ReadKey();
  }

  private void InitializeAppInfo()
  {
    this._appAssembly = this.GetType().Assembly;
    this._appName = Path.GetFileNameWithoutExtension(this._appAssembly.Location);
    this._appBaseDir = Environment.CurrentDirectory;
  }

  private void DisplayProgramBanner()
  {
    Output.WriteLine($"{this._appName} v{this._appAssembly.GetName(false).Version}: утилита установки расширения IPSAddIn для интегратора IPS и Altium Designer");
  }

  private void DoWork()
  {
    Output.WriteLine("... чтение реестра");
    AltiumBuild altiumBuild = this.ChoiceAltiumBuild();
    Output.WriteLine("... версия Altium Designer для установки расширения: " + altiumBuild.Version);
    string str = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), altiumBuild.Application);
    ExtensionsXML extensionsXml = Directory.Exists(str) ? new ExtensionsXML(str) : throw new Exception($"Для версии Altium Designer {altiumBuild.Version} отсутствует указанная в реестре папка {str}");
    AddInFolder folderForExtension = extensionsXml.GetFolderForExtension();
    Output.WriteLine("... производится установка в папку " + folderForExtension.FolderPath);
    Output.WriteLine("... копирование файлов расширения");
    folderForExtension.CopyFiles(Path.Combine(this._appBaseDir, Consts.IPSAddInFolderName), folderForExtension.FolderPath);
    Output.WriteLine("... создание резервной копии файла " + extensionsXml.ExtensionsFile);
    extensionsXml.CreateBackupFile();
    Output.WriteLine("... запись информации по расширению в файл " + extensionsXml.ExtensionsFile);
    PluginInfo info = PluginInfo.Create(folderForExtension.FolderPath, this._appAssembly.GetName(false).Version);
    extensionsXml.SetPluginInfo(info);
    Output.WriteLine("Установка успешно завершена!");
  }

  private AltiumBuild ChoiceAltiumBuild()
  {
    List<AltiumBuild> builds = RegistryReader.AltiumBuilds;
    if (builds.Count <= 1)
      return builds[0];
    Output.WriteLine("Обнаружено несколько установленных версий AltiumDesigner.");
    int num1 = 1;
    foreach (AltiumBuild altiumBuild in builds)
      Output.WriteLine($"{num1++}: {altiumBuild.Version}");
    int num2 = OutputQuestion<int>.AskUser("Введите порядковый номер нужной версии", (OutputQuestion<int>.AnswerHandler) ((string answer, out int number) => int.TryParse(answer, out number) && number >= 1 && number <= builds.Count));
    return builds[num2 - 1];
  }
}
