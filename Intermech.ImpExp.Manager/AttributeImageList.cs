// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Manager.AttributeImageList
// Assembly: Intermech.ImpExp.Manager, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 837A17E0-5EE6-46DB-9571-5E7918B22E69
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Manager.exe

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Manager.Properties;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.ImpExp.Manager;

public class AttributeImageList : IAttributeImageList
{
  private ImageList _imageList;
  private Dictionary<FieldTypes, int> _imageIndexes;

  private void CreateImageList()
  {
    this._imageList = new ImageList();
    this._imageList.ColorDepth = ColorDepth.Depth24Bit;
    this._imageIndexes = new Dictionary<FieldTypes, int>();
    this.LoadIcon4AttributeType(Resources.empty, FieldTypes.ftUnknown);
    this.LoadIcon4AttributeType(Resources.ftAutoInc, FieldTypes.ftAutoInc);
    this.LoadIcon4AttributeType(Resources.ftBoolean, FieldTypes.ftBoolean);
    this.LoadIcon4AttributeType(Resources.ftDate, FieldTypes.ftDateTime);
    this.LoadIcon4AttributeType(Resources.ftDouble, FieldTypes.ftDouble);
    this.LoadIcon4AttributeType(Resources.ftExternalLink, FieldTypes.ftExternalLink);
    this.LoadIcon4AttributeType(Resources.ftFile, FieldTypes.ftFile);
    this.LoadIcon4AttributeType(Resources.ftGuid, FieldTypes.ftGuid);
    this.LoadIcon4AttributeType(Resources.ftInteger, FieldTypes.ftInteger);
    this.LoadIcon4AttributeType(Resources.ftMeasured, FieldTypes.ftMeasured);
    this.LoadIcon4AttributeType(Resources.ftMemo, FieldTypes.ftMemo);
    this.LoadIcon4AttributeType(Resources.ftObjectLink, FieldTypes.ftObjectLink);
    this.LoadIcon4AttributeType(Resources.ftPassword, FieldTypes.ftPassword);
    this.LoadIcon4AttributeType(Resources.ftShortBlob, FieldTypes.ftShortBlob);
    this.LoadIcon4AttributeType(Resources.ftString, FieldTypes.ftString);
    this.LoadIcon4AttributeType(Resources.ftSystem, FieldTypes.ftSystem);
    this.LoadIcon4AttributeType(Resources.ftBlob, FieldTypes.ftBlob);
  }

  public AttributeImageList() => this.CreateImageList();

  private void LoadIcon4AttributeType(Icon icon, FieldTypes fieldTypes)
  {
    this._imageList.Images.Add(icon);
    this._imageIndexes.Add(fieldTypes, this._imageList.Images.Count - 1);
  }

  public ImageList ImageList => this._imageList;

  public int ImageIndex(FieldTypes fieldType)
  {
    int num = -1;
    this._imageIndexes.TryGetValue(fieldType, out num);
    return num;
  }

  public Image GetImage(FieldTypes fieldType)
  {
    int index = this.ImageIndex(fieldType);
    return index >= 0 ? this._imageList.Images[index] : (Image) null;
  }
}
