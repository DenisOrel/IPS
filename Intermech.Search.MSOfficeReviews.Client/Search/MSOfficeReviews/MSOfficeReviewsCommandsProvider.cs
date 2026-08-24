// Decompiled with JetBrains decompiler
// Type: Intermech.Search.MSOfficeReviews.MSOfficeReviewsCommandsProvider
// Assembly: Intermech.Search.MSOfficeReviews.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 4AB1E446-C278-4B7C-8A5E-DB94EF37D83B
// Assembly location: D:\IPS\Client\Intermech.Search.MSOfficeReviews.Client.dll

using Intermech.DataFormats;
using Intermech.Interfaces.Client;
using Intermech.Navigator.ContextMenu;
using Intermech.Navigator.Interfaces;
using Intermech.Search.Utilities;
using System;

#nullable disable
namespace Intermech.Search.MSOfficeReviews;

public sealed class MSOfficeReviewsCommandsProvider : ICommandsProvider
{
  public CommandsInfo GetMergedCommands(ISelectedItems items, IServiceProvider viewServices)
  {
    return CommandsInfo.Empty;
  }

  public CommandsInfo GetGroupCommands(ISelectedItems items, IServiceProvider viewServices)
  {
    if (items == null)
      throw new ArgumentNullException(nameof (items));
    if (viewServices == null)
      throw new ArgumentNullException(nameof (viewServices));
    CommandsInfo groupCommands = new CommandsInfo();
    IDBTypedObjectID typedObjectID = (IDBTypedObjectID) null;
    if (this.CheckParamsForEditReview(items, viewServices, out typedObjectID))
      groupCommands.Add("Review.Edit", new CommandInfo(0, new ClickEventHandler(this.EditReview)));
    if (this.CheckParamsForShowAllReviews(items, viewServices, out typedObjectID))
      groupCommands.Add("Review.Show.All", new CommandInfo(0, new ClickEventHandler(this.ShowAllReviews)));
    if (this.CheckParamsForShowOwnReview(items, viewServices, out typedObjectID))
      groupCommands.Add("Review.Show.Own", new CommandInfo(0, new ClickEventHandler(this.ShowOwnReview)));
    if (this.CheckParamsForSelectAndShowReview(items, viewServices, out typedObjectID))
      groupCommands.Add("Review.Show.Select", new CommandInfo(0, new ClickEventHandler(this.SelectAndShowReview)));
    if (this.CheckParamsForReplaceDocumentByReview(items, viewServices, out typedObjectID))
      groupCommands.Add("Review.ReplaceDocumentByReview", new CommandInfo(0, new ClickEventHandler(this.ReplaceDocumentByReview)));
    if (this.CheckParamsForCreateDocumentFromReview(items, viewServices, out typedObjectID))
      groupCommands.Add("Review.CreateDocumentFromReview", new CommandInfo(0, new ClickEventHandler(this.CreateDocumentFromReview)));
    if (this.CheckParamsForDeleteReview(items, viewServices, out typedObjectID))
      groupCommands.Add("Review.Delete", new CommandInfo(0, new ClickEventHandler(this.DeleteReview)));
    if (this.CheckParamsForDeleteAllReviews(items, viewServices, out typedObjectID))
      groupCommands.Add("Review.DeleteAll", new CommandInfo(0, new ClickEventHandler(this.DeleteAllReviews)));
    if (this.CheckParamsForSaveReview(items, viewServices, out typedObjectID))
      groupCommands.Add("Review.Save", new CommandInfo(0, new ClickEventHandler(this.SaveReview)));
    return groupCommands;
  }

  private bool CheckParamsForEditReview(
    ISelectedItems selectedItems,
    IServiceProvider serviceProvider,
    out IDBTypedObjectID typedObjectID)
  {
    return this.TryGetSingleTypedObjectIDWithObjectVersionIDAndObjectTypeIDAndSupportsReview(selectedItems, out typedObjectID);
  }

