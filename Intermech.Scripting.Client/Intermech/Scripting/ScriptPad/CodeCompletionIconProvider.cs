// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.ScriptPad.CodeCompletionIconProvider
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using Intermech.Scripting.Common.DesignTime;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Intermech.Scripting.ScriptPad;

internal static class CodeCompletionIconProvider
{
  private static Dictionary<CodeCompletionItemType, Image> codeCompletionIcons = new Dictionary<CodeCompletionItemType, Image>()
  {
    {
      CodeCompletionItemType.Class,
      (Image) IDEInternalResources.IR_Class
    },
    {
      CodeCompletionItemType.Constructor,
      (Image) IDEInternalResources.IR_Constructor
    },
    {
      CodeCompletionItemType.Delegate,
      (Image) IDEInternalResources.IR_Delegate
    },
    {
      CodeCompletionItemType.Enum,
      (Image) IDEInternalResources.IR_Enum
    },
    {
      CodeCompletionItemType.EnumValue,
      (Image) IDEInternalResources.IR_EnumValue
    },
    {
      CodeCompletionItemType.Event,
      (Image) IDEInternalResources.IR_Event
    },
    {
      CodeCompletionItemType.ExtensionMethod,
      (Image) IDEInternalResources.IR_ExtensionMethod
    },
    {
      CodeCompletionItemType.Field,
      (Image) IDEInternalResources.IR_Field
    },
    {
      CodeCompletionItemType.IndexerProperty,
      (Image) IDEInternalResources.IR_IndexerProperty
    },
    {
      CodeCompletionItemType.Interface,
      (Image) IDEInternalResources.IR_Interface
    },
    {
      CodeCompletionItemType.Literal,
      (Image) IDEInternalResources.IR_Literal
    },
    {
      CodeCompletionItemType.Method,
      (Image) IDEInternalResources.IR_Method
    },
    {
      CodeCompletionItemType.Namespace,
      (Image) IDEInternalResources.IR_Namespace
    },
    {
      CodeCompletionItemType.Operator,
      (Image) IDEInternalResources.IR_Operator
    },
    {
      CodeCompletionItemType.PInvokeMethod,
      (Image) IDEInternalResources.IR_PInvokeMethod
    },
    {
      CodeCompletionItemType.Property,
      (Image) IDEInternalResources.IR_Property
    },
    {
      CodeCompletionItemType.ReadOnlyField,
      (Image) IDEInternalResources.IR_ReadOnlyField
    },
    {
      CodeCompletionItemType.StaticClass,
      (Image) IDEInternalResources.IR_StaticClass
    },
    {
      CodeCompletionItemType.Struct,
      (Image) IDEInternalResources.IR_Struct
    },
    {
      CodeCompletionItemType.VirtualMethod,
      (Image) IDEInternalResources.IR_VirtualMethod
    }
  };

  public static Image GetIcon(CodeCompletionItemType itemType)
  {
    return !CodeCompletionIconProvider.codeCompletionIcons.ContainsKey(itemType) ? (Image) null : CodeCompletionIconProvider.codeCompletionIcons[itemType];
  }
}
