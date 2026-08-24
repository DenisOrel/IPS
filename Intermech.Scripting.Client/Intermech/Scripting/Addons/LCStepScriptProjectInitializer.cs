// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.Addons.LCStepScriptProjectInitializer
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using Intermech.Scripting.Services;

#nullable disable
namespace Intermech.Scripting.Addons;

internal sealed class LCStepScriptProjectInitializer : DBScriptProjectInitializer
{
  private const string csharpTemplate = "using System;\r\nusing System.Collections.Generic;\r\nusing System.Linq;\r\nusing System.Text;\r\nusing Intermech.Interfaces;\r\n\r\npublic class Script\r\n{\r\n    public ICSharpScriptContext ScriptContext { get; set; }\r\n\r\n    /// <summary>\r\n    /// Метод вызываемый при изменении шага ЖЦ\r\n    /// </summary>\r\n    /// <param name=\"sender\">Объект, шаг ЖЦ которого изменяется</param>\r\n    /// <param name=\"nextstep\">Следующий шаг ЖЦ, если метод вызывается при создании объекта то null</param>\r\n    /// <param name=\"session\">Сессия</param>\r\n    public void Execute(IDBObject sender, IDBLifecycleStep nextstep, IUserSession session)\r\n    {\r\n        //Вставьте ваш код здесь\r\n    }\r\n}\r\n";

  public LCStepScriptProjectInitializer()
    : base("using System;\r\nusing System.Collections.Generic;\r\nusing System.Linq;\r\nusing System.Text;\r\nusing Intermech.Interfaces;\r\n\r\npublic class Script\r\n{\r\n    public ICSharpScriptContext ScriptContext { get; set; }\r\n\r\n    /// <summary>\r\n    /// Метод вызываемый при изменении шага ЖЦ\r\n    /// </summary>\r\n    /// <param name=\"sender\">Объект, шаг ЖЦ которого изменяется</param>\r\n    /// <param name=\"nextstep\">Следующий шаг ЖЦ, если метод вызывается при создании объекта то null</param>\r\n    /// <param name=\"session\">Сессия</param>\r\n    public void Execute(IDBObject sender, IDBLifecycleStep nextstep, IUserSession session)\r\n    {\r\n        //Вставьте ваш код здесь\r\n    }\r\n}\r\n")
  {
  }
}
