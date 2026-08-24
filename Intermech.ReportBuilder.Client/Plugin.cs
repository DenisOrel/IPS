// Decompiled with JetBrains decompiler
// Type: Intermech.ReportBuilder.Client.Plugin
// Assembly: Intermech.ReportBuilder.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 84A30C1D-3856-44D0-92A6-A87D49736592
// Assembly location: D:\IPS\Client\Intermech.ReportBuilder.Client.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Plugins;
using Intermech.Navigator.ContextMenu;
using Intermech.Navigator.Interfaces;
using System;
using System.Collections.Generic;
using System.Reflection;

#nullable disable
namespace Intermech.ReportBuilder.Client;

public class Plugin : IPackage, ICommandsProvider
{
  private static int _menuGroupID = 8759398;
  private List<IMenuScript> _scripts;

  public void Load(IServiceProvider serviceProvider)
  {
    IOutputView service1 = (IOutputView) serviceProvider.GetService(typeof (IOutputView));
    this._scripts = new List<IMenuScript>();
    foreach (Type type in Assembly.GetExecutingAssembly().GetTypes())
    {
      if (type.IsClass && !type.IsAbstract)
      {
        if (typeof (IMenuScript).IsAssignableFrom(type))
        {
          try
          {
            this._scripts.Add((IMenuScript) Activator.CreateInstance(type));
          }
          catch (Exception ex)
          {
            service1.WriteString("Вывод", $"Ошибка регистрации {type.FullName}: {ex.Message}");
          }
        }
      }
    }
    if (this._scripts.Count == 0)
      return;
    IFactory service2 = (IFactory) serviceProvider.GetService(typeof (IFactory));
    if (service2 == null)
      return;
    service2.AddCommandsProvider(1, (ICommandsProvider) this);
    MenuTemplate contextMenuTemplate = service2.ContextMenuTemplate;
    if (contextMenuTemplate == null)
      return;
    MenuTemplateNode menuTemplateNode = service2.ContextMenuTemplate["Reports"];
    if (menuTemplateNode == null)
      return;
    contextMenuTemplate.BeginUpdate();
    for (int index = 0; index < this._scripts.Count; ++index)
    {
      IMenuScript script = this._scripts[index];
      MenuTemplateNode node = new MenuTemplateNode(script.CommandName, script.CommandText, -1, Plugin._menuGroupID, (index + 1) * 10);
      menuTemplateNode.Nodes.Add(node);
    }
    contextMenuTemplate.EndUpdate();
  }

  public void Unload()
  {
  }

  public string Name => "Плагин для разработки сценариев генерации документов";

  public CommandsInfo GetMergedCommands(ISelectedItems items, IServiceProvider viewServices)
  {
    return new CommandsInfo();
  }

  public CommandsInfo GetGroupCommands(ISelectedItems items, IServiceProvider viewServices)
  {
    CommandsInfo groupCommands = new CommandsInfo();
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      for (int index = 0; index < this._scripts.Count; ++index)
      {
        IMenuScript script = this._scripts[index];
        if (script.Visible(sessionKeeper.Session, items, viewServices))
          groupCommands.Add(script.CommandName, new CommandInfo(4, script.Target));
      }
    }
    return groupCommands;
  }
}
