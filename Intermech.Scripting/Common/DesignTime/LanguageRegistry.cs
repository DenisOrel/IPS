// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.Common.DesignTime.LanguageRegistry
// Assembly: Intermech.Scripting, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 8614EF74-D879-46F7-BBB4-441A9D335356
// Assembly location: D:\IPS\Client\RoslynScriptCompiler\Intermech.Scripting.dll
// XML documentation location: D:\IPS\Client\Intermech.Scripting.xml

using Intermech.Collections;
using Intermech.IO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

#nullable disable
namespace Intermech.Scripting.Common.DesignTime;

/// <summary>
/// Реализует сервис регистрации доступных для использования сценарных языков.
/// Реализация не является thread-safe.
/// </summary>
public class LanguageRegistry
{
  private readonly List<LanguageDescriptor> descriptors;
  private IList<LanguageInfo> cachedLanguageList;
  private IList<LanguageDescriptor> cachedDescriptorList;

  /// <summary>Создает объект.</summary>
  public LanguageRegistry() => this.descriptors = new List<LanguageDescriptor>();

  public void Add(LanguageDescriptor descriptor)
  {
    if (descriptor == null)
      throw new ArgumentNullException(nameof (descriptor));
    if (this.descriptors.Contains(descriptor))
      throw new InvalidOperationException($"Language '{descriptor.LanguageInfo.Name}' is already added.");
    if (this.GetByLanguageName(descriptor.LanguageInfo.Name, false) != null || this.GetByFileExtension(descriptor.LanguageInfo.SourceExtension, false) != null)
      throw new InvalidOperationException($"A language similar to '{descriptor.LanguageInfo.Name}' is already added.");
    this.descriptors.Add(descriptor);
    this.ResetCachedValues();
  }

  private void ResetCachedValues()
  {
    this.cachedLanguageList = (IList<LanguageInfo>) null;
    this.cachedDescriptorList = (IList<LanguageDescriptor>) null;
  }

  public LanguageDescriptor GetByLanguageName(string languageName, bool throwIfNotFound)
  {
    if (languageName == null)
      throw new ArgumentNullException(nameof (languageName));
    LanguageDescriptor languageDescriptor = this.descriptors.Find((Predicate<LanguageDescriptor>) (item => item.LanguageInfo.Name == languageName));
    return languageDescriptor != null || !throwIfNotFound ? languageDescriptor : throw new InvalidOperationException($"Language '{languageName}' is not found.");
  }

  public LanguageDescriptor GetByFileExtension(string fileExtension, bool throwIfNotFound)
  {
    if (fileExtension == null)
      throw new ArgumentNullException(nameof (fileExtension));
    LanguageDescriptor languageDescriptor = this.descriptors.Find((Predicate<LanguageDescriptor>) (item => PathUtils.IsSamePath(item.LanguageInfo.SourceExtension, fileExtension)));
    return languageDescriptor != null || !throwIfNotFound ? languageDescriptor : throw new InvalidOperationException($"Language for {fileExtension} files is not found.");
  }

  public IList<LanguageInfo> Languages
  {
    [DebuggerStepThrough] get
    {
      if (this.cachedLanguageList == null)
        this.cachedLanguageList = (IList<LanguageInfo>) new ReadOnlyCollection<LanguageInfo>((IList<LanguageInfo>) CollectionUtils.ConvertAsList<LanguageDescriptor, LanguageInfo>((ICollection<LanguageDescriptor>) this.descriptors, (Converter<LanguageDescriptor, LanguageInfo>) (x => x.LanguageInfo)));
      return this.cachedLanguageList;
    }
  }

  public IList<LanguageDescriptor> Descriptors
  {
    [DebuggerStepThrough] get
    {
      if (this.cachedDescriptorList == null)
        this.cachedDescriptorList = (IList<LanguageDescriptor>) new ReadOnlyCollection<LanguageDescriptor>((IList<LanguageDescriptor>) new List<LanguageDescriptor>((IEnumerable<LanguageDescriptor>) this.descriptors));
      return this.cachedDescriptorList;
    }
  }
}
