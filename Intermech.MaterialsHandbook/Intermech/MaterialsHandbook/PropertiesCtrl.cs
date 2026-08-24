// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.PropertiesCtrl
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using ImSSP;
using Intermech.Imbase;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Imbase;
using Intermech.Interfaces.MaterialsHandbook;
using Intermech.Localization;
using Intermech.MaterialsHandbook.Controls.MaterialProperties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;

#nullable disable
namespace Intermech.MaterialsHandbook;

public class PropertiesCtrl : UserControl
{
  private const string DOC = "doc";
  private const string SECTIONS = "sections";
  private const string SECTION = "section";
  private const string TABLE = "table";
  private const string COLUMNS = "columns";
  private const string COLUMN = "Col";
  private const string ROWS = "rows";
  private const string ROW = "row";
  private const string UNITED_ROW = "UnitedRow";
  private const string ATTR_NAME = "name";
  private const string ATTR_ID = "id";
  private long _objPropsID;
  private const string _attrPropsGuid = "cadd93d3-306c-11d8-b4e9-00304f19f545";
  private DataTable _dtSettings;
  private string _imbaseKey = string.Empty;
  private IContainer components;

  public bool ReadOnly { get; set; }

  public string Caption { get; private set; }

  public string ColMaterial { get; private set; }

  public string ColObject { get; private set; }

  public string ImbaseKey
  {
    get => this._imbaseKey;
    set
    {
      this._imbaseKey = value;
      this.Clear(false);
      if (!string.IsNullOrEmpty(this._imbaseKey))
        this.ParseXml(this.GetProperties(this._imbaseKey));
      else
        this.Caption = this._imbaseKey = string.Empty;
      this.OnSelectedElementChanged();
    }
  }

  public bool IsSettingsLoaded
  {
    get
    {
      return this._dtSettings != null && !string.IsNullOrEmpty(this.ColMaterial) && !string.IsNullOrEmpty(this.ColObject) && this._dtSettings.Columns.Contains(this.ColMaterial) && this._dtSettings.Columns.Contains(this.ColObject);
    }
  }

  public DataTable SettingsTable
  {
    get
    {
      if (this._dtSettings == null)
        this.LoadSettingsData(true);
      return this._dtSettings;
    }
  }

  public List<Page> Pages => this.Controls.OfType<Page>().ToList<Page>();

  public Page ActivePage { get; private set; }

  public int BetweenPagesDistance { get; set; } = 10;

  public PropertiesCtrl() => this.InitializeComponent();

  public event SelectedRibbonElementEventHandler SelectedElementChanged;

  public event EventHandler CaptionChanged;

  public event EventHandler DataChanged;

  protected override void OnMouseClick(MouseEventArgs e)
  {
    base.OnMouseClick(e);
    this.Page_PageSelected((object) null, new EventArgs());
    this.OnSelectedElementChanged();
  }

  protected override void OnLayout(LayoutEventArgs e)
  {
    this.CalcPagesLocation(this.ClientSize.Width);
    base.OnLayout(e);
  }

  private void CalcPagesLocation(int width)
  {
    if (this.Pages.Count <= 0)
      return;
    int y = this.Pages.Select<Page, int>((System.Func<Page, int>) (x => x.Top)).Min();
    int width1 = width - this.Padding.Horizontal;
    foreach (Page page in this.Pages)
    {
      page.Bounds = new Rectangle(this.Padding.Left, y, width1, page.Height);
      y = page.Bounds.Bottom + this.BetweenPagesDistance;
    }
  }

  private bool AddSettings(IUserSession session, long objId, out long tableId)
  {
    bool flag = false;
    tableId = 0L;
    if (session != null && this.IsSettingsLoaded && objId != 0L && this._dtSettings.Select(string.Format("[{0}]='{1}' or [{0}]='{2}'", (object) this.ColMaterial, (object) this._imbaseKey, (object) ImbaseHelper.ConvertImbaseKey(session, this._imbaseKey))).Length == 0)
    {
      DataRow row = this._dtSettings.NewRow();
      row["F_GUID"] = (object) Guid.NewGuid();
      row[this.ColObject] = (object) objId;
      row[this.ColMaterial] = (object) this._imbaseKey;
      this._dtSettings.Rows.Add(row);
      this._dtSettings.AcceptChanges();
      long objectIdByConstName = IMHHelper.GetObjectIDByConstName("MATERIAL_PROPERTIES_TABLE_NAME");
      tableId = IMHHelper.GetTableIDByTableRefID(objectIdByConstName);
      ITablesIndexer customService = session.GetCustomService(typeof (ITablesIndexer)) as ITablesIndexer;
      TableLoadHelper.StoreData(session, tableId, this._dtSettings.DataSet, customService);
      flag = true;
    }
    return flag;
  }

