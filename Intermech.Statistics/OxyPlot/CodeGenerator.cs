// Decompiled with JetBrains decompiler
// Type: OxyPlot.CodeGenerator
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

#nullable disable
namespace OxyPlot;

public class CodeGenerator
{
  private readonly StringBuilder sb;
  private readonly Dictionary<string, bool> variables;
  private string indentString;
  private int indents;

  public CodeGenerator(PlotModel model)
  {
    this.variables = new Dictionary<string, bool>();
    this.sb = new StringBuilder();
    this.Indents = 8;
    string title = model.Title ?? "Untitled";
    this.AppendLine("[Example({0})]", (object) CodeGeneratorStringExtensions.ToCode(title));
    this.AppendLine("public static PlotModel {0}()", (object) this.MakeValidVariableName(title));
    this.AppendLine("{");
    this.Indents += 4;
    string name = this.Add((object) model);
    this.AddChildren(name, "Axes", (IEnumerable) model.Axes);
    this.AddChildren(name, "Series", (IEnumerable) model.Series);
    this.AddChildren(name, "Annotations", (IEnumerable) model.Annotations);
    this.AppendLine("return {0};", (object) name);
    this.Indents -= 4;
    this.AppendLine("}");
  }

  private int Indents
  {
    get => this.indents;
    set
    {
      this.indents = value;
      this.indentString = new string(' ', value);
    }
  }

  public static string FormatCode(string format, params object[] values)
  {
    object[] objArray = new object[values.Length];
    for (int index = 0; index < values.Length; ++index)
      objArray[index] = (object) values[index].ToCode();
    return string.Format(format, objArray);
  }

  public static string FormatConstructor(Type type, string format, params object[] values)
  {
    return $"new {type.Name}({CodeGenerator.FormatCode(format, values)})";
  }

  public string ToCode() => this.sb.ToString();

  private string Add(object obj)
  {
    Type type = obj.GetType();
    if (!((IEnumerable<ConstructorInfo>) type.GetConstructors()).Any<ConstructorInfo>((Func<ConstructorInfo, bool>) (ci => ci.GetParameters().Length == 0)))
      return $"/* Cannot generate code for {type.Name} constructor */";
    object instance = Activator.CreateInstance(type);
    string newVariableName = this.GetNewVariableName(type);
    this.variables.Add(newVariableName, true);
    this.AppendLine("var {0} = new {1}();", (object) newVariableName, (object) type.Name);
    this.SetProperties(obj, newVariableName, instance);
    return newVariableName;
  }

  private void AddChildren(string name, string collectionName, IEnumerable children)
  {
    foreach (object child in children)
    {
      string str = this.Add(child);
      this.AppendLine("{0}.{1}.Add({2});", (object) name, (object) collectionName, (object) str);
    }
  }

  private void AddItems(string name, IList list)
  {
    foreach (object obj in (IEnumerable) list)
    {
      string code = obj.ToCode();
      if (code != null)
        this.AppendLine("{0}.Add({1});", (object) name, (object) code);
    }
  }

  private void AddArray(string name, Array array)
  {
    Type elementType = array.GetType().GetElementType();
    if (array.Rank == 1)
    {
      this.AppendLine("{0} = new {1}[{2}];", (object) name, (object) elementType.Name, (object) array.Length);
      for (int index = 0; index < array.Length; ++index)
      {
        string code = array.GetValue(index).ToCode();
        if (code != null)
          this.AppendLine("{0}[{1}] = {2};", (object) name, (object) index, (object) code);
      }
    }
    if (array.Rank == 2)
    {
      this.AppendLine("{0} = new {1}[{2}, {3}];", (object) name, (object) elementType.Name, (object) array.GetLength(0), (object) array.GetLength(1));
      for (int index1 = 0; index1 < array.GetLength(0); ++index1)
      {
        for (int index2 = 0; index2 < array.GetLength(1); ++index2)
        {
          string code = array.GetValue(index1, index2).ToCode();
          if (code != null)
            this.AppendLine("{0}[{1}, {2}] = {3};", (object) name, (object) index1, (object) index2, (object) code);
        }
      }
    }
    if (array.Rank > 2)
      throw new NotImplementedException();
  }

  private void AppendLine(string format, params object[] args)
  {
    if (args.Length != 0)
      this.sb.AppendLine(this.indentString + string.Format((IFormatProvider) CultureInfo.InvariantCulture, format, args));
    else
      this.sb.AppendLine(this.indentString + format);
  }

  private bool AreListsEqual(IList list1, IList list2)
  {
    if (list1 == null || list2 == null || list1.Count != list2.Count)
      return false;
    for (int index = 0; index < list1.Count; ++index)
    {
      if (!list1[index].Equals(list2[index]))
        return false;
    }
    return true;
  }

  private T GetFirstAttribute<T>(PropertyInfo pi) where T : Attribute
  {
    object[] customAttributes = pi.GetCustomAttributes(typeof (CodeGenerationAttribute), true);
    int index = 0;
    return index < customAttributes.Length ? (T) customAttributes[index] : default (T);
  }

  private string GetNewVariableName(Type type)
  {
    string name = type.Name;
    string str = char.ToLower(name[0]).ToString() + name.Substring(1);
    int num = 1;
    while (this.variables.ContainsKey(str + (object) num))
      ++num;
    return str + (object) num;
  }

  private string MakeValidVariableName(string title)
  {
    if (title == null)
      return (string) null;
    Regex regex = new Regex("[a-zA-Z_][a-zA-Z0-9_]*");
    StringBuilder stringBuilder = new StringBuilder();
    foreach (char ch in title)
    {
      string input = ch.ToString();
      if (regex.Match(input).Success)
        stringBuilder.Append(input);
    }
    return stringBuilder.ToString();
  }

  private void SetProperties(object instance, string varName, object defaultValues)
  {
    Type type = instance.GetType();
    Dictionary<string, IList> dictionary1 = new Dictionary<string, IList>();
    Dictionary<string, Array> dictionary2 = new Dictionary<string, Array>();
    foreach (PropertyInfo property in type.GetProperties(BindingFlags.Instance | BindingFlags.Public))
    {
      CodeGenerationAttribute firstAttribute = this.GetFirstAttribute<CodeGenerationAttribute>(property);
      if (firstAttribute == null || firstAttribute.GenerateCode)
      {
        string str = $"{varName}.{property.Name}";
        object list1 = property.GetValue(instance, (object[]) null);
        object list2 = property.GetValue(defaultValues, (object[]) null);
        if (!this.AreListsEqual(list1 as IList, list2 as IList))
        {
          switch (list1)
          {
            case Array array:
              dictionary2.Add(str, array);
              continue;
            case IList list:
              dictionary1.Add(str, list);
              continue;
            default:
              MethodInfo setMethod = property.GetSetMethod();
              if (!(setMethod == (MethodInfo) null) && setMethod.IsPublic && (list1 == null || !list1.Equals(list2)) && list1 != list2)
              {
                this.SetProperty(str, list1);
                continue;
              }
              continue;
          }
        }
      }
    }
    foreach (KeyValuePair<string, IList> keyValuePair in dictionary1)
      this.AddItems(keyValuePair.Key, keyValuePair.Value);
    foreach (KeyValuePair<string, Array> keyValuePair in dictionary2)
      this.AddArray(keyValuePair.Key, keyValuePair.Value);
  }

  private void SetProperty(string name, object value)
  {
    string code = value.ToCode();
    if (code == null)
      return;
    this.AppendLine("{0} = {1};", (object) name, (object) code);
  }
}
