// Decompiled with JetBrains decompiler
// Type: Intermech.Search.Pdm.Instances.CreateInstancesSettingsControl
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.DataFormats;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Kernel.Search;
using Intermech.Navigator;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.ListInstances;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Search.Pdm.Instances;

public sealed class CreateInstancesSettingsControl : UserControl
{
  private static readonly Regex InstanceNumberRegex = new Regex("-(?<instanceNumber>[0-9]+)$", RegexOptions.Compiled);
  private long _objectVersionID;
  private Guid _objectVersionGuid;
  private Guid _groupInstanceID;
  private CreateInstancesSettingsControl.ObjectNodeWithoutCompositionNodesFactorySupported _objectNodeWithoutCompositionNodesFactorySupported = new CreateInstancesSettingsControl.ObjectNodeWithoutCompositionNodesFactorySupported();
  private IContainer components;
  private TableLayoutPanel tableLayoutPanel3;
  private TableLayoutPanel tableLayoutPanel4;
  private Button _selectBaseDesignationButton;
  private Label label1;
  private TextBox _baseDesignationTextBox;
  private TableLayoutPanel tableLayoutPanel1;
  private GroupBox groupBox1;
  private RadioButton _withStepAndCountRadioButton;
  private RadioButton _fromToWithStepRadioButton;
  private GroupBox groupBox2;
  private RadioButton _withAdditionalNumberPartRadioButton;
  private RadioButton _withoutAdditionalNumberPartRadioButton;
  private GroupBox groupBox3;
  private RadioButton _applyToAdditionalNumberPartRadioButton;
  private RadioButton _applyToMainNumberPartRadioButton;
  private FlowLayoutPanel flowLayoutPanel1;
  private CheckBox _copyCompositionAndAttributesCheckBox;
  private Label _prototypeDesignationLabel;
  private TableLayoutPanel tableLayoutPanel2;
  private TextBox _mainNumberPartStartValueTextBox;
  private Label _mainNumberPartStartValueLabel;
  private Label _additionalNumberPartStartValueLabel;
  private Label _numberPartFinalValueLabel;
  private TextBox _countTextBox;
  private TextBox _additionalNumberPartStartValueTextBox;
  private Label _countLabel;
  private Label _stepLabel;
  private TextBox _stepTextBox;
  private TextBox _numberPartFinalValueTextBox;
  private Label label2;
  private ErrorProvider _errorProvider;

  public CreateInstancesSettingsControl()
  {
    this.InitializeComponent();
    this.UpdateControl();
  }

