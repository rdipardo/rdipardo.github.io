---
layout: post
title: Single or Double? (The Conclusion)
summary: Click-event handling in JavaScript - part 2
categories: tutorials
published: 2020-07-24 21:51 -0400
---

In a [previous post][cross-ref], we saw that single-threaded execution makes it hard to distinguish a genuine *double-click* event from two, separate *click* events.

Given this problem, could we ever set two different event handlers on the same HTML element, one just for *clicks*, the other just for *double-click*s?

### A (potential) solution
Here is a chance to leverage JavaScript's asynchronous features. Developers targeting [browser's that support asynchronous functions][browsers-with-async-fns] can delegate event-listening to a separate thread. Code running on this thread can inform the main thread about the type of event detected. The target thread can then decide what action to take.

With this in mind, here's one approach to solving our event handling problem:

```javascript
/**
 * Returns a Promise that will call `window.setTimeout()` when resolved;
 * `onSuccess` will execute when the timer runs out; the time limit will be the
 * value of `span`.
 */
function sleep(span) {
    return new Promise(function (onSuccess) {
        window.setTimeout(onSuccess, span);
    });
}

/**
 * Runs on a separate thread (`async`); can be paused by functions qualified
 * with `await`.
 */
async function changeElement(node) {
    let doubleClickEvent = false;

    // the callback executed by `setTimeout()`
    await sleep(200).then(() => doubleClickEvent = ClickWatcher.isDoubleClicked());

    if (!doubleClickEvent) {
        // handle a single-click event
    }
}

/**
 * Also runs on a separate thread (`async`).
 */
async function changeBack(element) {
    ClickWatcher.publishDoubleClick(); // see below

    if ( /* something changed? */ ) {

        // handle a double-click event

        // now forget the event
        await sleep(200).then(() => ClickWatcher.reset());
    } else {
        // this will cover us in case the user doesn't
        // click even once before double-clicking;
        // nothing was changed, so simply void this event
        await sleep(200).then(() => ClickWatcher.reset());
    }
}

/**
 * Finally, we need a function that can “remember” the last-assigned
 * value of a local variable; let's avoid a global variable and declare a
 * closure!
 */
const ClickWatcher = (function () {
    // the value of this variable will persist between function calls
    let doubleClick = false;

    return {
        // set our local variable
        publishDoubleClick: () => doubleClick = true,

        // return the value of our local variable
        isDoubleClicked: () => doubleClick,

        reset: () => doubleClick = false
    };
})();
```

If you have a laptop and a mouse handy, you can try out [**this quick demonstration**](/demos/js/dbclick-event-handling/) to see if it works as expected.


#### Caveat
Since this topic only deals with mouse events, it should go without saying that only websites intended for _**desktop**_ viewing can benefit from the above solution!

---

##### Acknowledgements

The `sleep()` function used above was devised by [Dan Dascalescu](https://stackoverflow.com/users/1269037/dan-dascalescu) in [this posting][js_sleep_fn].


##### See also

- [Async Function Definitions][async-fns-spec] (ECMA-262, 8th edition, June 2017)
- Bevacqua, N. (2016), [“Understanding JavaScript’s async await”][async-fns-dev-perspective] [blog]

[browsers-with-async-fns]: https://caniuse.com/#feat=async-functions
[async-fns-spec]: https://www.ecma-international.org/ecma-262/8.0/#sec-async-function-definitions
[async-fns-dev-perspective]: https://ponyfoo.com/articles/understanding-javascript-async-await
[js_sleep_fn]: https://stackoverflow.com/a/39914235
[cross-ref]: /posts/2020-05-18-click-evt-handling-in-js-part-1.html#main-content
