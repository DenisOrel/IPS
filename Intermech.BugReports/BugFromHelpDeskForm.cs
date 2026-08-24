// Decompiled with JetBrains decompiler
// Type: Intermech.BugReports.BugFromHelpDeskForm
// Assembly: Intermech.BugReports, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 16F80F46-2B9D-4747-9BFD-4CC209192F4E
// Assembly location: D:\IPS\Client\Intermech.BugReports.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.HelpDesk;
using Intermech.Kernel.Search;
using Intermech.Navigator.Controls;
using Intermech.Remoting.Sponsors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

#nullable disable
namespace Intermech.BugReports;

public class BugFromHelpDeskForm : Form
{
  private static readonly Guid ObjBugGuid = new Guid("cad00700-306c-11d8-b4e9-00304f19f545");
  private static readonly Guid ObjBugWithAttachmentGuid = new Guid("86b0b79c-2d71-4c13-80e5-4a208eee963f");
  private static readonly Guid ShortInfo = new Guid("cad00706-306c-11d8-b4e9-00304f19f545");
  private static readonly Guid BugInfo = new Guid("cad0070c-306c-11d8-b4e9-00304f19f545");
  private static readonly Guid BugHelpDeskId = new Guid("32793b80-58bf-4cb7-916b-ad831240dd75");
  private static string _userLogin;
  private static string _userPassword;
  private static readonly byte[] Bytes = Encoding.ASCII.GetBytes("HelpDesk");
  private IContainer components;
  private Button btnOk;
  private Button btnCancel;
  private Label label1;
  private TextBox tbBugId;
  private GroupBox gbAuthentication;
  private Label lPassword;
  private TextBox tbPassword;
  private TextBox tbLogin;
  private Label lLogin;

  public BugFromHelpDeskForm()
  {
    this.InitializeComponent();
    Dictionary<string, string> userAuth = BugFromHelpDeskForm.GetUserAuth();
    if (userAuth == null)
      return;
    this.tbLogin.Text = userAuth.Keys.First<string>();
    this.tbLogin.BackColor = SystemColors.Info;
    this.tbPassword.Text = userAuth.Values.First<string>();
    this.tbPassword.BackColor = SystemColors.Info;
    this.tbBugId.Select();
  }

  public static void Execute()
  {
    using (BugFromHelpDeskForm fromHelpDeskForm = new BugFromHelpDeskForm())
    {
      if (fromHelpDeskForm.ShowDialog() != DialogResult.OK)
        return;
      string text = fromHelpDeskForm.tbBugId.Text;
      if (text.Length <= 0 || Convert.ToInt64(text) <= 0L)
        return;
      DataTable helpDeskData = BugFromHelpDeskForm.GetHelpDeskData(Convert.ToInt64(text));
      if (helpDeskData != null)
        BugFromHelpDeskForm.CreateBugObj(helpDeskData);
      BugFromHelpDeskForm.SaveUserAuth(BugFromHelpDeskForm._userLogin, BugFromHelpDeskForm._userPassword);
    }
  }

