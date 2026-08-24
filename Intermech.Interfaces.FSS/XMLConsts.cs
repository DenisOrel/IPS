// Decompiled with JetBrains decompiler
// Type: Intermech.Interfaces.FSS.XMLConsts
// Assembly: Intermech.Interfaces.FSS, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 89C13FA3-8295-4BAF-985C-14C35172BA6B
// Assembly location: D:\IPS\Client\Intermech.Interfaces.FSS.dll
// XML documentation location: D:\IPS\Client\Intermech.Interfaces.FSS.xml

#nullable disable
namespace Intermech.Interfaces.FSS;

/// <summary>
/// Класс хранит константы с названиями и Guid-ами узлов настроек в XML документе
/// </summary>
public static class XMLConsts
{
  /// <summary>Стандартный заголовок без указания кодировки</summary>
  public const string xmlHeader = "<?xml version='1.0' ?>";
  /// <summary>Стандартный заголовок с кодировкой UTF8</summary>
  public const string xmlHeaderUTF8 = "<?xml version='1.0' encoding='utf-8' ?>";
  /// <summary>
  /// Шаблон пустого документа с корректным корневым узлом "IPS.FSS.V1"
  /// </summary>
  public const string xmlEmptyDoc = "<?xml version='1.0' encoding='utf-8' ?>\n<IPS.FSS.V1 />\n";
  /// <summary>Корневой узел - "IPS.FSS.V1"</summary>
  public const string xmlRootNode = "IPS.FSS.V1";
  /// <summary>Узел с настройками ремутинга TCP/IP - "Remoting.TCP"</summary>
  public const string xmlRemotingTCP = "Remoting.TCP";
  /// <summary>
  /// Узел с настройками ремутинга TCP/IP - "Server.Remoting.TCP"
  /// </summary>
  public const string xmlServerRemotingTCP = "Server.Remoting.TCP";
  /// <summary>
  /// Узел с адресом, на котором выполняется прослушивание ремутинга - "Address"
  /// </summary>
  public const string xmlAddress = "Address";
  /// <summary>
  /// Узел с номером порта, на котором надо выполнять прослушивание - "Port"
  /// </summary>
  public const string xmlPort = "Port";
}
