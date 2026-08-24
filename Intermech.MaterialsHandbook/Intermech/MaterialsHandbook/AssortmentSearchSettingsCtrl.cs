// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.AssortmentSearchSettingsCtrl
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using Intermech.Client.Core;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.MaterialsHandbook;
using Intermech.Kernel.Search;
using Intermech.PropertyEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace Intermech.MaterialsHandbook;

public class AssortmentSearchSettingsCtrl : UserControl
{
  private bool _lock;
  private Dictionary<IMHAssortmentClass, bool> _classesList = new Dictionary<IMHAssortmentClass, bool>();
  private IMHAssortmentClass _currentClass;
  private string _currentAbstractParamName = string.Empty;
  private Dictionary<string, AttributeTypeProperties> _typeProps = new Dictionary<string, AttributeTypeProperties>();
  private IContainer components;
  private Label _lbSelectedValues;
  private Button _btnAdd;
  private Button _btnDel;
  private ListView _lvAttrs;
  private Label _lbAttrs;
  private ImageList _imgButtons;
  private ListView _lvSelectedValues;
  private TableLayoutPanel _tlpTop;
  private ListView _lvAllValues;
  private System.Windows.Forms.ColumnHeader _colAllValues;
  private Button _btnRight;
  private Button _btnLeft;
  private Label _lbAllValues;
  private System.Windows.Forms.ColumnHeader _colAttrs;
  private System.Windows.Forms.ColumnHeader _colSelectedValues;
  private SplitContainer _slt;
  private SplitContainer _spltBottom;
  private Label _lbAbstractParam;
  private ListBox _lstbAbstractParam;
  private TextBox _txtAbstractParam;
  private Button _btnAddAbstractParam;
  private Button _btnDelAbstractParam;
  private Panel _pnlRight;
  private Panel _pnlLeft;
  private Panel _pnlMaterials;
  private GroupBox _grbMaterials;
  private CheckBox _chbMaterials;

  public Dictionary<string, List<Guid>> AddedAttributes { get; }

  public bool NeedIndexMaterial => this._chbMaterials.Checked;

  public List<IMHAssortmentClass> Settings
  {
    get
    {
      return this._classesList.Where<KeyValuePair<IMHAssortmentClass, bool>>((System.Func<KeyValuePair<IMHAssortmentClass, bool>, bool>) (x => x.Value)).Select<KeyValuePair<IMHAssortmentClass, bool>, IMHAssortmentClass>((System.Func<KeyValuePair<IMHAssortmentClass, bool>, IMHAssortmentClass>) (x => x.Key)).ToList<IMHAssortmentClass>();
    }
    set
    {
      this._classesList = (value != null ? value.ToDictionary<IMHAssortmentClass, IMHAssortmentClass, bool>((System.Func<IMHAssortmentClass, IMHAssortmentClass>) (x => x), (System.Func<IMHAssortmentClass, bool>) (y => true)) : (Dictionary<IMHAssortmentClass, bool>) null) ?? new Dictionary<IMHAssortmentClass, bool>(0);
    }
  }

  public Dictionary<string, List<Guid>> RemovedAttributes { get; }

  public AssortmentSearchSettingsCtrl()
  {
    this.InitializeComponent();
    this.AddedAttributes = new Dictionary<string, List<Guid>>();
    this.RemovedAttributes = new Dictionary<string, List<Guid>>();
  }

  public event EventHandler Changed;