  private static DataTable GetHelpDeskData(long objHDId)
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
      return sessionKeeper.Session.GetCustomService(typeof (IHelpDeskService)) is IHelpDeskService customService && customService.ExistWorkOrder(objHDId) ? customService.HelpDeskDataTable(objHDId, customService.ExistAttachment(objHDId)) : (DataTable) null;
  }

  private static void CreateBugObj(DataTable helpDeskDataTable)
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      if (!(sessionKeeper.Session.GetCustomService(typeof (IHelpDeskService)) is IHelpDeskService customService))
        return;
      bool flag = customService.ExistAttachment((long) helpDeskDataTable.Rows[0]["WORKORDERID"]);
      int objectTypeId = MetaDataHelper.GetObjectTypeID(flag ? BugFromHelpDeskForm.ObjBugWithAttachmentGuid : BugFromHelpDeskForm.ObjBugGuid);
      if (BugFromHelpDeskForm.CheckObjectDesignation(objectTypeId, helpDeskDataTable.Rows[0]["TITLE"].ToString()))
      {
        int num1 = (int) MessageBox.Show($"Ошибка \"{helpDeskDataTable.Rows[0]["TITLE"]}\"уже существует.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        IDBObject objToLock = sessionKeeper.Session.GetObjectCollection(objectTypeId).Create();
        IDBAttribute attributeByGuid1 = objToLock.GetAttributeByGuid(BugFromHelpDeskForm.ShortInfo);
        if (attributeByGuid1 != null)
          attributeByGuid1.Value = helpDeskDataTable.Rows[0]["TITLE"];
        IDBAttribute attributeByGuid2 = objToLock.GetAttributeByGuid(BugFromHelpDeskForm.BugInfo);
        if (attributeByGuid2 != null)
          attributeByGuid2.Value = (object) BugFromHelpDeskForm.RemoveHtmlTag(helpDeskDataTable.Rows[0]["FULLDESCRIPTION"].ToString());
        IDBAttribute attributeByGuid3 = objToLock.GetAttributeByGuid(BugFromHelpDeskForm.BugHelpDeskId);
        if (attributeByGuid3 != null)
          attributeByGuid3.Value = helpDeskDataTable.Rows[0]["WORKORDERID"];
        IDBAttribute attributeByGuid4 = objToLock.GetAttributeByGuid(new Guid("cad0004b-306c-11d8-b4e9-00304f19f545"));
        if (attributeByGuid4 != null & flag)
        {
          using (RemoteLock remoteLock = new RemoteLock())
          {
            remoteLock.Add((object) objToLock);
            for (int index = 0; index < helpDeskDataTable.Rows.Count; ++index)
            {
              try
              {
                MemoryStream aSourceStream = new MemoryStream(customService.GetFile(Convert.ToInt32(helpDeskDataTable.Rows[index]["ATTACHMENTID"]), helpDeskDataTable.Rows[index]["ATTACHMENTKEY"].ToString(), BugFromHelpDeskForm._userLogin, BugFromHelpDeskForm._userPassword));
                BlobInformation aBlobInformation = new BlobInformation(0L, 0L, DateTime.Now, BugFromHelpDeskForm.CreateFileName(helpDeskDataTable.Rows[index]["ATTACHMENTNAME"].ToString()), BugFromHelpDeskForm.CheckFileType(helpDeskDataTable.Rows[index]["ATTACHMENTNAME"].ToString()) ? ArcMethods.ZLibPacked : ArcMethods.NotPacked, "image");
                if (index > 0)
                  attributeByGuid4.AddValue((object) null);
                new BlobProcWriter(objToLock.ObjectID, AttributableElements.Object, attributeByGuid4.AttributeID, index, 0, aBlobInformation, (Stream) aSourceStream, (BlobProcCustomClass.ProgressEventHandler) null, (BlobProcCustomClass.ThreadFinishEventHandler) null).WriteData();
              }
              catch (Exception ex)
              {
                int num2 = (int) MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              }
            }
            remoteLock.Remove((object) objToLock);
          }
        }
        if (objToLock.IsCreationMode)
          objToLock.CommitCreation(true, true);
        long objectId = objToLock.ObjectID;
        int num3 = (int) PropertiesWindow.Execute(string.Empty, string.Empty, objToLock.ObjectID, true);
        ((INotificationService) ServicesManager.GetService(typeof (INotificationService)))?.FireEvent((object) null, (NotificationEventArgs) new DBObjectsEventArgs("ObjectsCreated", objectId));
      }
    }
  }

  public static long GetObjectWithDesignation(int objectType, string designation)
  {
    if (!MetaDataHelper.ExistsObjectType(objectType))
      return 0;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      DataTable dataTable = sessionKeeper.Session.GetObjectCollection(objectType).Select(new DBRecordSetParams(new ConditionStructure[1]
      {
        new ConditionStructure(MetaDataHelper.GetAttributeTypeID("cad00047-306c-11d8-b4e9-00304f19f545"), RelationalOperators.Equal, (object) designation, LogicalOperators.NONE, 0, false)
      }, new ColumnDescriptor[1]
      {
        new ColumnDescriptor((object) -2, AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.Index, SortOrders.NONE, -1)
      }, recordCount: 1));
      if (dataTable == null)
        return 0;
      try
      {
        return dataTable.Rows.Count == 0 ? 0L : Convert.ToInt64(dataTable.Rows[0][0]);
      }
      finally
      {
        dataTable.Dispose();
      }
    }
  }

  private static bool CheckObjectDesignation(int objectTypeId, string designation)
  {
    return BugFromHelpDeskForm.GetObjectWithDesignation(objectTypeId, designation) != 0L;
  }

  private static string CreateFileName(string fileName)
  {
    string[] source = fileName.Split('.');
    return $"{((IEnumerable<string>) source).First<string>()}_{DateTime.Now.ToFileTime()}.{((IEnumerable<string>) source).Last<string>()}";
  }

  private static string RemoveHtmlTag(string fullDescription)
  {
    fullDescription = Regex.Replace(fullDescription, "<\\s*\\w.*?>", "\n");
    fullDescription = Regex.Replace(fullDescription, "<[^\\>]*>", "");
    fullDescription = WebUtility.HtmlDecode(fullDescription);
    return fullDescription;
  }

  private static Dictionary<string, string> GetUserAuth()
  {
    Dictionary<string, string> userAuth = new Dictionary<string, string>();
    if (!(HelpDeskSetting.Default.Password != ""))
      return (Dictionary<string, string>) null;
    userAuth.Add(HelpDeskSetting.Default.UserName, BugFromHelpDeskForm.Decrypt(HelpDeskSetting.Default.Password));
    return userAuth;
  }

  private static void SaveUserAuth(string userName, string password)
  {
    HelpDeskSetting.Default.UserName = userName;
    HelpDeskSetting.Default.Password = BugFromHelpDeskForm.Encrypt(password);
    HelpDeskSetting.Default.Save();
  }

  private static void RemoveUserAuth()
  {
    HelpDeskSetting.Default.Password = "";
    HelpDeskSetting.Default.UserName = "";
    HelpDeskSetting.Default.Save();
  }

  private static string Encrypt(string input)
  {
    using (DESCryptoServiceProvider cryptoServiceProvider = new DESCryptoServiceProvider())
    {
      MemoryStream memoryStream = new MemoryStream();
      CryptoStream cryptoStream = new CryptoStream((Stream) memoryStream, cryptoServiceProvider.CreateEncryptor(BugFromHelpDeskForm.Bytes, BugFromHelpDeskForm.Bytes), CryptoStreamMode.Write);
      StreamWriter streamWriter = new StreamWriter((Stream) cryptoStream);
      streamWriter.Write(input);
      streamWriter.Flush();
      cryptoStream.FlushFinalBlock();
      streamWriter.Flush();
      return Convert.ToBase64String(memoryStream.GetBuffer(), 0, (int) memoryStream.Length);
    }
  }

  private static string Decrypt(string input)
  {
    using (DESCryptoServiceProvider cryptoServiceProvider = new DESCryptoServiceProvider())
      return new StreamReader((Stream) new CryptoStream((Stream) new MemoryStream(Convert.FromBase64String(input)), cryptoServiceProvider.CreateDecryptor(BugFromHelpDeskForm.Bytes, BugFromHelpDeskForm.Bytes), CryptoStreamMode.Read)).ReadToEnd();
  }

  private static bool CheckFileType(string file)
  {
    string[] source = file.Split('.');
    return ((IEnumerable<string>) source).Last<string>() != "rar" && ((IEnumerable<string>) source).Last<string>() != "zip";
  }

  private void tbBugId_KeyPress(object sender, KeyPressEventArgs e)
  {
    if (e.KeyChar > '/' && e.KeyChar < ':' || e.KeyChar == '\b')
      return;
    e.Handled = true;
  }

  private void btnOk_Click(object sender, EventArgs e)
  {
    if (this.tbBugId.Text.Length == 0)
    {
      int num1 = (int) MessageBox.Show("Заполните поле \"Идентификатор ошибки из HelpDesk\"", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
    }
    else
    {
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        if (!(sessionKeeper.Session.GetCustomService(typeof (IHelpDeskService)) is IHelpDeskService customService))
        {
          int num2 = (int) MessageBox.Show("Невозможно подключится к сервису HelpDesk.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          this.DialogResult = DialogResult.Cancel;
        }
        Dictionary<bool, string> dictionary = customService.AuthenticationHelpDesk(this.tbLogin.Text, this.tbPassword.Text);
        if (!dictionary.Keys.First<bool>())
        {
          int num3 = (int) MessageBox.Show(dictionary[dictionary.Keys.First<bool>()], "Информация", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
        else if (!customService.ExistWorkOrder(Convert.ToInt64(this.tbBugId.Text)))
        {
          int num4 = (int) MessageBox.Show($"В Системе HelpDesk отсутствует ошибку с идентификатором = {this.tbBugId.Text}", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
        else
        {
          BugFromHelpDeskForm._userLogin = this.tbLogin.Text;
          BugFromHelpDeskForm._userPassword = this.tbPassword.Text;
          this.DialogResult = DialogResult.OK;
        }
      }
    }
  }

  private void tbLogin_TextChanged(object sender, EventArgs e)
  {
    if (!(this.tbLogin.BackColor == SystemColors.Info))
      return;
    this.tbLogin.BackColor = SystemColors.Window;
    this.tbPassword.BackColor = SystemColors.Window;
    BugFromHelpDeskForm.RemoveUserAuth();
  }

  private void tbPassword_TextChanged(object sender, EventArgs e)
  {
    if (!(this.tbPassword.BackColor == SystemColors.Info))
      return;
    this.tbLogin.BackColor = SystemColors.Window;
    this.tbPassword.BackColor = SystemColors.Window;
    BugFromHelpDeskForm.RemoveUserAuth();
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.btnOk = new Button();
    this.btnCancel = new Button();
    this.label1 = new Label();
    this.tbBugId = new TextBox();
    this.gbAuthentication = new GroupBox();
    this.lPassword = new Label();
    this.tbPassword = new TextBox();
    this.tbLogin = new TextBox();
    this.lLogin = new Label();
    this.gbAuthentication.SuspendLayout();
    this.SuspendLayout();
    this.btnOk.Location = new Point(115, 161);
    this.btnOk.Name = "btnOk";
    this.btnOk.Size = new Size(75, 23);
    this.btnOk.TabIndex = 2;
    this.btnOk.Text = "OK";
    this.btnOk.UseVisualStyleBackColor = true;
    this.btnOk.Click += new EventHandler(this.btnOk_Click);
    this.btnCancel.DialogResult = DialogResult.Cancel;
    this.btnCancel.Location = new Point(196, 161);
    this.btnCancel.Name = "btnCancel";
    this.btnCancel.Size = new Size(75, 23);
    this.btnCancel.TabIndex = 3;
    this.btnCancel.Text = "Отмена";
    this.btnCancel.UseVisualStyleBackColor = true;
    this.label1.AutoSize = true;
    this.label1.Location = new Point(4, 119);
    this.label1.Name = "label1";
    this.label1.Size = new Size(193, 13);
    this.label1.TabIndex = 4;
    this.label1.Text = "Идентификатор ошибки из HelpDesk";
    this.tbBugId.Location = new Point(7, 135);
    this.tbBugId.Name = "tbBugId";
    this.tbBugId.Size = new Size(264, 20);
    this.tbBugId.TabIndex = 1;
    this.tbBugId.KeyPress += new KeyPressEventHandler(this.tbBugId_KeyPress);
    this.gbAuthentication.Controls.Add((Control) this.lPassword);
    this.gbAuthentication.Controls.Add((Control) this.tbPassword);
    this.gbAuthentication.Controls.Add((Control) this.tbLogin);
    this.gbAuthentication.Controls.Add((Control) this.lLogin);
    this.gbAuthentication.Location = new Point(7, 3);
    this.gbAuthentication.Name = "gbAuthentication";
    this.gbAuthentication.Size = new Size(264, 113);
    this.gbAuthentication.TabIndex = 0;
    this.gbAuthentication.TabStop = false;
    this.gbAuthentication.Text = "Подключение к серверу HelpDesk";
    this.lPassword.AutoSize = true;
    this.lPassword.Location = new Point(6, 65);
    this.lPassword.Name = "lPassword";
    this.lPassword.Size = new Size(45, 13);
    this.lPassword.TabIndex = 0;
    this.lPassword.Text = "Пароль";
    this.tbPassword.Location = new Point(6, 81);
    this.tbPassword.Name = "tbPassword";
    this.tbPassword.Size = new Size(252, 20);
    this.tbPassword.TabIndex = 2;
    this.tbPassword.UseSystemPasswordChar = true;
    this.tbPassword.TextChanged += new EventHandler(this.tbPassword_TextChanged);
    this.tbLogin.BackColor = SystemColors.Window;
    this.tbLogin.Location = new Point(6, 42);
    this.tbLogin.Name = "tbLogin";
    this.tbLogin.Size = new Size(252, 20);
    this.tbLogin.TabIndex = 1;
    this.tbLogin.TextChanged += new EventHandler(this.tbLogin_TextChanged);
    this.lLogin.AutoSize = true;
    this.lLogin.Location = new Point(3, 26);
    this.lLogin.Name = "lLogin";
    this.lLogin.Size = new Size(103, 13);
    this.lLogin.TabIndex = 0;
    this.lLogin.Text = "Имя пользователя";
    this.AcceptButton = (IButtonControl) this.btnOk;
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.CancelButton = (IButtonControl) this.btnCancel;
    this.ClientSize = new Size(279, 192 /*0xC0*/);
    this.Controls.Add((Control) this.gbAuthentication);
    this.Controls.Add((Control) this.tbBugId);
    this.Controls.Add((Control) this.label1);
    this.Controls.Add((Control) this.btnCancel);
    this.Controls.Add((Control) this.btnOk);
    this.MaximizeBox = false;
    this.MinimizeBox = false;
    this.Name = nameof (BugFromHelpDeskForm);
    this.Text = "Ошибка с HelpDesk";
    this.gbAuthentication.ResumeLayout(false);
    this.gbAuthentication.PerformLayout();
    this.ResumeLayout(false);
    this.PerformLayout();
  }
}
