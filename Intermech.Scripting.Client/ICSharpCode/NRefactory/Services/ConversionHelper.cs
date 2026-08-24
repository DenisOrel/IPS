// Decompiled with JetBrains decompiler
// Type: ICSharpCode.NRefactory.Services.ConversionHelper
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using ICSharpCode.NRefactory.CSharp;
using ICSharpCode.NRefactory.Editor;
using ICSharpCode.NRefactory.TypeSystem;
using System.IO;
using System.Text;
using System.Xml;

#nullable disable
namespace ICSharpCode.NRefactory.Services;

internal sealed class ConversionHelper
{
  private CSharpFormattingOptions formattingOptions;
  private CSharpAmbience typeAmbiance;
  private CSharpAmbience standardAmbiance;
  private CSharpAmbience overridesAmbiance;
  private CSharpAmbience nameOnlyAmbiance;
  private static readonly char[] xmlWhitespaceChars = new char[4]
  {
    '\r',
    '\n',
    '\t',
    ' '
  };

  public ConversionHelper()
  {
    this.formattingOptions = FormattingOptionsFactory.CreateAllman();
    this.typeAmbiance = new CSharpAmbience();
    this.typeAmbiance.ConversionFlags = ConversionFlags.UseFullyQualifiedTypeNames | ConversionFlags.ShowTypeParameterList;
    this.standardAmbiance = new CSharpAmbience();
    this.standardAmbiance.ConversionFlags = ConversionFlags.StandardConversionFlags;
    this.overridesAmbiance = new CSharpAmbience();
    this.overridesAmbiance.ConversionFlags = ConversionFlags.ShowParameterList | ConversionFlags.ShowParameterNames | ConversionFlags.ShowTypeParameterList;
    this.nameOnlyAmbiance = new CSharpAmbience();
    this.nameOnlyAmbiance.ConversionFlags = ConversionFlags.None;
  }

  public CSharpFormattingOptions FormattingOptions => this.formattingOptions;

  public CSharpAmbience TypeAmbiance => this.typeAmbiance;

  public CSharpAmbience StandardAmbiance => this.standardAmbiance;

  public CSharpAmbience OverridesAmbiance => this.overridesAmbiance;

  public CSharpAmbience NameOnlyAmbiance => this.nameOnlyAmbiance;

  public string ConvertSymbolToPlainText(ISymbol symbol, CSharpAmbience ambience)
  {
    using (StringWriter stringWriter = new StringWriter(new StringBuilder()))
    {
      TextWriterTokenWriter writer = new TextWriterTokenWriter((TextWriter) stringWriter);
      ambience.ConvertSymbol(symbol, (TokenWriter) writer, this.formattingOptions);
      return stringWriter.ToString();
    }
  }

  public string ConvertDocumentationToPlainText(ITextSource xmlDoc)
  {
    if (xmlDoc.TextLength == 0)
      return string.Empty;
    try
    {
      TextBuilder textBuilder = new TextBuilder();
      using (XmlTextReader xmlTextReader = new XmlTextReader((TextReader) new StringReader($"<root>{xmlDoc.Text}</root>")))
      {
        xmlTextReader.XmlResolver = (XmlResolver) null;
        while (xmlTextReader.Read())
        {
          switch (xmlTextReader.NodeType)
          {
            case XmlNodeType.Element:
              switch (xmlTextReader.Name)
              {
                case "filterpriority":
                  xmlTextReader.Skip();
                  continue;
                case "returns":
                  textBuilder.AppendLine();
                  textBuilder.Append("Returns");
                  textBuilder.Append(": ");
                  continue;
                case "param":
                  textBuilder.AppendLine();
                  textBuilder.Append(this.TrimXmlWhitespace(xmlTextReader.GetAttribute("name")));
                  textBuilder.Append(": ");
                  continue;
                case "remarks":
                  textBuilder.AppendLine();
                  textBuilder.Append("Remarks");
                  textBuilder.Append(": ");
                  continue;
                case "see":
                  if (xmlTextReader.IsEmptyElement)
                  {
                    textBuilder.Append(this.TrimXmlWhitespace(xmlTextReader.GetAttribute("cref")));
                    continue;
                  }
                  int content = (int) xmlTextReader.MoveToContent();
                  if (xmlTextReader.HasValue)
                  {
                    textBuilder.Append(this.TrimXmlWhitespace(xmlTextReader.Value));
                    continue;
                  }
                  textBuilder.Append(this.TrimXmlWhitespace(xmlTextReader.GetAttribute("cref")));
                  continue;
                default:
                  continue;
              }
            case XmlNodeType.Text:
              textBuilder.Append(this.TrimXmlWhitespace(xmlTextReader.Value));
              continue;
            default:
              continue;
          }
        }
      }
      return textBuilder.ToString();
    }
    catch (XmlException ex)
    {
      return string.Empty;
    }
  }

  private string TrimXmlWhitespace(string text) => text?.Trim(ConversionHelper.xmlWhitespaceChars);
}
