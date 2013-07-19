(function ($, undefined) {
    var parent = window.kendo.ui.Menu.renderItem; $.extend(window.kendo.ui.Menu, {
        renderItem: function (options) {
            var r = parent(options); var injects = ""; if (options.item.data) { injects = " data-data='" + JSON.stringify(options.item.data) + "'"; } if (injects.length > 0) {
                var re = /(<li)([^>]*>.*)/;
                var m = re.exec(r);
                r = m[1] + injects + m[2];
            }
            return r;
        }
    });

    var hiding = false;
    var showing = false;
    var MenuEx = window.kendo.ui.Menu.extend({/** @lends kendo.ui.Menu.prototype */        /**         * target object which was clicked         */
        target: {},        /**         * menu item which was clicked         */
        item: {},
        options:
            {
                name: "MenuEx",
                delay: 1000,
                event: 'contextmenu',
                orientation: 'vertical'
            },
        init: function (element, options) {
            var that = this;
            window.kendo.ui.Menu.fn.init.call(that, element, options);
            that.element.addClass('k-context-menu');
            if (options.anchor) {
                event = options.event || that.options.event;
                $(document).ready(function () {
                    $(options.anchor).bind(event, function (e) {
                        that.show(options.anchor, e);
                        return false;
                    });
                });

                this.bind('mouseleave', function () {
                    delay = options.delay || that.options.delay;
                    setTimeout(function () { that.hide() }, delay);
                });
            }
            $(document).click($.proxy(that._documentClick, that));
            var items = that.element.find('.k-item');
            $.each(options.dataSource, function (i, el) {
                if (el.click != undefined) {
                    jQuery(items[i]).click(function (e) {
                        //el.click.call($(e.target).parents('li'), e); 
                        that.item = $(e.target).parents('li');
                        el.click.call(that, e);
                    });
                }
            });
        },
        hide: function () {
            if (showing) {
                hiding = true;
                var $target = $(this.target);
                if ($target.hasClass('k-item')) {
                    $target.find('.k-in').removeClass('k-state-focused');
                }
                this.element.fadeOut(function () {
                    hiding = false;
                    showing = false;
                });
            }
        },

        show:
            function (anchor, e) {
                if (hiding == false) {
                    this.target = e.currentTarget; var $target = $(this.target);
                    if ($target.hasClass('k-item')) { $target.find('.k-in').addClass('k-state-focused'); }
                    this.element.css({ 'top': e.pageY, 'left': e.pageX }); this.element.fadeIn(function ()
                    { showing = true; });
                }
            }, _documentClick: function (e) { var that = this; that.hide(); }
    }); window.kendo.ui.plugin(MenuEx);
})(jQuery);