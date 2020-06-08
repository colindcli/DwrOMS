//
$(function () {
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
        },
        // search初始化
        initData: function () {
            var self = this;

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
                    url: "/clientApi/WarehouseBuyOrder/GetUnstockInOrder",
                    page: true,
                    height: "full-" + self.searchBarHeight,
                    where: self.searchKeys,
                    cols: [
                        [listField.numbers,
                            listField.buyOrderNumber,
                            listField.currencyName,
                            listField.supplierName,
                            listField.defaultSettlementName,
                            {
                                field: "PostPurcharseDate",
                                title: "提交采购",
                                width: 110,
                                templet: "<div>{{ d.PostPurcharseDate==null?'': d.PostPurcharseDate.ToDatetime() }}</div>"
                            },
                            {
                                field: "operator",
                                title: "操作",
                                width: 60,
                                fixed: "right",
                                align: "center",
                                toolbar: "#operator"
                            }
                        ]
                    ]
                }
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
                    win.iframe({
                        title: "订单："+data.BuyOrderNumber,
                        url: 'warehouseBuyOrderDetail.html?buyOrderId='+data.BuyOrderId,
                        done: function () {
                            //
                        },
                        close: function(){
                            self.render();
                        }
                    });
                }
            });
        },
        verify: function () {
            form.verify({
            });
        }
    };
    objTable.init();
});
