/**
 * ui-choose通用选择插件
 * 基于jQuery
 */
; + function($) {
    "use strict";
    // 默认实例化配置
    var defaults = {
        itemWidth: null,
        skin: '',
        multi: false,
        active: 'selected',
        full: false, //是否采用flex布局，每个元素宽度相同
        colNum: null, // 每行显示的个数
        dataKey: 'ui-choose', //实例化后的data键值，方便后续通过data('ui-choose')取出；
        change: null, //choose值改变时的回调；
        click: null //choose元素点击时的回调，diabled时不发生。
    };
    /**
     * ui-choose插件
     */
    $.fn.ui_choose = function(options) {
        var _this = $(this),
            _num = _this.length;
        // 当要实例的对象只有一个时，直接实例化返回对象；
        if (_num === 1) {
            return new UI_choose(_this, options);
        };
        // 当要实例的对象有多个时，循环实例化，不返回对象；
        if (_num > 1) {
            _this.each(function(index, el) {
                new UI_choose($(el), options);
            })
        }
        // 当元素个数为0时，不执行实例化。
    };

    /**
     * UI_choose对象
     * @param {[jQuery]} el  [jQuery选择后的对象，此处传入的为单个元素]
     * @param {[object]} opt [设置的参数]
     */
    function UI_choose(el, opt) {
        this.el = el;
        this._tag = this.el.prop('tagName').toLowerCase();
        this._opt = $.extend({}, defaults, opt);

        return this._init();
    }

    // UI_choose 原型链扩展。
    UI_choose.prototype = {

        // init初始化;
        _init: function() {
            var _data = this.el.data(this._opt.dataKey);
            // 如果已经实例化了，则直接返回
            if (_data)
                return _data;
            else
                this.el.data(this._opt.dataKey, this);

            // 设置是否多选
            if (this._tag == 'select') {
                this.multi = this.el.prop('multiple');
            } else {
                this.multi = this.el.attr('multiple') ? !!this.el.attr('multiple') : this._opt.multi;
            }

            // 根据不同的标签进行不同的元素组建
            var _setFunc = this['_setHtml_' + this._tag];
            if (_setFunc) {
                _setFunc.call(this);
            }
            if (this._opt.full) {
                this._wrap.addClass('choose-flex');
            }
            this._wrap.addClass(this._opt.skin);
            if (this.multi && !this._opt.skin)
                this._wrap.addClass('choose-type-right');
            this._bindEvent(); // 绑定事件
        },

        // 组建并获取相关的dom元素-ul;
        _setHtml_ul: function() {
            this._wrap = this.el;
            this._items = this.el.children('li');
            if (this._opt.itemWidth) {
                this._items.css('width', this._opt.itemWidth);
            }
        },

        // 组建并获取相关的dom元素-select;
        _setHtml_select: function() {
            var _ohtml = '<ul class="ui-choose">';
            this.el.find('option').each(function(index, el) {
                var _this = $(el),
                    _text = _this.text(),
                    _value = _this.prop('value'),
                    _selected = _this.prop('selected') ? 'selected' : '',
                    _disabled = _this.prop('disabled') ? ' disabled' : '';
                _ohtml += '<li title="' + _text + '" data-value="' + _value + '" class="' + _selected + _disabled + '">' + _text + '</li> ';
            });
            _ohtml += '</ul>';
            this.el.after(_ohtml);

            this._wrap = this.el.next('ul.ui-choose');
            this._items = this._wrap.children('li');
            if (this._opt.itemWidth) {
                this._items.css('width', this._opt.itemWidth);
            }
            this.el.hide();
        },

        // 绑定事件；
        _bindEvent: function() {
            var _this = this;
            _this._wrap.on('click', 'li', function() {
                var _self = $(this);
                if (_self.hasClass('disabled'))
                    return;

                if (!_this.multi) { // single select
                    var _val = _self.attr('data-value') || _self.index();
                    _this.val(_val);
                    _this._triggerClick(_val, _self);
                } else { // multiple
                    _self.toggleClass(_this._opt.active);
                    var _val = [];
                    _this._items.each(function(index, el) {
                        var _el = $(this);
                        if (_el.hasClass(_this._opt.active)) {
                            var _valOrIndex = _this._tag == 'select' ? _el.attr('data-value') : _el.index();
                            _val.push(_valOrIndex);
                        }
                    });
                    _this.val(_val);
                    _this._triggerClick(_val, _self);
                }
            });
            return _this;
        },

        // change 触发；value：值 ；item：选中的option；
        _triggerChange: function(value, item) {
            item = item || this._wrap;
            this.change(value, item);
            if (typeof this._opt.change == 'function')
                this._opt.change.call(this, value, item);
        },

        // click 触发；value：值 ；item：选中的option；
        _triggerClick: function(value, item) {
            this.click(value, item);
            if (typeof this._opt.click == 'function')
                this._opt.click.call(this, value, item);
        },

        // 获取或设置值:select
        _val_select: function(value) {
            // getValue
            if (arguments.length === 0) {
                return this.el.val();
            }
            // setValue
            var _oValue = this.el.val();
            if (!this.multi) { // single select
                var _selectedItem = this._wrap.children('li[data-value="' + value + '"]');
                if (!_selectedItem.length)
                    return this;
                this.el.val(value);
                _selectedItem.addClass(this._opt.active).siblings('li').removeClass(this._opt.active);
                if (value !== _oValue) {
                    this._triggerChange(value);
                }
            } else { // multiple select
                if (value == null || value == '' || value == []) {
                    this.el.val(null);
                    this._items.removeClass(this._opt.active);
                } else {
                    value = typeof value == 'object' ? value : [value];
                    this.el.val(value);
                    this._items.removeClass(this._opt.active);
                    for (var i in value) {
                        var _v = value[i];
                        this._wrap.children('li[data-value="' + _v + '"]').addClass(this._opt.active);
                    }
                }
                if ((value + '') !== (_oValue + '')) {
                    this._triggerChange(value);
                }
            }
            // multiple
            return this;
        },

        // 获取或设置值:ul
        _val_ul: function(index) {
            // getValue
            if (arguments.length === 0) {
                var _oActive = this._wrap.children('li.' + this._opt.active);
                if (!this.multi) { // single select
                    return _oActive.index() == -1 ? null : _oActive.index();
                } else { // single select
                    if (_oActive.length == 0) {
                        return null;
                    }
                    var _this = this,
                        _val = [];
                    _oActive.each(function(index, el) {
                        var _el = $(el);
                        if (_el.hasClass(_this._opt.active)) {
                            _val.push(_el.index());
                        }
                    });
                    return _val;
                }
            }
            // setValue
            var _oIndex = this._val_ul();
            if (!this.multi) { // single select
                var _selectedItem = this._wrap.children('li').eq(index);
                if (!_selectedItem.length)
                    return this;
                _selectedItem.addClass(this._opt.active).siblings('li').removeClass(this._opt.active);
                if (index !== _oIndex) {
                    this._triggerChange(index, _selectedItem);
                }
            } else { // multiple select
                if (index == null || index == '' || index == []) {
                    this._items.removeClass(this._opt.active);
                } else {
                    index = typeof index == 'object' ? index : [index];
                    this._items.removeClass(this._opt.active);
                    for (var i in index) {
                        var _no = index[i];
                        this._wrap.children('li').eq(_no).addClass(this._opt.active);
                    }
                }
                if ((index + '') !== (_oIndex + '')) {
                    this._triggerChange(index);
                }
            }
            // multiple
            return this;
        },

        // 获取或设置值
        val: function() {
            return this['_val_' + this._tag].apply(this, arguments);
        },

        // 值改变事件；
        change: function(value, item) {},

        // 点击事件；
        click: function(value, item) {},

        // 隐藏
        hide: function() {
            this._wrap.hide();
            return this;
        },

        // 显示
        show: function() {
            this._wrap.show();
            return this;
        },

        // 全选
        selectAll: function() {
            if (!this.multi)
                return this;
            if (this._tag == 'select') {
                this.el.find('option').not(':disabled').prop('selected', true);
                var _val = this.el.val();
                this.val(_val);
            } else {
                var _val = [];
                this._items.not('.disabled').each(function(index, el) {
                    _val.push(index);
                });
                this.val(_val);
            }
            return this;
        }
    };
}(jQuery);


