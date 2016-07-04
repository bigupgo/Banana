(function () {
    var iconSelect = function (options) {
        var _this = this;
        var iconCls = {
            'text-decoration': 'none',
            'cursor': 'pointer',
            'display': 'block',
            'float': 'left',
            'width': '18px',
            'margin': '3px',
            'height': '18px',
            'border': '1px solid #fff'
        };

        _this.wrap('<label class="iconSelect-wrap"></label>');

        var $selectIcon = $('<a>&nbsp;&nbsp;&nbsp;</a>');

        $selectIcon.css({
            'text-decoration': 'none'
        });
        $('.iconSelect-wrap').prepend($selectIcon);
        var defaults = {
            content: $('<div></div>'),
            position: 'bottom',
            showEvent: 'click',
            hideEvent: 'none',
            iconHref: 'iconsList.htm',
            onSelect: null
        };
        var config = $.extend({},
		defaults, options || {});
        if (config.icon) {
            $selectIcon.addClass(config.icon);
        }

        var eventConfig = {
            onShow: function () {
                _this.tooltip('tip').focus().unbind().bind('blur',
				function () {
				    _this.tooltip('hide');
				});
            },
            onUpdate: function (content) {
                var $c = content.panel({
                    width: 450,
                    height: 'auto',
                    border: false,
                    href: config.iconHref,
                    onLoad: function () {
                        $(this).find('a').css(iconCls);
                        $(this).on('mouseover', 'a',
						function () {
						    $(this).css('border-color', 'gray');
						}).on('mouseout', 'a',
						function () {
						    $(this).css('border-color', 'White');
						}).on('click', 'a',
						function () {
						    var name = $(this).attr('name');
						    $selectIcon.removeClass();
						    $selectIcon.addClass(name);
						    $selectIcon.attr('title', name);
						    _this.tooltip('hide');
						    if (typeof config.onSelect == 'function') {
						        config.onSelect(name);
						    }

						});
                    }
                });
            }
        };
        config = $.extend(config, eventConfig);

        _this.SetValue = function (val) {
            $selectIcon.removeClass();
            $selectIcon.addClass(val);
            $selectIcon.attr('title', val);
        }
        return _this.tooltip(config);
    };
    $.fn.iconSelect = iconSelect;
}($))