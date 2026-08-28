// Decompiled with JetBrains decompiler
// Type: Intermech.Portal.Server.Classes.Publishers.FileValue
// Assembly: Intermech.Portal.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 814BABAA-794A-446D-BCF7-B9A0D67EFF42
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Portal.Server.dll

using Intermech.Interfaces.WebPortal;

#nullable disable
namespace Intermech.Portal.Server.Classes.Publishers;

internal class FileValue : BlobValueBase
{
  public FileValue(AttributeInfo attrInfo, char siteCode, ValueInfo value)
    : base(attrInfo, siteCode, value)
  {
    this.key = $"{siteCode}{value.StringValue}";
  }
}
