// Decompiled with JetBrains decompiler
// Type: Intermech.Search.Pdm.CompositionCopying.CompositionCopyingWizardForm
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Client.Core;
using Intermech.Interfaces.Client;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Search.Pdm.CompositionCopying;

public sealed class CompositionCopyingWizardForm : Form
{
  private const string CompositionCopyingWizardControlStateKey = "CompositionCopyingWizardControlMemento";
  private long _objectVersionID;
  private IContainer components;
  private CompositionCopyingWizardControl _compositionCopyingWizardControl;

  public CompositionCopyingWizardForm() => this.InitializeComponent();

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public long ObjectVersionID
  {
    get => this._objectVersionID;
    set
    {
      if (this._objectVersionID == value)
        return;
      this._objectVersionID = value;
      this._compositionCopyingWizardControl.ObjectVersionID = this._objectVersionID;
    }
  }

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public int[] AllowableForCreateCopyObjectTypes
  {
    get => this._compositionCopyingWizardControl.AllowableForCreateCopyObjectTypes;
    set => this._compositionCopyingWizardControl.AllowableForCreateCopyObjectTypes = value;
  }

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public int[] RelationTypes
  {
    get => this._compositionCopyingWizardControl.RelationTypes;
    set => this._compositionCopyingWizardControl.RelationTypes = value;
  }

  public void Initialize(
    ICurrentUserAndRole currentUserAndRole,
    IFiltrationService filtrationService,
    INavigatorClientService navigatorClientService)
  {
    if (navigatorClientService == null)
      throw new ArgumentNullException(nameof (navigatorClientService));
    this._compositionCopyingWizardControl.Initialize(currentUserAndRole, filtrationService, navigatorClientService);
  }

  private void CompositionCopyingWizardForm_FormClosed(object sender, FormClosedEventArgs e)
  {
    FormStorage.SaveLayout((Control) this, (IDictionary) new Hashtable()
    {
      {
        (object) "CompositionCopyingWizardControlMemento",
        (object) this.SerializeCompositionCopyingWizardControlState(this._compositionCopyingWizardControl.CreateMemento())
      }
    });
  }

  private void CompositionCopyingWizardForm_Load(object sender, EventArgs e)
  {
    Hashtable hashtable = new Hashtable();
    FormStorage.LoadLayout((Control) this, (IDictionary) hashtable);
    if (!hashtable.ContainsKey((object) "CompositionCopyingWizardControlMemento"))
      return;
    string stateAsString = hashtable[(object) "CompositionCopyingWizardControlMemento"] as string;
    if (string.IsNullOrEmpty(stateAsString))
      return;
    try
    {
      object memento = this.DeserializeCompositionCopyingWizardControlState(stateAsString);
      if (memento == null)
        return;
      this._compositionCopyingWizardControl.SetMemento(memento);
    }
    catch
    {
    }
  }

  private void CompositionCopyingWizardControl_CancelButtonClicked(object sender, EventArgs e)
  {
    this.Close();
  }

  private string SerializeCompositionCopyingWizardControlState(object state)
  {
    using (MemoryStream serializationStream = new MemoryStream())
    {
      new BinaryFormatter().Serialize((Stream) serializationStream, state);
      return Convert.ToBase64String(serializationStream.GetBuffer());
    }
  }

  private object DeserializeCompositionCopyingWizardControlState(string stateAsString)
  {
    using (MemoryStream serializationStream = new MemoryStream(Convert.FromBase64String(stateAsString)))
      return new BinaryFormatter().Deserialize((Stream) serializationStream);
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this._compositionCopyingWizardControl = new CompositionCopyingWizardControl();
    this.SuspendLayout();
    this._compositionCopyingWizardControl.Dock = DockStyle.Fill;
    this._compositionCopyingWizardControl.Location = new Point(0, 0);
    this._compositionCopyingWizardControl.Name = "_compositionCopyingWizardControl";
    this._compositionCopyingWizardControl.Size = new Size(800, 450);
    this._compositionCopyingWizardControl.TabIndex = 0;
    this._compositionCopyingWizardControl.CancelButtonClicked += new EventHandler(this.CompositionCopyingWizardControl_CancelButtonClicked);
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.ClientSize = new Size(800, 450);
    this.Controls.Add((Control) this._compositionCopyingWizardControl);
    this.Name = nameof (CompositionCopyingWizardForm);
    this.ShowIcon = false;
    this.Text = "Состав по прототипу";
    this.FormClosed += new FormClosedEventHandler(this.CompositionCopyingWizardForm_FormClosed);
    this.Load += new EventHandler(this.CompositionCopyingWizardForm_Load);
    this.ResumeLayout(false);
  }
}
