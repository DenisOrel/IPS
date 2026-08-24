// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.SearchScheme.SearchSchemeCreatorForm
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces.Client;
using Intermech.PropertyEditors;
using System;

#nullable disable
namespace Intermech.Pdm.SearchScheme;

internal class SearchSchemeCreatorForm : IObjectCreatorCustomService
{
  public long CreateObjectDialog(
    int ObjectTypeID,
    long TemplateObjectID,
    int[] RelationTypeIDs,
    long[] RelatedObjectIDs,
    DateTime StartDate,
    bool isVersion)
  {
    return SearchSchemeEditor.Execute(ObjectTypeID, TemplateObjectID);
  }

  public static void Attach(IObjectCreatorService service)
  {
    service.RegisterCreatorCustomService(ObjectTypesHelper.GetObjTypeID("cad0012b-306c-11d8-b4e9-00304f19f545"), typeof (SearchSchemeCreatorForm));
    service.RegisterCreatorCustomService(ObjectTypesHelper.GetObjTypeID("cad0012a-306c-11d8-b4e9-00304f19f545"), typeof (SearchSchemeCreatorForm));
  }

  public static void Detach(IObjectCreatorService service)
  {
    service.UnregisterCreatorCustomService(ObjectTypesHelper.GetObjTypeID("cad0012b-306c-11d8-b4e9-00304f19f545"), typeof (SearchSchemeCreatorForm));
    service.UnregisterCreatorCustomService(ObjectTypesHelper.GetObjTypeID("cad0012a-306c-11d8-b4e9-00304f19f545"), typeof (SearchSchemeCreatorForm));
  }
}
