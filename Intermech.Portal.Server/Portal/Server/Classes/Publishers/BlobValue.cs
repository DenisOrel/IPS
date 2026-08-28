// Decompiled with JetBrains decompiler
// Type: Intermech.Portal.Server.Classes.Publishers.BlobValue
// Assembly: Intermech.Portal.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 814BABAA-794A-446D-BCF7-B9A0D67EFF42
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Portal.Server.dll

using Intermech.Interfaces.WebPortal;

#nullable disable
namespace Intermech.Portal.Server.Classes.Publishers;

internal sealed class BlobValue : BlobValueBase
{
  public BlobValue(AttributeInfo attrInfo, char siteCode, ValueInfo value)
    : base(attrInfo, siteCode, value)
  {
    this.key = $"{siteCode}{this.attrID}[{value.Index}]";
  }

  public override void PrepareStorage(RemarksStorage storage)
  {
    if (this.value.Index != 0)
      return;
    storage.ClearRemarkFiles($"{this.siteCode}{this.attrID}\\[\\d\\]", true);
  }
}
