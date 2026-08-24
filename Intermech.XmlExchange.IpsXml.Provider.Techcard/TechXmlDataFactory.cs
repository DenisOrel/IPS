// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Provider.Techcard.TechXmlDataFactory
// Assembly: Intermech.XmlExchange.IpsXml.Provider.Techcard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 6433FBE8-382D-4C90-9782-A3F865DC9A28
// Assembly location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Provider.Techcard.dll

using Intermech.XmlExchange.IpsXml.Interfaces;

#nullable disable
namespace Intermech.XmlExchange.IpsXml.Provider.Techcard;

public sealed class TechXmlDataFactory : IXmlDataFactory
{
  public IXmlDataProvider GetDataProvider(params string[] files)
  {
    TechXmlDataProvider dataProvider = new TechXmlDataProvider();
    if (files.Length != 0)
      dataProvider.Load(files[0]);
    return (IXmlDataProvider) dataProvider;
  }
}
