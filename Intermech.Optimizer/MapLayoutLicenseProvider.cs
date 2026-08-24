// Decompiled with JetBrains decompiler
// Type: Intermech.Map.Layout.MapLayoutLicenseProvider
// Assembly: Intermech.Optimizer, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: F2F8A027-9638-497B-A691-BEF61B30B332
// Assembly location: D:\IPS\Client\Intermech.Optimizer.dll
// XML documentation location: D:\IPS\Client\Intermech.Optimizer.xml

using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.Serialization;
using System.Security.Permissions;

#nullable disable
namespace Intermech.Map.Layout;

[Serializable]
internal sealed class MapLayoutLicenseProvider : LicenseProvider
{
  private int Dispose(string keystring, bool run) => 0;

  public override License GetLicense(
    LicenseContext context,
    Type type,
    object instance,
    bool allowExceptions)
  {
    return (License) new MapLayoutLicenseProvider.MapLayoutLicense("", 0);
  }

  private int ParseInt(string s) => int.Parse(s, (IFormatProvider) NumberFormatInfo.InvariantInfo);

  private string StringFloat(float f)
  {
    return f.ToString((IFormatProvider) NumberFormatInfo.InvariantInfo);
  }

  [Serializable]
  internal sealed class MapLayoutLicense : License
  {
    private string myKey;

    internal MapLayoutLicense(SerializationInfo info, StreamingContext context)
    {
      this.myKey = (string) null;
      this.myKey = (string) info.GetValue(nameof (myKey), typeof (string));
    }

    internal MapLayoutLicense(string key, int c)
    {
      this.myKey = (string) null;
      this.myKey = key;
    }

    public override void Dispose()
    {
    }

    [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      info.AddValue("myKey", (object) this.myKey);
    }

    public override string LicenseKey => this.myKey;
  }
}
