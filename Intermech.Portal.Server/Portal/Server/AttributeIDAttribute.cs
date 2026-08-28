// Decompiled with JetBrains decompiler
// Type: Intermech.Portal.Server.AttributeIDAttribute
// Assembly: Intermech.Portal.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 814BABAA-794A-446D-BCF7-B9A0D67EFF42
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Portal.Server.dll

using System;

#nullable disable
namespace Intermech.Portal.Server;

internal class AttributeIDAttribute : Attribute
{
  public object AttributeID { get; private set; }

  public AttributeIDAttribute(object attributeID) => this.AttributeID = attributeID;
}
