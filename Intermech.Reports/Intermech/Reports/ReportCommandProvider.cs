// Decompiled with JetBrains decompiler
// Type: Intermech.Reports.ReportCommandProvider
// Assembly: Intermech.Reports, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: A20B4FCB-3CA6-4E39-8837-1BB71F87F99A
// Assembly location: D:\IPS\Client\Intermech.Reports.dll
// XML documentation location: D:\IPS\Client\Intermech.Reports.xml

using Intermech.DataFormats;
using Intermech.Interfaces;
using Intermech.Interfaces.Compositions;
using Intermech.Interfaces.Reports;
using Intermech.Localization;
using Intermech.Navigator.ContextMenu;
using Intermech.Navigator.Interfaces;
using Intermech.Reports.Commands;
using Intermech.Reports.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Intermech.Reports;

/// <summary>
/// 
/// </summary>
internal class ReportCommandProvider : ICommandsProvider
{
  /// <summary>
  /// 
  /// </summary>
  /// <param name="items"></param>
  /// <param name="viewServices"></param>
  /// <param name="additionalInfo"></param>
  private static void GenerateComplectCommand(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    new ComplectGenerateCommand().Execute(items, viewServices, additionalInfo);
  }

  /// <summary>
  /// 
  /// </summary>
  /// <param name="items"></param>
  /// <param name="viewServices"></param>
  /// <param name="additionalInfo"></param>
  private static void UpdateComplectCommand(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    new ComplectUpdateCommand(ReportMode.Update).Execute(items, viewServices, additionalInfo);
  }

  /// <summary>
  /// 
  /// </summary>
  /// <param name="items"></param>
  /// <param name="viewServices"></param>
  /// <param name="additionalInfo"></param>
  private static void CreateComplectVersionCommand(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    new ComplectUpdateCommand(ReportMode.CreateVersion).Execute(items, viewServices, additionalInfo);
  }

  /// <summary>
  /// 
  /// </summary>
  /// <param name="items"></param>
  /// <param name="viewServices"></param>
  /// <param name="additionalInfo"></param>
  private static void GenerateAdditionalComplectCommand(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    ComplectGenerateCommand complectGenerateCommand = new ComplectGenerateCommand();
    complectGenerateCommand.TaskMode = ReportTaskMode.AdditionalComplect;
    complectGenerateCommand.Execute(items, viewServices, additionalInfo);
  }

  /// <summary>
  /// 
  /// </summary>
  /// <param name="items"></param>
  /// <param name="viewServices"></param>
  /// <param name="additionalInfo"></param>
  private static void UpdateAdditionalComplectCommand(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    ComplectUpdateCommand complectUpdateCommand = new ComplectUpdateCommand(ReportMode.Update);
    complectUpdateCommand.TaskMode = ReportTaskMode.AdditionalComplect;
    complectUpdateCommand.Execute(items, viewServices, additionalInfo);
  }

  /// <summary>
  /// 
  /// </summary>
  /// <param name="items"></param>
  /// <param name="viewServices"></param>
  /// <param name="additionalInfo"></param>
  private static void CreateAdditionalComplectVersionCommand(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    ComplectUpdateCommand complectUpdateCommand = new ComplectUpdateCommand(ReportMode.CreateVersion);
    complectUpdateCommand.TaskMode = ReportTaskMode.AdditionalComplect;
    complectUpdateCommand.Execute(items, viewServices, additionalInfo);
  }

  /// <summary>
  /// 
  /// </summary>
  /// <param name="items"></param>
  /// <param name="viewServices"></param>
  /// <param name="additionalInfo"></param>
  private static void PrintComplectCommand(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    new ComplectPrintCommand().Execute(items, viewServices, additionalInfo);
  }

  /// <summary>
  /// 
  /// </summary>
  /// <param name="items"></param>
  /// <param name="viewServices"></param>
  /// <param name="additionalInfo"></param>
  private static void DeleteComplectCommand(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    new ComplectDeleteCommand().Execute(items, viewServices, additionalInfo);
  }

  /// <summary>
  /// 
  /// </summary>
  /// <param name="items"></param>
  /// <param name="viewServices"></param>
  /// <param name="additionalInfo"></param>
  private static void ViewComplectCommand(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    new ComplectShowCommand().Execute(items, viewServices, additionalInfo);
  }

  /// <summary>
  /// 
  /// </summary>
  /// <param name="items"></param>
  /// <param name="viewServices"></param>
  /// <param name="additionalInfo"></param>
  private static void SaveComplectCommand(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    new ComplectSaveCommand().Execute(items, viewServices, additionalInfo);
  }

