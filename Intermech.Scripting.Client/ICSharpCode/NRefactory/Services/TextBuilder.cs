// Decompiled with JetBrains decompiler
// Type: ICSharpCode.NRefactory.Services.TextBuilder
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using System.Text;

#nullable disable
namespace ICSharpCode.NRefactory.Services;

internal sealed class TextBuilder
{
  private StringBuilder sb;

  public TextBuilder() => this.sb = new StringBuilder();

  public void Append(string text)
  {
    if (text == null)
      return;
    this.sb.Append(text);
  }

  public void AppendLine() => this.sb.AppendLine();

  public void AppendLine(string text)
  {
    if (text == null)
      return;
    this.sb.AppendLine(text);
  }

  public override string ToString() => this.sb.ToString();
}
