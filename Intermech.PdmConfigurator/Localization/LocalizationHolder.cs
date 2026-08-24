// Decompiled with JetBrains decompiler
// Type: Intermech.Localization.LocalizationHolder
// Assembly: Intermech.PdmConfigurator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: B5CB2E26-657B-4329-B46C-77AE46A32171
// Assembly location: D:\IPS\Client\Intermech.PdmConfigurator.dll

using System.Reflection;
using System.Resources;

#nullable disable
namespace Intermech.Localization;

internal class LocalizationHolder
{
  public static ResourceManager rm = new ResourceManager("Intermech.PdmConfigurator.Resources.PdmConfiguratorResources", Assembly.GetExecutingAssembly());
  public static ResourceManager rma = new ResourceManager("Intermech.PdmConfigurator.Resources.CustomAttributesResources", Assembly.GetExecutingAssembly());
}
