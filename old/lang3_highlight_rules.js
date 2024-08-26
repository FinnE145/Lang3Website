/* DO NOT EDIT HERE! EDIT `src/mode-lang3.js` INSTEAD! */

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
                regex: /(?:[a-zA-Z_]\w*(?=\s+[a-zA-Z_]\w*\s*=))/
            },
            {
                token: "variable",
                regex: /[a-zA-Z_]\w*/
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