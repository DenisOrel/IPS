// Decompiled with JetBrains decompiler
// Type: ICSharpCode.NRefactory.Services.CompletionItemTypes
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using ICSharpCode.NRefactory.TypeSystem;
using Intermech.Scripting.Common.DesignTime;
using System;

#nullable disable
namespace ICSharpCode.NRefactory.Services;

internal static class CompletionItemTypes
{
  public static CodeCompletionItemType GetItemType(IEntity entity)
  {
    if (entity == null)
      throw new ArgumentNullException(nameof (entity));
    switch (entity.SymbolKind)
    {
      case SymbolKind.TypeDefinition:
        return CompletionItemTypes.GetItemTypeForType(((IType) entity).Kind, entity.IsStatic);
      case SymbolKind.Field:
        IField field = (IField) entity;
        return field.IsConst ? (field.DeclaringTypeDefinition != null && field.DeclaringTypeDefinition.Kind == TypeKind.Enum ? CodeCompletionItemType.EnumValue : CodeCompletionItemType.Literal) : (!field.IsReadOnly ? CodeCompletionItemType.Field : CodeCompletionItemType.ReadOnlyField);
      case SymbolKind.Property:
        return CodeCompletionItemType.Property;
      case SymbolKind.Indexer:
        return CodeCompletionItemType.IndexerProperty;
      case SymbolKind.Event:
        return CodeCompletionItemType.Event;
      case SymbolKind.Method:
        IMethod method = (IMethod) entity;
        if (method.IsExtensionMethod)
          return CodeCompletionItemType.ExtensionMethod;
        return !method.IsOverridable ? CodeCompletionItemType.Method : CodeCompletionItemType.VirtualMethod;
      case SymbolKind.Operator:
      case SymbolKind.Destructor:
        return CodeCompletionItemType.Operator;
      case SymbolKind.Constructor:
        return CodeCompletionItemType.Constructor;
      default:
        return CodeCompletionItemType.Unknown;
    }
  }

  public static CodeCompletionItemType GetItemType(IUnresolvedEntity entity)
  {
    if (entity == null)
      throw new ArgumentNullException(nameof (entity));
    switch (entity.SymbolKind)
    {
      case SymbolKind.TypeDefinition:
        return CompletionItemTypes.GetItemTypeForType(((IUnresolvedTypeDefinition) entity).Kind, entity.IsStatic);
      case SymbolKind.Field:
        IUnresolvedField unresolvedField = (IUnresolvedField) entity;
        return unresolvedField.IsConst ? (unresolvedField.DeclaringTypeDefinition != null && unresolvedField.DeclaringTypeDefinition.Kind == TypeKind.Enum ? CodeCompletionItemType.EnumValue : CodeCompletionItemType.Literal) : (!unresolvedField.IsReadOnly ? CodeCompletionItemType.Field : CodeCompletionItemType.ReadOnlyField);
      case SymbolKind.Property:
        return CodeCompletionItemType.Property;
      case SymbolKind.Indexer:
        return CodeCompletionItemType.IndexerProperty;
      case SymbolKind.Event:
        return CodeCompletionItemType.Event;
      case SymbolKind.Method:
        return !((IUnresolvedMember) entity).IsOverridable ? CodeCompletionItemType.Method : CodeCompletionItemType.VirtualMethod;
      case SymbolKind.Operator:
      case SymbolKind.Destructor:
        return CodeCompletionItemType.Operator;
      case SymbolKind.Constructor:
        return CodeCompletionItemType.Constructor;
      default:
        return CodeCompletionItemType.Unknown;
    }
  }

  private static CodeCompletionItemType GetItemTypeForType(TypeKind typeKind, bool isStatic)
  {
    switch (typeKind)
    {
      case TypeKind.Class:
        return !isStatic ? CodeCompletionItemType.Class : CodeCompletionItemType.StaticClass;
      case TypeKind.Interface:
        return CodeCompletionItemType.Interface;
      case TypeKind.Struct:
      case TypeKind.Void:
        return CodeCompletionItemType.Struct;
      case TypeKind.Delegate:
        return CodeCompletionItemType.Delegate;
      case TypeKind.Enum:
        return CodeCompletionItemType.Enum;
      case TypeKind.Module:
        return CodeCompletionItemType.StaticClass;
      default:
        return CodeCompletionItemType.Unknown;
    }
  }
}
