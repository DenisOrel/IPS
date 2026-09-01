// Decompiled with JetBrains decompiler
// Type: Intermech.ApplicationModel.IAppAssemblyAutoLoader
// Assembly: Intermech.Serialization.Compatibility, Version=1.0.1.74, Culture=neutral, PublicKeyToken=null
// MVID: D3658D7B-7F63-413B-8D5F-ACD5662A960C
// Assembly location: D:\Projects\IPS Code\AltiumDesigner\IPSAddIn\Intermech.Serialization.Compatibility.dll

using System.Reflection;

#nullable disable
namespace Intermech.ApplicationModel;

public interface IAppAssemblyAutoLoader
{
  Assembly TryAutoLoadAssembly(AppAssemblyResolveRequest request);
}
