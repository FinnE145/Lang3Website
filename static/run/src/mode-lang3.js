define("ace/mode/lang3_highlight_rules",["require","exports","module","ace/lib/oop","ace/mode/text_highlight_rules"], function(require, exports, module) {
    "use strict";

    var oop = require("../lib/oop");
    var TextHighlightRules = require("./text_highlight_rules").TextHighlightRules;

    var lang3HighlightRules = function() {
        // regexp must not have capturing parentheses. Use (?:) instead.
        // regexps are ordered -> the first match is used

        this.$rules = {
            start: [
                {
                    token: "text",
                    regex: /^$/
                },
                {
                    include: "#all"
                },
                {
                    defaultToken: "text",
                }
            ],
            "#all": [
                {include: "blockcomment"},
                {include: "linecomment"},
                {include: "string"},
                {include: "num"},
                {include: "codeblock"},
                {include: "words"}
            ],
            
            "blockcomment": [
                {
                    token: "comment.block",
                    start: /#{2,}/,
                    end: /#{2,}/,
                    next: [{include: "inComment"}]
                }
            ],
            
            "linecomment": [
                {
                    token: "comment.line.number-sign",
                    start: /#/,
                    end: /$|\n/,
                    next: [{include: "inComment"}]
                }
            ],
            "inComment": [
                {
                    token: "constant.character.escape",
                    regex: /(?:\\:)|(\b:)/
                },
                {
                    token: "meta.code",
                    start: /\B:/,
                    end: /;/,
                    next: [{include: "inCodeblock"}]
                },
                {
                    token: "invalid.illegal",
                    regex: /![^\n;#!]*!?/
                }
            ],
            
            "string": [
                {
                    token: "string",
                    start: /"/,
                    end: /"/,
                    next: [{include: "inString"}]
                }
            ],
            "inString": [
                {
                    token: "constant.character.escape",
                    regex: /\\./
                }
            ],
            
            "num": [
                {
                    token: "constant.numeric",
                    regex: /\b\d+(?:\.\d+)?\b/
                }
            ],
            
            "codeblock": [
                {
                    token: "meta.code",
                    start: /:/,
                    end: /;/,
                    next: [{include: "inCodeblock"}]
                }
            ],
            "inCodeblock": [
                {
                    token: "constant.character.escape",
                    regex: /\\;/
                },
                {include: "#all"}
            ],
            
            "words": [
                {
                    token: ["keyword.operator", "text", "entity.name.type"],
                    regex: /\b(as)(\s+)([a-zA-Z_]\w*)\b/
                },
                {
                    token: ["keyword.control", "keyword.operator"],
                    regex: /\b(?:(if|else|while|for)|(as|in|is))\b/
                },
                {
                    token: "entity.name.type",
                    regex: /[a-zA-Z_]\w*(?=\s+[a-zA-Z_]\w*\s*(?:=|,|\]))/
                },
                //{include: "subtype"},
                {
                    token: ["entity.name.type", "paren.lparen"],
                    regex: /([a-zA-Z_]\w*)(\[)(?=[\w\[\]]*\])/,
                    push: "subtype"
                },
                {
                    token: "entity.name.function",
                    regex: /[a-zA-Z_]\w*(?=\()/
                },
                {
                    token: "variable",
                    regex: /[a-zA-Z_]\w*/
                }
            ],

            "subtype": [
                {
                    token: "entity.name.type",
                    regex: /[a-zA-Z_]\w*(?=\[[\w\[\]]*\])/
                },
                {
                    token: "entity.name.type",
                    regex: /[a-zA-Z_]\w*/
                },
                {
                    token: "paren.lparen",
                    regex: /\[/,
                    push: "subtype"
                },
                {
                    token: "paren.rparen",
                    regex: /\]/,
                    next: "pop"
                }
            ]
        }
        
        this.normalizeRules();
    };

    lang3HighlightRules.metaData = {
        "$schema": "https://raw.githubusercontent.com/martinring/tmlanguage/master/tmlanguage.json",
        scopeName: "source.lang3",
        name: "lang3"
    }


    oop.inherits(lang3HighlightRules, TextHighlightRules);

    exports.lang3HighlightRules = lang3HighlightRules;
});

define("ace/mode/lang3",["require","exports","module","ace/lib/oop","ace/mode/text","ace/mode/lang3_highlight_rules","ace/mode/folding/pythonic","ace/range"], function(require, exports, module) {
    "use strict";
    
    var oop = require("../lib/oop");
    var TextMode = require("./text").Mode;
    var lang3HighlightRules = require("./lang3_highlight_rules").lang3HighlightRules;
    //var PythonFoldMode = require("./folding/pythonic").FoldMode;
    var Range = require("../range").Range;
    
    var Mode = function() {
        this.HighlightRules = lang3HighlightRules;
        //this.foldingRules = new PythonFoldMode("\\:");
        this.$behaviour = this.$defaultBehaviour;
    };
    oop.inherits(Mode, TextMode);
    
    (function() {
    
        this.lineCommentStart = "#";
    
        this.getNextLineIndent = function(state, line, tab) {
            var indent = this.$getIndent(line);
    
            var tokenizedLine = this.getTokenizer().getLineTokens(line, state);
            var tokens = tokenizedLine.tokens;
    
            if (tokens.length && tokens[tokens.length-1].type == "comment") {
                return indent;
            }
    
            if (state == "start") {
                var match = line.match(/^.*[\{\(\[:]\s*$/);
                if (match) {
                    indent += tab;
                }
            }
    
            return indent;
        };
    
        var outdents = {
            "pass": 1,
            "return": 1,
            "raise": 1,
            "break": 1,
            "continue": 1
        };
        
        this.checkOutdent = function(state, line, input) {
            if (input !== "\r\n" && input !== "\r" && input !== "\n")
                return false;
    
            var tokens = this.getTokenizer().getLineTokens(line.trim(), state).tokens;
            
            if (!tokens)
                return false;
            do {
                var last = tokens.pop();
            } while (last && (last.type == "comment" || (last.type == "text" && last.value.match(/^\s+$/))));
            
            if (!last)
                return false;
            
            return (last.type == "keyword" && outdents[last.value]);
        };
    
        this.autoOutdent = function(state, doc, row) {
            
            row += 1;
            var indent = this.$getIndent(doc.getLine(row));
            var tab = doc.getTabString();
            if (indent.slice(-tab.length) == tab)
                doc.remove(new Range(row, indent.length-tab.length, row, indent.length));
        };
    
        this.$id = "ace/mode/lang3";
        //this.snippetFileId = "ace/snippets/python";
    }).call(Mode.prototype);
    
    exports.Mode = Mode;
});

(function() {
    window.require(["ace/mode/lang3"], function(m) {
        if (typeof module == "object" && typeof exports == "object" && module) {
            module.exports = m;
        }
    });
})();