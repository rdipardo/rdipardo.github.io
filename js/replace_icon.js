(function () {
    function replaceIcon() {
        const iconField = document.querySelector('a.navbar-item.navicon')
        const text = "View on GitHub"
        iconField.classList =
            Array.prototype.slice.call(iconField.classList)
                .filter(c => c !== 'navicon')
        iconField.firstElementChild.classList = []
        iconField.firstElementChild.innerHTML = text
    }

    const fa = document.createElement('link')
    fa.href = 'https://maxcdn.bootstrapcdn.com/font-awesome/4.7.0/css/font-awesome.min.css'
    fa.rel = 'stylesheet'
    fa.type = 'text/css'
    fa.media = 'screen'
    fa.onerror = replaceIcon
    const style = document.getElementsByTagName('link')[0]
    style.parentNode.insertBefore(fa, style)
})();
