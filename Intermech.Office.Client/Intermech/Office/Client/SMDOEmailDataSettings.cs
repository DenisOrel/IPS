// Decompiled with JetBrains decompiler
// Type: Intermech.Office.Client.SMDOEmailDataSettings
// Assembly: Intermech.Office.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 87EC380F-A344-4B99-B3CC-05ECB303FAD4
// Assembly location: D:\IPS\Client\Intermech.Office.Client.dll

using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

#nullable disable
namespace Intermech.Office.Client;

public class SMDOEmailDataSettings
{
  public string ConfName { get; set; }

  public int ConfValue { get; set; }

  public Dictionary<string, string> Organizations { get; set; }

  public bool IsHaveSigns { get; set; }

  public string OpenKeyID { get; set; }

  public bool IsHavePrivateKey { get; set; }

  public X509Certificate2 Certificate { get; set; }
}
