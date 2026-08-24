// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Security.AccessRightsMapper
// Assembly: Intermech.ImpExp.Security, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: B4185E78-CFCB-46F6-B1BC-486522A5A9AE
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Security.dll

using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.Security;

internal class AccessRightsMapper : Dictionary<BOType, RightsMap>
{
  public RightInfo Map(BOType boType, RightType rtype)
  {
    RightsMap rightsMap = (RightsMap) null;
    RightInfo rightInfo = (RightInfo) null;
    this.TryGetValue(boType, out rightsMap);
    if (rightsMap == null || !rightsMap.TryGetValue(rtype, out rightInfo) && !rightsMap.TryGetValue(RightType.Default, out rightInfo))
      this[BOType.All].TryGetValue(rtype, out rightInfo);
    return rightInfo;
  }

  public AccessRightsMapper()
  {
    RightsMap rightsMap1 = new RightsMap();
    rightsMap1.Add(RightType.arOpenObject, new RightInfo(ActionType.Open));
    rightsMap1.Add(RightType.arReadObject, new RightInfo(ActionType.View));
    rightsMap1.Add(RightType.arPrintObject, new RightInfo(ActionType.Print));
    rightsMap1.Add(RightType.arCopyObject, new RightInfo(ActionType.Copy));
    rightsMap1.Add(RightType.arEditObject, new RightInfo(ActionType.Edit));
    rightsMap1.Add(RightType.arDeleteObject, new RightInfo(ActionType.Delete));
    rightsMap1.Add(RightType.arListObject, new RightInfo(ActionType.List));
    rightsMap1.Add(RightType.arAddObject, new RightInfo(ActionType.Create));
    rightsMap1.Add(RightType.arAddChildObject, new RightInfo(ActionType.AddLink));
    rightsMap1.Add(RightType.arEditChildObject, (RightInfo) null);
    rightsMap1.Add(RightType.arDeleteChildObject, new RightInfo(ActionType.DeleteLink));
    rightsMap1.Add(RightType.arChangeStructure, new RightInfo(ActionType.EditProperties));
    rightsMap1.Add(RightType.arDeleteThisObject, new RightInfo(ActionType.Delete));
    rightsMap1.Add(RightType.arReadRights, new RightInfo(ActionType.GetAccess));
    rightsMap1.Add(RightType.arChangeRights, new RightInfo(ActionType.SetAccess));
    this.Add(BOType.All, rightsMap1);
    RightsMap rightsMap2 = new RightsMap();
    rightsMap2.Add(RightType.arReadObject, new RightInfo(ActionType.View, 17));
    rightsMap2.Add(RightType.arPrintObject, new RightInfo(ActionType.Print, 17));
    rightsMap2.Add(RightType.arCopyObject, new RightInfo(ActionType.SaveToDisk, 17));
    rightsMap2.Add(RightType.arAddObject, new RightInfo(ActionType.Create, 17));
    rightsMap2.Add(RightType.arEditObject, new RightInfo(ActionType.Edit, 17));
    rightsMap2.Add(RightType.arDeleteObject, new RightInfo(ActionType.Delete, 17));
    rightsMap2.Add(RightType.arRemoveObject, new RightInfo(ActionType.Remove, 17));
    rightsMap2.Add(RightType.arAddChildObject, (RightInfo) null);
    rightsMap2.Add(RightType.arDeleteChildObject, (RightInfo) null);
    rightsMap2.Add(RightType.arChangeStructure, new RightInfo(ActionType.EditProperties));
    rightsMap2.Add(RightType.arChangeObjectsRights, new RightInfo(ActionType.SetAccess, 17));
    rightsMap2.Add(RightType.arDocRegistry, new RightInfo(ActionType.DocRegistry, 17));
    this.Add(BOType.Archive, rightsMap2);
    RightsMap rightsMap3 = new RightsMap();
    rightsMap3.Add(RightType.arEditParameters, new RightInfo(ActionType.Edit));
    this.Add(BOType.Object, rightsMap3);
    RightsMap rightsMap4 = new RightsMap();
    rightsMap4.Add(RightType.arListObject, new RightInfo(new ActionType[2]
    {
      ActionType.View,
      ActionType.List
    }, 4));
    rightsMap4.Add(RightType.arAddObject, new RightInfo(ActionType.CreateChildItem, 4));
    rightsMap4.Add(RightType.arReadRights, new RightInfo(ActionType.GetAccess, 4));
    rightsMap4.Add(RightType.arChangeRights, new RightInfo(ActionType.SetAccess, 4));
    rightsMap4.Add(RightType.arEditObject, new RightInfo(ActionType.Edit, 7));
    rightsMap4.Add(RightType.arDeleteObject, new RightInfo(ActionType.Delete, 7));
    rightsMap4.Add(RightType.Default, (RightInfo) null);
    this.Add(BOType.ArticleType, rightsMap4);
    RightsMap rightsMap5 = new RightsMap();
    rightsMap5.Add(RightType.arChangeStructure, new RightInfo(ActionType.EditProperties, 3));
    rightsMap5.Add(RightType.Default, (RightInfo) null);
    this.Add(BOType.ThemeParamsGroup, rightsMap5);
    RightsMap rightsMap6 = new RightsMap();
    rightsMap6.Add(RightType.arReadObject, new RightInfo(ActionType.Read, 3));
    rightsMap6.Add(RightType.arEditObject, new RightInfo(ActionType.Write, 3));
    rightsMap6.Add(RightType.arRemoveObject, new RightInfo(ActionType.Delete, 3));
    rightsMap6.Add(RightType.arReadRights, new RightInfo(ActionType.GetAccess, 3));
    rightsMap6.Add(RightType.arChangeRights, new RightInfo(ActionType.SetAccess, 3));
    rightsMap6.Add(RightType.Default, (RightInfo) null);
    this.Add(BOType.ThemeParameter, rightsMap6);
    RightsMap rightsMap7 = new RightsMap();
    rightsMap7.Add(RightType.arOpenObject, new RightInfo(ActionType.Open, 4));
    rightsMap7.Add(RightType.arAddChildObject, new RightInfo(ActionType.Create, 4));
    rightsMap7.Add(RightType.arDeleteChildObject, new RightInfo(ActionType.Delete, 4));
    rightsMap7.Add(RightType.arEditChildObject, new RightInfo(ActionType.EditProperties, 4));
    rightsMap7.Add(RightType.arReadRights, new RightInfo(ActionType.GetAccess, 4));
    rightsMap7.Add(RightType.arChangeRights, new RightInfo(ActionType.SetAccess, 4));
    rightsMap7.Add(RightType.Default, (RightInfo) null);
    this.Add(BOType.Articles, rightsMap7);
    RightsMap rightsMap8 = new RightsMap();
    rightsMap8.Add(RightType.arListObject, new RightInfo(ActionType.View, 4));
    rightsMap8.Add(RightType.arAddClassif, new RightInfo(ActionType.CreateChildItem, 4));
    rightsMap8.Add(RightType.arReadRights, new RightInfo(ActionType.GetAccess, 7));
    rightsMap8.Add(RightType.arChangeRights, new RightInfo(ActionType.SetAccess, 7));
    rightsMap8.Add(RightType.arEditClassif, new RightInfo(ActionType.Edit, 7));
    rightsMap8.Add(RightType.arDeleteClassif, new RightInfo(ActionType.Delete, 7));
    rightsMap8.Add(RightType.Default, (RightInfo) null);
    this.Add(BOType.ClassificatorsList, rightsMap8);
    RightsMap rightsMap9 = new RightsMap();
    rightsMap9.Add(RightType.arListObject, new RightInfo(ActionType.View));
    rightsMap9.Add(RightType.arReadRights, new RightInfo(ActionType.GetAccess));
    rightsMap9.Add(RightType.arChangeRights, new RightInfo(ActionType.SetAccess));
    rightsMap9.Add(RightType.Default, (RightInfo) null);
    this.Add(BOType.Classificator, rightsMap9);
    RightsMap rightsMap10 = new RightsMap();
    rightsMap10.Add(RightType.arEditChildObject, new RightInfo(ActionType.Edit));
    this.Add(BOType.User, rightsMap10);
    RightsMap rightsMap11 = new RightsMap();
    rightsMap11.Add(RightType.arChangeStructure, new RightInfo(ActionType.Edit));
    this.Add(BOType.UserGroup, rightsMap11);
    RightsMap rightsMap12 = new RightsMap();
    rightsMap12.Add(RightType.arAddChildObject, new RightInfo(ActionType.CreateChildItem, 4));
    rightsMap12.Add(RightType.arDeleteChildObject, new RightInfo(ActionType.Delete, 7));
    rightsMap12.Add(RightType.Default, (RightInfo) null);
    this.Add(BOType.SchemesRoot, rightsMap12);
    RightsMap rightsMap13 = new RightsMap();
    rightsMap13.Add(RightType.arListObject, new RightInfo(ActionType.View));
    rightsMap13.Add(RightType.arLaunchProcess, new RightInfo(ActionType.wfLaunchProcess));
    rightsMap13.Add(RightType.arEditProcess, new RightInfo(ActionType.wfEditProcess));
    rightsMap13.Add(RightType.arAdminProcess, new RightInfo(ActionType.wfAdminProcess));
    rightsMap13.Add(RightType.arChangeStructure, new RightInfo(ActionType.Edit));
    rightsMap13.Add(RightType.arDeleteThisObject, new RightInfo(ActionType.Delete));
    rightsMap13.Add(RightType.arReadRights, new RightInfo(ActionType.GetAccess));
    rightsMap13.Add(RightType.arChangeRights, new RightInfo(ActionType.SetAccess));
    rightsMap13.Add(RightType.Default, (RightInfo) null);
    this.Add(BOType.Scheme, rightsMap13);
  }
}
