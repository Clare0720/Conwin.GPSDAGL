/**
 * Created by caihao on 17/2/22.
 */
define(['textbox', 'textarea', 'radio', 'checkbox', 'dropdown', 'button'],
    function (textbox, textarea, radiobox, checkbox, dropdown, button) {
        var control = {};
        control.initial = function () {
            textbox.initial(); ///文本框
            textarea.initial();///多文本
            dropdown.initial();///下拉框
            radiobox.initial(); ///单选框
            checkbox.initial(); ///多选框
        },
            control.buttonValid = function (options) {
                return button.initial(options);
            }
        return control;
    });