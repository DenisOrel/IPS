// Decompiled with JetBrains decompiler
// Type: OxyPlot.PdfWriter
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

#nullable disable
namespace OxyPlot;

internal class PdfWriter : IDisposable
{
  private BinaryWriter w;

  public PdfWriter(Stream s) => this.w = new BinaryWriter(s);

  public long Position => this.w.BaseStream.Position;

  public void Write(string format, params object[] args)
  {
    this.w.Write(Encoding.UTF8.GetBytes(string.Format((IFormatProvider) CultureInfo.InvariantCulture, format, args)));
  }

  public void WriteLine(string format, params object[] args) => this.Write(format + "\n", args);

  public void Write(Dictionary<string, object> dictionary)
  {
    this.WriteLine("<<");
    foreach (KeyValuePair<string, object> keyValuePair in dictionary)
    {
      this.Write(keyValuePair.Key);
      this.Write(" ");
      this.WriteCore(keyValuePair.Value);
      this.WriteLine();
    }
    this.Write(">>");
  }

  public void Write(byte[] bytes) => this.w.Write(bytes);

  public void WriteLine() => this.WriteLine(string.Empty);

  public void Dispose() => this.w.Dispose();

  private void WriteCore(object o)
  {
    switch (o)
    {
      case PdfWriter.IPortableDocumentObject portableDocumentObject:
        this.Write("{0} 0 R", (object) portableDocumentObject.ObjectNumber);
        break;
      case PdfWriter.ObjectType _:
        this.Write("/{0}", o);
        break;
      case int _:
      case double _:
        this.Write("{0}", o);
        break;
      case bool flag:
        this.Write(flag ? "true" : "false");
        break;
      case DateTime dateTime:
        this.Write($"(D:{dateTime.ToString("yyyyMMddHHmmsszz")}'00)");
        break;
      case string format:
        this.Write(format);
        break;
      case IList list:
        this.WriteList(list);
        break;
      case Dictionary<string, object> dictionary:
        this.Write(dictionary);
        break;
    }
  }

  private void WriteList(IList list)
  {
    this.Write("[");
    bool flag = true;
    foreach (object o in (IEnumerable) list)
    {
      if (!flag)
        this.Write(" ");
      else
        flag = false;
      this.WriteCore(o);
    }
    this.Write("]");
  }

  internal enum ObjectType
  {
    Catalog,
    Pages,
    Page,
    Font,
    XObject,
    ExtGState,
    FontDescriptor,
  }

  internal interface IPortableDocumentObject
  {
    int ObjectNumber { get; }
  }
}
