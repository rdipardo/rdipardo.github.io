---
layout: post
title: Saluting the (Midnight) Commander
summary: A tribute to <strong>mcedit</strong>
categories: blog
published: 2020-06-28 20:38 -0400
updated: 2021-03-12 13:26 -0500
---

My everyday workstation currently runs [BunsenLabs (Lithium)](https://www.bunsenlabs.org/index.html).
With Debian stable at its core, this snappy little distro lets me assign my preferred text editor to the *edit* action of the [**run-mailcap**](https://manpages.debian.org/buster/mime-support/run-mailcap.1.en.html) utility. At the moment, my choices are the following:

```shell
$ select-editor

Select an editor.  To change later, run 'select-editor'.
  1. /bin/nano        <---- easiest
  2. /usr/bin/code
  3. /usr/bin/emacs25
  4. /usr/bin/mcedit
  5. /usr/bin/vim.basic
  6. /usr/bin/vim.tiny

Choose 1-6 [1]:
```

There was a time, not long ago, when my first choice would have been #4 on the above list, namely, [Midnight Commander](https://midnight-commander.org)'s built-in editor, **mcedit**. As an absolute beginner, I appreciated the curses menus and dialog boxes. I especially liked having a numbered function-key menu at the bottom of the screen. You may recognize this interface if you've ever used **htop**:

![screen](/images/posts/2020.06.28/midnight-commander-mcedit.png)


---


More recently, I've gotten used to **vim**. In fact, it's the current target of my *edit* symlink. But I'm still grateful to **mcedit** for providing a soft introduction to the world of terminal-based editors.

Here are some custom mappings from my *.vimrc* file, inspired by **mcedit**'s function-key menu.

*To save the current buffer:*

```vim
nnoremap &lt;F2&gt; :w %&lt;CR&gt;
```

This next mapping is my invention, because I'm not sure what it's supposed to emulate. According to  [**mc**'s maintainers](https://midnight-commander.org/wiki/doc/editor/hotkeys), the default action of `F3` is moving the cursor to the *start* or (when `SHIFT`ed up) the *end* of a line. In [my distro's default version](https://packages.debian.org/buster/mc) (4.8.22), however, `F3` simply toggles the open file's byte order mark. Let's put it to better use!

*To list all open buffers:*

```vim
nnoremap &lt;F3&gt; :ls&lt;CR&gt;
```

*To copy the line under the cursor:*

```vim
nnoremap &lt;F5&gt; vVy
```

*. . . and paste it after the cursor:*

```vim
nnoremap &lt;F6&gt; p
```

*To delete the line under the cursor:*

```vim
nnoremap &lt;F8&gt; dd
```

*. . . or, to delete the selected text:*

```vim
vnoremap &lt;F8&gt; x
```

*To save all buffers and exit:*

```vim
nnoremap &lt;silent&gt; &lt;F10&gt; :set awa&lt;CR&gt;:qa&lt;CR&gt;
```

Happy editing!

---
**Note**

*In case you were wondering, option #2 in the output of **select-editor** is Microsoft's [VS Code](https://code.visualstudio.com/docs/setup/linux), which I keep around mainly for the convenience of its git integration.*
