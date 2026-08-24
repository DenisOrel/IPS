// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.Controls.MaterialProperties.DataProvider
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace Intermech.MaterialsHandbook.Controls.MaterialProperties;

public abstract class DataProvider
{
  public virtual List<Tuple<string, IEnumerable<DataTable>>> LoadData(string imbaseKey)
  {
    return new List<Tuple<string, IEnumerable<DataTable>>>();
  }

  public virtual void SaveData(string imbaseKey, List<Tuple<string, IEnumerable<DataTable>>> data)
  {
  }
}
