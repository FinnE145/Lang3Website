define("ace/theme/l3t",["require","exports","module","ace/lib/dom"], function(require, exports, module) {

    exports.isDark = true;
    exports.cssClass = "ace-l3t";
    exports.cssText = `

    .ace-l3t .ace_gutter {
        background: #434b54;
        color: #99acc1;
    }

    .ace-l3t .ace_print-margin {
        width: 1px;
        background: #434b54;
    }

    .ace-l3t {
        background-color: #32383f;
        color: #FFFFFF;
    }

    .ace-l3t .ace_cursor {
        color: #FFFFFF;
    }

    .ace-l3t .ace_marker-layer .ace_selection {
        background: rgba(89, 96, 116, 0.88);
    }

    .ace-l3t.ace_multiselect .ace_selection.ace_start {
        box-shadow: 0 0 3px 0px #32383f;
    }

    .ace-l3t .ace_marker-layer .ace_step {
        background: rgb(102, 82, 0);
    }

    .ace-l3t .ace_marker-layer .ace_bracket {
        margin: -1px 0 0 -1px;
        border: 1px solid #404040;
    }

    .ace-l3t .ace_marker-layer .ace_active-line {
        background: #3b424a;
    }

    .ace-l3t .ace_gutter-active-line {
        background-color: #3b424a;
    }

    .ace-l3t .ace_marker-layer .ace_selected-word {
        border: 1px solid rgba(90, 100, 126, 0.88);
    }

    .ace-l3t .ace_invisible {
        color: #404040;
    }

    .ace-l3t .ace_keyword{
        color: #ca006f;
    }

    .ace-l3t .ace_constant,
    .ace-l3t .ace_constant.ace_character,
    .ace-l3t .ace_constant.ace_character.ace_escape,
    .ace-l3t .ace_constant.ace_other,
    .ace-l3t .ace_support.ace_constant {
        color: #e3e323;
    }

    .ace-l3t .ace_invalid {
        color: #cc3333;
    }

    .ace-l3t .ace_fold {
        background-color: #cc33bf;
        border-color: #FFFFFF;
    }

    .ace-l3t .ace_support.ace_function,
    .ace-l3t .ace_entity.ace_name.ace_function {
        color: #ade134;
    }

    .ace-l3t .ace_string {
        color: #d8ae60;
    }

    .ace-l3t .ace_string.ace_regexp {
        color: #61cf12;
    }

    .ace-l3t .ace_comment {
        font-style: italic;
        color: #9f9f9f;
    }

    .ace-l3t .ace_meta.ace_tag {
        color: #9c0049;
    }

    .ace-l3t .ace_entity.ace_name.ace_type {
        color: #2775aa;
    }

    .ace-l3t .ace_collab.ace_user1 {
        color: #32383f;
        background-color: #80e8ff;
    }

    .ace-l3t .ace_variable {
        color: #6dc5ff;
    }

    .ace-l3t .ace_indent-guide {
        background: url(data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAEAAAACCAYAAACZgbYnAAAAEklEQVQImWMwMjLyZYiPj/8PAAreAwAI1+g0AAAAAElFTkSuQmCC) right repeat-y
    }

    `;

    var dom = require("../lib/dom");
    dom.importCssString(exports.cssText, exports.cssClass, false);
});

(function() {
    window.require(["ace/theme/l3t"], function(m) {
        if (typeof module == "object" && typeof exports == "object" && module) {
            module.exports = m;
        }
    });
})();