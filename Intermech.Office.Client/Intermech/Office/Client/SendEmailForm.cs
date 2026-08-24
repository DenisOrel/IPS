// Decompiled with JetBrains decompiler
// Type: Intermech.Office.Client.SendEmailForm
// Assembly: Intermech.Office.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 87EC380F-A344-4B99-B3CC-05ECB303FAD4
// Assembly location: D:\IPS\Client\Intermech.Office.Client.dll

using DevExpress.IM.XtraTreeList.Blending;
using ImSSP;
using Intermech.Client.Core;
using Intermech.Controls;
using Intermech.DataFormats;
using Intermech.Diagnostics;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Configuration;
using Intermech.Navigator;
using Intermech.Navigator.Interfaces;
using Intermech.Office.Client.Properties;
using Intermech.Office.Interfaces;
using Intermech.PropertyEditors;
using Intermech.Security;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Office.Client;

public class SendEmailForm : Form
{
  [CanBeNull]
  private string _filename;
  private long _documentID;
  private int _countFiles;
  private bool _registeredDoc;
  [NotNull]
  [ItemNotNull]
  private static List<string> _inputEmail = new List<string>();
  private const string SelectedAccount = "selectedAccount";
  private IContainer components;
  private MenuStrip menuStrip;
  private ToolStripMenuItem fileToolStripMenuItem;
  private ToolStripMenuItem ediToolStripMenuItem;
  private ToolStripMenuItem cutToolStripMenuItem;
  private ToolStripMenuItem copyToolStripMenuItem;
  private ToolStripMenuItem pasteToolStripMenuItem;
  private ToolStripSeparator toolStripMenuItem1;
  private ToolStripMenuItem selectAllToolStripMenuItem;
  private ToolStripSeparator toolStripMenuItem2;
  private ToolStripMenuItem findToolStripMenuItem;
  private TextEditor editor;
  private ToolStripMenuItem openToolStripMenuItem;
  private ToolStripMenuItem saveToolStripMenuItem;
  private ToolStripMenuItem saveAsToolStripMenuItem;
  private ToolStripMenuItem undoToolStripMenuItem;
  private ToolStripMenuItem redoToolStripMenuItem;
  private ToolStripSeparator toolStripMenuItem4;
  private Panel panel1;
  private Panel panel2;
  private Panel panel3;
  private Button bAddressee;
  private Label label2;
  private TextBox tbSubject;
  private Label label1;
  private Button bCopies;
  private Button bCancel;
  private Button bSend;
  private Button bDeleteAttachment;
  private Button bAddAttachment;
  private XtraTreeListBlending xtraTreeListBlending1;
  private ListView lvAttachments;
  private Label label3;
  private ComboBox cbAccounts;
  private Panel pFrom;
  private ComboBox tbAddressee;
  private ComboBox tbCopies;
  private TableLayoutPanel tableLayoutPanel1;

  public event OnSendClickEventHandler OnSendClickEvent;

  public SendEmailForm()
  {
    this.InitializeComponent();
    this.editor.Tick += new TextEditor.TickDelegate(this.editor_Tick);
    this.Text = Localization.GetString("Office.Client_66");
    FileAttributeStatics.InitImageList();
    this.lvAttachments.SmallImageList = FileAttributeStatics.imageList;
    Dictionary<string, string[]> dictionary = new Dictionary<string, string[]>(1);
    FormStorage.LoadLayout((Control) this, (IDictionary) dictionary);
    if (dictionary.Count <= 0)
      return;
    string[] strArray = dictionary["urls"];
    if (strArray.Length == 0)
      return;
    foreach (string str in strArray)
    {
      this.tbAddressee.Items.Add((object) str);
      this.tbCopies.Items.Add((object) str);
    }
  }

