// Decompiled with JetBrains decompiler
// Type: Intermech.Office.Client.OfficeDocSettingsForm
// Assembly: Intermech.Office.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 87EC380F-A344-4B99-B3CC-05ECB303FAD4
// Assembly location: D:\IPS\Client\Intermech.Office.Client.dll

using Intermech.DataFormats;
using Intermech.Diagnostics;
using Intermech.Holders;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Navigator;
using Intermech.Navigator.Interfaces;
using Intermech.Office.Interfaces;
using Intermech.PropertyEditors;
using Intermech.Workflow;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Office.Client;

public class OfficeDocSettingsForm : TabPageForm
{
  private int _objectType = -1;
  [NotNull]
  private readonly OfficeDocSettingsTabPage _page;
  [CanBeNull]
  private OrderProcessTemplates _processTemplates;
  private bool _feel;
  private IContainer components;
  private GroupBox groupBox1;
  private GroupBox groupBox2;
  private CheckBox cbInternal;
  private CheckBox cbOutgoing;
  private CheckBox cbIncoming;
  private TabControl tabControl1;
  private TabPage tpInput;
  private TabPage tpOutput;
  private TabPage tpInternal;
  private GroupBox groupBox3;
  private Button bNoControl;
  private TextBox tbNoControl;
  private Label label2;
  private Button bControl;
  private TextBox tbControl;
  private Label label1;
  private GroupBox groupBox4;
  private Button bSucNoControl;
  private TextBox tbSucNoControl;
  private Label label3;
  private Button bSucControl;
  private TextBox tbSucControl;
  private Label label4;

  public OfficeDocSettingsForm(Guid aInstGuid, [NotNull] OfficeDocSettingsTabPage page)
    : base(aInstGuid)
  {
    this.InitializeComponent();
    this._page = page;
    EventHandler modifyMethod = new EventHandler(this.SetModified);
    this.tpInput.Controls.Add((Control) new OfficeDocSettingsTemplateControl(modifyMethod, OfficeDocumentTypes.Incoming));
    this.tpOutput.Controls.Add((Control) new OfficeDocSettingsTemplateControl(modifyMethod, OfficeDocumentTypes.Outgoing));
    this.tpInternal.Controls.Add((Control) new OfficeDocSettingsTemplateControl(modifyMethod, OfficeDocumentTypes.Internal));
  }

