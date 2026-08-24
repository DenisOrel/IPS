// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Manager.BigImageList
// Assembly: Intermech.ImpExp.Manager, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 837A17E0-5EE6-46DB-9571-5E7918B22E69
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Manager.exe

using ImSSP;
using Intermech.Interfaces;
using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.ImpExp.Manager;

public class BigImageList : IBigImageList, IDisposable
{
  private ImageList _imageList;
  private Hashtable _namesId;
  private bool _dispose;

  public event EventHandler Changed;

  public BigImageList()
  {
    this._namesId = new Hashtable();
    this._imageList = new ImageList();
    this._imageList.ColorDepth = ColorDepth.Depth24Bit;
    this._imageList.ImageSize = new Size(48 /*0x30*/, 48 /*0x30*/);
    this._dispose = true;
  }

  public BigImageList(ImageList imagelist)
  {
    this._namesId = new Hashtable();
    this._imageList = imagelist;
    this._dispose = false;
  }

  internal void AddNames(string[] names)
  {
    int num1 = 0;
    int num2 = this._imageList.Images.Count - 1;
    foreach (string name in names)
    {
      if (num1 > num2)
        break;
      this._namesId.Add((object) name, (object) num1++);
    }
  }

  public int Add(Image image, string name)
  {
    if (image == null)
      throw new ArgumentException(sc_8408.ssp_imclient_8409());
    if (name == null || name.Length == 0)
      throw new ArgumentException(sc_8408.ssp_imclient_8410());
    if (this._namesId.ContainsKey((object) name))
    {
      int index = (int) this._namesId[(object) name];
      this._imageList.Images[index] = image;
      return index;
    }
    this._imageList.Images.Add(image);
    int num = this._imageList.Images.Count - 1;
    this._namesId[(object) name] = (object) num;
    this.OnChanged();
    return num;
  }

  public int AddStrip(Image images, string[] names)
  {
    int length = names.Length;
    int num = images.Width / images.Height;
    if (length != num)
      throw new ArgumentException(sc_8408.ssp_imclient_8411());
    int count = this._imageList.Images.Count;
    this._imageList.Images.AddStrip(images);
    for (int index = 0; index < length; ++index)
      this._namesId[(object) names[index]] = (object) (count + index);
    this.OnChanged();
    return count;
  }

  public int ImageIndex(string name)
  {
    return this._namesId.ContainsKey((object) name) ? (int) this._namesId[(object) name] : -1;
  }

  public ImageList ImageList => this._imageList;

  public string ImageName(int imageIndex)
  {
    foreach (string key in (IEnumerable) this._namesId.Keys)
    {
      if ((int) this._namesId[(object) key] == imageIndex)
        return key;
    }
    return (string) null;
  }

  private void OnChanged()
  {
    if (this.Changed == null)
      return;
    this.Changed((object) this, new EventArgs());
  }

  public ICollection Keys => this._namesId.Keys;

  public void Dispose()
  {
    if (this._dispose && this._imageList != null)
      this._imageList.Dispose();
    this._imageList = (ImageList) null;
  }
}
