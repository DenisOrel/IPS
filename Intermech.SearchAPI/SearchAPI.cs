// Decompiled with JetBrains decompiler
// Type: Intermech.SearchAPI.SearchAPI
// Assembly: Intermech.SearchAPI, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D1D502F5-7810-48B3-B639-4FF6D7A8DD6F
// Assembly location: D:\IPS\Client\Intermech.SearchAPI.dll

using Intermech.Commands;
using Intermech.DataFormats;
using Intermech.Files;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Pdm;
using Intermech.Kernel.Search;
using Intermech.Navigator.Controls;
using Intermech.Navigator.Interfaces;
using Intermech.Runtime.ComInterop.LocalServer;
using Intermech.Tools.Data;
using Intermech.Tools.LaunchActions;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

#nullable disable
namespace Intermech.SearchAPI;

[ComVisible(true)]
[Guid("47B9C8A0-D320-412F-9592-F1FE70BA9AB4")]
[ProgId("IPS.IMSearchAPI")]
[ClassInterface(ClassInterfaceType.None)]
[ComDefaultInterface(typeof (ISearchAPIforIPS))]
public class SearchAPI : SingleThreadedObject, ISearchAPIforIPS
{
  private const int _apiversion = 9;
  private long _opened_doc_id;
  private long _opened_prjlink_id;
  private string _errormsg = "";
  private string _errorstack;
  private int _errocode;
  private List<long> _artdocs;
  private long[] _selection;
  private static int relationTypeSP = -1;
  private static int relationTypeDoc = -1;
  private static int attrDesignationID = -1;
  private const uint SW_RESTORE = 9;

  private void begincmd()
  {
    this._errocode = 0;
    this._errormsg = "";
  }

  private void endcmd()
  {
    this._errocode = 0;
    this._errormsg = "";
  }

  private void breakcmd(Exception e)
  {
    this._errocode = 1;
    this._errormsg = e.Message;
    this._errorstack = e.StackTrace;
  }

  private string DataTableToClientDataSetXML(DataTable table)
  {
    StringBuilder output = new StringBuilder();
    using (XmlWriter xmlWriter = XmlWriter.Create(output))
    {
      xmlWriter.WriteStartDocument(true);
      xmlWriter.WriteStartElement("DATAPACKET");
      xmlWriter.WriteAttributeString("Version", "2.0");
      xmlWriter.WriteStartElement("METADATA");
      xmlWriter.WriteStartElement("FIELDS");
      foreach (DataColumn column in (InternalDataCollectionBase) table.Columns)
      {
        xmlWriter.WriteStartElement("FIELD");
        xmlWriter.WriteAttributeString("attrname", column.ColumnName);
        string clientDataSetType = this.DataTypeToClientDataSetType(column.DataType);
        xmlWriter.WriteAttributeString("fieldtype", clientDataSetType);
        if (clientDataSetType == "string")
          xmlWriter.WriteAttributeString("WIDTH", "255");
        xmlWriter.WriteEndElement();
      }
      xmlWriter.WriteEndElement();
      xmlWriter.WriteEndElement();
      xmlWriter.WriteStartElement("ROWDATA");
      foreach (DataRow row in (InternalDataCollectionBase) table.Rows)
      {
        xmlWriter.WriteStartElement("ROW");
        foreach (DataColumn column in (InternalDataCollectionBase) table.Columns)
          xmlWriter.WriteAttributeString(column.ColumnName, row[column].ToString());
        xmlWriter.WriteEndElement();
      }
      xmlWriter.WriteEndElement();
      xmlWriter.WriteEndElement();
      xmlWriter.WriteEndDocument();
    }
    return output.ToString();
  }

  private string DataTypeToClientDataSetType(Type type)
  {
    if (type == typeof (string))
      return "string";
    if (type == typeof (short) || type == typeof (int) || type == typeof (long))
      return "i8";
    if (type == typeof (double))
      return "r10";
    return type == typeof (Decimal) ? "r8" : "string";
  }

  [DllImport("user32.dll")]
  private static extern int ShowWindow(IntPtr hWnd, uint Msg);

  public bool OpenInNewWindow(long ObjectID)
  {
    bool flag = false;
    try
    {
      flag = HyperlinkHandler.OpenUrl($"ips://object/{ObjectID}");
      this.endcmd();
    }
    catch (Exception ex)
    {
      this.breakcmd(ex);
    }
    return flag;
  }

