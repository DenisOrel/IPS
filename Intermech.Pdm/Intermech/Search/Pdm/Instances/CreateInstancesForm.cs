// Decompiled with JetBrains decompiler
// Type: Intermech.Search.Pdm.Instances.CreateInstancesForm
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Client.Core;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Pdm;
using Intermech.Kernel.Search;
using Intermech.UI;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Search.Pdm.Instances;

public sealed class CreateInstancesForm : Form
{
  private long _objectVersionID;
  private CreateInstancesForm.CreateInstancesStep _createInstancesStep;
  private long[] _lastCreatedInstancesVersionIds = new long[0];
  private IContainer components;
  private Button _cancelButton;
  private Button _okButton;
  private Button _nextStepButton;
  private Button _previousStepButton;
  private Bevel bevel1;
  private Panel _panel;
  private CreateInstancesSettingsControl _createInstancesSettingsControl;
  private InstancesBlanksListControl _instancesListControl;

  public CreateInstancesForm()
  {
    this.InitializeComponent();
    this._createInstancesStep = CreateInstancesForm.CreateInstancesStep.Settings;
    this.UpdateForm();
  }

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
      this._createInstancesSettingsControl.ObjectVersionID = this._objectVersionID;
    }
  }

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public long SpecFID { get; set; } = -1;

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public long[] LastCreatedInstancesVersionIds => this._lastCreatedInstancesVersionIds;

  private void CreateInstancesGroupForm_Load(object sender, EventArgs e)
  {
    this._lastCreatedInstancesVersionIds = new long[0];
    FormStorage.LoadLayout((Control) this);
  }

  private void CreateInstancesGroupForm_FormClosing(object sender, FormClosingEventArgs e)
  {
    FormStorage.SaveLayout((Control) this);
  }

  private void CreateInstancesSettingsControl_Changed(object sender, EventArgs e)
  {
    this.UpdateForm();
  }

  private void PreviousStepButton_Click(object sender, EventArgs e)
  {
    this._createInstancesStep = CreateInstancesForm.CreateInstancesStep.Settings;
    this.UpdateForm();
  }

  private void NextStepButton_Click(object sender, EventArgs e)
  {
    this._createInstancesStep = CreateInstancesForm.CreateInstancesStep.BlanksList;
    this._instancesListControl.Blanks = this.CreateBlanks();
    this.UpdateForm();
  }

  private void OKButton_Click(object sender, EventArgs e)
  {
    InstanceBlank[] blanks = this.CreateBlanks();
    if (blanks.Length == 0)
      return;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBObject dbObject = sessionKeeper.Session.GetObject(blanks[0].PrototypeVersionID);
      IMSAttribute4ObjectType attribute4ObjectType = MetaDataHelper.GetAttribute4ObjectType(dbObject.ObjectType, InstancesConstants.DesignationAttributeTypeID);
      object obj = dbObject.GetAttributeByID(InstancesConstants.GroupProductIDAttributeTypeID)?.Value;
      List<InstanceBlank> instanceBlankList = new List<InstanceBlank>();
      if (attribute4ObjectType != null && attribute4ObjectType.Unique != UniqueValueModes.NotUnique)
      {
        IDBObjectCollection objectCollection;
        if (attribute4ObjectType.Unique == UniqueValueModes.AllVerTypes)
        {
          objectCollection = sessionKeeper.Session.GetObjectCollection(-1);
          objectCollection.LocalTypesMode = true;
          objectCollection.ShowAllModifications = true;
        }
        else
        {
          if (attribute4ObjectType.Unique != UniqueValueModes.TypeOnly && attribute4ObjectType.Unique != UniqueValueModes.VerTypeOnly)
            throw new NotSupportedEnumException((Enum) attribute4ObjectType.Unique);
          objectCollection = sessionKeeper.Session.GetObjectCollection(dbObject.ObjectType);
          objectCollection.ShowAllModifications = true;
        }
        DBRecordSetParams dbRecordSetParams = new DBRecordSetParams();
        dbRecordSetParams.Columns = new object[1]
        {
          (object) InstancesConstants.DesignationAttributeTypeID
        };
        ref DBRecordSetParams local = ref dbRecordSetParams;
        ConditionStructure[] conditionStructureArray;
        if (obj == null)
          conditionStructureArray = new ConditionStructure[1]
          {
            new ConditionStructure()
            {
              Attribute = (object) InstancesConstants.DesignationAttributeTypeID,
              RelationalOperator = RelationalOperators.In,
              Value = (object) ((IEnumerable<InstanceBlank>) blanks).Select<InstanceBlank, string>((System.Func<InstanceBlank, string>) (o => o.Designation)).ToArray<string>(),
              SQL = string.Empty
            }
          };
        else
          conditionStructureArray = new ConditionStructure[2]
          {
            new ConditionStructure()
            {
              Attribute = (object) InstancesConstants.DesignationAttributeTypeID,
              RelationalOperator = RelationalOperators.In,
              Value = (object) ((IEnumerable<InstanceBlank>) blanks).Select<InstanceBlank, string>((System.Func<InstanceBlank, string>) (o => o.Designation)).ToArray<string>(),
              SQL = string.Empty,
              LogicalOperator = LogicalOperators.AND
            },
            new ConditionStructure()
            {
              Attribute = (object) InstancesConstants.GroupProductIDAttributeTypeID,
              RelationalOperator = RelationalOperators.Equal,
              Value = obj,
              SQL = string.Empty,
              LogicalOperator = LogicalOperators.AND
            }
          };
        local.Conditions = conditionStructureArray;
        dbRecordSetParams.RecordCount = -1;
        DBRecordSetParams paramSet = dbRecordSetParams;
        string[] array = objectCollection.Select(paramSet).Rows.Cast<DataRow>().Select<DataRow, string>((System.Func<DataRow, string>) (o => DataSetProcessor.GetStringValue(o, 0, (string) null))).ToArray<string>();
        bool flag = false;
        foreach (InstanceBlank instanceBlank in blanks)
        {
          if (!((IEnumerable<string>) array).Contains<string>(instanceBlank.Designation))
            instanceBlankList.Add(instanceBlank);
          else
            flag = true;
        }
        if (flag)
        {
          string str = obj == null ? "(в базе данных существуют объекты с данными обозначениями)" : "для данного группового изделия";
          if (instanceBlankList.Count > 0)
          {
            if (MessageBox.Show($"Некоторые из сгенерированных обозначений не являются уникальными {str}. \r\nИгнорировать повторные обозначения и продолжить создание исполений?", "Intermech Professional Solution", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
              return;
          }
          else
          {
            int num = (int) MessageBox.Show($"Ошибка создания исполнений.\r\nНи одно из сгенерированных обозначений не являются уникальным {str}.", "Intermech Professional Solution", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            return;
          }
        }
      }
      if (this.SpecFID == -1L)
        this.SpecFID = CreateInstancesForm.GetSpecificationFIDForProduct(this._objectVersionID, sessionKeeper.Session);
      InstanceBlank[] instanceBlankArray = instanceBlankList.Count > 0 ? instanceBlankList.ToArray() : blanks;
      foreach (InstanceBlank instanceBlank in instanceBlankArray)
        instanceBlank.BasedOnVersionID = InstancesClientService.CheckExistingProductVersion(this.SpecFID, instanceBlank.Designation, this._objectVersionID, sessionKeeper.Session);
      IInstancesServerService customService = sessionKeeper.Session.GetCustomService(typeof (IInstancesServerService)) as IInstancesServerService;
      CreateInstancesParams createInstancesParams = new CreateInstancesParams()
      {
        Blanks = instanceBlankArray
      };
      sessionKeeper.Session.StartLogHistory();
      try
      {
        this._lastCreatedInstancesVersionIds = customService.CreateInstances(sessionKeeper.Session.SessionGUID, createInstancesParams);
        List<long> list = ((IEnumerable<CategoryValue>) sessionKeeper.Session.GetModificationsHistoryArray()).Where<CategoryValue>((System.Func<CategoryValue, bool>) (o => o.ActionID == ActionType.Create && o.CategoryType == 1)).Select<CategoryValue, long>((System.Func<CategoryValue, long>) (o => o.CategoryID)).ToList<long>();
        INotificationService service = (INotificationService) ServicesManager.GetService(typeof (INotificationService));
        service.FireEvent((object) this, (NotificationEventArgs) new DBObjectsEventArgs("ObjectsChanged", instanceBlankArray[0].PrototypeVersionID));
        service.FireEvent((object) this, (NotificationEventArgs) new DBObjectsEventArgs("ObjectsCreated", (IList<long>) list));
      }
      catch
      {
        sessionKeeper.Session.StopLogHistory();
        throw;
      }
    }
    int num1 = (int) MessageBox.Show("Создание исполнений успешно завершено", "Intermech Professional Solution", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
    this.Close();
  }

  private void CancelButton_Click(object sender, EventArgs e)
  {
    this._lastCreatedInstancesVersionIds = new long[0];
    this.Close();
  }

  private void UpdateForm()
  {
    this._createInstancesSettingsControl.Visible = this._createInstancesStep == CreateInstancesForm.CreateInstancesStep.Settings;
    this._instancesListControl.Visible = this._createInstancesStep == CreateInstancesForm.CreateInstancesStep.BlanksList;
    this._previousStepButton.Enabled = this._createInstancesStep == CreateInstancesForm.CreateInstancesStep.BlanksList;
    this._nextStepButton.Enabled = this._createInstancesStep == CreateInstancesForm.CreateInstancesStep.Settings && !this._createInstancesSettingsControl.HasErrors;
    this._okButton.Enabled = !this._createInstancesSettingsControl.HasErrors;
  }

  private InstanceBlank[] CreateBlanks()
  {
    List<InstanceBlank> instanceBlankList = new List<InstanceBlank>();
    if (this._createInstancesSettingsControl.WayToCreateNumberParts == CreateInstancesSettingsControl.WayToCreateInstancesNumberParts.FromTo)
    {
      if (this._createInstancesSettingsControl.GenerateDesignationsWithAdditionalNumberParts)
      {
        if (this._createInstancesSettingsControl.ApplyToNumberPartType == CreateInstancesSettingsControl.InstanceNumberPartType.Main)
        {
          for (int numberPartStartValue = this._createInstancesSettingsControl.MainNumberPartStartValue; numberPartStartValue <= this._createInstancesSettingsControl.NumberPartFinalValue; ++numberPartStartValue)
          {
            InstanceBlank blank = this.CreateBlank(numberPartStartValue, this._createInstancesSettingsControl.AdditionalNumberPartStartValue);
            instanceBlankList.Add(blank);
          }
        }
        else if (this._createInstancesSettingsControl.ApplyToNumberPartType == CreateInstancesSettingsControl.InstanceNumberPartType.Additional)
        {
          for (int numberPartStartValue = this._createInstancesSettingsControl.AdditionalNumberPartStartValue; numberPartStartValue <= this._createInstancesSettingsControl.NumberPartFinalValue; ++numberPartStartValue)
          {
            InstanceBlank blank = this.CreateBlank(this._createInstancesSettingsControl.MainNumberPartStartValue, numberPartStartValue);
            instanceBlankList.Add(blank);
          }
        }
      }
      else
      {
        for (int numberPartStartValue = this._createInstancesSettingsControl.MainNumberPartStartValue; numberPartStartValue <= this._createInstancesSettingsControl.NumberPartFinalValue; ++numberPartStartValue)
        {
          InstanceBlank blank = this.CreateBlank(numberPartStartValue);
          instanceBlankList.Add(blank);
        }
      }
    }
    else if (this._createInstancesSettingsControl.WayToCreateNumberParts == CreateInstancesSettingsControl.WayToCreateInstancesNumberParts.WithCountAndStep)
    {
      if (this._createInstancesSettingsControl.GenerateDesignationsWithAdditionalNumberParts)
      {
        if (this._createInstancesSettingsControl.ApplyToNumberPartType == CreateInstancesSettingsControl.InstanceNumberPartType.Main)
        {
          for (int index = 0; index < this._createInstancesSettingsControl.Count; ++index)
          {
            InstanceBlank blank = this.CreateBlank(this._createInstancesSettingsControl.MainNumberPartStartValue + this._createInstancesSettingsControl.Step * index, this._createInstancesSettingsControl.AdditionalNumberPartStartValue);
            instanceBlankList.Add(blank);
          }
        }
        else if (this._createInstancesSettingsControl.ApplyToNumberPartType == CreateInstancesSettingsControl.InstanceNumberPartType.Additional)
        {
          for (int index = 0; index < this._createInstancesSettingsControl.Count; ++index)
          {
            InstanceBlank blank = this.CreateBlank(this._createInstancesSettingsControl.MainNumberPartStartValue, this._createInstancesSettingsControl.AdditionalNumberPartStartValue + this._createInstancesSettingsControl.Step * index);
            instanceBlankList.Add(blank);
          }
        }
      }
      else
      {
        for (int index = 0; index < this._createInstancesSettingsControl.Count; ++index)
        {
          InstanceBlank blank = this.CreateBlank(this._createInstancesSettingsControl.MainNumberPartStartValue + this._createInstancesSettingsControl.Step * index);
          instanceBlankList.Add(blank);
        }
      }
    }
    return instanceBlankList.ToArray();
  }

  private InstanceBlank CreateBlank(int mainNumberPartValue, int additionalNumberPartValue = -1)
  {
    InstanceBlank blank = new InstanceBlank(this._objectVersionID);
    string numberPart1 = this.CreateNumberPart(mainNumberPartValue, this._createInstancesSettingsControl.MainNumberPartFormat);
    string str;
    if (additionalNumberPartValue != -1)
    {
      string numberPart2 = this.CreateNumberPart(additionalNumberPartValue, this._createInstancesSettingsControl.AdditionalNumberPartFormat);
      str = $"{numberPart1}.{numberPart2}";
    }
    else
      str = numberPart1;
    blank.Number = str;
    blank.Designation = $"{this._createInstancesSettingsControl.BaseDesignation}-{str}";
    blank.CopyCompositionAndAttributesOfPrototype = this._createInstancesSettingsControl.CopyCompositionAndAttributesOfPrototype;
    return blank;
  }

  private string CreateNumberPart(
    int numberPartValue,
    CreateInstancesSettingsControl.InstanceNumberPartFormat numberPartFormat)
  {
    if (numberPartFormat == CreateInstancesSettingsControl.InstanceNumberPartFormat.TwoDigits)
      return numberPartValue.ToString("00");
    if (numberPartFormat == CreateInstancesSettingsControl.InstanceNumberPartFormat.ThreeDigits)
      return numberPartValue.ToString("000");
    throw new Exception();
  }

  public static long GetSpecificationFIDForProduct(long productObjID, IUserSession session)
  {
    IDBRelationCollection relationCollection = session.GetRelationCollection(session.IdentHelper.DocRelationTypeID);
    IMSObjectType objectType = MetaDataHelper.GetObjectType(new Guid("cad00133-306c-11d8-b4e9-00304f19f545"));
    int parentType = objectType != null ? objectType.ObjectTypeID : -1;
    relationCollection.ObjectTypeID = parentType;
    DBRecordSetParams paramSet = new DBRecordSetParams((ConditionStructure[]) null, new ColumnDescriptor[3]
    {
      new ColumnDescriptor((object) ObligatoryObjectAttributes.F_ID, AttributeSourceTypes.Object, ColumnContents.Value, ColumnNameMapping.ID, SortOrders.NONE, 0),
      new ColumnDescriptor((object) ObligatoryObjectAttributes.F_OBJECT_ID, AttributeSourceTypes.Object, ColumnContents.Value, ColumnNameMapping.ID, SortOrders.NONE, 0),
      new ColumnDescriptor((object) ObligatoryObjectAttributes.F_OBJECT_TYPE, AttributeSourceTypes.Object, ColumnContents.Value, ColumnNameMapping.ID, SortOrders.NONE, 0)
    });
    if (paramSet.Tags == null)
      paramSet.Tags = new HybridDictionary();
    paramSet.Tags[(object) "{AB419A02-DE8A-4A8E-905A-D782F5B720E5}"] = (object) new long[2]
    {
      0L,
      1L
    };
    paramSet.Tags[(object) "{2FACA180-73B8-4F24-9928-5623661BBBE6}"] = (object) true;
    paramSet.Tags[(object) "{325F5CDB-8B8E-4B2D-9AA9-5624A0A64D7E}"] = (object) true;
    paramSet.Tags[(object) "{0422E069-0A1D-4235-85E8-C52C3516CFC1}"] = (object) true;
    DataTable dataTable1 = relationCollection.ConsistFrom(paramSet, productObjID);
    foreach (DataRow row in (InternalDataCollectionBase) dataTable1.Rows)
    {
      int int32 = Convert.ToInt32(row[2]);
      if (parentType == int32 || MetaDataHelper.IsObjectTypeChildOf(int32, parentType))
      {
        long int64 = Convert.ToInt64(row[0]);
        dataTable1.Dispose();
        return int64;
      }
    }
    dataTable1.Dispose();
    long[] articlesByGroupId = ((IArticleService) ServicesManager.GetService(typeof (IArticleService))).FindArticlesByGroupID(productObjID, (object) session);
    if (articlesByGroupId == null)
      return -1;
    foreach (long projectID in articlesByGroupId)
    {
      if (productObjID != projectID)
      {
        DataTable dataTable2 = relationCollection.ConsistFrom(paramSet, projectID);
        foreach (DataRow row in (InternalDataCollectionBase) dataTable2.Rows)
        {
          int int32 = Convert.ToInt32(row[2]);
          if (parentType == int32 || MetaDataHelper.IsObjectTypeChildOf(int32, parentType))
          {
            long int64 = Convert.ToInt64(row[0]);
            dataTable2.Dispose();
            return int64;
          }
        }
        dataTable2.Dispose();
      }
    }
    return -1;
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this._cancelButton = new Button();
    this._okButton = new Button();
    this._nextStepButton = new Button();
    this._previousStepButton = new Button();
    this._panel = new Panel();
    this._instancesListControl = new InstancesBlanksListControl();
    this._createInstancesSettingsControl = new CreateInstancesSettingsControl();
    this.bevel1 = new Bevel();
    this._panel.SuspendLayout();
    this.SuspendLayout();
    this._cancelButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this._cancelButton.Location = new Point(900, 298);
    this._cancelButton.Name = "_cancelButton";
    this._cancelButton.Size = new Size(75, 23);
    this._cancelButton.TabIndex = 0;
    this._cancelButton.Text = "Отмена";
    this._cancelButton.UseVisualStyleBackColor = true;
    this._cancelButton.Click += new EventHandler(this.CancelButton_Click);
    this._okButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this._okButton.Location = new Point(819, 298);
    this._okButton.Name = "_okButton";
    this._okButton.Size = new Size(75, 23);
    this._okButton.TabIndex = 0;
    this._okButton.Text = "Готово";
    this._okButton.UseVisualStyleBackColor = true;
    this._okButton.Click += new EventHandler(this.OKButton_Click);
    this._nextStepButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this._nextStepButton.Location = new Point(738, 298);
    this._nextStepButton.Name = "_nextStepButton";
    this._nextStepButton.Size = new Size(75, 23);
    this._nextStepButton.TabIndex = 0;
    this._nextStepButton.Text = "Далее";
    this._nextStepButton.UseVisualStyleBackColor = true;
    this._nextStepButton.Click += new EventHandler(this.NextStepButton_Click);
    this._previousStepButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this._previousStepButton.Location = new Point(657, 298);
    this._previousStepButton.Name = "_previousStepButton";
    this._previousStepButton.Size = new Size(75, 23);
    this._previousStepButton.TabIndex = 0;
    this._previousStepButton.Text = "Назад";
    this._previousStepButton.UseVisualStyleBackColor = true;
    this._previousStepButton.Click += new EventHandler(this.PreviousStepButton_Click);
    this._panel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
    this._panel.Controls.Add((Control) this._instancesListControl);
    this._panel.Controls.Add((Control) this._createInstancesSettingsControl);
    this._panel.Location = new Point(13, 13);
    this._panel.Name = "_panel";
    this._panel.Size = new Size(962, 271);
    this._panel.TabIndex = 2;
    this._instancesListControl.Dock = DockStyle.Fill;
    this._instancesListControl.Location = new Point(0, 0);
    this._instancesListControl.Name = "_instancesListControl";
    this._instancesListControl.Size = new Size(962, 271);
    this._instancesListControl.TabIndex = 1;
    this._instancesListControl.Visible = false;
    this._createInstancesSettingsControl.Dock = DockStyle.Fill;
    this._createInstancesSettingsControl.Location = new Point(0, 0);
    this._createInstancesSettingsControl.Name = "_createInstancesSettingsControl";
    this._createInstancesSettingsControl.Size = new Size(962, 271);
    this._createInstancesSettingsControl.TabIndex = 0;
    this._createInstancesSettingsControl.Changed += new EventHandler(this.CreateInstancesSettingsControl_Changed);
    this.bevel1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
    this.bevel1.Location = new Point(12, 290);
    this.bevel1.Name = "bevel1";
    this.bevel1.Size = new Size(963, 2);
    this.bevel1.TabIndex = 1;
    this.bevel1.Text = "bevel1";
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.ClientSize = new Size(987, 333);
    this.Controls.Add((Control) this._panel);
    this.Controls.Add((Control) this.bevel1);
    this.Controls.Add((Control) this._previousStepButton);
    this.Controls.Add((Control) this._nextStepButton);
    this.Controls.Add((Control) this._okButton);
    this.Controls.Add((Control) this._cancelButton);
    this.MinimumSize = new Size(1000, 370);
    this.Name = nameof (CreateInstancesForm);
    this.ShowIcon = false;
    this.Text = "Создание группы исполнений";
    this.FormClosing += new FormClosingEventHandler(this.CreateInstancesGroupForm_FormClosing);
    this.Load += new EventHandler(this.CreateInstancesGroupForm_Load);
    this._panel.ResumeLayout(false);
    this.ResumeLayout(false);
  }

  private enum CreateInstancesStep
  {
    Settings,
    BlanksList,
  }
}
