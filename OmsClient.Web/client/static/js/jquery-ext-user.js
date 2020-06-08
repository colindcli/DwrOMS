try {
    layer;
} catch (ex) {
    alert('layer;');
}

if ($.confirm === undefined) {
    alert('jquery-confirm.min.js');
}

//刷新页签数量
var tabNum = function (){
    if(window.parent.reloadNum){
        console.log('刷新数量1');
        window.parent.reloadNum();
    } else if(window.parent.parent.reloadNum){
        console.log('刷新数量2');
        window.parent.parent.reloadNum();
    } else if(window.parent.parent.parent.reloadNum){
        console.log('刷新数量3');
        window.parent.parent.parent.reloadNum();
    } else if(window.parent.parent.parent.parent.reloadNum){
        console.log('刷新数量4');
        window.parent.parent.parent.parent.reloadNum();
    }
}

var win = {};
var form = layui.form;
var table = layui.table;
var upload = layui.upload;
var laydate = layui.laydate;
var element = layui.element;

form.verify({
    //价格
    price: [/^[0-9]{1,9}$|^[0-9]{1,8}\.[0-9]{1,2}$/, '输入格式错误'],
    alpha: function(a){
        var reg = /^1$|^0$|^0\.[0-9]{1,2}$/;
        if(!reg.test(a)){
            return "请输入0到1之间的数值";
        }
        var v = parseFloat(a);
        if(v<0 || v>1){
            return "取值范围在0到1之间";
        }
    }
});

//layui模板
var Price1 = '<div><span class="priceConvert" data-price="{{ d.Price1 }}">{{ d.Price1 }}</span></div>';
var Price10 = '<div><span class="priceConvert" data-price="{{ d.Price10 }}">{{ d.Price10 }}</span></div>';
var Price100 = '<div><span class="priceConvert" data-price="{{ d.Price100 }}">{{ d.Price100 }}</span></div>';

var PriceA = '<div>{{ d.Symbol+d.PriceA }}</div>';
var PriceB = '<div>{{ d.Symbol+d.PriceB }}</div>';
var PriceC = '<div>{{ d.Symbol+d.PriceC }}</div>';
var PriceAvg = '<div>{{ d.Symbol+d.PriceAvg }}</div>';
var Price = '<div>{{ d.Symbol+d.Price }}</div>';

var InTransitQty = '<div><span class="showInTransitQty" data-productid="{{ d.ProductId }}">{{ d.InTransitQty }}</span></div>';
var SaleQty = '<div>{{ d.SaleQty }}</div>';
var BuyQty = '<div>{{ d.BuyQty }}</div>';
var HoldQty = '<div><span class="showHoldQty" data-productid="{{ d.ProductId }}">{{ d.HoldQty }}</span></div>';
var ImageQty = "<div>{{ d.ImageQty==0?'-': '<a href=\"/ProductImage?id='+d.ProductId+'\" target=\"_blank\">'+d.ImageQty+'张</a>' }}</div>";

//收款状态
var ReceiveStatus = "<div>{{ d.Status == 4 || d.Status == 7 || d.Status == 9 ? '<span class=\"pay\" title=\"款项全部收到\">已完成</span>' : ( d.Status == 3 || d.Status == 6 || d.Status == 8 ? '<span class=\"unpay\" title=\"已确认但未完成收款\">已确认</span>' : '<span class=\"unpay\" title=\"款项没收到，或者收到部分\">未完成</span>') }}</div>";

//付款状态
var PayStatus = "<div>{{ d.Status == 7 || d.Status == 9 ? '<span class=\"pay\" title=\"完成付款\">已付款</span>' : '<span class=\"unpay\" title=\"货款没付或付部分\">未完成</span>' }}</div>";

