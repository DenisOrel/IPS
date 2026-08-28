// Decompiled with JetBrains decompiler
// Type: Intermech.Localization.LocalizationHolder
// Assembly: Intermech.Portal.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 814BABAA-794A-446D-BCF7-B9A0D67EFF42
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Portal.Server.dll

using System.Reflection;
using System.Resources;

#nullable disable
namespace Intermech.Localization;

internal class LocalizationHolder
{
  public static ResourceManager rm = new ResourceManager("Intermech.Portal.Server.Resources.PortalServerResources", Assembly.GetExecutingAssembly());
  public static ResourceManager rma = new ResourceManager("Intermech.Portal.Server.Resources.CustomAttributesResources", Assembly.GetExecutingAssembly());
}
