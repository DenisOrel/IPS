// Decompiled with JetBrains decompiler
// Type: Intermech.Office.Client.OfficeSupervisorsListControl
// Assembly: Intermech.Office.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 87EC380F-A344-4B99-B3CC-05ECB303FAD4
// Assembly location: D:\IPS\Client\Intermech.Office.Client.dll

using Intermech.Common;
using Intermech.Diagnostics;
using Intermech.Extensions;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Navigator.ContextMenu;
using Intermech.Navigator.ContextMenu.Extensions;
using Intermech.Navigator.Interfaces;
using Intermech.Office.Interfaces;
using Intermech.Windows.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Office.Client;

public class OfficeSupervisorsListControl : 
  ObjectsListUserControl,
  IComponent,
  IDisposable,
  IDropTarget,
  ISynchronizeInvoke,
  IWin32Window,
  IBindableComponent,
  IContainerControl,
  IContextAware,
  ISupportSaveLocks,
  INamedContext,
  ICanBeReadOnly,
  ICanBeReadOnly2,
  IIODestination,
  ICommandsProvider,
  ILocalCommandsProvider,
  ICommandsFilter,
  IPropertyPage,
  IPropertyPageSearchOptionEvents
{
  [CanBeNull]
  private readonly long[] _supervisorsObjVerIDs;
  private bool _skipResize;
  private int _oldWidth;
  private IContainer components;

  public OfficeSupervisorsListControl() => this.InitializeComponent();

  public OfficeSupervisorsListControl([NotNull] IUserSession session)
    : this()
  {
    this._supervisorsObjVerIDs = session.GetCustomService<IOfficeGeneralSettingsService>().SupervisorObjVerIDs;
    if (this._supervisorsObjVerIDs.Length <= 1)
      return;
    this._supervisorsObjVerIDs = ((IEnumerable<long>) this._supervisorsObjVerIDs).Where<long>((Func<long, bool>) (objVerId => !Session.GetObjectInfo(objVerId).Empty)).ToArray<long>(this._supervisorsObjVerIDs.Length);
  }

  protected override void FireFirstPaint()
  {
    this.Init((IServiceProvider) null, (IReadOnlyCollection<int>) new int[3]
    {
      OfficeConsts.ObjtypeUsersID,
      OfficeConsts.ObjtypeGroupsID,
      OfficeConsts.ObjtypeRoleID
    }, "Пользователи, группы пользователей, роли", (IReadOnlyCollection<long>) Intermech.Diagnostics.Check.NotNull<long[]>(this._supervisorsObjVerIDs, "_supervisorsObjVerIDs"), (IReadOnlyCollection<long>) new long[1]
    {
      OfficeConsts.ObjectAdminRoleID
    });
    base.FireFirstPaint();
    this._objectsView.DisableGroupBox = true;
  }

  public event EventHandler Changed;

  protected override void FireListChanged(ObjectsListUserControl.ChangeType changeType)
  {
    base.FireListChanged(changeType);
    if (changeType != ObjectsListUserControl.ChangeType.FirstChange || this.Changed == null)
      return;
    this.Changed((object) this, EventArgs.Empty);
  }

  public PropertyPageType Type => PropertyPageType.Control;

  [NotNull]
  public object Control => (object) this;

  [NotNull]
  public string PageName => OfficeClientConsts.OfficeSupervisorsPageName;

  public void Apply()
  {
    if (!this.WasInit)
      return;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IOfficeGeneralSettingsService customService = sessionKeeper.Session.GetCustomService<IOfficeGeneralSettingsService>();
      long[] array = ((IEnumerable<long>) customService.SupervisorObjVerIDs).Concat<long>((IEnumerable<long>) this.AddedObjectVerIDs).Except<long>((IEnumerable<long>) this.RemovedObjectVerIDs).Abs().Distinct<long>().ToArray<long>();
      customService.WriteSupervisorsList(sessionKeeper.Session.SessionGUID, array);
    }
    this.ResetChangesInternal();
  }

  public void Cancel()
  {
    if (!this.WasInit)
      return;
    this.ResetChangesInternal();
  }

  protected override void ResetChangesInternal()
  {
    long[] supervisorObjVerIds;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
      supervisorObjVerIds = sessionKeeper.Session.GetCustomService<IOfficeGeneralSettingsService>().SupervisorObjVerIDs;
    this.ObjectVerIDs = (IReadOnlyCollection<long>) supervisorObjVerIds;
  }

  [CanBeNull]
  public string HelpTopicID => (string) null;

  [NotNull]
  public string HeaderText => string.Empty;

  public List<string> GetOptionNames()
  {
    return !(this.Control is System.Windows.Forms.Control control) ? new List<string>() : IPropertyPageHelper.GetOptionNames(control);
  }

  private void _labelDescription_Resize([CanBeNull] object sender, [NotNull] EventArgs e)
  {
    if (this._skipResize || this.Width == this._oldWidth)
      return;
    this._oldWidth = this.Width;
    this._skipResize = true;
    try
    {
      this._labelDescription.MaximumSize = new Size(this._panelTreeCaption.Width - 36, 0);
      this._labelDescription.AutoSize = true;
      this._labelDescription.AutoSize = false;
      this._labelDescription.MaximumSize = new Size(0, 0);
    }
    finally
    {
      this._skipResize = false;
    }
  }

  private void _objectsView_Enter([CanBeNull] object sender, [NotNull] EventArgs e)
  {
    this._objectsView.DisableGroupBox = true;
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this._panelTreeCaption.SuspendLayout();
    this._panelButtons.SuspendLayout();
    this.SuspendLayout();
    this._panelTreeCaption.Size = new Size(661, 47);
    this._labelDescription.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
    this._labelDescription.AutoSize = false;
    this._labelDescription.Size = new Size(565, 39);
    this._labelDescription.Text = "Выбранные супервизоры канцелярии - роли, группы пользователей или пользователи которые будут видеть поручения всех других пользователей, смогут выпускать по ним отчёты и производить прочие действия:";
    this._labelDescription.Resize += new EventHandler(this._labelDescription_Resize);
    this._panelButtons.Location = new Point(577, 47);
    this._panelButtons.Size = new Size(84, 533);
    this._objectsView.AllowCustomGroupValues = false;
    this._objectsView.DisableCheckedOutColumn = true;
    this._objectsView.DisableDoubleClicks = true;
    this._objectsView.Location = new Point(0, 47);
    this._objectsView.Size = new Size(577, 533);
    this._objectsView.Enter += new EventHandler(this._objectsView_Enter);
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.CustomAddDialogCaption = "Добавить суперзоров канцелярии";
    this.CustomAddDialogDescription = "Выберите роли, группы пользователей или пользователей которые будут видеть поручения всех других пользоватлей, смогут выпускать по ним отчёты и производить прочие действия";
    this.CustomDeleteConfirmationCaption = "Удаление супервизоров";
    this.CustomDeleteConfirmationQuestionOneObject = "Удалить выбранного супервизора?";
    this.CustomDeleteConfirmationQuestionSomeObjects = "Удалить выбранных супервизоров (кол-во {0})?";
    this.CustomResetConfirmationQuestion = "Отменить все изменения в списке супервизоров?";
    this.Description = "Выбранные супервизоры канцелярии - роли, группы пользователей или пользователи которые будут видеть поручения всех других пользователей, смогут выпускать по ним отчёты и производить прочие действия:";
    this.Name = nameof (OfficeSupervisorsListControl);
    this.ContextName = "OfficeSupervisors";
    this._panelTreeCaption.ResumeLayout(false);
    this._panelButtons.ResumeLayout(false);
    this.ResumeLayout(false);
    this.PerformLayout();
  }
}