//
$(function () {
    $.extend({
        choose: function (opt) {
            var option = {
                id: '',
                name: '',
                data: '',
                selected: '',
                multiple: false,
            };
            $.extend(option, opt);

            var objChoose = {
                id: '',
                name: '',
                dataArray: [],
                selected: '',
                isSelectAll: false,
                init: function (id, name, data, select, multiple) {
                    var self = this;
                    self.id = id;
                    self.name = name;
                    self.multiple = multiple;
                    self.selected = select;

                    if (data == null || data == '') {
                        $("#" + self.id).html('<div class="chooseNodata">没有设置选项</div>');
                        return;
                    }

                    var lis = data.split('|');
                    var ses = select == null || select == '' ? [] : select.split('|');
                    $.each(lis, function (i, item) {
                        var arrs = item.split(':');
                        if (arrs.length > 1) {
                            var se = '';
                            $.each(ses, function (j, s) {
                                var n = s.split(':');
                                if (n[0] == arrs[0]) {
                                    se = n[1];
                                    return;
                                }
                            });

                            var ars = arrs[1].split(';');
                            var ls = [];
                            $.each(ars, function (m, k) {
                                if(self.multiple){
                                    if(se==null || se==''){
                                        ls.push({
                                            text: k,
                                            selected: false
                                        });
                                    }else{
                                        var s = se.split(';');
                                        var b = false;
                                        $.each(s, function(m, n){
                                            if(n==k){
                                                b = true;
                                                return;
                                            }
                                        });
                                        ls.push({
                                            text: k,
                                            selected: b
                                        });
                                    }
                                }else{
                                    ls.push({
                                        text: k,
                                        selected: k == se
                                    });
                                }
                            });
                            self.dataArray.push({
                                name: arrs[0],
                                lis: ls
                            });
                        }
                    });
                    self.render();
                },
                render: function () {
                    var self = this;
                    var array = [];
                    $.each(self.dataArray, function (i, item) {
                        array.push('<div class="row rowlr chooseRow">');
                        array.push('	<div class="col-xs-1 name">' + item.name + '</div>');
                        array.push('	<div class="col-xs-11">');
                        if (self.multiple) {
                            array.push('		<ul class="ui-choose" multiple="multiple">');
                        } else {
                            array.push('		<ul class="ui-choose">');
                        }

                        $.each(item.lis, function (j, row) {
                            if (row.selected) {
                                array.push('			<li class="selected">' + row.text + '</li>');
                            } else {
                                array.push('			<li>' + row.text + '</li>');
                            }
                        })
                        array.push('		</ul>');
                        array.push('	</div>');
                        array.push('</div>');
                    });

                    array.push('<input name="' + self.name + '" type="hidden">');
                    $("#" + self.id).html(array.join(''));

                    $(".ui-choose").ui_choose({
                        click: function () {
                            self.select();
                        }
                    });

                    self.select();
                },
                select: function () {
                    var self = this;
                    var array = [];
                    var isSelected = true;
                    $('#' + self.id + ' .chooseRow').each(function (i, item) {
                        var name = $(this).find(".name").text();
                        var selecteds = [];
                        $(this).find('.ui-choose .selected').each(function (j, row) {
                            selecteds.push($(row).text())
                        });

                        if (selecteds.length == 0) {
                            isSelected = false;
                        }
                        array.push(name + ":" + selecteds.join(';'));
                    });

                    $("#" + self.id + ' [name="' + self.name + '"]').val(array.join('|')).data('isSelectedAll', isSelected);
                    self.isSelectAll = isSelected;
                }
            }
            objChoose.init(option.id, option.name, option.data, option.selected, option.multiple);
        }
    });
});