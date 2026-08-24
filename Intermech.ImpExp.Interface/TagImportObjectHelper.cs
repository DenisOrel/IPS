// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.TagImportObjectHelper
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using System.IO;
using System.Text;

#nullable disable
namespace Intermech.ImpExp.Interface;

/// <summary>Функции кэша</summary>
public class TagImportObjectHelper
{
  /// <summary>
  /// По уникальнову идентификатору возвращает объект,
  /// передаваемого в качестве параметра tag функциям IImportingData
  /// </summary>
  /// <param name="classID">Уникальный идентификатор класса</param>
  public static ITagImportObject GetImportObject(int classID)
  {
    switch (classID)
    {
      case 1:
        return (ITagImportObject) new DocTypesSettings();
      case 2:
        return (ITagImportObject) new ImbaseGroup();
      case 3:
        return (ITagImportObject) new ImbaseTableAttributes();
      case 4:
        return (ITagImportObject) new ImbaseGroupAttributes();
      case 5:
        return (ITagImportObject) new AttributeType();
      case 6:
        return (ITagImportObject) new Archive();
      case 7:
        return (ITagImportObject) new ObjectType();
      case 8:
        return (ITagImportObject) new ArticleTag();
      case 9:
        return (ITagImportObject) new DocumentTag();
      case 10:
        return (ITagImportObject) new CompositionTag();
      case 11:
        return (ITagImportObject) new TechDiffTag();
      case 13:
        return (ITagImportObject) new TechObjectTag();
      case 14:
        return (ITagImportObject) new LCSteps4Archives();
      case 15:
        return (ITagImportObject) new ObjectInfo();
      case 16 /*0x10*/:
        return (ITagImportObject) new VCompositionTag();
      case 17:
        return (ITagImportObject) new SignTag();
      case 18:
        return (ITagImportObject) new MaterialTag();
      case 19:
        return (ITagImportObject) new TechDraftTag();
      case 20:
        return (ITagImportObject) new TableLinkTag();
      case 21:
        return (ITagImportObject) new UserTag();
      case 22:
        return (ITagImportObject) new TechRecordObjectTag();
      case 23:
        return (ITagImportObject) new ArticleOptionsTag();
      case 24:
        return (ITagImportObject) new ListIntTag();
      case 25:
        return (ITagImportObject) new BlobTag();
      case 26:
        return (ITagImportObject) new LinkTag();
      case 27:
        return (ITagImportObject) new TableAttributesPV();
      case 28:
        return (ITagImportObject) new ImportingObjectTag();
      case 29:
        return (ITagImportObject) new ImportingRelationTag();
      case 30:
        return (ITagImportObject) new SearchArticleID();
      case 31 /*0x1F*/:
        return (ITagImportObject) new ProcRouteEntryTag();
      case 32 /*0x20*/:
        return (ITagImportObject) new ProcRoutesTag();
      case 33:
        return (ITagImportObject) new ProductionCopyInfo();
      case 34:
        return (ITagImportObject) new ObjectInfoEx();
      default:
        return (ITagImportObject) null;
    }
  }

  /// <summary>Чтение строки из потока</summary>
  public static string GetString(BinaryReader br)
  {
    if (br == null)
      return string.Empty;
    string empty = string.Empty;
    int length = br.ReadInt32();
    if (length > 0)
      empty = TagImportObjectHelper.GetString(length, br);
    return empty;
  }

  /// <remarks>Оставили для совместимости</remarks>
  public static string GetString(int length, BinaryReader br)
  {
    if (br == null || length <= 0)
      return string.Empty;
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append(br.ReadChars(length));
    return stringBuilder.ToString();
  }

  /// <summary>Запись строки в поток</summary>
  public static bool SetString(string strData, BinaryWriter bw)
  {
    if (bw == null)
      return false;
    int length = string.IsNullOrEmpty(strData) ? 0 : strData.Length;
    bw.Write(length);
    if (length > 0)
    {
      char[] charArray = strData.ToCharArray();
      bw.Write(charArray);
    }
    return true;
  }
}
