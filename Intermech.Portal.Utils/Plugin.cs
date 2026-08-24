// Decompiled with JetBrains decompiler
// Type: Intermech.Portal.Utils.Plugin
// Assembly: Intermech.Portal.Utils, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 99780CCF-14B7-482E-A297-41CC169803AE
// Assembly location: D:\IPS\Client\Intermech.Portal.Utils.dll

using Intermech.Bars;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Plugins;
using Intermech.Search;
using System;

#nullable disable
namespace Intermech.Portal.Utils;

public class Plugin : IPackage
{
  private string menuGroupName = "Portal.Utils";

  public void Load(IServiceProvider serviceProvider)
  {
    if (!(ServicesManager.GetService(typeof (IMainMenuService)) is IMainMenuService service))
      return;
    MenuButtonItem menuButtonItem1 = new MenuButtonItem("Утилиты портала", (EventHandler) null);
    menuButtonItem1.CommandName = this.menuGroupName;
    MenuButtonItem menuButtonItem2 = new MenuButtonItem("Корректировка данных файла опубликованных связей");
    menuButtonItem2.AutoToggle = AutoToggleType.Single;
    menuButtonItem2.BeginGroup = false;
    menuButtonItem2.Click += new EventHandler(this.menuCorrectRelationsClick);
    menuButtonItem2.CommandName = "Portal.Utils.RelationsFileCorrect";
    menuButtonItem1.Items.Add((ToolbarItemBase) menuButtonItem2);
    MenuButtonItem[] menuButtonItemArray = new MenuButtonItem[1]
    {
      menuButtonItem1
    };
    service.RegisterMenuItemsGroup(MainMenuItemSite.AdministratorUtilities, MainMenuItemPosition.Default, false, menuButtonItemArray);
  }

  public void Unload()
  {
  }

  private void menuCorrectRelationsClick(object sender, EventArgs e)
  {
    using (CorrectRelationsForm correctRelationsForm = new CorrectRelationsForm())
    {
      int num = (int) correctRelationsForm.ShowDialog();
    }
  }

  public string Name => "Корректировка опубликованных данных";
}
