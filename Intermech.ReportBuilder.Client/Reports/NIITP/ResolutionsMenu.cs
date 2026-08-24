// Decompiled with JetBrains decompiler
// Type: Intermech.ReportBuilder.Client.Reports.NIITP.ResolutionsMenu
// Assembly: Intermech.ReportBuilder.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 84A30C1D-3856-44D0-92A6-A87D49736592
// Assembly location: D:\IPS\Client\Intermech.ReportBuilder.Client.dll

using Intermech.DataFormats;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Document;
using Intermech.Navigator;
using Intermech.Navigator.ContextMenu;
using Intermech.Navigator.Interfaces;
using System;
using System.IO;
using System.Windows.Forms;

#nullable disable
namespace Intermech.ReportBuilder.Client.Reports.NIITP;

internal class ResolutionsMenu : IMenuScript
{
  private static long _reportTemplateID = -1;

  public string CommandName => "NIITP_Resolutions";

  public string CommandText => "Отчет по поручениям для НИИТП";

  public ClickEventHandler Target => new ClickEventHandler(ResolutionsMenu.TargetMethod);

  public bool Visible(IUserSession session, ISelectedItems items, IServiceProvider viewServices)
  {
    return Intermech.ReportBuilder.Client.Helper.ObjectsInTypes(items, MetaDataHelper.GetObjectTypeChildrenIDRecursive(new Guid("cad00070-306c-11d8-b4e9-00304f19f545")));
  }

  private static void TargetMethod(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    long objectID = -1;
    if (ResolutionsMenu._reportTemplateID == -1L)
    {
      if (SelectionWindow.Select("Выберите шаблон отчета", (IDescriptor) new Intermech.Navigator.DBObjectTypes.Descriptor(MetaDataHelper.GetObjectTypeID(new Guid("cad00134-306c-11d8-b4e9-00304f19f545"))), typeof (IDBObjectID), SelectionOptions.Default | SelectionOptions.DisableSelectAbstractTypes | SelectionOptions.DisableMultiselect) is IDBObjectID[] dbObjectIdArray && dbObjectIdArray.Length == 1)
      {
        objectID = dbObjectIdArray[0].Value;
        if (MessageBox.Show("Сохранить выбор шаблона в памяти, чтобы в дальнейшем не выбирать его? Если Да, то изменить выбор можно будет только после перезагрузки клиента IPS.", "Сохранение выбора", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
          ResolutionsMenu._reportTemplateID = objectID;
      }
    }
    else
      objectID = ResolutionsMenu._reportTemplateID;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBObject templateObject = sessionKeeper.Session.GetObject(objectID);
      ImDocumentData template = (ImDocumentData) null;
      int fileAttributeId = sessionKeeper.Session.IdentHelper.FileAttributeID;
      Stream stream = Intermech.ReportBuilder.Client.Helper.LoadXMLFromObject(templateObject, fileAttributeId);
      if (stream != null)
        template = ImDocumentData.LoadFromXml(stream);
      ImDocumentData documentFromTemplate = ImDocumentData.CreateDocumentFromTemplate(template);
      new Resolutions().Execute(sessionKeeper.Session, documentFromTemplate, Intermech.ReportBuilder.Client.Helper.ConvertToInt64(items));
      if (Intermech.ReportBuilder.Client.Helper.SaveToObjectDocument(sessionKeeper.Session, documentFromTemplate, MetaDataHelper.GetObjectTypeID(new Guid("cad00293-306c-11d8-b4e9-00304f19f545"))) == 0L)
        return;
      int num = (int) MessageBox.Show("Отчет сформирован успешно!");
    }
  }
}