$(function () {
    // 初始化
    var initObj = {
        init: function () {
            var self = this;
            self.date();
            self.layverifyType();
            self.verify();
            self.extend();
        },
        date: function () {
            $('.date').each(function () {
                laydate.render({
                    elem: this,
                    theme: '#337ab7',
                    trigger: 'click',
                    format: 'yyyy-MM-dd'
                });
            });
            $('.yearMonth').each(function () {
                laydate.render({
                    elem: this,
                    theme: '#337ab7',
                    trigger: 'click',
                    type: 'month',
                    format: 'yyyy-MM'
                });
            });
            $('.datetime').each(function () {
                laydate.render({
                    elem: this,
                    type: 'datetime',
                    theme: '#337ab7',
                    trigger: 'click',
                    format: 'yyyy-MM-dd HH:mm:ss'
                });
            });
            $('.dateRange').each(function () {
                laydate.render({
                    elem: this,
                    theme: '#337ab7',
                    trigger: 'click',
                    range: true,
                });
            });
        },
        layverifyType: function () {
            $('[lay-verify]').attr('lay-verType', 'tips');
        },
        verify: function () {
            //自定义验证规则
            form.verify({
                pass: [/(.+){6,12}$/, '密码必须6到12位'],
                select: function(value, item){ //value：表单的值、item：表单的DOM对象
                    if(value==='' || value==='0'){
                      return '请选择';
                    }
                  }
            });
        },
        extend: function(){
            $.extend({
                formData: function(data){
                    var reg = /(\d+){4}-(\d+){2}-(\d+){2}T(\d+){2}:(\d+){2}:(\d+){2}/;
                    $.each(data, function(i, item){
                        if(reg.test(item)){
                            data[i] = item.substr(0, 10);
                        }
                    });
                    return data;
                }
            });
        }
    };
    initObj.init();


    //对话框
    var dialogObj = {
        obj: win,
        confirmObj: null,
        alertObj: null,
        init: function () {
            var self = this;
            self.extend();
            self.keyup();
        },
        extend: function () {
            var self = this;
            //win.confirm('', function(){});
            $.extend(self.obj, {
                confirm: function (content, okCallback) {
                    var jc = self.confirmObj;
                    if (jc != null) {
                        if (jc.isOpen()) {
                            jc.close();
                        }
                    }

                    jc = $.confirm({
                        title: '对话框',
                        content: content,
                        type: 'blue',
                        closeIcon: true,
                        boxWidth: '300',
                        animation: 'news',
                        closeAnimation: 'news',
                        // columnClass: 'col-md-4',
                        buttons: {
                            primary: {
                                btnClass: 'btn-primary',
                                text: '确定',
                                keys: ['enter'],
                                action: function () {
                                    if (okCallback) {
                                        okCallback.call(new Object());
                                    }
                                }
                            },
                            danger: {
                                btnClass: 'btn-default', // multiple classes.
                                text: '取消',
                                keys: ['esc'],
                                action: function () {

                                }
                            },
                        }
                    });
                },
                //win.alert('');
                alert: function (content) {
                    var jc = self.alertObj;
                    if (jc != null) {
                        if (jc.isOpen()) {
                            jc.close();
                        }
                    }

                    jc = $.confirm({
                        title: '温馨提示',
                        content: content,
                        type: 'orange',
                        typeAnimated: true,
                        columnClass: 'col-md-4',
                        animation: 'zoom',
                        closeAnimation: 'scale',
                        buttons: {
                            tryAgain: {
                                text: '我知道了',
                                btnClass: 'btn-orange',
                                action: function () {}
                            }
                        }
                    });
                },
                //win.msg('');
                msg: function (content) {
                    layer.msg(content);
                },
                /*win.open({
                    id: '',
                    title: '',
                    width: $(window).width() - 30,
                    height: $(window).height() - 30,
                    done: function () {

                    }
                });*/
                open: function (opt) {
                    var option = {
                        id: '',
                        title: '',
                        width: $(window).width() - 30,
                        height: $(window).height() - 30,
                        done: function () {

                        }
                    };
                    $.extend(option, opt);
                    var obj = $('#' + option.id);
                    obj.removeClass('hide');
                    layer.open({
                        type: 1,
                        area: [option.width + 'px', option.height + 'px'],
                        title: option.title,
                        resize: true,
                        anim: 1,
                        maxmin: true,
                        content: obj,
                        cancel: function (index, layero) {
                            self.hideDialog();
                        },
                        success: function (layero, index) {
                            $('.layui-layer-close').attr('title', '按ESC关闭窗口');
                            if (option.done) {
                                option.done.call(new Object(), layero, index);
                            }
                        }
                    });
                },
                iframe: function (opt) {
                    var option = {
                        title: '',
                        url: '',
                        width: $(window).width()-100,
                        height: $(window).height(),
                        done: function () {

                        },
                        close: function(){

                        }
                    };
                    $.extend(option, opt);
                    layer.open({
                        type: 2,
                        area: [option.width + 'px', option.height + 'px'],
                        title: option.title,
                        resize: false,
                        anim: 1,
                        maxmin: true,
                        content: option.url,
                        cancel: function (index, layero) {
                            self.hideDialog();
                            if(option.close){
                                option.close.call(new Object());
                            }
                        },
                        success: function (layero, index) {
                            $('.layui-layer-close').attr('title', '按ESC关闭窗口');
                            if (option.done) {
                                option.done.call(new Object(), layero, index);
                            }
                        }
                    });
                },
                tips: function(opt){
                    var option = {
                        el: '',
                        txt: '',
                    };
                    $.extend(option, opt);
                    layer.tips(option.txt, option.el, {
                        tips: [4, '#337ab7']
                    });
                },
                close: function () {
                    self.hideDialog();
                },
                formFocus: function (layFilter) {
                    var array = $('[lay-filter="' + layFilter + '"]').find('input:visible,select:visible');
                    for (let i = 0; i < array.length; i++) {
                        const element = array[i];
                        var has = $(element).attr('readonly') || $(element).attr('disabled');
                        if (!has) {
                            $(element).focus();
                            break;
                        }
                    }
                },
                searchBarHeight: ($(".searchBar").length > 0 ? $(".searchBar").height() : -6) + 13
            });
        },
        hideDialog: function () {
            layer.closeAll();
            setTimeout(function () {
                $(".otherDialog").addClass("hide");
            }, 0);
        },
        keyup: function () {
            var self = this;
            $(window).on("keyup", function (a) {
                if (a.keyCode == 27) {
                    self.hideDialog();
                }
            });
        }
    };
    dialogObj.init();

    //异步请求
    var ajaxObj = {
        loadingNumber: 0,
        loadingFn: top.window.loadingfn,
        init: function () {
            var self = this;
            self.setup();
            self.extend();
        },
        setup: function () {
            var self = this;
            $.ajaxSetup({
                //contentType: "application/json; charset=utf-8",
                dataType: "json",
                error: function (jqXhr, textStatus, errorMsg) {
                    win.alert('发送AJAX请求到"' + this.url + '"时出错[' + jqXhr.status + ']：' + errorMsg);
                },
                beforeSend: function (XHR) {
                    self.loadingNumber++;
                    if (self.loadingNumber > 0) {
                        if (self.loadingFn) {
                            self.loadingFn(true);
                        } else {
                            layer.load(1);
                        }
                    }
                },
                complete: function (XHR, TS) {
                    self.loadingNumber--;
                    if (self.loadingNumber <= 0) {
                        if (self.loadingFn) {
                            self.loadingFn(false);
                        } else {
                            layer.closeAll('loading');
                        }
                    }
                }
            });
        },
        resetPage: function (res) {
            win.alert(res.msg);
            if (res.code === -2) {
                setTimeout(function () {
                        window.top.location.href = '/login';
                    },
                    2000);
            } else if (res.code === -3) {
                $("body").html('');
            }
        },
        extend: function () {
            var self = this;
            $.extend({
                //$.postdata('', {}, function(){});
                postdata: function (url, data, callback) {
                    $.ajax({
                        type: "POST",
                        url: url,
                        contentType: "application/json; charset=utf-8",
                        data: JSON.stringify(data),
                        success: function (res) {
                            if (res.code !== 0) {
                                self.resetPage(res);
                            } else {
                                callback(res.data, res.obj);
                            }
                        }
                    });
                },
                //$.getdata('', {}, function(){});
                getdata: function (url, data, callback) {
                    $.get(url,
                        data,
                        function (res) {
                            if (res.code !== 0) {
                                self.resetPage(res);
                            } else {
                                callback(res.data, res.obj);
                            }
                        });
                },
            });
        }
    };
    ajaxObj.init();

    //表格
    //$.tableObject({});
    var tableObj = {
        pageSize: 20,
        init: function () {
            var self = this;
            self.size();
            self.extend();
        },
        size: function () {
            var self = this;
            var size = localStorage["pageSize"];
            if (size == null) {
                localStorage["pageSize"] = self.pageSize;
            } else {
                self.pageSize = size;
            }
        },
        extend: function () {
            var _this = this;
            $.extend({
                tableObject: function (option) {
                    var obj = {
                        tableIns: null,
                        options: null,
                    };
                    var tableIns;
                    var tableObj = {
                        tableId: '',
                        tableOption: {
                            url: '',
                            page: false,
                            where: {},
                            cols: [
                                []
                            ],
                            doneOk: function(){},
                        },
                        menuItem: null,
                        dialogHight: null,
                        init: function () {
                            var self = this;
                            $.extend(self, option);
                            self.initView();
                            self.render();
                            self.sort();
                        },
                        initView: function () {
                            var self = this;
                            $.extend(self, option);
                            var obj = $("#" + self.tableId).parent();
                            if (!obj.hasClass(self.tableId)) {
                                obj.addClass(self.tableId);
                            }
                            if (!$('#' + self.tableId).attr('lay-filter')) {
                                $('#' + self.tableId).attr('lay-filter', self.tableId);
                            }
                        },
                        render: function () {
                            var self = this;
                            var table = layui.table;
                            var opt = {
                                elem: '#' + self.tableId,
                                // even: true,
                                //skin: 'line',
                                limit: _this.pageSize,
                                limits: [10, 15, 20, 25, 30],
                                done: function (res, curr, count) {
                                    self.menu();
                                    self.title();
                                    // self.width();
                                    self.selectPageSize();
                                    if(self.tableOption.doneOk){
                                        self.tableOption.doneOk.call(new Object(), res, curr, count);
                                    }
                                }
                            };
                            $.extend(opt, self.tableOption);
                            obj.tableIns = table.render(opt);
                            obj.opt;
                        },
                        menu: function () {
                            var self = this;
                            if (!$.isEmptyObject(self.menuItem)) {
                                $.contextMenu({
                                    selector: '.' + self.tableId + ' .layui-table-body table tr',
                                    items: self.menuItem,
                                    events: {
                                        show: function (options) {
                                            removeBgRow(self.tableId);
                                            this.addClass("selectMenuBg");
                                        }
                                    },
                                    autoHide: true
                                });
                            }
                            $('.' + self.tableId + ' .layui-table-body table tr').unbind().on("click", function () {
                                $(this).addClass("selectMenuBg");
                            });
                        },
                        title: function () {
                            var self = this;
                            $('.' + self.tableId + " tbody .layui-table-cell").each(function () {
                                $(this).parent().attr("title", $(this).text());
                            });
                        },
                        width: function () {
                            var self = this;
                            $('.' + self.tableId + " tbody .layui-table-cell").each(function () {
                                var w = $(this).parent().width();
                                $(this).css({
                                    width: "auto",
                                    "max-width": 300,
                                    padding: "0 1px 0 2px"
                                }).attr("title", $(this).text());
                            });
                            $('.' + self.tableId + " thead .layui-table-cell").each(function () {
                                var w = $(this).parent().width();
                                $(this).css({
                                    width: "auto",
                                    "max-width": 300,
                                    padding: "0 1px 0 2px"
                                }).attr("title", $(this).text());
                            });

                            var tdw = [];
                            var objTd = $('.' + self.tableId + " tbody tr").first().find("td");
                            objTd.each(function () {
                                var w = $(this).width();
                                tdw.push(w);
                            });

                            var thw = [];
                            var objTh = $('.' + self.tableId + " .layui-table-header thead tr").first().find("th");
                            objTh.each(function () {
                                var w = $(this).width();
                                thw.push(w);
                            });

                            for (var i = 0; i < tdw.length; i++) {
                                
                                if (tdw[i] > thw[i]) {
                                    $(objTh[i]).css({
                                        width: tdw[i],
                                        padding: "0 1px 0 2px"
                                    }).find(">div").css({
                                        width: tdw[i]
                                    });
                                } else if (tdw[i] < thw[i]) {
                                    $(objTd[i]).css({
                                        width: thw[i],
                                        padding: "0 1px 0 2px"
                                    }).find(">div").css({
                                        width: thw[i]
                                    });;
                                }
                            }
                        },
                        sort: function () {
                            var self = this;
                            var table = layui.table;
                            var filterName = $('#' + self.tableId).attr('lay-filter');
                            table.on('sort(' + filterName + ')', function (obj) {
                                var where = $.extend({}, self.tableOption.where || {}, {
                                    field: obj.field,
                                    order: obj.type
                                });
                                table.reload(self.tableId, {
                                    initSort: obj,
                                    where: where
                                });
                            });
                        },
                        selectPageSize: function () {
                            $(".layui-laypage-limits select").unbind("change").on("change", function () {
                                localStorage["pageSize"] = $(this).val();
                            });
                        }
                    }
                    tableObj.init();
                    return obj;
                },

            });
        }
    };
    tableObj.init();

    // 附件
    var fileImgObj = {
        init: function () {
            var self = this;
            self.render();
        },
        render: function () {
            $.extend({
                //产品图片
                images: function (opt) {
                    var defaultOpt = {
                        id: '', //class="uploadImg"标签的Id值
                        paths: [], // ['']
                        productId: '',
                        sortEvent: function(){},
                        deleteEvent: function(path){},
                    };
                    var option = $.extend({}, defaultOpt, opt);

                    var imgObj = {
                        divId: '',
                        productId: '',
                        loadingIndex: null,
                        option: {
                            sortEvent: function(){},
                            deleteEvent: function(path){},
                        },
                        init: function (ot) {
                            var self = this;

                            //赋值
                            self.divId = ot.id;
                            self.productId = ot.productId;
                            $.extend(self.option, ot);
                            var paths = ot.paths;

                            $('#' + self.divId + ' .imgList').html('');
                            if (paths) {
                                var arr = [];
                                $.each(paths, function (i, item) {
                                    var div = self.getDiv(item);
                                    arr.push(div);
                                });
                                $('#' + self.divId + ' .imgList').append(arr.join(''));
                                if (arr.length > 0) {
                                    self.event();
                                }
                            }

                            var isBind = $('#' + ot.id).data('isInit');
                            if (!isBind) {
                                self.render();
                                $('#' + ot.id).data('isInit', true);
                            }
                        },
                        render: function () {
                            var self = this;
                            var uploadInst = upload.render({
                                elem: '#' + self.divId + ' button', //绑定元素
                                url: '/clientApi/UserProduct/Upload?productId='+self.productId, //上传接口
                                accept: 'images',
                                acceptMime: 'image/*',
                                exts: 'jpg|png|gif|bmp|jpeg',
                                multiple: true,
                                number: 10,
                                size: 1024, //单位kb
                                drag: true,
                                before: function (obj) {
                                    self.loadingIndex = layer.msg('上传中...', {
                                        icon: 16,
                                        shade: 0.01
                                    });
                                },
                                done: function (res) {
                                    layer.close(self.loadingIndex); //关闭loading
                                    //上传完毕回调
                                    if (res.code != 0) {
                                        ajaxObj.resetPage(res);
                                    } else {
                                        var div = self.getDiv(res.data);
                                        $('#' + self.divId + ' .imgList').append(div);
                                        self.event();
                                    }
                                },
                                error: function () {
                                    //请求异常回调
                                    win.alert('上传失败！');
                                }
                            });
                        },
                        getDiv: function (path) {
                            var div = '<div class="thumbnail" data-path="' + path + '"><img src="' + path +
                                '" class="img-rounded" /><i class="iconfont icon-tubiao39"></i></div>';
                            return div;
                        },
                        event: function () {
                            var self = this;
                            self.sortEvent();
                            $('#' + self.divId + ' .imgList .thumbnail i').unbind("click").on("click", function () {
                                var path = $(this).parent().data("path");
                                $(this).parent().remove();
                                self.getImgs();
                                self.option.deleteEvent(path);
                            });
                            self.getImgs();
                        },
                        getImgs: function () {
                            var self = this;
                            var ids = [];
                            $('#' + self.divId + ' .imgList .thumbnail img').each(function () {
                                var id = $(this).data('path');
                                ids.push(id);
                            });

                            $('#' + self.divId + ' .btnDiv input[name="ProductImages"]').val(ids.join(';'));
                        },
                        sortableObj: null,
                        sortEvent: function(){
                            var self = this;
                            if(Sortable){
                                var el = $('#' + self.divId + ' .imgList')[0];
                                self.sortableObj = Sortable.create(el, {
                                    onUpdate: function(){
                                        self.getImgs();
                                        self.option.sortEvent();
                                    }
                                });
                            }
                        }
                    };
                    imgObj.init(option);
                },
                image: function (opt) {
                    var defaultOpt = {
                        id: '', //class="uploadImg"标签的Id值
                        path: '' //图片路径
                    };
                    var option = $.extend({}, defaultOpt, opt);

                    var imgObj = {
                        divId: '',
                        loadingIndex: null,
                        init: function (ot) {
                            var self = this;

                            //赋值
                            self.divId = ot.id;

                            $('#' + self.divId + ' .imgList').html('');

                            if (ot.path) {
                                var div = self.getDiv(ot.path);
                                $('#' + self.divId + ' .imgList').html(div);
                                self.event();
                            }

                            var isBind = $('#' + ot.id).data('isInit');
                            if (!isBind) {
                                self.render();
                                $('#' + ot.id).data('isInit', true);
                            }
                        },
                        render: function () {
                            var self = this;
                            var uploadInst = upload.render({
                                elem: '#' + self.divId + ' button', //绑定元素
                                url: '/clientApi/UserFile/Upload', //上传接口
                                accept: 'images',
                                acceptMime: 'image/*',
                                exts: 'jpg|png|gif|bmp|jpeg',
                                multiple: false,
                                number: 10,
                                drag: true,
                                before: function (obj) {
                                    self.loadingIndex = layer.msg('上传中...', {
                                        icon: 16,
                                        shade: 0.01
                                    });
                                },
                                done: function (res) {
                                    layer.close(self.loadingIndex); //关闭loading
                                    //上传完毕回调
                                    if (res.code != 0) {
                                        ajaxObj.resetPage(res);
                                    } else {
                                        var div = self.getDiv(res.data);
                                        $('#' + self.divId + ' .imgList').html(div);
                                        self.event();
                                    }
                                },
                                error: function () {
                                    //请求异常回调
                                    win.alert('上传失败！');
                                }
                            });
                        },
                        getDiv: function (url) {
                            var div = '<div class="thumbnail"><img data-url="'+url+'" src="' + url +
                                '" class="img-rounded" /><i class="iconfont icon-tubiao39"></i></div>';
                            return div;
                        },
                        event: function () {
                            var self = this;
                            $('#' + self.divId + ' .imgList .thumbnail i').unbind("click").on("click", function () {
                                $(this).parent().remove();
                                self.getImgs();
                            });
                            self.getImgs();
                        },
                        getImgs: function () {
                            var self = this;
                            var urls = [];
                            $('#' + self.divId + ' .imgList .thumbnail img').each(function () {
                                var url = $(this).data('url');
                                urls.push(url);
                            });

                            $('#' + self.divId + ' .btnDiv input[name="'+self.divId+'"]').val(urls.join(';'));
                        }
                    };

                    imgObj.init(option);
                }
            })
        }
    };
    fileImgObj.init();

    // 扩展
    $.extend({
        // 编辑器抓取远程图片、A标签替换
        editorImgAhref: function(um){
            if(UM){
                um.addListener('afterpaste',function(a,b){
                    var imgs = UM.dom.domUtils.getElementsByTagName(um.document, "img");
                    $.each(imgs, function(i, item){
                        var url = $(item).attr('src');
                        if(!/data:image.*?;base64/.test(url) && url.indexOf(window.location.host)==-1){
                            $.ajax({
                                type: "get",
                                url: "/clientApi/UserFile/UploadByUmeditorCopy",
                                data: { url: url},
                                dataType: "json",
                                success: function (res) {
                                    if(res.code==0){
                                        // var newSrc = window.location.origin+res.data;
                                        var newSrc = res.data;
                                        UM.dom.domUtils.setAttributes(item, {
                                            "src": newSrc,
                                            "_src": newSrc
                                        });
                                    }
                                }
                            });
                        }
                    });

                    //
                    var ahrefs = UM.dom.domUtils.getElementsByTagName(um.document, "a");
                    $.each(ahrefs, function(i, item){
                        var url = $(item).attr('href');
                        if(url.indexOf(window.location.host)==-1){
                            // var href = window.location.origin + "/";
                            var href = "/";
                            UM.dom.domUtils.setAttributes(item, {
                                "href": href,
                                "_href": href,
                            });
                        }
                    });
                });
            }
        },

        // 编辑器
        getEditor: function(id){
            if(UM){
                var um = UM.getEditor(id);
                // 编辑器抓取远程图片、A标签替换
                $.editorImgAhref(um);
                return um;
            }
            return null;
        }
    });
});