  public void Init(
    [NotNull] IUserSession session,
    [NotNull] IDBObject doc,
    bool registeredDoc,
    [CanBeNull] EmailAccaunt[] accounts)
  {
    this._documentID = doc.ObjectID;
    this._registeredDoc = registeredDoc;
    this.tbSubject.Text = doc.Caption;
    this.Text = $"{this.Text} - {doc.NameInMessages}";
    IDBAttribute attributeById1 = doc.GetAttributeByID(OfficeConsts.AttrFileID);
    if (attributeById1 != null && attributeById1.ValuesCount > 0)
    {
      for (int index = 0; index < attributeById1.ValuesCount; ++index)
      {
        attributeById1.Index = index;
        if (!attributeById1.IsNull)
        {
          ++this._countFiles;
          this.AddListItem(attributeById1.AsString, index);
        }
      }
    }
    IDBAttribute attributeById2 = doc.GetAttributeByID(OfficeConsts.AttrAddresseesID);
    if (attributeById2 != null && attributeById2.ValuesCount > 0)
    {
      for (int index = 0; index < attributeById2.ValuesCount; ++index)
      {
        attributeById2.Index = index;
        if (!SendEmailForm.AddAddress(session, attributeById2.AsInteger, this.tbAddressee))
          break;
      }
    }
    if (accounts == null || accounts.Length == 1)
    {
      this.pFrom.Visible = false;
    }
    else
    {
      Guid guid = Guid.Empty;
      if (Holder.ConfigurationManager != null)
      {
        IConfiguration configuration = Holder.ConfigurationManager.Open(this.GetType().Name);
        if (configuration != null)
        {
          string property = configuration.GetProperty("selectedAccount");
          if (property == string.Empty)
            property = configuration.GetProperty("selectedAccaunt");
          if (property != string.Empty && GuidHelper.IsGuid(property))
            guid = new Guid(property);
        }
      }
      int num = 0;
      for (int index = 0; index < accounts.Length; ++index)
      {
        this.cbAccounts.Items.Add((object) accounts[index]);
        if (guid == accounts[index].Guid)
          num = index;
      }
      this.cbAccounts.SelectedIndex = num;
    }
    this.RefreshAttachmentButtons();
  }

  private static bool AddAddress([NotNull] IUserSession session, long addresseeObjectID, [NotNull] ComboBox box)
  {
    IDBObject dbObject = session.GetObject(addresseeObjectID);
    IDBAttribute attributeById = dbObject.GetAttributeByID(OfficeConsts.AttrEmailAddressID);
    if (attributeById != null && EmailHelper.IsEmail(attributeById.AsString))
    {
      if (box.Text.IndexOf(attributeById.AsString, StringComparison.Ordinal) >= 0)
        return true;
      if (box.Text.Length > 0)
        box.Text += "; ";
      ComboBox comboBox = box;
      comboBox.Text = $"{comboBox.Text}{dbObject.Caption}<{attributeById.AsString}>";
      return true;
    }
    int num = (int) MessageBox.Show(string.Format(sc_15068.ssp_office_15069(), (object) dbObject.Caption), "Ошибка добавления получателя", MessageBoxButtons.OK, MessageBoxIcon.Hand);
    return false;
  }

  private void AddListItem([NotNull] string fileName, int index)
  {
    ListViewItem listViewItem = new ListViewItem(fileName);
    listViewItem.Tag = (object) index;
    string str = Path.GetExtension(fileName);
    listViewItem.ImageIndex = FileAttributeStatics.GetExtImageIndex(str.ToLower());
    this.lvAttachments.Items.Add(listViewItem);
  }

  private void editor_Tick()
  {
    this.undoToolStripMenuItem.Enabled = this.editor.CanUndo();
    this.redoToolStripMenuItem.Enabled = this.editor.CanRedo();
    this.cutToolStripMenuItem.Enabled = this.editor.CanCut();
    this.copyToolStripMenuItem.Enabled = this.editor.CanCopy();
    this.pasteToolStripMenuItem.Enabled = this.editor.CanPaste();
  }

  private void exitToolStripMenuItem_Click([CanBeNull] object sender, [NotNull] EventArgs e)
  {
    this.Close();
  }

  private void saveToolStripMenuItem_Click([CanBeNull] object sender, [NotNull] EventArgs e)
  {
    if (this._filename == null && !this.SaveFileDialog())
      return;
    this.SaveFile(Intermech.Diagnostics.Check.NotNull<string>(this._filename, "_filename"));
  }

