// Decompiled with JetBrains decompiler
// Type: CSharpPlugin.IPSAddInProxy
// Assembly: IPSAddIn, Version=8.0.3.1634, Culture=neutral, PublicKeyToken=null
// MVID: F6758E82-0F4D-46BA-A517-315691E31B38
// Assembly location: D:\Projects\IPS Code\AltiumDesigner\IPSAddIn\IPSAddIn.dll

using EDP;
using Intermech.AltiumDesigner.Interfaces;
using Intermech.Diagnostics;
using Intermech.Extensions;
using Intermech.Integrators;
using Intermech.Settings;
using SCH;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;

#nullable disable
namespace CSharpPlugin;

internal sealed class IPSAddInProxy : LongLifeObject, IIPSAddIn, IConnectableApp
{
  private static readonly bool DebugMode;

  public ISchDocument FindSCHObject(string fileName)
  {
    IPSAddInProxy.AddDebugMessage($"FindSCHObject({fileName})");
    if (string.IsNullOrEmpty(fileName))
      throw new ArgumentNullException(nameof (fileName));
    if (!File.Exists(fileName))
      return (ISchDocument) null;
    IProject projectIntf = Helper.Workspace.DM_FocusedProject();
    if (projectIntf == null)
      return (ISchDocument) null;
    for (int argIndex = 0; argIndex < projectIntf.DM_LogicalDocumentCount(); ++argIndex)
    {
      IDocument document = projectIntf.DM_LogicalDocuments(argIndex);
      string str1 = document.DM_DocumentKind();
      string str2 = document.DM_FullPath();
      if (str1.Equals("SCH") && str2.Equals(fileName))
        return this.GetSchDocument(fileName, true);
    }
    return (ISchDocument) null;
  }

  public void OpenObject(string fileName)
  {
    IPSAddInProxy.AddDebugMessage($"OpenObject({fileName})");
    this.CheckExistsFileName(fileName);
    this.RunWorkspaceManagerCommand(WorkspaceCommand.OpenObject, fileName);
  }

  public void CloseObject(string fileName)
  {
    IPSAddInProxy.AddDebugMessage($"CloseObject({fileName})");
    this.CheckExistsFileName(fileName);
    if (!DXP.GlobalVars.Client.IsDocumentOpen(fileName))
      return;
    int num = int.Parse(((IEnumerable<string>) DXP.GlobalVars.Client.GetProductVersion().Split('.')).FirstOrDefault<string>() ?? "0");
    Action action = (Action) (() => this.RunWorkspaceManagerCommand(WorkspaceCommand.CloseObject, (IDictionary<string, string>) new Dictionary<string, string>()
    {
      {
        "ObjectKind",
        ADDocumentTypeHelper.ParseFromFileName(fileName) == ADDocumentType.Project ? "ProjectAndDocuments" : "Document"
      }
    }));
    if (num <= 19)
      Task.Run(action);
    else
      action();
  }

  public void SaveObject(string fileName)
  {
    IPSAddInProxy.AddDebugMessage($"SaveObject({fileName})");
    this.CheckExistsFileName(fileName);
    this.RunWorkspaceManagerCommand(WorkspaceCommand.OpenObject, fileName);
    this.RunWorkspaceManagerCommand(WorkspaceCommand.SaveObject, fileName);
  }

  private void CheckExistsFileName(string fileName)
  {
    IPSAddInProxy.AddDebugMessage($"CheckExistsFileName({fileName})");
    if (string.IsNullOrEmpty(fileName))
      throw new ArgumentNullException(nameof (fileName));
    if (!File.Exists(fileName))
      throw new FileNotFoundException($"Не найден файл {Path.GetFileName(fileName)}!", fileName);
  }

  public IntPtr GetMainWindowHandle() => new IntPtr((long) DXP.GlobalVars.Client.GetMainWindowHandle());

  public string GetVersion() => this.GetType().Assembly.GetName(false).Version.ToString();