//日期时间格式化
var formatDate = function (dateTime, format) {
    format = format.replace("yyyy", dateTime.getFullYear());
    format = format.replace("yy", dateTime.getFullYear().toString().substr(2));
    format = format.replace("MM", ("0" + (dateTime.getMonth() + 1).toString()).slice(-2));
    format = format.replace("dd", ("0" + (dateTime.getDate()).toString()).slice(-2));
    format = format.replace("hh", ("0" + (dateTime.getHours()).toString()).slice(-2));
    format = format.replace("mm", ("0" + (dateTime.getMinutes()).toString()).slice(-2));
    format = format.replace("ss", ("0" + (dateTime.getSeconds()).toString()).slice(-2));
    format = format.replace("ms", ("0" + (dateTime.getMilliseconds()).toString()).slice(-2));
    return format;
};
String.prototype.ToDate = function () {
    if(this==null){
        return "";
    }
    var dateTime = new Date(this);
    return formatDate(dateTime, "yyyy-MM-dd");
}
String.prototype.ToDatetime = function () {
    if(this==null){
        return "";
    }
    var dateTime = new Date(this);
    return formatDate(dateTime, "yyyy-MM-dd hh:mm");
}
String.prototype.ToYearMonth = function () {
    if(this==null){
        return "";
    }
    var dateTime = new Date(this);
    return formatDate(dateTime, "yyyy-MM");
}

