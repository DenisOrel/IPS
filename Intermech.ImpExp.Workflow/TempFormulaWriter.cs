// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Workflow.TempFormulaWriter
// Assembly: Intermech.ImpExp.Workflow, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 3E5C231D-9C58-4E51-9000-3F9F7E271790
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Workflow.dll

using Intermech.Expert;
using System.IO;
using System.Text;
using System.Xml;

#nullable disable
namespace Intermech.ImpExp.Workflow;

internal class TempFormulaWriter
{
  private TempFormula _formula;

  public TempFormulaWriter(TempFormula tf) => this._formula = tf;

  public void SaveToStream(Stream stream)
  {
    XmlTextWriter writer = new XmlTextWriter(stream, Encoding.UTF8);
    writer.Formatting = Formatting.Indented;
    this._formula.WriteToXML(ref writer);
    writer.Flush();
  }
}
