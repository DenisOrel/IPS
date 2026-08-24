// Decompiled with JetBrains decompiler
// Type: Intermech.ProEngineer.Integrator.StripVersionNumbersModule
// Assembly: Intermech.ProEngineer.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 19987673-5EB5-4BB3-AE60-6A96614A14F3
// Assembly location: D:\IPS\Client\Intermech.ProEngineer.Integrator.dll

using Intermech.ApplicationModel;
using Intermech.Bars;
using Intermech.Files;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Mvp;
using Intermech.ProEngineer.Integrator.Properties;
using Intermech.Search;
using Intermech.Settings;
using Intermech.Tools.EnterpriseArchive;
using Intermech.Tools.Integrators;
using System;
using System.Drawing;

#nullable disable
namespace Intermech.ProEngineer.Integrator;

internal sealed class StripVersionNumbersModule : InitializerModule
{
  private MenuButtonItem button;

  protected override void DoInitialize()
  {
    base.DoInitialize();
    if (!(ServicesManager.GetService(typeof (IMainMenuService)) is IMainMenuService service))
      return;
    this.button = new MenuButtonItem(string.Format(Localization.rm.GetString("ProEngineer.Integrator_4"), (object) PEConsts.AppName));
    this.button.BeginGroup = true;
    this.button.Image = (Image) Resources.pe16;
    this.button.CommandName = "StripVersionNumbers";
    this.button.Click += new EventHandler(StripVersionNumbersModule.StripVersionNumbers);
    MenuButtonItem[] menuButtonItemArray = new MenuButtonItem[1]
    {
      this.button
    };
    service.RegisterMenuItems(MainMenuItemSite.TuningMiddle, MainMenuItemPosition.Penultimate, menuButtonItemArray);
  }

  protected override void DoShutdown()
  {
    base.DoShutdown();
    if (this.button == null)
      return;
    this.button.Dispose();
    this.button = (MenuButtonItem) null;
  }

  private static void StripVersionNumbers(object sender, EventArgs e)
  {
    IPackAndGoService service = IntegratorServices.GetService<IPackAndGoService>(new IntegratorObject(PEConsts.PEIntegratorId, PEConsts.IntegratorName), false);
    if (service == null)
      return;
    string strip = StripVersionNumbersModule.SelectDirectoryToStrip();
    service.AdaptDocumentCopy(strip, true);
  }

  private static string SelectDirectoryToStrip()
  {
    StripVersionNumbersTargetPresenter numbersTargetPresenter = new StripVersionNumbersTargetPresenter();
    MvpContext.ViewService.ShowModal((IPresenter) numbersTargetPresenter);
    switch (numbersTargetPresenter.GetSelectedTarget())
    {
      case StripVersionNumbersTarget.Workspace:
        return ServiceUtils.GetService<IFileVault>((object) ServicesManager.ServiceContainer, true).WorkArea.AreaPath;
      case StripVersionNumbersTarget.EnterpriseArchive:
        return (string) (ValueCell<string>) ArchiveParameters.Common.Location;
      default:
        throw new NotImplementedException();
    }
  }
}
