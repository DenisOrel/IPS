// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Client.SignControlPropertyEditorForm
// Assembly: Intermech.Signs, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: A3C02709-D794-49CE-8C55-5624449406B7
// Assembly location: D:\IPS\Client\Intermech.Signs.dll
// XML documentation location: D:\IPS\Client\Intermech.Signs.xml

using Intermech.Client.Core;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Kernel.Search;
using Intermech.Localization;
using Intermech.Signs.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Signs.Client;

/// <summary>
/// Форма редактирования настройки подписей для типов объекта
/// </summary>
internal class SignControlPropertyEditorForm : Form
{
  private System.ComponentModel.Container components;
  private Panel panel1;
  private Panel panel2;
  private Button _bOk;
  private Panel _forControl;
  private Button _bCancel;
  private SignControl _control;
  private SignControlPropertyClass _originalClass;
  private SignControlPropertyClass _internalClass;
  private bool _modified;

  public SignControlPropertyEditorForm()
  {
    this.InitializeComponent();
    this._control = new SignControl();
    this._forControl.SuspendLayout();
    this._forControl.Controls.Add((Control) this._control);
    this._control.Dock = DockStyle.Fill;
    this._control.OnModified += new EventHandler(this._control_OnModified);
    this._forControl.ResumeLayout(false);
    HelpProvidersClass.SetHelpOptionForControl((Control) this, 1267);
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  /// <summary>Класс настройки подписей</summary>
  public SignControlPropertyClass SignControlPropertyClass
  {
    get
    {
      if (!this._modified)
        return this._originalClass;
      SignControlPropertyClass internalClass = this._internalClass;
      internalClass.GraphsSet = this._control.Set;
      return internalClass;
    }
    set
    {
      this._originalClass = value;
      this._internalClass = SignControlPropertyClass.Clone(this._originalClass);
      this._control.Set = this._internalClass.GraphsSet;
    }
  }

  /// <summary>Информация только для просмотра (никаких изменений)</summary>
  public bool ReadOnly
  {
    set
    {
      this._control.ReadOnly |= value;
      this._bOk.Enabled = !value;
    }
    get => this._control.ReadOnly;
  }

  /// <summary>
  /// Required method for Designer support - do not modify
  /// the contents of this method with the code editor.
  /// </summary>
  private void InitializeComponent()
  {
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (SignControlPropertyEditorForm));
    this.panel1 = new Panel();
    this.panel2 = new Panel();
    this._bCancel = new Button();
    this._bOk = new Button();
    this._forControl = new Panel();
    this.panel1.SuspendLayout();
    this.panel2.SuspendLayout();
    this.SuspendLayout();
    this.panel1.Controls.Add((Control) this.panel2);
    componentResourceManager.ApplyResources((object) this.panel1, "panel1");
    this.panel1.Name = "panel1";
    this.panel2.Controls.Add((Control) this._bCancel);
    this.panel2.Controls.Add((Control) this._bOk);
    componentResourceManager.ApplyResources((object) this.panel2, "panel2");
    this.panel2.Name = "panel2";
    this._bCancel.DialogResult = DialogResult.Cancel;
    componentResourceManager.ApplyResources((object) this._bCancel, "_bCancel");
    this._bCancel.Name = "_bCancel";
    componentResourceManager.ApplyResources((object) this._bOk, "_bOk");
    this._bOk.Name = "_bOk";
    this._bOk.Click += new EventHandler(this._bOk_Click);
    componentResourceManager.ApplyResources((object) this._forControl, "_forControl");
    this._forControl.Name = "_forControl";
    this.AcceptButton = (IButtonControl) this._bOk;
    componentResourceManager.ApplyResources((object) this, "$this");
    this.CancelButton = (IButtonControl) this._bCancel;
    this.Controls.Add((Control) this._forControl);
    this.Controls.Add((Control) this.panel1);
    this.HelpButton = true;
    this.MaximizeBox = false;
    this.MinimizeBox = false;
    this.Name = nameof (SignControlPropertyEditorForm);
    this.ShowInTaskbar = false;
    this.Load += new EventHandler(this.SignControlPropertyEditorForm_Load);
    this.Closed += new EventHandler(this.SignControlPropertyEditorForm_Closed);
    this.panel1.ResumeLayout(false);
    this.panel2.ResumeLayout(false);
    this.ResumeLayout(false);
  }