  public IADProject GetProject(string fileName)
  {
    IPSAddInProxy.AddDebugMessage($"GetProject({fileName})");
    this.OpenObject(fileName);
    return (IADProject) new Project(this, Helper.Workspace.DM_GetProjectFromPath(fileName));
  }

  public ISchDocument GetSchDocument(string fileName, bool open)
  {
    IPSAddInProxy.AddDebugMessage($"{nameof (GetSchDocument)}({fileName},{open})");
    ISch_Document schDocument;
    if (open)
    {
      this.OpenObject(fileName);
      schDocument = Helper.SCHServer.GetCurrentSchDocument();
    }
    else
      schDocument = Helper.SCHServer.GetSchDocumentByPath(fileName);
    Helper.CheckEntity((object) schDocument, typeof (ISch_Document));
    return (ISchDocument) new SchDocument(this, schDocument, fileName);
  }

  public IPCBDwfDocument GetPCBDwfDocument(string fileName)
  {
    IPSAddInProxy.AddDebugMessage($"GetPCBDwfDocument({fileName})");
    return (IPCBDwfDocument) new PCBDwfDocument(this, fileName);
  }

  public IMbsDocument GetMbsDocument(string fileName)
  {
    IPSAddInProxy.AddDebugMessage($"GetMbsDocument({fileName})");
    return (IMbsDocument) new MbsDocument(this, fileName);
  }

  public IPCBDocument GetPCBDocument(string fileName)
  {
    IPSAddInProxy.AddDebugMessage($"GetPCBDocument({fileName})");
    this.OpenObject(fileName);
    return (IPCBDocument) new PCBDocument(fileName);
  }

  private void RunWorkspaceManagerCommand(WorkspaceCommand command, string fileName)
  {
    this.RunWorkspaceManagerCommand(command, (IDictionary<string, string>) new Dictionary<string, string>()
    {
      {
        "Filename",
        fileName
      }
    });
  }

  private void RunWorkspaceManagerCommand(
    WorkspaceCommand command,
    IDictionary<string, string> specialParams)
  {
    IPSAddInProxy.AddDebugMessage($"{nameof (RunWorkspaceManagerCommand)}({command}, {JsonSerializer.Serialize<IDictionary<string, string>>(specialParams)})");
    string str = string.Empty;
    foreach (KeyValuePair<string, string> specialParam in (IEnumerable<KeyValuePair<string, string>>) specialParams)
      str = str.AppendInBuilder(specialParam.Key.AppendInBuilder(specialParam.Value, "="), "|");
    DXP.Utils.RunCommand($"WorkspaceManager:{command.ToString()}", str);
  }

  public void ExportToPDF(string fileName, string authenticFilePath)
  {
    this.ExportToPDF(new string[1]{ fileName }, authenticFilePath);
  }