  public void OpenDocument(long DocID)
  {
    try
    {
      this._opened_doc_id = 0L;
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        sessionKeeper.Session.GetObject(DocID, true);
        this._opened_doc_id = DocID;
      }
      this.endcmd();
    }
    catch (Exception ex)
    {
      this.breakcmd(ex);
    }
  }

  public void CloseDocument()
  {
    this._opened_doc_id = 0L;
    this.endcmd();
  }

  public string GetDocTypeName()
  {
    try
    {
      if (this._opened_doc_id == 0L)
        throw new Exception("Call function OpenDocument at first");
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        IDBObject dbObject = sessionKeeper.Session.GetObject(this._opened_doc_id, true);
        IDBObjectType objectType = sessionKeeper.Session.GetObjectType(dbObject.ObjectType);
        this.endcmd();
        return objectType.ObjectTypeName;
      }
    }
    catch (Exception ex)
    {
      this.breakcmd(ex);
      return "";
    }
  }

  public string GetFieldValue(string fldName)
  {
    try
    {
      if (this._opened_doc_id == 0L)
        throw new Exception("Call function OpenDocument at first");
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        IDBObject dbObject = sessionKeeper.Session.GetObject(this._opened_doc_id, true);
        dbObject.Attributes.AllAttributesMode = true;
        IDBAttribute attributeByName = dbObject.GetAttributeByName(fldName, true);
        string asString = attributeByName.AsString;
        if (attributeByName.DataType == FieldTypes.ftMemo)
          asString = attributeByName.Value.ToString();
        this.endcmd();
        return asString;
      }
    }
    catch (Exception ex)
    {
      this.breakcmd(ex);
      return "";
    }
  }

  public void SetFieldValue(string fldName, string fldValue)
  {
    this.begincmd();
    try
    {
      if (this._opened_doc_id == 0L)
        throw new Exception("Call function OpenDocument at first");
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        IDBObject dbObject = sessionKeeper.Session.GetObject(this._opened_doc_id, true);
        IDBAttribute dbAttribute;
        try
        {
          dbAttribute = dbObject.GetAttributeByName(fldName, true);
        }
        catch (Exception ex)
        {
          List<IMSAttribute4ObjectType> attribute4ObjectTypeList = MetaDataHelper.GetAttribute4ObjectTypeList(dbObject.ObjectType);
          if (attribute4ObjectTypeList == null)
            throw;
          IMSAttribute4ObjectType attribute4ObjectType = attribute4ObjectTypeList.Find((Predicate<IMSAttribute4ObjectType>) (x =>
          {
            IMSAttributeType attributeType = MetaDataHelper.GetAttributeType(x.AttributeID);
            return attributeType.Name == fldName || attributeType.ShortName == fldName;
          }));
          if (attribute4ObjectType == null)
            throw;
          dbAttribute = dbObject.Attributes.AddAttribute(attribute4ObjectType.AttributeID, false);
        }
        if (Intermech.SearchAPI.SearchAPI.attrDesignationID == -1)
          Intermech.SearchAPI.SearchAPI.attrDesignationID = MetaDataHelper.GetAttributeTypeID("cad0001f-306c-11d8-b4e9-00304f19f545");
        if (dbAttribute.AttributeID == Intermech.SearchAPI.SearchAPI.attrDesignationID)
          fldValue = DocumentDesignationHelper.AppendDocCode(fldValue, dbObject.ObjectType);
        if (dbAttribute.DataType == FieldTypes.ftBoolean)
          fldValue = fldValue == "1" || fldValue.ToLower() == "true" ? "True" : "False";
        dbAttribute.Value = (object) fldValue;
        this.endcmd();
      }
    }
    catch (Exception ex)
    {
      this.breakcmd(ex);
    }
  }

  public void OpenArtDocuments(long ArtID, int DocTypeID)
  {
    try
    {
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        IDBRelationCollection relationCollection = sessionKeeper.Session.GetRelationCollection(MetaDataHelper.GetRelationTypeID("cad00154-306c-11d8-b4e9-00304f19f545"));
        relationCollection.ObjectTypeID = DocTypeID <= 0 ? MetaDataHelper.GetObjectTypeID("cad00070-306c-11d8-b4e9-00304f19f545") : DocTypeID;
        DBRecordSetParams paramSet = new DBRecordSetParams(new ConditionStructure[0], new object[1]
        {
          (object) ObligatoryObjectAttributes.F_OBJECT_ID
        });
        DataTable dataTable = relationCollection.ConsistFrom(paramSet, ArtID);
        this._artdocs = new List<long>();
        if (dataTable != null)
        {
          if (dataTable.Rows != null)
          {
            foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
              this._artdocs.Add(Convert.ToInt64(row[0]));
          }
        }
      }
      this.endcmd();
    }
    catch (Exception ex)
    {
      this.breakcmd(ex);
    }
  }

  public void CloseArtDocuments()
  {
    this._artdocs = (List<long>) null;
    this.endcmd();
  }

  public int GetDocumentsCount()
  {
    try
    {
      if (this._artdocs == null)
        throw new Exception("Call OpenArtDocuments at first");
      this.endcmd();
      return this._artdocs.Count;
    }
    catch (Exception ex)
    {
      this.breakcmd(ex);
      return 0;
    }
  }

  public long GetArtDocumentID(int i)
  {
    try
    {
      if (this._artdocs == null)
        throw new Exception("Call OpenArtDocuments at first");
      this.endcmd();
      return this._artdocs[i];
    }
    catch (Exception ex)
    {
      this.breakcmd(ex);
      return -1;
    }
  }

  public int ErrorCode => this._errocode;

  public string ErrorMessage => this._errormsg;

  public string PrepareDocOwnersList(long DocID)
  {
    string str = "";
    try
    {
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        IDBObject dbObject = sessionKeeper.Session.GetObject(DocID, true);
        DataTable table = sessionKeeper.Session.GetRelationCollection(MetaDataHelper.GetRelationTypeID("cad0057c-306c-11d8-b4e9-00304f19f545")).EntersIn(new DBRecordSetParams(new ConditionStructure[0], new object[1]
        {
          (object) ObligatoryObjectAttributes.F_OBJECT_ID
        }), dbObject.ID);
        if (table != null)
        {
          if (table.Rows != null)
          {
            table.Columns[0].ColumnName = "DOC_ID";
            str = this.DataTableToClientDataSetXML(table);
          }
        }
      }
      this.endcmd();
    }
    catch (Exception ex)
    {
      this.breakcmd(ex);
    }
    return str;
  }

  public string PrepareDocRefsList(long DocID)
  {
    string str = "";
    try
    {
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        IDBObject dbObject = sessionKeeper.Session.GetObject(DocID, true);
        DataTable table = sessionKeeper.Session.GetRelationCollection(MetaDataHelper.GetRelationTypeID("cad0057c-306c-11d8-b4e9-00304f19f545")).ConsistFrom(new DBRecordSetParams(new ConditionStructure[0], new object[1]
        {
          (object) ObligatoryObjectAttributes.F_OBJECT_ID
        }), dbObject.ObjectID);
        if (table != null)
        {
          if (table.Rows != null)
          {
            table.Columns[0].ColumnName = "DOC_ID";
            str = this.DataTableToClientDataSetXML(table);
          }
        }
      }
      this.endcmd();
    }
    catch (Exception ex)
    {
      this.breakcmd(ex);
    }
    return str;
  }

  public void Minimize()
  {
    try
    {
      SearchAPIPlugin._serviceProvider.GetService<IMainFormUpdate>().MainForm.WindowState = FormWindowState.Minimized;
      this.endcmd();
    }
    catch (Exception ex)
    {
      this.breakcmd(ex);
    }
  }

  public void Restore()
  {
    try
    {
      Intermech.SearchAPI.SearchAPI.ShowWindow(SearchAPIPlugin._serviceProvider.GetService<IMainFormUpdate>().MainForm.Handle, 9U);
      this.endcmd();
    }
    catch (Exception ex)
    {
      this.breakcmd(ex);
    }
  }

  public void Edit()
  {
    try
    {
      if (this._opened_doc_id == 0L)
        throw new Exception("Call function OpenDocument at first");
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        IDBObject dbObject = sessionKeeper.Session.GetObject(this._opened_doc_id, true);
        ClientContext.LaunchActions.Launch(new LaunchParams(LaunchType.Edit, dbObject.ObjectID, dbObject.ObjectType, VersionsRuleSources.GetEditorRule()));
        this.endcmd();
      }
    }
    catch (Exception ex)
    {
      this.breakcmd(ex);
    }
  }

  public void View()
  {
    try
    {
      if (this._opened_doc_id == 0L)
        throw new Exception("Call function OpenDocument at first");
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        IDBObject dbObject = sessionKeeper.Session.GetObject(this._opened_doc_id, true);
        ClientContext.LaunchActions.Launch(new LaunchParams(LaunchType.View, dbObject.ObjectID, dbObject.ObjectType, VersionsRuleSources.GetEditorRule()));
        this.endcmd();
      }
    }
    catch (Exception ex)
    {
      this.breakcmd(ex);
    }
  }

  public int APIVersion => 9;

  public void SelectDocs()
  {
    try
    {
      this._selection = Intermech.Navigator.SelectionWindow.SelectObjects("Выбор документа", "Выберите документ.", MetaDataHelper.GetObjectTypeID("cad00070-306c-11d8-b4e9-00304f19f545"), SelectionOptions.SelectObjects | SelectionOptions.DisableSelectFromTree);
      this.endcmd();
    }
    catch (Exception ex)
    {
      this.breakcmd(ex);
    }
  }

  public int SelectedDocsCount()
  {
    try
    {
      if (this._selection == null)
        throw new Exception("Call function SelectDocs at first");
      this.endcmd();
      return this._selection.Length;
    }
    catch (Exception ex)
    {
      this.breakcmd(ex);
      return -1;
    }
  }

  public long GetSelectedDocID(int i)
  {
    try
    {
      long selectedDocId = this._selection != null ? this._selection[i] : throw new Exception("Call function SelectDocs at first");
      this.endcmd();
      return selectedDocId;
    }
    catch (Exception ex)
    {
      this.breakcmd(ex);
      return -1;
    }
  }

  public int GetFieldCount()
  {
    try
    {
      if (this._opened_doc_id == 0L)
        throw new Exception("Call function OpenDocument at first");
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        int count = sessionKeeper.Session.GetObject(this._opened_doc_id, true).Attributes.Count;
        this.endcmd();
        return count;
      }
    }
    catch (Exception ex)
    {
      this.breakcmd(ex);
      return -1;
    }
  }

  public string GetFieldName(int fldNo)
  {
    try
    {
      if (this._opened_doc_id == 0L)
        throw new Exception("Call function OpenDocument at first");
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        string name = sessionKeeper.Session.GetObject(this._opened_doc_id, true).Attributes[fldNo].Name;
        this.endcmd();
        return name;
      }
    }
    catch (Exception ex)
    {
      this.breakcmd(ex);
      return "";
    }
  }

  public void GetDocIDVersionIDbyFileName(string aFileName, out long aDocID, out long aVersionID)
  {
    aVersionID = 0L;
    try
    {
      if (string.IsNullOrEmpty(aFileName))
        throw new ArgumentException("Путь к файлу не может быть пустым", "filePath");
      if (!Path.IsPathRooted(aFileName))
        throw new ArgumentException("Требуется абсолютный путь к файлу.", "filePath");
      FileOrigin fileOrigin = ClientContext.FileVault.WorkArea.GetFileOrigin(aFileName, false);
      switch (fileOrigin.OriginType)
      {
        case FileOriginType.WorkFile:
          aDocID = fileOrigin.WorkObject.ObjectId;
          break;
        case FileOriginType.DetachedFile:
          VersionsRulePackage editorRule = VersionsRuleSources.GetEditorRule();
          using (SessionKeeper sessionKeeper = new SessionKeeper())
          {
            IDBObject objectByVersionsRule = sessionKeeper.Session.GetObjectByVersionsRule(fileOrigin.Id, editorRule.OwnerId, true);
            aDocID = objectByVersionsRule.ObjectID;
            break;
          }
        default:
          aDocID = 0L;
          break;
      }
      this.endcmd();
    }
    catch (Exception ex)
    {
      this.breakcmd(ex);
      aDocID = 0L;
    }
  }

  public bool EditParameters2()
  {
    try
    {
      if (this._opened_doc_id == 0L)
        throw new Exception("Call function OpenDocument at first");
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        IDBObject dbObject = sessionKeeper.Session.GetObject(this._opened_doc_id, true);
        int num = PropertiesWindow.Execute(string.Empty, string.Empty, dbObject.ObjectID) == DialogResult.OK ? 1 : 0;
        this.endcmd();
        return num != 0;
      }
    }
    catch (Exception ex)
    {
      this.breakcmd(ex);
      return false;
    }
  }

  public void SaveChanges()
  {
    try
    {
      if (this._opened_doc_id == 0L)
        throw new Exception("Call function OpenDocument at first");
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        IDBObject dbObject = sessionKeeper.Session.GetObject(this._opened_doc_id, true);
        ObjectCommand saveChangesCommand = ObjectCommandFactory.CreateSaveChangesCommand(true);
        saveChangesCommand.ObjectId = dbObject.ObjectID;
        saveChangesCommand.UpdateUI = false;
        saveChangesCommand.Execute();
      }
      this.endcmd();
    }
    catch (Exception ex)
    {
      this.breakcmd(ex);
    }
  }

  public long LogFileInArchive(string fileName)
  {
    try
    {
      long num = SearchAPIPlugin._serviceProvider.GetService(typeof (IFileImportService)) is IFileImportService service ? service.ImportFile(fileName) : throw new Exception("IFileImportService not found");
      this.endcmd();
      return num;
    }
    catch (Exception ex)
    {
      this.breakcmd(ex);
      return 0;
    }
  }

  public string GetDocWorkCopyPath()
  {
    try
    {
      if (this._opened_doc_id == 0L)
        throw new Exception("Call function OpenDocument at first");
      string path = "";
      IFileVault service = SearchAPIPlugin._serviceProvider.GetService<IFileVault>();
      if (service.WorkArea.IsObjectPublished(this._opened_doc_id))
      {
        string masterFileName = service.DBFilesInfo.GetMasterFileName(this._opened_doc_id, false);
        if (masterFileName != null)
        {
          path = Path.Combine(service.WorkArea.AreaPath, masterFileName);
          if (!File.Exists(path))
            path = "";
        }
      }
      this.endcmd();
      return path;
    }
    catch (Exception ex)
    {
      this.breakcmd(ex);
      return "";
    }
  }

  public void CheckOut()
  {
    this.begincmd();
    try
    {
      if (this._opened_doc_id == 0L)
        throw new Exception("Call function OpenDocument at first");
      if (this._opened_doc_id <= 0L)
        return;
      ObjectCopyCommand checkoutCommand = ObjectCommandFactory.CreateCheckoutCommand(true);
      checkoutCommand.ObjectId = this._opened_doc_id;
      checkoutCommand.Execute();
      this._opened_doc_id = checkoutCommand.NewObjectId;
    }
    catch (Exception ex)
    {
      this.breakcmd(ex);
    }
  }

  public void CheckIn()
  {
    this.begincmd();
    try
    {
      if (this._opened_doc_id == 0L)
        throw new Exception("Call function OpenDocument at first");
      if (this._opened_doc_id >= 0L)
        return;
      ObjectCopyCommand checkinCommand = ObjectCommandFactory.CreateCheckinCommand(true);
      checkinCommand.ObjectId = this._opened_doc_id;
      checkinCommand.Execute();
      this._opened_doc_id = checkinCommand.NewObjectId;
    }
    catch (Exception ex)
    {
      this.breakcmd(ex);
    }
  }

  public long FindArticle(string aDesignatio, string aName, string aOKP_Code)
  {
    this.begincmd();
    try
    {
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        VersionsRulePackage editorRule = VersionsRuleSources.GetEditorRule();
        long articleId = SearchAPIPlugin._serviceProvider.GetService<IArticleService>().FindArticleID(aDesignatio, aOKP_Code, aName, editorRule.OwnerId, (object) sessionKeeper.Session);
        this.endcmd();
        return articleId;
      }
    }
    catch (Exception ex)
    {
      this.breakcmd(ex);
      return 0;
    }
  }

  public long AddNewArticle2(
    string aDesignation,
    string aOKPCode,
    string aName,
    string aSectionID)
  {
    this.begincmd();
    try
    {
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        IDBObject dbObject = sessionKeeper.Session.GetObjectCollection(new Guid(aSectionID)).Create();
        if (Intermech.SearchAPI.SearchAPI.attrDesignationID == -1)
          Intermech.SearchAPI.SearchAPI.attrDesignationID = MetaDataHelper.GetAttributeTypeID("cad0001f-306c-11d8-b4e9-00304f19f545");
        int attributeTypeId1 = MetaDataHelper.GetAttributeTypeID("cad00020-306c-11d8-b4e9-00304f19f545");
        int attributeTypeId2 = MetaDataHelper.GetAttributeTypeID("cad0038a-306c-11d8-b4e9-00304f19f545");
        dbObject.SetAttributesValues(new AttributeValues[3]
        {
          new AttributeValues(Intermech.SearchAPI.SearchAPI.attrDesignationID, (object) aDesignation),
          new AttributeValues(attributeTypeId1, (object) aName),
          new AttributeValues(attributeTypeId2, (object) aOKPCode)
        });
        dbObject.CommitCreation(true, false);
        return dbObject.ObjectID;
      }
    }
    catch (Exception ex)
    {
      this.breakcmd(ex);
      return 0;
    }
  }

  private long getRazdelID(string Razdel, IUserSession session)
  {
    Guid result1;
    if (Guid.TryParse(Razdel, out result1))
      return session.GetObject(result1, true).ObjectID;
    int result2;
    if (int.TryParse(Razdel, out result2))
    {
      DataTable dataTable = session.GetObjectCollection(new Guid("cad00254-306c-11d8-b4e9-00304f19f545")).Select(new DBRecordSetParams(new ConditionStructure[1]
      {
        new ConditionStructure(session.GetAttributeType(new Guid("cad00279-306c-11d8-b4e9-00304f19f545")).AttributeID, RelationalOperators.Equal, (object) result2, LogicalOperators.NONE, 0, false)
      }, new object[1]
      {
        (object) ObligatoryObjectAttributes.F_OBJECT_ID
      })
      {
        Contents = new ColumnContents[1]
        {
          ColumnContents.ID
        }
      });
      return dataTable.Rows.Count == 0 ? 0L : Convert.ToInt64(dataTable.Rows[0][0]);
    }
    DataTable dataTable1 = session.GetObjectCollection(new Guid("cad00254-306c-11d8-b4e9-00304f19f545")).Select(new DBRecordSetParams(new ConditionStructure[1]
    {
      new ConditionStructure(session.GetAttributeType(new Guid("cad00020-306c-11d8-b4e9-00304f19f545")).AttributeID, RelationalOperators.Equal, (object) Razdel, LogicalOperators.NONE, 0, false)
    }, new object[1]
    {
      (object) ObligatoryObjectAttributes.F_OBJECT_ID
    })
    {
      Contents = new ColumnContents[1]{ ColumnContents.ID }
    });
    return dataTable1.Rows.Count == 0 ? 0L : Convert.ToInt64(dataTable1.Rows[0][0]);
  }

  public long AddBOMItem(
    long ProjAID,
    long PartAID,
    string CountPC,
    string Razdel,
    string Position,
    string Note)
  {
    this.begincmd();
    try
    {
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        if (Intermech.SearchAPI.SearchAPI.relationTypeSP == -1)
          Intermech.SearchAPI.SearchAPI.relationTypeSP = MetaDataHelper.GetRelationTypeID(new Guid("cad00023-306c-11d8-b4e9-00304f19f545"));
        IDBRelation dbRelation = sessionKeeper.Session.GetRelationCollection(Intermech.SearchAPI.SearchAPI.relationTypeSP).Create(sessionKeeper.Session.GetObject(ProjAID, true).ObjectID, sessionKeeper.Session.GetObject(PartAID, true).ObjectID, new AttributeValues[4]
        {
          new AttributeValues(MetaDataHelper.GetAttributeTypeID("cad00270-306c-11d8-b4e9-00304f19f545"), (object) Position),
          new AttributeValues(MetaDataHelper.GetAttributeTypeID("cad00021-306c-11d8-b4e9-00304f19f545"), (object) Note),
          new AttributeValues(MetaDataHelper.GetAttributeTypeID("cad00267-306c-11d8-b4e9-00304f19f545"), (object) CountPC),
          new AttributeValues(MetaDataHelper.GetAttributeTypeID("cad00266-306c-11d8-b4e9-00304f19f545"), (object) this.getRazdelID(Razdel, sessionKeeper.Session))
        });
        this.endcmd();
        return dbRelation.RelationID;
      }
    }
    catch (Exception ex)
    {
      this.breakcmd(ex);
      return 0;
    }
  }

  public void OpenBOMItem(long prjLinkID)
  {
    this.begincmd();
    try
    {
      this._opened_prjlink_id = 0L;
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        sessionKeeper.Session.GetRelation(prjLinkID, true);
        this._opened_prjlink_id = prjLinkID;
        this.endcmd();
      }
    }
    catch (Exception ex)
    {
      this.breakcmd(ex);
    }
  }

  public void SetFieldValue_BOM(string fldName, string fldValue)
  {
    this.begincmd();
    try
    {
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        IDBRelation relation = sessionKeeper.Session.GetRelation(this._opened_prjlink_id, true);
        IDBAttribute dbAttribute;
        try
        {
          dbAttribute = relation.GetAttributeByName(fldName, true);
        }
        catch (Exception ex)
        {
          List<IMSAttribute4RelationType> relationTypeList = MetaDataHelper.GetAttribute4RelationTypeList(relation.RelationType);
          if (relationTypeList == null)
            throw;
          IMSAttribute4RelationType attribute4RelationType = relationTypeList.Find((Predicate<IMSAttribute4RelationType>) (x =>
          {
            IMSAttributeType attributeType = MetaDataHelper.GetAttributeType(x.AttributeID);
            return attributeType.Name == fldName || attributeType.ShortName == fldName;
          }));
          if (attribute4RelationType == null)
            throw;
          dbAttribute = relation.Attributes.AddAttribute(attribute4RelationType.AttributeID, false);
        }
        if (dbAttribute.DataType == FieldTypes.ftBoolean)
          fldValue = fldValue == "1" || fldValue.ToLower() == "true" ? "True" : "False";
        dbAttribute.Value = (object) fldValue;
        this.endcmd();
      }
    }
    catch (Exception ex)
    {
      this.breakcmd(ex);
    }
  }

  public string GetFieldValue_BOM(string fldName)
  {
    this.begincmd();
    try
    {
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        string asString = sessionKeeper.Session.GetRelation(this._opened_prjlink_id, true).GetAttributeByName(fldName, true).AsString;
        this.endcmd();
        return asString;
      }
    }
    catch (Exception ex)
    {
      this.breakcmd(ex);
      return "";
    }
  }

  public void DeleteAllBOMItems(long ProjAID)
  {
    this.begincmd();
    try
    {
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        if (Intermech.SearchAPI.SearchAPI.relationTypeSP == -1)
          Intermech.SearchAPI.SearchAPI.relationTypeSP = MetaDataHelper.GetRelationTypeID(new Guid("cad00023-306c-11d8-b4e9-00304f19f545"));
        IDBRelationCollection relationCollection = sessionKeeper.Session.GetRelationCollection(Intermech.SearchAPI.SearchAPI.relationTypeSP);
        IDBObject dbObject = sessionKeeper.Session.GetObject(ProjAID, true);
        DBRecordSetParams paramSet = new DBRecordSetParams((ConditionStructure[]) null, new object[1]
        {
          (object) -20
        });
        long objectId = dbObject.ObjectID;
        DataTable dataTable = relationCollection.ConsistFrom(paramSet, objectId);
        for (int index = 0; index < dataTable.Rows.Count; ++index)
          sessionKeeper.Session.GetRelation(Convert.ToInt64(dataTable.Rows[index][0])).Delete(0L);
        this.endcmd();
      }
    }
    catch (Exception ex)
    {
      this.breakcmd(ex);
    }
  }

  public long LinkDocToArticle(long ArtID, long DocID, int LinkType, int LinkToIsp)
  {
    try
    {
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        if (Intermech.SearchAPI.SearchAPI.relationTypeDoc == -1)
          Intermech.SearchAPI.SearchAPI.relationTypeDoc = MetaDataHelper.GetRelationTypeID(new Guid("cad00154-306c-11d8-b4e9-00304f19f545"));
        IDBRelationCollection relationCollection = sessionKeeper.Session.GetRelationCollection(Intermech.SearchAPI.SearchAPI.relationTypeDoc);
        IDBObject dbObject1 = sessionKeeper.Session.GetObject(ArtID, true);
        IDBObject dbObject2 = sessionKeeper.Session.GetObject(DocID, true);
        long objectId1 = dbObject1.ObjectID;
        long objectId2 = dbObject2.ObjectID;
        IDBRelation dbRelation = relationCollection.Create(objectId1, objectId2);
        this.endcmd();
        return dbRelation.RelationID;
      }
    }
    catch (Exception ex)
    {
      this.breakcmd(ex);
      return 0;
    }
  }

  public string GetComposition(long ObjectID, long SchemeID)
  {
    string composition = "";
    try
    {
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        ICompositionService customService = (ICompositionService) sessionKeeper.Session.GetCustomService(typeof (ICompositionService));
        List<ColumnDescriptor> columns = new List<ColumnDescriptor>();
        columns.Add(new ColumnDescriptor((object) ObligatoryObjectAttributes.F_OBJECT_ID, AttributeSourceTypes.Object, ColumnContents.Value, ColumnNameMapping.ID, SortOrders.NONE, 0));
        columns.Add(new ColumnDescriptor((object) ObligatoryObjectAttributes.F_OBJECT_TYPE, AttributeSourceTypes.Object, ColumnContents.Value, ColumnNameMapping.ID, SortOrders.NONE, 0));
        columns.Add(new ColumnDescriptor((object) ObligatoryObjectAttributes.CAPTION, AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.ID, SortOrders.NONE, 0));
        columns.Add(new ColumnDescriptor((object) ObligatoryObjectAttributes.F_PRJLINK_ID, AttributeSourceTypes.Relation, ColumnContents.Value, ColumnNameMapping.ID, SortOrders.NONE, 0));
        columns.Add(new ColumnDescriptor((object) ObligatoryObjectAttributes.F_ID, AttributeSourceTypes.Object, ColumnContents.Value, ColumnNameMapping.ID, SortOrders.NONE, 0));
        Guid selectGUID = Guid.NewGuid();
        customService.Select(sessionKeeper.Session.SessionGUID, ObjectID, SchemeID, columns, selectGUID, "", (HybridDictionary) null);
        CompositionInfo info;
        for (info = customService.GetInfo(selectGUID); info != null && !info.ErrorPresent && info.Percent < 100; info = customService.GetInfo(selectGUID))
          Thread.Sleep(25);
        if (info.ErrorPresent)
          throw info.ErrorException;
        DataTable table = info.Result != null ? (DataTable) info.Result : throw new Exception("CompositionService.Result == null");
        table.Columns[0].ColumnName = "F_OBJECT_ID";
        table.Columns[1].ColumnName = "F_OBJECT_TYPE";
        table.Columns[2].ColumnName = "CAPTION";
        table.Columns[3].ColumnName = "F_PRJLINK_ID";
        table.Columns[4].ColumnName = "F_ID";
        composition = this.DataTableToClientDataSetXML(table);
        this.endcmd();
      }
    }
    catch (Exception ex)
    {
      this.breakcmd(ex);
    }
    return composition;
  }

  public void OpenUrl(string url)
  {
    try
    {
      HyperlinkHandler.OpenUrl(url);
      this.endcmd();
    }
    catch (Exception ex)
    {
      this.breakcmd(ex);
    }
  }

  public long GetObjectBaseVersionByID(long ID)
  {
    long objectBaseVersionById1 = -1;
    try
    {
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        IDBObject objectBaseVersionById2 = sessionKeeper.Session.GetObjectBaseVersionByID(ID, true);
        if (objectBaseVersionById2 != null)
          objectBaseVersionById1 = objectBaseVersionById2.ObjectID;
      }
      this.endcmd();
    }
    catch (Exception ex)
    {
      this.breakcmd(ex);
    }
    return objectBaseVersionById1;
  }

  public long ActiveObjectID()
  {
    long num = 0;
    try
    {
      if (ServicesManager.GetService(typeof (ISimpleSelectedItems)) is ISimpleSelectedItems service && service.GetItemData(0, typeof (IDBObjectID)) is IDBObjectID itemData)
        num = itemData.Value;
      this.endcmd();
    }
    catch (Exception ex)
    {
      this.breakcmd(ex);
    }
    return num;
  }

  public string FindObjects(string ObjectTypeGuid, string AttributeName, string AttributeValue)
  {
    string objects = "";
    try
    {
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        IDBObjectCollection objectCollection = sessionKeeper.Session.GetObjectCollection(new Guid(ObjectTypeGuid));
        DataTable dataTable = objectCollection.Select(new DBRecordSetParams(new ConditionStructure[1]
        {
          new ConditionStructure((MetaDataHelper.GetAttribute4ObjectTypeList(objectCollection.ObjectTypeID).Find((Predicate<IMSAttribute4ObjectType>) (x =>
          {
            IMSAttributeType attributeType = MetaDataHelper.GetAttributeType(x.AttributeID);
            return attributeType.Name == AttributeName || attributeType.ShortName == AttributeName;
          })) ?? throw new Exception($"Атрибут {AttributeName} не найден")).AttributeID, RelationalOperators.Equal, (object) AttributeValue, LogicalOperators.NONE, 0, false)
        }, new object[1]
        {
          (object) ObligatoryObjectAttributes.F_OBJECT_ID
        })
        {
          Contents = new ColumnContents[1]
          {
            ColumnContents.ID
          }
        });
        if (dataTable != null)
        {
          if (dataTable.Rows.Count > 0)
            objects = string.Join(",", dataTable.Rows.OfType<DataRow>().Select<DataRow, string>((System.Func<DataRow, string>) (x => x[0].ToString())));
        }
      }
      this.endcmd();
    }
    catch (Exception ex)
    {
      this.breakcmd(ex);
    }
    return objects;
  }
}
