// Decompiled with JetBrains decompiler
// Type: Intermech.Portal.Server.VersionsColumnNames
// Assembly: Intermech.Portal.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 814BABAA-794A-446D-BCF7-B9A0D67EFF42
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Portal.Server.dll

using Intermech.Kernel.Search;

#nullable disable
namespace Intermech.Portal.Server;

internal enum VersionsColumnNames
{
  [AttributeID(ObligatoryObjectAttributes.F_OBJECT_ID)] ObjectID,
  [AttributeID(ObligatoryObjectAttributes.F_ID)] ID,
  [AttributeID("cad0156a-306c-11d8-b4e9-00304f19f545")] LinkedGuid,
  [AttributeID("cad001a0-306c-11d8-b4e9-00304f19f545")] ObjectTypeGuid,
  [AttributeID("cad014cf-306c-11d8-b4e9-00304f19f545")] ObjectTypeName,
}
