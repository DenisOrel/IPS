// Decompiled with JetBrains decompiler
// Type: Intermech.NX.Integrator.NXSettingsCodec
// Assembly: Intermech.NX.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D5A5DA32-DA1F-4D5A-845A-F0226BC2C153
// Assembly location: D:\IPS\Client\Intermech.NX.Integrator.dll

using Intermech.Tools.Integrators.CADInterface;
using Intermech.Tools.Settings;
using System.Xml;

#nullable disable
namespace Intermech.NX.Integrator;

internal sealed class NXSettingsCodec(string integratorName, ISettingsObjectFactory factory) : 
  CADSettingsCodec(integratorName, factory)
{
  protected override void EncodeCustomSettings(
    CADSettings settingsObject,
    SettingsXmlBuilder settingsBuilder)
  {
    base.EncodeCustomSettings(settingsObject, settingsBuilder);
    NXSettings nxSettings = (NXSettings) settingsObject;
    XmlElement element = settingsBuilder.CreateElement("ModelJTFiles");
    settingsBuilder.AppendAttribute((XmlNode) element, "Enabled", (object) nxSettings.EnableModelJTFiles);
    settingsBuilder.AppendElement((XmlNode) element);
  }

  protected override void DecodeCustomSettings(
    SettingsXmlBuilder settingsBuilder,
    CADSettings settingsObject)
  {
    base.DecodeCustomSettings(settingsBuilder, settingsObject);
    NXSettings nxSettings = (NXSettings) settingsObject;
    XmlNode parentNode = settingsBuilder.SelectSingleNode("ModelJTFiles[@Enabled]");
    if (parentNode == null)
      return;
    nxSettings.EnableModelJTFiles = settingsBuilder.ReadAttribute<bool>(parentNode, "Enabled", false);
  }
}
