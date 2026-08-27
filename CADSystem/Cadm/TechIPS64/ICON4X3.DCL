icon4x3 : dialog {
        key = "label";
        : row {
            : row {
                : column {
                    : icon_image {  key = "icon1"; width=18;  action="(OnIconClick 1)"; }
                    : icon_image {  key = "icon2"; width=18;  action="(OnIconClick 2)"; }
                    : icon_image {  key = "icon3"; width=18;  action="(OnIconClick 3)"; }
                }
                : column {
                    : icon_image {  key = "icon4"; width=18;  action="(OnIconClick 4)"; }
                    : icon_image {  key = "icon5"; width=18;  action="(OnIconClick 5)"; }
                    : icon_image {  key = "icon6"; width=18;  action="(OnIconClick 6)"; }
                }
                : column {
                    : icon_image {  key = "icon7"; width=18;  action="(OnIconClick 7)"; }
                    : icon_image {  key = "icon8"; width=18;  action="(OnIconClick 8)"; }
                    : icon_image {  key = "icon9"; width=18;  action="(OnIconClick 9)"; }
                }
                : column {
                    : icon_image {  key = "icon10"; width=18; action="(OnIconClick 10)"; }
                    : icon_image {  key = "icon11"; width=18; action="(OnIconClick 11)"; }
                    : icon_image {  key = "icon12"; width=18; action="(OnIconClick 12)"; }
                }
            }
        }
        : boxed_column {
            : text_part {
                key = "descrip";
            }
        }
        spacer;
        : row {
            spacer;
            fixed_width = true;
            : button {
               label = "Предыдущ.";
               mnemonic = "П";
               key = "prev";
               width = 12;
            }
            spacer_1;
            : button {
               label = "Следующ.";
               mnemonic = "С";
               key = "next";
               width = 12;
            }
            : spacer { width=2; }
            : button {
                label = "Выбрать";
                is_default = true;
                key = "accept";
                width = 12;
             }
             spacer_1;
             : button {
                label = "Отмена";
                is_cancel = true;
                key = "cancel";
                width = 12;
             }
             spacer;
        }
}