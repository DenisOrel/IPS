// Decompiled with JetBrains decompiler
// Type: Intermech.Search.MSOfficeReviews.MSOfficeReviewsClientService
// Assembly: Intermech.Search.MSOfficeReviews.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 4AB1E446-C278-4B7C-8A5E-DB94EF37D83B
// Assembly location: D:\IPS\Client\Intermech.Search.MSOfficeReviews.Client.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Navigator;
using Intermech.Navigator.ContextCommands;
using Intermech.Navigator.DBObjects;
using Intermech.Navigator.Interfaces;
using Intermech.Search.UI;
using Intermech.Search.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Search.MSOfficeReviews;

public sealed class MSOfficeReviewsClientService : IMSOfficeReviewsClientService
{
  public void EditReview(long documentVersionID)
  {
    long num = !ObjectHelper.IsUnknownObjectVersionID(documentVersionID) ? this.FindOwnReviewForDocument(documentVersionID) : throw new ArgumentException();
    if (ObjectHelper.IsUnknownObjectVersionID(num))
      num = this.CreateReview(documentVersionID);
    else if (!this.IsActualReview(num, documentVersionID) && !this.DontShowOldReviewNotification(num))
    {
      using (LabelWithCheckBoxForm withCheckBoxForm = new LabelWithCheckBoxForm())
      {
        withCheckBoxForm.LabelText = "Внимание. Рецензия устарела, желаете заменить ее содержимым документа?";
        withCheckBoxForm.CheckBoxText = "Больше не сообщать для данной рецензии";
        if (withCheckBoxForm.ShowDialog() == DialogResult.Yes)
          this.ReplaceReviewByDocument(num, documentVersionID);
        if (withCheckBoxForm.Checked)
          this.SetDontShowOldReviewNotification(num);
      }
    }
    this.OpenReviewEditor(num);
  }

  public void ShowAllReviews(long documentVersionID)
  {
    long[] reviewVersionIds = !ObjectHelper.IsUnknownObjectVersionID(documentVersionID) ? this.FindAllActualReviewsForDocument(documentVersionID) : throw new ArgumentException();
    if (reviewVersionIds.Length == 0)
      this.ShowActaulReviewsNotFoundMessageBox();
    else if (reviewVersionIds.Length == 1)
    {
      this.ShowReview(reviewVersionIds[0]);
    }
    else
    {
      long num = MSOfficeReviewsClientHelper.MergeReviews(reviewVersionIds);
      try
      {
        this.ShowReview(num);
      }
      finally
      {
        this.DeleteObject(num);
      }
    }
  }

  public void ShowOwnReview(long documentVersionID)
  {
    long num = !ObjectHelper.IsUnknownObjectVersionID(documentVersionID) ? this.FindOwnReviewForDocument(documentVersionID) : throw new ArgumentException();
    if (ObjectHelper.IsUnknownObjectVersionID(num))
      this.ShowReviewsNotFoundMessageBox();
    else
      this.ShowReview(num);
  }

  public void SelectAndShowReview(long documentVersionID)
  {
    long[] reviewVewsionIds = !ObjectHelper.IsUnknownObjectVersionID(documentVersionID) ? this.FindAllReviewsForDocument(documentVersionID) : throw new ArgumentException();
    if (reviewVewsionIds.Length == 0)
    {
      this.ShowReviewsNotFoundMessageBox();
    }
    else
    {
      long[] numArray = this.SelectReviews(reviewVewsionIds, "Выберите рецензию для просмотра");
      if (numArray == null || numArray.Length == 0)
        return;
      this.ShowReview(numArray[0]);
    }
  }

  public void ReplaceDocumentByReview(long documentVersionID)
  {
    long[] reviewVersionIds = !ObjectHelper.IsUnknownObjectVersionID(documentVersionID) ? this.FindAllActualReviewsForDocument(documentVersionID) : throw new ArgumentException();
    if (reviewVersionIds.Length == 0)
    {
      this.ShowActaulReviewsNotFoundMessageBox();
    }
    else
    {
      bool isMergedReview = false;
      long mergedReview = this.SelectReviewOrCreateMergedReview(reviewVersionIds, out isMergedReview);
      if (ObjectHelper.IsUnknownObjectVersionID(mergedReview))
        return;
      try
      {
        this.ReplaceDocumentByReview(documentVersionID, mergedReview);
      }
      finally
      {
        if (isMergedReview)
          this.DeleteObject(mergedReview);
      }
    }
  }