  private bool SaveFileDialog()
  {
    using (System.Windows.Forms.SaveFileDialog saveFileDialog = new System.Windows.Forms.SaveFileDialog())
    {
      saveFileDialog.AddExtension = true;
      saveFileDialog.DefaultExt = "htm";
      saveFileDialog.Filter = "HTML files (*.html;*.htm)|*.html;*.htm";
      saveFileDialog.RestoreDirectory = true;
      if (saveFileDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
        return false;
      this._filename = saveFileDialog.FileName;
      return true;
    }
  }

  private void SaveFile([NotNull] string filename)
  {
    using (StreamWriter text = File.CreateText(filename))
    {
      text.Write(this.editor.DocumentText);
      text.Close();
    }
  }

  private void LoadFile([NotNull] string filename)
  {
    using (StreamReader streamReader = File.OpenText(filename))
    {
      this.editor.DocumentText = streamReader.ReadToEnd();
      streamReader.Close();
      this.Text = this.editor.DocumentTitle;
    }
  }

  private void openToolStripMenuItem_Click([CanBeNull] object sender, [NotNull] EventArgs e)
  {
    using (OpenFileDialog openFileDialog = new OpenFileDialog())
    {
      openFileDialog.Filter = "HTML files (*.html;*.htm)|*.html;*.htm";
      openFileDialog.RestoreDirectory = true;
      if (openFileDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
        return;
      this._filename = openFileDialog.FileName;
    }
    this.LoadFile(Intermech.Diagnostics.Check.NotNull<string>(this._filename, "_filename"));
  }

  private void saveAsToolStripMenuItem_Click([CanBeNull] object sender, [NotNull] EventArgs e)
  {
    if (!this.SaveFileDialog())
      return;
    this.SaveFile(Intermech.Diagnostics.Check.NotNull<string>(this._filename, "_filename"));
  }

  private void findToolStripMenuItem_Click([CanBeNull] object sender, [NotNull] EventArgs e)
  {
    using (SearchDialog searchDialog = new SearchDialog((ISearchableBrowser) this.editor))
    {
      int num = (int) searchDialog.ShowDialog((IWin32Window) this);
    }
  }

  private void selectAllToolStripMenuItem_Click([CanBeNull] object sender, [NotNull] EventArgs e)
  {
    this.editor.SelectAll();
  }

  private void cutToolStripMenuItem_Click([CanBeNull] object sender, [NotNull] EventArgs e)
  {
    this.editor.Cut();
  }

  private void copyToolStripMenuItem_Click([CanBeNull] object sender, [NotNull] EventArgs e)
  {
    this.editor.Copy();
  }

  private void pasteToolStripMenuItem_Click([CanBeNull] object sender, [NotNull] EventArgs e)
  {
    this.editor.Paste();
  }

  private void undoToolStripMenuItem_Click([CanBeNull] object sender, [NotNull] EventArgs e)
  {
    this.editor.Undo();
  }

  private void redoToolStripMenuItem_Click([CanBeNull] object sender, [NotNull] EventArgs e)
  {
    this.editor.Redo();
  }

  private void bAddAttachment_Click([CanBeNull] object sender, [NotNull] EventArgs e)
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBObject dbObject = sessionKeeper.Session.GetObject(this._documentID);
      IDBAttribute attributeById = dbObject.GetAttributeByID(OfficeConsts.AttrFileID);
      if (attributeById != null && attributeById.ValuesCount > 0)
      {
        List<string> captions = new List<string>(attributeById.ValuesCount);
        List<object> values = new List<object>(attributeById.ValuesCount);
        for (int index = 0; index < attributeById.ValuesCount; ++index)
        {
          attributeById.Index = index;
          if (!attributeById.IsNull)
          {
            captions.Add(attributeById.AsString);
            values.Add((object) index);
          }
        }
        ChoiceForm choiceForm = new ChoiceForm();
        choiceForm.Init(Localization.GetString("Office.Client_67"), captions, values);
        if (choiceForm.ShowDialog() == DialogResult.OK)
        {
          if (choiceForm.SelectedValue != null)
            this.AddListItem(choiceForm.SelectedCaption, (int) choiceForm.SelectedValue);
        }
      }
      else
      {
        int num = (int) IMMessageBox.Show(Localization.GetString(sc_15068.ssp_office_15070()), Localization.GetString("Office.Client_69", (object) dbObject.NameInMessages), MessageBoxButtons.OK, IMMessageBoxImage.Warning);
      }
    }
    this.RefreshAttachmentButtons();
  }

  private void bDeleteAttachment_Click([CanBeNull] object sender, [NotNull] EventArgs e)
  {
    if (this.lvAttachments.SelectedItems.Count == 1)
      this.lvAttachments.Items.Remove(this.lvAttachments.SelectedItems[0]);
    this.RefreshAttachmentButtons();
  }

