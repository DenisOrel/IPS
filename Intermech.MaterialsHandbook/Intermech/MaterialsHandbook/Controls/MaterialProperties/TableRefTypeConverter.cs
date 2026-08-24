// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.Controls.MaterialProperties.TableRefTypeConverter
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Imbase;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

#nullable disable
namespace Intermech.MaterialsHandbook.Controls.MaterialProperties;

internal class TableRefTypeConverter : TypeConverter
{
  public override object ConvertTo(
    ITypeDescriptorContext context,
    CultureInfo culture,
    object value,
    Type destinationType)
  {
    if (typeof (string) == destinationType)
    {
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        if (!(sessionKeeper.Session.GetCustomService(typeof (IImbaseServer)) is IImbaseServer customService))
          return base.ConvertTo(context, culture, value, destinationType);
        Guid sessionGuid = sessionKeeper.Session.SessionGUID;
        Dictionary<string, string> source = customService.NameRecordReferences(sessionGuid, new List<string>()
        {
          value.ToString()
        });
        if (source != null)
        {
          if (source.Count > 0)
            return (object) source.First<KeyValuePair<string, string>>().Value;
        }
      }
    }
    return base.ConvertTo(context, culture, value, destinationType);
  }
}
