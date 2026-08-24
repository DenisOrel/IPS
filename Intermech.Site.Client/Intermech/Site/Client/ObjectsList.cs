// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.ObjectsList
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Interfaces.WebPortal;
using Intermech.Navigator.Controls;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Site.Client;

public class ObjectsList : Form
{
  private IContainer components;
  private ObjectsListControl objectsListControl1;
  private Button bCancel;

  public ObjectsList() => this.InitializeComponent();

  public static DialogResult ShowDialog(List<PublishCompositionObject> objects)
  {
    PublishCompositionDescriptor rootDescriptor = new PublishCompositionDescriptor(objects);
    ServiceContainer services = new ServiceContainer();
    services.AddService(typeof (IViewState), (object) new ViewStateService(ViewStateFlags.ReadOnly));
    using (ObjectsList objectsList = new ObjectsList())
    {
      objectsList.objectsListControl1.Initialize((IDescriptor) rootDescriptor, (IServiceProvider) services);
      objectsList.objectsListControl1.SetColumns(PublishCompositionNode.DefaultColumns, false);
      objectsList.objectsListControl1.Activate((IView) null);
      return objectsList.ShowDialog();
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
    this.objectsListControl1 = new ObjectsListControl();
    this.bCancel = new Button();
    this.SuspendLayout();
    this.objectsListControl1.AllowCustomGroupValues = true;
    this.objectsListControl1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
    this.objectsListControl1.Control = (object) this.objectsListControl1;
    this.objectsListControl1.DataLoaded = false;
    this.objectsListControl1.DisableContextSearch = true;
    this.objectsListControl1.DisableFiltration = true;
    this.objectsListControl1.DisableKeyDownEvents = false;
    this.objectsListControl1.EmbeddedFocusAndSelection = (iFocusAndSelection) null;
    this.objectsListControl1.Font = new Font("Tahoma", 8.25f);
    this.objectsListControl1.Location = new Point(12, 12);
    this.objectsListControl1.Name = "objectsListControl1";
    this.objectsListControl1.Size = new Size(592, 319);
    this.objectsListControl1.TabIndex = 0;
    this.bCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.bCancel.DialogResult = DialogResult.Cancel;
    this.bCancel.Location = new Point(482, 339);
    this.bCancel.Name = "bCancel";
    this.bCancel.Size = new Size(121, 27);
    this.bCancel.TabIndex = 2;
    this.bCancel.Text = "Закрыть";
    this.bCancel.UseVisualStyleBackColor = true;
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.ClientSize = new Size(615, 378);
    this.Controls.Add((Control) this.bCancel);
    this.Controls.Add((Control) this.objectsListControl1);
    this.Name = nameof (ObjectsList);
    this.StartPosition = FormStartPosition.CenterParent;
    this.Text = "Список публикуемых объектов";
    this.ResumeLayout(false);
  }
}
