// Decompiled with JetBrains decompiler
// Type: Intermech.MG.Integrator.ExPCBProjectBoardsReader
// Assembly: Intermech.MG.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DC8032C5-2D09-47AD-9096-064F93238E19
// Assembly location: D:\IPS\Client\Intermech.MG.Integrator.dll

using Intermech.Data;
using Intermech.Tools.Integrators.Electrical;
using MGCPCB;

#nullable disable
namespace Intermech.MG.Integrator;

internal class ExPCBProjectBoardsReader(ECADIntegratorSettings settings) : 
  BoardReader<IMGProjectItem>(settings)
{
  protected override string ReadDesignation(string boardName, IValueBagContainer component)
  {
    return this.ReadArticleKey(component);
  }

  protected override string ReadName(string boardName, IValueBagContainer component)
  {
    return ((MGComponent<Document>) component).PartNumber;
  }

  protected override bool ReadIsMain(IValueBagContainer component) => true;

  protected override IValueBagContainer GetAsmComponent(IMGProjectItem board)
  {
    return (IValueBagContainer) board.AssemblyComponent;
  }

  protected override string ReadArticleKey(IValueBagContainer component)
  {
    return ((IElectricalComponent) component).UID;
  }
}