//获取url参数
function getQueryValue(variable)
{
    var query = window.location.search.substring(1);
    var vars = query.split("&");
    for (var i=0;i<vars.length;i++) {
            var pair = vars[i].split("=");
            if(pair[0] == variable){return pair[1];}
    }
    return(false);
}

//
// for(var i=0; i<10; i++){
//     var d = 1.00+i/1000;
//     var v =  getFixed(d);
//     var r = i==0?1:1.01;
//     if(v!=r){
//         console.log('------------------------------------');
//         console.log('测试不相等');
//         console.log('------------------------------------');
//     }
// }

$.extend({
    getFixed: function(v){
        var d = parseFloat(v.toFixed(2));
        if(v>d){
            d+=0.01;
        }
        return d.toFixed(2);
    },
    showQty: function(id){
        //在途数量
        $(".showInTransitQty").unbind("hover").hover(function(){
            var pid = $(this).data('productid');
            var top = $(this).offset().top+28;
            var left = $(this).offset().left+20;
            if($('#inTransitDiv').length==0){
                $('body').append('<div class="orderDiv hide" id="inTransitDiv"></div>');
            }
            $('#inTransitDiv').html('数据加载中').css({top: top, left: left}).removeClass('hide');
            $.getdata('/clientApi/UserProduct/GetInTransitQty?productId='+pid, {}, function(res){
                //
                var arr = [];
                arr.push('<table><thead><tr><th>采购单号</th><th>采购员</th><th>数量</th><th>状态</th><th>预计到达</th></tr></thead><tbody>');
                $.each(res, function(i, item){
                    var s = item.BuyOrderId === id? "thisOrder":"";

                    arr.push('<tr class="'+s+'"><td>'+item.OrderNumber+'</td>')
                    arr.push('<td>'+item.NickName+'</td>');
                    arr.push('<td>'+item.Qty+'</td>');
                    arr.push('<td>'+item.StatusName+'</td>');
                    arr.push('<td>'+item.ArrivalsDate+'</td></tr>');
                });
                arr.push('</tbody></table>');
                //
                $('#inTransitDiv').html(arr.join(''));
            });
        }, function(){
            $('#inTransitDiv').addClass('hide');
        });

        //挂起数量
        $(".showHoldQty").unbind("hover").hover(function(){
            var pid = $(this).data('productid');
            var top = $(this).offset().top+28;
            var left = $(this).offset().left+20;
            if($('#holdDiv').length==0){
                $('body').append('<div class="orderDiv hide" id="holdDiv"></div>');
            }
            $('#holdDiv').html('数据加载中').css({top: top, left: left}).removeClass('hide');
            $.getdata('/clientApi/UserProduct/GetHoldQty?productId='+pid, {}, function(res){
                //
                var arr = [];
                arr.push('<table><thead><tr><th>销售单号</th><th>销售员</th><th>数量</th><th>状态</th><th>挂起天数</th></tr></thead><tbody>');
                $.each(res, function(i, item){
                    var s = item.SaleOrderId === id? "thisOrder":"";

                    arr.push('<tr class="'+s+'"><td>'+item.OrderNumber+'</td>')
                    arr.push('<td>'+item.NickName+'</td>');
                    arr.push('<td>'+item.Qty+'</td>');
                    arr.push('<td>'+item.StatusName+'</td>');
                    arr.push('<td>'+item.Days+'</td></tr>');
                });
                arr.push('</tbody></table>');
                //
                $('#holdDiv').html(arr.join(''));
            });
        }, function(){
            $('#holdDiv').addClass('hide');
        });
    },

});

