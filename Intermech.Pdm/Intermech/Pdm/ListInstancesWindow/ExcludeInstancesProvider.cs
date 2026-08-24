// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.ListInstancesWindow.ExcludeInstancesProvider
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.DataFormats;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Pdm;
using Intermech.Navigator.ContextMenu;
using Intermech.Navigator.Interfaces;
using System;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Pdm.ListInstancesWindow;

internal class ExcludeInstancesProvider : ICommandsProvider
{
  public CommandsInfo GetMergedCommands(ISelectedItems items, IServiceProvider viewServices)
  {
    return CommandsInfo.Empty;
  }

  public CommandsInfo GetGroupCommands(ISelectedItems items, IServiceProvider viewServices)
  {
    CommandsInfo groupCommands = new CommandsInfo();
    if (items.Count == 1)
    {
      IListInstancesInfo itemData = items.GetItemData(0, typeof (IListInstancesInfo)) as IListInstancesInfo;
      if (items.GetParentPath(0).RootDescriptor.GetType().Name == "ListInstancesDescriptor" && itemData == null)
        groupCommands.Add("PDM.Exclude", new CommandInfo(0, new ClickEventHandler(this.ExcludeInstance)));
    }
    return groupCommands;
  }

  private void ExcludeInstance(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    if (MessageBox.Show("Исключить исполнение?", "Исключение исполнения.", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
      return;
    long num = (items.GetItemData(0, typeof (IDBObjectID)) as IDBObjectID).Value;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBObject dbObject = sessionKeeper.Session.GetObject(num);
      IDBAttribute attributeByGuid = dbObject.GetAttributeByGuid(new Guid("cad001f9-306c-11d8-b4e9-00304f19f545"), false);
      if (attributeByGuid == null)
        throw new Exception("Невозможно удалить выбранное исполнение: Атрибут 'Идентификатор группового изделия' отсутствует у выбранного объекта.");
      if (PDMHelper.Validation3DModelInComposition(sessionKeeper.Session, num))
        throw new Exception("Исполнения изделия должны удаляться в электронной моделе!");
      if (PDMHelper.ValidationSpecificationInComposition(sessionKeeper.Session, num))
        throw new Exception("Нельзя удалить исполнение так как оно создано по спецификации!");
      if (attributeByGuid.AsString == string.Empty)
        throw new KernelException("Невозможно исключить исполнение т.к. атрибут 'Идентификатор группового изделия' пустой");
      try
      {
        attributeByGuid.AsString = string.Empty;
        DBObjectsEventArgs e = new DBObjectsEventArgs("ObjectsChanged", dbObject.ObjectID);
        if (!(ServicesManager.GetService(typeof (INotificationService)) is INotificationService service))
          return;
        service.FireEvent((object) null, (NotificationEventArgs) e);
      }
      catch (Exception ex)
      {
        throw new KernelException(ex.Message, ex.InnerException);
      }
    }
  }
}
