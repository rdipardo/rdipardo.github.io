"use strict";

(function () {
  var source = document.getElementsByClassName('prettyprint source linenums');
  var i = 0;
  var lineNumber = 0;
  var lineId = 0;
  var lines = [];
  var totalLines = 0;
  var anchorHash = '#';

  if (source && source[0]) {
    anchorHash = document.location.hash.substring(1);
    lines = source[0].getElementsByTagName('li');
    totalLines = lines.length;

    for (; i < totalLines; i++) {
      lineNumber++;
      lineId = "line".concat(lineNumber);
      lines[i].id = lineId;

      if (lineId === anchorHash) {
        lines[i].className += ' selected';
      }
    }
  }
})();