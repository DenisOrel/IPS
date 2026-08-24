// Decompiled with JetBrains decompiler
// Type: OxyPlot.Reporting.HeaderHelper
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

#nullable disable
namespace OxyPlot.Reporting;

public class HeaderHelper
{
  private readonly int[] headerLevel = new int[10];

  public string GetHeader(int level)
  {
    for (int index = level - 1; index > 0; --index)
    {
      if (this.headerLevel[index] == 0)
        this.headerLevel[index] = 1;
    }
    ++this.headerLevel[level];
    for (int index = level + 1; index < 10; ++index)
      this.headerLevel[index] = 0;
    string empty = string.Empty;
    for (int index = 1; index <= level; ++index)
    {
      if (index > 1)
        empty += ".";
      empty += (string) (object) this.headerLevel[index];
    }
    return empty;
  }
}