  private void On_btnAdd_Click(object sender, EventArgs e)
  {
    if (this._lstbAbstractParam.SelectedItems.Count <= 0)
      return;
    using (AttributesSelectDlg attributesSelectDlg = new AttributesSelectDlg(true))
    {
      FieldTypes[] collection = new FieldTypes[7]
      {
        FieldTypes.ftBlob,
        FieldTypes.ftFile,
        FieldTypes.ftShortBlob,
        FieldTypes.ftSystem,
        FieldTypes.ftExternalLink,
        FieldTypes.ftPassword,
        FieldTypes.ftAutoInc
      };
      attributesSelectDlg.ShowCreateAttrBtn = false;
      attributesSelectDlg.SelectorFilter = (ISelectorFilter) new ForbiddenAttrs(this.AddedAttrsIDs());
      attributesSelectDlg.ForbiddenAttrsTypesFilter.AddRange((IEnumerable<FieldTypes>) collection);
      if (attributesSelectDlg.ShowDialog((IWin32Window) this) != DialogResult.OK || attributesSelectDlg.SelectedAttributesGuid.Count <= 0)
        return;
      foreach (Guid guid in attributesSelectDlg.SelectedAttributesGuid)
      {
        string str = Convert.ToString((object) guid);
        AttributeTypeProperties attributeProperties = this.GetAttributeProperties(guid);
        if (!(attributeProperties.AttributeGuid == Guid.Empty))
        {
          this.AddListViewItem(this._lvAttrs, str, attributeProperties.Name, Statics.IconSrv.IndexOf(3, -1, (object) attributeProperties.FieldType));
          this._currentClass.AddAttribute(this._currentAbstractParamName, str, false);
          this.SaveChangedInfo(this.RemovedAttributes, this.AddedAttributes, guid);
        }
      }
      this.OnChanged();
    }
  }

  private void On_btnAddAbstractParam_Click(object sender, EventArgs e)
  {
    string text = this._txtAbstractParam.Text;
    if (string.IsNullOrEmpty(text))
      return;
    if (this._currentClass.Parameters.ContainsKey(text))
    {
      foreach (object obj in this._lstbAbstractParam.Items)
      {
        if (!(Convert.ToString(obj) != text))
        {
          this._lstbAbstractParam.SelectedItem = obj;
          break;
        }
      }
    }
    else
    {
      this._lstbAbstractParam.SelectedIndex = this._lstbAbstractParam.Items.Add((object) text);
      this._currentClass.AddAbstractName(text);
    }
  }

  private void On_btnDel_Click(object sender, EventArgs e)
  {
    if (this._lvAttrs.SelectedItems.Count <= 0)
      return;
    ListViewItem[] dest = new ListViewItem[this._lvAttrs.SelectedItems.Count];
    this._lvAttrs.SelectedItems.CopyTo((Array) dest, 0);
    foreach (ListViewItem listViewItem in dest)
    {
      this._lvAttrs.Items.Remove(listViewItem);
      this._currentClass.DelAttribute(this._currentAbstractParamName, listViewItem.Name);
      this.SaveChangedInfo(this.AddedAttributes, this.RemovedAttributes, new Guid(listViewItem.Name));
    }
    this.OnChanged();
  }

  private void On_btnDelAbstractParam_Click(object sender, EventArgs e)
  {
    int num = -1;
    try
    {
      this._lstbAbstractParam.SuspendLayout();
      if (this._lstbAbstractParam.SelectedItem != null)
      {
        num = this._lstbAbstractParam.SelectedIndex;
        object selectedItem = this._lstbAbstractParam.SelectedItem;
        this._currentClass.DelAbstractName(Convert.ToString(selectedItem));
        this._lstbAbstractParam.Items.Remove(selectedItem);
        this.OnChanged();
      }
      if (this._lstbAbstractParam.Items.Count <= 0)
        return;
      if (num == -1)
        this._lstbAbstractParam.SelectedIndex = 0;
      else if (num < this._lstbAbstractParam.Items.Count)
        this._lstbAbstractParam.SelectedIndex = num;
      else
        this._lstbAbstractParam.SelectedIndex = this._lstbAbstractParam.Items.Count - 1;
    }
    finally
    {
      this._lstbAbstractParam.ResumeLayout();
    }
  }

