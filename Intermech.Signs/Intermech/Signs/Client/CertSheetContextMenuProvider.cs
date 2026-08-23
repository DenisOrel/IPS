// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Client.CertSheetContextMenuProvider
// Assembly: Intermech.Signs, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: A3C02709-D794-49CE-8C55-5624449406B7
// Assembly location: D:\IPS\Client\Intermech.Signs.dll
// XML documentation location: D:\IPS\Client\Intermech.Signs.xml

using Intermech.Controls;
using Intermech.DataFormats;
using Intermech.Document.Client;
using Intermech.Document.Model;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Document;
using Intermech.Localization;
using Intermech.Navigator.ContextMenu;
using Intermech.Navigator.Interfaces;
using Intermech.Signs.Interfaces;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Signs.Client;

public class CertSheetContextMenuProvider : ICommandsProvider
{
  public CommandsInfo GetMergedCommands(ISelectedItems items, IServiceProvider viewServices)
  {
    return CommandsInfo.Empty;
  }

  public CommandsInfo GetGroupCommands(ISelectedItems items, IServiceProvider viewServices)
  {
    CommandsInfo groupCommands = CommandsInfo.Empty;
    if (items != null)
    {
      for (int index1 = 0; index1 < items.Count; ++index1)
      {
        if (!(items.GetItemData(index1, typeof (IDBTypedObjectID)) is IDBTypedObjectID itemData))
          return groupCommands;
        List<int> parentsIdReverse = MetaDataHelper.GetObjectTypeParentsIDReverse(itemData.ObjectType);
        bool flag = false;
        for (int index2 = 0; index2 < parentsIdReverse.Count; ++index2)
        {
          flag = parentsIdReverse[index2] == SignsHolder.DocumentObjectTypeID;
          if (flag)
            break;
        }
        if (!flag)
          return groupCommands;
      }
      groupCommands = new CommandsInfo();
      groupCommands.Add("CreateCertSheet", new CommandInfo(2, new ClickEventHandler(CertSheetContextMenuProvider.CreateCertSheetCommand)));
    }
    return groupCommands;
  }

  /// <summary>Команда Сформировать УЛ</summary>
  /// <param name="items"></param>
  /// <param name="viewServices"></param>
  /// <param name="additionalInfo"></param>
  public static void CreateCertSheetCommand(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    List<long> objectsIdList = CertSheetContextMenuProvider.GetObjectsIDList(items);
    bool flag1 = true;
    while (flag1)
    {
      ExpiredAuthFileUsing expiredAuthFileUsingMode = ExpiredAuthFileUsing.None;
      List<ImDocument> certSheets = ServiceUtils.GetService<ICertSheetClientService>((object) ServicesManager.ServiceContainer, true).CreateCertSheets(objectsIdList, false, ref expiredAuthFileUsingMode);
      bool flag2 = false;
      if (certSheets != null)
      {
        for (int index = 0; index < certSheets.Count; ++index)
        {
          if (certSheets[index] != null)
          {
            certSheets[index].UpdateLayout(0, true, true, true, true);
            DocumentEditorPlugin.Instance.OpenDocument((DocumentTreeNode) certSheets[index], true, true);
            flag2 = true;
          }
        }
        if (!flag2)
        {
          if (IMMessageBox.ShowEx(MessageDialogs.msgInformation, LocalizationHolder.rm.GetString("CertSheetNotPossibleToCreate"), new IMMessageBoxButton[2]
          {
            new IMMessageBoxButton("OK", DialogResult.OK, (object) DialogResult.OK),
            new IMMessageBoxButton("Повторить", DialogResult.Retry, (object) DialogResult.Retry)
          }, IMMessageBoxImage.Information) is DialogResult.Retry)
            continue;
        }
      }
      flag1 = false;
    }
  }

  /// <summary>
  /// 
  /// </summary>
  /// <param name="items"></param>
  /// <returns></returns>
  private static List<long> GetObjectsIDList(ISelectedItems items)
  {
    List<long> objectsIdList = new List<long>();
    for (int index = 0; index < items.Count; ++index)
    {
      IDBTypedObjectID itemData = items.GetItemData(index, typeof (IDBTypedObjectID)) as IDBTypedObjectID;
      objectsIdList.Add(itemData.ObjectID);
    }
    return objectsIdList;
  }
}
