// Decompiled with JetBrains decompiler
// Type: Intermech.Office.Client.EmailMessagesCommands
// Assembly: Intermech.Office.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 87EC380F-A344-4B99-B3CC-05ECB303FAD4
// Assembly location: D:\IPS\Client\Intermech.Office.Client.dll

using ImSSP;
using Intermech.Controls;
using Intermech.DataFormats;
using Intermech.Diagnostics;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Configuration;
using Intermech.Kernel.Search;
using Intermech.Navigator;
using Intermech.Navigator.ContextMenu;
using Intermech.Navigator.DBObjects;
using Intermech.Navigator.Interfaces;
using Intermech.Office.Interfaces;
using Intermech.Workflow;
using Intermech.Workflow.Design;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Windows.Forms;
using System.Xml;

#nullable disable
namespace Intermech.Office.Client;

internal class EmailMessagesCommands : ICommandsProvider
{
  [CanBeNull]
  private EmailMessagesCommands.RegisterMailData _mailData;
  [NotNull]
  private const string ConfigSelectedObjectType = "registration_selectedObjectType";
  [NotNull]
  private const string ConfigSectionName = "Office.Client";
  [NotNull]
  private string _addMessage = string.Empty;
  private string addMessage = string.Empty;

  [NotNull]
  public CommandsInfo GetMergedCommands(ISelectedItems items, IServiceProvider viewServices)
  {
    CommandsInfo mergedCommands = new CommandsInfo();
    mergedCommands.Add(OfficeClientConsts.CmdRegisterAttachments, new CommandInfo(0, new ClickEventHandler(this.RegisterAttachments)));
    mergedCommands.Add(OfficeClientConsts.CmdConvertToInternalMessage, new CommandInfo(0, new ClickEventHandler(this.ConvertToInternalMessage)));
    mergedCommands.Add("Office.ImportFromSMDO", new CommandInfo(0, new ClickEventHandler(this.ImportFromSMDO)));
    return mergedCommands;
  }

  public CommandsInfo GetGroupCommands(ISelectedItems items, IServiceProvider viewServices)
  {
    return CommandsInfo.Empty;
  }

  private static long GetInReplyToObjectID([NotNull] IUserSession session, [NotNull] IDBObject message)
  {
    IDBObjectCollection objectCollection = session.GetObjectCollection(OfficeConsts.ObjtypeDocumentsID);
    IDBAttribute attributeById = message.GetAttributeByID(OfficeConsts.AttrInReplyToID);
    if (attributeById != null && attributeById.AsString != string.Empty)
    {
      DataTable dataTable = objectCollection.SelectWithLocalObjects(new DBRecordSetParams(new ConditionStructure[1]
      {
        new ConditionStructure(OfficeConsts.AttrMessageIdentityID, RelationalOperators.Equal, (object) attributeById.AsString, LogicalOperators.AND, 0, true)
      }, new object[1]{ (object) -2 }));
      if (dataTable.Rows.Count == 1)
        return Convert.ToInt64(dataTable.Rows[0][0]);
    }
    return 0;
  }

  private static int GetFileIndex([CanBeNull] IDBAttribute attrFile)
  {
    int fileIndex = -1;
    if (attrFile != null && attrFile.ValuesCount > 0)
    {
      if (attrFile.ValuesCount > 1)
      {
        List<string> captions = new List<string>(attrFile.ValuesCount);
        List<object> values = new List<object>(attrFile.ValuesCount);
        for (int index = 0; index < attrFile.ValuesCount; ++index)
        {
          attrFile.Index = index;
          captions.Add(attrFile.AsString);
          values.Add((object) index);
        }
        ChoiceForm choiceForm = new ChoiceForm();
        choiceForm.Init(Localization.GetString("Office.Client_34"), captions, values);
        if (choiceForm.ShowDialog() == DialogResult.OK && choiceForm.SelectedValue != null)
          fileIndex = (int) choiceForm.SelectedValue;
      }
      else
        fileIndex = 0;
    }
    return fileIndex;
  }

  private void ChangeFile(
    [CanBeNull] IDBAttribute attrFileSource,
    [NotNull] IDBAttribute attrFileDest,
    long ID,
    [NotNull] string fileName,
    [NotNull] string note)
  {
    this.ChangeFile(attrFileSource, (byte[]) null, attrFileDest, ID, fileName, note);
  }

  private void ChangeFile(
    [CanBeNull] byte[] ba,
    [NotNull] IDBAttribute attrFileDest,
    long ID,
    [NotNull] string fileName,
    [NotNull] string note)
  {
    this.ChangeFile((IDBAttribute) null, ba, attrFileDest, ID, fileName, note);
  }

