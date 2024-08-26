/* DO NOT EDIT HERE! EDIT `src/mode-lang3.js` INSTEAD! */

"use strict";

var oop = require("../lib/oop");
var TextMode = require("./text").Mode;
var lang3HighlightRules = require("./lang3_highlight_rules").lang3HighlightRules;
// TODO: pick appropriate fold mode
var FoldMode = require("./folding/cstyle").FoldMode;

var Mode = function() {
    this.HighlightRules = lang3HighlightRules;
    this.foldingRules = new FoldMode();
};
oop.inherits(Mode, TextMode);

(function() {
    this.lineCommentStart = /#/;
    this.blockComment = {start: /#{2,}/, end: /#{2,}/};
    // Extra logic goes here.
    this.$id = "ace/mode/lang3";
}).call(Mode.prototype);

exports.Mode = Mode;