// Decompiled with JetBrains decompiler
// Type: Intermech.Interfaces.FSS.IXMLStorageLoadSave
// Assembly: Intermech.Interfaces.FSS, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 89C13FA3-8295-4BAF-985C-14C35172BA6B
// Assembly location: D:\IPS\Client\Intermech.Interfaces.FSS.dll
// XML documentation location: D:\IPS\Client\Intermech.Interfaces.FSS.xml

using System.Xml;

#nullable disable
namespace Intermech.Interfaces.FSS;

/// <summary>
/// Интерфейс, позволяющий выполнять сохранение и загрузку данных в хранилище XML
/// </summary>
public interface IXMLStorageLoadSave
{
  /// <summary>Загрузить данные из указанного узла настроек</summary>
  /// <param name="xmlStorage">Хранилище настроек</param>
  /// <param name="node">Узел с данными</param>
  void Load(XMLSettingsStorage xmlStorage, XmlNode node);

  /// <summary>
  /// Сохранить данные в состав указанного родительского узла
  /// </summary>
  /// <param name="xmlStorage">Хранилище настроек</param>
  /// <param name="parentNode">Родительский узел или null (тогда сохранение можно выполнять в корневой узел)</param>
  void Save(XMLSettingsStorage xmlStorage, XmlNode parentNode);
}
