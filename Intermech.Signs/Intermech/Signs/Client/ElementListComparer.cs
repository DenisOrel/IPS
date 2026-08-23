// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Client.ElementListComparer
// Assembly: Intermech.Signs, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: A3C02709-D794-49CE-8C55-5624449406B7
// Assembly location: D:\IPS\IPS.Installer.Full\IPS.InstClient\Client\Intermech.Signs.dll

using System.Collections.Generic;

#nullable disable
namespace Intermech.Signs.Client;

internal class ElementListComparer : IComparer<CertSheetTableElement>
{
  private CertSheetGraphSortMethod method;

  public ElementListComparer(CertSheetGraphSortMethod lMethod) => this.method = lMethod;

  public int Compare(CertSheetTableElement x, CertSheetTableElement y)
  {
    int num;
    switch (this.method)
    {
      case CertSheetGraphSortMethod.ByValue:
        num = x.GraphId.CompareTo(y.GraphId);
        break;
      case CertSheetGraphSortMethod.ByDescription:
        num = x.GraphDescription.CompareTo(y.GraphDescription);
        break;
      default:
        return 0;
    }
    if (num == 0)
    {
      if (!x.Empty && y.Empty)
        num = -1;
      else if (x.Empty && !y.Empty)
        num = 1;
    }
    return num;
  }
}