$.fn.extend({
    showPrice: function(currency){
        $(".priceConvert").unbind("hover").hover(function(){
            if($("#convDiv").length==0){
                $('body').append('<div class="convDiv hide" id="convDiv"></div>');
            }

            var p = $(this).data('price');
            var top = $(this).offset().top+20;
            var left = $(this).offset().left+15;
            var arr = [];
            $.each(currency, function(i, item){
                var v = p*1.00/item.CurrencyRate;
                var pri = $.getFixed(v);
                arr.push({ text: item.CurrencyName, price: pri});
            });

            var array = [];
            $.each(arr, function (i, item){
                array.push('<div class="convRow clearfix">');
                array.push('<div class="convTxt">'+item.text+'</div>');
                array.push('<div class="convVal">'+item.price+'</div>');
                array.push('</div>');
            });
            $('#convDiv').html(array.join('')).css({top: top+8, left: left+10}).removeClass('hide');
        }, function(){
            $('#convDiv').addClass('hide');
        });
    },
});

var objBuyDetail = {
    setBuyDetailValue: function(res){
        var order = res.BuyOrder;
        $('#formDiv [name="BuyOrderNumber"]').text(order.BuyOrderNumber);
        $('#formDiv [name="UserNickName"]').text(order.UserNickName);
        $('#formDiv [name="Title"]').text(order.Title);
        $('#formDiv [name="CurrencyText"]').text(order.CurrencyName);
        $('#formDiv [name="CurrencyRate"]').text(order.CurrencyRate);
        //
        $('#formDiv [name="ArrivalsDate"]').text(order.ArrivalsDate.ToDate());
        $('#formDiv [name="ShipFeight"]').text(order.CurrencySymbol+order.ShipFeight);
        $('#formDiv [name="ShipFee"]').text(order.CurrencySymbol+order.ShipFee);
        $('#formDiv [name="Discount"]').text(order.CurrencySymbol+order.Discount);
        //
        $('#formDiv [name="DefaultSettlementName"]').text(order.DefaultSettlementName);
        $('#formDiv [name="DefaultShip"]').text(order.DefaultShip);
        $('#formDiv [name="DefaultAccount"]').text(order.DefaultAccount);
        $('#formDiv [name="DefaultRemark"]').text(order.DefaultRemark);
        //
        $('#formDiv [name="SupplierName"]').text(order.SupplierName);
        $('#formDiv [name="SupplierCompany"]').text(order.SupplierCompany);
        $('#formDiv [name="SupplierTel"]').text(order.SupplierTel);
        $('#formDiv [name="SupplierMobilePhone"]').text(order.SupplierMobilePhone);
        $('#formDiv [name="SupplierEmail"]').text(order.SupplierEmail);
        $('#formDiv [name="SupplierQQ"]').text(order.SupplierQQ);
        $('#formDiv [name="SupplierWechat"]').text(order.SupplierWechat);
        $('#formDiv [name="SupplierAddress"]').text(order.SupplierAddress);

        //统计
        $("#CountProductWeights").text(res.Count.ProductWeights+'克');
        $("#CountProductQtys").text(res.Count.ProductQtys);
        $("#CountInStockTotal").text(res.Count.InStockTotal);
        $("#CountShipFeight").text(res.Count.CurrencySymbol+res.Count.ShipFeight);
        $("#CountShipFee").text(res.Count.CurrencySymbol+res.Count.ShipFee);
        $("#CountDiscount").text(res.Count.CurrencySymbol+res.Count.Discount);
        $("#CountProductAmount").text(res.Count.CurrencySymbol+res.Count.ProductAmount);
        $("#CountTotal").text(res.Count.CurrencySymbol+res.Count.Total);
        $("#CountTotalRmb").text('￥'+res.Count.TotalRmb);
                
        //入库后
        if(res.Count.IsStockIn){
            $(".stockin").removeClass('hide');
            $("#CountInProductAmount").text(res.Count.CurrencySymbol+res.Count.InProductAmount);
            $("#CountInTotal").text(res.Count.CurrencySymbol+res.Count.InTotal);
            $("#CountInTotalRmb").text('￥'+res.Count.InTotalRmb);
        }else{
            $(".stockin").addClass('hide');
        }

        //备货记录
        $('#StockInQcUserName').text(order.StockInQcUserName);
        $('#StockInRemark').text(order.StockInRemark);

        //入库记录
        $('#StockInDate').text(order.StockInDate && order.StockInDate.ToDate());
        $('#StockInUserName').text(order.StockInUserName);
    },
    payList: function(res){
        var arr = [];
        $.each(res.BuyPays, function(i, item){
            arr.push('<tr>');
            arr.push('<td>'+item.AccountInfo+'</td>');
            arr.push('<td>'+item.CurrencySymbol+item.Amount+'</td>');
            arr.push('<td>'+item.CurrencyRate+'</td>');
            arr.push('<td>'+item.TransactionNumber+'</td>');
            arr.push('<td>'+item.Remark+'</td>');
            arr.push('<td>'+item.CreateName+'</td>');
            arr.push('<td>'+item.CreateDate+'</td>');
            arr.push('</tr>');
        });
        $('#financeRow').html(arr.join(''));
        table.init('financeTableList');

        //
        $('#financeTotal').text('￥'+res.TotalRmb);

        //
        $('#receiveStatus').text(res.ReceiveStatusName).addClass(res.ReceiveStatusClass);
    }
};