  private void lvAttachments_SelectedIndexChanged([CanBeNull] object sender, [NotNull] EventArgs e)
  {
    this.RefreshAttachmentButtons();
  }

  private void GetNewAddress([NotNull] ComboBox box)
  {
    DescriptorCollection descriptors = new DescriptorCollection();
    descriptors.Add((IDescriptor) new Intermech.Navigator.DBObjectTypes.Descriptor(MetaDataHelper.GetObjectTypeID("cadd9235-306c-11d8-b4e9-00304f19f545")));
    if (!this._registeredDoc)
      descriptors.Add((IDescriptor) new UsersGroupsDescriptor());
    object[] source = SelectionWindow.Select(Localization.GetString("Office.Client_70"), (IDescriptor) new Intermech.Navigator.CustomNode.Descriptor(Localization.GetString("Office.Client_71"), descriptors), typeof (IDBTypedObjectID), SelectionOptions.SelectObjects);
    if (source == null || source.Length == 0)
      return;
    using (SessionKeeper keeper = new SessionKeeper())
    {
      using (IEnumerator<IDBTypedObjectID> enumerator = source.OfType<IDBTypedObjectID>().Where<IDBTypedObjectID>((Func<IDBTypedObjectID, bool>) (selObject => !SendEmailForm.AddAddress(keeper.Session, selObject.ObjectID, box))).GetEnumerator())
      {
        if (!enumerator.MoveNext())
          return;
        IDBTypedObjectID current = enumerator.Current;
      }
    }
  }

  private void bAddressee_Click([CanBeNull] object sender, [NotNull] EventArgs e)
  {
    this.GetNewAddress(this.tbAddressee);
  }

  private void bCopies_Click([CanBeNull] object sender, [NotNull] EventArgs e)
  {
    this.GetNewAddress(this.tbCopies);
  }

  private void bSend_Click([CanBeNull] object sender, [NotNull] EventArgs e)
  {
    if (this.Addresses == null)
    {
      int num = (int) IMMessageBox.Show(Localization.GetString(sc_15068.ssp_office_15071()), Localization.GetString("Office.Client_72"), MessageBoxButtons.OK, IMMessageBoxImage.Error);
    }
    else
    {
      if (this.tbSubject.Text == string.Empty && IMMessageBox.Show(Localization.GetString(sc_15068.ssp_office_15072()), Localization.GetString("Office.Client_74"), MessageBoxButtons.YesNo, IMMessageBoxImage.Question) == DialogResult.No)
        return;
      if (this.OnSendClickEvent != null)
      {
        StringBuilder stringBuilder = new StringBuilder();
        List<string> addresses = this.Addresses;
        for (int index = 0; index < addresses.Count; ++index)
        {
          if (index > 0)
            stringBuilder.Append(';');
          stringBuilder.Append(addresses[index]);
        }
        List<string> copies = this.Copies;
        if (copies != null)
        {
          foreach (string str in copies)
          {
            stringBuilder.Append(';');
            stringBuilder.Append(str);
          }
        }
        if (!this.OnSendClickEvent((object) this, new OnSendClickEventArgs(stringBuilder.ToString(), this.Subject, this.Message, (IEnumerable<int>) this.FileIndexes, this.AccountGuid)))
          return;
      }
      this.DialogResult = DialogResult.OK;
      this.Close();
    }
  }

  [CanBeNull]
  public List<string> ParseAddresses([NotNull] string str)
  {
    string[] source = str.Split(';');
    if (source.Length == 0)
      return (List<string>) null;
    List<string> stringList = new List<string>(source.Length);
    stringList.AddRange(((IEnumerable<string>) source).Select<string, string>(new Func<string, string>(EmailHelper.GetEmail)).Where<string>((Func<string, bool>) (email => email != string.Empty)));
    return stringList.Count != 0 ? stringList : (List<string>) null;
  }

  [CanBeNull]
  public List<string> Addresses => this.ParseAddresses(this.tbAddressee.Text);

  [CanBeNull]
  public List<string> Copies => this.ParseAddresses(this.tbCopies.Text);

