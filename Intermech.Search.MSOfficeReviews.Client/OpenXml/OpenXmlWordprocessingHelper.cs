// Decompiled with JetBrains decompiler
// Type: Intermech.OpenXml.OpenXmlWordprocessingHelper
// Assembly: Intermech.Search.MSOfficeReviews.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 4AB1E446-C278-4B7C-8A5E-DB94EF37D83B
// Assembly location: D:\IPS\Client\Intermech.Search.MSOfficeReviews.Client.dll

using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Intermech.OpenXml;

public static class OpenXmlWordprocessingHelper
{
  public static void MergeComments(
    WordprocessingDocument sourceDocument,
    WordprocessingDocument destinationDocument)
  {
    if (sourceDocument == null)
      throw new ArgumentNullException(nameof (sourceDocument));
    if (destinationDocument == null)
      throw new ArgumentNullException(nameof (destinationDocument));
    try
    {
      if (sourceDocument.MainDocumentPart.WordprocessingCommentsPart == null || sourceDocument.MainDocumentPart.WordprocessingCommentsPart.Comments == null || sourceDocument.MainDocumentPart.WordprocessingCommentsPart.Comments.Descendants<Comment>().Count<Comment>() <= 0)
        return;
      if (destinationDocument.MainDocumentPart.WordprocessingCommentsPart == null)
        destinationDocument.MainDocumentPart.AddNewPart<WordprocessingCommentsPart>().Comments = new Comments();
      foreach (Comment comment in (OpenXmlElement) sourceDocument.MainDocumentPart.WordprocessingCommentsPart.Comments)
      {
        Comment sourceComment = comment;
        Comment newChild = (Comment) sourceComment.CloneNode(true);
        newChild.Id = (StringValue) OpenXmlWordprocessingHelper.GetNextCommentID(destinationDocument);
        destinationDocument.MainDocumentPart.WordprocessingCommentsPart.Comments.AppendChild<Comment>(newChild);
        destinationDocument.MainDocumentPart.WordprocessingCommentsPart.Comments.Save();
        CommentRangeStart commentRangeStart1 = sourceDocument.MainDocumentPart.Document.Descendants<CommentRangeStart>().FirstOrDefault<CommentRangeStart>((Func<CommentRangeStart, bool>) (o => o.Id.Value == sourceComment.Id.Value));
        WordprocessingDocument sourceDocument1 = sourceDocument;
        WordprocessingDocument destinationDocument1 = destinationDocument;
        CommentRangeStart sourceElement1 = commentRangeStart1;
        OpenXmlElement[] destinationElements1 = new OpenXmlElement[1];
        CommentRangeStart commentRangeStart2 = new CommentRangeStart();
        commentRangeStart2.Id = newChild.Id;
        destinationElements1[0] = (OpenXmlElement) commentRangeStart2;
        OpenXmlWordprocessingHelper.InsertElementsFromOneDocumentToOtherDocument(sourceDocument1, destinationDocument1, (OpenXmlElement) sourceElement1, destinationElements1);
        CommentRangeEnd commentRangeEnd1 = sourceDocument.MainDocumentPart.Document.Descendants<CommentRangeEnd>().FirstOrDefault<CommentRangeEnd>((Func<CommentRangeEnd, bool>) (o => o.Id.Value == sourceComment.Id.Value));
        WordprocessingDocument sourceDocument2 = sourceDocument;
        WordprocessingDocument destinationDocument2 = destinationDocument;
        CommentRangeEnd sourceElement2 = commentRangeEnd1;
        OpenXmlElement[] destinationElements2 = new OpenXmlElement[2];
        CommentReference commentReference = new CommentReference();
        commentReference.Id = newChild.Id;
        destinationElements2[0] = (OpenXmlElement) commentReference;
        CommentRangeEnd commentRangeEnd2 = new CommentRangeEnd();
        commentRangeEnd2.Id = newChild.Id;
        destinationElements2[1] = (OpenXmlElement) commentRangeEnd2;
        OpenXmlWordprocessingHelper.InsertElementsFromOneDocumentToOtherDocument(sourceDocument2, destinationDocument2, (OpenXmlElement) sourceElement2, destinationElements2);
      }
    }
    catch (Exception ex)
    {
      throw new Exception("Во время объдинения комментариев документов произошла ошибка, возможно документы отличаются друг от друга не только комментариями.", ex);
    }
  }

  private static string GetNextCommentID(WordprocessingDocument document)
  {
    string nextCommentId = "0";
    if (document.MainDocumentPart.WordprocessingCommentsPart != null && document.MainDocumentPart.WordprocessingCommentsPart.Comments != null)
      nextCommentId = (document.MainDocumentPart.WordprocessingCommentsPart.Comments.Descendants<Comment>().Select<Comment, int>((Func<Comment, int>) (e => Convert.ToInt32(e.Id.Value))).Max() + 1).ToString();
    return nextCommentId;
  }

