// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.SimpleLogger
// Assembly: Intermech.ImpExp.SearchData, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 218D3933-9EC7-421F-AD43-19C3596D6EE8
// Assembly location: D:\IPS\Client\Intermech.ImpExp.SearchData.dll

using System;
using System.IO;
using System.Text;

#nullable disable
namespace Intermech.ImpExp;

public class SimpleLogger
{
  private string _fileName = "";
  private string _prevMessage = "";
  private DateTime startTime = DateTime.MinValue;

  public SimpleLogger(string FileName) => this._fileName = FileName;

  public void Write(string message, bool LogTime)
  {
    if (LogTime)
      return;
    StreamWriter streamWriter = new StreamWriter(this._fileName, true, Encoding.UTF8);
    try
    {
      if (this.startTime != DateTime.MinValue)
      {
        TimeSpan timeSpan = DateTime.Now.Subtract(this.startTime);
        string str = $"{timeSpan.Minutes:00}:{timeSpan.Seconds:00}.{timeSpan.Milliseconds:######}";
        streamWriter.WriteLine($"{DateTime.Now:G}\t{this._prevMessage}\t{str}");
        this.startTime = DateTime.MinValue;
      }
      if (message == null)
        return;
      streamWriter.WriteLine($"{DateTime.Now:G}\t{message}");
    }
    finally
    {
      streamWriter.Close();
    }
  }

  public void Write(string message) => this.Write(message, false);

  public void Flush()
  {
    if (!(this.startTime != DateTime.MinValue))
      return;
    this.Write((string) null, false);
  }

  public string FileName => this._fileName;
}
