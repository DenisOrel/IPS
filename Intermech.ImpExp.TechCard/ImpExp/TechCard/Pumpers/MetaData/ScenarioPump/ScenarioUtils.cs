// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.MetaData.ScenarioPump.ScenarioUtils
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.TechCard.Common;
using Intermech.Interfaces.TechCard;
using System;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.MetaData.ScenarioPump;

internal static class ScenarioUtils
{
  private static int GetProductionId(Scenario scenario)
  {
    int productionId = 0;
    if (scenario.Property != null && scenario.Property.Catalog != null)
      productionId = scenario.Property.Catalog.Production;
    return productionId;
  }

  public static Guid GetImportingObjectType(Scenario scenario)
  {
    Guid empty = Guid.Empty;
    if (scenario == null)
      return empty;
    ScenarioKind kind = scenario.Kind;
    int productionId = ScenarioUtils.GetProductionId(scenario);
    switch (kind)
    {
      case ScenarioKind.Zagot:
      case ScenarioKind.ZagSourceSearch:
      case ScenarioKind.ZagSourceImbase:
        return TechPumpData.TechType.TechTypeList.GetObjTypeGuid(TechcardConsts.TpRecordType.MaterialMain);
      case ScenarioKind.Oper:
        return TechPumpData.TechType.TechTypeList.GetObjTypeGuid(TechcardConsts.TpRecordType.Oper);
      case ScenarioKind.Rez:
        return TechPumpData.TechType.TechTypeList.GetObjTypeGuid(TechcardConsts.TpRecordType.Rezhim);
      case ScenarioKind.OperNorm:
        return TechCardConsts.ObjectTypes.OperationNormGUID;
      case ScenarioKind.PerehConc:
        return TechPumpData.TechType.TechTypeList.GetObjTypeGuid(TechcardConsts.TpRecordType.Perehod);
      case ScenarioKind.PerehNorm:
        return TechCardConsts.ObjectTypes.PerehodNormGUID;
      case ScenarioKind.Osn:
        return TechPumpData.TechType.TechTypeList.GetObjTypeGuid(TechcardConsts.TpRecordType.Tool);
      case ScenarioKind.Mat:
        return TechPumpData.TechType.TechTypeList.GetObjTypeGuid(TechcardConsts.TpRecordType.MaterialMain);
      case ScenarioKind.Comm:
        return productionId == 19 ? TechPumpData.TechType.TechTypeList.GetObjTypeGuid(TechcardConsts.TpRecordType.MaterialMain) : TechCardConsts.ObjectTypes.TechProcEdinGUID;
      case ScenarioKind.AddMat:
        return TechPumpData.TechType.TechTypeList.GetObjTypeGuid(TechcardConsts.TpRecordType.MaterialAdd);
      case ScenarioKind.TipTp:
        return TechCardConsts.ObjectTypes.PerehodGUID;
      case ScenarioKind.Pmvm:
      case ScenarioKind.ZagPage:
      case ScenarioKind.OperPage:
      case ScenarioKind.OborPage:
      case ScenarioKind.ToolPage:
      case ScenarioKind.DetPage:
      case ScenarioKind.DocPage:
      case ScenarioKind.SurfacePage:
      case ScenarioKind.Reports:
      case ScenarioKind.Dce:
      case ScenarioKind.DcePage:
      case ScenarioKind.SurfaceExt:
      case ScenarioKind.WorkerPage:
      case ScenarioKind.TpChange:
      case ScenarioKind.KtpOper:
      case ScenarioKind.KtpPereh:
      case ScenarioKind.KtpObor:
      case ScenarioKind.KtpTool:
      case ScenarioKind.OsnArm:
      case ScenarioKind.Ktd:
      case ScenarioKind.AutoParam:
      case ScenarioKind.ConParPage:
      case ScenarioKind.VsRep:
      case ScenarioKind.VsRepPage:
      case ScenarioKind.Route:
      case ScenarioKind.RzIzw:
      case ScenarioKind.KtdAlb:
      case ScenarioKind.KtdPage:
      case ScenarioKind.KtdAlbPage:
      case ScenarioKind.KtdVedPage:
      case ScenarioKind.KtdSostavPage:
      case ScenarioKind.ZakDocArtLink:
      case ScenarioKind.Izw:
      case ScenarioKind.RouteIzw:
      case ScenarioKind.ZagIzw:
      case ScenarioKind.MatIzw:
      case ScenarioKind.MatGrIzw:
      case ScenarioKind.IzwDocs:
      case ScenarioKind.Arts:
      case ScenarioKind.SingleGroupComm:
      case ScenarioKind.SingleTypeComm:
      case ScenarioKind.OperDiffTblPrototype:
      case ScenarioKind.PerehDiffTblPrototype:
      case ScenarioKind.TpDiffTblPrototype:
      case ScenarioKind.NormDiffTblPrototype:
      case ScenarioKind.AttachingGr:
      case ScenarioKind.Attaching:
      case ScenarioKind.NotLinkedOpers:
      case ScenarioKind.LinkedTp2Route:
      case ScenarioKind.SKtpPage:
      case ScenarioKind.DceFromAnTp:
        return empty;
      case ScenarioKind.OperMode:
        return TechPumpData.TechType.TechTypeList.GetObjTypeGuid(TechcardConsts.TpRecordType.Oper);
      case ScenarioKind.TpNorm:
        return TechcardConsts.TypeConsts.otTPNorm;
      case ScenarioKind.Surface:
        TechPumpData.Tables.ImTablesData.GetIpsImObjectGuid(TechcardConsts.imTablesConsts.Surfaces);
        goto case ScenarioKind.Pmvm;
      case ScenarioKind.Equipment:
        return TechPumpData.Tables.ImTablesData.GetIpsImObjectGuid(TechcardConsts.imTablesConsts.Oborud);
      case ScenarioKind.Worker:
        return TechPumpData.Tables.ImTablesData.GetIpsImObjectGuid(TechcardConsts.imTablesConsts.Works);
      case ScenarioKind.SpecTool:
        return TechPumpData.TechType.TechTypeList.GetObjTypeGuid(TechcardConsts.TpRecordType.Tool);
      case ScenarioKind.NormPerehConc:
        return TechPumpData.TechType.TechTypeList.GetObjTypeGuid(TechcardConsts.TpRecordType.Perehod);
      case ScenarioKind.NormPerehRez:
      case ScenarioKind.NormPerehNorm:
        return TechCardConsts.ObjectTypes.PerehodNormGUID;
      case ScenarioKind.RouteTemplate:
        return TechPumpData.TechType.TechTypeList.GetObjTypeGuid(TechcardConsts.TpRecordType.RouteTemplate);
      case ScenarioKind.RouteElement:
        return TechPumpData.TechType.TechTypeList.GetObjTypeGuid(TechcardConsts.TpRecordType.TemplateElem);
      case ScenarioKind.MatSetCommon:
        return TechCardConsts.ObjectTypes.MaterialGroupGUID;
      case ScenarioKind.MatConc:
        return TechPumpData.Tables.ImTablesData.GetIpsImObjectGuid(TechcardConsts.imTablesConsts.SupportMater);
      case ScenarioKind.GroupComm:
        return TechCardConsts.ObjectTypes.TechProcGroupGUID;
      case ScenarioKind.TypeComm:
        return TechCardConsts.ObjectTypes.TechProcTipovGUID;
      default:
        return Guid.Empty;
    }
  }
}
