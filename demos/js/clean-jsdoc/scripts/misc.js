"use strict";

function copy(value) {
  var el = document.createElement('textarea');
  el.value = value;
  document.body.appendChild(el);
  el.select();
  document.execCommand('copy');
  document.body.removeChild(el);
}

function showTooltip(id) {
  var tooltip = document.getElementById(id);
  tooltip.classList.add('show-tooltip');
  setTimeout(function () {
    tooltip.classList.remove('show-tooltip');
  }, 3000);
}
/* eslint-disable-next-line */


function copyFunction(id) {
  // Selecting the pre element
  var code = document.getElementById(id); // Selecting the code element of that pre element

  code = code.childNodes[0].childNodes[0].innerText; // Copy

  copy(code); // Show tooltip

  showTooltip("tooltip-".concat(id));
}

(function () {
  // Capturing all pre element on the page
  var allPre = document.getElementsByTagName('pre');

  for (var i = 0; i < allPre.length; i++) {
    // Get the list of class in current pre element
    var classList = allPre[i].classList;
    var id = "pre-id-".concat(i); // Tooltip

    var tooltip = "<div class=\"tooltip\" id=\"tooltip-".concat(id, "\">Copied!</div>"); // Template of copy to clipboard icon container

    var copyToClipboard = "<div class=\"code-copy-icon-container\" onclick=\"copyFunction('".concat(id, "')\"><div><svg class=\"sm-icon\" alt=\"click to copy\"><use xlink:href=\"#copy-icon\"></use></svg>").concat(tooltip, "<div></div>"); // Extract the code language

    var langName = classList && classList.length ? classList[classList.length - 1].split('-')[1] || 'javascript' : '';
    var langNameDiv = "<div class=\"code-lang-name-container\"><div class=\"code-lang-name\">".concat(langName.toLocaleUpperCase(), "</div></div>"); // Appending everything to the current pre element

    allPre[i].innerHTML += langNameDiv + (langName.length ? copyToClipboard : '');
    allPre[i].setAttribute('id', id);
  }
})();