// Decompiled with JetBrains decompiler
// Type: Intermech.PdmConfigurator.Creator.CodeCreatorControl
// Assembly: Intermech.PdmConfigurator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: B5CB2E26-657B-4329-B46C-77AE46A32171
// Assembly location: D:\IPS\Client\Intermech.PdmConfigurator.dll

using DevExpress.IM.XtraEditors;
using DevExpress.IM.XtraEditors.Controls;
using DevExpress.IM.XtraEditors.Mask;
using Intermech.Client.Core.ObjectCreator;
using Intermech.Client.Core.ObjectCreator.Controls;
using Intermech.DataFormats;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.PdmConfigurator;
using Intermech.Kernel.Search;
using Intermech.Localization;
using Intermech.Navigator;
using Intermech.Navigator.DBObjects;
using Intermech.Navigator.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.PdmConfigurator.Creator;

public class CodeCreatorControl : ObjectCreatorControl
{
  private long orderID;
  private int orderTypeID = -1;
  private bool isChanged;
  private long objCodeID;
  private bool isDesignationExsist = true;
  private bool isNameExsist = true;
  private IContainer components;
  private Label lbCode;
  private Label lb;
  private TextBox tbName;
  private ButtonEdit beCode;
  private Button btnUpdate;
  private Panel panel1;
  private Panel panel2;

  public CodeCreatorControl(CreatedObjectItem createdObject)
    : base(createdObject)
  {
    this.InitializeComponent();
    this.orderID = createdObject.ObjectID;
    this.orderTypeID = createdObject.ObjectTypeID;
    IMSAttribute4ObjectType attribute4ObjectType1 = MetaDataHelper.GetAttribute4ObjectType(createdObject.ObjectTypeID, MetaDataHelper.GetAttributeTypeID("cad0001f-306c-11d8-b4e9-00304f19f545"));
    IMSAttribute4ObjectType attribute4ObjectType2 = MetaDataHelper.GetAttribute4ObjectType(createdObject.ObjectTypeID, MetaDataHelper.GetAttributeTypeID("cad00020-306c-11d8-b4e9-00304f19f545"));
    this.isNameExsist = attribute4ObjectType2 != null && attribute4ObjectType2.Computed == ComputeValueModes.NotComputableValue;
    this.isDesignationExsist = attribute4ObjectType1 != null && attribute4ObjectType1.Computed == ComputeValueModes.NotComputableValue;
    this.UpdateControls();
    HelpProvidersClass.SetHelpOptionForControl((Control) this, 1835);
  }