  private XmlDocument GetProperties(string imbaseKey)
  {
    XmlDocument properties = new XmlDocument();
    this.LoadSettingsData(false);
    if (this.IsSettingsLoaded)
    {
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        string str1 = ImbaseHelper.ConvertImbaseKey(sessionKeeper.Session, imbaseKey);
        DataRow[] dataRowArray = this._dtSettings.Select(string.Format("[{0}]='{1}' or [{0}]='{2}'", (object) this.ColMaterial, (object) imbaseKey, (object) str1));
        if (dataRowArray.Length != 0)
        {
          long result;
          if (long.TryParse(Convert.ToString(dataRowArray[0][this.ColObject]), out result))
          {
            IDBObject objectActualCopy = sessionKeeper.Session.GetObjectActualCopy(result, false);
            if (objectActualCopy != null)
            {
              this._objPropsID = result;
              string str2 = this.ReadBlob(objectActualCopy.GetAttributeByGuid(new Guid("cadd93d3-306c-11d8-b4e9-00304f19f545")));
              if (!string.IsNullOrEmpty(str2))
                properties.InnerXml = str2;
              else
                properties.RemoveAll();
            }
          }
        }
      }
    }
    return properties;
  }

  private void LoadSettingsData(bool bReload)
  {
    if (this._dtSettings == null | bReload)
    {
      DataSet imbaseDs = IMHHelper.GetImbaseDS("MATERIAL_PROPERTIES_TABLE_NAME");
      if (imbaseDs != null && imbaseDs.Tables.Contains("IMS_DATA"))
      {
        this._dtSettings = imbaseDs.Tables["IMS_DATA"];
        if (this._dtSettings != null)
        {
          using (SessionKeeper sessionKeeper = new SessionKeeper())
          {
            if (sessionKeeper.Session.GetCustomService(typeof (IIMHSystemSettingsService)) is IIMHSystemSettingsService customService)
            {
              List<string> names = new List<string>((IEnumerable<string>) new string[2]
              {
                "MATERIAL_PROPERTIES_COLUMN_MATERIAL",
                "MATERIAL_PROPERTIES_COLUMN_OBJECT"
              });
              Dictionary<string, Guid> objectGuidsByNames = customService.GetObjectGuidsByNames(names);
              if (objectGuidsByNames != null)
              {
                this.ColMaterial = !objectGuidsByNames.ContainsKey("MATERIAL_PROPERTIES_COLUMN_MATERIAL") || !(objectGuidsByNames["MATERIAL_PROPERTIES_COLUMN_MATERIAL"] != Guid.Empty) ? string.Empty : objectGuidsByNames["MATERIAL_PROPERTIES_COLUMN_MATERIAL"].ToString();
                this.ColObject = !objectGuidsByNames.ContainsKey("MATERIAL_PROPERTIES_COLUMN_OBJECT") || !(objectGuidsByNames["MATERIAL_PROPERTIES_COLUMN_OBJECT"] != Guid.Empty) ? string.Empty : objectGuidsByNames["MATERIAL_PROPERTIES_COLUMN_OBJECT"].ToString();
              }
            }
          }
        }
      }
    }
    this.OnSelectedElementChanged();
  }

  private void OnCaptionChanged()
  {
    EventHandler captionChanged = this.CaptionChanged;
    if (captionChanged == null)
      return;
    captionChanged((object) this, new EventArgs());
  }

  public void OnSelectedElementChanged()
  {
    if (this.SelectedElementChanged == null)
      return;
    SelectedElement element = SelectedElement.None;
    int index = -1;
    int elementsCount = 0;
    bool flag = false;
    int num1 = -1;
    int num2 = -1;
    if (this.ActivePage != null)
    {
      if (!this.ActivePage.IsTableClicked)
      {
        element = SelectedElement.Page;
        index = this.Pages.IndexOf(this.ActivePage);
        elementsCount = this.Pages.Count;
      }
      else if (this.ActivePage.ClickedTable != null)
      {
        TableDescription clickedTable = this.ActivePage.ClickedTable;
        element = clickedTable.SelectedElement;
        switch (element)
        {
          case SelectedElement.Table:
            index = this.ActivePage.Tables.IndexOf(clickedTable);
            elementsCount = this.ActivePage.Tables.Count;
            break;
          case SelectedElement.Column:
            index = clickedTable.ColumnClicked;
            elementsCount = clickedTable.ColumnsCount;
            break;
          case SelectedElement.Row:
            index = clickedTable.RowClicked - 1;
            elementsCount = clickedTable.RowsCount - 1;
            flag = clickedTable.UnitedRowIndex.Contains(clickedTable.RowClicked);
            num1 = clickedTable.ColumnsCount;
            break;
          case SelectedElement.Cell:
            index = clickedTable.RowClicked - 1;
            num1 = clickedTable.ColumnsCount;
            num2 = clickedTable.RowsCount - 1;
            break;
        }
      }
    }
    SelectedRibbonElementEventArgs e = new SelectedRibbonElementEventArgs(element, index, elementsCount)
    {
      IsUnitedRow = flag,
      ColumnCount = num1,
      RowCount = num2
    };
    SelectedRibbonElementEventHandler selectedElementChanged = this.SelectedElementChanged;
    if (selectedElementChanged == null)
      return;
    selectedElementChanged((object) this, e);
  }

  private DataTable ParseTable(XmlNode tableNode)
  {
    DataTable dataTable = (DataTable) null;
    XmlNodeList childNodes = tableNode.SelectSingleNode("columns")?.ChildNodes;
    if (childNodes != null && childNodes.Count > 0)
    {
      XmlAttribute attribute1 = tableNode.Attributes?["name"];
      dataTable = new DataTable(attribute1 != null ? attribute1.Value : string.Empty);
      foreach (XmlNode xmlNode in childNodes)
      {
        string columnName = xmlNode.Attributes?["id"]?.Value;
        if (!string.IsNullOrEmpty(columnName))
        {
          DataColumn column = new DataColumn(columnName);
          XmlAttribute attribute2 = xmlNode.Attributes["name"];
          column.Caption = attribute2 != null ? attribute2.Value : string.Empty;
          dataTable.Columns.Add(column);
        }
      }
      if (dataTable.Columns.Count > 0)
      {
        XmlNode xmlNode = tableNode.SelectSingleNode("rows");
        if (xmlNode != null)
        {
          foreach (XmlNode childNode in xmlNode.ChildNodes)
          {
            DataRow row = dataTable.NewRow();
            if (childNode.Name == "UnitedRow")
            {
              row.RowError = "United";
              XmlAttribute attribute3 = childNode.Attributes?["name"];
              if (attribute3 != null)
                row[0] = (object) attribute3.Value;
            }
            else
            {
              int count = dataTable.Columns.Count;
              for (int index = 0; index < count; ++index)
              {
                XmlAttribute attribute4 = childNode.Attributes?[dataTable.Columns[index].ColumnName];
                if (attribute4 != null)
                  row[index] = (object) attribute4.Value;
              }
            }
            dataTable.Rows.Add(row);
          }
        }
      }
    }
    return dataTable == null || dataTable.Rows.Count <= 0 ? (DataTable) null : dataTable;
  }

  private void ParseXml(XmlDocument doc)
  {
    if (doc == null)
      return;
    XmlNode xmlNode1 = doc.SelectSingleNode("doc/description");
    XmlAttribute xmlAttribute = (XmlAttribute) null;
    if (xmlNode1 != null)
      xmlAttribute = xmlNode1.Attributes?["name"];
    this.Caption = xmlAttribute != null ? xmlAttribute.Value : string.Empty;
    XmlNodeList xmlNodeList1 = doc.SelectNodes("//section");
    if (xmlNodeList1 == null || xmlNodeList1.Count <= 0)
      return;
    List<Tuple<string, IEnumerable<DataTable>>> parms = new List<Tuple<string, IEnumerable<DataTable>>>();
    foreach (XmlNode xmlNode2 in xmlNodeList1)
    {
      XmlAttribute attribute = xmlNode2.Attributes?["name"];
      XmlNodeList xmlNodeList2 = xmlNode2.SelectNodes("table");
      if (xmlNodeList2 != null)
      {
        List<DataTable> dataTableList = new List<DataTable>(xmlNodeList2.Count);
        foreach (XmlNode tableNode in xmlNodeList2)
        {
          DataTable table = this.ParseTable(tableNode);
          if (table != null)
            dataTableList.Add(table);
        }
        if (dataTableList.Count > 0)
          parms.Add(new Tuple<string, IEnumerable<DataTable>>(attribute != null ? attribute.Value : string.Empty, (IEnumerable<DataTable>) dataTableList));
      }
    }
    if (parms.Count <= 0)
      return;
    this.AddPages((IEnumerable<Tuple<string, IEnumerable<DataTable>>>) parms);
  }

  private string ReadBlob(IDBAttribute attr)
  {
    string str = string.Empty;
    if (attr != null)
    {
      try
      {
        if (attr is IBlobReader blobReader)
        {
          BlobInformation blobInformation = blobReader.OpenBlob(0);
          if (blobInformation.RealFileSize != 0L)
          {
            byte[] buffer = blobReader.ReadDataBlock();
            blobReader.CloseBlob();
            if (buffer != null)
            {
              if (buffer.Length > sc_14509.ssp_imbase_14510(1824475157))
              {
                IPackedStream service = ServiceUtils.GetService<IPackedStream>((object) ApplicationServices.Container, true);
                using (MemoryStream inStream = new MemoryStream(buffer))
                {
                  inStream.Position = 0L;
                  using (MemoryStream memoryStream = new MemoryStream((int) blobInformation.RealFileSize))
                  {
                    service.UnpackStream((Stream) memoryStream, (Stream) inStream);
                    memoryStream.Position = 0L;
                    using (BinaryReader binaryReader = new BinaryReader((Stream) memoryStream, Encoding.UTF8))
                      str = binaryReader.ReadString();
                  }
                }
              }
            }
          }
        }
      }
      catch (Exception ex)
      {
        return ex.Message;
      }
    }
    return str;
  }

  private XmlDocument StoreData()
  {
    XmlDocument doc = (XmlDocument) null;
    if (this.Pages.Count > 0)
    {
      doc = new XmlDocument();
      XmlNode element1 = (XmlNode) doc.CreateElement("doc");
      XmlNode element2 = (XmlNode) doc.CreateElement("sections");
      XmlElement element3 = doc.CreateElement("description");
      element3.SetAttribute("name", this.Caption);
      element1.AppendChild((XmlNode) element3);
      foreach (Page page in this.Pages)
      {
        XmlElement element4 = doc.CreateElement("section");
        if (!string.IsNullOrEmpty(page.Header.Text))
          element4.SetAttribute("name", page.Header.Text);
        foreach (TableDescription table in page.Tables)
        {
          XmlElement newChild = this.StoreDataForTableNode(doc, table);
          if (newChild != null)
            element4.AppendChild((XmlNode) newChild);
        }
        if (element4.ChildNodes.Count != 0)
          element2.AppendChild((XmlNode) element4);
      }
      element1.AppendChild(element2);
      doc.AppendChild(element1);
    }
    return doc;
  }

  private XmlElement StoreDataForTableNode(XmlDocument doc, TableDescription table)
  {
    XmlElement xmlElement = (XmlElement) null;
    if (doc != null && table != null)
    {
      DataTable table1 = table.Table;
      if (table1 != null && table1.Columns.Count > 0 && table1.Rows.Count > 0)
      {
        xmlElement = doc.CreateElement(nameof (table));
        if (!string.IsNullOrEmpty(table.Header.Text))
          xmlElement.SetAttribute("name", table.Header.Text);
        XmlElement element1 = doc.CreateElement("columns");
        foreach (DataColumn column in (InternalDataCollectionBase) table1.Columns)
        {
          XmlElement element2 = doc.CreateElement("Col");
          element2.SetAttribute("id", column.ColumnName);
          element2.SetAttribute("name", column.Caption);
          element1.AppendChild((XmlNode) element2);
        }
        xmlElement.AppendChild((XmlNode) element1);
        XmlElement element3 = doc.CreateElement("rows");
        for (int index = 0; index < table1.Rows.Count; ++index)
        {
          DataRow row = table1.Rows[index];
          int num = table.DrawTablesHeader ? index + 1 : index;
          XmlElement element4;
          if (table.UnitedRowIndex.Contains(num))
          {
            element4 = doc.CreateElement("UnitedRow");
            element4.SetAttribute("name", Convert.ToString(row[0]));
          }
          else
          {
            element4 = doc.CreateElement("row");
            foreach (DataColumn column in (InternalDataCollectionBase) table1.Columns)
            {
              string str = Convert.ToString(row[column.ColumnName]);
              if (!string.IsNullOrEmpty(str))
                element4.SetAttribute(column.ColumnName, str);
            }
          }
          element3.AppendChild((XmlNode) element4);
        }
        xmlElement.AppendChild((XmlNode) element3);
      }
    }
    return xmlElement;
  }

  private void WriteBlob(IDBAttribute attr, string strProperties)
  {
    IPackedStream service = ServiceUtils.GetService<IPackedStream>((object) ApplicationServices.Container, true);
    using (MemoryStream memoryStream = new MemoryStream(strProperties.Length))
    {
      using (BinaryWriter binaryWriter = new BinaryWriter((Stream) memoryStream))
      {
        binaryWriter.Write(strProperties);
        binaryWriter.Flush();
        memoryStream.Position = 0L;
        using (MemoryStream outStream = new MemoryStream((int) memoryStream.Length / 2))
        {
          service.PackStream((Stream) outStream, (Stream) memoryStream, 9);
          outStream.Position = 0L;
          byte[] buffer = outStream.GetBuffer();
          byte[] data = new byte[outStream.Length];
          byte[] dst = data;
          int length = (int) outStream.Length;
          Buffer.BlockCopy((Array) buffer, 0, (Array) dst, 0, length);
          try
          {
            BlobInformation blobInfo = new BlobInformation(outStream.Length, outStream.Length, DateTime.Now, string.Empty, ArcMethods.ZLibPacked, string.Empty);
            if (attr is IBlobWriter blobWriter)
              blobWriter.OpenBlob(blobInfo, false);
            blobWriter?.WriteDataBlock(data);
          }
          catch (Exception ex)
          {
            throw;
          }
        }
      }
    }
  }

  private void Page_PageSizeChanged(object sender, EventArgs e) => this.PerformLayout();

  private void Page_PageSelected(object sender, EventArgs e)
  {
    if (this.ActivePage != (Page) sender)
    {
      this.ActivePage?.LostSelection();
      this.ActivePage = (Page) sender;
    }
    this.Invalidate(true);
    this.OnSelectedElementChanged();
  }

  private void PropertiesCtrl_ValueChanged(object sender, EventArgs e) => this.OnDataChanged();

  public Page AddPage(
    string caption,
    IEnumerable<DataTable> tables = null,
    bool drawLines = true,
    bool drawTablesHeader = true,
    bool forbiddenColumnsAdd = false)
  {
    Page page = new Page(caption, tables, drawLines, drawTablesHeader, forbiddenColumnsAdd);
    this.Controls.Add((Control) page);
    this.PerformLayout();
    return page;
  }

  public Page[] AddPages(
    IEnumerable<Tuple<string, IEnumerable<DataTable>>> parms)
  {
    Page[] array = parms.ToList<Tuple<string, IEnumerable<DataTable>>>().Select<Tuple<string, IEnumerable<DataTable>>, Page>((System.Func<Tuple<string, IEnumerable<DataTable>>, Page>) (x => new Page(x.Item1, x.Item2))).ToArray<Page>();
    if (array.Length != 0)
      this.Controls.AddRange((Control[]) array);
    return array;
  }

  public void Clear(bool bInvalidate)
  {
    this._objPropsID = 0L;
    this.ActivePage = (Page) null;
    this.Caption = string.Empty;
    this.SuspendLayout();
    this.Controls.OfType<Page>().ToList<Page>().ForEach((Action<Page>) (x => this.Controls.Remove((Control) x)));
    this.ResumeLayout();
    if (!bInvalidate)
      return;
    this.Invalidate(true);
  }

  public void ExpandAll(bool isExpand)
  {
    foreach (Page page in this.Pages)
    {
      page.IsExpanded = isExpand;
      page.ExpandAllTables(page.IsExpanded);
    }
    this.Invalidate();
  }

  public void OnBtnAddClick()
  {
    if (this.ActivePage != null)
    {
      if (!this.ActivePage.IsTableClicked)
      {
        int childIndex = this.Controls.GetChildIndex((Control) this.ActivePage);
        Page child = this.AddPage(string.Empty);
        int num;
        this.Controls.SetChildIndex((Control) child, num = childIndex + 1);
        child.Selected = true;
        child.Focus();
      }
      else if (this.ActivePage.ClickedTable != null)
      {
        switch (this.ActivePage.ClickedTable.SelectedElement)
        {
          case SelectedElement.Table:
            TableDescription tableDescription = this.ActivePage.AddTable();
            this.ActivePage.ClickedTable = tableDescription;
            tableDescription.Selected = true;
            tableDescription.Focus();
            break;
          case SelectedElement.Column:
            this.ActivePage.ClickedTable.AddColumn();
            break;
          case SelectedElement.Row:
            this.ActivePage.ClickedTable.AddRow();
            break;
          case SelectedElement.Cell:
            if (this.ActivePage.ClickedTable.ColumnsCount == 1)
            {
              this.ActivePage.ClickedTable.AddRow();
              break;
            }
            break;
        }
      }
    }
    else
    {
      Page page = this.AddPage(string.Empty);
      page.Selected = true;
      page.Focus();
    }
    this.OnSelectedElementChanged();
  }

  public void OnBtnEditClick()
  {
    if (this.ActivePage == null)
      return;
    TableDescription clickedTable = this.ActivePage.ClickedTable;
    if (clickedTable != null)
      clickedTable.EditValue();
    else
      this.ActivePage.EditValue();
  }

  public void OnBtnMoveBegClick()
  {
    if (this.ActivePage != null)
    {
      if (!this.ActivePage.IsTableClicked)
      {
        this.Controls.SetChildIndex((Control) this.ActivePage, 0);
        this.ActivePage.Focus();
      }
      else if (this.ActivePage.ClickedTable != null)
      {
        switch (this.ActivePage.ClickedTable.SelectedElement)
        {
          case SelectedElement.Table:
            this.ActivePage.MoveClickedTableBegin();
            break;
          case SelectedElement.Column:
            this.ActivePage.ClickedTable.MoveClickedColumnBegin();
            break;
          case SelectedElement.Row:
          case SelectedElement.Cell:
            this.ActivePage.ClickedTable.MoveClickedRowBegin();
            break;
        }
      }
    }
    this.OnSelectedElementChanged();
  }

  public void OnBtnMoveUpClick()
  {
    if (this.ActivePage != null)
    {
      if (!this.ActivePage.IsTableClicked)
      {
        int childIndex = this.Controls.GetChildIndex((Control) this.ActivePage);
        if (childIndex > 0)
          this.Controls.SetChildIndex((Control) this.ActivePage, childIndex - 1);
      }
      else if (this.ActivePage.ClickedTable != null)
      {
        switch (this.ActivePage.ClickedTable.SelectedElement)
        {
          case SelectedElement.Table:
            this.ActivePage.MoveClickedTableUp();
            break;
          case SelectedElement.Row:
          case SelectedElement.Cell:
            this.ActivePage.ClickedTable.MoveClickedRowUp();
            break;
        }
      }
    }
    this.OnSelectedElementChanged();
  }

  public void OnBtnMoveDownClick()
  {
    if (this.ActivePage != null)
    {
      if (!this.ActivePage.IsTableClicked)
      {
        int childIndex = this.Controls.GetChildIndex((Control) this.ActivePage);
        if (childIndex < this.Pages.Count - 1)
          this.Controls.SetChildIndex((Control) this.ActivePage, childIndex + 1);
      }
      else if (this.ActivePage.ClickedTable != null)
      {
        switch (this.ActivePage.ClickedTable.SelectedElement)
        {
          case SelectedElement.Table:
            this.ActivePage.MoveClickedTableDown();
            break;
          case SelectedElement.Row:
          case SelectedElement.Cell:
            this.ActivePage.ClickedTable.MoveClickedRowDown();
            break;
        }
      }
    }
    this.OnSelectedElementChanged();
  }

  public void OnBtnMoveEndClick()
  {
    if (this.ActivePage != null)
    {
      if (!this.ActivePage.IsTableClicked)
      {
        this.Controls.SetChildIndex((Control) this.ActivePage, this.Pages.Count - 1);
        this.ActivePage.Focus();
      }
      else if (this.ActivePage.ClickedTable != null)
      {
        switch (this.ActivePage.ClickedTable.SelectedElement)
        {
          case SelectedElement.Table:
            this.ActivePage.MoveClickedTableEnd();
            break;
          case SelectedElement.Column:
            this.ActivePage.ClickedTable.MoveClickedColumnEnd();
            break;
          case SelectedElement.Row:
          case SelectedElement.Cell:
            this.ActivePage.ClickedTable.MoveClickedRowEnd();
            break;
        }
      }
    }
    this.OnSelectedElementChanged();
  }

  public void OnBtnMoveLeftClick()
  {
    if (this.ActivePage != null && this.ActivePage.IsTableClicked)
    {
      TableDescription clickedTable = this.ActivePage.ClickedTable;
      if (clickedTable != null && clickedTable.SelectedElement == SelectedElement.Column)
        clickedTable.MoveClickedColumnLeft();
    }
    this.OnSelectedElementChanged();
  }

  public void OnBtnMoveRightClick()
  {
    if (this.ActivePage != null && this.ActivePage.IsTableClicked)
    {
      TableDescription clickedTable = this.ActivePage.ClickedTable;
      if (clickedTable != null && clickedTable.SelectedElement == SelectedElement.Column)
        clickedTable.MoveClickedColumnRight();
    }
    this.OnSelectedElementChanged();
  }

  public void OnBtnRemoveClick()
  {
    if (this.ActivePage != null)
    {
      Form form = this.FindForm();
      string caption = LocalizationHolder.rm.GetString("IMH_DeleteData_Caption");
      if (!this.ActivePage.IsTableClicked)
      {
        string text = LocalizationHolder.rm.GetString("IMH_DeletePage_Msg");
        if (MessageBox.Show((IWin32Window) form, text, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
        {
          int index = this.Controls.IndexOf((Control) this.ActivePage);
          this.Controls.Remove((Control) this.ActivePage);
          if (index != 0)
          {
            this.ActivePage = index < this.Pages.Count ? this.Pages[index] : this.Pages[index - 1];
            this.ActivePage.Selected = true;
            this.ActivePage.Focus();
          }
          else
            this.ActivePage = (Page) null;
        }
      }
      else if (this.ActivePage.ClickedTable != null)
      {
        switch (this.ActivePage.ClickedTable.SelectedElement)
        {
          case SelectedElement.Table:
            if (this.ActivePage.Tables.Count > 1)
            {
              string text = LocalizationHolder.rm.GetString("IMH_DeleteTable_Msg");
              if (MessageBox.Show((IWin32Window) form, text, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
              {
                this.ActivePage.RemoveClickedTable();
                break;
              }
              break;
            }
            break;
          case SelectedElement.Column:
            if (this.ActivePage.ClickedTable.CanRemoveColumn)
            {
              string text = LocalizationHolder.rm.GetString("IMH_DeleteColumn_Msg");
              if (MessageBox.Show((IWin32Window) form, text, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
              {
                this.ActivePage.ClickedTable.RemoveClickedColumn();
                break;
              }
              break;
            }
            break;
          case SelectedElement.Row:
          case SelectedElement.Cell:
            if (this.ActivePage.ClickedTable.CanRemoveRows)
            {
              string text = LocalizationHolder.rm.GetString("IMH_DeleteString_Msg");
              if (MessageBox.Show((IWin32Window) form, text, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
              {
                this.ActivePage.ClickedTable.RemoveClickedRow();
                break;
              }
              break;
            }
            break;
        }
      }
    }
    this.OnSelectedElementChanged();
  }

  public void OnBtnUnionClick(bool isCombine)
  {
    if (this.ActivePage?.ClickedTable != null && this.ActivePage.ClickedTable.SelectedElement == SelectedElement.Row)
      this.ActivePage.ClickedTable.CombineRow(this.ActivePage.ClickedTable.RowClicked, isCombine);
    this.OnSelectedElementChanged();
  }

  public void ReloadSettingsData() => this.ImbaseKey = this._imbaseKey;

  public void SaveProperties()
  {
    XmlDocument xmlDocument = this.StoreData();
    bool flag1 = false;
    bool flag2 = false;
    bool flag3 = false;
    bool flag4 = this._objPropsID == 0L;
    if (flag4)
    {
      using (MaterialPropertiesFileNameDlg propertiesFileNameDlg = new MaterialPropertiesFileNameDlg())
      {
        if (propertiesFileNameDlg.ShowDialog() == DialogResult.OK)
          this.Caption = propertiesFileNameDlg.FileName;
        else
          flag1 = true;
      }
    }
    if (flag1)
      return;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      long tableId = 0;
      IDBObject objectActualCopy;
      if (!flag4)
      {
        objectActualCopy = sessionKeeper.Session.GetObjectActualCopy(this._objPropsID, false);
        if (objectActualCopy == null)
          throw new Exception(string.Format(LocalizationHolder.rm.GetString("IMH_GetObjectError_Msg"), (object) this._objPropsID));
      }
      else
      {
        objectActualCopy = (sessionKeeper.Session.GetObjectCollection(Intermech.Imbase.Consts.MaterialPropertiesObjTypeGuid) ?? throw new Exception(LocalizationHolder.rm.GetString("IMH_EmptyPropertiesCollection_Msg"))).Create();
        if (objectActualCopy == null)
          throw new Exception(LocalizationHolder.rm.GetString("IMH_CreateObjectError_Msg"));
        objectActualCopy.Caption = this.Caption;
        objectActualCopy.CommitCreation(true);
        this._objPropsID = objectActualCopy.ObjectID;
        flag2 = true;
        flag3 = this.AddSettings(sessionKeeper.Session, this._objPropsID, out tableId);
      }
      this.WriteBlob(objectActualCopy.GetAttributeByGuid(new Guid("cadd93d3-306c-11d8-b4e9-00304f19f545")) ?? throw new Exception(LocalizationHolder.rm.GetString("IMH_GetAttributeError_Msg")), xmlDocument?.InnerXml ?? string.Empty);
      INotificationService service = ServiceUtils.GetService<INotificationService>((object) ApplicationServices.Container, false);
      if (service != null)
      {
        service.FireEvent((object) this, (NotificationEventArgs) new DBObjectsEventArgs(flag2 ? "ObjectsCreated" : "ObjectsChanged", this._objPropsID));
        if (flag3 && tableId != 0L)
          service.FireEvent((object) this, (NotificationEventArgs) new DBObjectsEventArgs("ObjectsChanged", tableId));
      }
      if (!flag4)
        return;
      this.OnCaptionChanged();
    }
  }

  public void LeaveProperties() => this.OnLeave(EventArgs.Empty);

  private void PropertiesCtrl_ControlAdded(object sender, ControlEventArgs e)
  {
    if (!(e.Control is Page control))
      return;
    control.PageSelected += new EventHandler(this.Page_PageSelected);
    control.BeforePageClicked += new EventHandler(this.PropertiesCtrl_BeforePageClicked);
    control.PageSizeChanged += new EventHandler(this.Page_PageSizeChanged);
    control.ValueChanged += new EventHandler(this.PropertiesCtrl_ValueChanged);
    control.Enabled = !this.ReadOnly;
  }

  private void PropertiesCtrl_BeforePageClicked(object sender, EventArgs e)
  {
    this.Pages.ForEach((Action<Page>) (x => x.LostSelection()));
  }

  private void PropertiesCtrl_ControlRemoved(object sender, ControlEventArgs e)
  {
    if (!(e.Control is Page))
      return;
    ((Page) e.Control).PageSelected -= new EventHandler(this.Page_PageSelected);
    ((Page) e.Control).BeforePageClicked -= new EventHandler(this.PropertiesCtrl_BeforePageClicked);
    ((Page) e.Control).PageSizeChanged -= new EventHandler(this.Page_PageSizeChanged);
    ((ControlWithEditor) e.Control).ValueChanged -= new EventHandler(this.PropertiesCtrl_ValueChanged);
  }

  protected virtual void OnDataChanged()
  {
    EventHandler dataChanged = this.DataChanged;
    if (dataChanged == null)
      return;
    dataChanged((object) this, EventArgs.Empty);
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (PropertiesCtrl));
    this.SuspendLayout();
    componentResourceManager.ApplyResources((object) this, "$this");
    this.AutoScaleMode = AutoScaleMode.Font;
    this.BackColor = SystemColors.Window;
    this.BorderStyle = BorderStyle.FixedSingle;
    this.DoubleBuffered = true;
    this.Name = nameof (PropertiesCtrl);
    this.ControlAdded += new ControlEventHandler(this.PropertiesCtrl_ControlAdded);
    this.ControlRemoved += new ControlEventHandler(this.PropertiesCtrl_ControlRemoved);
    this.ResumeLayout(false);
  }

  private enum MovePosition
  {
    Beg,
    Up,
    Left,
    Right,
    Down,
    End,
  }
}
