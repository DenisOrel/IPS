// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Client.SignUpContextMenuProvider
// Assembly: Intermech.Signs, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: A3C02709-D794-49CE-8C55-5624449406B7
// Assembly location: D:\IPS\Client\Intermech.Signs.dll
// XML documentation location: D:\IPS\Client\Intermech.Signs.xml

using Intermech.DataFormats;
using Intermech.Interfaces;
using Intermech.Navigator.ContextMenu;
using Intermech.Navigator.Controls;
using Intermech.Navigator.Interfaces;
using Intermech.Signs.Interfaces;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.Signs.Client;

/// <summary>Провайдер подписей на объекты</summary>
internal class SignUpContextMenuProvider : ICommandsProvider
{
  public CommandsInfo GetMergedCommands(ISelectedItems items, IServiceProvider viewServices)
  {
    return CommandsInfo.Empty;
  }

  public CommandsInfo GetGroupCommands(ISelectedItems items, IServiceProvider viewServices)
  {
    if (((viewServices.GetService(typeof (IViewState)) is IViewState service ? (long) service.ViewState : 0L) & 2L) != 0L)
      return CommandsInfo.Empty;
    List<IDBTypedObjectID> typedObjectIDs = new List<IDBTypedObjectID>(items.Count);
    CommandsInfo groupCommands = new CommandsInfo();
    bool flag1 = true;
    bool flag2 = false;
    for (int index = 0; index < items.Count; ++index)
    {
      if (items.GetItemData(index, typeof (IDBTypedObjectID)) is IDBTypedObjectID itemData && itemData.ObjectID < 0L)
        flag2 = true;
      if (itemData != null && MetaDataHelper.HasApplicability(itemData.ObjectType, SignsHolder.SignObjectTypeID, SignsHolder.SignRelationTypeID))
      {
        if (items.Count == 1)
          groupCommands.Add("SignView", new CommandInfo(4, new ClickEventHandler(SignUpContextMenuProvider.SignsView)));
        typedObjectIDs.Add(itemData);
      }
      else
        flag1 = false;
    }
    if (flag1 && !flag2)
      groupCommands.Add("SignAs", new CommandInfo(4, new ClickEventHandler(SignUpContextMenuProvider.SignAsCommand)));
    List<string> graphs = SignsCache.UserSignsCard.GetGraphs(typedObjectIDs);
    if (!flag2)
    {
      foreach (string key in graphs)
      {
        if (SignsCache.PossibleGraphs.ContainsKey(key))
        {
          groupCommands.Add("SignUp", new CommandInfo(4, new ClickEventHandler(SignUpContextMenuProvider.SignUpCommand)));
          groupCommands.Add("CryptoSignUp", new CommandInfo(4, new ClickEventHandler(SignUpContextMenuProvider.CryptoSignUp)));
          break;
        }
      }
    }
    return groupCommands;
  }

  /// <summary>Команда "Показать подписи"</summary>
  public static void SignsView(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    if (!(items.GetItemData(0, typeof (IDBTypedObjectID)) is IDBTypedObjectID itemData))
      return;
    int num = (int) PropertiesWindow.Execute(string.Empty, string.Empty, itemData.ObjectID, nameof (SignsView));
  }

  /// <summary>Подписать объект</summary>
  /// <param name="items"></param>
  /// <param name="viewServices"></param>
  /// <param name="additionalInfo"></param>
  public static void SignUpCommand(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    SignsCommands.SignUpCommand(SignUpContextMenuProvider.GetObjectsList(items));
  }

  /// <summary>Подписать объект от имени</summary>
  /// <param name="items"></param>
  /// <param name="viewServices"></param>
  /// <param name="additionalInfo"></param>
  public static void SignAsCommand(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    SignsCommands.SignAsCommand(SignUpContextMenuProvider.GetObjectsList(items));
  }

  /// <summary>Подставить ЭЦП подпись</summary>
  /// <param name="items"></param>
  /// <param name="viewServices"></param>
  /// <param name="additionalInfo"></param>
  public static void CryptoSignUp(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    SignsCommands.CryptoSignUp(SignUpContextMenuProvider.GetObjectsList(items));
  }

  /// <summary>
  /// 
  /// </summary>
  /// <param name="items"></param>
  /// <returns></returns>
  private static List<IDBTypedObjectID> GetObjectsList(ISelectedItems items)
  {
    List<IDBTypedObjectID> objectsList = new List<IDBTypedObjectID>(items.Count);
    for (int index = 0; index < items.Count; ++index)
    {
      if (items.GetItemData(index, typeof (IDBTypedObjectID)) is IDBTypedObjectID itemData)
        objectsList.Add(itemData);
    }
    return objectsList;
  }
}