  /// <summary>Constructor</summary>
  /// <param name="factory"></param>
  public ReportCommandProvider(IFactory factory)
  {
    MenuTemplate contextMenuTemplate = factory.ContextMenuTemplate;
    MenuTemplateNode node = factory.ContextMenuTemplate["Reports"];
    contextMenuTemplate.BeginUpdate();
    try
    {
      if (node == null)
      {
        INamedImageList service = ServiceUtils.GetService<INamedImageList>((object) ApplicationServices.Container, false);
        int imageIndex = service != null ? service.ImageIndex("imgReport") : -1;
        node = new MenuTemplateNode("Reports", LocalizationHolder.rm.GetString("Reports_40"), imageIndex, 40, 30);
        contextMenuTemplate.Nodes.Add(node);
      }
      node.Nodes.Add(new MenuTemplateNode("GenerateReport", LocalizationHolder.rm.GetString("Reports_9"), -1, 10, 20));
      node.Nodes.Add(new MenuTemplateNode("UpdateReport", LocalizationHolder.rm.GetString("Reports_16"), -1, 10, 23));
      node.Nodes.Add(new MenuTemplateNode("CreateReportVersion", LocalizationHolder.rm.GetString("Reports_41"), -1, 10, 26));
      node.Nodes.Add(new MenuTemplateNode("GenerateAdditionalComplect", LocalizationHolder.rm.GetString("Reports_66"), -1, 10, 30));
      node.Nodes.Add(new MenuTemplateNode("UpdateAdditionalComplect", LocalizationHolder.rm.GetString("Reports_67"), -1, 10, 33));
      node.Nodes.Add(new MenuTemplateNode("CreateAdditionalComplectVersion", LocalizationHolder.rm.GetString("Reports_68"), -1, 10, 36));
    }
    finally
    {
      contextMenuTemplate.EndUpdate();
    }
  }

  /// <summary>
  /// 
  /// </summary>
  /// <param name="items"></param>
  /// <param name="viewServices"></param>
  /// <returns></returns>
  public CommandsInfo GetMergedCommands(ISelectedItems items, IServiceProvider viewServices)
  {
    if (items == null || viewServices == null)
      return CommandsInfo.Empty;
    IViewState service = ServiceUtils.GetService<IViewState>((object) viewServices, false);
    ViewStateFlags viewStateFlags = service != null ? service.ViewState : ViewStateFlags.None;
    if ((viewStateFlags & ViewStateFlags.ReadOnly) != ViewStateFlags.None)
      return CommandsInfo.Empty;
    CommandsInfo mergedCommands = new CommandsInfo();
    mergedCommands.Add("GenerateReport", new CommandInfo(4, new ClickEventHandler(ReportCommandProvider.GenerateComplectCommand)));
    mergedCommands.Add("GenerateAdditionalComplect", new CommandInfo(4, new ClickEventHandler(ReportCommandProvider.GenerateAdditionalComplectCommand)));
    IList<ObjInfoItem> objInfoList;
    ReportUtils.GetSelectedItemsInfo(items, out objInfoList, false);
    if ((objInfoList == null ? 0 : (objInfoList.All<ObjInfoItem>((Func<ObjInfoItem, bool>) (item => item.ObjTypeID == -1 || MetaDataHelper.IsObjectTypeChildOf(item.ObjTypeID, ReportsConsts.DocPackageBaseTypeID))) ? 1 : 0)) != 0)
    {
      if ((viewStateFlags & ViewStateFlags.ReadOnly) == ViewStateFlags.None)
        mergedCommands.Add("Delete", new CommandInfo(4, new ClickEventHandler(ReportCommandProvider.DeleteComplectCommand)));
      if (items.Count == 1)
      {
        if (items.GetParentData(0, typeof (IDBObjectID)) is IDBObjectID)
        {
          mergedCommands.Add("UpdateReport", new CommandInfo(4, new ClickEventHandler(ReportCommandProvider.UpdateComplectCommand)));
          mergedCommands.Add("UpdateAdditionalComplect", new CommandInfo(4, new ClickEventHandler(ReportCommandProvider.UpdateAdditionalComplectCommand)));
        }
        mergedCommands.Add("CreateReportVersion", new CommandInfo(4, new ClickEventHandler(ReportCommandProvider.CreateComplectVersionCommand)));
        mergedCommands.Add("CreateAdditionalComplectVersion", new CommandInfo(4, new ClickEventHandler(ReportCommandProvider.CreateAdditionalComplectVersionCommand)));
        mergedCommands.Add("ViewDocument", new CommandInfo(3, new ClickEventHandler(ReportCommandProvider.ViewComplectCommand)));
        mergedCommands.Add("PrintDocument", new CommandInfo(3, new ClickEventHandler(ReportCommandProvider.PrintComplectCommand)));
        mergedCommands.Add("SaveToDisk", new CommandInfo(3, new ClickEventHandler(ReportCommandProvider.SaveComplectCommand)));
        mergedCommands.Suppress("EditDocument", 0);
        return mergedCommands;
      }
    }
    return mergedCommands;
  }

  /// <summary>
  /// 
  /// </summary>
  /// <param name="items"></param>
  /// <param name="viewServices"></param>
  /// <returns></returns>
  public CommandsInfo GetGroupCommands(ISelectedItems items, IServiceProvider viewServices)
  {
    return CommandsInfo.Empty;
  }
}
