// Decompiled with JetBrains decompiler
// Type: Intermech.MG.Integrator.DescriptionHandler
// Assembly: Intermech.MG.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DC8032C5-2D09-47AD-9096-064F93238E19
// Assembly location: D:\IPS\Client\Intermech.MG.Integrator.dll

using MGCPCB;

#nullable disable
namespace Intermech.MG.Integrator;

internal sealed class DescriptionHandler : ISpecialAttributeHandler
{
  private ExPCBPartEditor _partEditor;

  public DescriptionHandler(ExPCBPartEditor partEditor) => this._partEditor = partEditor;

  public string ReadValue(Component component)
  {
    return this._partEditor.GetPartDescription(component.PartNumber);
  }

  public void WriteValue(Component component, string value)
  {
    this._partEditor.SetPartDescription(component.PartNumber, value);
  }

  public string AttributeName => SpecialAttributesConsts.DescriptionAtribute;
}
