// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Interfaces.OnVisitObject
// Assembly: Intermech.XmlExchange.IpsXml.Interfaces, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 741EAC98-7C4B-42E4-B0B7-F40794536EF7
// Assembly location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Interfaces.dll
// XML documentation location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Interfaces.xml

#nullable disable
namespace Intermech.XmlExchange.IpsXml.Interfaces;

/// <summary>Событие посещения объекта.</summary>
/// <param name="parentObj">Родительский объект по связи.</param>
/// <param name="obj">Посещаемый объект.</param>
/// <param name="rel">Связь между родительским и дочерним объектами.</param>
/// <param name="stopTraversing">Прекратить обход.false - по умолчанию.</param>
/// <remarks>parentObj и rel могут быть = null в случае если посещается головной объект.</remarks>
public delegate void OnVisitObject(
  IXmlObject parentObj,
  IXmlObject obj,
  IXmlRelation rel,
  ref bool stopTraversing);
