// Decompiled with JetBrains decompiler
// Type: Intermech.Localization.LocalizationHolder
// Assembly: Intermech.Reports.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: B97D7940-CE11-4EF0-80CD-76A0AE479D33
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Reports.Server.dll

using System.Reflection;
using System.Resources;

#nullable disable
namespace Intermech.Localization;

internal class LocalizationHolder
{
  public static ResourceManager rm = new ResourceManager("Intermech.Reports.Server.Resources.ReportsServerResources", Assembly.GetExecutingAssembly());
  public static ResourceManager rma = new ResourceManager("Intermech.Reports.Server.Resources.CustomAttributesResources", Assembly.GetExecutingAssembly());
}
