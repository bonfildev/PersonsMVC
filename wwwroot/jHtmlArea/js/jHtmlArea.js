﻿/*
* jHtmlArea - WYSIWYG Html Editor jQuery Plugin
* Copyright (c) 2013-2023 Chris Pietschmann
* https://github.com/crpietschmann/jHtmlArea
* MIT License
* https://github.com/crpietschmann/jHtmlArea/license
*/
(function ($, window) {

    var $jhtmlarea = window.$jhtmlarea = {};
    var $browser = $jhtmlarea.browser = {};
    (function () {
        $browser.msie = false;
        $browser.mozilla = false;
        $browser.safari = false;
        $browser.version = 0;
        
        if (navigator.userAgent.match(/MSIE ([0-9]+)\./)) {
            $browser.msie = true;
            $browser.version = parseFloat(RegExp.$1);
        } else if (navigator.userAgent.match(/Trident\/([0-9]+)\./)) {
            $browser.msie = true;
            $browser.version = RegExp.$1;
            if (navigator.userAgent.match(/rv:([0-9]+)\./)) {
                $browser.version = parseFloat(RegExp.$1);
            }
        }
        if (navigator.userAgent.match(/Mozilla\/([0-9]+)\./)) {
            $browser.mozilla = true;
            if ($browser.version === 0) {
                $browser.version = parseFloat(RegExp.$1);
            }
        }
        if (navigator.userAgent.match(/Safari ([0-9]+)\./)) {
            $browser.safari = true;
            $browser.version = RegExp.$1;
            if (navigator.userAgent.match(/Version\/([0-9]+)\./)) {
                if ($browser.version === 0) {
                    $browser.version = parseFloat(RegExp.$1);
                }
            }
        }
    })();

    $.fn.htmlarea = function (opts) {
        if (opts && typeof (opts) === "string") {
            var args = [];
            for (var i = 1; i < arguments.length; i++) { args.push(arguments[i]); }
            var htmlarea = jHtmlArea(this[0]);
            var f = htmlarea[opts];
            if (f) { return f.apply(htmlarea, args); }
        }
        return this.each(function () { jHtmlArea(this, opts); });
    };
    var jHtmlArea = window.jHtmlArea = function (elem, options) {
        if (elem.jquery) {
            return jHtmlArea(elem[0]);
        }
        if (elem.jhtmlareaObject) {
            return elem.jhtmlareaObject;
        } else {
            return new jHtmlArea.fn.init(elem, options);
        }
    };
    jHtmlArea.fn = jHtmlArea.prototype = {

        // The current version of jHtmlArea being used
        jhtmlarea: "1.0",

        init: function (elem, options) {
            if (elem.nodeName.toLowerCase() === "textarea") {
                var opts = $.extend({}, jHtmlArea.defaultOptions, options);
                elem.jhtmlareaObject = this;

                var textarea = this.textarea = $(elem);
                var container = this.container = $("<div/>").addClass("jHtmlArea").width(textarea.width()).insertAfter(textarea);

                var toolbar = this.toolbar = $("<div/>").addClass("ToolBar").appendTo(container);
                priv.initToolBar.call(this, opts);

                var iframe = this.iframe = $("<iframe/>");

                var htmlarea = this.htmlarea = $("<div/>").addClass("Body").append(iframe);

                container.append(htmlarea).append(textarea.hide());

                priv.initEditor.call(this, opts);
                priv.attachEditorEvents.call(this);

                // Fix total height to match TextArea
                //htmlarea.height(iframe.height() - toolbar.height())                

                if (opts.loaded) { opts.loaded.call(this); }
            }
        },
        dispose: function () {
            this.textarea.show().insertAfter(this.container);
            this.container.remove();
            this.textarea[0].jhtmlareaObject = null;
        },
        execCommand: function (a, b, c) {
            this.iframe[0].contentWindow.focus();
            
            if ($browser.msie === true && $browser.version >= 11) {
                if (this.previousRange) {
                    var rng = this.previousRange;
                    var sel = this.getSelection()
                    sel.removeAllRanges();
                    sel.addRange(rng);
                }
            }
            
            this.editor.execCommand(a, b || false, c || null);
            this.updateTextArea();
        },
        ec: function (a, b, c) {
            this.execCommand(a, b, c);
        },
        queryCommandValue: function (a) {
            this.iframe[0].contentWindow.focus();
            return this.editor.queryCommandValue(a);
        },
        qc: function (a) {
            return this.queryCommandValue(a);
        },
        getSelectedHTML: function () {
            if ($browser.msie) {
                return this.getRange().htmlText;
            } else {
                var elem = this.getRange().cloneContents();
                return $("<p/>").append($(elem)).html();
            }
        },
        getSelection: function () {
            if ($browser.msie === true && $browser.version < 11) {
                //return (this.editor.parentWindow.getSelection) ? this.editor.parentWindow.getSelection() : this.editor.selection;
                return this.editor.selection;
            } else {
                return this.iframe[0].contentDocument.defaultView.getSelection();
            }
        },
        getRange: function () {
            var s = this.getSelection();
            if (!s) { return null; }
            if (s.getRangeAt) {
                if (s.rangeCount > 0) {
                    return s.getRangeAt(0);
                }
            }
            if (s.createRange) {
                return s.createRange();
            }
            return null;
            //return (s.getRangeAt) ? s.getRangeAt(0) : s.createRange();
        },
        html: function (v) {
            if (v !== undefined) {
                this.textarea.val(v);
                this.updateHtmlArea();
            } else {
                return this.toHtmlString();
            }
        },
        pasteHTML: function (html) {
            this.iframe[0].contentWindow.focus();
            var r = this.getRange();
            if ($browser.msie) {
                r.pasteHTML(html);
            } else if ($browser.mozilla) {
                r.deleteContents();
                r.insertNode($((html.indexOf("<") != 0) ? $("<span/>").append(html) : html)[0]);
            } else { // Safari
                r.deleteContents();
                r.insertNode($(this.iframe[0].contentWindow.document.createElement("span")).append($((html.indexOf("<") != 0) ? "<span>" + html + "</span>" : html))[0]);
            }
            r.collapse(false);
        },
        cut: function () {
            this.ec("cut");
        },
        copy: function () {
            this.ec("copy");
        },
        paste: function () {
            var that = this;
            navigator.clipboard.readText().then((clipText) => {
                that.pasteHTML(clipText);
            });
        },
        bold: function () { this.ec("bold"); },
        italic: function () { this.ec("italic"); },
        underline: function () { this.ec("underline"); },
        strikeThrough: function () { this.ec("strikethrough"); },
        image: function (url) {
            if ($browser.msie === true && !url) {
                this.ec("insertImage", true);
            } else {
                this.ec("insertImage", false, (url || prompt("Image URL:", "http://")));
            }
        },
        removeFormat: function () {
            this.ec("removeFormat", false, []);
            this.unlink();
        },
        link: function () {
            if ($browser.msie === true) {
                this.ec("createLink", true);
            } else {
                this.ec("createLink", false, prompt("Link URL:", "http://"));
            }
        },
        unlink: function () { this.ec("unlink", false, []); },
        orderedList: function () { this.ec("insertorderedlist"); },
        unorderedList: function () { this.ec("insertunorderedlist"); },
        superscript: function () { this.ec("superscript"); },
        subscript: function () { this.ec("subscript"); },

        p: function () {
            this.formatBlock("<p>");
        },
        h1: function () {
            this.heading(1);
        },
        h2: function () {
            this.heading(2);
        },
        h3: function () {
            this.heading(3);
        },
        h4: function () {
            this.heading(4);
        },
        h5: function () {
            this.heading(5);
        },
        h6: function () {
            this.heading(6);
        },
        heading: function (h) {
            this.formatBlock($browser.msie === true ? "Heading " + h : "h" + h);
        },

        indent: function () {
            this.ec("indent");
        },
        outdent: function () {
            this.ec("outdent");
        },

        insertHorizontalRule: function () {
            this.ec("insertHorizontalRule", false, "ht");
        },

        justifyLeft: function () {
            this.ec("justifyLeft");
        },
        justifyCenter: function () {
            this.ec("justifyCenter");
        },
        justifyRight: function () {
            this.ec("justifyRight");
        },

        increaseFontSize: function () {
            if ($browser.msie === true) {
                this.ec("fontSize", false, this.qc("fontSize") + 1);
            } else if ($browser.safari) {
                this.getRange().surroundContents($(this.iframe[0].contentWindow.document.createElement("span")).css("font-size", "larger")[0]);
            } else {
                this.ec("increaseFontSize", false, "big");
            }
        },
        decreaseFontSize: function () {
            if ($browser.msie === true) {
                this.ec("fontSize", false, this.qc("fontSize") - 1);
            } else if ($browser.safari) {
                this.getRange().surroundContents($(this.iframe[0].contentWindow.document.createElement("span")).css("font-size", "smaller")[0]);
            } else {
                this.ec("decreaseFontSize", false, "small");
            }
        },

        forecolor: function (c) {
            this.ec("foreColor", false, c !== undefined ? c : prompt("Enter HTML Color:", "#"));
        },

        formatBlock: function (v) {
            this.ec("formatblock", false, v || null);
        },

        showHTMLView: function () {
            this.updateTextArea();
            this.textarea.show();
            this.htmlarea.hide();
            $("ul li:not(li:has(a.html))", this.toolbar).hide();
            $("ul:not(:has(:visible))", this.toolbar).hide();
            $("ul li a.html", this.toolbar).addClass("highlighted");
        },
        hideHTMLView: function () {
            this.updateHtmlArea();
            this.textarea.hide();
            this.htmlarea.show();
            $("ul", this.toolbar).show();
            $("ul li", this.toolbar).show().find("a.html").removeClass("highlighted");
        },
        toggleHTMLView: function () {
            (this.textarea.is(":hidden")) ? this.showHTMLView() : this.hideHTMLView();
        },

        toHtmlString: function () {
            return this.editor.body.innerHTML;
        },
        toString: function () {
            return this.editor.body.innerText;
        },

        updateTextArea: function () {
            this.textarea.val(this.toHtmlString());
        },
        updateHtmlArea: function () {
            this.editor.body.innerHTML = this.textarea.val();
        }
    };
    jHtmlArea.fn.init.prototype = jHtmlArea.fn;

    jHtmlArea.defaultOptions = {
        toolbar: [
        ["html"], ["bold", "italic", "underline", "strikethrough", "|", "subscript", "superscript"],
        ["increasefontsize", "decreasefontsize"],
        ["orderedlist", "unorderedlist"],
        ["indent", "outdent"],
        ["justifyleft", "justifycenter", "justifyright"],
        ["link", "unlink", "image", "horizontalrule"],
        ["p", "h1", "h2", "h3", "h4", "h5", "h6"],
        ["cut", "copy", "paste"]
    ],
        css: null,
        toolbarText: {
            bold: "Bold", italic: "Italic", underline: "Underline", strikethrough: "Strike-Through",
            cut: "Cut", copy: "Copy", paste: "Paste",
            h1: "Heading 1", h2: "Heading 2", h3: "Heading 3", h4: "Heading 4", h5: "Heading 5", h6: "Heading 6", p: "Paragraph",
            indent: "Indent", outdent: "Outdent", horizontalrule: "Insert Horizontal Rule",
            justifyleft: "Left Justify", justifycenter: "Center Justify", justifyright: "Right Justify",
            increasefontsize: "Increase Font Size", decreasefontsize: "Decrease Font Size", forecolor: "Text Color",
            link: "Insert Link", unlink: "Remove Link", image: "Insert Image",
            orderedlist: "Insert Ordered List", unorderedlist: "Insert Unordered List",
            subscript: "Subscript", superscript: "Superscript",
            html: "Show/Hide HTML Source View"
        }
    };
    var priv = {
        toolbarButtons: {
            strikethrough: "strikeThrough", orderedlist: "orderedList", unorderedlist: "unorderedList",
            horizontalrule: "insertHorizontalRule",
            justifyleft: "justifyLeft", justifycenter: "justifyCenter", justifyright: "justifyRight",
            increasefontsize: "increaseFontSize", decreasefontsize: "decreaseFontSize",
            html: function (btn) {
                this.toggleHTMLView();
            }
        },
        initEditor: function (options) {
            var edit = this.editor = this.iframe[0].contentWindow.document;
            edit.designMode = 'on';
            edit.open();
            edit.write(this.textarea.val());
            edit.close();
            if (options.css) {
                var e = edit.createElement('link'); e.rel = 'stylesheet'; e.type = 'text/css'; e.href = options.css; edit.getElementsByTagName('head')[0].appendChild(e);
            }
        },
        initToolBar: function (options) {
            var that = this;

            var menuItem = function (className, altText, action) {
                return $("<li/>").append($("<a href='javascript:void(0);'/>").addClass(className).attr("title", altText).click(function () { action.call(that, $(this)); }));
            };

            function addButtons(arr) {
                var ul = $("<ul/>").appendTo(that.toolbar);
                for (var i = 0; i < arr.length; i++) {
                    var e = arr[i];
                    if ((typeof (e)).toLowerCase() === "string") {
                        if (e === "|") {
                            ul.append($('<li class="separator"/>'));
                        } else {
                            var f = (function (e) {
                                // If button name exists in priv.toolbarButtons then call the "method" defined there, otherwise call the method with the same name
                                var m = priv.toolbarButtons[e] || e;
                                if ((typeof (m)).toLowerCase() === "function") {
                                    return function (btn) { m.call(this, btn); };
                                } else {
                                    return function () { this[m](); this.editor.body.focus(); };
                                }
                            })(e.toLowerCase());
                            var t = options.toolbarText[e.toLowerCase()];
                            ul.append(menuItem(e.toLowerCase(), t || e, f));
                        }
                    } else {
                        ul.append(menuItem(e.css, e.text, e.action));
                    }
                }
            };
            if (options.toolbar.length !== 0 && priv.isArray(options.toolbar[0])) {
                for (var i = 0; i < options.toolbar.length; i++) {
                    addButtons(options.toolbar[i]);
                }
            } else {
                addButtons(options.toolbar);
            }
        },
        attachEditorEvents: function () {
            var t = this;

            var fnHA = function () {
                t.updateHtmlArea();
            };

            this.textarea.click(fnHA).
                keyup(fnHA).
                keydown(fnHA).
                mousedown(fnHA).
                blur(fnHA);

            this.iframe.blur(function () {
                t.previousRange = t.getRange();
            });

            var fnTA = function () {
                t.updateTextArea();
            };

            $(this.editor.body).click(fnTA).
                keyup(fnTA).
                keydown(fnTA).
                mousedown(fnTA).
                blur(fnTA);

            $('form').submit(function () { t.toggleHTMLView(); t.toggleHTMLView(); });
            //$(this.textarea[0].form).submit(function() { //this.textarea.closest("form").submit(function() {


            // Fix for ASP.NET Postback Model
            if (window.__doPostBack) {
                var old__doPostBack = __doPostBack;
                window.__doPostBack = function () {
                    if (t) {
                        if (t.toggleHTMLView) {
                            t.toggleHTMLView();
                            t.toggleHTMLView();
                        }
                    }
                    return old__doPostBack.apply(window, arguments);
                };
            }

        },
        isArray: function (v) {
            return v && typeof v === 'object' && typeof v.length === 'number' && typeof v.splice === 'function' && !(v.propertyIsEnumerable('length'));
        }
    };
})(jQuery, window);