  private void On_btnLeftRight_Click(object sender, EventArgs e)
  {
    int int16 = (int) Convert.ToInt16(sender is Button button1 ? button1.Tag : (object) null);
    this._lvAllValues.SuspendLayout();
    this._lvSelectedValues.SuspendLayout();
    try
    {
      if (int16 == 0)
      {
        while (this._lvSelectedValues.SelectedItems.Count > 0)
          this._lvSelectedValues.SelectedItems[0].Selected = false;
        while (this._lvAllValues.SelectedItems.Count > 0)
        {
          ListViewItem selectedItem = this._lvAllValues.SelectedItems[0];
          IMHAssortmentClass classByName = this.GetClassByName(selectedItem.Text);
          if (classByName == null)
            this._classesList.Add(new IMHAssortmentClass(selectedItem.Text), true);
          else
            this._classesList[classByName] = true;
          this._lvAllValues.Items.Remove(selectedItem);
          this._lvSelectedValues.Items.Add(selectedItem);
          selectedItem.Focused = true;
        }
      }
      else
      {
        while (this._lvAllValues.SelectedItems.Count > 0)
          this._lvAllValues.SelectedItems[0].Selected = false;
        while (this._lvSelectedValues.SelectedItems.Count > 0)
        {
          ListViewItem selectedItem = this._lvSelectedValues.SelectedItems[0];
          this._lvSelectedValues.Items.Remove(selectedItem);
          this._lvAllValues.Items.Add(selectedItem);
          this._classesList[this._currentClass] = false;
          selectedItem.Focused = true;
        }
      }
    }
    finally
    {
      this._lvAllValues.ResumeLayout();
      this._lvSelectedValues.ResumeLayout();
    }
    if (sender is Button button2)
      button2.Focus();
    this.OnChanged();
  }

  private void On_chbMaterials_CheckedChanged(object sender, EventArgs e) => this.OnChanged();

  private void On_lstbAbstractParam_SelectedValueChanged(object sender, EventArgs e)
  {
    this._lvAttrs.Items.Clear();
    if (this._lstbAbstractParam.SelectedItems.Count > 0)
    {
      this._btnDelAbstractParam.Enabled = this._btnAdd.Enabled = true;
      this._currentAbstractParamName = this._lstbAbstractParam.SelectedItems[0].ToString();
      Dictionary<string, List<string>> parameters = this._currentClass.Parameters;
      if (parameters == null || !parameters.ContainsKey(this._currentAbstractParamName))
        return;
      List<string> stringList = parameters[this._currentAbstractParamName];
      if (stringList == null)
        return;
      foreach (string str in stringList)
      {
        if (this._typeProps.ContainsKey(str))
        {
          AttributeTypeProperties typeProp = this._typeProps[str];
          this.AddListViewItem(this._lvAttrs, str, typeProp.Name, Statics.IconSrv.IndexOf(3, -1, (object) typeProp.FieldType));
        }
        else if (GuidHelper.IsGuid(str))
        {
          AttributeTypeProperties attributeProperties = this.GetAttributeProperties(new Guid(str));
          if (!(attributeProperties.AttributeGuid == Guid.Empty))
            this.AddListViewItem(this._lvAttrs, str, attributeProperties.Name, Statics.IconSrv.IndexOf(3, -1, (object) attributeProperties.FieldType));
        }
      }
      if (this._lvAttrs.Items.Count <= 0)
        return;
      this._lvAttrs.Items[0].Selected = true;
    }
    else
    {
      this._currentAbstractParamName = string.Empty;
      this._btnDelAbstractParam.Enabled = this._btnAdd.Enabled = false;
    }
  }

  private void On_lv_SizeChanged(object sender, EventArgs e)
  {
    if (this._lock || !(sender is ListView listView) || listView.Columns.Count == 0 || listView.Columns[0] == null)
      return;
    this._lock = true;
    listView.Columns[0].Width = -2;
    this._lock = false;
  }

  private void On_lvAllValues_DoubleClick(object sender, EventArgs e)
  {
    this.On_btnLeftRight_Click((object) this._btnRight, e);
  }

  private void On_lvAllValues_SelectedIndexChanged(object sender, EventArgs e)
  {
    this._btnRight.Enabled = this._lvAllValues.SelectedItems.Count > 0;
  }

  private void On_lvAttrs_SelectedIndexChanged(object sender, EventArgs e)
  {
    this._btnDel.Enabled = this._lvAttrs.SelectedItems.Count > 0;
  }

  private void On_lvSelectedValues_DoubleClick(object sender, EventArgs e)
  {
    this.On_btnLeftRight_Click((object) this._btnLeft, e);
  }

