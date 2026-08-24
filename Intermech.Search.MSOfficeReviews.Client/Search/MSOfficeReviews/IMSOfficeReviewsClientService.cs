// Decompiled with JetBrains decompiler
// Type: Intermech.Search.MSOfficeReviews.IMSOfficeReviewsClientService
// Assembly: Intermech.Search.MSOfficeReviews.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 4AB1E446-C278-4B7C-8A5E-DB94EF37D83B
// Assembly location: D:\IPS\Client\Intermech.Search.MSOfficeReviews.Client.dll

#nullable disable
namespace Intermech.Search.MSOfficeReviews;

public interface IMSOfficeReviewsClientService
{
  void EditReview(long documentVersionID);

  void ShowAllReviews(long documentVersionID);

  void ShowOwnReview(long documentVersionID);

  void SelectAndShowReview(long documentVersionID);

  void ReplaceDocumentByReview(long documentVersionID);

  void CreateDocumentFromReview(long documentVersionID);

  void DeleteReview(long documentVersionID);

  void DeleteAllReviews(long documentVersionID);

  void SaveReview(long documentVersionID);
}