  private void _control_OnModified(object sender, EventArgs e)
  {
    this._modified = this._control.Modified;
  }

  private void SignControlPropertyEditorForm_Load(object sender, EventArgs e)
  {
    FormStorage.LoadLayout((Control) this);
  }

  private void SignControlPropertyEditorForm_Closed(object sender, EventArgs e)
  {
    FormStorage.SaveLayout((Control) this);
  }

  private void _bOk_Click(object sender, EventArgs e)
  {
    if (!this._modified)
    {
      this.Close();
      this.DialogResult = DialogResult.Cancel;
    }
    else
    {
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        IUserSession session = sessionKeeper.Session;
        bool flag = true;
        this._internalClass.GraphsSet = this._control.Set;
        if (this._internalClass.isFilledOk)
        {
          DataTable dataTable = (DataTable) null;
          ISignsService customService = session.GetCustomService(typeof (ISignsService)) as ISignsService;
          if (this._internalClass.LCStep >= 0)
          {
            DBRecordSetParams dbRecordSetParams = new DBRecordSetParams(new ConditionStructure[1]
            {
              new ConditionStructure(new Guid("cad0002b-306c-11d8-b4e9-00304f19f545"), RelationalOperators.Equal, (object) this._internalClass.LCStep, LogicalOperators.NONE, 0)
            }, new object[1]
            {
              (object) ObligatoryObjectAttributes.F_OBJECT_ID
            });
            dataTable = session.ObjectsSelect(this._internalClass.ObjectTypeID, dbRecordSetParams);
          }
          if (this._internalClass.LCLevel >= 0)
          {
            List<int> intList = new List<int>();
            foreach (IMSObjectType objectTypes in MetaDataHelper.GetObjectTypesList())
            {
              IMSApplicability applicability = MetaDataHelper.GetApplicability(objectTypes.ObjectTypeID, SignsHolder.SignObjectTypeID, SignsHolder.SignRelationTypeID);
              if (applicability != null && applicability.ApplicabilityMode != ApplicabilityModes.Disabled && !intList.Contains(objectTypes.ObjectTypeID))
                intList.Add(objectTypes.ObjectTypeID);
            }
            DBRecordSetParams dbRecordSetParams = new DBRecordSetParams(new ConditionStructure[2]
            {
              new ConditionStructure(new Guid("cad0002e-306c-11d8-b4e9-00304f19f545"), RelationalOperators.In, (object) intList.ToArray(), LogicalOperators.AND, 0),
              new ConditionStructure(new Guid("cad00030-306c-11d8-b4e9-00304f19f545"), RelationalOperators.Equal, (object) this._internalClass.LCLevel, LogicalOperators.NONE, 0)
            }, new object[1]
            {
              (object) ObligatoryObjectAttributes.F_OBJECT_ID
            });
            dataTable = session.ObjectsSelect(-1, dbRecordSetParams);
          }
          long[] objectIDs = new long[0];
          if (dataTable != null)
          {
            ArrayList arrayList = new ArrayList();
            foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
            {
              long int64 = Convert.ToInt64(row[0]);
              if (!arrayList.Contains((object) int64))
                arrayList.Add((object) int64);
            }
            objectIDs = arrayList.ToArray(typeof (long)) as long[];
          }
          if (objectIDs.Length != 0)
          {
            try
            {
              string errorMessage = (string) null;
              object[] additionalInfo = (object[]) null;
              flag = customService.CheckSigns(objectIDs, this._internalClass.GraphsSet, session.SessionGUID, true, out errorMessage, out additionalInfo);
              if (!flag)
              {
                int num = (int) MessageBox.Show(errorMessage == null ? LocalizationHolder.rm.GetString("Signs_UnsignedObjectsDetected") : errorMessage, MessageDialogs.msgWarning);
              }
            }
            catch (Exception ex)
            {
              throw ex;
            }
          }
        }
        if (!flag)
          return;
        this.Close();
        this.DialogResult = DialogResult.OK;
      }
    }
  }
}
