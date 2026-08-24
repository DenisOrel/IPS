// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.ScriptPad.Views.AvalonCodeEditor.CodeCompletionImages
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using System;
using System.Reflection;
using System.Windows.Media;
using System.Windows.Media.Imaging;

#nullable disable
namespace Intermech.Scripting.ScriptPad.Views.AvalonCodeEditor;

internal sealed class CodeCompletionImages
{
  private static readonly string currentAssemblyName = CodeCompletionImages.GetCurrentAssemblyName();
  private static readonly BitmapImage literalImage = CodeCompletionImages.LoadBitmapFromResource("Literal.png");
  private static readonly BitmapImage namespaceImage = CodeCompletionImages.LoadBitmapFromResource("Namespace.png");
  private static readonly BitmapImage fieldImage = CodeCompletionImages.LoadBitmapFromResource("Field.png");
  private static readonly BitmapImage readOnlyFieldImage = CodeCompletionImages.LoadBitmapFromResource("ReadOnlyField.png");
  private static readonly BitmapImage classImage = CodeCompletionImages.LoadBitmapFromResource("Class.png");
  private static readonly BitmapImage staticClassImage = CodeCompletionImages.LoadBitmapFromResource("StaticClass.png");
  private static readonly BitmapImage structImage = CodeCompletionImages.LoadBitmapFromResource("Struct.png");
  private static readonly BitmapImage interfaceImage = CodeCompletionImages.LoadBitmapFromResource("Interface.png");
  private static readonly BitmapImage delegateImage = CodeCompletionImages.LoadBitmapFromResource("Delegate.png");
  private static readonly BitmapImage enumImage = CodeCompletionImages.LoadBitmapFromResource("Enum.png");
  private static readonly BitmapImage enumValueImage = CodeCompletionImages.LoadBitmapFromResource("EnumValue.png");
  private static readonly BitmapImage constructorImage = CodeCompletionImages.LoadBitmapFromResource("Constructor.png");
  private static readonly BitmapImage methodImage = CodeCompletionImages.LoadBitmapFromResource("Method.png");
  private static readonly BitmapImage virtualMethodImage = CodeCompletionImages.LoadBitmapFromResource("VirtualMethod.png");
  private static readonly BitmapImage extensionMethodImage = CodeCompletionImages.LoadBitmapFromResource("ExtensionMethod.png");
  private static readonly BitmapImage pInvokeMethodImage = CodeCompletionImages.LoadBitmapFromResource("PInvokeMethod.png");
  private static readonly BitmapImage operatorImage = CodeCompletionImages.LoadBitmapFromResource("Operator.png");
  private static readonly BitmapImage propertyImage = CodeCompletionImages.LoadBitmapFromResource("Property.png");
  private static readonly BitmapImage indexerPropertyImage = CodeCompletionImages.LoadBitmapFromResource("IndexerProperty.png");
  private static readonly BitmapImage eventImage = CodeCompletionImages.LoadBitmapFromResource("Event.png");

  public static ImageSource LiteralImage => (ImageSource) CodeCompletionImages.literalImage;

  public static ImageSource NamespaceImage => (ImageSource) CodeCompletionImages.namespaceImage;

  public static ImageSource FieldImage => (ImageSource) CodeCompletionImages.fieldImage;

  public static ImageSource ReadOnlyFieldImage
  {
    get => (ImageSource) CodeCompletionImages.readOnlyFieldImage;
  }

  public static ImageSource ClassImage => (ImageSource) CodeCompletionImages.classImage;

  public static ImageSource StaticClassImage => (ImageSource) CodeCompletionImages.staticClassImage;

  public static ImageSource StructImage => (ImageSource) CodeCompletionImages.structImage;

  public static ImageSource InterfaceImage => (ImageSource) CodeCompletionImages.interfaceImage;

  public static ImageSource DelegateImage => (ImageSource) CodeCompletionImages.delegateImage;

  public static ImageSource EnumImage => (ImageSource) CodeCompletionImages.enumImage;

  public static ImageSource EnumValueImage => (ImageSource) CodeCompletionImages.enumValueImage;

  public static ImageSource ConstructorImage => (ImageSource) CodeCompletionImages.constructorImage;

  public static ImageSource MethodImage => (ImageSource) CodeCompletionImages.methodImage;

  public static ImageSource VirtualMethodImage
  {
    get => (ImageSource) CodeCompletionImages.virtualMethodImage;
  }

  public static ImageSource ExtensionMethodImage
  {
    get => (ImageSource) CodeCompletionImages.extensionMethodImage;
  }

  public static ImageSource PInvokeMethodImage
  {
    get => (ImageSource) CodeCompletionImages.pInvokeMethodImage;
  }

  public static ImageSource OperatorImage => (ImageSource) CodeCompletionImages.operatorImage;

  public static ImageSource PropertyImage => (ImageSource) CodeCompletionImages.propertyImage;

  public static ImageSource IndexerPropertyImage
  {
    get => (ImageSource) CodeCompletionImages.indexerPropertyImage;
  }

  public static ImageSource EventImage => (ImageSource) CodeCompletionImages.eventImage;

  private static string GetCurrentAssemblyName() => Assembly.GetExecutingAssembly().FullName;

  private static BitmapImage LoadBitmapFromResource(string fileName)
  {
    BitmapImage bitmapImage = new BitmapImage(new Uri($"pack://application:,,,/{CodeCompletionImages.currentAssemblyName};component/ScriptPad/Icons/CodeCompletion/{fileName}"));
    bitmapImage.Freeze();
    return bitmapImage;
  }
}