  public void CreateDocumentFromReview(long documentVersionID)
  {
    long[] reviewVersionIds = !ObjectHelper.IsUnknownObjectVersionID(documentVersionID) ? this.FindAllActualReviewsForDocument(documentVersionID) : throw new ArgumentException();
    if (reviewVersionIds.Length == 0)
    {
      this.ShowActaulReviewsNotFoundMessageBox();
    }
    else
    {
      bool isMergedReview = false;
      long mergedReview = this.SelectReviewOrCreateMergedReview(reviewVersionIds, out isMergedReview);
      if (ObjectHelper.IsUnknownObjectVersionID(mergedReview))
        return;
      try
      {
        long objectVersion = this.CreateObjectVersion(documentVersionID);
        this.ReplaceDocumentByReview(objectVersion, mergedReview);
        ServiceLocator.Get<INotificationService>()?.FireEvent((object) this, (NotificationEventArgs) new DBObjectsEventArgs("ObjectsCreated", objectVersion));
      }
      finally
      {
        if (isMergedReview)
          this.DeleteObject(mergedReview);
      }
    }
  }

  public void DeleteReview(long documentVersionID)
  {
    if (ObjectHelper.IsUnknownObjectVersionID(documentVersionID))
      throw new ArgumentException();
    if (MessageBox.Show("Выполнение команды приведет к удалению вашей рецензии для данного документа. Желаете продолжить?", "Intermech Professional Solution", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
      return;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      using (NotificationContext.Create(sessionKeeper.Session))
        (sessionKeeper.Session.GetCustomService(typeof (IMSOfficeReviewsServerService)) as IMSOfficeReviewsServerService).RemoveOwnReviewForDocument(sessionKeeper.Session.SessionGUID, documentVersionID);
    }
  }

  public void DeleteAllReviews(long documentVersionID)
  {
    if (ObjectHelper.IsUnknownObjectVersionID(documentVersionID))
      throw new ArgumentException();
    if (MessageBox.Show("Выполнение команды приведет к удалению всех рецензий для данного документа. Желаете продолжить?", "Intermech Professional Solution", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
      return;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      using (NotificationContext.Create(sessionKeeper.Session))
        (sessionKeeper.Session.GetCustomService(typeof (IMSOfficeReviewsServerService)) as IMSOfficeReviewsServerService).RemoveAllReviewsForDocument(sessionKeeper.Session.SessionGUID, documentVersionID);
    }
  }

  public void SaveReview(long documentVersionID)
  {
    long num = !ObjectHelper.IsUnknownObjectVersionID(documentVersionID) ? this.FindOwnReviewForDocument(documentVersionID) : throw new ArgumentException();
    if (ObjectHelper.IsUnknownObjectVersionID(num))
      return;
    ObjectCommands.SaveChangesCommand(SelectedItemsHelper.CreateSelectedItemsForObject(num), (IServiceProvider) ServicesManager.ServiceContainer, (object) null);
  }

  private long FindOwnReviewForDocument(long documentVersionID)
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
      return (sessionKeeper.Session.GetCustomService(typeof (IMSOfficeReviewsServerService)) as IMSOfficeReviewsServerService).FindOwnReviewForDocument(sessionKeeper.Session.SessionGUID, documentVersionID);
  }

  private long CreateReview(long documentVersionID)
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      using (NotificationContext.Create(sessionKeeper.Session))
        return (sessionKeeper.Session.GetCustomService(typeof (IMSOfficeReviewsServerService)) as IMSOfficeReviewsServerService).CreateReviewForDocument(sessionKeeper.Session.SessionGUID, documentVersionID);
    }
  }

  private bool IsActualReview(long reviewVersionID, long documentVersionID)
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
      return (sessionKeeper.Session.GetCustomService(typeof (IMSOfficeReviewsServerService)) as IMSOfficeReviewsServerService).IsActualReview(sessionKeeper.Session.SessionGUID, reviewVersionID, documentVersionID);
  }

  private bool DontShowOldReviewNotification(long reviewVersionID)
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
      return (sessionKeeper.Session.GetCustomService(typeof (IMSOfficeReviewsServerService)) as IMSOfficeReviewsServerService).DontShowOldReviewNotification(sessionKeeper.Session.SessionGUID, reviewVersionID);
  }

  private void SetDontShowOldReviewNotification(long reviewVersionID)
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      using (NotificationContext.Create(sessionKeeper.Session))
        (sessionKeeper.Session.GetCustomService(typeof (IMSOfficeReviewsServerService)) as IMSOfficeReviewsServerService).SetDontShowOldReviewNotification(sessionKeeper.Session.SessionGUID, reviewVersionID);
    }
  }

  private void ReplaceReviewByDocument(long reviewVersionID, long documentVersionID)
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      using (NotificationContext.Create(sessionKeeper.Session))
        (sessionKeeper.Session.GetCustomService(typeof (IMSOfficeReviewsServerService)) as IMSOfficeReviewsServerService).ReplaceReviewByDocument(sessionKeeper.Session.SessionGUID, reviewVersionID, documentVersionID);
    }
  }

  private void OpenReviewEditor(long reviewVersionID)
  {
    ObjectCommands.EditCommand(SelectedItemsHelper.CreateSelectedItemsForObject(reviewVersionID), (IServiceProvider) ServicesManager.ServiceContainer, (object) null);
  }

  private long[] FindAllActualReviewsForDocument(long documentVersionID)
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
      return (sessionKeeper.Session.GetCustomService(typeof (IMSOfficeReviewsServerService)) as IMSOfficeReviewsServerService).FindAllActualReviewsForDocument(sessionKeeper.Session.SessionGUID, documentVersionID);
  }

  private void ShowActaulReviewsNotFoundMessageBox()
  {
    int num = (int) MessageBox.Show("Невозможно выполнить команду. Для данного документа не найдено ни одной актуальной рецензии.", "Intermech Professional Solution", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
  }

  private void ShowReview(long reviewVersionID)
  {
    ObjectCommands.ViewCommand(SelectedItemsHelper.CreateSelectedItemsForObject(reviewVersionID), (IServiceProvider) ServicesManager.ServiceContainer, (object) null);
  }

  private void ShowReviewsNotFoundMessageBox()
  {
    int num = (int) MessageBox.Show("Для данного документа рецензий пока cоздано не было.", "Intermech Professional Solution", MessageBoxButtons.OK);
  }

  private long[] FindAllReviewsForDocument(long documentVersionID)
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
      return (sessionKeeper.Session.GetCustomService(typeof (IMSOfficeReviewsServerService)) as IMSOfficeReviewsServerService).FindAllReviewsForDocument(sessionKeeper.Session.SessionGUID, documentVersionID);
  }

  private long[] SelectReviews(long[] reviewVewsionIds, string text, bool multiselect = false)
  {
    SelectionOptions options = SelectionOptions.SelectObjects;
    if (!multiselect)
      options |= SelectionOptions.DisableMultiselect;
    return SelectionWindow.SelectObjects("Intermech Professional Solution", text, (IDescriptor) new ListDescriptor(Intermech.Navigator.Consts.CategoryMultipleObjectsNode, -1, "Рецензии", (IList) ((IEnumerable<long>) reviewVewsionIds).ToList<long>()), options);
  }

  private long SelectReviewOrCreateMergedReview(long[] reviewVersionIds, out bool isMergedReview)
  {
    isMergedReview = false;
    long mergedReview = 0;
    if (reviewVersionIds.Length == 1)
    {
      mergedReview = reviewVersionIds[0];
    }
    else
    {
      long[] reviewVersionIds1 = this.SelectReviews(reviewVersionIds, "Выберите рецензии для замены документа. Выбранные рецензии будут объединены и документ будет заменен объединенной рецензией.", true);
      if (reviewVersionIds1 != null && reviewVersionIds1.Length != 0)
      {
        if (reviewVersionIds1.Length == 1)
        {
          mergedReview = reviewVersionIds1[0];
        }
        else
        {
          mergedReview = MSOfficeReviewsClientHelper.MergeReviews(reviewVersionIds1);
          isMergedReview = true;
        }
      }
    }
    return mergedReview;
  }

  private void ReplaceDocumentByReview(long documentVersionID, long reviewVersionID)
  {
    ObjectCommands.SaveChangesCommand(SelectedItemsHelper.CreateSelectedItemsForObject(reviewVersionID), (IServiceProvider) ServicesManager.ServiceContainer, (object) null);
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      using (NotificationContext.Create(sessionKeeper.Session))
        (sessionKeeper.Session.GetCustomService(typeof (IMSOfficeReviewsServerService)) as IMSOfficeReviewsServerService).ReplaceDocumentByReview(sessionKeeper.Session.SessionGUID, documentVersionID, reviewVersionID);
    }
  }

  private void DeleteObject(long objectVersionID)
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      using (NotificationContext.Create(sessionKeeper.Session))
        sessionKeeper.Session.GetObject(objectVersionID).Delete((long) Intermech.Consts.PurgeMode);
    }
  }

  private long CreateObjectVersion(long objectVersionID)
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBObject dbObject = sessionKeeper.Session.GetObject(objectVersionID);
      IDBObject version = sessionKeeper.Session.GetObjectCollection(dbObject.ObjectType).CreateVersion(objectVersionID);
      version.CommitCreation(true, true);
      return version.ObjectID;
    }
  }
}
