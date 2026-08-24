// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.RelationVisualizer.VisObject
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces;
using Intermech.Interfaces.BlobStream;
using Intermech.Interfaces.Expert;
using Intermech.Interfaces.Pdm.RelationVisualizer;
using Intermech.Pdm.VisDialogs;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

#nullable disable
namespace Intermech.Pdm.RelationVisualizer;

public class VisObject : IVisObject, IVisObjectData, ICloneable
{
  private int _level;
  public bool Disabled;
  public int LayoutWeight;
  public static readonly int PreviewWid = 67;
  public static readonly int PreviewHei = 50;
  private static readonly int fileAttrId = MetaDataHelper.GetAttributeTypeID(SystemGUIDs.attributePreview);
  private HybridRowExp dr;
  public static readonly string sep = "\r\n";

  public int Level
  {
    get => this._level;
    set => this._level = value;
  }

  public Point Org { get; set; }

  public Size Size { get; set; }

  public Rectangle Rect
  {
    get
    {
      Point org = this.Org;
      int x = org.X - this.Size.Width / 2;
      org = this.Org;
      int y = org.Y - this.Size.Height / 2;
      org = this.Org;
      int width = org.X + this.Size.Width / 2;
      org = this.Org;
      int height = org.Y + this.Size.Height / 2;
      return new Rectangle(x, y, width, height);
    }
  }

  public IVisObjectData VisObjectData { get; internal set; }

  public long ObjVerId
  {
    get => this.VisObjectData.ObjVerId;
    set => this.VisObjectData.ObjVerId = value;
  }

  public int ObjTypeId
  {
    get => this.VisObjectData.ObjTypeId;
    set => this.VisObjectData.ObjTypeId = value;
  }

  public int LCLevelId
  {
    get => this.VisObjectData.LCLevelId;
    set => this.VisObjectData.LCLevelId = value;
  }

  public string Caption
  {
    get => this.VisObjectData.Caption;
    set => this.VisObjectData.Caption = value;
  }

  public List<VisStatus> StatusList
  {
    get => this.VisObjectData.StatusList;
    set => this.VisObjectData.StatusList = value;
  }

  public void Init(IUserSession ius) => this.VisObjectData.Init(ius);

  public bool SameObject(long objId) => Math.Abs(this.ObjVerId) == objId;

  public List<VisRelation> ParentRels { get; set; }

  public List<VisRelation> ChildRels { get; set; }

  internal VisNode Node { get; set; }