  public override void FillForm(IFolder folder)
  {
    this._folder = folder as CustomFolder;
    this._feel = true;
    try
    {
      if (StatesController.GetLoadState((object) this._page))
        return;
      this._objectType = Convert.ToInt32(folder.Id);
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        List<int> childrenIdRecursive = MetaDataHelper.GetObjectTypeChildrenIDRecursive(OfficeConsts.ObjtypeOfficeDocumentsID);
        OfficeDocumentTypeSettings settings = sessionKeeper.Session.GetCustomService<IOfficeDocumentTypeService>().GetSettings(sessionKeeper.Session.SessionGUID, this._objectType);
        this.cbIncoming.Checked = false;
        this.cbInternal.Checked = false;
        this.cbOutgoing.Checked = false;
        if (settings.EnableTypes != null)
        {
          foreach (OfficeDocumentTypes enableType in settings.EnableTypes)
          {
            switch (enableType)
            {
              case OfficeDocumentTypes.Incoming:
                this.cbIncoming.Checked = true;
                break;
              case OfficeDocumentTypes.Outgoing:
                this.cbOutgoing.Checked = true;
                break;
              case OfficeDocumentTypes.Internal:
                this.cbInternal.Checked = true;
                break;
            }
          }
        }
        this.ResetTemplateControl(this.tpInput);
        this.ResetTemplateControl(this.tpOutput);
        this.ResetTemplateControl(this.tpInternal);
        if (settings.Templates != null && settings.Templates.Count > 0)
        {
          foreach (KeyValuePair<OfficeDocumentTypes, RegNumberSettings> template in settings.Templates)
          {
            OfficeDocSettingsTemplateControl control = this.GetControl(template.Key);
            control.EnableEmpty = template.Value.EnableEmptyRegNumbers;
            control.Generate = template.Value.AutoGenerateRegNumber;
            control.TemplateString = template.Value.Template;
            control.ResetType = template.Value.CountResetType;
            control.TypeNumbering = template.Value.CountWithinType;
            control.UnitNumbering = template.Value.CountWithinUnit;
            control.DesignationEqualRegNumber = template.Value.DesignationEqualRegNumber;
          }
        }
        if (!childrenIdRecursive.Contains(this._objectType))
        {
          foreach (OfficeDocumentTypes type in Enum.GetValues(typeof (OfficeDocumentTypes)))
          {
            if (type != OfficeDocumentTypes.Unknown)
              this.GetControl(type).SetEnableEmpty(false);
          }
        }
        else if (settings.EnableEmptyRegNumbers != null && settings.EnableEmptyRegNumbers.Count > 0)
        {
          foreach (KeyValuePair<OfficeDocumentTypes, bool> enableEmptyRegNumber in settings.EnableEmptyRegNumbers)
          {
            OfficeDocSettingsTemplateControl control = this.GetControl(enableEmptyRegNumber.Key);
            control.SetEnableEmpty(true);
            control.EnableEmpty = enableEmptyRegNumber.Value;
          }
        }
        this._processTemplates = settings.ProcessTemplates;
        this.tbControl.Text = this._processTemplates.Control != Guid.Empty ? this.GetTemplateCaption(sessionKeeper.Session, this._processTemplates.Control) : string.Empty;
        this.tbNoControl.Text = this._processTemplates.NoControl != Guid.Empty ? this.GetTemplateCaption(sessionKeeper.Session, this._processTemplates.NoControl) : string.Empty;
        this.tbSucControl.Text = this._processTemplates.SuccessiveControl != Guid.Empty ? this.GetTemplateCaption(sessionKeeper.Session, this._processTemplates.SuccessiveControl) : string.Empty;
        this.tbSucNoControl.Text = this._processTemplates.SuccessiveNoControl != Guid.Empty ? this.GetTemplateCaption(sessionKeeper.Session, this._processTemplates.SuccessiveNoControl) : string.Empty;
      }
      StatesController.SetLoadState((object) this._page, true);
    }
    finally
    {
      this._feel = false;
    }
  }

  [NotNull]
  private OfficeDocSettingsTemplateControl GetControl(OfficeDocumentTypes type)
  {
    switch (type)
    {
      case OfficeDocumentTypes.Incoming:
        return (OfficeDocSettingsTemplateControl) this.tpInput.Controls[0];
      case OfficeDocumentTypes.Outgoing:
        return (OfficeDocSettingsTemplateControl) this.tpOutput.Controls[0];
      case OfficeDocumentTypes.Internal:
        return (OfficeDocSettingsTemplateControl) this.tpInternal.Controls[0];
      default:
        throw new NotSupportedException($"Unsupported {"OfficeDocumentTypes"} value:{type}");
    }
  }

  [Pure]
  [NotNull]
  private string GetTemplateCaption([NotNull] IUserSession session, Guid templateGuid)
  {
    QuickObjectInfo objectInfo = session.GetObjectInfo(templateGuid);
    Intermech.Diagnostics.Check.Assert(!objectInfo.Empty, "!Empty");
    return objectInfo.Caption;
  }

  public override bool SaveForm(IFolder folder)
  {
    if (StatesController.GetModifiedState((object) this._page))
    {
      OfficeDocumentTypeSettings settings = OfficeDocumentTypeSettings.CreateDefault();
      List<OfficeDocumentTypes> officeDocumentTypesList = new List<OfficeDocumentTypes>();
      if (this.cbIncoming.Checked)
        officeDocumentTypesList.Add(OfficeDocumentTypes.Incoming);
      if (this.cbOutgoing.Checked)
        officeDocumentTypesList.Add(OfficeDocumentTypes.Outgoing);
      if (this.cbInternal.Checked)
        officeDocumentTypesList.Add(OfficeDocumentTypes.Internal);
      if (officeDocumentTypesList.Count > 0)
        settings.EnableTypes = officeDocumentTypesList.ToArray();
      int capacity = Enum.GetValues(typeof (OfficeDocumentTypes)).Length - 1;
      settings.Templates = new Dictionary<OfficeDocumentTypes, RegNumberSettings>(capacity);
      settings.EnableEmptyRegNumbers = new Dictionary<OfficeDocumentTypes, bool>(capacity);
      OfficeDocSettingsTemplateControl control1 = (OfficeDocSettingsTemplateControl) this.tpInput.Controls[0];
      settings.Templates.Add(OfficeDocumentTypes.Incoming, this.GetRegNumberTemplate(control1));
      settings.EnableEmptyRegNumbers.Add(OfficeDocumentTypes.Incoming, control1.EnableEmpty);
      OfficeDocSettingsTemplateControl control2 = (OfficeDocSettingsTemplateControl) this.tpOutput.Controls[0];
      settings.Templates.Add(OfficeDocumentTypes.Outgoing, this.GetRegNumberTemplate(control2));
      settings.EnableEmptyRegNumbers.Add(OfficeDocumentTypes.Outgoing, control2.EnableEmpty);
      OfficeDocSettingsTemplateControl control3 = (OfficeDocSettingsTemplateControl) this.tpInternal.Controls[0];
      settings.Templates.Add(OfficeDocumentTypes.Internal, this.GetRegNumberTemplate(control3));
      settings.EnableEmptyRegNumbers.Add(OfficeDocumentTypes.Internal, control3.EnableEmpty);
      Intermech.Diagnostics.Check.NotNull<OrderProcessTemplates>(this._processTemplates, "_processTemplates");
      settings.ProcessTemplates = this._processTemplates;
      using (SessionKeeper sessionKeeper = new SessionKeeper())
        sessionKeeper.Session.GetCustomService<IOfficeDocumentTypeService>().SetSettings(sessionKeeper.Session.SessionGUID, this._objectType, settings);
      StatesController.SetModifiedState((object) this._page, false);
    }
    return true;
  }

  [NotNull]
  private RegNumberSettings GetRegNumberTemplate([NotNull] OfficeDocSettingsTemplateControl control)
  {
    return new RegNumberSettings(control.TemplateString, control.ResetType, control.TypeNumbering, control.UnitNumbering, control.EnableEmpty, control.Generate, control.DesignationEqualRegNumber);
  }

  private void ResetTemplateControl([NotNull] TabPage tabPage)
  {
    OfficeDocSettingsTemplateControl control = (OfficeDocSettingsTemplateControl) tabPage.Controls[0];
    control._ObjectTypeID = this._objectType;
    control.ResetControl();
  }

  private void SetModified([CanBeNull] object sender, [NotNull] EventArgs e)
  {
    if (this._feel)
      return;
    StatesController.SetModifiedState((object) this._page, true);
    EventsHolder.FireWasChanged(sender, this.instGuid, e);
  }

  private void bControl_Click([CanBeNull] object sender, [NotNull] EventArgs e)
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBObject dbObject = this.SelectProcessTemplate(sessionKeeper.Session);
      Intermech.Diagnostics.Check.NotNull<OrderProcessTemplates>(this._processTemplates, "_processTemplates");
      if (dbObject == null || !(this._processTemplates.Control != dbObject.ObjectGUID))
        return;
      this.tbControl.Text = dbObject.Caption;
      this._processTemplates.Control = dbObject.ObjectGUID;
      this.SetModified((object) this, new EventArgs());
    }
  }

  private void bNoControl_Click([CanBeNull] object sender, [NotNull] EventArgs e)
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBObject dbObject = this.SelectProcessTemplate(sessionKeeper.Session);
      Intermech.Diagnostics.Check.NotNull<OrderProcessTemplates>(this._processTemplates, "_processTemplates");
      if (dbObject == null || !(this._processTemplates.NoControl != dbObject.ObjectGUID))
        return;
      this.tbNoControl.Text = dbObject.Caption;
      this._processTemplates.NoControl = dbObject.ObjectGUID;
      this.SetModified((object) this, new EventArgs());
    }
  }

  private void CbCheckedChanged([CanBeNull] object sender, [NotNull] EventArgs e)
  {
    this.SetModified((object) this, EventArgs.Empty);
  }

  [CanBeNull]
  private IDBObject SelectProcessTemplate([NotNull] IUserSession session)
  {
    IDescriptor rootDescriptor = (IDescriptor) new Intermech.Navigator.DBObjectTypes.Descriptor(wfConsts.SchemesTypeID);
    object[] objArray = SelectionWindow.Select(Localization.GetString("Office.Client_2"), rootDescriptor, typeof (IDBTypedObjectID), SelectionOptions.SelectObjects | SelectionOptions.DisableMultiselect);
    return objArray != null && objArray.Length != 0 ? session.GetObject(((IDBTypedObjectID) objArray[0]).ObjectID) : (IDBObject) null;
  }

  [NotNull]
  public override string HelpTopicID => "2497";

  private void bSucControl_Click([CanBeNull] object sender, [NotNull] EventArgs e)
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBObject dbObject = this.SelectProcessTemplate(sessionKeeper.Session);
      Intermech.Diagnostics.Check.NotNull<OrderProcessTemplates>(this._processTemplates, "_processTemplates");
      if (dbObject == null || !(this._processTemplates.SuccessiveControl != dbObject.ObjectGUID))
        return;
      this.tbSucControl.Text = dbObject.Caption;
      this._processTemplates.SuccessiveControl = dbObject.ObjectGUID;
      this.SetModified((object) this, new EventArgs());
    }
  }

  private void bSucNoControl_Click([CanBeNull] object sender, [NotNull] EventArgs e)
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBObject dbObject = this.SelectProcessTemplate(sessionKeeper.Session);
      Intermech.Diagnostics.Check.NotNull<OrderProcessTemplates>(this._processTemplates, "_processTemplates");
      if (dbObject == null || !(this._processTemplates.SuccessiveNoControl != dbObject.ObjectGUID))
        return;
      this.tbSucNoControl.Text = dbObject.Caption;
      this._processTemplates.SuccessiveNoControl = dbObject.ObjectGUID;
      this.SetModified((object) this, new EventArgs());
    }
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (OfficeDocSettingsForm));
    this.groupBox3 = new GroupBox();
    this.bNoControl = new Button();
    this.tbNoControl = new TextBox();
    this.label2 = new Label();
    this.bControl = new Button();
    this.tbControl = new TextBox();
    this.label1 = new Label();
    this.groupBox2 = new GroupBox();
    this.cbInternal = new CheckBox();
    this.cbOutgoing = new CheckBox();
    this.cbIncoming = new CheckBox();
    this.groupBox1 = new GroupBox();
    this.tabControl1 = new TabControl();
    this.tpInput = new TabPage();
    this.tpOutput = new TabPage();
    this.tpInternal = new TabPage();
    this.groupBox4 = new GroupBox();
    this.bSucNoControl = new Button();
    this.tbSucNoControl = new TextBox();
    this.label3 = new Label();
    this.bSucControl = new Button();
    this.tbSucControl = new TextBox();
    this.label4 = new Label();
    this.groupBox3.SuspendLayout();
    this.groupBox2.SuspendLayout();
    this.groupBox1.SuspendLayout();
    this.tabControl1.SuspendLayout();
    this.groupBox4.SuspendLayout();
    this.SuspendLayout();
    this.groupBox3.Controls.Add((Control) this.bNoControl);
    this.groupBox3.Controls.Add((Control) this.tbNoControl);
    this.groupBox3.Controls.Add((Control) this.label2);
    this.groupBox3.Controls.Add((Control) this.bControl);
    this.groupBox3.Controls.Add((Control) this.tbControl);
    this.groupBox3.Controls.Add((Control) this.label1);
    this.groupBox3.ForeColor = SystemColors.HotTrack;
    componentResourceManager.ApplyResources((object) this.groupBox3, "groupBox3");
    this.groupBox3.Name = "groupBox3";
    this.groupBox3.TabStop = false;
    this.bNoControl.ForeColor = SystemColors.ControlText;
    componentResourceManager.ApplyResources((object) this.bNoControl, "bNoControl");
    this.bNoControl.Name = "bNoControl";
    this.bNoControl.UseVisualStyleBackColor = true;
    this.bNoControl.Click += new EventHandler(this.bNoControl_Click);
    this.tbNoControl.BackColor = SystemColors.Window;
    this.tbNoControl.ForeColor = SystemColors.WindowText;
    componentResourceManager.ApplyResources((object) this.tbNoControl, "tbNoControl");
    this.tbNoControl.Name = "tbNoControl";
    this.tbNoControl.ReadOnly = true;
    componentResourceManager.ApplyResources((object) this.label2, "label2");
    this.label2.ForeColor = SystemColors.ControlText;
    this.label2.Name = "label2";
    this.bControl.ForeColor = SystemColors.ControlText;
    componentResourceManager.ApplyResources((object) this.bControl, "bControl");
    this.bControl.Name = "bControl";
    this.bControl.UseVisualStyleBackColor = true;
    this.bControl.Click += new EventHandler(this.bControl_Click);
    this.tbControl.BackColor = SystemColors.Window;
    this.tbControl.ForeColor = SystemColors.WindowText;
    componentResourceManager.ApplyResources((object) this.tbControl, "tbControl");
    this.tbControl.Name = "tbControl";
    this.tbControl.ReadOnly = true;
    componentResourceManager.ApplyResources((object) this.label1, "label1");
    this.label1.ForeColor = SystemColors.ControlText;
    this.label1.Name = "label1";
    this.groupBox2.Controls.Add((Control) this.cbInternal);
    this.groupBox2.Controls.Add((Control) this.cbOutgoing);
    this.groupBox2.Controls.Add((Control) this.cbIncoming);
    this.groupBox2.ForeColor = SystemColors.HotTrack;
    componentResourceManager.ApplyResources((object) this.groupBox2, "groupBox2");
    this.groupBox2.Name = "groupBox2";
    this.groupBox2.TabStop = false;
    this.cbInternal.AccessibleRole = AccessibleRole.None;
    componentResourceManager.ApplyResources((object) this.cbInternal, "cbInternal");
    this.cbInternal.Checked = true;
    this.cbInternal.CheckState = CheckState.Checked;
    this.cbInternal.ForeColor = SystemColors.ControlText;
    this.cbInternal.Name = "cbInternal";
    this.cbInternal.UseVisualStyleBackColor = true;
    this.cbInternal.CheckedChanged += new EventHandler(this.CbCheckedChanged);
    componentResourceManager.ApplyResources((object) this.cbOutgoing, "cbOutgoing");
    this.cbOutgoing.Checked = true;
    this.cbOutgoing.CheckState = CheckState.Checked;
    this.cbOutgoing.ForeColor = SystemColors.ControlText;
    this.cbOutgoing.Name = "cbOutgoing";
    this.cbOutgoing.UseVisualStyleBackColor = true;
    this.cbOutgoing.CheckedChanged += new EventHandler(this.CbCheckedChanged);
    componentResourceManager.ApplyResources((object) this.cbIncoming, "cbIncoming");
    this.cbIncoming.Checked = true;
    this.cbIncoming.CheckState = CheckState.Checked;
    this.cbIncoming.ForeColor = SystemColors.ControlText;
    this.cbIncoming.Name = "cbIncoming";
    this.cbIncoming.UseVisualStyleBackColor = true;
    this.cbIncoming.CheckedChanged += new EventHandler(this.CbCheckedChanged);
    this.groupBox1.Controls.Add((Control) this.tabControl1);
    this.groupBox1.ForeColor = SystemColors.HotTrack;
    componentResourceManager.ApplyResources((object) this.groupBox1, "groupBox1");
    this.groupBox1.Name = "groupBox1";
    this.groupBox1.TabStop = false;
    this.tabControl1.Controls.Add((Control) this.tpInput);
    this.tabControl1.Controls.Add((Control) this.tpOutput);
    this.tabControl1.Controls.Add((Control) this.tpInternal);
    componentResourceManager.ApplyResources((object) this.tabControl1, "tabControl1");
    this.tabControl1.Name = "tabControl1";
    this.tabControl1.SelectedIndex = 0;
    this.tpInput.BackColor = SystemColors.Control;
    componentResourceManager.ApplyResources((object) this.tpInput, "tpInput");
    this.tpInput.Name = "tpInput";
    this.tpOutput.BackColor = SystemColors.Control;
    componentResourceManager.ApplyResources((object) this.tpOutput, "tpOutput");
    this.tpOutput.Name = "tpOutput";
    this.tpInternal.BackColor = SystemColors.Control;
    componentResourceManager.ApplyResources((object) this.tpInternal, "tpInternal");
    this.tpInternal.Name = "tpInternal";
    this.groupBox4.Controls.Add((Control) this.bSucNoControl);
    this.groupBox4.Controls.Add((Control) this.tbSucNoControl);
    this.groupBox4.Controls.Add((Control) this.label3);
    this.groupBox4.Controls.Add((Control) this.bSucControl);
    this.groupBox4.Controls.Add((Control) this.tbSucControl);
    this.groupBox4.Controls.Add((Control) this.label4);
    this.groupBox4.ForeColor = SystemColors.HotTrack;
    componentResourceManager.ApplyResources((object) this.groupBox4, "groupBox4");
    this.groupBox4.Name = "groupBox4";
    this.groupBox4.TabStop = false;
    this.bSucNoControl.ForeColor = SystemColors.ControlText;
    componentResourceManager.ApplyResources((object) this.bSucNoControl, "bSucNoControl");
    this.bSucNoControl.Name = "bSucNoControl";
    this.bSucNoControl.UseVisualStyleBackColor = true;
    this.bSucNoControl.Click += new EventHandler(this.bSucNoControl_Click);
    this.tbSucNoControl.BackColor = SystemColors.Window;
    this.tbSucNoControl.ForeColor = SystemColors.WindowText;
    componentResourceManager.ApplyResources((object) this.tbSucNoControl, "tbSucNoControl");
    this.tbSucNoControl.Name = "tbSucNoControl";
    this.tbSucNoControl.ReadOnly = true;
    componentResourceManager.ApplyResources((object) this.label3, "label3");
    this.label3.ForeColor = SystemColors.ControlText;
    this.label3.Name = "label3";
    this.bSucControl.ForeColor = SystemColors.ControlText;
    componentResourceManager.ApplyResources((object) this.bSucControl, "bSucControl");
    this.bSucControl.Name = "bSucControl";
    this.bSucControl.UseVisualStyleBackColor = true;
    this.bSucControl.Click += new EventHandler(this.bSucControl_Click);
    this.tbSucControl.BackColor = SystemColors.Window;
    this.tbSucControl.ForeColor = SystemColors.WindowText;
    componentResourceManager.ApplyResources((object) this.tbSucControl, "tbSucControl");
    this.tbSucControl.Name = "tbSucControl";
    this.tbSucControl.ReadOnly = true;
    componentResourceManager.ApplyResources((object) this.label4, "label4");
    this.label4.ForeColor = SystemColors.ControlText;
    this.label4.Name = "label4";
    componentResourceManager.ApplyResources((object) this, "$this");
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add((Control) this.groupBox4);
    this.Controls.Add((Control) this.groupBox3);
    this.Controls.Add((Control) this.groupBox2);
    this.Controls.Add((Control) this.groupBox1);
    this.Name = nameof (OfficeDocSettingsForm);
    this.groupBox3.ResumeLayout(false);
    this.groupBox3.PerformLayout();
    this.groupBox2.ResumeLayout(false);
    this.groupBox2.PerformLayout();
    this.groupBox1.ResumeLayout(false);
    this.tabControl1.ResumeLayout(false);
    this.groupBox4.ResumeLayout(false);
    this.groupBox4.PerformLayout();
    this.ResumeLayout(false);
  }
}