  private static void InsertElementsFromOneDocumentToOtherDocument(
    WordprocessingDocument sourceDocument,
    WordprocessingDocument destinationDocument,
    OpenXmlElement sourceElement,
    OpenXmlElement[] destinationElements)
  {
    if (sourceElement.Parent is Paragraph)
    {
      int element = OpenXmlWordprocessingHelper.CountSymbolsFromStartParagraphToElement(sourceElement);
      OpenXmlWordprocessingHelper.InsertElementsAtTextPosition(OpenXmlWordprocessingHelper.GetParagraphForElementFromOtherDocument(sourceElement, destinationDocument), element, destinationElements);
    }
    else
    {
      if (!(sourceElement.Parent is Body))
        return;
      int index = Array.IndexOf<OpenXmlElement>(sourceDocument.MainDocumentPart.Document.Body.ChildElements.ToArray<OpenXmlElement>(), sourceElement);
      foreach (OpenXmlElement newChild in ((IEnumerable<OpenXmlElement>) destinationElements).Reverse<OpenXmlElement>())
        destinationDocument.MainDocumentPart.Document.Body.InsertAt<OpenXmlElement>(newChild, index);
    }
  }

  private static Paragraph GetParagraphForElementFromOtherDocument(
    OpenXmlElement openXmlElement,
    WordprocessingDocument wordprocessingDocument)
  {
    Tuple<Type, int>[] openXmlElementPath = OpenXmlWordprocessingHelper.GetOpenXmlElementPath(openXmlElement.Parent);
    return OpenXmlWordprocessingHelper.GetOpenXmlElementByPath(wordprocessingDocument, openXmlElementPath) as Paragraph;
  }

  private static Tuple<Type, int>[] GetOpenXmlElementPath(OpenXmlElement openXmlElement)
  {
    List<Tuple<Type, int>> source = new List<Tuple<Type, int>>();
    for (; openXmlElement != null; openXmlElement = openXmlElement.Parent)
    {
      if (openXmlElement.Parent != null)
      {
        int num = Array.IndexOf<OpenXmlElement>(openXmlElement.Parent.Descendants().Where<OpenXmlElement>((Func<OpenXmlElement, bool>) (o => o.GetType() == openXmlElement.GetType())).ToArray<OpenXmlElement>(), openXmlElement);
        Tuple<Type, int> tuple = new Tuple<Type, int>(openXmlElement.GetType(), num);
        source.Add(tuple);
      }
    }
    return source.Reverse<Tuple<Type, int>>().ToArray<Tuple<Type, int>>();
  }

  private static OpenXmlElement GetOpenXmlElementByPath(
    WordprocessingDocument wordprocessingDocument,
    Tuple<Type, int>[] path)
  {
    OpenXmlElement xmlElementByPath = (OpenXmlElement) wordprocessingDocument.MainDocumentPart.Document;
    foreach (Tuple<Type, int> tuple in path)
    {
      Tuple<Type, int> pathPart = tuple;
      if (xmlElementByPath != null)
        xmlElementByPath = xmlElementByPath.Descendants().Where<OpenXmlElement>((Func<OpenXmlElement, bool>) (o => o.GetType() == pathPart.Item1)).ElementAtOrDefault<OpenXmlElement>(pathPart.Item2);
      else
        break;
    }
    return xmlElementByPath;
  }

  private static int CountSymbolsFromStartParagraphToElement(OpenXmlElement openXmlElement)
  {
    int element = 0;
    foreach (OpenXmlElement openXmlElement1 in openXmlElement.Parent)
    {
      if (openXmlElement1 != openXmlElement)
      {
        if (openXmlElement1 is Run)
        {
          string innerText = openXmlElement1.InnerText;
          element += innerText != null ? innerText.Length : 0;
        }
      }
      else
        break;
    }
    return element;
  }

  private static void InsertElementsAtTextPosition(
    Paragraph paragraph,
    int position,
    OpenXmlElement[] openXmlElements)
  {
    int num = 0;
    foreach (OpenXmlElement openXmlElement1 in (OpenXmlElement) paragraph)
    {
      if (openXmlElement1 is Run)
      {
        Run run = (Run) openXmlElement1;
        string innerText = run.InnerText;
        int length = innerText != null ? innerText.Length : 0;
        if (num == position)
        {
          foreach (OpenXmlElement openXmlElement2 in openXmlElements)
            run.InsertBeforeSelf<OpenXmlElement>(openXmlElement2);
          break;
        }
        if (num + length == position)
        {
          foreach (OpenXmlElement openXmlElement3 in openXmlElements)
            run.InsertAfterSelf<OpenXmlElement>(openXmlElement3);
          break;
        }
        if (num + length > position)
        {
          OpenXmlElement[] openXmlElementArray1 = new OpenXmlElement[1];
          Text text1 = new Text(innerText.Substring(0, position - num));
          text1.Space = new EnumValue<SpaceProcessingModeValues>(SpaceProcessingModeValues.Preserve);
          openXmlElementArray1[0] = (OpenXmlElement) text1;
          Run newElement1 = new Run(openXmlElementArray1);
          OpenXmlElement[] openXmlElementArray2 = new OpenXmlElement[1];
          Text text2 = new Text(innerText.Substring(position - num));
          text2.Space = new EnumValue<SpaceProcessingModeValues>(SpaceProcessingModeValues.Preserve);
          openXmlElementArray2[0] = (OpenXmlElement) text2;
          Run newElement2 = new Run(openXmlElementArray2);
          run.InsertAfterSelf<Run>(newElement2);
          run.InsertAfterSelf<Run>(newElement1);
          run.Remove();
          foreach (OpenXmlElement openXmlElement4 in openXmlElements)
            newElement1.InsertAfterSelf<OpenXmlElement>(openXmlElement4);
          break;
        }
        num += length;
      }
    }
  }
}
