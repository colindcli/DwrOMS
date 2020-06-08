//
$(function () {
    var saleOrderId = getQueryValue("saleOrderId");

    var objTable = {
        tableId: "tableList",
        searchKeys: {},
        searchBarHeight: win.searchBarHeight,
        init: function () {
            var self = this;
            self.initData();
            self.search();
            self.render();
            self.operator();
            self.addInit();
            self.verify();
        },
        // search初始化
        initData: function () {
            var self = this;
            
            $.getdata('/clientApi/WarehouseSaleTrack/GetAccountCurrency?type=2', {}, function(res){
                //
                var currency = ['<option value="">请选择</option>'];
                $.each(res.Currencies, function(i, item){
                    currency.push('<option value="'+item.CurrencyId+'">'+item.CurrencyName+'（汇率'+item.CurrencyRate+'）</option>')
                });
                $('[name="CurrencyId"]').html(currency.join(''));
            });
        },
        // 点击搜索
        search: function () {
            var self = this;
            form.on("submit(searchFormFilter)", function (data) {
                $.extend(self.searchKeys, data.field);
                self.render();
                return false;
            });
        },
        // 显示表格
        render: function () {
            var self = this;
            $.tableObject({
                tableId: self.tableId,
                tableOption: {
                    url: "/clientApi/WarehouseSaleTrack/GetSaleTrackList?saleOrderId=" + saleOrderId,
                    page: true,
                    height: "full-" + self.searchBarHeight,
                    where: self.searchKeys,
                    cols: [
                        [listField.numbers,
                            {
                                field: "TrackName",
                                title: "快递名称",
                            },
                            {
                                field: "TrackNumber",
                                title: "快递单号",
                            },
                            {
                                field: "Feight",
                                title: "运费",
                                templet: '<div>{{ d.CurrencySymbol + d.Feight }}</div>',
                                width: 90,
                            },
                            {
                                field: "CurrencyRate",
                                title: "汇率",
                                width: 60,
                            },
                            {
                                field: "TrackRemark",
                                title: "快递备注",
                            },
                            listField.oprationName,
                            listField.createDate,
                            {
                                field: "operator",
                                title: "操作",
                                width: 100,
                                fixed: "right",
                                align: "center",
                                toolbar: "#operator"
                            }
                        ]
                    ]
                }
            });
        },
        // 添加初始化
        addInit: function () {
            var self = this;
            $("#btnAdd")
                .unbind("click")
                .on("click", function () {
                    win.open({
                        id: "add",
                        title: "添加",
                        width: 500,
                        height: $(window).height() - 100,
                        done: function () {
                            win.formFocus('addFormFilter');

                            form.on("submit(addBtnFilter)", function (data) {
                                data.field.SaleOrderId = saleOrderId;
                                $.postdata("/clientApi/WarehouseSaleTrack/AddSaleTrack", data.field, function (res) {
                                    if (res) {
                                        win.close();
                                        win.msg("添加成功");
                                        self.render();

                                        // 重置
                                        data.form.reset();

                                    } else {
                                        win.alert("添加失败");
                                    }
                                });
                                return false;
                            });
                        }
                    });
                });
        },
        // 表格操作
        operator: function () {
            var self = this;
            //监听工具条
            table.on("tool(" + self.tableId + ")", function (obj) {
                //注：tool是工具条事件名，test是table原始容器的属性 lay-filter="对应的值"
                var data = obj.data; //获得当前行数据
                var layEvent = obj.event; //获得 lay-event 对应的值（也可以是表头的 event 参数对应的值）
                var tr = obj.tr; //获得当前行 tr 的DOM对象
                if (layEvent === "edit") {
                    win.open({
                        id: "info",
                        title: "编辑",
                        width: 500,
                        height: $(window).height() - 100,
                        done: function () {
                            form.val("editFormFilter", $.formData(data));

                            win.formFocus('editFormFilter');

                            form.on("submit(editBtnFilter)", function (data) {
                                data.field.SaleOrderId = saleOrderId;
                                $.postdata("/clientApi/WarehouseSaleTrack/UpdateSaleTrack", data.field, function (res) {
                                    if (res) {
                                        win.close();
                                        win.msg("修改成功");
                                        self.render();

                                        // 重置
                                        data.form.reset();
                                    } else {
                                        win.alert("修改失败");
                                    }
                                });
                                return false;
                            });
                        }
                    });
                } else if (layEvent === "del") {
                    //删除
                    win.confirm("确定删除 [" + obj.data.TrackName + "] 吗？", function () {
                        obj.data.SaleOrderId = saleOrderId;
                        $.postdata("/clientApi/WarehouseSaleTrack/DeleteSaleTrack", obj.data, function (data) {
                            if (data) {
                                win.msg("删除成功");
                                self.render();
                            } else {
                                win.alert("删除失败");
                            }
                        });
                    });
                }
            });
        },
        verify: function () {
            form.verify({});
        }
    };
    objTable.init();
});