// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.FormParser
// Assembly: Intermech.ImpExp.SearchData, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 218D3933-9EC7-421F-AD43-19C3596D6EE8
// Assembly location: D:\IPS\Client\Intermech.ImpExp.SearchData.dll

using System;
using System.Text.RegularExpressions;

#nullable disable
namespace Intermech.ImpExp;

internal class FormParser
{
  private string _formText = "";
  private static Regex _objRegex = new Regex("(?<prefix>\\s*)object ((?<name>Free.*):\\s*)?(?<class>TFree.*)", RegexOptions.Compiled);
  private static Regex _longLineGlue = new Regex("'\\s\\+\\r\\n\\s*'", RegexOptions.Compiled | RegexOptions.Singleline);
  private static Regex _longLineUp = new Regex("=\\s*\\r\\n\\s*(['#])", RegexOptions.Compiled | RegexOptions.Singleline);
  private Regex _entitiesRegex = new Regex("('#(?<code>\\d{2})'?)|('?#(?<code>\\d{2})')", RegexOptions.Compiled);
  private string _valDelim = " = ";
  private int _valDelimLen;
  private FormObjectList objList = new FormObjectList();

  public FormObjectList Objects => this.objList;

  public FormParser(string text)
  {
    this._formText = FormParser._longLineGlue.Replace(text, "");
    this._formText = FormParser._longLineUp.Replace(this._formText, "= $1");
    this._valDelimLen = this._valDelim.Length;
  }

  private string EntityReplacer(Match m)
  {
    return ((char) Convert.ToInt32(m.Groups["code"].Value)).ToString();
  }

  private string ReplaceStringEntities(string s)
  {
    return s.StartsWith("'") || s.StartsWith("#") ? this._entitiesRegex.Replace(s, new MatchEvaluator(this.EntityReplacer)) : s;
  }

  private void ParseObject(FormObjectList parentList, string name, string cname, string text)
  {
    int num1 = text.IndexOf("\r\n");
    if (num1 == -1 || !(cname != ""))
      return;
    if (name == "")
      name = cname + (object) parentList.Count;
    FormObject formObject = new FormObject();
    formObject.Name = name;
    formObject.Class = cname;
    do
    {
      int num2 = num1 + 2;
      num1 = text.IndexOf("\r\n", num2);
      if (num1 != -1)
      {
        string str1 = text.Substring(num2, num1 - num2).Trim();
        if (!(str1 == "end"))
        {
          if (str1.StartsWith("object"))
          {
            int objects = this.FindObjects(formObject.Children, text, num2);
            if (objects >= 0)
              num1 = objects - 2;
            else
              break;
          }
          else
          {
            int length = str1.IndexOf(this._valDelim);
            if (length != -1)
            {
              string str2 = this.ReplaceStringEntities(str1.Substring(length + this._valDelimLen, str1.Length - length - this._valDelimLen).Trim()).Trim('\'');
              formObject.Add(str1.Substring(0, length), str2);
            }
          }
        }
        else
          break;
      }
    }
    while (num1 != -1);
    parentList.Add(formObject);
  }

  private int FindObjects(FormObjectList parentList, string s, int ind)
  {
    int objects = -1;
    int startIndex;
    do
    {
      startIndex = -1;
      int num = s.IndexOf("\r\n", ind);
      if (num != -1)
      {
        string input = s.Substring(ind, num - ind);
        Match match = FormParser._objRegex.Match(input);
        if (match.Success)
        {
          string str1 = match.Groups["prefix"].ToString();
          string name = match.Groups["name"].ToString();
          string cname = match.Groups["class"].ToString();
          startIndex = ind + match.Groups["prefix"].Index + match.Groups["prefix"].Length;
          string str2 = $"\r\n{str1}end\r\n";
          objects = s.IndexOf(str2, ind);
          if (objects != -1)
          {
            objects += str2.Length;
            ind = objects;
            this.ParseObject(parentList, name, cname, s.Substring(startIndex, objects - startIndex));
          }
        }
      }
    }
    while (startIndex != -1 && objects != -1);
    return objects;
  }

  public void Parse()
  {
    this.objList.Clear();
    this.FindObjects(this.objList, this._formText, 0);
  }
}
