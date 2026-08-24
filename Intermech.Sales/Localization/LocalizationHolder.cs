// Decompiled with JetBrains decompiler
// Type: Intermech.Localization.LocalizationHolder
// Assembly: Intermech.Sales, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0D9A9043-6548-439B-99F7-AF22F44A5D2B
// Assembly location: D:\IPS\Client\Intermech.Sales.dll

using System.Reflection;
using System.Resources;

#nullable disable
namespace Intermech.Localization;

internal class LocalizationHolder
{
  public static ResourceManager rm = new ResourceManager("Intermech.Sales.Resources.SalesResources", Assembly.GetExecutingAssembly());
  public static ResourceManager rma = new ResourceManager("Intermech.Sales.Resources.CustomAttributesResources", Assembly.GetExecutingAssembly());
}
