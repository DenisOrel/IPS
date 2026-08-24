// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.NotificationsSettings
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using DevExpress.IM.Utils;
using DevExpress.IM.XtraTreeList;
using DevExpress.IM.XtraTreeList.Columns;
using DevExpress.IM.XtraTreeList.Nodes;
using ImSSP;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.WebPortal;
using Intermech.Localization;
using Intermech.Site.Client.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Site.Client;

public class NotificationsSettings : UserControl, IPropertyPage
{
  private bool _changed;
  private Dictionary<TaskType, TaskNotifications> _notifications;
  private IContainer components;
  private Panel panel4;
  private Button bDelete;
  private Button bAdd;
  private Button bEdit;
  private TreeList treeList1;
  private TreeListColumn treeListColumn1;
  private TreeList treeList2;
  private TreeListColumn colUser;
  private TreeListColumn colEmail;
  private SplitContainer splitContainer1;
  private Panel panel1;
  private ComboBox cbSenderAccaunt;
  private Label label1;
  private Button bRefresh;

  public event EventHandler Changed;

  public PropertyPageType Type => PropertyPageType.Control;

  public object Control => (object) this;

  public string PageName => LocalizationHolder.rm.GetString("Site.Client_71");

  public string HeaderText
  {
    [DebuggerStepThrough] get => this.PageName;
  }

