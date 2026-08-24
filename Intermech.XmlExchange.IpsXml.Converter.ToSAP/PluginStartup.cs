// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Converter.ToSAP.PluginStartup
// Assembly: Intermech.XmlExchange.IpsXml.Converter.ToSAP, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 946972C6-4ABC-4C4A-94A5-3ADC51FD9A58
// Assembly location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Converter.ToSAP.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Plugins;
using Intermech.Interfaces.Server;
using Intermech.Interfaces.XmlExchange;
using Intermech.XmlExchange.IpsXml.Converter.ToSAP.XmlExtention;
using System;

#nullable disable
namespace Intermech.XmlExchange.IpsXml.Converter.ToSAP;

public class PluginStartup : IPackage
{
  private IPluginManager _manager;
  private IXmlExchangeExtension _xmlExtension;

  public void Unload()
  {
    if (this._manager != null)
      this._manager.LoadComplete -= new EventHandler(this._manager_LoadComplete);
    this._manager_Unload();
  }

  public string Name => "Конвертор экспорта XML в формат SAP";

  public void Load(IServiceProvider serviceProvider)
  {
    this._manager = serviceProvider.GetService(typeof (IPluginManager)) as IPluginManager;
    if (this._manager == null)
      return;
    this._manager.LoadComplete += new EventHandler(this._manager_LoadComplete);
  }

  private void _manager_LoadComplete(object sender, EventArgs e)
  {
    IXmlExchangeService service = ServiceUtils.GetService<IXmlExchangeService>((object) ServerServices.ServiceContainer, true);
    this._xmlExtension = (IXmlExchangeExtension) new XmlConvertor2SAPExtention();
    IXmlExchangeExtension xmlExtension = this._xmlExtension;
    service.RegisterExtension(xmlExtension);
  }

  private void _manager_Unload()
  {
  }
}