var objSaleDetail = {
    setSaleDetailValue: function(res){
        var order = res.SaleOrder;
        $('#formDiv [name="SaleOrderNumber"]').text(order.SaleOrderNumber);
        $('#formDiv [name="UserNickName"]').text(order.UserNickName);
        $('#formDiv [name="Title"]').text(order.Title);
        $('#formDiv [name="CurrencyText"]').text(order.CurrencyName);
        $('#formDiv [name="CurrencyRate"]').text(order.CurrencyRate);
        //
        $('#formDiv [name="ShipDate"]').text(order.ShipDate.ToDate());
        $('#formDiv [name="ShipFeight"]').text(order.CurrencySymbol+order.ShipFeight);
        $('#formDiv [name="ShipFee"]').text(order.CurrencySymbol+order.ShipFee);
        $('#formDiv [name="Discount"]').text(order.CurrencySymbol+order.Discount);
        //
        $('#formDiv [name="DefaultSettlementName"]').text(order.DefaultSettlementName);
        $('#formDiv [name="DefaultShip"]').text(order.DefaultShip);
        $('#formDiv [name="DefaultAccount"]').text(order.DefaultAccount);
        $('#formDiv [name="DefaultRemark"]').text(order.DefaultRemark);
        //
        $('#formDiv [name="ToConsignee"]').text(order.ToConsignee);
        $('#formDiv [name="ToTelphone"]').text(order.ToTelphone);
        $('#formDiv [name="ToCompanyName"]').text(order.ToCompanyName);
        $('#formDiv [name="ToZipcode"]').text(order.ToZipcode);
        $('#formDiv [name="ToAddress"]').text(order.ToAddress);
        $('#formDiv [name="ToRemark"]').text(order.ToRemark);

        //统计
        $("#CountProductWeights").text(res.Count.ProductWeights+'克');
        $("#CountProductQtys").text(res.Count.ProductQtys);
        $("#CountProductAmount").text(res.Count.CurrencySymbol+res.Count.ProductAmount);
        $("#CountShipFeight").text(res.Count.CurrencySymbol+res.Count.ShipFeight);
        $("#CountShipFee").text(res.Count.CurrencySymbol+res.Count.ShipFee);
        $("#CountDiscount").text(res.Count.CurrencySymbol+res.Count.Discount);
        $("#CountTotal").text(res.Count.CurrencySymbol+res.Count.Total);
        $("#CountTotalRmb").text('￥'+res.Count.TotalRmb);

        //备货记录
        $('#StockStartInfo').text(res.StockInfo.StockStartInfo);
        $('#StockEndInfo').text(res.StockInfo.StockEndInfo);

        //发货记录
        $('#StockShipDate').text(order.StockShipDate && order.StockShipDate.ToDate());
        $('#StockRemark').text(order.StockRemark);
        $('#StockOutDate').text(order.StockOutDate && order.StockOutDate.ToDatetime());
        $('#StockOutName').text(order.StockOutName);
    },
    receiveList: function(res){
        var arr = [];
        $.each(res.SaleReceives, function(i, item){
            arr.push('<tr>');
            arr.push('<td>'+item.AccountInfo+'</td>');
            arr.push('<td>'+item.CurrencySymbol+item.Amount+'</td>');
            arr.push('<td>'+item.CurrencySymbol+item.TranFee+'</td>');
            arr.push('<td>'+item.CurrencyRate+'</td>');
            arr.push('<td>'+item.TransactionNumber+'</td>');
            arr.push('<td>'+item.Remark+'</td>');
            arr.push('<td>'+item.CreateName+'</td>');
            arr.push('<td>'+item.CreateDate+'</td>');
            arr.push('</tr>');
        });
        $('#financeRow').html(arr.join(''));
        table.init('financeTableList');
        
        //
        $('#financeTotal').text('￥'+res.TotalRmb);

        //
        $('#receiveStatus').text(res.ReceiveStatusName).addClass(res.ReceiveStatusClass);
    },
    trackList: function(res){
        var arr = [];
        $.each(res.Tracks, function(i, item){
            arr.push('<tr>');
            arr.push('<td>'+item.TrackName+'</td>');
            arr.push('<td>'+item.TrackNumber+'</td>');
            arr.push('<td>'+item.CurrencySymbol+item.Feight+'</td>');
            arr.push('<td>'+item.CurrencyRate+'</td>');
            arr.push('<td>'+item.TrackRemark+'</td>');
            arr.push('<td>'+item.CreateName+'</td>');
            arr.push('<td>'+item.CreateDate+'</td>');
            arr.push('</tr>');
        });
        $('#trackRow').html(arr.join(''));
        table.init('trackTableList');

        //
        $('#trackTotal').text('￥'+res.TotalRmb);
    }
};