  [CanBeNull]
  public List<int> FileIndexes
  {
    get
    {
      if (this.lvAttachments.Items.Count == 0)
        return (List<int>) null;
      List<int> fileIndexes = new List<int>(this.lvAttachments.Items.Count);
      for (int index = 0; index < this.lvAttachments.Items.Count; ++index)
        fileIndexes.Add((int) this.lvAttachments.Items[index].Tag);
      return fileIndexes;
    }
  }

  [NotNull]
  public string Subject => this.tbSubject.Text;

  [NotNull]
  public string Message => this.editor.DocumentText;

  public Guid AccountGuid
  {
    get
    {
      return this.cbAccounts.Items.Count <= 1 ? Guid.Empty : ((EmailAccaunt) this.cbAccounts.SelectedItem).Guid;
    }
  }

  private void RefreshAttachmentButtons()
  {
    this.bAddAttachment.Enabled = this.lvAttachments.Items.Count != this._countFiles;
    this.bDeleteAttachment.Enabled = this.lvAttachments.SelectedItems.Count == 1;
  }

  private void SendEmailForm_FormClosing([CanBeNull] object sender, [NotNull] FormClosingEventArgs e)
  {
    if (this.cbAccounts.Items.Count > 1 && Holder.ConfigurationManager != null)
      (Holder.ConfigurationManager.Open(this.GetType().Name) ?? Holder.ConfigurationManager.Create(this.GetType().Name)).SetProperty("selectedAccount", ((EmailAccaunt) this.cbAccounts.SelectedItem).Guid.ToString());
    List<string> urls = new List<string>();
    if (this.tbAddressee.Text != string.Empty)
      urls.Add(this.tbAddressee.Text);
    if (this.tbAddressee.Items.Count > 0)
    {
      foreach (string str in this.tbAddressee.Items.Cast<object>().Select<object, string>((Func<object, string>) (addressee => addressee.ToString())).Where<string>((Func<string, bool>) (addressee => addressee != string.Empty && !urls.Contains(addressee))))
        urls.Add(str);
    }
    if (this.tbCopies.Text != string.Empty && !urls.Contains(this.tbCopies.Text))
      urls.Add(this.tbCopies.Text);
    if (this.tbCopies.Items.Count > 0)
    {
      foreach (string str in this.tbAddressee.Items.Cast<object>().Select<object, string>((Func<object, string>) (addressee => addressee.ToString())).Where<string>((Func<string, bool>) (addressee => addressee != string.Empty && !urls.Contains(addressee))))
        urls.Add(str);
    }
    FormStorage.SaveLayout((Control) this, (IDictionary) new Dictionary<string, string[]>(1)
    {
      {
        "urls",
        urls.ToArray()
      }
    });
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (SendEmailForm));
    this.menuStrip = new MenuStrip();
    this.fileToolStripMenuItem = new ToolStripMenuItem();
    this.openToolStripMenuItem = new ToolStripMenuItem();
    this.saveToolStripMenuItem = new ToolStripMenuItem();
    this.saveAsToolStripMenuItem = new ToolStripMenuItem();
    this.ediToolStripMenuItem = new ToolStripMenuItem();
    this.undoToolStripMenuItem = new ToolStripMenuItem();
    this.redoToolStripMenuItem = new ToolStripMenuItem();
    this.toolStripMenuItem4 = new ToolStripSeparator();
    this.cutToolStripMenuItem = new ToolStripMenuItem();
    this.copyToolStripMenuItem = new ToolStripMenuItem();
    this.pasteToolStripMenuItem = new ToolStripMenuItem();
    this.toolStripMenuItem1 = new ToolStripSeparator();
    this.selectAllToolStripMenuItem = new ToolStripMenuItem();
    this.toolStripMenuItem2 = new ToolStripSeparator();
    this.findToolStripMenuItem = new ToolStripMenuItem();
    this.editor = new TextEditor();
    this.panel1 = new Panel();
    this.bCancel = new Button();
    this.bSend = new Button();
    this.panel2 = new Panel();
    this.panel3 = new Panel();
    this.tbCopies = new ComboBox();
    this.tbAddressee = new ComboBox();
    this.bAddAttachment = new Button();
    this.bDeleteAttachment = new Button();
    this.lvAttachments = new ListView();
    this.label2 = new Label();
    this.tbSubject = new TextBox();
    this.label1 = new Label();
    this.bCopies = new Button();
    this.bAddressee = new Button();
    this.pFrom = new Panel();
    this.label3 = new Label();
    this.cbAccounts = new ComboBox();
    this.xtraTreeListBlending1 = new XtraTreeListBlending();
    this.tableLayoutPanel1 = new TableLayoutPanel();
    this.menuStrip.SuspendLayout();
    this.panel1.SuspendLayout();
    this.panel2.SuspendLayout();
    this.panel3.SuspendLayout();
    this.pFrom.SuspendLayout();
    this.tableLayoutPanel1.SuspendLayout();
    this.SuspendLayout();
    this.menuStrip.Items.AddRange(new ToolStripItem[2]
    {
      (ToolStripItem) this.fileToolStripMenuItem,
      (ToolStripItem) this.ediToolStripMenuItem
    });
    componentResourceManager.ApplyResources((object) this.menuStrip, "menuStrip");
    this.menuStrip.Name = "menuStrip";
    this.fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[3]
    {
      (ToolStripItem) this.openToolStripMenuItem,
      (ToolStripItem) this.saveToolStripMenuItem,
      (ToolStripItem) this.saveAsToolStripMenuItem
    });
    this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
    componentResourceManager.ApplyResources((object) this.fileToolStripMenuItem, "fileToolStripMenuItem");
    this.openToolStripMenuItem.Image = (Image) Resources.Open;
    this.openToolStripMenuItem.Name = "openToolStripMenuItem";
    componentResourceManager.ApplyResources((object) this.openToolStripMenuItem, "openToolStripMenuItem");
    this.openToolStripMenuItem.Click += new EventHandler(this.openToolStripMenuItem_Click);
    this.saveToolStripMenuItem.Image = (Image) Resources.Save;
    this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
    componentResourceManager.ApplyResources((object) this.saveToolStripMenuItem, "saveToolStripMenuItem");
    this.saveToolStripMenuItem.Click += new EventHandler(this.saveToolStripMenuItem_Click);
    this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
    componentResourceManager.ApplyResources((object) this.saveAsToolStripMenuItem, "saveAsToolStripMenuItem");
    this.saveAsToolStripMenuItem.Click += new EventHandler(this.saveAsToolStripMenuItem_Click);
    this.ediToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[10]
    {
      (ToolStripItem) this.undoToolStripMenuItem,
      (ToolStripItem) this.redoToolStripMenuItem,
      (ToolStripItem) this.toolStripMenuItem4,
      (ToolStripItem) this.cutToolStripMenuItem,
      (ToolStripItem) this.copyToolStripMenuItem,
      (ToolStripItem) this.pasteToolStripMenuItem,
      (ToolStripItem) this.toolStripMenuItem1,
      (ToolStripItem) this.selectAllToolStripMenuItem,
      (ToolStripItem) this.toolStripMenuItem2,
      (ToolStripItem) this.findToolStripMenuItem
    });
    this.ediToolStripMenuItem.Name = "ediToolStripMenuItem";
    componentResourceManager.ApplyResources((object) this.ediToolStripMenuItem, "ediToolStripMenuItem");
    this.undoToolStripMenuItem.Image = (Image) Resources.Undo;
    this.undoToolStripMenuItem.Name = "undoToolStripMenuItem";
    componentResourceManager.ApplyResources((object) this.undoToolStripMenuItem, "undoToolStripMenuItem");
    this.undoToolStripMenuItem.Click += new EventHandler(this.undoToolStripMenuItem_Click);
    this.redoToolStripMenuItem.Image = (Image) Resources.Redo;
    this.redoToolStripMenuItem.Name = "redoToolStripMenuItem";
    componentResourceManager.ApplyResources((object) this.redoToolStripMenuItem, "redoToolStripMenuItem");
    this.redoToolStripMenuItem.Click += new EventHandler(this.redoToolStripMenuItem_Click);
    this.toolStripMenuItem4.Name = "toolStripMenuItem4";
    componentResourceManager.ApplyResources((object) this.toolStripMenuItem4, "toolStripMenuItem4");
    this.cutToolStripMenuItem.Image = (Image) Resources.Cut;
    this.cutToolStripMenuItem.Name = "cutToolStripMenuItem";
    componentResourceManager.ApplyResources((object) this.cutToolStripMenuItem, "cutToolStripMenuItem");
    this.cutToolStripMenuItem.Click += new EventHandler(this.cutToolStripMenuItem_Click);
    this.copyToolStripMenuItem.Image = (Image) Resources.Copy;
    this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
    componentResourceManager.ApplyResources((object) this.copyToolStripMenuItem, "copyToolStripMenuItem");
    this.copyToolStripMenuItem.Click += new EventHandler(this.copyToolStripMenuItem_Click);
    this.pasteToolStripMenuItem.Image = (Image) Resources.Paste;
    this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
    componentResourceManager.ApplyResources((object) this.pasteToolStripMenuItem, "pasteToolStripMenuItem");
    this.pasteToolStripMenuItem.Click += new EventHandler(this.pasteToolStripMenuItem_Click);
    this.toolStripMenuItem1.Name = "toolStripMenuItem1";
    componentResourceManager.ApplyResources((object) this.toolStripMenuItem1, "toolStripMenuItem1");
    this.selectAllToolStripMenuItem.Name = "selectAllToolStripMenuItem";
    componentResourceManager.ApplyResources((object) this.selectAllToolStripMenuItem, "selectAllToolStripMenuItem");
    this.selectAllToolStripMenuItem.Click += new EventHandler(this.selectAllToolStripMenuItem_Click);
    this.toolStripMenuItem2.Name = "toolStripMenuItem2";
    componentResourceManager.ApplyResources((object) this.toolStripMenuItem2, "toolStripMenuItem2");
    this.findToolStripMenuItem.Image = (Image) Resources.Search;
    this.findToolStripMenuItem.Name = "findToolStripMenuItem";
    componentResourceManager.ApplyResources((object) this.findToolStripMenuItem, "findToolStripMenuItem");
    this.findToolStripMenuItem.Click += new EventHandler(this.findToolStripMenuItem_Click);
    this.editor.BackColor = SystemColors.Control;
    this.editor.BodyBackgroundColor = Color.White;
    this.editor.BodyHtml = (string) null;
    this.editor.BodyText = (string) null;
    componentResourceManager.ApplyResources((object) this.editor, "editor");
    this.editor.DocumentText = componentResourceManager.GetString("editor.DocumentText");
    this.editor.EditorBackColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
    this.editor.EditorForeColor = Color.FromArgb(0, 0, 0);
    this.editor.FontSize = FontSizes.Three;
    this.editor.Html = (string) null;
    this.editor.Name = "editor";
    this.panel1.Controls.Add((Control) this.bCancel);
    this.panel1.Controls.Add((Control) this.bSend);
    componentResourceManager.ApplyResources((object) this.panel1, "panel1");
    this.panel1.Name = "panel1";
    componentResourceManager.ApplyResources((object) this.bCancel, "bCancel");
    this.bCancel.DialogResult = DialogResult.Cancel;
    this.bCancel.Name = "bCancel";
    this.bCancel.UseVisualStyleBackColor = true;
    componentResourceManager.ApplyResources((object) this.bSend, "bSend");
    this.bSend.Name = "bSend";
    this.bSend.UseVisualStyleBackColor = true;
    this.bSend.Click += new EventHandler(this.bSend_Click);
    componentResourceManager.ApplyResources((object) this.panel2, "panel2");
    this.panel2.Controls.Add((Control) this.editor);
    this.panel2.Name = "panel2";
    this.panel3.Controls.Add((Control) this.tbCopies);
    this.panel3.Controls.Add((Control) this.tbAddressee);
    this.panel3.Controls.Add((Control) this.bAddAttachment);
    this.panel3.Controls.Add((Control) this.bDeleteAttachment);
    this.panel3.Controls.Add((Control) this.lvAttachments);
    this.panel3.Controls.Add((Control) this.label2);
    this.panel3.Controls.Add((Control) this.tbSubject);
    this.panel3.Controls.Add((Control) this.label1);
    this.panel3.Controls.Add((Control) this.bCopies);
    this.panel3.Controls.Add((Control) this.bAddressee);
    componentResourceManager.ApplyResources((object) this.panel3, "panel3");
    this.panel3.Name = "panel3";
    componentResourceManager.ApplyResources((object) this.tbCopies, "tbCopies");
    this.tbCopies.FormattingEnabled = true;
    this.tbCopies.Name = "tbCopies";
    componentResourceManager.ApplyResources((object) this.tbAddressee, "tbAddressee");
    this.tbAddressee.FormattingEnabled = true;
    this.tbAddressee.Name = "tbAddressee";
    componentResourceManager.ApplyResources((object) this.bAddAttachment, "bAddAttachment");
    this.bAddAttachment.Image = (Image) Resources.Plus;
    this.bAddAttachment.Name = "bAddAttachment";
    this.bAddAttachment.UseVisualStyleBackColor = true;
    this.bAddAttachment.Click += new EventHandler(this.bAddAttachment_Click);
    componentResourceManager.ApplyResources((object) this.bDeleteAttachment, "bDeleteAttachment");
    this.bDeleteAttachment.Image = (Image) Resources.Delete;
    this.bDeleteAttachment.Name = "bDeleteAttachment";
    this.bDeleteAttachment.UseVisualStyleBackColor = true;
    this.bDeleteAttachment.Click += new EventHandler(this.bDeleteAttachment_Click);
    componentResourceManager.ApplyResources((object) this.lvAttachments, "lvAttachments");
    this.lvAttachments.HideSelection = false;
    this.lvAttachments.MultiSelect = false;
    this.lvAttachments.Name = "lvAttachments";
    this.lvAttachments.UseCompatibleStateImageBehavior = false;
    this.lvAttachments.View = View.SmallIcon;
    this.lvAttachments.SelectedIndexChanged += new EventHandler(this.lvAttachments_SelectedIndexChanged);
    componentResourceManager.ApplyResources((object) this.label2, "label2");
    this.label2.Name = "label2";
    componentResourceManager.ApplyResources((object) this.tbSubject, "tbSubject");
    this.tbSubject.Name = "tbSubject";
    componentResourceManager.ApplyResources((object) this.label1, "label1");
    this.label1.Name = "label1";
    componentResourceManager.ApplyResources((object) this.bCopies, "bCopies");
    this.bCopies.Name = "bCopies";
    this.bCopies.UseVisualStyleBackColor = true;
    this.bCopies.Click += new EventHandler(this.bCopies_Click);
    componentResourceManager.ApplyResources((object) this.bAddressee, "bAddressee");
    this.bAddressee.Name = "bAddressee";
    this.bAddressee.UseVisualStyleBackColor = true;
    this.bAddressee.Click += new EventHandler(this.bAddressee_Click);
    componentResourceManager.ApplyResources((object) this.pFrom, "pFrom");
    this.pFrom.Controls.Add((Control) this.label3);
    this.pFrom.Controls.Add((Control) this.cbAccounts);
    this.pFrom.Name = "pFrom";
    componentResourceManager.ApplyResources((object) this.label3, "label3");
    this.label3.Name = "label3";
    this.cbAccounts.DropDownStyle = ComboBoxStyle.DropDownList;
    this.cbAccounts.FormattingEnabled = true;
    componentResourceManager.ApplyResources((object) this.cbAccounts, "cbAccounts");
    this.cbAccounts.Name = "cbAccounts";
    componentResourceManager.ApplyResources((object) this.tableLayoutPanel1, "tableLayoutPanel1");
    this.tableLayoutPanel1.Controls.Add((Control) this.pFrom, 0, 0);
    this.tableLayoutPanel1.Controls.Add((Control) this.panel2, 0, 2);
    this.tableLayoutPanel1.Controls.Add((Control) this.panel3, 0, 1);
    this.tableLayoutPanel1.Name = "tableLayoutPanel1";
    componentResourceManager.ApplyResources((object) this, "$this");
    this.AutoScaleMode = AutoScaleMode.Font;
    this.BackColor = SystemColors.Control;
    this.CancelButton = (IButtonControl) this.bCancel;
    this.Controls.Add((Control) this.tableLayoutPanel1);
    this.Controls.Add((Control) this.panel1);
    this.Controls.Add((Control) this.menuStrip);
    this.MainMenuStrip = this.menuStrip;
    this.Name = nameof (SendEmailForm);
    this.FormClosing += new FormClosingEventHandler(this.SendEmailForm_FormClosing);
    this.menuStrip.ResumeLayout(false);
    this.menuStrip.PerformLayout();
    this.panel1.ResumeLayout(false);
    this.panel2.ResumeLayout(false);
    this.panel3.ResumeLayout(false);
    this.panel3.PerformLayout();
    this.pFrom.ResumeLayout(false);
    this.pFrom.PerformLayout();
    this.tableLayoutPanel1.ResumeLayout(false);
    this.ResumeLayout(false);
    this.PerformLayout();
  }
}