  public event EventHandler Changed;

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public long ObjectVersionID
  {
    get => this._objectVersionID;
    set
    {
      if (value == this._objectVersionID)
        return;
      this._objectVersionID = value;
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        IDBObject dbObject = sessionKeeper.Session.GetObject(this._objectVersionID);
        this._objectVersionGuid = dbObject.ObjectGUID;
        IDBAttribute attributeById1 = dbObject.GetAttributeByID(InstancesConstants.DesignationAttributeTypeID);
        if (attributeById1 != null)
          this._prototypeDesignationLabel.Text = attributeById1.AsString;
        else
          this._prototypeDesignationLabel.Text = string.Empty;
        IDBAttribute attributeById2 = dbObject.GetAttributeByID(InstancesConstants.GroupProductIDAttributeTypeID);
        Guid result = Guid.Empty;
        if (attributeById2 != null && Guid.TryParse(attributeById2.AsString, out result))
        {
          this._groupInstanceID = result;
          IDBObjectCollection objectCollection = sessionKeeper.Session.GetObjectCollection(dbObject.ObjectType);
          objectCollection.ShowAllModifications = true;
          DBRecordSetParams dbRecordSetParams = new DBRecordSetParams();
          (^ref dbRecordSetParams).Conditions = new ConditionStructure[1]
          {
            new ConditionStructure()
            {
              Attribute = (object) InstancesConstants.GroupProductIDAttributeTypeID,
              RelationalOperator = RelationalOperators.Equal,
              Value = (object) result,
              SQL = string.Empty
            }
          };
          dbRecordSetParams.Columns = new object[2]
          {
            (object) ObligatoryObjectAttributes.F_OBJECT_ID,
            (object) InstancesConstants.DesignationAttributeTypeID
          };
          dbRecordSetParams.RecordCount = -1;
          DBRecordSetParams paramSet = dbRecordSetParams;
          DataTable dataTable = objectCollection.Select(paramSet);
          List<Tuple<long, string>> source = new List<Tuple<long, string>>();
          foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
          {
            long int64Value = DataSetProcessor.GetInt64Value(row, 0, 0L);
            string stringValue = DataSetProcessor.GetStringValue(row, 1, string.Empty);
            source.Add(new Tuple<long, string>(int64Value, stringValue));
          }
          this._baseDesignationTextBox.Text = source.OrderBy<Tuple<long, string>, int>((System.Func<Tuple<long, string>, int>) (o => o.Item2.Length)).First<Tuple<long, string>>().Item2;
          this.CreateStartAndFinalValues(source.OrderBy<Tuple<long, string>, long>((System.Func<Tuple<long, string>, long>) (o => Math.Abs(o.Item1))).Last<Tuple<long, string>>().Item2);
        }
        else
        {
          this._selectBaseDesignationButton.Enabled = false;
          if (attributeById1 != null)
            this._baseDesignationTextBox.Text = attributeById1.AsString;
          else
            this._baseDesignationTextBox.Text = string.Empty;
          this.CreateStartAndFinalValues(attributeById1.AsString);
        }
      }
    }
  }

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public string BaseDesignation => this._baseDesignationTextBox.Text;

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public int MainNumberPartStartValue
  {
    get => Convert.ToInt32(this._mainNumberPartStartValueTextBox.Text);
  }

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public int AdditionalNumberPartStartValue
  {
    get => Convert.ToInt32(this._additionalNumberPartStartValueTextBox.Text);
  }

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public int NumberPartFinalValue => Convert.ToInt32(this._numberPartFinalValueTextBox.Text);

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public int Step => Convert.ToInt32(this._stepTextBox.Text);

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public int Count => Convert.ToInt32(this._countTextBox.Text);

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public bool HasErrors
  {
    get
    {
      return !string.IsNullOrEmpty(this._errorProvider.GetError((Control) this._mainNumberPartStartValueTextBox)) || !string.IsNullOrEmpty(this._errorProvider.GetError((Control) this._additionalNumberPartStartValueTextBox)) || !string.IsNullOrEmpty(this._errorProvider.GetError((Control) this._numberPartFinalValueTextBox)) || !string.IsNullOrEmpty(this._errorProvider.GetError((Control) this._stepTextBox)) || !string.IsNullOrEmpty(this._errorProvider.GetError((Control) this._countTextBox));
    }
  }

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public bool GenerateDesignationsWithAdditionalNumberParts
  {
    get => this._withAdditionalNumberPartRadioButton.Checked;
  }

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public CreateInstancesSettingsControl.WayToCreateInstancesNumberParts WayToCreateNumberParts
  {
    get
    {
      return this._fromToWithStepRadioButton.Checked ? CreateInstancesSettingsControl.WayToCreateInstancesNumberParts.FromTo : CreateInstancesSettingsControl.WayToCreateInstancesNumberParts.WithCountAndStep;
    }
  }

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public CreateInstancesSettingsControl.InstanceNumberPartType ApplyToNumberPartType
  {
    get
    {
      return this._applyToMainNumberPartRadioButton.Checked ? CreateInstancesSettingsControl.InstanceNumberPartType.Main : CreateInstancesSettingsControl.InstanceNumberPartType.Additional;
    }
  }

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public bool CopyCompositionAndAttributesOfPrototype
  {
    get => this._copyCompositionAndAttributesCheckBox.Checked;
  }

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public CreateInstancesSettingsControl.InstanceNumberPartFormat MainNumberPartFormat
  {
    get => this.GetInstanceNumberPartFormat(this._mainNumberPartStartValueTextBox.Text);
  }

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public CreateInstancesSettingsControl.InstanceNumberPartFormat AdditionalNumberPartFormat
  {
    get => this.GetInstanceNumberPartFormat(this._additionalNumberPartStartValueTextBox.Text);
  }

  private void SelectBaseDesignationButton_Click(object sender, EventArgs e)
  {
    ServiceContainer nodesContext = new ServiceContainer();
    nodesContext.AddService(typeof (INodesFactorySupported), (object) this._objectNodeWithoutCompositionNodesFactorySupported);
    object[] objArray = SelectionWindow.Select("Выберите исполнение", "Выберите исполнение, обозначение которого выступит в качестве \"Базового обозначения\" для вновь создаваемых исполнений", (IDescriptor) new ListInstancesDescriptor(this._groupInstanceID, this._objectVersionGuid), typeof (IDBTypedObjectID), (IServiceProvider) nodesContext, SelectionOptions.HideViews | SelectionOptions.SelectObjects | SelectionOptions.DisableMultiselect);
    if (objArray == null || objArray.Length == 0)
      return;
    long objectId = ((IDBTypedObjectID) objArray[0]).ObjectID;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBAttribute attributeById = sessionKeeper.Session.GetObject(objectId).GetAttributeByID(InstancesConstants.DesignationAttributeTypeID);
      if (attributeById != null)
        this._baseDesignationTextBox.Text = attributeById.AsString;
      else
        this._baseDesignationTextBox.Text = string.Empty;
    }
  }

  private void FromToWithStepRadioButton_CheckedChanged(object sender, EventArgs e)
  {
    this.UpdateControl();
    this.OnChanged();
  }

  private void WithStepAndCountRadioButton_CheckedChanged(object sender, EventArgs e)
  {
    this.UpdateControl();
    this.OnChanged();
  }

  private void WithoutAdditionalNumberPartRadioButton_CheckedChanged(object sender, EventArgs e)
  {
    this.UpdateControl();
    this.OnChanged();
  }

  private void WithAdditionalNumberPartRadioButton_CheckedChanged(object sender, EventArgs e)
  {
    this.UpdateControl();
    this.OnChanged();
  }

  private void ApplyToMainNumberPartRadioButton_CheckedChanged(object sender, EventArgs e)
  {
    this.UpdateControl();
    this.OnChanged();
  }

  private void ApplyToAdditionalNumberPartRadioButton_CheckedChanged(object sender, EventArgs e)
  {
    this.UpdateControl();
    this.OnChanged();
  }

  private void MainNumberPartStartValueTextBox_TextChanged(object sender, EventArgs e)
  {
    this.UpdateControl();
    this.OnChanged();
  }

  private void AdditionalNumberPartStartValueTextBox_TextChanged(object sender, EventArgs e)
  {
    this.UpdateControl();
    this.OnChanged();
  }

  private void NumberPartFinalValueTextBox_TextChanged(object sender, EventArgs e)
  {
    this.UpdateControl();
    this.OnChanged();
  }

  private void CountTextBox_TextChanged(object sender, EventArgs e)
  {
    this.UpdateControl();
    this.OnChanged();
  }

  private void StepTextBox_TextChanged(object sender, EventArgs e)
  {
    this.UpdateControl();
    this.OnChanged();
  }

  public void OnChanged()
  {
    EventHandler changed = this.Changed;
    if (changed == null)
      return;
    changed((object) this, EventArgs.Empty);
  }

  private void UpdateControl()
  {
    this._applyToAdditionalNumberPartRadioButton.Enabled = this._withAdditionalNumberPartRadioButton.Checked;
    this._additionalNumberPartStartValueLabel.Enabled = this._additionalNumberPartStartValueTextBox.Enabled = this._withAdditionalNumberPartRadioButton.Checked;
    this._numberPartFinalValueLabel.Enabled = this._numberPartFinalValueTextBox.Enabled = this._fromToWithStepRadioButton.Checked;
    this._countLabel.Enabled = this._countTextBox.Enabled = this._withStepAndCountRadioButton.Checked;
    this._stepLabel.Enabled = this._stepTextBox.Enabled = this._withStepAndCountRadioButton.Checked;
    if (!this._withAdditionalNumberPartRadioButton.Checked)
      this._applyToMainNumberPartRadioButton.Checked = true;
    this._errorProvider.Clear();
    int result1;
    bool flag1 = int.TryParse(this._mainNumberPartStartValueTextBox.Text, out result1);
    int result2;
    bool flag2 = int.TryParse(this._additionalNumberPartStartValueTextBox.Text, out result2);
    int result3;
    bool flag3 = int.TryParse(this._numberPartFinalValueTextBox.Text, out result3);
    if ((this._mainNumberPartStartValueTextBox.Text == null || this._mainNumberPartStartValueTextBox.Text.Length < 2 || this._mainNumberPartStartValueTextBox.Text.Length > 3 || !flag1 || result1 < 0) && this._mainNumberPartStartValueTextBox.Enabled)
      this._errorProvider.SetError((Control) this._mainNumberPartStartValueTextBox, "В данное поле можно ввести только целое число (кроме отрицательных) с минимальным количеством символов=2 и максимальным=3");
    if ((this._additionalNumberPartStartValueTextBox.Text == null || this._additionalNumberPartStartValueTextBox.Text.Length < 2 || this._additionalNumberPartStartValueTextBox.Text.Length > 3 || !flag2 || result2 < 0) && this._additionalNumberPartStartValueTextBox.Enabled)
      this._errorProvider.SetError((Control) this._additionalNumberPartStartValueTextBox, "В данное поле можно ввести только целое число (кроме отрицательных) с минимальным количеством символов=2 и максимальным=3");
    if ((this._numberPartFinalValueTextBox.Text == null || this._numberPartFinalValueTextBox.Text.Length < 2 || this._numberPartFinalValueTextBox.Text.Length > 3 || !flag3 || result3 < 0) && this._numberPartFinalValueTextBox.Enabled)
      this._errorProvider.SetError((Control) this._numberPartFinalValueTextBox, "В данное поле можно ввести только целое число (кроме отрицательных) с минимальным количеством символов=2 и максимальным=3");
    int result4;
    if ((this._stepTextBox.Text == null || this._stepTextBox.Text.Length > 3 || !int.TryParse(this._stepTextBox.Text, out result4) || result4 <= 0) && this._stepTextBox.Enabled)
      this._errorProvider.SetError((Control) this._stepTextBox, "В данное поле можно ввести только целое число (кроме отрицательных и нуля) с максимальным количеством символов=3");
    int result5;
    if ((this._countTextBox.Text == null || this._countTextBox.Text.Length > 3 || !int.TryParse(this._countTextBox.Text, out result5) || result5 <= 0) && this._countTextBox.Enabled)
      this._errorProvider.SetError((Control) this._countTextBox, "В данное поле можно ввести только целое число (кроме отрицательных и нуля) с максимальным количеством символов=3");
    if (!(this.WayToCreateNumberParts == CreateInstancesSettingsControl.WayToCreateInstancesNumberParts.FromTo & flag3) || (!(this.ApplyToNumberPartType == CreateInstancesSettingsControl.InstanceNumberPartType.Main & flag1) || result1 <= result3) && (!(this.ApplyToNumberPartType == CreateInstancesSettingsControl.InstanceNumberPartType.Additional & flag2) || result2 <= result3) || !this._numberPartFinalValueTextBox.Enabled)
      return;
    this._errorProvider.SetError((Control) this._numberPartFinalValueTextBox, "Конечное значение меньше начального");
  }

  private CreateInstancesSettingsControl.InstanceNumberPartFormat GetInstanceNumberPartFormat(
    string text)
  {
    if (text == null)
      return CreateInstancesSettingsControl.InstanceNumberPartFormat.Unknown;
    if (text.Length == 2)
      return CreateInstancesSettingsControl.InstanceNumberPartFormat.TwoDigits;
    return text.Length == 3 ? CreateInstancesSettingsControl.InstanceNumberPartFormat.ThreeDigits : CreateInstancesSettingsControl.InstanceNumberPartFormat.Unknown;
  }

  private void CreateStartAndFinalValues(string designation)
  {
    Group group = CreateInstancesSettingsControl.InstanceNumberRegex.Match(designation).Groups["instanceNumber"];
    if (group != null && !string.IsNullOrEmpty(group.Value))
    {
      int int32 = Convert.ToInt32(group.Value);
      this._mainNumberPartStartValueTextBox.Text = this.ConvertNumberToString(int32 + 1, group.Value.Length);
      this._numberPartFinalValueTextBox.Text = this.ConvertNumberToString(int32 + 2, group.Value.Length);
    }
    else
    {
      this._mainNumberPartStartValueTextBox.Text = "01";
      this._numberPartFinalValueTextBox.Text = "02";
    }
  }

  private string ConvertNumberToString(int number, int length)
  {
    return number.ToString(new string('0', length));
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
    this.tableLayoutPanel3 = new TableLayoutPanel();
    this.tableLayoutPanel4 = new TableLayoutPanel();
    this._selectBaseDesignationButton = new Button();
    this.label1 = new Label();
    this._baseDesignationTextBox = new TextBox();
    this.tableLayoutPanel1 = new TableLayoutPanel();
    this.groupBox1 = new GroupBox();
    this._withStepAndCountRadioButton = new RadioButton();
    this._fromToWithStepRadioButton = new RadioButton();
    this.groupBox2 = new GroupBox();
    this._withAdditionalNumberPartRadioButton = new RadioButton();
    this._withoutAdditionalNumberPartRadioButton = new RadioButton();
    this.groupBox3 = new GroupBox();
    this._applyToAdditionalNumberPartRadioButton = new RadioButton();
    this._applyToMainNumberPartRadioButton = new RadioButton();
    this.flowLayoutPanel1 = new FlowLayoutPanel();
    this._copyCompositionAndAttributesCheckBox = new CheckBox();
    this._prototypeDesignationLabel = new Label();
    this.tableLayoutPanel2 = new TableLayoutPanel();
    this._mainNumberPartStartValueTextBox = new TextBox();
    this._mainNumberPartStartValueLabel = new Label();
    this._additionalNumberPartStartValueLabel = new Label();
    this._numberPartFinalValueLabel = new Label();
    this._countTextBox = new TextBox();
    this._additionalNumberPartStartValueTextBox = new TextBox();
    this._countLabel = new Label();
    this._stepLabel = new Label();
    this._stepTextBox = new TextBox();
    this._numberPartFinalValueTextBox = new TextBox();
    this.label2 = new Label();
    this._errorProvider = new ErrorProvider(this.components);
    this.tableLayoutPanel3.SuspendLayout();
    this.tableLayoutPanel4.SuspendLayout();
    this.tableLayoutPanel1.SuspendLayout();
    this.groupBox1.SuspendLayout();
    this.groupBox2.SuspendLayout();
    this.groupBox3.SuspendLayout();
    this.flowLayoutPanel1.SuspendLayout();
    this.tableLayoutPanel2.SuspendLayout();
    ((ISupportInitialize) this._errorProvider).BeginInit();
    this.SuspendLayout();
    this.tableLayoutPanel3.ColumnCount = 1;
    this.tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
    this.tableLayoutPanel3.Controls.Add((Control) this.tableLayoutPanel4, 0, 0);
    this.tableLayoutPanel3.Controls.Add((Control) this.tableLayoutPanel1, 0, 1);
    this.tableLayoutPanel3.Controls.Add((Control) this.flowLayoutPanel1, 0, 4);
    this.tableLayoutPanel3.Controls.Add((Control) this.tableLayoutPanel2, 0, 3);
    this.tableLayoutPanel3.Controls.Add((Control) this.label2, 0, 2);
    this.tableLayoutPanel3.Dock = DockStyle.Fill;
    this.tableLayoutPanel3.Location = new Point(0, 0);
    this.tableLayoutPanel3.Name = "tableLayoutPanel3";
    this.tableLayoutPanel3.RowCount = 5;
    this.tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 40f));
    this.tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 50f));
    this.tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 20f));
    this.tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 50f));
    this.tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 40f));
    this.tableLayoutPanel3.Size = new Size(874, 268);
    this.tableLayoutPanel3.TabIndex = 12;
    this.tableLayoutPanel4.ColumnCount = 3;
    this.tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150f));
    this.tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
    this.tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100f));
    this.tableLayoutPanel4.Controls.Add((Control) this._selectBaseDesignationButton, 2, 0);
    this.tableLayoutPanel4.Controls.Add((Control) this.label1, 0, 0);
    this.tableLayoutPanel4.Controls.Add((Control) this._baseDesignationTextBox, 1, 0);
    this.tableLayoutPanel4.Dock = DockStyle.Fill;
    this.tableLayoutPanel4.Location = new Point(3, 3);
    this.tableLayoutPanel4.Name = "tableLayoutPanel4";
    this.tableLayoutPanel4.RowCount = 1;
    this.tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
    this.tableLayoutPanel4.Size = new Size(868, 34);
    this.tableLayoutPanel4.TabIndex = 13;
    this._selectBaseDesignationButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
    this._selectBaseDesignationButton.Location = new Point(790, 3);
    this._selectBaseDesignationButton.Name = "_selectBaseDesignationButton";
    this._selectBaseDesignationButton.Size = new Size(75, 23);
    this._selectBaseDesignationButton.TabIndex = 2;
    this._selectBaseDesignationButton.Text = "Выбрать";
    this._selectBaseDesignationButton.UseVisualStyleBackColor = true;
    this._selectBaseDesignationButton.Click += new EventHandler(this.SelectBaseDesignationButton_Click);
    this.label1.AutoSize = true;
    this.label1.Location = new Point(3, 0);
    this.label1.Name = "label1";
    this.label1.Size = new Size(121, 13);
    this.label1.TabIndex = 0;
    this.label1.Text = "Базовое обозначение:";
    this._baseDesignationTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
    this._baseDesignationTextBox.Location = new Point(153, 3);
    this._baseDesignationTextBox.Name = "_baseDesignationTextBox";
    this._baseDesignationTextBox.ReadOnly = true;
    this._baseDesignationTextBox.Size = new Size(612, 20);
    this._baseDesignationTextBox.TabIndex = 1;
    this.tableLayoutPanel1.ColumnCount = 3;
    this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33333f));
    this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33333f));
    this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33333f));
    this.tableLayoutPanel1.Controls.Add((Control) this.groupBox1, 0, 0);
    this.tableLayoutPanel1.Controls.Add((Control) this.groupBox2, 1, 0);
    this.tableLayoutPanel1.Controls.Add((Control) this.groupBox3, 2, 0);
    this.tableLayoutPanel1.Dock = DockStyle.Fill;
    this.tableLayoutPanel1.Location = new Point(3, 43);
    this.tableLayoutPanel1.Name = "tableLayoutPanel1";
    this.tableLayoutPanel1.RowCount = 1;
    this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
    this.tableLayoutPanel1.Size = new Size(868, 78);
    this.tableLayoutPanel1.TabIndex = 11;
    this.groupBox1.Controls.Add((Control) this._withStepAndCountRadioButton);
    this.groupBox1.Controls.Add((Control) this._fromToWithStepRadioButton);
    this.groupBox1.Dock = DockStyle.Fill;
    this.groupBox1.Location = new Point(3, 3);
    this.groupBox1.Name = "groupBox1";
    this.groupBox1.Size = new Size(283, 72);
    this.groupBox1.TabIndex = 3;
    this.groupBox1.TabStop = false;
    this.groupBox1.Text = "Вариант создания:";
    this._withStepAndCountRadioButton.AutoSize = true;
    this._withStepAndCountRadioButton.Location = new Point(6, 42);
    this._withStepAndCountRadioButton.Name = "_withStepAndCountRadioButton";
    this._withStepAndCountRadioButton.Size = new Size(209, 17);
    this._withStepAndCountRadioButton.TabIndex = 1;
    this._withStepAndCountRadioButton.Text = "через шаг и количество исполнений";
    this._withStepAndCountRadioButton.UseVisualStyleBackColor = true;
    this._withStepAndCountRadioButton.CheckedChanged += new EventHandler(this.WithStepAndCountRadioButton_CheckedChanged);
    this._fromToWithStepRadioButton.AutoSize = true;
    this._fromToWithStepRadioButton.Checked = true;
    this._fromToWithStepRadioButton.Location = new Point(6, 19);
    this._fromToWithStepRadioButton.Name = "_fromToWithStepRadioButton";
    this._fromToWithStepRadioButton.Size = new Size(225, 17);
    this._fromToWithStepRadioButton.TabIndex = 0;
    this._fromToWithStepRadioButton.TabStop = true;
    this._fromToWithStepRadioButton.Text = "\"с __ по __\" (с шагом равным единице)";
    this._fromToWithStepRadioButton.UseVisualStyleBackColor = true;
    this._fromToWithStepRadioButton.CheckedChanged += new EventHandler(this.FromToWithStepRadioButton_CheckedChanged);
    this.groupBox2.Controls.Add((Control) this._withAdditionalNumberPartRadioButton);
    this.groupBox2.Controls.Add((Control) this._withoutAdditionalNumberPartRadioButton);
    this.groupBox2.Dock = DockStyle.Fill;
    this.groupBox2.Location = new Point(292, 3);
    this.groupBox2.Name = "groupBox2";
    this.groupBox2.Size = new Size(283, 72);
    this.groupBox2.TabIndex = 4;
    this.groupBox2.TabStop = false;
    this.groupBox2.Text = "Доп. номер исполнений:";
    this._withAdditionalNumberPartRadioButton.AutoSize = true;
    this._withAdditionalNumberPartRadioButton.Location = new Point(7, 45);
    this._withAdditionalNumberPartRadioButton.Name = "_withAdditionalNumberPartRadioButton";
    this._withAdditionalNumberPartRadioButton.Size = new Size(104, 17);
    this._withAdditionalNumberPartRadioButton.TabIndex = 0;
    this._withAdditionalNumberPartRadioButton.Text = "с доп. номером";
    this._withAdditionalNumberPartRadioButton.UseVisualStyleBackColor = true;
    this._withAdditionalNumberPartRadioButton.CheckedChanged += new EventHandler(this.WithAdditionalNumberPartRadioButton_CheckedChanged);
    this._withoutAdditionalNumberPartRadioButton.AutoSize = true;
    this._withoutAdditionalNumberPartRadioButton.Checked = true;
    this._withoutAdditionalNumberPartRadioButton.Location = new Point(7, 22);
    this._withoutAdditionalNumberPartRadioButton.Name = "_withoutAdditionalNumberPartRadioButton";
    this._withoutAdditionalNumberPartRadioButton.Size = new Size(108, 17);
    this._withoutAdditionalNumberPartRadioButton.TabIndex = 0;
    this._withoutAdditionalNumberPartRadioButton.TabStop = true;
    this._withoutAdditionalNumberPartRadioButton.Text = "без доп. номера";
    this._withoutAdditionalNumberPartRadioButton.UseVisualStyleBackColor = true;
    this._withoutAdditionalNumberPartRadioButton.CheckedChanged += new EventHandler(this.WithoutAdditionalNumberPartRadioButton_CheckedChanged);
    this.groupBox3.Controls.Add((Control) this._applyToAdditionalNumberPartRadioButton);
    this.groupBox3.Controls.Add((Control) this._applyToMainNumberPartRadioButton);
    this.groupBox3.Dock = DockStyle.Fill;
    this.groupBox3.Location = new Point(581, 3);
    this.groupBox3.Name = "groupBox3";
    this.groupBox3.Size = new Size(284, 72);
    this.groupBox3.TabIndex = 5;
    this.groupBox3.TabStop = false;
    this.groupBox3.Text = "Применить счетчик к:";
    this._applyToAdditionalNumberPartRadioButton.AutoSize = true;
    this._applyToAdditionalNumberPartRadioButton.Location = new Point(6, 42);
    this._applyToAdditionalNumberPartRadioButton.Name = "_applyToAdditionalNumberPartRadioButton";
    this._applyToAdditionalNumberPartRadioButton.Size = new Size(149, 17);
    this._applyToAdditionalNumberPartRadioButton.TabIndex = 0;
    this._applyToAdditionalNumberPartRadioButton.Text = "доп. номеру исполнения";
    this._applyToAdditionalNumberPartRadioButton.UseVisualStyleBackColor = true;
    this._applyToAdditionalNumberPartRadioButton.CheckedChanged += new EventHandler(this.ApplyToAdditionalNumberPartRadioButton_CheckedChanged);
    this._applyToMainNumberPartRadioButton.AutoSize = true;
    this._applyToMainNumberPartRadioButton.Checked = true;
    this._applyToMainNumberPartRadioButton.Location = new Point(6, 19);
    this._applyToMainNumberPartRadioButton.Name = "_applyToMainNumberPartRadioButton";
    this._applyToMainNumberPartRadioButton.Size = new Size(195, 17);
    this._applyToMainNumberPartRadioButton.TabIndex = 0;
    this._applyToMainNumberPartRadioButton.TabStop = true;
    this._applyToMainNumberPartRadioButton.Text = "порядковому номеру исполнения";
    this._applyToMainNumberPartRadioButton.UseVisualStyleBackColor = true;
    this._applyToMainNumberPartRadioButton.CheckedChanged += new EventHandler(this.ApplyToMainNumberPartRadioButton_CheckedChanged);
    this.flowLayoutPanel1.Controls.Add((Control) this._copyCompositionAndAttributesCheckBox);
    this.flowLayoutPanel1.Controls.Add((Control) this._prototypeDesignationLabel);
    this.flowLayoutPanel1.Dock = DockStyle.Fill;
    this.flowLayoutPanel1.Location = new Point(3, 231);
    this.flowLayoutPanel1.Name = "flowLayoutPanel1";
    this.flowLayoutPanel1.Size = new Size(868, 34);
    this.flowLayoutPanel1.TabIndex = 10;
    this._copyCompositionAndAttributesCheckBox.AutoSize = true;
    this._copyCompositionAndAttributesCheckBox.Checked = true;
    this._copyCompositionAndAttributesCheckBox.CheckState = CheckState.Checked;
    this._copyCompositionAndAttributesCheckBox.Location = new Point(3, 3);
    this._copyCompositionAndAttributesCheckBox.Name = "_copyCompositionAndAttributesCheckBox";
    this._copyCompositionAndAttributesCheckBox.Size = new Size(267, 17);
    this._copyCompositionAndAttributesCheckBox.TabIndex = 9;
    this._copyCompositionAndAttributesCheckBox.Text = "Копировать состав и параметры у исполнения ";
    this._copyCompositionAndAttributesCheckBox.UseVisualStyleBackColor = true;
    this._prototypeDesignationLabel.AutoSize = true;
    this._prototypeDesignationLabel.Location = new Point(276, 3);
    this._prototypeDesignationLabel.Margin = new Padding(3);
    this._prototypeDesignationLabel.Name = "_prototypeDesignationLabel";
    this._prototypeDesignationLabel.Size = new Size(35, 13);
    this._prototypeDesignationLabel.TabIndex = 10;
    this._prototypeDesignationLabel.Text = "label8";
    this.tableLayoutPanel2.ColumnCount = 4;
    this.tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25f));
    this.tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25f));
    this.tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25f));
    this.tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25f));
    this.tableLayoutPanel2.Controls.Add((Control) this._mainNumberPartStartValueTextBox, 1, 0);
    this.tableLayoutPanel2.Controls.Add((Control) this._mainNumberPartStartValueLabel, 0, 0);
    this.tableLayoutPanel2.Controls.Add((Control) this._additionalNumberPartStartValueLabel, 0, 1);
    this.tableLayoutPanel2.Controls.Add((Control) this._numberPartFinalValueLabel, 2, 0);
    this.tableLayoutPanel2.Controls.Add((Control) this._countTextBox, 3, 1);
    this.tableLayoutPanel2.Controls.Add((Control) this._additionalNumberPartStartValueTextBox, 1, 1);
    this.tableLayoutPanel2.Controls.Add((Control) this._countLabel, 2, 1);
    this.tableLayoutPanel2.Controls.Add((Control) this._stepLabel, 2, 2);
    this.tableLayoutPanel2.Controls.Add((Control) this._stepTextBox, 3, 2);
    this.tableLayoutPanel2.Controls.Add((Control) this._numberPartFinalValueTextBox, 3, 0);
    this.tableLayoutPanel2.Dock = DockStyle.Fill;
    this.tableLayoutPanel2.Location = new Point(3, 147);
    this.tableLayoutPanel2.Name = "tableLayoutPanel2";
    this.tableLayoutPanel2.RowCount = 3;
    this.tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33333f));
    this.tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33333f));
    this.tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33333f));
    this.tableLayoutPanel2.Size = new Size(868, 78);
    this.tableLayoutPanel2.TabIndex = 0;
    this._mainNumberPartStartValueTextBox.Location = new Point(220, 3);
    this._mainNumberPartStartValueTextBox.Name = "_mainNumberPartStartValueTextBox";
    this._mainNumberPartStartValueTextBox.Size = new Size(100, 20);
    this._mainNumberPartStartValueTextBox.TabIndex = 8;
    this._mainNumberPartStartValueTextBox.TextChanged += new EventHandler(this.MainNumberPartStartValueTextBox_TextChanged);
    this._mainNumberPartStartValueLabel.AutoSize = true;
    this._mainNumberPartStartValueLabel.Location = new Point(3, 0);
    this._mainNumberPartStartValueLabel.Name = "_mainNumberPartStartValueLabel";
    this._mainNumberPartStartValueLabel.Size = new Size(183, 26);
    this._mainNumberPartStartValueLabel.TabIndex = 7;
    this._mainNumberPartStartValueLabel.Text = "Начальное значение порядкового номера:";
    this._additionalNumberPartStartValueLabel.AutoSize = true;
    this._additionalNumberPartStartValueLabel.Location = new Point(3, 26);
    this._additionalNumberPartStartValueLabel.Name = "_additionalNumberPartStartValueLabel";
    this._additionalNumberPartStartValueLabel.Size = new Size(180, 13);
    this._additionalNumberPartStartValueLabel.TabIndex = 7;
    this._additionalNumberPartStartValueLabel.Text = "Начальное значение доп. номера:";
    this._additionalNumberPartStartValueLabel.TextAlign = ContentAlignment.TopRight;
    this._numberPartFinalValueLabel.AutoSize = true;
    this._numberPartFinalValueLabel.Location = new Point(437, 0);
    this._numberPartFinalValueLabel.Name = "_numberPartFinalValueLabel";
    this._numberPartFinalValueLabel.Size = new Size(108, 13);
    this._numberPartFinalValueLabel.TabIndex = 7;
    this._numberPartFinalValueLabel.Text = "Конечное значение:";
    this._numberPartFinalValueLabel.TextAlign = ContentAlignment.TopRight;
    this._countTextBox.Location = new Point(654, 29);
    this._countTextBox.Name = "_countTextBox";
    this._countTextBox.Size = new Size(100, 20);
    this._countTextBox.TabIndex = 8;
    this._countTextBox.TextChanged += new EventHandler(this.CountTextBox_TextChanged);
    this._additionalNumberPartStartValueTextBox.Location = new Point(220, 29);
    this._additionalNumberPartStartValueTextBox.Name = "_additionalNumberPartStartValueTextBox";
    this._additionalNumberPartStartValueTextBox.Size = new Size(100, 20);
    this._additionalNumberPartStartValueTextBox.TabIndex = 8;
    this._additionalNumberPartStartValueTextBox.TextChanged += new EventHandler(this.AdditionalNumberPartStartValueTextBox_TextChanged);
    this._countLabel.AutoSize = true;
    this._countLabel.Location = new Point(437, 26);
    this._countLabel.Name = "_countLabel";
    this._countLabel.Size = new Size(204, 13);
    this._countLabel.TabIndex = 7;
    this._countLabel.Text = "Количество создаваемых исполнений:";
    this._countLabel.TextAlign = ContentAlignment.TopRight;
    this._stepLabel.AutoSize = true;
    this._stepLabel.Location = new Point(437, 52);
    this._stepLabel.Name = "_stepLabel";
    this._stepLabel.Size = new Size(30, 13);
    this._stepLabel.TabIndex = 7;
    this._stepLabel.Text = "Шаг:";
    this._stepLabel.TextAlign = ContentAlignment.TopRight;
    this._stepTextBox.Location = new Point(654, 55);
    this._stepTextBox.Name = "_stepTextBox";
    this._stepTextBox.Size = new Size(100, 20);
    this._stepTextBox.TabIndex = 8;
    this._stepTextBox.TextChanged += new EventHandler(this.StepTextBox_TextChanged);
    this._numberPartFinalValueTextBox.Location = new Point(654, 3);
    this._numberPartFinalValueTextBox.Name = "_numberPartFinalValueTextBox";
    this._numberPartFinalValueTextBox.Size = new Size(100, 20);
    this._numberPartFinalValueTextBox.TabIndex = 8;
    this._numberPartFinalValueTextBox.TextChanged += new EventHandler(this.NumberPartFinalValueTextBox_TextChanged);
    this.label2.AutoSize = true;
    this.label2.Location = new Point(3, 124);
    this.label2.Name = "label2";
    this.label2.Size = new Size(102, 13);
    this.label2.TabIndex = 6;
    this.label2.Text = "Исходные данные:";
    this._errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
    this._errorProvider.ContainerControl = (ContainerControl) this;
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add((Control) this.tableLayoutPanel3);
    this.Name = nameof (CreateInstancesSettingsControl);
    this.Size = new Size(874, 268);
    this.tableLayoutPanel3.ResumeLayout(false);
    this.tableLayoutPanel3.PerformLayout();
    this.tableLayoutPanel4.ResumeLayout(false);
    this.tableLayoutPanel4.PerformLayout();
    this.tableLayoutPanel1.ResumeLayout(false);
    this.groupBox1.ResumeLayout(false);
    this.groupBox1.PerformLayout();
    this.groupBox2.ResumeLayout(false);
    this.groupBox2.PerformLayout();
    this.groupBox3.ResumeLayout(false);
    this.groupBox3.PerformLayout();
    this.flowLayoutPanel1.ResumeLayout(false);
    this.flowLayoutPanel1.PerformLayout();
    this.tableLayoutPanel2.ResumeLayout(false);
    this.tableLayoutPanel2.PerformLayout();
    ((ISupportInitialize) this._errorProvider).EndInit();
    this.ResumeLayout(false);
  }

  public enum WayToCreateInstancesNumberParts
  {
    FromTo,
    WithCountAndStep,
  }

  public enum InstanceNumberPartType
  {
    Main,
    Additional,
  }

  public enum InstanceNumberPartFormat
  {
    Unknown,
    TwoDigits,
    ThreeDigits,
  }

  private sealed class ObjectNodeWithoutCompositionNodesFactorySupported : INodesFactorySupported
  {
    private CreateInstancesSettingsControl.ObjectNodeWithoutCompositionNodesFactory _objectNodeWithoutCompositionNodesFactory = new CreateInstancesSettingsControl.ObjectNodeWithoutCompositionNodesFactory();

    public INodesFactory GetNodesFactory(IServiceProvider services, INodeID nodeID)
    {
      return (INodesFactory) this._objectNodeWithoutCompositionNodesFactory;
    }
  }

  private sealed class ObjectNodeWithoutCompositionNodesFactory : INodesFactory
  {
    public INode GetNode(int categoryID, int typeID) => (INode) null;

    public INode GetNode(INodeID nodeID, params object[] args) => (INode) null;
  }
}
