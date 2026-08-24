// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.FillCompositionForm
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using ImSSP;
using Intermech.Client.Core;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Kernel.Search;
using Intermech.Localization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Threading;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Pdm.Compositions;

internal class FillCompositionForm : Form
{
  private long _rootAssemblyID;
  private Thread _fillThread;
  private int _attrFirstApplicabilityID;
  private bool _abortThread;
  private INotificationService _notificationService;
  private List<long> _objectsForNotification = new List<long>();
  private List<int> _typesForNotification = new List<int>();
  private IContainer components;
  private Panel panel1;
  private Label lCurObjName;
  private Label label1;
  private Panel panel2;
  private Button bOK;
  private Panel panel3;
  private ListBox listBox1;

  public FillCompositionForm(long rootAssemblyID)
  {
    this._rootAssemblyID = rootAssemblyID;
    this._attrFirstApplicabilityID = MetaDataHelper.GetAttributeTypeID("cad00285-306c-11d8-b4e9-00304f19f545");
    this.InitializeComponent();
    this._notificationService = ApplicationServices.Container.GetService<INotificationService>();
    FormStorage.LoadLayout((Control) this);
    this.bOK.Enabled = false;
    this.listBox1.Items.Clear();
  }

  public void Start()
  {
    using (FixEditingContext fixEditingContext = new FixEditingContext())
    {
      this._abortThread = false;
      this._fillThread = new Thread(fixEditingContext.SendEditingContextToThread(new ThreadStart(this.FillThread)));
      this._fillThread.IsBackground = true;
      this._fillThread.Name = "FillComposition_Thread";
      this._fillThread.Start();
    }
    if (this.ShowDialog() == DialogResult.Cancel && this._fillThread != null && this._fillThread.IsAlive)
    {
      this._abortThread = true;
      this._fillThread.Join();
    }
    FormStorage.SaveLayout((Control) this);
  }