  public void ExportToPDF(string[] fileNames, string authenticFilePath)
  {
    Helper.Workspace.DM_AddOutputLine("Запущено формирование аутентичного файла " + fileNames[0], false, false);
    for (int index = 1; index < fileNames.Length; ++index)
      Helper.Workspace.DM_AddOutputLine("  + " + fileNames[index], false, false);
    if (File.Exists(authenticFilePath))
      File.Delete(authenticFilePath);
    string path3 = Path.GetExtension(authenticFilePath);
    bool flag1 = path3.ToLower() == ".step";
    string fileName = fileNames[0];
    string str1 = Path.GetExtension(fileName);
    if (str1.ToUpper() == ".MbsDoc".ToUpper())
      throw new NotImplementedException($"Формирование PDF файла для формата {str1} в настроящий момент не предусмотрено из-за ошибки API MultiBoardAssembly в Altium Designer.".NewLine().AppendInBuilder("Сформируйте и прикрепите файл вручную"));
    this.OpenObject(fileName);
    ADDocumentType fromFileName = ADDocumentTypeHelper.ParseFromFileName(fileName);
    IProject projectIntf;
    switch (fromFileName)
    {
      case ADDocumentType.Project:
        projectIntf = Helper.Workspace.DM_GetProjectFromPath(fileName);
        break;
      case ADDocumentType.SCH:
      case ADDocumentType.PCB:
      case ADDocumentType.DWG:
        IDocument documentFromPath = Helper.Workspace.DM_GetDocumentFromPath(fileName);
        projectIntf = (documentFromPath != null ? documentFromPath.DM_Project() : (IProject) null) ?? Helper.Workspace.DM_FreeDocumentsProject();
        break;
      default:
        throw new NotImplementedException($"Для выбранного типа файла {str1} не предусмотрен экспорт в PDF! Обратитесь к разработчику");
    }
    if (projectIntf == null)
      throw new ItemNullsNotAllowedException("ADProject");
    string str2 = fileName + ".OutJob";
    if (File.Exists(str2))
      File.Delete(str2);
    using (File.Create(str2))
      ;
    IniFile iniFile = new IniFile(str2);
    string section1 = "OutputJobFile";
    iniFile.Write(section1, "Version", "1.0");
    string section2 = "OutputGroup1";
    iniFile.Write(section2, "TargetOutputMedium", flag1 ? "Folder Structure" : "PDF");
    iniFile.Write(section2, "OutputMedium1", flag1 ? "Folder Structure" : "PDF");
    iniFile.Write(section2, "OutputMedium1_Type", flag1 ? "GeneratedFiles" : "Publish");
    bool flag2 = fromFileName == ADDocumentType.SCH;
    List<string> stringList1 = new List<string>();
    List<string> stringList2 = new List<string>();
    List<string> stringList3 = new List<string>();
    List<string> stringList4 = new List<string>();
    for (int argIndex = 0; argIndex < projectIntf.DM_LogicalDocumentCount(); ++argIndex)
      stringList4.Add(projectIntf.DM_LogicalDocuments(argIndex).DM_FullPath());
    string fileName1 = stringList4.Find((Predicate<string>) (f => f != fileName && Path.GetFileName(f) == Path.GetFileName(fileName)));
    if (!fileName1.IsNullOrWhiteSpace())
      this.CloseObject(fileName1);
    if (fromFileName == ADDocumentType.Project)
    {
      foreach (string path in stringList4)
      {
        if (Path.HasExtension(path))
        {
          string upper = Path.GetExtension(path).ToUpper();
          if (upper.Equals(".SchDoc".ToUpper()))
            flag2 = true;
          else if (upper.Equals(".PcbDoc".ToUpper()))
            stringList1.Add(Path.GetFileName(path));
        }
      }
    }
    else if (fromFileName == ADDocumentType.SCH && ((IEnumerable<string>) fileNames).Count<string>() > 1)
    {
      foreach (string str3 in ((IEnumerable<string>) fileNames).Where<string>((Func<string, bool>) (f => f != fileName)))
      {
        string file = str3;
        if (projectIntf.DM_IndexOfSourceDocument(file) < 0)
        {
          string fileName2 = stringList4.Find((Predicate<string>) (f => f != file && Path.GetFileName(f) == Path.GetFileName(file)));
          if (!fileName2.IsNullOrWhiteSpace())
            this.CloseObject(fileName2);
          stringList3.Add(file);
          this.OpenObject(file);
        }
      }
    }
    else
    {
      switch (fromFileName)
      {
        case ADDocumentType.PCB:
          stringList1.Add(Path.GetFileName(fileName));
          break;
        case ADDocumentType.DWG:
          stringList2.Add(Path.GetFileName(fileName));
          break;
      }
    }
    int num = 1;
    if (flag2 && !flag1)
    {
      foreach (string fileName3 in fileNames)
      {
        iniFile.Write(section2, $"OutputCategory{num}", "Documentation");
        iniFile.Write(section2, $"OutputType{num}", "Schematic Print");
        iniFile.Write(section2, $"OutputName{num}", "Schematic Prints".AppendInBuilder(fromFileName == ADDocumentType.SCH ? " of " + Path.GetFileName(fileName3) : ""));
        if (fromFileName == ADDocumentType.SCH)
          iniFile.Write(section2, $"OutputDocumentPath{num}", Path.GetFileName(fileName3));
        ++num;
      }
    }
    foreach (string str4 in stringList1)
    {
      iniFile.Write(section2, $"OutputCategory{num}", flag1 ? "Export" : "Documentation");
      iniFile.Write(section2, $"OutputType{num}", flag1 ? "ExportSTEP" : "PCB Print");
      iniFile.Write(section2, $"OutputName{num}", flag1 ? "Export STEP" : "PCB Prints");
      iniFile.Write(section2, $"OutputDocumentPath{num}", str4);
      ++num;
    }
    if (!flag1)
    {
      foreach (string str5 in stringList2)
      {
        iniFile.Write(section2, $"OutputCategory{num}", "Documentation");
        iniFile.Write(section2, $"OutputType{num}", "PCBDrawing");
        iniFile.Write(section2, $"OutputName{num}", "Draftsman");
        iniFile.Write(section2, $"OutputDocumentPath{num}", str5);
        ++num;
      }
    }
    string section3 = "PublishSettings";
    string directoryName = Path.GetDirectoryName(authenticFilePath);
    iniFile.Write(section3, "OutputFilePath1", flag1 ? directoryName : Path.Combine(directoryName, ".", path3));
    iniFile.Write(section3, "OutputBasePath1", flag1 ? directoryName : ".\\");
    iniFile.Write(section3, "OutputFileName1", flag1 ? "" : Path.GetFileName(authenticFilePath));
    if (flag1)
    {
      iniFile.Write(section3, "OutputFileNameMulti1", Path.GetFileName(authenticFilePath));
      iniFile.Write(section3, "UseOutputNameForMulti1", "0");
      iniFile.Write(section3, "ReleaseManaged1", "0");
      iniFile.Write(section3, "OutputPathOutputer1", string.Empty);
      string section4 = "GeneratedFilesSettings";
      iniFile.Write(section4, "RelativeOutputPath1", directoryName);
    }
    projectIntf.DM_AddSourceDocument(str2);
    this.OpenObject(str2);
    if (flag1)
      this.RunWorkspaceManagerCommand(WorkspaceCommand.GenerateReport, (IDictionary<string, string>) new Dictionary<string, string>()
      {
        {
          "Action",
          "Run"
        },
        {
          "ObjectKind",
          "OutputBatch"
        }
      });
    else
      this.RunWorkspaceManagerCommand(WorkspaceCommand.Print, (IDictionary<string, string>) new Dictionary<string, string>()
      {
        {
          "Action",
          "PublishToPDF"
        },
        {
          "ObjectKind",
          "OutputBatch"
        },
        {
          "DisableDialog",
          "True"
        },
        {
          "OpenOutput",
          "False"
        },
        {
          "OutputFilePath",
          directoryName
        },
        {
          "OutputFileName",
          Path.GetFileName(authenticFilePath)
        }
      });
    this.CloseObject(str2);
    projectIntf.DM_RemoveSourceDocument(str2);
    File.Delete(str2);
    if (fromFileName == ADDocumentType.Project)
    {
      Helper.Workspace.DM_CloseProject(projectIntf.DM_ProjectFileName());
    }
    else
    {
      foreach (string fileName4 in stringList3)
        this.CloseObject(fileName4);
      this.CloseObject(fileName);
    }
  }

  public bool Connect() => !string.IsNullOrWhiteSpace(this.GetVersion());

  public void ExecuteModuleCommand(string commandName, long objectId)
  {
    IPSAddInProxy.AddDebugMessage($"{nameof (ExecuteModuleCommand)}({commandName}, {objectId})");
    Task.Run((Action) (() => DXP.Utils.RunCommand($"{Consts.ModuleName}:{commandName}", string.Empty)));
  }

  private static string LogPath
  {
    get
    {
      return Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "IPSAddin.log");
    }
  }

  public static void AddDebugMessage(string message)
  {
    if (!IPSAddInProxy.DebugMode)
      return;
    Helper.Workspace.DM_AddOutputLine(message, false, false);
    File.AppendAllText(IPSAddInProxy.LogPath, message.NewLine());
  }
}