  private void On_lvSelectedValues_SelectedIndexChanged(object sender, EventArgs e)
  {
    this._lstbAbstractParam.Items.Clear();
    this._lvAttrs.Items.Clear();
    if (this._lvSelectedValues.SelectedItems.Count == 1)
    {
      this._btnLeft.Enabled = this._btnAddAbstractParam.Enabled = true;
      this._currentClass = this.GetClassByName(this._lvSelectedValues.SelectedItems[0].Text);
      Dictionary<string, List<string>> parameters = this._currentClass.Parameters;
      if (parameters != null)
        this._lstbAbstractParam.Items.AddRange((object[]) parameters.Keys.ToArray<string>());
      if (this._lstbAbstractParam.Items.Count <= 0)
        return;
      this._lstbAbstractParam.SelectedIndex = 0;
    }
    else
    {
      this._btnLeft.Enabled = this._lvSelectedValues.SelectedItems.Count > 0;
      this._btnAddAbstractParam.Enabled = false;
      this._btnAddAbstractParam.Enabled = this._btnDelAbstractParam.Enabled = this._btnAdd.Enabled = this._btnDel.Enabled = false;
    }
  }

  private List<int> AddedAttrsIDs()
  {
    List<int> intList = new List<int>(this._lvAttrs.Items.Count);
    foreach (ListViewItem listViewItem in this._lvAttrs.Items)
    {
      if (this._typeProps.ContainsKey(listViewItem.Name))
      {
        AttributeTypeProperties typeProp = this._typeProps[listViewItem.Name];
        if (!intList.Contains(typeProp.AttributeID))
          intList.Add(typeProp.AttributeID);
      }
    }
    return intList;
  }

  private void AddListViewItem(ListView lv, string name, string text, int imgIndex)
  {
    lv.SuspendLayout();
    try
    {
      ListViewItem listViewItem = new ListViewItem(text, imgIndex)
      {
        Name = name
      };
      lv.Items.Add(listViewItem);
    }
    finally
    {
      lv.ResumeLayout();
    }
  }

  private void Clear()
  {
    this._lvAllValues.Items.Clear();
    this._lvSelectedValues.Items.Clear();
    this._lstbAbstractParam.Items.Clear();
    this._lvAttrs.Items.Clear();
    this._btnRight.Enabled = this._btnLeft.Enabled = false;
    this._btnAddAbstractParam.Enabled = this._btnDelAbstractParam.Enabled = false;
    this._btnAdd.Enabled = this._btnDel.Enabled = false;
  }

  private AttributeTypeProperties GetAttributeProperties(Guid attrGuid)
  {
    AttributeTypeProperties attributeProperties = new AttributeTypeProperties();
    string key = Convert.ToString((object) attrGuid);
    if (this._typeProps.ContainsKey(key))
    {
      attributeProperties = this._typeProps[key];
    }
    else
    {
      IDBAttributeTypeInfo attributeType = ApplicationServices.Container.GetService<IClientMetadataCache>().GetAttributeType(attrGuid, false);
      if (attributeType != null)
      {
        AttributeTypeProperties attributeTypeProperties = new AttributeTypeProperties(attributeType.Name, attributeType.AttributeType)
        {
          AttributeGuid = attrGuid,
          AttributeID = attributeType.PropertiesStructure.AttributeID
        };
        if (!this._typeProps.ContainsKey(key))
          this._typeProps.Add(key, attributeTypeProperties);
        attributeProperties = attributeTypeProperties;
      }
    }
    return attributeProperties;
  }