  public override bool Refresh(PageRefreshArgs args)
  {
    try
    {
      List<int> childObjectTypes = new List<int>();
      foreach (IMSApplicability typeApplicability in MetaDataHelper.GetObjectTypeApplicabilities(this.orderTypeID))
      {
        if (MetaDataHelper.IsPdmConfigurableRelationType(typeApplicability.RelationTypeID) && !childObjectTypes.Contains(typeApplicability.ChildObjectTypeID))
          childObjectTypes.Add(typeApplicability.ChildObjectTypeID);
      }
      string str1 = string.Empty;
      string str2 = string.Empty;
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        IDBObject dbObject1 = sessionKeeper.Session.GetObject(this.orderID);
        if (this.isNameExsist)
          str1 = (dbObject1.GetAttributeByGuid(new Guid("cad00020-306c-11d8-b4e9-00304f19f545")) ?? dbObject1.Attributes.AddAttribute(MetaDataHelper.GetAttributeTypeID("cad00020-306c-11d8-b4e9-00304f19f545"), false)).AsString;
        if (this.isDesignationExsist)
          str2 = (dbObject1.GetAttributeByGuid(new Guid("cad0001f-306c-11d8-b4e9-00304f19f545")) ?? dbObject1.Attributes.AddAttribute(MetaDataHelper.GetAttributeTypeID("cad0001f-306c-11d8-b4e9-00304f19f545"), false)).AsString;
        IDBRelationCollection relationCollection = sessionKeeper.Session.GetRelationCollection(-1);
        relationCollection.ChildObjectTypes = (IList<int>) MetaDataHelper.OptimizeChildObjectTypes((IEnumerable<int>) childObjectTypes);
        DBRecordSetParams paramsSet = new DBRecordSetParams((ConditionStructure[]) null, new object[2]
        {
          (object) -2,
          (object) -20
        });
        FiltrationHelper.BlockPluginFiltrations(ref paramsSet, (HybridDictionary) null);
        paramsSet.RecordCount = 1;
        DataTable dataTable = relationCollection.ConsistFrom(paramsSet, this.orderID);
        this.beCode.Properties.Buttons[0].Enabled = dataTable.Rows.Count != 0;
        this.btnUpdate.Enabled = dataTable.Rows.Count != 0 && this.objCodeID != 0L;
        if (this.isDesignationExsist && string.IsNullOrEmpty(str2) || this.isNameExsist && string.IsNullOrEmpty(str1))
        {
          if (dataTable != null)
          {
            if (dataTable.Rows.Count == 1)
            {
              long int64 = Convert.ToInt64(dataTable.Rows[0][1]);
              IDBRelation relation = sessionKeeper.Session.GetRelation(int64);
              this.objCodeID = Convert.ToInt64(dataTable.Rows[0][0]);
              IDBObject dbObject2 = sessionKeeper.Session.GetObject(this.objCodeID);
              if (this.isDesignationExsist)
              {
                string str3 = ConfigurationCode.BuildConfigurationCode(relation, dbObject2, sessionKeeper.Session);
                if (str3 == string.Empty)
                {
                  IDBAttribute attributeByGuid = dbObject2.GetAttributeByGuid(new Guid("cad0001f-306c-11d8-b4e9-00304f19f545"));
                  if (attributeByGuid != null)
                    str3 = attributeByGuid.AsString;
                }
                this.beCode.Text = str3;
              }
              if (this.isNameExsist)
              {
                IDBAttribute attributeByGuid = dbObject2.GetAttributeByGuid(new Guid("cad00020-306c-11d8-b4e9-00304f19f545"));
                if (attributeByGuid != null)
                  this.tbName.Text = attributeByGuid.AsString;
              }
              this.isChanged = true;
            }
          }
        }
        else
        {
          this.beCode.Text = str2;
          this.tbName.Text = str1;
        }
      }
    }
    catch (Exception ex)
    {
      args.Error = ex;
      return false;
    }
    return true;
  }

  public override bool Save(PageSaveArgs args)
  {
    if (this.isChanged)
    {
      try
      {
        using (SessionKeeper sessionKeeper = new SessionKeeper())
        {
          IDBObject dbObject = sessionKeeper.Session.GetObject(this.orderID);
          if (this.isNameExsist)
            dbObject.GetAttributeByGuid(new Guid("cad00020-306c-11d8-b4e9-00304f19f545")).Value = (object) this.tbName.Text;
          if (this.isDesignationExsist)
            dbObject.GetAttributeByGuid(new Guid("cad0001f-306c-11d8-b4e9-00304f19f545")).Value = (object) this.beCode.Text;
        }
        this.isChanged = false;
      }
      catch (Exception ex)
      {
        args.Error = ex;
        return false;
      }
    }
    return true;
  }

  private void beCode_ButtonClick(object sender, ButtonPressedEventArgs e)
  {
    List<long> objectIDs = new List<long>();
    List<int> childObjectTypes = new List<int>();
    foreach (IMSApplicability typeApplicability in MetaDataHelper.GetObjectTypeApplicabilities(this.orderTypeID))
    {
      if (MetaDataHelper.IsPdmConfigurableRelationType(typeApplicability.RelationTypeID) && !childObjectTypes.Contains(typeApplicability.ChildObjectTypeID))
        childObjectTypes.Add(typeApplicability.ChildObjectTypeID);
    }
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBRelationCollection relationCollection = sessionKeeper.Session.GetRelationCollection(-1);
      relationCollection.ChildObjectTypes = (IList<int>) MetaDataHelper.OptimizeChildObjectTypes((IEnumerable<int>) childObjectTypes);
      DBRecordSetParams paramsSet = new DBRecordSetParams((ConditionStructure[]) null, new object[2]
      {
        (object) -2,
        (object) -20
      });
      FiltrationHelper.BlockPluginFiltrations(ref paramsSet, (HybridDictionary) null);
      DataTable dataTable = relationCollection.ConsistFrom(paramsSet, this.orderID);
      if (dataTable != null)
      {
        foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
        {
          long int64_1 = Convert.ToInt64(row[1]);
          if (MetaDataHelper.IsPdmConfigurableRelationType(sessionKeeper.Session.GetRelation(int64_1).RelationType))
          {
            long int64_2 = Convert.ToInt64(row[0]);
            objectIDs.Add(int64_2);
          }
        }
      }
    }
    SelectObjectsDescriptor rootDescriptor = new SelectObjectsDescriptor(LocalizationHolder.rm.GetString("PdmConfigurator_10"), objectIDs);
    SelectionOptions options = SelectionOptions.HideTree | SelectionOptions.HideViewsToolbar | SelectionOptions.HideViewsGroupingBox | SelectionOptions.SelectObjects | SelectionOptions.DisableObjectListFilter | SelectionOptions.DisableMultiselect | SelectionOptions.ForceRebuildNavTree;
    object[] objArray = SelectionWindow.Select(LocalizationHolder.rm.GetString("PdmConfigurator_10"), LocalizationHolder.rm.GetString("PdmConfigurator_11"), (IDescriptor) rootDescriptor, typeof (IDBTypedObjectID), options);
    if (objArray == null || objArray.Length != 1)
      return;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      this.objCodeID = (objArray[0] as IDBTypedObjectID).ObjectID;
      IDBObject dbObject = sessionKeeper.Session.GetObject(this.objCodeID);
      string str = ConfigurationCode.BuildConfigurationCode(sessionKeeper.Session.GetRelation(this.orderID, this.objCodeID, true), dbObject, sessionKeeper.Session);
      if (str == string.Empty)
      {
        IDBAttribute attributeByGuid = dbObject.GetAttributeByGuid(new Guid("cad0001f-306c-11d8-b4e9-00304f19f545"));
        if (attributeByGuid != null)
          str = attributeByGuid.AsString;
      }
      this.beCode.Text = str;
      if (!this.isNameExsist)
        return;
      IDBAttribute attributeByGuid1 = dbObject.GetAttributeByGuid(new Guid("cad00020-306c-11d8-b4e9-00304f19f545"));
      if (attributeByGuid1 == null)
        return;
      this.tbName.Text = attributeByGuid1.AsString;
    }
  }

  private void beCode_TextChanged(object sender, EventArgs e) => this.isChanged = true;

  private void tbName_TextChanged(object sender, EventArgs e) => this.isChanged = true;

  private void btnUpdate_Click(object sender, EventArgs e)
  {
    if (this.objCodeID == 0L)
      return;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBObject dbObject = sessionKeeper.Session.GetObject(this.objCodeID);
      string str = ConfigurationCode.BuildConfigurationCode(sessionKeeper.Session.GetRelation(this.orderID, this.objCodeID, true), dbObject, sessionKeeper.Session);
      if (str == string.Empty)
      {
        IDBAttribute attributeByGuid = dbObject.GetAttributeByGuid(new Guid("cad0001f-306c-11d8-b4e9-00304f19f545"));
        if (attributeByGuid != null)
          str = attributeByGuid.AsString;
      }
      this.beCode.Text = str;
      IDBAttribute attributeByGuid1 = dbObject.GetAttributeByGuid(new Guid("cad00020-306c-11d8-b4e9-00304f19f545"));
      if (attributeByGuid1 == null)
        return;
      this.tbName.Text = attributeByGuid1.AsString;
    }
  }

  public override int HelpTopicID => 1839;

  private void UpdateControls()
  {
    this.panel1.Visible = this.isDesignationExsist;
    this.panel2.Visible = this.isNameExsist;
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (CodeCreatorControl));
    this.lbCode = new Label();
    this.lb = new Label();
    this.tbName = new TextBox();
    this.beCode = new ButtonEdit();
    this.btnUpdate = new Button();
    this.panel1 = new Panel();
    this.panel2 = new Panel();
    this.beCode.Properties.BeginInit();
    this.panel1.SuspendLayout();
    this.panel2.SuspendLayout();
    this.SuspendLayout();
    componentResourceManager.ApplyResources((object) this.lbCode, "lbCode");
    this.lbCode.Name = "lbCode";
    componentResourceManager.ApplyResources((object) this.lb, "lb");
    this.lb.Name = "lb";
    componentResourceManager.ApplyResources((object) this.tbName, "tbName");
    this.tbName.Name = "tbName";
    this.tbName.TextChanged += new EventHandler(this.tbName_TextChanged);
    componentResourceManager.ApplyResources((object) this.beCode, "beCode");
    this.beCode.Name = "beCode";
    this.beCode.Properties.Buttons.AddRange(new EditorButton[1]
    {
      new EditorButton()
    });
    this.beCode.Properties.MaskData.BeepOnError = (bool) componentResourceManager.GetObject("beCode.Properties.MaskData.BeepOnError");
    this.beCode.Properties.MaskData.Blank = componentResourceManager.GetString("beCode.Properties.MaskData.Blank");
    this.beCode.Properties.MaskData.EditMask = componentResourceManager.GetString("beCode.Properties.MaskData.EditMask");
    this.beCode.Properties.MaskData.IgnoreMaskBlank = (bool) componentResourceManager.GetObject("beCode.Properties.MaskData.IgnoreMaskBlank");
    this.beCode.Properties.MaskData.MaskType = (MaskType) componentResourceManager.GetObject("beCode.Properties.MaskData.MaskType");
    this.beCode.Properties.MaskData.SaveLiteral = (bool) componentResourceManager.GetObject("beCode.Properties.MaskData.SaveLiteral");
    this.beCode.ButtonClick += new ButtonPressedEventHandler(this.beCode_ButtonClick);
    this.beCode.TextChanged += new EventHandler(this.beCode_TextChanged);
    componentResourceManager.ApplyResources((object) this.btnUpdate, "btnUpdate");
    this.btnUpdate.Name = "btnUpdate";
    this.btnUpdate.UseVisualStyleBackColor = true;
    this.btnUpdate.Click += new EventHandler(this.btnUpdate_Click);
    componentResourceManager.ApplyResources((object) this.panel1, "panel1");
    this.panel1.BackColor = SystemColors.Control;
    this.panel1.Controls.Add((Control) this.lbCode);
    this.panel1.Controls.Add((Control) this.btnUpdate);
    this.panel1.Controls.Add((Control) this.beCode);
    this.panel1.Name = "panel1";
    componentResourceManager.ApplyResources((object) this.panel2, "panel2");
    this.panel2.BackColor = SystemColors.Control;
    this.panel2.Controls.Add((Control) this.tbName);
    this.panel2.Controls.Add((Control) this.lb);
    this.panel2.Name = "panel2";
    componentResourceManager.ApplyResources((object) this, "$this");
    this.Controls.Add((Control) this.panel2);
    this.Controls.Add((Control) this.panel1);
    this.Name = nameof (CodeCreatorControl);
    this.beCode.Properties.EndInit();
    this.panel1.ResumeLayout(false);
    this.panel1.PerformLayout();
    this.panel2.ResumeLayout(false);
    this.panel2.PerformLayout();
    this.ResumeLayout(false);
  }
}