var listField = {
    numbers: {
        field: "numbers",
        title: "序号",
        type: "numbers",
        width: 50,
        fixed: "left"
    },
    category: {
        field: "CategoryName",
        title: "分类"
    },
    productSku: {
        field: "ProductSku",
        title: "编码",
        width: 80,
    },
    productName: {
        field: "ProductName",
        title: "名称",
    },
    specification: {
        field: "ProductSpecification",
        title: "规格",
    },
    productRemark: {
        field: "ProductRemark",
        title: "描述",
    },
    productWeight: {
        field: "ProductWeight",
        title: "重量",
        width: 50,
    },
    orderQty: {
        field: "OrderQty",
        title: "起订",
        width: 50,
    },
    inTransitQty: {
        field: "InTransitQty",
        title: "在途",
        width: 50,
        templet: InTransitQty
    },
    saleQty: {
        field: "SaleQty",
        title: "可售",
        width: 50,
        templet: SaleQty
    },
    holdQty: {
        field: "HoldQty",
        title: "挂起",
        width: 50,
        templet: HoldQty
    },
    priceA: {
        field: "PriceA",
        title: "售价1",
        width: 65,
        templet: PriceA
    },
    priceB: {
        field: "PriceB",
        title: "售价10",
        width: 65,
        templet: PriceB
    },
    priceC: {
        field: "PriceC",
        title: "售价100",
        width: 65,
        templet: PriceC
    },
    price1: {
        field: "Price1",
        title: "售价1",
        width: 65,
        templet: Price1
    },
    price10: {
        field: "Price10",
        title: "售价10",
        width: 65,
        templet: Price10
    },
    price100: {
        field: "Price100",
        title: "售价100",
        width: 65,
        templet: Price100
    },
    priceAvg: {
        field: "PriceAvg",
        title: "采购均价",
        width: 65,
        align: 'center',
        templet: PriceAvg,
    },
    priceAvgFixed: {
        field: "PriceAvg",
        title: "采购均价",
        width: 60,
        fixed: "right",
        align: 'center'
    },
    priceAvgFixedTem: {
        field: "PriceAvg",
        title: "采购均价",
        width: 60,
        align: 'center',
        templet: PriceAvg,
        fixed: "right",
    },
    imageQty: {
        field: "ImageQty",
        title: "图片数",
        width: 60,
        templet: ImageQty
    },
    areaPosition: {
        field: "AreaPosition",
        title: "存仓位置",
        width: 80,
    },
    areaPositionSelect: {
        field: "AreaPosition",
        title: "存仓位置",
        width: 80,
        templet: '#AreaPosition'
    },
    inqty: {
        field: "InQty",
        title: "入库数量",
        width: 65,
    },
    qty: {
        field: "Qty",
        title: "采购数量",
        width: 65,
    },
    qtyfixed: {
        field: "Qty",
        title: "采购数量",
        width: 65,
        align: 'center',
        fixed: "right"
    },
    qtyfixedTemp: {
        field: "Qty",
        title: "采购数量",
        width: 65,
        templet: '#QtyEdit',
        fixed: "right",
    },
    price: {
        field: "Price",
        title: "价格",
        width: 65,
    },
    priceFixed: {
        field: "Price",
        title: "价格",
        templet: Price,
        width: 65,
        fixed: "right"
    },
    priceFixedEdit: {
        field: "Price",
        title: "价格",
        templet: '#PriceEdit',
        width: 65,
        fixed: "right"
    },
    remark: {
        field: "Remark",
        title: "备注",
    },
    remarkFixed: {
        field: "Remark",
        title: "备注",
        fixed: "right"
    },
    //sale
    saleOrderNumber: {
        field: "SaleOrderNumber",
        title: "订单号",
        width: 90,
    },
    stockInfo: {
        field: "StockInfo",
        title: "备货状态",
    },
    receiveStatus: {
        field: "ReceiveStatus",
        title: "收款状态",
        width: 80,
        templet: ReceiveStatus
    },
    stockOutDate: {
        field: "StockOutDate",
        title: "出库时间",
        width: 110,
        templet: "<div>{{ d.StockOutDate==null?'': d.StockOutDate.ToDatetime() }}</div>"
    },
    postDate: {
        field: "PostDate",
        title: "提交时间",
        width: 110,
        templet: "<div>{{ d.PostDate==null?'': d.PostDate.ToDatetime() }}</div>"
    },
    stockEnd: {
        field: "StockEnd",
        title: "完成备货",
        width: 110,
        templet: "<div>{{ d.StockEnd==null?'': d.StockEnd.ToDatetime() }}</div>"
    },
    //buy
    buyOrderNumber: {
        field: "BuyOrderNumber",
        title: "订单号",
        width: 90,
    },
    currencyName: {
        field: "CurrencyName",
        title: "币种",
        width: 80,
    },
    title: {
        field: "Title",
        title: "标题",
    },
    supplierName: {
        field: "SupplierName",
        title: "供应商",
        width: 125,
    },
    defaultSettlementName: {
        field: "DefaultSettlementName",
        title: "结款方式",
    },
    payStatus: {
        field: "PayStatus",
        title: "付款状态",
        templet: PayStatus
    },
    createName: {
        field: "CreateName",
        title: "创建人",
        width: 60,
    },
    oprationName: {
        field: "CreateName",
        title: "操作人",
        width: 80,
    },
    createDate: {
        field: "CreateDate",
        title: "创建时间",
        width: 110,
        templet: "<div>{{ d.CreateDate==null?'': d.CreateDate.ToDatetime() }}</div>"
    },
    oprationDate: {
        field: "CreateDate",
        title: "操作时间",
        width: 110,
        templet: "<div>{{ d.CreateDate==null?'': d.CreateDate.ToDatetime() }}</div>"
    },
}