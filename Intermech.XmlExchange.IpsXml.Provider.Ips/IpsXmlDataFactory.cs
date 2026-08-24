// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Provider.Ips.IpsXmlDataFactory
// Assembly: Intermech.XmlExchange.IpsXml.Provider.Ips, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: B2EE3099-B947-440E-865D-611E406056AB
// Assembly location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Provider.Ips.dll

using Intermech.XmlExchange.IpsXml.Interfaces;

#nullable disable
namespace Intermech.XmlExchange.IpsXml.Provider.Ips;

public sealed class IpsXmlDataFactory : IXmlDataFactory
{
  public IXmlDataProvider GetDataProvider(params string[] files)
  {
    return (IXmlDataProvider) this.GetIpsXMlDataProvider(files);
  }

  public IpsXmlDataProvider GetIpsXMlDataProvider(params string[] files)
  {
    IpsXmlDataProvider ipsXmlDataProvider = new IpsXmlDataProvider();
    ipsXmlDataProvider.Load(files);
    return ipsXmlDataProvider;
  }
}
