$(function () {
    var objTable = {
        tableId: "tableList",
        searchKeys: {},
        searchBarHeight: win.searchBarHeight,
        category: [],
        init: function () {
            var self = this;
            self.category();
            self.user();
            self.search();
        },
        initCate: false,
        initUser: false,
        initAll: function(){
            var self = this;
            if(self.initCate && self.initUser){
                $('#btnSearch').click();
            }
        },
        // search初始化
        category: function () {
            var self = this;
            $.getdata("/clientApi/BuyStatistics/GetCategoryList", {}, function (res) {
                var rows = $.toTree({
                    data: res,
                    //根节点Id值
                    rootIdValue: "0",
                    //ParentId名
                    parentId: 'CategoryParentId',
                    //Id名
                    id: 'CategoryId',
                    //排除子树
                    expId: ''
                });

                var arr = ['<option value="0">不限</option>'];
                $.each(rows, function (i, item) {
                    var s = "";
                    if (item.Depth > 1) {
                        for (var j = 0; j < item.Depth - 1; j++) {
                            s += "│";
                        }
                        s += "└";
                    }
                    arr.push('<option value="' + item.CategoryId + '">' + s + item.CategoryName + '</option>');
                });
                $('[name="CategoryId"]').html(arr.join(""));
                self.category = res;

                self.initCate = true;
                self.initAll();
            });
        },
        user: function () {
            var self = this;
            $.getdata('/clientApi/BuyStatistics/GetUserList', {}, function (res) {
                //
                self.dt = res.Dates;

                //
                var arr = ['<option value="">全部</option>'];
                $.each(res.Row, function (i, item) {
                    arr.push('<option value="' + item.UserId + '">' + item
                        .UserName + '</option>')
                });
                $('[name="UserId"]').html(arr.join(''));
                
                //
                $('[name="BeginDatetime"]').val(self.dt[2]);
                $('[name="EndDatetime"]').val(self.dt[3]);

                self.event();

                self.initUser = true;
                self.initAll();
            });
        },
        event: function () {
            var self = this;
            $('#lastMonth').unbind('click').on('click', function () {
                $('[name="BeginDatetime"]').val(self.dt[0]);
                $('[name="EndDatetime"]').val(self.dt[1]);
                $('#btnSearch').click();
            });
            $('#thisMonth').unbind('click').on('click', function () {
                $('[name="BeginDatetime"]').val(self.dt[2]);
                $('[name="EndDatetime"]').val(self.dt[3]);
                $('#btnSearch').click();
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
                    url: "/clientApi/BuyStatistics/GetBuyProductStatistics",
                    page: true,
                    height: "full-" + self.searchBarHeight,
                    where: self.searchKeys,
                    cols: [
                        [
                            listField.numbers,
                            listField.category,
                            listField.productSku,
                            listField.productName,
                            listField.specification,
                            listField.productRemark,
                            listField.productWeight,
                            listField.orderQty,
                            listField.inTransitQty,
                            listField.saleQty,
                            listField.holdQty,
                            listField.price1,
                            listField.price10,
                            listField.price100,
                            listField.imageQty,
                            listField.areaPosition,
                            listField.createName,
                            listField.createDate,
                            {
                                field: "StockInDate",
                                title: "最近入库",
                                width: 110,
                                templet: "<div>{{ d.StockInDate==null?'': d.StockInDate.ToDatetime() }}</div>"
                            },
                            {
                                field: "Qty",
                                title: "销售数量",
                                fixed: "right",
                                width: 70,
                            },
                            {
                                field: "Amount",
                                title: "销售总额",
                                fixed: "right",
                                templet: "<div>￥{{ d.Amount }}</div>",
                                width: 70,
                            },
                            {
                                field: "Avg",
                                title: "销售均价",
                                fixed: "right",
                                templet: "<div>￥{{ d.Avg }}</div>",
                                width: 70,
                            },
                            
                        ]
                    ],
                    doneOk: function (a) {
                        //显示数量
                        $.showQty();
                    },
                }
            });
        },
    };
    objTable.init();

});