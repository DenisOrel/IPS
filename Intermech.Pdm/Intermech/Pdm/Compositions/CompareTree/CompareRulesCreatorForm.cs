// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.CompareTree.CompareRulesCreatorForm
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Pdm;
using System;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Pdm.Compositions.CompareTree;

internal sealed class CompareRulesCreatorForm : IObjectCreatorCustomService
{
  public long CreateObjectDialog(
    int ObjectTypeID,
    long TemplateObjectID,
    int[] RelationTypeIDs,
    long[] RelatedObjectIDs,
    DateTime StartDate,
    bool isVersion)
  {
    using (CompareRulesForm compareRulesForm = new CompareRulesForm())
    {
      compareRulesForm.ParentMode = 1;
      compareRulesForm.ObjectType = ObjectTypeID;
      compareRulesForm.LoadObjectData(Guid.Empty, 0);
      return compareRulesForm.ShowDialog() == DialogResult.OK ? compareRulesForm.RuleID : 0L;
    }
  }

  public static void Attach(IObjectCreatorService service)
  {
    service.RegisterCreatorCustomService(MetaDataHelper.GetObjectTypeID(PDMHelper.objtypeCommonCompositionRules), typeof (CompareRulesCreatorForm));
    service.RegisterCreatorCustomService(MetaDataHelper.GetObjectTypeID(PDMHelper.objtypePersonalCompositionRules), typeof (CompareRulesCreatorForm));
  }

  public static void Detach(IObjectCreatorService service)
  {
    service.UnregisterCreatorCustomService(MetaDataHelper.GetObjectTypeID(PDMHelper.objtypeCommonCompositionRules), typeof (CompareRulesCreatorForm));
    service.UnregisterCreatorCustomService(MetaDataHelper.GetObjectTypeID(PDMHelper.objtypePersonalCompositionRules), typeof (CompareRulesCreatorForm));
  }
}
