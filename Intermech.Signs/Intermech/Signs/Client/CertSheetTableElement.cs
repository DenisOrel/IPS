// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Client.CertSheetTableElement
// Assembly: Intermech.Signs, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: A3C02709-D794-49CE-8C55-5624449406B7
// Assembly location: D:\IPS\Client\Intermech.Signs.dll
// XML documentation location: D:\IPS\Client\Intermech.Signs.xml

using Intermech.Document.Model;

#nullable disable
namespace Intermech.Signs.Client;

internal class CertSheetTableElement
{
  public string GraphId { get; set; }

  public string GraphDescription { get; set; }

  public TableElement TableElement { get; set; }

  public bool Empty { get; set; }

  public CertSheetTableElement(string id, string descr, TableElement tableElement)
    : this(id, descr, tableElement, false)
  {
  }

  public CertSheetTableElement(string id, string descr, TableElement tableElement, bool empty)
  {
    this.GraphId = id;
    this.GraphDescription = descr;
    this.TableElement = tableElement;
    this.Empty = empty;
  }
}
