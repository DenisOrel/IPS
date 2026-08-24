// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.Common.DesignTime.InMemoryScriptProjectFile
// Assembly: Intermech.Scripting, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 8614EF74-D879-46F7-BBB4-441A9D335356
// Assembly location: D:\IPS\Client\RoslynScriptCompiler\Intermech.Scripting.dll
// XML documentation location: D:\IPS\Client\Intermech.Scripting.xml

using System;
using System.IO;
using System.Text;

#nullable disable
namespace Intermech.Scripting.Common.DesignTime;

/// <summary>
/// Реализация файла сценария в оперативной памяти.
/// Реализация не является thread safe.
/// </summary>
internal sealed class InMemoryScriptProjectFile : ScriptProjectFile
{
  private byte[] content;
  private static readonly byte[] emptyContent = new byte[0];

  /// <summary>Создает объект.</summary>
  public InMemoryScriptProjectFile() => this.content = InMemoryScriptProjectFile.emptyContent;

  /// <summary>Возвращает содержимое файла в виде массива байт.</summary>
  /// <returns>Содержимое файла</returns>
  public override byte[] GetContent()
  {
    return this.content.Length == 0 ? this.content : (byte[]) this.content.Clone();
  }

  /// <summary>Возвращает содержимое файла в текста.</summary>
  /// <param name="encoding">Кодировка файла</param>
  /// <returns>Содержимое файла</returns>
  public override string GetContentAsText(Encoding encoding)
  {
    if (encoding == null)
      throw new ArgumentNullException(nameof (encoding));
    if (this.content.Length == 0)
      return string.Empty;
    using (MemoryStream memoryStream = new MemoryStream(this.content, false))
    {
      using (StreamReader streamReader = new StreamReader((Stream) memoryStream, encoding))
        return streamReader.ReadToEnd();
    }
  }

  /// <summary>Задает содержимое файла в виде массива байт.</summary>
  /// <param name="content">Содержимое файла</param>
  public override void SetContent(byte[] content)
  {
    if (content == null)
      throw new ArgumentNullException(nameof (content));
    if (content.Length == 0)
      this.content = InMemoryScriptProjectFile.emptyContent;
    else
      this.content = (byte[]) content.Clone();
  }

  /// <summary>
  /// Задает содержимое файла в текста в указанной кодировке.
  /// </summary>
  /// <param name="text">Содержимое файла</param>
  /// <param name="encoding">Кодировка файла</param>
  public override void SetContentAsText(string text, Encoding encoding)
  {
    if (text == null)
      throw new ArgumentNullException(nameof (text));
    if (encoding == null)
      throw new ArgumentNullException(nameof (encoding));
    if (string.IsNullOrEmpty(text))
    {
      this.content = InMemoryScriptProjectFile.emptyContent;
    }
    else
    {
      using (MemoryStream memoryStream = new MemoryStream(text.Length * 2 + 3))
      {
        using (StreamWriter streamWriter = new StreamWriter((Stream) memoryStream, encoding))
        {
          streamWriter.Write(text);
          streamWriter.Flush();
          this.content = memoryStream.ToArray();
        }
      }
    }
  }

  /// <summary>Возвращает признак пустого файла.</summary>
  /// <returns>Признак пустого файла</returns>
  public override bool IsEmpty() => this.content.Length == 0;
}
