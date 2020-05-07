"use strict";

(function () {
  var targets = Array.prototype.slice.call(document.querySelectorAll('pre'));
  setTimeout(function () {
    targets.forEach(function (item) {
      var innerHTML = item.innerHTML;
      var divElement = document.createElement('div');
      divElement.innerHTML = innerHTML;
      item.innerHTML = '';
      item.appendChild(divElement);
    });
  }, 300);
})();