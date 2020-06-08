$(function () {
    var buyOrderId = getQueryValue("buyOrderId");

    var objTable = {
        tableId: "tableList",
        searchKeys: {},
        searchBarHeight: win.searchBarHeight,
        category: [],
        init: function () {
            var self = this;
            self.initData();
            self.search();
            self.render();
            self.operator();
            self.optDivInit();
        },
        // search初始化
        initData: function () {
            var self = this;
            $.getdata("/clientApi/UserProduct/GetCategoryList", {}, function (res) {
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
                $.each(rows, function(i, item){
                    var s = "";
                    if(item.Depth>1){
                        for(var j=0;j<item.Depth-1;j++){
                            s+="│";
                        }
                        s+="└";
                    }
                    arr.push('<option value="'+item.CategoryId+'">'+s+item.CategoryName+'</option>');
                });
                $('[name="CategoryId"]').html(arr.join(""));
                self.category = res;
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
                    url: "/clientApi/WarehouseBuyOrder/GetProductSelectList?buyOrderId="+buyOrderId,
                    page: true,
                    height: "full-" + self.searchBarHeight,
                    where: self.searchKeys,
                    cols: [
                        [listField.numbers,
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
                            listField.priceA,
                            listField.priceB,
                            listField.priceC,
                            listField.imageQty,
                            listField.areaPositionSelect,
                            listField.priceAvgFixed,
                            {
                                field: "operator",
                                title: "操作",
                                width: 60,
                                fixed: "right",
                                align: "center",
                                toolbar: "#operator"
                            }
                        ]
                    ],
                    doneOk: function(){
                        $(".disRow").each(function(){
                            $(this).parent().parent().parent().addClass("disRow");
                        });
                        
                        //显示数量
                        $.showQty(buyOrderId);

                        $("#optDiv").addClass("hide");
                    },
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
                if (layEvent === "addPro") {
                    var top = $(tr).offset().top
                    $("#optDiv").css({"top": top+40+"px", "right": 80+'px'}).removeClass("hide");
                    $('#optDiv [name="InQty"]').focus();

                    $('#optDiv [name="InQty"]').val('');
                    // $('#optDiv [name="Price"]').val('');
                    $('#optDiv [name="InStockRemark"]').val('');

                    //提交数据
                    form.on("submit(optDivBtnFilter)", function (da) {
                        da.field.BuyOrderId = buyOrderId;
                        da.field.ProductId = data.ProductId;
                        $.postdata("/clientApi/WarehouseBuyOrder/AddOrderProduct", da.field, function (res) {
                            if(res){
                                $("#optDiv").addClass("hide");
                                win.msg("添加成功");
                                self.render();
                            }else{
                                win.alert("添加失败");
                            }
                        });
                        return false;
                    });
                }
            });
        },
        optDivInit: function(){
            var self = this;
            $("#optDiv .optClose").unbind("click").on("click", function(){
                $("#optDiv").addClass("hide");
            });
        }
    };
    objTable.init();

});
