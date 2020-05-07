---
layout: post
title: Single or Double?
excerpt: Click-event handling in JavaScript - part 1
excerpt_separator: "<!--end-brief-->"
categories: tutorials
date: 2020-05-18 19:40 -0400
---

In this tutorial, I address the challenge of accurately responding to *double-click* events in JavaScript.

### Problem
Assign two click-event handlers to the same HTML element, one for single *click*s, one for *double-click*s. The effects of one handler **should not** overlap with the effects of the second. In other words, the browser should know a single *click* from a *double-click*, and execute **only one** callback based on that input.

### Obstacle
As far as browsers are concerned, a *double-click* is simply two *click*s that happen within a predetermined time interval. Even before the second *click* is registered, the event is handled like an ordinary single *click*. The time interval can vary across browsers, but the general sequence of events looks like this:

```
click! -> single-click event -> fire `onclick` handler -> click! -> double-click event -> fire `ondblclick` handler

0 ms ……........................….. 100 ms ……......................….. 200 ms  ……......................….. 300 ms ……................................…...
```

The source of the problem is JavaScript's built-in tendency for synchronous execution. Remember that, by design,

> [JavaScript is a single threaded language](https://dev.to/steelvoltage/if-javascript-is-single-threaded-how-is-it-asynchronous-56gd).

This means our *double-click* handler is blocked until the preceding *click* handler has run its course.

Suppose we wrapped the latter in a call to `setTimeout()`, hoping to “wait out” the first event? No good: **the same thread is used to listen for, and execute, both events!** When the timeout expires, the browser will just resume the normal chain of events illustrated above.

Now suppose our second event listener could do its job on a separate thread? Stay tuned to find out how . . .
