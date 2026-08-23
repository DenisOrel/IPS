// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Client.CertSheetGraphSortConverter
// Assembly: Intermech.Signs, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: A3C02709-D794-49CE-8C55-5624449406B7
// Assembly location: D:\IPS\IPS.Installer.Full\IPS.InstClient\Client\Intermech.Signs.dll

using Intermech.PropertyEditors;
using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;

#nullable disable
namespace Intermech.Signs.Client;

internal class CertSheetGraphSortConverter : DropDownTypeConverter
{
  public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
  {
    return sourceType == typeof (string) || base.CanConvertFrom(context, sourceType);
  }

  public override object ConvertFrom(
    ITypeDescriptorContext context,
    CultureInfo culture,
    object value)
  {
    return value.GetType() == typeof (string) ? (object) new CertSheetGraphSortClass(CertSheetGrapSorthHelper.GetCertSheetGraphSortMethod((string) value)) : base.ConvertFrom(context, culture, value);
  }

  public override ArrayList GetStandardValuesCustomList(
    ITypeDescriptorContext context,
    params object[] args)
  {
    ArrayList valuesCustomList = new ArrayList((ICollection) Enum.GetValues(typeof (CertSheetGraphSortMethod)));
    for (int index = 0; index < valuesCustomList.Count; ++index)
      valuesCustomList[index] = (object) new CertSheetGraphSortClass((CertSheetGraphSortMethod) valuesCustomList[index]);
    return valuesCustomList;
  }
}
