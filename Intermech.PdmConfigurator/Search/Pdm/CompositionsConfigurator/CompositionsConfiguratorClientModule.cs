// Decompiled with JetBrains decompiler
// Type: Intermech.Search.Pdm.CompositionsConfigurator.CompositionsConfiguratorClientModule
// Assembly: Intermech.PdmConfigurator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: B5CB2E26-657B-4329-B46C-77AE46A32171
// Assembly location: D:\IPS\Client\Intermech.PdmConfigurator.dll

using Intermech.Search.Configuration;

#nullable disable
namespace Intermech.Search.Pdm.CompositionsConfigurator;

public sealed class CompositionsConfiguratorClientModule
{
  private CompositionsConfiguratorModule _compositionsConfiguratorModule = new CompositionsConfiguratorModule();

  public void Load()
  {
    this._compositionsConfiguratorModule.Load();
    ServiceLocator.Get<IConfigurationOptionInfoProvider>().RegisterEditor(CompositionsConfiguratorConfigurationOptionKeys.ApplicationConditionsDisplaySettings, typeof (ApplicationConditionsDisplaySettingsEditor));
    ConfigurationPageHelper.CreateAndRegisterPages();
  }
}
