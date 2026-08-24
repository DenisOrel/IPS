// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Search.CommonParametersHandler
// Assembly: Intermech.ImpExp.Search, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DCC7C774-0788-47B1-BD86-E2BCE31689FD
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Search.dll

using Intermech.ImpExp.Interface.CommonData;
using Intermech.ImpExp.Interface.CommonData.SettingsItems;
using Intermech.ImpExp.Search.ItemFactories;
using Intermech.Interfaces.Client;

#nullable disable
namespace Intermech.ImpExp.Search;

internal abstract class CommonParametersHandler
{
  protected PumpCommonParameters pumper;

  protected abstract string GroupName { get; }

  protected abstract SettingsGroupType SettingsGroupType { get; }

  public CommonParametersHandler(PumpCommonParameters pumper) => this.pumper = pumper;

  public void CreateSettingsGroup()
  {
    SettingsGroup settingsGroup = new SettingsGroup(this.GroupName, this.SettingsGroupType);
    if (ServicesManager.ServiceContainer.GetService(typeof (ISettingsGroupService)) is ISettingsGroupService service)
      service.Groups.Add((ISettingsGroup) settingsGroup);
    settingsGroup.ObjectCreated += new ObjectCreatedEventHandler(this.Group_ObjectCreated);
  }

  public abstract TypeAttributeItem ReadAttributes();

  private void Group_ObjectCreated()
  {
  }
}
