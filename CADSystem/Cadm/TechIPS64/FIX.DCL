fix_mnu : dialog {
  key = "fix_mnu";
  : boxed_column {
     label = " Параметры устройства ";  
     :toggle {
       label = "Рифленая рабочая &поверхность";  
       mnemonic = "п";
       key = "rifl";
     }
     : popup_list {
        label = "Рабочая поверхность :";  
        key =  "shape";
        mnemonic = "Р";
 list = /*MSG261*/"\nНет\nПлоская\nСферическая\nЦилиндрическая\nПризматическая\nКоническая\nРомбическая\nТрехгранная";
        edit_width = 18;
        value = "0";
     }
     : popup_list {
        label = "Устройство зажима :";  
        key =  "fix_type";
        mnemonic = "У";
  list = /*MSG261*/"\nПневматическое\nПрочее\nГидравлическое\nЭлектрическое\nМагнитное\nЭлектромагнитное";
        edit_width = 18;
        value = "1";
     }
     : edit_box{
        label = "Масштаб   значка :";
        key = "scale";
        mnemonic = "М";
        edit_width = 19;
        value = "1.0";
     }
     : spacer{ height=0.0; }
  }
  spacer_1;
  ok_cancel;
  errtile;
}
