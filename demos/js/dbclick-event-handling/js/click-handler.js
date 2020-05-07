/*
 * Any copyright is dedicated to the Public Domain.
 * For more information, please refer to http://unlicense.org/
 */

/* eslint-env es2017 */
const ClickWatcher = (function () {
    let doubleClick = false;

    return {
        'publishDoubleClick': () => doubleClick = true,
        'reset': () => doubleClick = false,
        'isDoubleClicked': () => doubleClick
    };
}());

const ClickHandler = (function () {
    const sleep = function (span) {
        return new Promise(onSuccess => {
            setTimeout(onSuccess, span);
        });
    };

    return {
        async 'changeImage' (element) {
            let doubleClickEvent = false;

            await sleep(200).then(() => doubleClickEvent = ClickWatcher.isDoubleClicked());

            if (!doubleClickEvent) {
                if (element.src.match(/.*(orange-1.png)$/u)) {
                    element.src = 'img/orange-2.png';
                    element.title = 'Double-click to put the orange back together!';
                }
            }
        },
        async 'changeBack' (element) {
            ClickWatcher.publishDoubleClick();

            if (!element.src.match(/.*(orange-1.png)$/u)) {
                element.src = 'img/orange-1.png';
                element.title = 'Click to slice the orange!';
                await sleep(200).then(() => ClickWatcher.reset());
            } else {
                await sleep(200).then(() => ClickWatcher.reset());
            }
        }
    };
}());