  public void Apply()
  {
    if (!this._changed)
      return;
    if (!(this.cbSenderAccaunt.SelectedItem is EmailAccaunt))
      throw new Exception(LocalizationHolder.rm.GetString(sc_18669.ssp_webportal_18670()));
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      TaskNotifications.SetAccauntSender(sessionKeeper.Session, ((EmailAccaunt) this.cbSenderAccaunt.SelectedItem).Guid);
      foreach (TaskType key in Enum.GetValues(typeof (TaskType)))
        this._notifications[key].Save(sessionKeeper.Session);
    }
    this._changed = false;
  }

  public void Cancel()
  {
    Array values = Enum.GetValues(typeof (TaskType));
    this._notifications = new Dictionary<TaskType, TaskNotifications>(values.Length);
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      foreach (TaskType taskType in values)
        this._notifications.Add(taskType, TaskNotifications.GetNotifications(sessionKeeper.Session, taskType));
      this.ReloadCombo(sessionKeeper.Session);
    }
    this.ReloadControl();
    this.ReloadButtons();
    this._changed = false;
  }

  public string HelpTopicID => string.Empty;

  private void ReloadCombo(IUserSession session)
  {
    this.cbSenderAccaunt.Items.Clear();
    IEmailService customService = (IEmailService) session.GetCustomService(typeof (IEmailService));
    EmailServer[] servers = customService.Servers;
    if (servers == null || servers.Length == 0)
      return;
    Guid accauntSender = TaskNotifications.GetAccauntSender(session);
    int num = 0;
    for (int index1 = 0; index1 < servers.Length; ++index1)
    {
      EmailAccaunt[] accaunts = customService.GetAccaunts(servers[index1].Guid);
      if (accaunts != null && accaunts.Length != 0)
      {
        this.cbSenderAccaunt.Items.Add((object) $"----- {servers[index1].Name} -----");
        for (int index2 = 0; index2 < accaunts.Length; ++index2)
        {
          this.cbSenderAccaunt.Items.Add((object) accaunts[index2]);
          if (accaunts[index2].Guid == accauntSender)
            num = this.cbSenderAccaunt.Items.Count - 1;
        }
      }
    }
    if (num <= 0)
      return;
    this.cbSenderAccaunt.SelectedIndex = num;
  }

  public NotificationsSettings()
  {
    this.InitializeComponent();
    Array values = Enum.GetValues(typeof (TaskType));
    this._notifications = new Dictionary<TaskType, TaskNotifications>(values.Length);
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      this.ReloadCombo(sessionKeeper.Session);
      foreach (TaskType taskType in values)
      {
        this.treeList1.AppendNode((object) new object[1]
        {
          (object) EnumDescConverter.GetEnumDescription((Enum) taskType)
        }, (TreeListNode) null).Tag = (object) taskType;
        this._notifications.Add(taskType, TaskNotifications.GetNotifications(sessionKeeper.Session, taskType));
      }
    }
    if (this.treeList1.Nodes.Count <= 0)
      return;
    this.treeList1.FocusedNode = this.treeList1.Nodes[0];
    this.ReloadControl();
    this.ReloadButtons();
  }

  private void treeList1_FocusedNodeChanged(object sender, FocusedNodeChangedEventArgs e)
  {
    this.ReloadControl();
    this.ReloadButtons();
  }

  private void ReloadButtons()
  {
    this.bEdit.Enabled = this.bDelete.Enabled = this.treeList2.FocusedNode != null;
  }

  private TreeListNode AddNotificationNode(IUserSession session, TaskNotification notif)
  {
    TreeListNode treeListNode = this.treeList2.AppendNode((object) new object[2]
    {
      (object) notif.User,
      (object) notif.Email
    }, (TreeListNode) null);
    treeListNode.Tag = (object) notif;
    return treeListNode;
  }

  private void ReloadNode(IUserSession session, TreeListNode node, TaskNotification notif)
  {
    node.SetValue((object) 0, (object) notif.User);
    node.SetValue((object) 1, (object) notif.Email);
  }

  private void ReloadControl()
  {
    this.treeList2.Nodes.Clear();
    if (this._notifications == null || this.treeList1.FocusedNode == null || this.treeList1.FocusedNode.Tag == null)
      return;
    List<TaskNotification> notifications = this._notifications[(TaskType) this.treeList1.FocusedNode.Tag].Notifications;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      for (int index = 0; index < notifications.Count; ++index)
        this.AddNotificationNode(sessionKeeper.Session, notifications[index]);
    }
    if (this.treeList2.Nodes.Count <= 0)
      return;
    this.treeList2.FocusedNode = this.treeList2.Nodes[0];
  }

  private void OnChanged()
  {
    if (this.Changed != null)
      this.Changed((object) this, new EventArgs());
    this._changed = true;
  }

  private void bAdd_Click(object sender, EventArgs e)
  {
    TaskNotification taskNotification = new TaskNotification(string.Empty, string.Empty, false);
    if (!NewNotificationForm.Show(taskNotification))
      return;
    this._notifications[(TaskType) this.treeList1.FocusedNode.Tag].Notifications.Add(taskNotification);
    using (SessionKeeper sessionKeeper = new SessionKeeper())
      this.treeList2.FocusedNode = this.AddNotificationNode(sessionKeeper.Session, taskNotification);
    this.OnChanged();
  }

  private void bDelete_Click(object sender, EventArgs e)
  {
    if (this.treeList2.FocusedNode == null)
      return;
    this._notifications[(TaskType) this.treeList1.FocusedNode.Tag].Notifications.Remove((TaskNotification) this.treeList2.FocusedNode.Tag);
    this.treeList2.Nodes.Remove(this.treeList2.FocusedNode);
    this.OnChanged();
  }

  private void bEdit_Click(object sender, EventArgs e)
  {
    if (this.treeList2.FocusedNode == null)
      return;
    if (NewNotificationForm.Show((TaskNotification) this.treeList2.FocusedNode.Tag))
    {
      using (SessionKeeper sessionKeeper = new SessionKeeper())
        this.ReloadNode(sessionKeeper.Session, this.treeList2.FocusedNode, (TaskNotification) this.treeList2.FocusedNode.Tag);
    }
    this.OnChanged();
  }

  private void treeList2_FocusedNodeChanged(object sender, FocusedNodeChangedEventArgs e)
  {
    this.ReloadButtons();
  }

  private void treeList2_CustomDrawNodeCell(object sender, CustomDrawNodeCellEventArgs e)
  {
    TaskNotification tag = (TaskNotification) e.Node.Tag;
    e.Style = tag.Enable ? this.treeList2.Styles["Row"] : this.treeList2.Styles["Style1"];
  }

  private void treeList2_DoubleClick(object sender, EventArgs e) => this.bEdit_Click(sender, e);

  private void cbSenderAccaunt_SelectedIndexChanged(object sender, EventArgs e) => this.OnChanged();

  private void bRefresh_Click(object sender, EventArgs e)
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
      this.ReloadCombo(sessionKeeper.Session);
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (NotificationsSettings));
    this.treeList2 = new TreeList();
    this.colUser = new TreeListColumn();
    this.colEmail = new TreeListColumn();
    this.panel4 = new Panel();
    this.bEdit = new Button();
    this.bDelete = new Button();
    this.bAdd = new Button();
    this.treeList1 = new TreeList();
    this.treeListColumn1 = new TreeListColumn();
    this.splitContainer1 = new SplitContainer();
    this.panel1 = new Panel();
    this.bRefresh = new Button();
    this.cbSenderAccaunt = new ComboBox();
    this.label1 = new Label();
    this.treeList2.BeginInit();
    this.panel4.SuspendLayout();
    this.treeList1.BeginInit();
    this.splitContainer1.BeginInit();
    this.splitContainer1.Panel1.SuspendLayout();
    this.splitContainer1.Panel2.SuspendLayout();
    this.splitContainer1.SuspendLayout();
    this.panel1.SuspendLayout();
    this.SuspendLayout();
    componentResourceManager.ApplyResources((object) this.treeList2, "treeList2");
    this.treeList2.Columns.AddRange(new TreeListColumn[2]
    {
      this.colUser,
      this.colEmail
    });
    this.treeList2.Name = "treeList2";
    this.treeList2.Styles.AddReplace("HideSelectionRow", (object) new ViewStyle("HideSelectionRow", "TreeList", new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204), "", StyleOptions.StyleEnabled | StyleOptions.UseBackColor | StyleOptions.UseDrawFocusRect | StyleOptions.UseFont | StyleOptions.UseForeColor | StyleOptions.UseImage, true, false, false, HorzAlignment.Default, VertAlignment.Center, (Image) null, SystemColors.Window, SystemColors.WindowText));
    this.treeList2.Styles.AddReplace("FocusedRow", (object) new ViewStyle("FocusedRow", "TreeList", new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204), "", StyleOptions.StyleEnabled | StyleOptions.UseBackColor | StyleOptions.UseDrawFocusRect | StyleOptions.UseFont | StyleOptions.UseForeColor | StyleOptions.UseImage, true, false, false, HorzAlignment.Default, VertAlignment.Center, (Image) null, SystemColors.Window, SystemColors.WindowText));
    this.treeList2.Styles.AddReplace("Style1", (object) new ViewStyle("Style1", "", new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204), "", StyleOptions.StyleEnabled | StyleOptions.UseBackColor | StyleOptions.UseDrawEndEllipsis | StyleOptions.UseDrawFocusRect | StyleOptions.UseFont | StyleOptions.UseForeColor | StyleOptions.UseHorzAlignment | StyleOptions.UseImage | StyleOptions.UseWordWrap | StyleOptions.UseVertAlignment, true, false, false, HorzAlignment.Default, VertAlignment.Center, (Image) null, SystemColors.Window, SystemColors.ControlDarkDark));
    this.treeList2.Styles.AddReplace("SelectedRow", (object) new ViewStyle("SelectedRow", "TreeList", new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204), "", StyleOptions.StyleEnabled | StyleOptions.UseBackColor | StyleOptions.UseDrawFocusRect | StyleOptions.UseFont | StyleOptions.UseForeColor | StyleOptions.UseImage, true, false, false, HorzAlignment.Default, VertAlignment.Center, (Image) null, SystemColors.Window, SystemColors.WindowText));
    this.treeList2.FocusedNodeChanged += new FocusedNodeChangedEventHandler(this.treeList2_FocusedNodeChanged);
    this.treeList2.CustomDrawNodeCell += new CustomDrawNodeCellEventHandler(this.treeList2_CustomDrawNodeCell);
    this.treeList2.DoubleClick += new EventHandler(this.treeList2_DoubleClick);
    componentResourceManager.ApplyResources((object) this.colUser, "colUser");
    this.colUser.Name = "colUser";
    componentResourceManager.ApplyResources((object) this.colEmail, "colEmail");
    this.colEmail.Name = "colEmail";
    this.panel4.Controls.Add((System.Windows.Forms.Control) this.bEdit);
    this.panel4.Controls.Add((System.Windows.Forms.Control) this.bDelete);
    this.panel4.Controls.Add((System.Windows.Forms.Control) this.bAdd);
    componentResourceManager.ApplyResources((object) this.panel4, "panel4");
    this.panel4.Name = "panel4";
    componentResourceManager.ApplyResources((object) this.bEdit, "bEdit");
    this.bEdit.Name = "bEdit";
    this.bEdit.UseVisualStyleBackColor = true;
    this.bEdit.Click += new EventHandler(this.bEdit_Click);
    componentResourceManager.ApplyResources((object) this.bDelete, "bDelete");
    this.bDelete.Name = "bDelete";
    this.bDelete.UseVisualStyleBackColor = true;
    this.bDelete.Click += new EventHandler(this.bDelete_Click);
    componentResourceManager.ApplyResources((object) this.bAdd, "bAdd");
    this.bAdd.Name = "bAdd";
    this.bAdd.UseVisualStyleBackColor = true;
    this.bAdd.Click += new EventHandler(this.bAdd_Click);
    componentResourceManager.ApplyResources((object) this.treeList1, "treeList1");
    this.treeList1.Columns.AddRange(new TreeListColumn[1]
    {
      this.treeListColumn1
    });
    this.treeList1.Name = "treeList1";
    this.treeList1.FocusedNodeChanged += new FocusedNodeChangedEventHandler(this.treeList1_FocusedNodeChanged);
    componentResourceManager.ApplyResources((object) this.treeListColumn1, "treeListColumn1");
    this.treeListColumn1.Name = "treeListColumn1";
    componentResourceManager.ApplyResources((object) this.splitContainer1, "splitContainer1");
    this.splitContainer1.Name = "splitContainer1";
    this.splitContainer1.Panel1.Controls.Add((System.Windows.Forms.Control) this.treeList1);
    this.splitContainer1.Panel2.Controls.Add((System.Windows.Forms.Control) this.treeList2);
    this.splitContainer1.Panel2.Controls.Add((System.Windows.Forms.Control) this.panel4);
    this.panel1.Controls.Add((System.Windows.Forms.Control) this.bRefresh);
    this.panel1.Controls.Add((System.Windows.Forms.Control) this.cbSenderAccaunt);
    this.panel1.Controls.Add((System.Windows.Forms.Control) this.label1);
    componentResourceManager.ApplyResources((object) this.panel1, "panel1");
    this.panel1.Name = "panel1";
    this.bRefresh.Image = (Image) Intermech.Site.Client.Properties.Resources.refresh;
    componentResourceManager.ApplyResources((object) this.bRefresh, "bRefresh");
    this.bRefresh.Name = "bRefresh";
    this.bRefresh.UseVisualStyleBackColor = true;
    this.bRefresh.Click += new EventHandler(this.bRefresh_Click);
    this.cbSenderAccaunt.FormattingEnabled = true;
    componentResourceManager.ApplyResources((object) this.cbSenderAccaunt, "cbSenderAccaunt");
    this.cbSenderAccaunt.Name = "cbSenderAccaunt";
    this.cbSenderAccaunt.SelectedIndexChanged += new EventHandler(this.cbSenderAccaunt_SelectedIndexChanged);
    componentResourceManager.ApplyResources((object) this.label1, "label1");
    this.label1.Name = "label1";
    componentResourceManager.ApplyResources((object) this, "$this");
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add((System.Windows.Forms.Control) this.splitContainer1);
    this.Controls.Add((System.Windows.Forms.Control) this.panel1);
    this.Name = nameof (NotificationsSettings);
    this.treeList2.EndInit();
    this.panel4.ResumeLayout(false);
    this.treeList1.EndInit();
    this.splitContainer1.Panel1.ResumeLayout(false);
    this.splitContainer1.Panel2.ResumeLayout(false);
    this.splitContainer1.EndInit();
    this.splitContainer1.ResumeLayout(false);
    this.panel1.ResumeLayout(false);
    this.panel1.PerformLayout();
    this.ResumeLayout(false);
  }
}