  private bool TryGetSingleTypedObjectIDWithObjectVersionIDAndObjectTypeIDAndSupportsReview(
    ISelectedItems selectedItems,
    out IDBTypedObjectID typedObjectID)
  {
    return SelectedItemsHelper.TryGetSingleTypedObjectIDWithObjectVersionIDAndObjectTypeID(selectedItems, out typedObjectID) && MSOfficeReviewsHelper.IsObjectTypeSupportsReview(typedObjectID.ObjectType);
  }

  private void EditReview(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    if (items == null)
      throw new ArgumentNullException(nameof (items));
    if (viewServices == null)
      throw new ArgumentNullException(nameof (viewServices));
    IDBTypedObjectID typedObjectID = (IDBTypedObjectID) null;
    if (!this.CheckParamsForEditReview(items, viewServices, out typedObjectID))
      throw new ArgumentException();
    ServiceLocator.Get<IMSOfficeReviewsClientService>().EditReview(typedObjectID.ObjectID);
  }

  private bool CheckParamsForShowAllReviews(
    ISelectedItems selectedItems,
    IServiceProvider serviceProvider,
    out IDBTypedObjectID typedObjectID)
  {
    return this.TryGetSingleTypedObjectIDWithObjectVersionIDAndObjectTypeIDAndSupportsReview(selectedItems, out typedObjectID);
  }

  private void ShowAllReviews(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    if (items == null)
      throw new ArgumentNullException(nameof (items));
    if (viewServices == null)
      throw new ArgumentNullException(nameof (viewServices));
    IDBTypedObjectID typedObjectID = (IDBTypedObjectID) null;
    if (!this.CheckParamsForShowAllReviews(items, viewServices, out typedObjectID))
      throw new ArgumentException();
    ServiceLocator.Get<IMSOfficeReviewsClientService>().ShowAllReviews(typedObjectID.ObjectID);
  }

  private bool CheckParamsForShowOwnReview(
    ISelectedItems selectedItems,
    IServiceProvider serviceProvider,
    out IDBTypedObjectID typedObjectID)
  {
    return this.TryGetSingleTypedObjectIDWithObjectVersionIDAndObjectTypeIDAndSupportsReview(selectedItems, out typedObjectID);
  }

  private void ShowOwnReview(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    if (items == null)
      throw new ArgumentNullException(nameof (items));
    if (viewServices == null)
      throw new ArgumentNullException(nameof (viewServices));
    IDBTypedObjectID typedObjectID = (IDBTypedObjectID) null;
    if (!this.CheckParamsForShowOwnReview(items, viewServices, out typedObjectID))
      throw new ArgumentException();
    ServiceLocator.Get<IMSOfficeReviewsClientService>().ShowOwnReview(typedObjectID.ObjectID);
  }

  private bool CheckParamsForSelectAndShowReview(
    ISelectedItems selectedItems,
    IServiceProvider serviceProvider,
    out IDBTypedObjectID typedObjectID)
  {
    return this.TryGetSingleTypedObjectIDWithObjectVersionIDAndObjectTypeIDAndSupportsReview(selectedItems, out typedObjectID);
  }

  private void SelectAndShowReview(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    if (items == null)
      throw new ArgumentNullException(nameof (items));
    if (viewServices == null)
      throw new ArgumentNullException(nameof (viewServices));
    IDBTypedObjectID typedObjectID = (IDBTypedObjectID) null;
    if (!this.CheckParamsForSelectAndShowReview(items, viewServices, out typedObjectID))
      throw new ArgumentException();
    ServiceLocator.Get<IMSOfficeReviewsClientService>().SelectAndShowReview(typedObjectID.ObjectID);
  }

  private bool CheckParamsForReplaceDocumentByReview(
    ISelectedItems selectedItems,
    IServiceProvider serviceProvider,
    out IDBTypedObjectID typedObjectID)
  {
    return this.TryGetSingleTypedObjectIDWithObjectVersionIDAndObjectTypeIDAndSupportsReview(selectedItems, out typedObjectID);
  }

  private void ReplaceDocumentByReview(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    if (items == null)
      throw new ArgumentNullException(nameof (items));
    if (viewServices == null)
      throw new ArgumentNullException(nameof (viewServices));
    IDBTypedObjectID typedObjectID = (IDBTypedObjectID) null;
    if (!this.CheckParamsForReplaceDocumentByReview(items, viewServices, out typedObjectID))
      throw new ArgumentException();
    ServiceLocator.Get<IMSOfficeReviewsClientService>().ReplaceDocumentByReview(typedObjectID.ObjectID);
  }

