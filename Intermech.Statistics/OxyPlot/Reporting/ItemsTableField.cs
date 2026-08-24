// Decompiled with JetBrains decompiler
// Type: OxyPlot.Reporting.ItemsTableField
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System;
using System.Reflection;

#nullable disable
namespace OxyPlot.Reporting;

public class ItemsTableField
{
  public ItemsTableField(string header, string path, string stringFormat = null, Alignment alignment = Alignment.Center)
  {
    this.Header = header;
    this.Path = path;
    this.StringFormat = stringFormat;
    this.Alignment = alignment;
  }

  public Alignment Alignment { get; set; }

  public string Header { get; set; }

  public string Path { get; set; }

  public string StringFormat { get; set; }

  public double Width { get; set; }

  public string GetText(object item, IFormatProvider formatProvider)
  {
    object obj = RuntimeReflectionExtensions.GetRuntimeProperty(item.GetType(), this.Path).GetValue(item, (object[]) null);
    if (obj is IFormattable formattable)
      return formattable.ToString(this.StringFormat, formatProvider);
    return obj?.ToString();
  }
}
