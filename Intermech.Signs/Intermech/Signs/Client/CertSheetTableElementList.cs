// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Client.CertSheetTableElementList
// Assembly: Intermech.Signs, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: A3C02709-D794-49CE-8C55-5624449406B7
// Assembly location: D:\IPS\Client\Intermech.Signs.dll
// XML documentation location: D:\IPS\Client\Intermech.Signs.xml

using System.Collections.Generic;

#nullable disable
namespace Intermech.Signs.Client;

internal class CertSheetTableElementList : List<CertSheetTableElement>
{
  private CertSheetGraphSortMethod certSheetGraphSortMethod;

  public CertSheetTableElementList()
    : this(CertSheetGraphSortMethod.ByDefault)
  {
  }

  public CertSheetTableElementList(CertSheetGraphSortMethod lCertSheetGraphSortMethod)
  {
    this.certSheetGraphSortMethod = lCertSheetGraphSortMethod;
  }

  /// <summary>Сортировка элементов списка</summary>
  public void SortItems()
  {
    if (this.certSheetGraphSortMethod == CertSheetGraphSortMethod.ByDefault)
      return;
    this.Sort((IComparer<CertSheetTableElement>) new ElementListComparer(this.certSheetGraphSortMethod));
  }
}
