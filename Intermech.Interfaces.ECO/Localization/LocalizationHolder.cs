// Decompiled with JetBrains decompiler
// Type: Intermech.Localization.LocalizationHolder
// Assembly: Intermech.Interfaces.ECO, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B25D666E-9146-4B6E-9222-8722321C22A6
// Assembly location: D:\IPS\Client\Intermech.Interfaces.ECO.dll
// XML documentation location: D:\IPS\Client\Intermech.Interfaces.ECO.xml

using System.Reflection;
using System.Resources;

#nullable disable
namespace Intermech.Localization;

internal class LocalizationHolder
{
  public static ResourceManager rm = new ResourceManager("Intermech.Interfaces.ECO.Resources.InterfacesECOResources", Assembly.GetExecutingAssembly());
  public static ResourceManager rma = new ResourceManager("Intermech.Interfaces.ECO.Resources.CustomAttributesResources", Assembly.GetExecutingAssembly());
}