  private void ChangeFile(
    [CanBeNull] IDBAttribute attrFileSource,
    [CanBeNull] byte[] ba,
    [NotNull] IDBAttribute attrFileDest,
    long ID,
    [NotNull] string fileName,
    [NotNull] string note)
  {
    BlobInformation blobInfo;
    if (attrFileSource is IBlobReader blobReader)
    {
      blobInfo = blobReader.OpenBlob(0);
    }
    else
    {
      Intermech.Diagnostics.Check.NotNull<byte[]>(ba, nameof (ba));
      blobInfo = new BlobInformation();
      blobInfo.ArcMethod = ArcMethods.NotPacked;
      blobInfo.RealFileSize = (long) ba.Length;
      blobInfo.PackedFileSize = (long) ba.Length;
    }
    try
    {
      IBlobWriter blobWriter = attrFileDest as IBlobWriter;
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        IFileNamesService service = ServiceUtils.GetService<IFileNamesService>((object) sessionKeeper.Session, true);
        blobInfo.FileName = service.GetUniqueFileName(fileName, ID, sessionKeeper.Session.SessionGUID);
      }
      blobInfo.Note = note;
      blobInfo.ModifyDate = DateTime.Now;
      blobWriter.OpenBlob(blobInfo, false);
      if (attrFileSource != null)
      {
        if (blobInfo.RealFileSize <= 0L)
          return;
        blobWriter.WriteDataBlock(Intermech.Diagnostics.Check.NotNull<IBlobReader>(blobReader, "br").ReadDataBlock());
      }
      else
        blobWriter.WriteDataBlock(ba);
    }
    finally
    {
      blobReader?.CloseBlob();
    }
  }

  private static int SelectedTypeFromConfig
  {
    get
    {
      if (Holder.ConfigurationManager != null)
      {
        IConfiguration configuration = Holder.ConfigurationManager.Open("Office.Client");
        if (configuration != null)
        {
          string property = configuration.GetProperty("registration_selectedObjectType");
          if (property != string.Empty)
            return Convert.ToInt32(property);
        }
      }
      return -1;
    }
    set
    {
      if (Holder.ConfigurationManager == null)
        return;
      (Holder.ConfigurationManager.Open("Office.Client") ?? Holder.ConfigurationManager.Create("Office.Client")).SetProperty("registration_selectedObjectType", value.ToString());
    }
  }

  private long CreateNewOfficeDocument([NotNull] IUserSession session)
  {
    Holder.ObjectCreatorService.AfterDraftCreatedEvent += new AfterDraftCreatedEventHandler(this.cDlg_ObjectCreatorDraftCreatedEvent);
    try
    {
      List<int> intList = new List<int>();
      foreach (DataRow row in (InternalDataCollectionBase) session.GetObjectTypeCollection(OfficeConsts.ObjtypeDocumentsID, true).SelectRecursive(string.Empty).Rows)
      {
        if (Convert.ToInt32(row["F_VERSIONABLE"]) != 0)
          intList.Add(Convert.ToInt32(row["F_OBJECT_TYPE"]));
      }
      long objectByTypeDialog;
      if (Intermech.Diagnostics.Check.NotNull<EmailMessagesCommands.RegisterMailData>(this._mailData, "_mailData").InReplyToID == 0L)
        objectByTypeDialog = Holder.ObjectCreatorService.CreateObjectByTypeDialog(intList.ToArray(), (ObjectRelationLink[]) null, EmailMessagesCommands.SelectedTypeFromConfig);
      else
        objectByTypeDialog = Holder.ObjectCreatorService.CreateObjectByTypeDialog(intList.ToArray(), new ObjectRelationLink[1]
        {
          new ObjectRelationLink(this._mailData.InReplyToID, OfficeConsts.ReltypeAnswerID)
        }, EmailMessagesCommands.SelectedTypeFromConfig);
      long objectID = objectByTypeDialog;
      if (objectID == -1L)
        return 0;
      EmailMessagesCommands.SelectedTypeFromConfig = session.GetObjectInfo(objectID).ObjectTypeID;
      return objectID;
    }
    finally
    {
      Holder.ObjectCreatorService.AfterDraftCreatedEvent -= new AfterDraftCreatedEventHandler(this.cDlg_ObjectCreatorDraftCreatedEvent);
      this._mailData = (EmailMessagesCommands.RegisterMailData) null;
    }
  }

  private static long GetObjectFromEmail(
    [NotNull] IUserSession session,
    int objectTypeID,
    [NotNull] string email,
    out long id)
  {
    DataTable dataTable = session.GetObjectCollection(objectTypeID).SelectWithLocalObjects(new DBRecordSetParams(new ConditionStructure[1]
    {
      new ConditionStructure(OfficeConsts.AttrEmailAddressID, RelationalOperators.Equal, (object) email, LogicalOperators.AND, 0, true)
    }, new object[2]{ (object) -2, (object) -3 }));
    if (dataTable.Rows.Count == 1)
    {
      id = Convert.ToInt64(dataTable.Rows[0][1]);
      return Convert.ToInt64(dataTable.Rows[0][0]);
    }
    id = 0L;
    return 0;
  }

  private static long GetOrganization([NotNull] IUserSession session, [NotNull] string email, out long id)
  {
    return EmailMessagesCommands.GetObjectFromEmail(session, OfficeConsts.ObjtypeOrganizationUnitsID, email, out id);
  }

  private static long GetUser([NotNull] IUserSession session, [NotNull] string email, out long id)
  {
    return EmailMessagesCommands.GetObjectFromEmail(session, OfficeConsts.ObjtypeUsersID, email, out id);
  }

  [NotNull]
  private static string TrimFileName([NotNull] string fileName, Guid messageGuid)
  {
    if (fileName.IndexOf(messageGuid.ToString(), StringComparison.OrdinalIgnoreCase) == 0)
    {
      fileName = fileName.Remove(0, messageGuid.ToString().Length);
      if (fileName.Length > 0 && (int) fileName[0] == (int) Path.DirectorySeparatorChar)
        fileName = fileName.Remove(0, 1);
    }
    return fileName;
  }

  private long RegisterFile([NotNull] IUserSession session, long messageID, long inReplyToID, int fileIndex)
  {
    IDBObjectCollection objectCollection = session.GetObjectCollection(OfficeConsts.ObjtypeDocumentsID);
    IDBObject iDbAttributable1 = session.GetObject(messageID);
    IDBAttribute dbAttribute = iDbAttributable1.AttributeByID(OfficeConsts.AttrFileID);
    dbAttribute.Index = fileIndex;
    long num1 = 0;
    IBlobReader blobReader = Intermech.Diagnostics.Check.NotNull<IBlobReader>(dbAttribute as IBlobReader, "attrFile as IBlobReader");
    BlobInformation blobInformation = blobReader.OpenBlob(0);
    string note;
    string fileName;
    try
    {
      note = blobInformation.Note;
      fileName = blobInformation.FileName;
    }
    finally
    {
      blobReader.CloseBlob();
    }
    string str = EmailMessagesCommands.TrimFileName(fileName, iDbAttributable1.ObjectGUID);
    ConditionStructure[] conditions = new ConditionStructure[3]
    {
      new ConditionStructure(OfficeConsts.AttrRegNumberID, RelationalOperators.AttributeExists, (object) null, LogicalOperators.AND, 0, true),
      new ConditionStructure(OfficeConsts.AttrRegNumberID, RelationalOperators.NotEmpty, (object) null, LogicalOperators.AND, 0, true),
      new ConditionStructure(OfficeConsts.AttrFileID, RelationalOperators.Substring, (object) str, (object) null, LogicalOperators.AND, 0, false, AttributeSourceTypes.Object, ColumnContents.String)
    };
    DataTable dataTable = objectCollection.SelectWithLocalObjects(new DBRecordSetParams(conditions, new object[1]
    {
      (object) -2
    }));
    if (dataTable.Rows.Count > 0)
    {
      long num2 = 0;
      DialogResult dialogResult;
      if (dataTable.Rows.Count == 1)
      {
        IDBObject dbObject = session.GetObject(Convert.ToInt64(dataTable.Rows[0][0]));
        num2 = dbObject.ObjectID;
        dialogResult = IMMessageBox.Show(Localization.GetString("Office.Client_35"), string.Format($"{Localization.GetString("Office.Client_36")}\n{Localization.GetString("Office.Client_37")}", (object) dbObject.NameInMessages, (object) str), new IMMessageBoxButton[4]
        {
          new IMMessageBoxButton(Localization.GetString("Office.Client_38"), DialogResult.OK),
          new IMMessageBoxButton(Localization.GetString("Office.Client_39"), DialogResult.Abort),
          new IMMessageBoxButton(Localization.GetString("Office.Client_40"), DialogResult.Ignore),
          new IMMessageBoxButton(Localization.GetString("Office.Client_41"), DialogResult.Cancel)
        }, IMMessageBoxImage.Question);
      }
      else
      {
        dialogResult = IMMessageBox.Show(Localization.GetString("Office.Client_35"), string.Format($"{Localization.GetString("Office.Client_42")}\n{Localization.GetString("Office.Client_43")}", (object) dataTable.Rows.Count, (object) str), new IMMessageBoxButton[4]
        {
          new IMMessageBoxButton(Localization.GetString("Office.Client_38"), DialogResult.OK),
          new IMMessageBoxButton(Localization.GetString("Office.Client_39"), DialogResult.Abort),
          new IMMessageBoxButton(Localization.GetString("Office.Client_40"), DialogResult.Ignore),
          new IMMessageBoxButton(Localization.GetString("Office.Client_41"), DialogResult.Cancel)
        }, IMMessageBoxImage.Question);
        switch (dialogResult)
        {
          case DialogResult.OK:
          case DialogResult.Abort:
            List<long> objectIDs = new List<long>(dataTable.Rows.Count);
            for (int index = 0; index < dataTable.Rows.Count; ++index)
              objectIDs.Add(Convert.ToInt64(dataTable.Rows[index][0]));
            object[] objArray = SelectionWindow.Select(Localization.GetString("Office.Client_44"), Localization.GetString("Office.Client_45", dialogResult == DialogResult.OK ? (object) Localization.GetString("Office.Client_46") : (object) Localization.GetString("Office.Client_47")), (IDescriptor) new ListDescriptor(Intermech.Navigator.Consts.CategoryVersionsObjectNode, -1, Localization.GetString("Office.Client_78"), (IList) objectIDs), typeof (IDBTypedObjectID), SelectionOptions.HideTree | SelectionOptions.HideViewsToolbar | SelectionOptions.HideViewsGroupingBox | SelectionOptions.SelectObjects | SelectionOptions.DisableObjectListFilter | SelectionOptions.DisableMultiselect | SelectionOptions.ForceRebuildNavTree);
            if (objArray == null || objArray.Length != 1)
              return 0;
            num2 = ((IDBTypedObjectID) objArray[0]).ObjectID;
            break;
          case DialogResult.Cancel:
            return 0;
        }
      }
      IDBAttribute attrFileSource = session.GetObject(messageID).AttributeByID(OfficeConsts.AttrFileID);
      attrFileSource.Index = fileIndex;
      switch (dialogResult - 1)
      {
        case DialogResult.None:
          IDBObject iDbAttributable2 = session.GetObject(num2);
          IDBAttribute attrFileDest = iDbAttributable2.AttributeByID(OfficeConsts.AttrFileID);
          bool flag = false;
          for (int index = 0; index < attrFileDest.ValuesCount; ++index)
          {
            dbAttribute.Index = index;
            if (dbAttribute.AsString.Contains(str))
            {
              this.ChangeFile(attrFileSource, attrFileDest, iDbAttributable2.ID, Path.Combine(iDbAttributable2.ObjectGUID.ToString(), str), note);
              flag = true;
              break;
            }
          }
          if (!flag)
          {
            int num3 = (int) IMMessageBox.Show(Localization.GetString(sc_15046.ssp_office_15047()), Localization.GetString("Office.Client_49"), MessageBoxButtons.OK, IMMessageBoxImage.Warning);
            return 0;
          }
          num1 = iDbAttributable2.ObjectID;
          break;
        case DialogResult.OK:
          return 0;
        case DialogResult.Cancel:
          this._mailData = new EmailMessagesCommands.RegisterMailData(messageID, fileIndex, str, note, inReplyToID);
          Holder.ObjectCreatorService.AfterDraftCreatedEvent += new AfterDraftCreatedEventHandler(this.cDlg_ObjectCreatorDraftCreatedEvent);
          try
          {
            num1 = Holder.ObjectCreatorService.CreateObjectVersionByTemplateDialog(num2);
            break;
          }
          finally
          {
            Holder.ObjectCreatorService.AfterDraftCreatedEvent -= new AfterDraftCreatedEventHandler(this.cDlg_ObjectCreatorDraftCreatedEvent);
            this._mailData = (EmailMessagesCommands.RegisterMailData) null;
          }
        case DialogResult.Retry:
          FileRenameForm fileRenameForm = new FileRenameForm(str, note);
          if (fileRenameForm.ShowDialog() == DialogResult.OK)
          {
            this._mailData = new EmailMessagesCommands.RegisterMailData(messageID, fileIndex, fileRenameForm.NewFileName, fileRenameForm.NewNote, inReplyToID);
            num1 = this.CreateNewOfficeDocument(session);
            break;
          }
          break;
      }
    }
    else
    {
      this._mailData = new EmailMessagesCommands.RegisterMailData(messageID, fileIndex, str, note, inReplyToID);
      num1 = this.CreateNewOfficeDocument(session);
    }
    return num1;
  }

  protected void RegisterAttachments(
    [NotNull] ISelectedItems items,
    [CanBeNull] IServiceProvider viewServices,
    [CanBeNull] object additionalInfo)
  {
    IDBTypedObjectID itemData = items.GetItemData<IDBTypedObjectID>(0);
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBObject message = sessionKeeper.Session.GetObject(itemData.ObjectID);
      long inReplyToObjectId = EmailMessagesCommands.GetInReplyToObjectID(sessionKeeper.Session, message);
      int fileIndex = EmailMessagesCommands.GetFileIndex(message.GetAttributeByID(OfficeConsts.AttrFileID));
      if (fileIndex != -1)
      {
        this.RegisterFile(sessionKeeper.Session, itemData.ObjectID, inReplyToObjectId, fileIndex);
      }
      else
      {
        this._mailData = new EmailMessagesCommands.RegisterMailData(itemData.ObjectID, fileIndex, string.Empty, string.Empty, inReplyToObjectId);
        this.CreateNewOfficeDocument(sessionKeeper.Session);
      }
    }
  }

  private void cDlg_ObjectCreatorDraftCreatedEvent([CanBeNull] object sender, [NotNull] AfterDraftCreatedEventArgs e)
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      if (MetaDataHelper.GetObjectTypeChildrenIDRecursive(OfficeConsts.ObjtypeOfficeDocumentsID).IndexOf(e.ObjectTypeID) < 0)
      {
        Intermech.Diagnostics.Check.NotNull<EmailMessagesCommands.RegisterMailData>(this._mailData, "_mailData");
        if (!OfficeClientHelper.RegisterNewDocument(sessionKeeper.Session, e.ObjectID, e.ObjectTypeID, new OfficeDocumentTypes[2]
        {
          OfficeDocumentTypes.Incoming,
          OfficeDocumentTypes.Internal
        }, this._mailData.InReplyToID))
          return;
      }
      IDBObject iDbAttributable1 = sessionKeeper.Session.GetObject(e.ObjectID);
      Intermech.Diagnostics.Check.NotNull<EmailMessagesCommands.RegisterMailData>(this._mailData, "_mailData");
      IDBObject iDbAttributable2 = sessionKeeper.Session.GetObject(this._mailData.MessageID);
      if (!string.IsNullOrEmpty(iDbAttributable2.Caption))
      {
        OfficeGeneralSettings settings = sessionKeeper.Session.GetCustomService<IOfficeGeneralSettingsService>().Settings;
        if (settings.CaptionAttributeForEmailMessages != 0)
          (iDbAttributable1.GetAttributeByID(settings.CaptionAttributeForEmailMessages) ?? iDbAttributable1.Attributes.AddAttribute(settings.CaptionAttributeForEmailMessages, false)).Value = (object) iDbAttributable2.Caption;
        else
          iDbAttributable1.Caption = iDbAttributable2.Caption;
      }
      IDBAttribute dbAttribute1 = iDbAttributable2.AttributeByID(OfficeConsts.AttrEmailSenderID);
      IDBAttribute dbAttribute2 = iDbAttributable2.AttributeByID(OfficeConsts.AttrEmailAddressID);
      switch ((OfficeDocumentTypes) iDbAttributable1.AttributeByID(OfficeConsts.AttrOfficeDocumentTypeID).AsInteger)
      {
        case OfficeDocumentTypes.Incoming:
          long id;
          long organization = EmailMessagesCommands.GetOrganization(sessionKeeper.Session, dbAttribute1.AsString, out id);
          if (organization != 0L)
          {
            iDbAttributable1.Attributes.AddAttribute(OfficeConsts.AttrAddresserID, false, new object[1]
            {
              (object) organization
            });
          }
          else
          {
            long user = EmailMessagesCommands.GetUser(sessionKeeper.Session, dbAttribute1.AsString, out id);
            if (user != 0L)
            {
              long userUnit = sessionKeeper.Session.GetCustomService<IOfficeRegistrationService>().GetUserUnit(user);
              if (userUnit != 0L)
              {
                iDbAttributable1.Attributes.AddAttribute(OfficeConsts.AttrAddresserID, false, new object[1]
                {
                  (object) userUnit
                });
                IDBAttribute dbAttribute3 = sessionKeeper.Session.GetObject(user).AttributeByID(OfficeConsts.AttrUserNameID);
                iDbAttributable1.Attributes.AddAttribute(OfficeConsts.AttrSignatoryID, false, new object[1]
                {
                  (object) dbAttribute3.AsString
                });
              }
            }
          }
          long user1 = EmailMessagesCommands.GetUser(sessionKeeper.Session, dbAttribute2.AsString, out id);
          if (user1 != 0L)
          {
            iDbAttributable1.Attributes.AddAttribute(OfficeConsts.AttrAddresseesID, false, new object[1]
            {
              (object) user1
            });
            break;
          }
          break;
        case OfficeDocumentTypes.Internal:
          long user2 = EmailMessagesCommands.GetUser(sessionKeeper.Session, dbAttribute2.AsString, out long _);
          if (user2 != 0L)
          {
            iDbAttributable1.Attributes.AddAttribute(OfficeConsts.AttrAddresseesID, false, new object[1]
            {
              (object) user2
            });
            break;
          }
          break;
      }
      if (this._mailData.IndexFile == -1)
        return;
      IDBAttribute attrFileDest = iDbAttributable1.GetAttributeByID(OfficeConsts.AttrFileID) ?? iDbAttributable1.Attributes.AddAttribute(OfficeConsts.AttrFileID, false);
      if (!attrFileDest.IsNull && attrFileDest.ValuesCount > 0)
      {
        int num = -1;
        for (int index = 0; index < attrFileDest.ValuesCount; ++index)
        {
          attrFileDest.Index = index;
          if (attrFileDest.AsString.Contains(this._mailData.FileName))
          {
            num = index;
            break;
          }
        }
        if (num == -1)
          attrFileDest.AddValue((object) null);
      }
      IDBAttribute attrFileSource = iDbAttributable2.AttributeByID(OfficeConsts.AttrFileID);
      attrFileSource.Index = this._mailData.IndexFile;
      this.ChangeFile(attrFileSource, attrFileDest, iDbAttributable1.ID, Path.Combine(iDbAttributable1.ObjectGUID.ToString(), this._mailData.FileName), this._mailData.Note);
    }
  }

  protected void ConvertToInternalMessage(
    [NotNull] ISelectedItems items,
    [CanBeNull] IServiceProvider viewServices,
    [CanBeNull] object additionalInfo)
  {
    IDBTypedObjectID itemData = items.GetItemData<IDBTypedObjectID>(0);
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBObject message = sessionKeeper.Session.GetObject(itemData.ObjectID);
      long inReplyToObjectId = EmailMessagesCommands.GetInReplyToObjectID(sessionKeeper.Session, message);
      IDBAttribute byId = message.Attributes.FindByID(OfficeConsts.AttrFileID);
      AttachmentList attachs = new AttachmentList();
      bool flag = false;
      if (byId != null && !byId.IsNull && byId.ValuesCount > 0)
      {
        IDBTransactions customService = (IDBTransactions) sessionKeeper.Session.GetCustomService(typeof (IDBTransactions));
        customService.StartTransaction();
        try
        {
          for (int fileIndex = 0; fileIndex < byId.ValuesCount; ++fileIndex)
          {
            byId.Index = fileIndex;
            if (!byId.IsNull)
            {
              long ObjectID = this.RegisterFile(sessionKeeper.Session, message.ObjectID, inReplyToObjectId, fileIndex);
              if (ObjectID == 0L)
              {
                flag = true;
                break;
              }
              attachs.AddAttachment(ObjectID);
            }
          }
          if (flag)
            customService.Rollback();
          else
            customService.Commit();
        }
        catch
        {
          customService.Rollback();
          throw;
        }
      }
      if (flag)
        return;
      wfFunx.CreateProcess(0L, attachs, message.GetAttributeByID(OfficeConsts.AttrSubjectID).AsString, Convert.ToString(message.GetAttributeByID(OfficeConsts.AttrMessageID).Value));
    }
  }

  private void ImportFromSMDO(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    IDBTypedObjectID itemData = items.GetItemData(0, typeof (IDBTypedObjectID)) as IDBTypedObjectID;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBAttribute attributeById = sessionKeeper.Session.GetObject(itemData.ObjectID).GetAttributeByID(OfficeConsts.AttrFileID);
      List<string> source = new List<string>(attributeById.ValuesCount);
      if (attributeById.ValuesCount > 0)
      {
        for (int index = 0; index < attributeById.ValuesCount; ++index)
        {
          attributeById.Index = index;
          source.Add(attributeById.AsString);
        }
        attributeById.Index = 0;
      }
      string str1 = source.FirstOrDefault<string>((System.Func<string, bool>) (x => x.EndsWith(".xml")));
      int aIndex = str1 != null ? source.IndexOf(str1) : throw new KernelException("Не найден xml файл для обработки СМДО. Проверьте правильность файлов на закладке 'Файлы'.");
      using (MemoryStream memoryStream1 = new MemoryStream())
      {
        new BlobProcReader(itemData.ObjectID, AttributableElements.Object, attributeById.AttributeID, aIndex, 0, (Stream) memoryStream1, (BlobProcCustomClass.ProgressEventHandler) null, (BlobProcCustomClass.ThreadFinishEventHandler) null).ReadData();
        memoryStream1.Position = 0L;
        string path1 = $"{ClientContext.FileVault.WorkArea.AreaPath}\\smdo\\ERROR";
        string path2 = $"{ClientContext.FileVault.WorkArea.AreaPath}\\smdo\\REFUSAL";
        if (!Directory.Exists(path1))
          Directory.CreateDirectory(path1);
        if (!Directory.Exists(path2))
          Directory.CreateDirectory(path2);
        List<long> longList = new List<long>();
        Dictionary<string, string> dictionary1 = new Dictionary<string, string>();
        ISMDOSettingsService customService = (ISMDOSettingsService) sessionKeeper.Session.GetCustomService(typeof (ISMDOSettingsService));
        SMDOSettings settings = customService.Settings;
        if (string.IsNullOrEmpty(settings.SmdoEmail))
          throw new KernelException("Не задан e-mail адрес сервера СМДО в общих настройках Канцелярия/СМДО!");
        if (string.IsNullOrEmpty(settings.CompanySMDOid))
          throw new KernelException("Не задан идентификатор вашей организации в общих настройках Канцелярия/СМДО!");
        if (string.IsNullOrEmpty(settings.CompanyName))
          throw new KernelException("Не задано наименование вашей организации в общих настройках Канцелярия/СМДО!");
        if (string.IsNullOrEmpty(settings.SystemID))
          throw new KernelException("Не задан идентификатор(GUID) системы в справочнике СМДО в общих настройках Канцелярия/СМДО!");
        XmlDocument xmlDocument = new XmlDocument();
        try
        {
          xmlDocument.Load((Stream) memoryStream1);
        }
        catch (Exception ex)
        {
          throw new KernelException($"Ошибка открытия файла {str1}", ex);
        }
        if (xmlDocument.SelectSingleNode("/" + Tag.Envelop) == null)
          throw new KernelException($"Корневой элемент файла '{str1}' не соответствует формату СМДО. Возможно данное письмо не отправлено системой СМДО. Продолжение невозможно проверьте пакет на закладке 'Файлы'.");
        int num1 = OfficeClientConsts.TranslateSmdoVerToInt(xmlDocument.SelectSingleNode($"/{Tag.Envelop}/@{Tag.type}").Value);
        XmlNode xmlNode1 = xmlDocument.SelectSingleNode($"/{Tag.Envelop}/@{Tag.msg_id}");
        string empty1 = string.Empty;
        if (xmlNode1 != null)
          empty1 = xmlNode1.Value;
        XmlNode xmlNode2 = xmlDocument.SelectSingleNode($"/{Tag.Envelop}/@{Tag.dtstamp}");
        string empty2 = string.Empty;
        if (xmlNode2 != null)
        {
          string str2 = xmlNode2.Value;
        }
        XmlNode xmlNode3 = xmlDocument.SelectSingleNode($"/{Tag.Envelop}/@{Tag.subject}");
        string empty3 = string.Empty;
        if (xmlNode3 != null)
          empty3 = xmlNode3.Value;
        XmlNode xmlNode4 = xmlDocument.SelectSingleNode($"/{Tag.Envelop}/{Tag.Header}/@msg_type");
        if (xmlNode4 == null)
          throw new KernelException("Ошибка при разборе XML-пакета: структура XML не соответствует формату СМДО или была повреждена. Возможно открываемый файл не является файлом системы СМДО.");
        if (xmlNode4.Value == "1" || xmlNode4.Value == "3")
        {
          XmlNode xmlNode5 = xmlDocument.SelectSingleNode($"/{Tag.Envelop}/{Tag.Body}/{Tag.Document}/{Tag.RegNumber}");
          string regNum = string.Empty;
          if (xmlNode5 != null)
            regNum = xmlNode5.InnerText;
          XmlNode xmlNode6 = xmlDocument.SelectSingleNode($"/{Tag.Envelop}/{Tag.Body}/{Tag.Document}/{Tag.RegNumber}/@{Tag.regdate}");
          string empty4 = string.Empty;
          if (xmlNode6 != null)
            empty4 = xmlNode6.Value;
          XmlNode xmlNode7 = xmlDocument.SelectSingleNode($"/{Tag.Envelop}/{Tag.Header}/{Tag.Sender}/@{Tag.name}");
          XmlNode xmlNode8 = xmlDocument.SelectSingleNode($"/{Tag.Envelop}/{Tag.Header}/{Tag.Sender}/@{Tag.id}");
          xmlDocument.SelectNodes($"/{Tag.Envelop}/{Tag.Body}/{Tag.Document}");
          XmlNode xmlNode9 = xmlDocument.SelectSingleNode($"/{Tag.Envelop}/{Tag.Body}/{Tag.Document}/@{Tag.kind}");
          XmlNodeList xmlNodeList1 = xmlDocument.SelectNodes($"/{Tag.Envelop}/{Tag.Body}/{Tag.Document}/{Tag.DocTransfer}");
          string empty5 = string.Empty;
          string empty6 = string.Empty;
          bool flag = true;
          try
          {
            empty5 = xmlNode7.Value;
            empty6 = xmlNode8.Value;
            if (xmlNodeList1 != null && (num1 >= OfficeClientConsts.SmdoVer211Int ? (xmlNode9 != null ? 1 : 0) : 1) != 0 && xmlNode1 != null && xmlNode3 != null && xmlNode6 != null && xmlNode5 != null)
            {
              foreach (XmlNode xmlNode10 in xmlNodeList1)
              {
                string str3 = xmlNode10.Attributes[Tag.name].Value;
                XmlNode childNode = xmlNode10.ChildNodes[0];
                string msgAttachName = string.Empty;
                string s = string.Empty;
                if (childNode.Attributes[Tag.referenceid] != null)
                  msgAttachName = childNode.Attributes[Tag.referenceid].Value;
                else
                  s = childNode.InnerText;
                if (!string.IsNullOrEmpty(msgAttachName))
                {
                  if (!dictionary1.ContainsKey(msgAttachName))
                    dictionary1.Add(msgAttachName, str3);
                }
                else if (!dictionary1.ContainsKey("DATABYTES_" + s))
                  dictionary1.Add("DATABYTES_" + s, str3);
                XmlNodeList xmlNodeList2 = xmlNode10.SelectNodes(Tag.Signature);
                if (xmlNodeList2 == null || xmlNodeList2.Count == 0)
                {
                  flag = false;
                  this.GenerateAckXML(1, -22, settings, empty5, empty6, empty1, (string) null, (string) null, empty3, empty4, regNum, customService, $"Файл {str3}: ");
                  int num2 = (int) MessageBox.Show("Отсутствует ЭП для одного или нескольких документов");
                  break;
                }
                byte[] numArray = (byte[]) null;
                if (!string.IsNullOrEmpty(msgAttachName))
                {
                  using (MemoryStream memoryStream2 = new MemoryStream())
                  {
                    string str4 = source.FirstOrDefault<string>((System.Func<string, bool>) (x => x.EndsWith(msgAttachName)));
                    new BlobProcReader(itemData.ObjectID, AttributableElements.Object, attributeById.AttributeID, source.IndexOf(str4), 0, (Stream) memoryStream2, (BlobProcCustomClass.ProgressEventHandler) null, (BlobProcCustomClass.ThreadFinishEventHandler) null).ReadData();
                    memoryStream2.Position = 0L;
                    using (BinaryReader binaryReader = new BinaryReader((Stream) memoryStream2))
                      numArray = binaryReader.ReadBytes((int) memoryStream2.Length);
                  }
                }
                else
                  numArray = Convert.FromBase64String(s);
                foreach (XmlNode xmlNode11 in xmlNodeList2)
                {
                  int num3 = 0;
                  try
                  {
                    string innerText = xmlNode11.InnerText;
                    X509Certificate2 x509Certificate2 = (X509Certificate2) null;
                    byte[] messageData = numArray;
                    ref X509Certificate2 local1 = ref x509Certificate2;
                    ref int local2 = ref num3;
                    int num4 = Win32.CheckMessageSign(innerText, messageData, ref local1, out local2);
                    Dictionary<string, string> dictionary2 = (Dictionary<string, string>) null;
                    string empty7 = string.Empty;
                    string str5 = string.Empty;
                    if (x509Certificate2 != null)
                      dictionary2 = this.X509Parse(x509Certificate2.Subject);
                    string str6;
                    if (dictionary2 != null)
                    {
                      str6 = dictionary2["SN"];
                      str5 = dictionary2.ContainsKey("Отчество") ? dictionary2["Отчество"] : (dictionary2.ContainsKey("OID.2.5.4.41") ? dictionary2["OID.2.5.4.41"] : string.Empty);
                    }
                    else
                      str6 = "* недоступно из-за ошибки проверки подписи *";
                    this.addMessage = $"Файл {str3}: Владелец подписи: '{str6} {str5}': ";
                    if (num4 != 0)
                    {
                      flag = false;
                      if (num4 != -1)
                      {
                        switch (num4 - -4)
                        {
                          case 0:
                            this.addMessage = $"{this.addMessage}{"Ненадёжный корневой сертификат"}";
                            break;
                          case 1:
                            this.addMessage = $"{this.addMessage}{"Сертификат был отозван"}";
                            break;
                          case 2:
                            this.addMessage = $"{this.addMessage}{"Срок действия сертификата истёк"}";
                            break;
                        }
                        this.GenerateAckXML(1, -23, settings, empty5, empty6, empty1, (string) null, (string) null, empty3, empty4, regNum, customService, this.addMessage);
                        int num5 = (int) MessageBox.Show(this.addMessage);
                        break;
                      }
                      this.GenerateAckXML(1, -21, settings, empty5, empty6, empty1, (string) null, (string) null, empty3, empty4, regNum, customService, this.addMessage);
                      string str7 = string.Empty;
                      if (num3 != 0)
                        str7 = "\nКод ошибки WinAPI: 0x" + num3.ToString("X");
                      int num6 = (int) MessageBox.Show("ЭП не верна: нарушена целостность подписанного документа(ов) или не найден файл, на который существует ссылка." + str7);
                      break;
                    }
                  }
                  catch (Exception ex)
                  {
                    this.GenerateAckXML(1, -21, settings, empty5, empty6, empty1, (string) null, (string) null, empty3, empty4, regNum, customService, this.addMessage);
                    flag = false;
                    string str8 = string.Empty;
                    if (num3 != 0)
                      str8 = "\nКод ошибки WinAPI: 0x" + num3.ToString("X");
                    int num7 = (int) MessageBox.Show("Неверная структура ЭП." + str8);
                    break;
                  }
                }
                if (!flag)
                  break;
              }
              if (flag)
                this.GenerateAckXML(1, 0, settings, empty5, empty6, empty1, (string) null, (string) null, empty3, empty4, regNum, customService);
            }
            else
            {
              this.GenerateAckXML(1, -1, settings, empty5, empty6, empty1, (string) null, (string) null, empty3, empty4, regNum, customService);
              flag = false;
              int num8 = (int) MessageBox.Show("Ошибка при разборе XML-пакета: структура XML не соответствует формату СМДО или была повреждена");
            }
          }
          catch (Exception ex)
          {
            this.GenerateAckXML(1, -1, settings, empty5, empty6, empty1, (string) null, (string) null, empty3, empty4, regNum, customService);
            flag = false;
            int num9 = (int) MessageBox.Show(ex.Message);
          }
          if (!flag)
            return;
          string path3 = $"{ClientContext.FileVault.WorkArea.AreaPath}\\smdo\\IN";
          if (!Directory.Exists(path3))
            Directory.CreateDirectory(path3);
          foreach (KeyValuePair<string, string> keyValuePair in dictionary1)
          {
            KeyValuePair<string, string> msgAttachFile = keyValuePair;
            string path4 = $"{path3}\\{msgAttachFile.Value}";
            try
            {
              int num10 = -1;
              string fileDataBytes = (string) null;
              if (msgAttachFile.Key.StartsWith("DATABYTES_"))
              {
                fileDataBytes = msgAttachFile.Key.Substring("DATABYTES_".Length);
              }
              else
              {
                string str9 = source.FirstOrDefault<string>((System.Func<string, bool>) (x => x.EndsWith(msgAttachFile.Key)));
                num10 = source.IndexOf(str9);
              }
              this._mailData = new EmailMessagesCommands.RegisterMailData(itemData.ObjectID, num10, msgAttachFile.Value, "Файл пришедший от СМДО", 0L);
              longList.Add(this.CreateNewDocument(sessionKeeper.Session, msgAttachFile.Value, attributeById, num10, fileDataBytes));
            }
            catch (FaultException ex)
            {
              this.GenerateAckXML(2, 2, settings, empty5, empty6, empty1, Path.GetFileName(path4), DateTime.Now.ToString((IFormatProvider) CultureInfo.InvariantCulture), empty3, empty4, regNum, customService, ex.Message);
              int num11 = (int) MessageBox.Show(ex.Message);
            }
          }
          foreach (long num12 in longList)
          {
            try
            {
              if (num12 == 0L)
              {
                this.GenerateAckXML(2, 1, settings, empty5, empty6, empty1, "В регистрации отказано.", DateTime.Now.ToString(OfficeClientConsts.SmdoDateFormat), empty3, empty4, regNum, customService, "Процесс импорта файла прерван. Регистрация в канцелярии невозможна.");
                int num13 = (int) MessageBox.Show("Процесс импорта файла прерван. Регистрация в канцелярии невозможна.");
              }
              else
              {
                OfficeDocumentCommands.PublicRegister(sessionKeeper.Session, num12);
                IDBObject dbObject = sessionKeeper.Session.GetObject(num12);
                IDBAttribute byId = dbObject.Attributes.FindByID(OfficeConsts.AttrRegNumberID);
                string regObjectID = byId != null ? byId.AsString : dbObject.Caption;
                this.GenerateAckXML(2, 0, settings, empty5, empty6, empty1, regObjectID, dbObject.CreateDate.ToString(OfficeClientConsts.SmdoDateFormat), empty3, empty4, regNum, customService);
              }
            }
            catch (Exception ex)
            {
              this.GenerateAckXML(2, 1, settings, empty5, empty6, empty1, "Регистрации не подлежит", DateTime.Now.ToString(OfficeClientConsts.SmdoDateFormat), empty3, empty4, regNum, customService, ex.Message);
              throw new KernelException(ex.Message, ex);
            }
          }
        }
        else
        {
          if (!(xmlNode4.Value == "0"))
            return;
          if (xmlDocument.SelectSingleNode($"/{Tag.Envelop}/{Tag.Body}/{Tag.Acknowledgement}/@{Tag.ack_type}") == null)
            throw new KernelException("Неверный формат уведомления");
          try
          {
            int num14 = (int) MessageBox.Show(xmlDocument.SelectSingleNode($"/{Tag.Envelop}/{Tag.Body}/{Tag.Acknowledgement}/{Tag.AckResult}").InnerText, "Ответ на сообщение СМДО");
          }
          catch (Exception ex)
          {
            throw new KernelException("Неверный формат уведомления", ex);
          }
        }
      }
    }
  }

  public Dictionary<string, string> X509Parse(string X509Value)
  {
    Dictionary<string, string> dictionary = new Dictionary<string, string>();
    string str1 = X509Value;
    string[] separator = new string[1]{ ", " };
    foreach (string str2 in str1.Split(separator, StringSplitOptions.RemoveEmptyEntries))
    {
      int length = str2.IndexOf('=');
      if (length != -1)
      {
        string key = str2.Substring(0, length);
        string str3 = str2.Remove(0, length + 1).TrimStart('"').TrimEnd('"');
        if (!dictionary.ContainsKey(key))
          dictionary[key] = str3;
      }
    }
    return dictionary;
  }

  private long CreateNewDocument(
    IUserSession session,
    string messageCaption,
    IDBAttribute msgFileAttr,
    int msgFileAttrIndex,
    string fileDataBytes)
  {
    if (!(SelectionWindow.Select("Выберите тип канцелярского документа", (IDescriptor) new Intermech.Navigator.DBObjectTypes.Descriptor(OfficeConsts.ObjtypeOfficeDocumentsID), typeof (IDBObjectTypeID), SelectionOptions.HideViews | SelectionOptions.SelectObjectTypes | SelectionOptions.DisableMultiselect) is IDBObjectTypeID[] dbObjectTypeIdArray))
      return 0;
    OfficeDocumentTypeSettings settings = ((IOfficeDocumentTypeService) session.GetCustomService(typeof (IOfficeDocumentTypeService))).GetSettings(session.SessionGUID, dbObjectTypeIdArray[0].Value);
    if (settings.EnableTypes != null)
    {
      bool flag = false;
      for (int index = 0; index < settings.EnableTypes.Length; ++index)
      {
        if (settings.EnableTypes[index] == OfficeDocumentTypes.Incoming)
        {
          flag = true;
          break;
        }
      }
      if (!flag)
      {
        int num = (int) MessageBox.Show("В настройках Канцелярия для данного типа объектов не указано, что он поддерживает входящие виды документов", "Внимание", MessageBoxButtons.OK);
        return 0;
      }
    }
    IDBObject dbObject = session.GetObjectCollection(dbObjectTypeIdArray[0].Value).Create();
    dbObject.Caption = Path.GetFileNameWithoutExtension(messageCaption);
    IDBAttribute attributeByGuid = dbObject.GetAttributeByGuid(OfficeConsts.AttrOfficeDocumentTypeGuid);
    if (attributeByGuid != null)
      attributeByGuid.AsInteger = 0L;
    IDBAttribute attributeById = dbObject.GetAttributeByID(OfficeConsts.AttrFileID);
    if (msgFileAttrIndex >= 0)
    {
      msgFileAttr.Index = msgFileAttrIndex;
      this.ChangeFile(msgFileAttr, attributeById, dbObject.ID, messageCaption, "");
    }
    else
      this.ChangeFile(Convert.FromBase64String(fileDataBytes), attributeById, dbObject.ID, messageCaption, "");
    dbObject.CommitCreation(false);
    return dbObject.ObjectID;
  }

  private void GenerateAckXML(
    int ackID,
    int errorCode,
    SMDOSettings settings,
    string receiverName,
    string receiverID,
    string msgGuid,
    string regObjectID,
    string regObjectDate,
    string subjects,
    string regDate,
    string regNum,
    ISMDOSettingsService settingsService,
    string addMessage = "")
  {
    string str1 = string.Empty;
    string str2 = string.Empty;
    switch (ackID)
    {
      case 1:
        str2 = "Уведомление о неудачной доставке документа";
        switch (errorCode)
        {
          case -23:
            str1 = $"Документ отклонён. {addMessage}";
            break;
          case -22:
            str1 = $"Документ отклонён. {addMessage} Отсутствует ЭП для одного или нескольких документов";
            break;
          case -21:
            str1 = $"Документ отклонён. {addMessage} ЭП не верна: нарушена целостность подписанного документа(ов) или не найден файл, на который существует ссылка";
            break;
          case -1:
            str1 = "Документ отклонён. Ошибка при разборе XML-пакета: структура XML не соответствует формату СМДО или была повреждена";
            break;
          case 0:
            str2 = "Уведомление о доставке документа";
            str1 = string.Format("Документ исх. № {1} доставлен в систему документооборота {0}", (object) DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"), (object) regNum);
            break;
          default:
            str1 = "Документ отклонён. Неизвестная ошибка";
            break;
        }
        break;
      case 2:
        str2 = "Уведомление о неудачной регистрации документа";
        switch (errorCode)
        {
          case 0:
            str2 = "Уведомление о регистрации документа";
            str1 = string.Format("Документ исх. № {2} зарегистрирован номером {0} от {1}", (object) regObjectID, (object) regObjectDate, (object) regNum);
            break;
          case 1:
            str1 = string.Format("Документ отклонён. Документ исх. № {1} относится к категории нерегистрируемых: {0}", (object) addMessage, (object) regNum);
            break;
          default:
            str1 = $"Документ отклонён. {addMessage}";
            break;
        }
        break;
    }
    string path = $"{ClientContext.FileVault.WorkArea.AreaPath}\\smdo\\OUT";
    if (!Directory.Exists(path))
      Directory.CreateDirectory(path);
    string str3 = Guid.NewGuid().ToString();
    string str4;
    for (str4 = $"{path}\\{str3}_ack.xml"; File.Exists(str4); str4 = $"{path}\\{str3}_ack.xml")
      str3 = Guid.NewGuid().ToString();
    XmlTextWriter xmlTextWriter = new XmlTextWriter(str4, Encoding.UTF8);
    xmlTextWriter.WriteStartDocument();
    xmlTextWriter.WriteStartElement(Tag.Envelop);
    xmlTextWriter.WriteEndElement();
    xmlTextWriter.Close();
    XmlDocument xmlDocument = new XmlDocument();
    try
    {
      xmlDocument.Load(str4);
    }
    catch (Exception ex)
    {
      throw new KernelException($"Ошибка открытия файла {str4}", ex);
    }
    XmlAttribute attribute1 = xmlDocument.CreateAttribute(Tag.type);
    attribute1.Value = OfficeClientConsts.SmdoVerActualStr;
    xmlDocument.DocumentElement.Attributes.Append(attribute1);
    XmlAttribute attribute2 = xmlDocument.CreateAttribute(Tag.msg_id);
    attribute2.Value = str3;
    xmlDocument.DocumentElement.Attributes.Append(attribute2);
    XmlAttribute attribute3 = xmlDocument.CreateAttribute(Tag.dtstamp);
    attribute3.Value = DateTime.UtcNow.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'");
    xmlDocument.DocumentElement.Attributes.Append(attribute3);
    XmlAttribute attribute4 = xmlDocument.CreateAttribute(Tag.subject);
    attribute4.Value = $"{str2} {subjects}";
    xmlDocument.DocumentElement.Attributes.Append(attribute4);
    XmlNode element1 = (XmlNode) xmlDocument.CreateElement(Tag.Header);
    xmlDocument.DocumentElement.AppendChild(element1);
    XmlAttribute attribute5 = xmlDocument.CreateAttribute(Tag.msg_type);
    attribute5.Value = "0";
    element1.Attributes.Append(attribute5);
    XmlNode element2 = (XmlNode) xmlDocument.CreateElement(Tag.Sender);
    element1.AppendChild(element2);
    XmlAttribute attribute6 = xmlDocument.CreateAttribute(Tag.id);
    attribute6.Value = settings.CompanySMDOid;
    element2.Attributes.Append(attribute6);
    XmlAttribute attribute7 = xmlDocument.CreateAttribute(Tag.name);
    attribute7.Value = settings.CompanyName;
    element2.Attributes.Append(attribute7);
    XmlAttribute attribute8 = xmlDocument.CreateAttribute(Tag.sys_id);
    attribute8.Value = settings.SystemID;
    element2.Attributes.Append(attribute8);
    XmlAttribute attribute9 = xmlDocument.CreateAttribute(Tag.system);
    attribute9.Value = "IPS";
    element2.Attributes.Append(attribute9);
    XmlAttribute attribute10 = xmlDocument.CreateAttribute(Tag.system_details);
    attribute10.Value = $"Версия {typeof (DocumentCommands).Assembly.GetName().Version.Major}.{typeof (DocumentCommands).Assembly.GetName().Version.Minor}";
    element2.Attributes.Append(attribute10);
    XmlNode element3 = (XmlNode) xmlDocument.CreateElement(Tag.Receiver);
    element1.AppendChild(element3);
    XmlAttribute attribute11 = xmlDocument.CreateAttribute(Tag.id);
    attribute11.Value = receiverID;
    element3.Attributes.Append(attribute11);
    XmlAttribute attribute12 = xmlDocument.CreateAttribute(Tag.name);
    attribute12.Value = receiverName;
    element3.Attributes.Append(attribute12);
    XmlNode element4 = (XmlNode) xmlDocument.CreateElement(Tag.Organization);
    element3.AppendChild(element4);
    XmlAttribute attribute13 = xmlDocument.CreateAttribute(Tag.organization_string);
    attribute13.Value = receiverName;
    element4.Attributes.Append(attribute13);
    XmlNode element5 = (XmlNode) xmlDocument.CreateElement(Tag.Body);
    xmlDocument.DocumentElement.AppendChild(element5);
    XmlNode element6 = (XmlNode) xmlDocument.CreateElement(Tag.Acknowledgement);
    element5.AppendChild(element6);
    XmlAttribute attribute14 = xmlDocument.CreateAttribute(Tag.ack_type);
    attribute14.Value = ackID.ToString();
    element6.Attributes.Append(attribute14);
    XmlAttribute attribute15 = xmlDocument.CreateAttribute(Tag.msg_id);
    attribute15.Value = msgGuid;
    element6.Attributes.Append(attribute15);
    XmlNode element7 = (XmlNode) xmlDocument.CreateElement(Tag.RegNumber);
    element6.AppendChild(element7);
    XmlAttribute attribute16 = xmlDocument.CreateAttribute(Tag.regdate);
    attribute16.Value = regDate;
    element7.Attributes.Append(attribute16);
    element7.InnerText = regNum;
    if (ackID == 2)
    {
      XmlNode element8 = (XmlNode) xmlDocument.CreateElement(Tag.IncNumber);
      element6.AppendChild(element8);
      element8.InnerText = regObjectID;
      XmlAttribute attribute17 = xmlDocument.CreateAttribute(Tag.regdate);
      attribute17.Value = regObjectDate;
      element8.Attributes.Append(attribute17);
    }
    XmlNode element9 = (XmlNode) xmlDocument.CreateElement(Tag.AckResult);
    element6.AppendChild(element9);
    element9.InnerText = str1;
    XmlAttribute attribute18 = xmlDocument.CreateAttribute(Tag.errorcode);
    attribute18.Value = errorCode.ToString();
    element9.Attributes.Append(attribute18);
    xmlDocument.Save(str4);
    DialogResult dialogResult = DialogResult.Yes;
    StringBuilder stringBuilder = new StringBuilder();
    if (string.IsNullOrEmpty(settings.MyCompanyEmail))
      stringBuilder.Append("E-mail адрес компании;\n");
    if (string.IsNullOrEmpty(settings.Password))
      stringBuilder.Append("Пароль;\n");
    if (string.IsNullOrEmpty(settings.SMDOHost))
      stringBuilder.Append("Адрес сервера СМДО;\n");
    if (settings.Port == 0)
      stringBuilder.Append("Порт сервера СМДО;\n");
    if (string.IsNullOrEmpty(settings.UserName))
      stringBuilder.Append("Имя пользователя;");
    if (dialogResult == DialogResult.Yes && stringBuilder.Length > 10)
      dialogResult = MessageBox.Show($"Внимание! В общих настройках Канцелярия/СМДО не заданы: {stringBuilder}\n Открыть письмо в почтовом клиенте?", "Внимание", MessageBoxButtons.YesNo) == DialogResult.Yes ? DialogResult.No : DialogResult.Cancel;
    switch (dialogResult)
    {
      case DialogResult.Yes:
        string str5 = string.Empty;
        using (FileStream fileStream = new FileStream(str4, FileMode.Open, FileAccess.Read))
        {
          Dictionary<FileStream, string> attachments = new Dictionary<FileStream, string>()
          {
            {
              fileStream,
              Path.GetFileName(str4)
            }
          };
          str5 = settingsService.SendEmail(settings, $"{str2} {subjects}", attachments);
        }
        int num = str5 == "Сообщение отправлено" ? (int) MessageBox.Show(str5) : throw new KernelException(str5);
        break;
      case DialogResult.No:
        new MAPI().ComposeMail(new string[1]
        {
          settings.SmdoEmail
        }, $"{str2} {subjects}", subjects, new string[1]
        {
          str4
        });
        break;
    }
  }

  private class RegisterMailData
  {
    public readonly long MessageID;
    public readonly long InReplyToID;
    public readonly int IndexFile;
    [NotNull]
    public readonly string FileName;
    [NotNull]
    public readonly string Note;

    public RegisterMailData(
      long messageID,
      int fileIndex,
      [NotNull] string fileName,
      [NotNull] string note,
      long inReplyToID)
    {
      this.MessageID = messageID;
      this.IndexFile = fileIndex;
      this.FileName = fileName;
      this.Note = note;
      this.InReplyToID = inReplyToID;
    }
  }
}
