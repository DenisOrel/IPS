// Decompiled with JetBrains decompiler
// Type: Intermech.Portal.Server.Classes.Publishers.BlobValueBase
// Assembly: Intermech.Portal.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 814BABAA-794A-446D-BCF7-B9A0D67EFF42
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Portal.Server.dll

using Intermech.Interfaces.WebPortal;

#nullable disable
namespace Intermech.Portal.Server.Classes.Publishers;

internal abstract class BlobValueBase : IBlobValue
{
  protected char siteCode;
  protected string key;
  protected string attrID;
  protected ValueInfo value;

  public BlobValueBase(AttributeInfo attrInfo, char siteCode, ValueInfo value)
  {
    this.attrID = string.IsNullOrEmpty(attrInfo.Guid) ? attrInfo.Name : attrInfo.Guid;
    this.value = value;
    this.siteCode = siteCode;
  }

  public virtual void PrepareStorage(RemarksStorage storage)
  {
    storage.ClearRemarkFiles(this.key, false);
  }

  public string Key => this.key;
}