  private DataTable GetData()
  {
    DataTable data = (DataTable) null;
    string classifFolderKey = IMHHelper.GetClassifFolderKey("ASSORTMENT_FOLDER_NAME");
    if (!string.IsNullOrEmpty(classifFolderKey))
    {
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        IDBObjectCollection objectCollection = sessionKeeper.Session.GetObjectCollection(Intermech.Imbase.Consts.ImbaseTableRefTypeID);
        if (objectCollection != null)
        {
          List<ColumnDescriptor> columnDescriptorList = new List<ColumnDescriptor>(1)
          {
            new ColumnDescriptor((object) Intermech.Imbase.Consts.ClassAttrID, AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.ID, SortOrders.NONE, 0)
          };
          DBRecordSetParams paramSet = new DBRecordSetParams(new ConditionStructure[2]
          {
            new ConditionStructure(Intermech.Imbase.Consts.ClassifFolderKeyAttId, RelationalOperators.StartString, (object) classifFolderKey, LogicalOperators.AND, 0, false),
            new ConditionStructure(Intermech.Imbase.Consts.ClassAttrID, RelationalOperators.NotEmpty, (object) null, LogicalOperators.NONE, 0, false)
          }, columnDescriptorList.ToArray());
          data = objectCollection.Select(paramSet);
        }
      }
    }
    return data;
  }

  private IMHAssortmentClass GetClassByName(string className)
  {
    IMHAssortmentClass classByName = (IMHAssortmentClass) null;
    if (this._classesList != null)
    {
      foreach (KeyValuePair<IMHAssortmentClass, bool> classes in this._classesList)
      {
        if (!(classes.Key.Name != className))
        {
          classByName = classes.Key;
          break;
        }
      }
    }
    return classByName;
  }

  private void LoadClassValues()
  {
    DataTable data = this.GetData();
    if (data == null)
      return;
    List<string> stringList = new List<string>();
    string columnName = Convert.ToString(Intermech.Imbase.Consts.ClassAttrID);
    this._lvAllValues.SuspendLayout();
    try
    {
      foreach (DataRow row in (InternalDataCollectionBase) data.Rows)
      {
        string str = Convert.ToString(row[columnName]);
        if (!stringList.Contains(str) && this.GetClassByName(str) == null)
        {
          stringList.Add(str);
          this._lvAllValues.Items.Add(new ListViewItem(str));
        }
      }
    }
    finally
    {
      this._lvAllValues.ResumeLayout();
    }
  }

  private void OnChanged()
  {
    EventHandler changed = this.Changed;
    if (changed == null)
      return;
    changed((object) this, new EventArgs());
  }

  private void SaveChangedInfo(
    Dictionary<string, List<Guid>> firstList,
    Dictionary<string, List<Guid>> secondList,
    Guid g)
  {
    string name = this._currentClass.Name;
    if (firstList.ContainsKey(name) && firstList[name].Contains(g))
    {
      firstList[name].Remove(g);
      if (firstList[name].Count != 0)
        return;
      firstList.Remove(name);
    }
    else if (secondList.ContainsKey(name))
    {
      if (secondList[name].Contains(g))
        return;
      secondList[name].Add(g);
    }
    else
      secondList.Add(name, new List<Guid>((IEnumerable<Guid>) new Guid[1]
      {
        g
      }));
  }

  public void CancelChanged() => this.LoadData();

  public void ClearLists()
  {
    this.AddedAttributes.Clear();
    this.RemovedAttributes.Clear();
  }

  public void LoadData()
  {
    this.Clear();
    if (Statics.IconSrv != null)
      this._lvAttrs.SmallImageList = Statics.IconSrv.ImageList;
    this.LoadClassValues();
    if (this._lvAllValues.Items.Count > 0)
      this._lvAllValues.Items[0].Selected = true;
    if (this._classesList.Count <= 0)
      return;
    this._lvSelectedValues.Items.AddRange(this._classesList.Where<KeyValuePair<IMHAssortmentClass, bool>>((System.Func<KeyValuePair<IMHAssortmentClass, bool>, bool>) (x => x.Value)).Select<KeyValuePair<IMHAssortmentClass, bool>, ListViewItem>((System.Func<KeyValuePair<IMHAssortmentClass, bool>, ListViewItem>) (x => new ListViewItem(x.Key.Name))).ToArray<ListViewItem>());
    if (this._lvSelectedValues.Items.Count <= 0)
      return;
    this._lvSelectedValues.Items[0].Selected = true;
  }

  private void AssortmentSearchSettingsCtrl_Resize(object sender, EventArgs e)
  {
    this._txtAbstractParam.Width = this._btnAddAbstractParam.Left - this._txtAbstractParam.Left - 8;
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.components = (IContainer) new System.ComponentModel.Container();
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (AssortmentSearchSettingsCtrl));
    this._lbSelectedValues = new Label();
    this._btnAdd = new Button();
    this._btnDel = new Button();
    this._lvAttrs = new ListView();
    this._colAttrs = new System.Windows.Forms.ColumnHeader();
    this._lbAttrs = new Label();
    this._imgButtons = new ImageList(this.components);
    this._lvSelectedValues = new ListView();
    this._colSelectedValues = new System.Windows.Forms.ColumnHeader();
    this._tlpTop = new TableLayoutPanel();
    this._lvAllValues = new ListView();
    this._colAllValues = new System.Windows.Forms.ColumnHeader();
    this._lbAllValues = new Label();
    this._btnLeft = new Button();
    this._btnRight = new Button();
    this._slt = new SplitContainer();
    this._spltBottom = new SplitContainer();
    this._lstbAbstractParam = new ListBox();
    this._lbAbstractParam = new Label();
    this._pnlLeft = new Panel();
    this._txtAbstractParam = new TextBox();
    this._btnAddAbstractParam = new Button();
    this._btnDelAbstractParam = new Button();
    this._pnlRight = new Panel();
    this._pnlMaterials = new Panel();
    this._grbMaterials = new GroupBox();
    this._chbMaterials = new CheckBox();
    this._tlpTop.SuspendLayout();
    this._slt.BeginInit();
    this._slt.Panel1.SuspendLayout();
    this._slt.Panel2.SuspendLayout();
    this._slt.SuspendLayout();
    this._spltBottom.BeginInit();
    this._spltBottom.Panel1.SuspendLayout();
    this._spltBottom.Panel2.SuspendLayout();
    this._spltBottom.SuspendLayout();
    this._pnlLeft.SuspendLayout();
    this._pnlRight.SuspendLayout();
    this._pnlMaterials.SuspendLayout();
    this._grbMaterials.SuspendLayout();
    this.SuspendLayout();
    componentResourceManager.ApplyResources((object) this._lbSelectedValues, "_lbSelectedValues");
    this._lbSelectedValues.Name = "_lbSelectedValues";
    componentResourceManager.ApplyResources((object) this._btnAdd, "_btnAdd");
    this._btnAdd.Name = "_btnAdd";
    this._btnAdd.UseVisualStyleBackColor = true;
    this._btnAdd.Click += new EventHandler(this.On_btnAdd_Click);
    componentResourceManager.ApplyResources((object) this._btnDel, "_btnDel");
    this._btnDel.Name = "_btnDel";
    this._btnDel.UseVisualStyleBackColor = true;
    this._btnDel.Click += new EventHandler(this.On_btnDel_Click);
    this._lvAttrs.Columns.AddRange(new System.Windows.Forms.ColumnHeader[1]
    {
      this._colAttrs
    });
    componentResourceManager.ApplyResources((object) this._lvAttrs, "_lvAttrs");
    this._lvAttrs.FullRowSelect = true;
    this._lvAttrs.HeaderStyle = ColumnHeaderStyle.None;
    this._lvAttrs.HideSelection = false;
    this._lvAttrs.Name = "_lvAttrs";
    this._lvAttrs.UseCompatibleStateImageBehavior = false;
    this._lvAttrs.View = View.Details;
    this._lvAttrs.SelectedIndexChanged += new EventHandler(this.On_lvAttrs_SelectedIndexChanged);
    this._lvAttrs.SizeChanged += new EventHandler(this.On_lv_SizeChanged);
    componentResourceManager.ApplyResources((object) this._colAttrs, "_colAttrs");
    componentResourceManager.ApplyResources((object) this._lbAttrs, "_lbAttrs");
    this._lbAttrs.Name = "_lbAttrs";
    this._imgButtons.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("_imgButtons.ImageStream");
    this._imgButtons.TransparentColor = Color.Transparent;
    this._imgButtons.Images.SetKeyName(0, "Right.ico");
    this._imgButtons.Images.SetKeyName(1, "Left.ico");
    this._lvSelectedValues.Columns.AddRange(new System.Windows.Forms.ColumnHeader[1]
    {
      this._colSelectedValues
    });
    componentResourceManager.ApplyResources((object) this._lvSelectedValues, "_lvSelectedValues");
    this._lvSelectedValues.FullRowSelect = true;
    this._lvSelectedValues.HeaderStyle = ColumnHeaderStyle.None;
    this._lvSelectedValues.HideSelection = false;
    this._lvSelectedValues.Name = "_lvSelectedValues";
    this._tlpTop.SetRowSpan((Control) this._lvSelectedValues, 4);
    this._lvSelectedValues.UseCompatibleStateImageBehavior = false;
    this._lvSelectedValues.View = View.Details;
    this._lvSelectedValues.SelectedIndexChanged += new EventHandler(this.On_lvSelectedValues_SelectedIndexChanged);
    this._lvSelectedValues.SizeChanged += new EventHandler(this.On_lv_SizeChanged);
    this._lvSelectedValues.DoubleClick += new EventHandler(this.On_lvSelectedValues_DoubleClick);
    componentResourceManager.ApplyResources((object) this._colSelectedValues, "_colSelectedValues");
    componentResourceManager.ApplyResources((object) this._tlpTop, "_tlpTop");
    this._tlpTop.Controls.Add((Control) this._lvAllValues, 0, 1);
    this._tlpTop.Controls.Add((Control) this._lvSelectedValues, 2, 1);
    this._tlpTop.Controls.Add((Control) this._lbAllValues, 0, 0);
    this._tlpTop.Controls.Add((Control) this._lbSelectedValues, 2, 0);
    this._tlpTop.Controls.Add((Control) this._btnLeft, 1, 3);
    this._tlpTop.Controls.Add((Control) this._btnRight, 1, 2);
    this._tlpTop.Name = "_tlpTop";
    this._lvAllValues.Columns.AddRange(new System.Windows.Forms.ColumnHeader[1]
    {
      this._colAllValues
    });
    componentResourceManager.ApplyResources((object) this._lvAllValues, "_lvAllValues");
    this._lvAllValues.FullRowSelect = true;
    this._lvAllValues.HeaderStyle = ColumnHeaderStyle.None;
    this._lvAllValues.HideSelection = false;
    this._lvAllValues.Name = "_lvAllValues";
    this._tlpTop.SetRowSpan((Control) this._lvAllValues, 4);
    this._lvAllValues.UseCompatibleStateImageBehavior = false;
    this._lvAllValues.View = View.Details;
    this._lvAllValues.SelectedIndexChanged += new EventHandler(this.On_lvAllValues_SelectedIndexChanged);
    this._lvAllValues.SizeChanged += new EventHandler(this.On_lv_SizeChanged);
    this._lvAllValues.DoubleClick += new EventHandler(this.On_lvAllValues_DoubleClick);
    componentResourceManager.ApplyResources((object) this._colAllValues, "_colAllValues");
    componentResourceManager.ApplyResources((object) this._lbAllValues, "_lbAllValues");
    this._lbAllValues.Name = "_lbAllValues";
    componentResourceManager.ApplyResources((object) this._btnLeft, "_btnLeft");
    this._btnLeft.ImageList = this._imgButtons;
    this._btnLeft.Name = "_btnLeft";
    this._btnLeft.Tag = (object) "1";
    this._btnLeft.UseVisualStyleBackColor = true;
    this._btnLeft.Click += new EventHandler(this.On_btnLeftRight_Click);
    componentResourceManager.ApplyResources((object) this._btnRight, "_btnRight");
    this._btnRight.ImageList = this._imgButtons;
    this._btnRight.Name = "_btnRight";
    this._btnRight.Tag = (object) "0";
    this._btnRight.UseVisualStyleBackColor = true;
    this._btnRight.Click += new EventHandler(this.On_btnLeftRight_Click);
    componentResourceManager.ApplyResources((object) this._slt, "_slt");
    this._slt.Name = "_slt";
    this._slt.Panel1.Controls.Add((Control) this._tlpTop);
    this._slt.Panel2.Controls.Add((Control) this._spltBottom);
    componentResourceManager.ApplyResources((object) this._spltBottom, "_spltBottom");
    this._spltBottom.Name = "_spltBottom";
    this._spltBottom.Panel1.Controls.Add((Control) this._lstbAbstractParam);
    this._spltBottom.Panel1.Controls.Add((Control) this._lbAbstractParam);
    this._spltBottom.Panel1.Controls.Add((Control) this._pnlLeft);
    this._spltBottom.Panel2.Controls.Add((Control) this._lvAttrs);
    this._spltBottom.Panel2.Controls.Add((Control) this._pnlRight);
    this._spltBottom.Panel2.Controls.Add((Control) this._lbAttrs);
    componentResourceManager.ApplyResources((object) this._lstbAbstractParam, "_lstbAbstractParam");
    this._lstbAbstractParam.FormattingEnabled = true;
    this._lstbAbstractParam.Name = "_lstbAbstractParam";
    this._lstbAbstractParam.SelectedValueChanged += new EventHandler(this.On_lstbAbstractParam_SelectedValueChanged);
    componentResourceManager.ApplyResources((object) this._lbAbstractParam, "_lbAbstractParam");
    this._lbAbstractParam.Name = "_lbAbstractParam";
    this._pnlLeft.Controls.Add((Control) this._txtAbstractParam);
    this._pnlLeft.Controls.Add((Control) this._btnAddAbstractParam);
    this._pnlLeft.Controls.Add((Control) this._btnDelAbstractParam);
    componentResourceManager.ApplyResources((object) this._pnlLeft, "_pnlLeft");
    this._pnlLeft.Name = "_pnlLeft";
    componentResourceManager.ApplyResources((object) this._txtAbstractParam, "_txtAbstractParam");
    this._txtAbstractParam.Name = "_txtAbstractParam";
    componentResourceManager.ApplyResources((object) this._btnAddAbstractParam, "_btnAddAbstractParam");
    this._btnAddAbstractParam.Name = "_btnAddAbstractParam";
    this._btnAddAbstractParam.UseVisualStyleBackColor = true;
    this._btnAddAbstractParam.Click += new EventHandler(this.On_btnAddAbstractParam_Click);
    componentResourceManager.ApplyResources((object) this._btnDelAbstractParam, "_btnDelAbstractParam");
    this._btnDelAbstractParam.Name = "_btnDelAbstractParam";
    this._btnDelAbstractParam.UseVisualStyleBackColor = true;
    this._btnDelAbstractParam.Click += new EventHandler(this.On_btnDelAbstractParam_Click);
    this._pnlRight.Controls.Add((Control) this._btnDel);
    this._pnlRight.Controls.Add((Control) this._btnAdd);
    componentResourceManager.ApplyResources((object) this._pnlRight, "_pnlRight");
    this._pnlRight.Name = "_pnlRight";
    this._pnlMaterials.Controls.Add((Control) this._grbMaterials);
    componentResourceManager.ApplyResources((object) this._pnlMaterials, "_pnlMaterials");
    this._pnlMaterials.Name = "_pnlMaterials";
    this._grbMaterials.Controls.Add((Control) this._chbMaterials);
    componentResourceManager.ApplyResources((object) this._grbMaterials, "_grbMaterials");
    this._grbMaterials.Name = "_grbMaterials";
    this._grbMaterials.TabStop = false;
    componentResourceManager.ApplyResources((object) this._chbMaterials, "_chbMaterials");
    this._chbMaterials.Name = "_chbMaterials";
    this._chbMaterials.UseVisualStyleBackColor = true;
    this._chbMaterials.CheckedChanged += new EventHandler(this.On_chbMaterials_CheckedChanged);
    componentResourceManager.ApplyResources((object) this, "$this");
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add((Control) this._slt);
    this.Controls.Add((Control) this._pnlMaterials);
    this.DoubleBuffered = true;
    this.Name = nameof (AssortmentSearchSettingsCtrl);
    this.Resize += new EventHandler(this.AssortmentSearchSettingsCtrl_Resize);
    this._tlpTop.ResumeLayout(false);
    this._slt.Panel1.ResumeLayout(false);
    this._slt.Panel2.ResumeLayout(false);
    this._slt.EndInit();
    this._slt.ResumeLayout(false);
    this._spltBottom.Panel1.ResumeLayout(false);
    this._spltBottom.Panel2.ResumeLayout(false);
    this._spltBottom.EndInit();
    this._spltBottom.ResumeLayout(false);
    this._pnlLeft.ResumeLayout(false);
    this._pnlLeft.PerformLayout();
    this._pnlRight.ResumeLayout(false);
    this._pnlMaterials.ResumeLayout(false);
    this._grbMaterials.ResumeLayout(false);
    this._grbMaterials.PerformLayout();
    this.ResumeLayout(false);
  }
}