  public void ObjectChanged()
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
      this.VisObjectData.Init(sessionKeeper.Session);
  }

  public Image LCLevelImage => ImageHolder.GetLCLevelImage(this.VisObjectData.LCLevelId);

  public Image TypeImage => ImageHolder.GetTypeImage(this.ObjTypeId);

  public VisLevel ParentLevel { get; set; }

  public VisScheme ParentScheme { get; set; }

  public bool PreviewChecked { get; set; }

  public bool HasPreviewType { get; set; }

  public string GetToolTipText()
  {
    return $"{this.Caption}\n{MetaDataHelper.GetObjectTypeName(this.ObjTypeId)}: {this.ObjVerId.ToString()}";
  }

  public bool ChildsOpen { get; set; } = true;

  public bool ParentsOpen { get; set; } = true;

  public bool Visible { get; set; } = true;

  public bool VisibleChanged { get; set; }

  internal bool ChildsParentsVisible { get; set; }

  public VisObject(IVisObjectData iObjData, VisLevel parLevel)
  {
    this.VisObjectData = iObjData;
    this.ParentLevel = parLevel;
    this.ParentRels = new List<VisRelation>();
    this.ChildRels = new List<VisRelation>();
    this.PreviewChecked = false;
    this.HasPreviewType = false;
  }

  internal virtual string GetTopCaption() => !(this.Caption != "") ? "<???>" : this.Caption;

  internal string GetBottomCaption(RelVisPred.NoCaptionFormula noCaptionFormula)
  {
    switch (noCaptionFormula)
    {
      case RelVisPred.NoCaptionFormula.Nom:
        return this.ObjVerId.ToString();
      case RelVisPred.NoCaptionFormula.ObjType_Nom:
        return $"{MetaDataHelper.GetObjectTypeName(this.ObjTypeId)} №{this.ObjVerId.ToString()}";
      case RelVisPred.NoCaptionFormula.St_ObjType_St_Nom:
        return $"[{MetaDataHelper.GetObjectTypeName(this.ObjTypeId)}] {this.ObjVerId.ToString()}";
      case RelVisPred.NoCaptionFormula.St_Nom_St_ObjType:
        return $"[{this.ObjVerId.ToString()}] {MetaDataHelper.GetObjectTypeName(this.ObjTypeId)}";
      default:
        return "";
    }
  }

  public StyleKind styleKind { get; set; }

  public string UpperStr { get; set; }

  public string UpperHint { get; set; }

  public string MainHint { get; set; }

  public string LowerHint { get; set; }

  public string LowerStr { get; set; }

  public Image Preview { get; set; }

  public Image PreviewHint { get; set; }

  public void SetDataRow(HybridRowExp row) => this.dr = row;

  public void PreparePreview(Image prevImage)
  {
    this.styleKind = prevImage != null ? StyleKind.ObjPreview : StyleKind.CommonObject;
    if (prevImage != null)
      this.Preview = prevImage;
    this.UpdateStyle();
  }

  public void UpdateStyle()
  {
    StyleData styleData = this.ParentScheme.StyleData;
    if (this.Preview != null)
    {
      PreviewNodeData previewStyle = styleData.GetPreviewStyle(this.ObjTypeId);
      this.UpperHint = this.ProcessPreviewString(previewStyle.UpperHint, this.dr);
      this.LowerHint = this.ProcessPreviewString(previewStyle.LowerHint, this.dr);
      this.MainHint = $"{this.Caption}\n{Convert.ToString(this.ObjVerId)}";
    }
    else
    {
      ObjNodeData objectStyle = styleData.GetObjectStyle(this.ObjTypeId);
      this.UpperStr = this.ProcessPreviewString(objectStyle.UpperStr, this.dr);
      this.UpperHint = this.ProcessPreviewString(objectStyle.UpperHint, this.dr);
      this.LowerHint = this.ProcessPreviewString(objectStyle.LowerHint, this.dr);
      this.LowerStr = this.ProcessPreviewString(objectStyle.LowerStr, this.dr);
      this.MainHint = this.ProcessPreviewString(objectStyle.MainHint, this.dr);
    }
  }

  public void ProcessRoot(HybridColumnsExp columns)
  {
    if (this.UpperStr != null)
      return;
    AttributeValues[] attributeValuesArray = (AttributeValues[]) null;
    Image prevImage = (Image) null;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBObject dbObject = sessionKeeper.Session.GetObject(this.ObjVerId, false);
      if (dbObject == null)
        return;
      BlobReaderStream blobReaderStream = new BlobReaderStream(this.ObjVerId, AttributableElements.Object, VisObject.fileAttrId, 0, 0, sessionKeeper.Session);
      BlobInformation blobInformation = blobReaderStream.BlobInformation;
      if (blobReaderStream.CanRead)
      {
        int realFileSize = (int) blobInformation.RealFileSize;
        if (realFileSize > 0)
        {
          byte[] buffer = new byte[realFileSize];
          blobReaderStream.Read(buffer, 0, realFileSize);
          using (MemoryStream memoryStream = new MemoryStream(buffer))
            prevImage = Image.FromStream((Stream) memoryStream);
        }
      }
      attributeValuesArray = dbObject.GetAttributesValues(GetAttributeValuesModes.IncludeName | GetAttributeValuesModes.IncludeGuid | GetAttributeValuesModes.IncludeObligatoryAttributes | GetAttributeValuesModes.IncludeDescriptions);
    }
    HybridRowExp row = new HybridRowExp(columns);
    foreach (AttributeValues attributeValues in attributeValuesArray)
    {
      int colIndexByName = row.GetColIndexByName(attributeValues.AttributeGuid.ToString());
      if (colIndexByName >= 0)
        row[colIndexByName] = attributeValues.Value;
    }
    this.SetDataRow(row);
    this.PreparePreview(prevImage);
  }

  protected string ProcessPreviewString(string pattern, HybridRowExp row)
  {
    string str1 = pattern.Clone() as string;
    int startIndex1 = 0;
    do
    {
      int startIndex2 = str1.IndexOf('{', startIndex1);
      if (startIndex2 >= 0)
      {
        int num = str1.IndexOf('}', startIndex2);
        if (num >= 0)
        {
          string attributeID = str1.Substring(startIndex2 + 1, num - startIndex2 - 1);
          string str2 = str1.Remove(startIndex2, num - startIndex2 + 1);
          string str3 = "<?>";
          int attributeId = MetaDataHelper.GetAttributeID((object) attributeID);
          if (attributeId != 0)
          {
            string lower = MetaDataHelper.GetAttributeTypeGuid(attributeId).ToString().ToLower();
            int indexByName = row.Columns.GetIndexByName(lower);
            if (indexByName >= 0)
              str3 = row[indexByName].ToString();
          }
          str1 = str2.Insert(startIndex2, str3);
          startIndex1 = startIndex2 + str3.Length;
        }
        else
          break;
      }
      else
        break;
    }
    while (startIndex1 < str1.Length);
    string[] separator = new string[1]{ VisObject.sep };
    string[] strArray = str1.Split(separator, StringSplitOptions.None);
    bool flag = false;
    for (int index = 0; index < strArray.Length; ++index)
    {
      string str4 = strArray[index];
      if (str4.Length > 40)
      {
        int num = (str4.Length - 40) / 2;
        strArray[index] = str4.Remove(str4.Length / 2 - num, str4.Length - 40);
        flag = true;
      }
    }
    if (flag)
      str1 = string.Join(VisObject.sep, strArray);
    return str1;
  }

  public static Bitmap ResizeImage(Image image, int height)
  {
    int width = (int) Math.Round((double) height * 1.0 * (double) image.Width / (double) image.Height);
    Rectangle destRect = new Rectangle(0, 0, width, height);
    Bitmap bitmap = new Bitmap(width, height);
    bitmap.SetResolution(image.HorizontalResolution, image.VerticalResolution);
    using (Graphics graphics = Graphics.FromImage((Image) bitmap))
    {
      graphics.CompositingMode = CompositingMode.SourceCopy;
      graphics.CompositingQuality = CompositingQuality.HighQuality;
      graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
      graphics.SmoothingMode = SmoothingMode.HighQuality;
      graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
      using (ImageAttributes imageAttr = new ImageAttributes())
      {
        imageAttr.SetWrapMode(WrapMode.TileFlipXY);
        graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, imageAttr);
      }
    }
    return bitmap;
  }

  public object Clone() => (object) new VisObject(this.VisObjectData, (VisLevel) null);
}
