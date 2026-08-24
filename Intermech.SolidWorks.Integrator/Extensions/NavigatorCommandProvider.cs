// Decompiled with JetBrains decompiler
// Type: Intermech.SolidWorks.Integrator.Extensions.NavigatorCommandProvider
// Assembly: Intermech.SolidWorks.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: C58B767B-0480-4923-A6B5-4C5307770AFD
// Assembly location: D:\IPS\Client\Intermech.SolidWorks.Integrator.dll

using Intermech.CADInterface.Proxies;
using Intermech.DataFormats;
using Intermech.Diagnostics;
using Intermech.Files;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Navigator.ContextMenu;
using Intermech.Navigator.Interfaces;
using Intermech.Runtime;
using Intermech.Tools.Integrators;
using Intermech.Tools.Integrators.CADInterface;
using Intermech.UI;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

#nullable disable
namespace Intermech.SolidWorks.Integrator.Extensions;

internal sealed class NavigatorCommandProvider : ICommandsProvider
{
  private SWIntegratorModule integratorModule;
  private IFileVault fileVaultService;
  private IOutputView outputViewService;

  public NavigatorCommandProvider(
    SWIntegratorModule integratorModule,
    IFileVault fileVaultService,
    IOutputView outputViewService)
  {
    if (integratorModule == null)
      throw new ArgumentNullException(nameof (integratorModule));
    if (fileVaultService == null)
      throw new ArgumentNullException(nameof (fileVaultService));
    if (outputViewService == null)
      throw new ArgumentNullException(nameof (outputViewService));
    this.integratorModule = integratorModule;
    this.fileVaultService = fileVaultService;
    this.outputViewService = outputViewService;
  }

  public CommandsInfo GetMergedCommands(ISelectedItems items, IServiceProvider viewServices)
  {
    if (items == null)
      throw new ArgumentNullException(nameof (items));
    if (viewServices == null)
      throw new ArgumentNullException(nameof (viewServices));
    return CommandsInfo.Empty;
  }

  public CommandsInfo GetGroupCommands(ISelectedItems items, IServiceProvider viewServices)
  {
    if (items == null)
      throw new ArgumentNullException(nameof (items));
    if (viewServices == null)
      throw new ArgumentNullException(nameof (viewServices));
    if (items.Count != 0)
    {
      CADSettings integratorSettings = this.TryGetIntegratorSettings();
      if (integratorSettings != null)
      {
        bool flag = true;
        for (int index = 0; index < items.Count; ++index)
        {
          INodeID itemId = items.GetItemID(index);
          if (integratorSettings.FileDocumentGroups.FindByDocumentType(itemId.TypeID, false) == null)
          {
            flag = false;
            break;
          }
          if (((IDBObjectID) items.GetItemData(index, typeof (IDBObjectID))).Value >= 0L)
          {
            flag = false;
            break;
          }
        }
        if (flag)
        {
          CommandsInfo groupCommands = new CommandsInfo();
          groupCommands.Add(NavigatorCommandConsts.RepairFileReferencesCommandName, new CommandInfo(0, new ClickEventHandler(this.OnRepairFileReferencesCommand)));
          return groupCommands;
        }
      }
    }
    return CommandsInfo.Empty;
  }

  private CADSettings TryGetIntegratorSettings()
  {
    try
    {
      return ServiceUtils.GetService<ICADSettingsService>((object) this.integratorModule.Integrator, true).GetCADSettings();
    }
    catch (Exception ex)
    {
      string currentMethodName = this.GetCurrentMethodName(nameof (TryGetIntegratorSettings));
      SuppressedExceptions.TraceException(ex, currentMethodName);
      return (CADSettings) null;
    }
  }

  private void OnRepairFileReferencesCommand(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    VersionsRulePackage editorRule = VersionsRuleSources.GetEditorRule();
    using (CADApiSession cadApiSession = new CADApiSession((IApplicationApiService) ServiceUtils.GetService<ICADInterfaceService>((object) this.integratorModule.Integrator, true)))
    {
      CADSystemProxy cadSystem = cadApiSession.Application;
      if (cadSystem.GetOpenFiles(true).Count != 0)
      {
        int num1 = (int) MessageBox.Show("Для корректного исправления файловых ссылок требуется, чтобы в CAD-системе не было открытых документов. Закройте все документы и повторите попытку.", NavigatorCommandConsts.RepairFileReferencesDisplayName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        RepairFileReferencesAction repairAction = new RepairFileReferencesAction(this.fileVaultService);
        List<ErrorInfo> errors = new List<ErrorInfo>();
        ProgressSinks.DialogService.Invoke(NavigatorCommandConsts.RepairFileReferencesDisplayName, ProgressSinkDialogFlags.Default, (Action<IPercentageProgressSink>) (progressSink =>
        {
          IProgressUpdater progressUpdater = ProgressSinks.CreateProgressUpdater(progressSink, items.Count);
          for (int index = 0; index < items.Count; ++index)
          {
            IDBObjectID itemData = (IDBObjectID) items.GetItemData(index, typeof (IDBObjectID));
            progressSink.SetState(itemData.Caption);
            try
            {
              repairAction.RepairMovedFileReferences(itemData.Value, editorRule, cadSystem);
            }
            catch (Exception ex)
            {
              string message = $"Не удалось обработать документ \"{itemData.Caption}\" (ид. версии = {itemData.Value}).";
              errors.Add(ErrorInfo.FromException(ex, message));
            }
            progressUpdater.AddCompletedTasks(1);
            if (progressSink.IsCancelled)
              break;
          }
        }));
        if (errors.Count == 0)
          return;
        new ErrorReporterAdapter((IMessageReporter) new MultilineMessageReporter((IMessageReporter) new OutputViewMessageReporter(this.outputViewService, "Вывод")))
        {
          CaptionGenerator = ((Func<ICollection<ErrorInfo>, string>) (errorList => $"Отчет о выполнении команды \"{NavigatorCommandConsts.RepairFileReferencesDisplayName}\""))
        }.ReportErrors((ICollection<ErrorInfo>) errors);
        int num2 = (int) MessageBox.Show("При выполнении команды возникли ошибки, из-за которых не все выбранные документы были успешно обработаны. Подробные сведения об ошибках можно получить в окне \"Вывод\".", NavigatorCommandConsts.RepairFileReferencesDisplayName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }
  }
}