  private bool CheckParamsForCreateDocumentFromReview(
    ISelectedItems selectedItems,
    IServiceProvider serviceProvider,
    out IDBTypedObjectID typedObjectID)
  {
    return this.TryGetSingleTypedObjectIDWithObjectVersionIDAndObjectTypeIDAndSupportsReview(selectedItems, out typedObjectID) && ObjectTypeHelper.IsVersionedObjectTypeID(typedObjectID.ObjectType);
  }

  private void CreateDocumentFromReview(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    if (items == null)
      throw new ArgumentNullException(nameof (items));
    if (viewServices == null)
      throw new ArgumentNullException(nameof (viewServices));
    IDBTypedObjectID typedObjectID = (IDBTypedObjectID) null;
    if (!this.CheckParamsForCreateDocumentFromReview(items, viewServices, out typedObjectID))
      throw new ArgumentException();
    ServiceLocator.Get<IMSOfficeReviewsClientService>().CreateDocumentFromReview(typedObjectID.ObjectID);
  }

  private bool CheckParamsForDeleteReview(
    ISelectedItems selectedItems,
    IServiceProvider serviceProvider,
    out IDBTypedObjectID typedObjectID)
  {
    return this.TryGetSingleTypedObjectIDWithObjectVersionIDAndObjectTypeIDAndSupportsReview(selectedItems, out typedObjectID);
  }

  private void DeleteReview(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    if (items == null)
      throw new ArgumentNullException(nameof (items));
    if (viewServices == null)
      throw new ArgumentNullException(nameof (viewServices));
    IDBTypedObjectID typedObjectID = (IDBTypedObjectID) null;
    if (!this.CheckParamsForDeleteReview(items, viewServices, out typedObjectID))
      throw new ArgumentException();
    ServiceLocator.Get<IMSOfficeReviewsClientService>().DeleteReview(typedObjectID.ObjectID);
  }

  private bool CheckParamsForDeleteAllReviews(
    ISelectedItems selectedItems,
    IServiceProvider serviceProvider,
    out IDBTypedObjectID typedObjectID)
  {
    if (this.TryGetSingleTypedObjectIDWithObjectVersionIDAndObjectTypeIDAndSupportsReview(selectedItems, out typedObjectID) && ServiceLocator.Get<ICurrentUserAndRole>().UserID == typedObjectID.Owner)
      return true;
    typedObjectID = (IDBTypedObjectID) null;
    return false;
  }

  private void DeleteAllReviews(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    if (items == null)
      throw new ArgumentNullException(nameof (items));
    if (viewServices == null)
      throw new ArgumentNullException(nameof (viewServices));
    IDBTypedObjectID typedObjectID = (IDBTypedObjectID) null;
    if (!this.CheckParamsForDeleteAllReviews(items, viewServices, out typedObjectID))
      throw new ArgumentException();
    ServiceLocator.Get<IMSOfficeReviewsClientService>().DeleteAllReviews(typedObjectID.ObjectID);
  }

  private bool CheckParamsForSaveReview(
    ISelectedItems selectedItems,
    IServiceProvider serviceProvider,
    out IDBTypedObjectID typedObjectID)
  {
    return this.TryGetSingleTypedObjectIDWithObjectVersionIDAndObjectTypeIDAndSupportsReview(selectedItems, out typedObjectID);
  }

  private void SaveReview(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    if (items == null)
      throw new ArgumentNullException(nameof (items));
    if (viewServices == null)
      throw new ArgumentNullException(nameof (viewServices));
    IDBTypedObjectID typedObjectID = (IDBTypedObjectID) null;
    if (!this.CheckParamsForSaveReview(items, viewServices, out typedObjectID))
      throw new ArgumentException();
    ServiceLocator.Get<IMSOfficeReviewsClientService>().SaveReview(typedObjectID.ObjectID);
  }
}
