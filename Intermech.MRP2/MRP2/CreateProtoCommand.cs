// Decompiled with JetBrains decompiler
// Type: Intermech.MRP2.CreateProtoCommand
// Assembly: Intermech.MRP2, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: C0BCFFEE-338E-4233-ADA0-6E6F7936896C
// Assembly location: D:\IPS\Client\Intermech.MRP2.dll
// XML documentation location: D:\IPS\Client\Intermech.MRP2.xml

using Intermech.Client.Core.Navigator.Classes.ObjectNode;
using Intermech.DataFormats;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Navigator.Interfaces;
using Intermech.PropertyEditors;
using System;
using System.Windows.Forms;

#nullable disable
namespace Intermech.MRP2;

internal class CreateProtoCommand
{
  private static int _objTypeListCount;

  internal static void OnCreateNewObject(object sender, AfterObjectCreatedEventArgs e)
  {
    BaseHolder.NotificationService.FireEvent((object) null, (NotificationEventArgs) new DBObjectsEventArgs("ObjectsCreated", e.ObjectID, e.ObjectTypeID));
  }

  internal static void Handler(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    if (items == null || items.Count == 0)
      return;
    IObjectCreatorService service = ServicesManager.GetService(typeof (IObjectCreatorService)) as IObjectCreatorService;
    service.AfterObjectCreatedEvent += new AfterObjectCreatedEventHandler(CreateProtoCommand.OnCreateNewObject);
    try
    {
      long aTemplateObjectID = (items.GetItemData(0, typeof (IDBObjectID)) as IDBObjectID).Value;
      if (CreateProtoCommand._objTypeListCount == 0)
        CreateProtoCommand._objTypeListCount = MetaDataHelper.GetObjectTypeChildrenIDRecursive(MRP2Consts.objtypeIdProductionLists).Count;
      long newObjectID = 0;
      if (CreateProtoCommand._objTypeListCount == 1)
      {
        newObjectID = service.CreateObjectByTemplateDialog(aTemplateObjectID);
      }
      else
      {
        using (SelectorForm selectorForm = new SelectorForm("Выберите тип производственной ведомости", 4, false))
        {
          selectorForm.SelectorFilter = (ISelectorFilter) new TypeSelectorFilter(new int[1]
          {
            MRP2Consts.objtypeIdProductionLists
          }, true, true);
          selectorForm.NodeSelectorFilter = (INodeSelectorFilter) new NodeSelectorFilter();
          if (selectorForm.ShowDialog() != DialogResult.OK || selectorForm.IDList.Count != 1)
            return;
          int id = (int) selectorForm.IDList[0];
          OpenEditorMode openEditor = OpenEditorMode.None;
          newObjectID = service.CreateObjectByTypeDialog(id, aTemplateObjectID, (ObjectRelationLink[]) null, DateTime.Now, false, ref openEditor, (IObjectCreatorParams) null);
        }
      }
      AfterObjectCreatorDialogHandlers.Handle(newObjectID, 0, items, viewServices, additionalInfo);
    }
    finally
    {
      service.AfterObjectCreatedEvent -= new AfterObjectCreatedEventHandler(CreateProtoCommand.OnCreateNewObject);
    }
  }
}