  private void FillThread()
  {
    bool flag = false;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      DBRecordSetParams dbParams = new DBRecordSetParams((ConditionStructure[]) null, new object[1]
      {
        (object) -2
      });
      IDBRelationCollection relationCollection1 = sessionKeeper.Session.GetRelationCollection(sessionKeeper.Session.IdentHelper.DocRelationTypeID);
      IDBRelationCollection relationCollection2 = sessionKeeper.Session.GetRelationCollection(sessionKeeper.Session.IdentHelper.SPRelationTypeID);
      try
      {
        this.FillInLevel(sessionKeeper.Session, dbParams, sessionKeeper.Session.GetObject(this._rootAssemblyID), (IDBObject) null, relationCollection1, relationCollection2);
      }
      catch (Exception ex)
      {
        flag = true;
        if (!this._abortThread)
          ExceptionHelper.ExceptionService.ShowException(ex);
      }
      finally
      {
        this._notificationService.FireEvent((object) this, (NotificationEventArgs) new DBObjectsEventArgs("ObjectsChanged", (IList<long>) this._objectsForNotification, (IList<int>) this._typesForNotification));
      }
    }
    if (this._abortThread)
      this.CloseForm();
    else if (flag)
      this.SetError();
    else
      this.SetEnableOKButton();
  }

  private void FillInLevel(
    IUserSession session,
    DBRecordSetParams dbParams,
    IDBObject rootObject,
    IDBObject parentObject,
    IDBRelationCollection relCollDoc,
    IDBRelationCollection relCollArt)
  {
    if (this._abortThread)
      return;
    string empty = string.Empty;
    IDBAttribute attributeByGuid1 = rootObject.GetAttributeByGuid(new Guid("cad0001f-306c-11d8-b4e9-00304f19f545"));
    string attributeValue1 = attributeByGuid1 == null || !(attributeByGuid1.AsString != string.Empty) ? string.Format(LocalizationHolder.rm.GetString("Pdm_59"), (object) rootObject.ObjectID) : attributeByGuid1.AsString;
    string attributeValue2 = string.Empty;
    if (parentObject != null)
    {
      IDBAttribute attributeByGuid2 = parentObject.GetAttributeByGuid(new Guid("cad0001f-306c-11d8-b4e9-00304f19f545"));
      attributeValue2 = attributeByGuid2 == null || !(attributeByGuid2.AsString != string.Empty) ? string.Format(LocalizationHolder.rm.GetString("Pdm_59"), (object) parentObject.ObjectID) : attributeByGuid2.AsString;
    }
    DataTable dataTable1 = relCollDoc.ConsistFrom(dbParams, rootObject.ObjectID);
    if (dataTable1.Rows.Count > 0)
    {
      foreach (DataRow row in (InternalDataCollectionBase) dataTable1.Rows)
      {
        if (this._abortThread)
          return;
        IDBObject dbObj = session.GetObject(Convert.ToInt64(row[0]), false) ?? session.GetObject(Math.Abs(Convert.ToInt64(row[0])), false);
        if (dbObj != null)
          this.SetAttributeFirstApplicability(session, dbObj, attributeValue2);
      }
    }
    DataTable dataTable2 = relCollArt.ConsistFrom(dbParams, rootObject.ObjectID);
    if (dataTable2.Rows.Count <= 0)
      return;
    foreach (DataRow row in (InternalDataCollectionBase) dataTable2.Rows)
    {
      if (this._abortThread)
        break;
      IDBObject dbObject = session.GetObject(Convert.ToInt64(row[0]), false);
      this.SetAttributeFirstApplicability(session, dbObject, attributeValue1);
      this.FillInLevel(session, dbParams, dbObject, rootObject, relCollDoc, relCollArt);
    }
  }

  private void SetAttributeFirstApplicability(
    IUserSession session,
    IDBObject dbObj,
    string attributeValue)
  {
    bool flag = false;
    if (dbObj == null)
      return;
    long objectId = dbObj.ObjectID;
    int num = MetaDataHelper.GetAttribute4ObjectType(dbObj.ObjectType, this._attrFirstApplicabilityID) != null ? 1 : 0;
    IDBAttribute attributeById = dbObj.GetAttributeByID(this._attrFirstApplicabilityID);
    if (num == 0 && attributeById == null)
      return;
    this.SetObjectName(dbObj.NameInMessages);
    string text = string.Empty;
    try
    {
      if (dbObj.CheckoutBy != 0L && dbObj.CheckoutBy != session.UserID)
        text = string.Format(LocalizationHolder.rm.GetString(sc_16689.ssp_pdm_16690()), (object) dbObj.NameInMessages);
      else if (dbObj.ObjectModifyMode != ObjectModifyModes.CantModify && dbObj.ObjectModifyMode != ObjectModifyModes.CreateVersion)
      {
        IDBAttribute byId = dbObj.Attributes.FindByID(this._attrFirstApplicabilityID);
        if (byId != null && !string.IsNullOrEmpty(byId.AsString))
          return;
        if (dbObj.CheckoutBy == 0L && dbObj.ObjectModifyMode == ObjectModifyModes.Checkout)
        {
          dbObj = dbObj.CheckOut();
          flag = true;
        }
        AttributeValues attributeValues = new AttributeValues(this._attrFirstApplicabilityID, (object) attributeValue);
        dbObj.SetAttributesValuesEx(new AttributeValues[1]
        {
          attributeValues
        }, false, false, false, GetAttributeValuesModes.IncludeName);
        if (flag)
        {
          try
          {
            dbObj.CheckIn();
          }
          catch
          {
            dbObj.CancelChanges();
            throw;
          }
        }
        this._objectsForNotification.Add(dbObj.ObjectID);
        this._typesForNotification.Add(dbObj.ObjectType);
        text = string.Format(LocalizationHolder.rm.GetString("Pdm_61"), (object) dbObj.NameInMessages, (object) attributeValue);
      }
      else
        text = string.Format(LocalizationHolder.rm.GetString(sc_16689.ssp_pdm_16691()), (object) dbObj.NameInMessages);
    }
    catch (Exception ex)
    {
      text = string.Format(sc_16689.ssp_pdm_16692(), (object) dbObj.NameInMessages, (object) ex.Message);
    }
    finally
    {
      if (!string.IsNullOrEmpty(text))
        this.SetResultText(text);
    }
  }

  private void SetResultText(string text)
  {
    if (!this.Visible)
      return;
    this.Invoke((Delegate) new FillCompositionForm.SetResultTextDelegate(this._setResultText), (object) text);
  }

  private void _setResultText(string text) => this.listBox1.Items.Add((object) text);

  private void SetObjectName(string objName)
  {
    if (!this.Visible)
      return;
    this.Invoke((Delegate) new FillCompositionForm.SetObjectNameDelegate(this._setObjectName), (object) objName);
  }

  private void _setObjectName(string objName) => this.lCurObjName.Text = objName;

  private void SetEnableOKButton()
  {
    if (!this.Visible)
      return;
    this.Invoke((Delegate) new MethodInvoker(this._setEnableOKButton));
  }

  private void SetError()
  {
    if (!this.Visible)
      return;
    this.Invoke((Delegate) new MethodInvoker(this._setError));
  }

  private void CloseForm()
  {
    if (!this.Visible)
      return;
    this.Invoke((Delegate) new MethodInvoker(this._closeForm));
  }

  private void _closeForm() => this.Close();

  private void _setEnableOKButton()
  {
    this.lCurObjName.Text = LocalizationHolder.rm.GetString("Pdm_63");
    this.bOK.Enabled = true;
  }

  private void _setError()
  {
    this.lCurObjName.Text = LocalizationHolder.rm.GetString("Pdm_527");
    this.bOK.Enabled = false;
  }

  private void bCancel_Click(object sender, EventArgs e)
  {
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (FillCompositionForm));
    this.panel1 = new Panel();
    this.lCurObjName = new Label();
    this.label1 = new Label();
    this.panel2 = new Panel();
    this.bOK = new Button();
    this.panel3 = new Panel();
    this.listBox1 = new ListBox();
    this.panel1.SuspendLayout();
    this.panel2.SuspendLayout();
    this.panel3.SuspendLayout();
    this.SuspendLayout();
    this.panel1.Controls.Add((Control) this.lCurObjName);
    this.panel1.Controls.Add((Control) this.label1);
    componentResourceManager.ApplyResources((object) this.panel1, "panel1");
    this.panel1.Name = "panel1";
    componentResourceManager.ApplyResources((object) this.lCurObjName, "lCurObjName");
    this.lCurObjName.Name = "lCurObjName";
    componentResourceManager.ApplyResources((object) this.label1, "label1");
    this.label1.Name = "label1";
    this.panel2.Controls.Add((Control) this.bOK);
    componentResourceManager.ApplyResources((object) this.panel2, "panel2");
    this.panel2.Name = "panel2";
    componentResourceManager.ApplyResources((object) this.bOK, "bOK");
    this.bOK.DialogResult = DialogResult.OK;
    this.bOK.Name = "bOK";
    this.bOK.UseVisualStyleBackColor = true;
    this.panel3.Controls.Add((Control) this.listBox1);
    componentResourceManager.ApplyResources((object) this.panel3, "panel3");
    this.panel3.Name = "panel3";
    componentResourceManager.ApplyResources((object) this.listBox1, "listBox1");
    this.listBox1.FormattingEnabled = true;
    this.listBox1.Name = "listBox1";
    this.AcceptButton = (IButtonControl) this.bOK;
    componentResourceManager.ApplyResources((object) this, "$this");
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add((Control) this.panel3);
    this.Controls.Add((Control) this.panel2);
    this.Controls.Add((Control) this.panel1);
    this.MaximizeBox = false;
    this.MinimizeBox = false;
    this.Name = nameof (FillCompositionForm);
    this.Tag = (object) " ";
    this.panel1.ResumeLayout(false);
    this.panel1.PerformLayout();
    this.panel2.ResumeLayout(false);
    this.panel3.ResumeLayout(false);
    this.ResumeLayout(false);
  }

  private delegate void SetResultTextDelegate(string text);

  private delegate void SetObjectNameDelegate(string objName);
}
