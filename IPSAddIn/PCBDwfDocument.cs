// Decompiled with JetBrains decompiler
// Type: CSharpPlugin.PCBDwfDocument
// Assembly: IPSAddIn, Version=8.0.3.1634, Culture=neutral, PublicKeyToken=null
// MVID: F6758E82-0F4D-46BA-A517-315691E31B38
// Assembly location: D:\Projects\IPS Code\AltiumDesigner\IPSAddIn\IPSAddIn.dll

using Intermech.AltiumDesigner.Interfaces;
using Intermech.Data;
using K4os.Compression.LZ4.Legacy;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

#nullable disable
namespace CSharpPlugin;

internal sealed class PCBDwfDocument(IPSAddInProxy parent, string fileName) : 
  FileDocument<XDocument>(new XDocument(), fileName, parent),
  IPCBDwfDocument,
  IParametrable,
  IValueBagContainer,
  IIdentification,
  IDisposable
{
  private void LoadDocument()
  {
    using (FileStream innerStream = new FileStream(this.fileName, FileMode.Open, FileAccess.Read))
    {
      this.parametrableObject = XDocument.Load((Stream) new LZ4Stream((Stream) innerStream, LZ4StreamMode.Decompress));
      if (this.parametrableObject.Root == null)
        throw new Exception("Не найден корневой элемент при чтении файла " + this.fileName);
    }
  }

  private XElement RootParametersNode(bool check = true)
  {
    XElement xelement = this.parametrableObject.Root.Elements().First<XElement>((Func<XElement, bool>) (x => x.Name.LocalName == "Parameters"));
    return !check || xelement != null ? xelement : throw new Exception("Не найден корневой элемент для параметров <Parameters> в файле " + this.fileName);
  }

  protected override Parameter[] GetParameters()
  {
    this.LoadDocument();
    List<Parameter> parameterList = (List<Parameter>) null;
    IEnumerable<XElement> source = this.RootParametersNode(false)?.Elements();
    if (source != null)
      parameterList = source.ToList<XElement>().ConvertAll<Parameter>((Converter<XElement, Parameter>) (x => new Parameter(x.Elements().First<XElement>((Func<XElement, bool>) (y => y.Name.LocalName.Equals("Name"))).Value, (object) x.Elements().First<XElement>((Func<XElement, bool>) (y => y.Name.LocalName.Equals("Value"))).Value, false, typeof (string))));
    return parameterList == null ? new Parameter[0] : parameterList.ToArray();
  }

  protected override void WriteNewParameter(Parameter parameter)
  {
    XElement xelement = this.RootParametersNode();
    xelement.Add((object) new XElement(XName.Get("DrawingDocumentParameterData", xelement.Name.NamespaceName), new object[2]
    {
      (object) new XElement(XName.Get("Name", xelement.Name.NamespaceName), (object) parameter.Name),
      (object) new XElement(XName.Get("Value", xelement.Name.NamespaceName), (object) Convert.ToString(parameter.Value))
    }));
    if (this.setParameters)
      return;
    this.SaveDocument();
  }

  protected override void WriteParameterValue(Parameter parameter)
  {
    this.RootParametersNode().Elements().Where<XElement>((Func<XElement, bool>) (x => x.Elements().Where<XElement>((Func<XElement, bool>) (y => y.Name.LocalName.Equals("Name") && y.Value.Equals(parameter.Name, StringComparison.OrdinalIgnoreCase))).Count<XElement>() > 0)).SingleOrDefault<XElement>().Elements().First<XElement>((Func<XElement, bool>) (x => x.Name.LocalName.Equals("Value"))).Value = Convert.ToString(parameter.Value);
    if (this.setParameters)
      return;
    this.SaveDocument();
  }

  protected override void SaveDocument()
  {
    this.parent.CloseObject(this.fileName);
    using (FileStream innerStream = new FileStream(this.fileName, FileMode.Create, FileAccess.Write))
    {
      using (LZ4Stream lz4Stream = new LZ4Stream((Stream) innerStream, LZ4StreamMode.Compress))
        this.parametrableObject.Save((Stream) lz4Stream);
    }
  }
}
