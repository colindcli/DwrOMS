try {
    layer;
} catch (ex) {
    alert('layer;');
}

if ($.confirm === undefined) {
    alert('jquery-confirm.min.js');
}

var win = {};
var form = layui.form;
var table = layui.table;
var upload = layui.upload;
var laydate = layui.laydate;

//获取网址query值
function GetUrlParam(paraName) {
    var url = document.location.toString();
    var arrObj = url.split("?");

    if (arrObj.length > 1) {
        var arrPara = arrObj[1].split("&");
        var arr;

        for (var i = 0; i < arrPara.length; i++) {
            arr = arrPara[i].split("=");

            if (arr != null && arr[0] == paraName) {
                return arr[1];
            }
        }
        return "";
    } else {
        return "";
    }
}

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
            });
        },
        extend: function () {
            $.extend({
                formData: function (data) {
                    var reg = /(\d+){4}-(\d+){2}-(\d+){2}T(\d+){2}:(\d+){2}:(\d+){2}/;
                    $.each(data, function (i, item) {
                        if (reg.test(item)) {
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
                        boxWidth: '30%',
                        animation: 'news',
                        closeAnimation: 'news',
                        columnClass: 'col-md-4',
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
                            if (option.done) {
                                option.done.call(new Object(), layero, index);
                            }
                        }
                    });
                },
                iframe: function (opt) {
                    var option = {
                        url: '',
                        title: '',
                        width: $(window).width() - 30,
                        height: $(window).height() - 30,
                        done: function () {

                        }
                    };
                    $.extend(option, opt);
                    layer.open({
                        type: 2,
                        area: [option.width + 'px', option.height + 'px'],
                        title: option.title,
                        resize: true,
                        anim: 1,
                        maxmin: true,
                        content: option.url,
                        cancel: function (index, layero) {
                            self.hideDialog();
                        },
                        success: function (layero, index) {
                            if (option.done) {
                                option.done.call(new Object(), layero, index);
                            }
                        }
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
                        window.top.location.href = '/xft-admin/login.html';
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
                                callback(res.data);
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
                                callback(res.data);
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
                            ]
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
                                limits: [10, 20, 30, 40, 50, 100],
                                done: function (res, curr, count) {
                                    self.menu();
                                    self.title();
                                    // self.width();
                                    self.selectPageSize();
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
                files: function (opt) {
                    var defaultOpt = {
                        id: '', //class="uploadFile"标签的Id值
                        items: [] // [{Id:'', Title:''}]
                    };
                    var option = $.extend({}, defaultOpt, opt);

                    //上传文件
                    var fileObj = {
                        divId: '',
                        loadingIndex: null,
                        init: function (ot) {
                            var self = this;
                            //赋值
                            self.divId = ot.id;
                            var items = ot.items;

                            $('#' + self.divId + ' .fileList').html('');
                            if (items) {
                                var arr = [];
                                $.each(items, function (i, item) {
                                    var div = self.getDiv(item);
                                    arr.push(div);
                                });
                                $('#' + self.divId + ' .fileList').append(arr.join(''));
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
                                url: '/api/AdminFile/Upload', //上传接口
                                accept: 'file',
                                multiple: true,
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
                                        var data = res.data;
                                        var div = self.getDiv(data);
                                        $('#' + self.divId + ' .fileList').append(div);
                                        self.event();
                                    }
                                },
                                error: function () {
                                    //请求异常回调
                                    win.alert('上传失败！');
                                }
                            });
                        },
                        getDiv: function (item) {
                            var url = '/api/AdminFile/Get/' + item.Id;
                            var div = '<label class="clearfix filerow" data-id="' + item.Id +
                                '"><i class="iconfont icon-error ml-5"></i><span class="iconfont icon-fujian"></span><a href="' + url + '" target="_blank" title="点击下载">' +
                                item.Title + '</a></label>';
                            return div;
                        },
                        event: function () {
                            var self = this;
                            self.sortEvent();
                            $('#' + self.divId + ' .fileList .filerow i').unbind("click").on("click",
                                function () {
                                    $(this).parent().remove();
                                    self.getFiles();
                                });
                            self.getFiles();
                        },
                        getFiles: function () {
                            var self = this;
                            var ids = [];
                            $('#' + self.divId + ' .fileList .filerow').each(function () {
                                var id = $(this).data('id');
                                ids.push(id);
                            });

                            $('#' + self.divId + ' .btnDiv input[type="hidden"]').val(ids.join(';'));
                        },
                        sortableObj: null,
                        sortEvent: function () {
                            var self = this;
                            if (Sortable) {
                                var el = $('#' + self.divId + ' .fileList')[0];
                                self.sortableObj = Sortable.create(el, {
                                    onUpdate: function () {
                                        self.getFiles();
                                    }
                                });
                            }
                        }
                    };
                    fileObj.init(option);
                },
                images: function (opt) {
                    var defaultOpt = {
                        id: '', //class="uploadImg"标签的Id值
                        items: [] // [{Id:'', Title:''}]
                    };
                    var option = $.extend({}, defaultOpt, opt);

                    var imgObj = {
                        divId: '',
                        loadingIndex: null,
                        init: function (ot) {
                            var self = this;

                            //赋值
                            self.divId = ot.id;
                            var items = ot.items;

                            $('#' + self.divId + ' .imgList').html('');
                            if (items) {
                                var arr = [];
                                $.each(items, function (i, item) {
                                    var div = self.getDiv(item.Id);
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
                                url: '/api/AdminFile/Upload?createSmallPic=true', //上传接口
                                accept: 'images',
                                acceptMime: 'image/*',
                                exts: 'jpg|png|gif|bmp|jpeg',
                                multiple: true,
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
                                        var id = res.data.Id;
                                        var div = self.getDiv(id);
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
                        getDiv: function (id) {
                            var url = '/api/AdminFile/Get/' + id + '?type=' + 1;
                            var div = '<div class="thumbnail" data-id="' + id + '"><img src="' + url +
                                '" class="img-rounded" /><i class="iconfont icon-tubiao39"></i></div>';
                            return div;
                        },
                        event: function () {
                            var self = this;
                            self.sortEvent();
                            $('#' + self.divId + ' .imgList .thumbnail i').unbind("click").on("click", function () {
                                $(this).parent().remove();
                                self.getImgs();
                            });
                            self.getImgs();
                        },
                        getImgs: function () {
                            var self = this;
                            var ids = [];
                            $('#' + self.divId + ' .imgList .thumbnail').each(function () {
                                var id = $(this).data('id');
                                ids.push(id);
                            });

                            $('#' + self.divId + ' .btnDiv input[type="hidden"]').val(ids.join(';'));
                        },
                        sortableObj: null,
                        sortEvent: function () {
                            var self = this;
                            if (Sortable) {
                                var el = $('#' + self.divId + ' .imgList')[0];
                                self.sortableObj = Sortable.create(el, {
                                    onUpdate: function () {
                                        self.getImgs();
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
                        item: '' // 'AttachmentId'
                    };
                    var option = $.extend({}, defaultOpt, opt);

                    var imgObj = {
                        divId: '',
                        loadingIndex: null,
                        init: function (ot) {
                            var self = this;

                            //赋值
                            self.divId = ot.id;
                            var items = ot.items;

                            $('#' + self.divId + ' .imgList').html('');

                            if (ot.item) {
                                var div = self.getDiv(ot.item);
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
                                url: '/api/AdminFile/Upload?createSmallPic=true', //上传接口
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
                                        var id = res.data.Id;
                                        var div = self.getDiv(id);
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
                        getDiv: function (id) {
                            var url = '/api/AdminFile/Get/' + id + '?type=' + 1;
                            var div = '<div class="thumbnail" data-id="' + id + '"><img src="' + url +
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
                            var ids = [];
                            $('#' + self.divId + ' .imgList .thumbnail').each(function () {
                                var id = $(this).data('id');
                                ids.push(id);
                            });

                            $('#' + self.divId + ' .btnDiv input[type="hidden"]').val(ids.join(';'));
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
        // 品牌
        class: function (opt) {
            var option = {
                id: '',
                name: '',
                url: '',
                value: '',
                required: false
            };
            $.extend(option, opt);

            var objClass = {
                id: '',
                name: '',
                url: '',
                value: '',
                required: false,
                init: function (id, name, url, value, required) {
                    var self = this;
                    self.id = id;
                    self.name = name;
                    self.url = url;
                    self.value = value;
                    self.required = required;
                    self.render();
                },
                render: function () {
                    var self = this;
                    $.getdata(self.url, {}, function (res) {
                        var array = self.renderControl(res);
                        $('#' + self.id).html(array.join(''));
                        self.event();
                    });
                },
                renderControl: function (res) {
                    var self = this;
                    var array = [];
                    array.push('<select class="form-control pull-left classSelect" lay-ignore>');
                    array.push('	<option value="">新建</option>');
                    if (res) {
                        $.each(res, function (i, item) {
                            if (item == self.value) {
                                array.push('	<option value="' + item + '" selected="selected">' + item + '</option>');
                            } else {
                                array.push('	<option value="' + item + '">' + item + '</option>');
                            }
                        });
                    }
                    array.push('</select>');
                    if (self.required) {
                        array.push('<input class="form-control w-25 pull-left" name="' + self.name + '" type="text" required lay-verify="required" placeholder="请输入" value="' + self.value + '" />');
                    } else {
                        array.push('<input class="form-control w-25 pull-left" name="' + self.name + '" type="text" placeholder="请输入" value="' + self.value + '" />');
                    }

                    return array;
                },
                event: function () {
                    var self = this;
                    $('#' + self.id + ' select').unbind("change").on("change", function () {
                        var v = $(this).val();
                        $('#' + self.id + ' [name="' + self.name + '"]').val(v);
                        if (v == "") {
                            $('#' + self.id + ' [name="' + self.name + '"]').focus();
                        }
                    });
                },
            };

            objClass.init(option.id, option.name, option.url, option.value, option.required);
        },

        // 编辑器抓取远程图片、A标签替换
        editorImgAhref: function (um) {
            if (UM) {
                um.addListener('afterpaste', function (a, b) {
                    var imgs = UM.dom.domUtils.getElementsByTagName(um.document, "img");
                    $.each(imgs, function (i, item) {
                        var url = $(item).attr('src');
                        if (!/data:image.*?;base64/.test(url) && url.indexOf(window.location.host) == -1) {
                            $.ajax({
                                type: "get",
                                url: "/api/AdminFile/UploadByUmeditorCopy",
                                data: {
                                    url: url
                                },
                                dataType: "json",
                                success: function (res) {
                                    if (res.code == 0) {
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
                    $.each(ahrefs, function (i, item) {
                        var url = $(item).attr('href');
                        if (url.indexOf(window.location.host) == -1) {
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
        getEditor: function (id) {
            if (UM) {
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
    var dateTime = new Date(this);
    return formatDate(dateTime, "yyyy-MM-dd");
}
String.prototype.ToDatetime = function () {
    var dateTime = new Date(this);
    return formatDate(dateTime, "yyyy-MM-dd hh:mm");
}