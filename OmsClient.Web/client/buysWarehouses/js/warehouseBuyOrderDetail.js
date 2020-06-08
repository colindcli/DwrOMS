//
$(function () {
    var buyOrderId = getQueryValue("buyOrderId");
    
    var objOrder = {
        tableId: "tableList",
        init: function (callback) {
            var self = this;
            self.initData();
            self.render();
            self.initEvent();
            
            self.finance();
            
            self.addProduct();

            if(callback){
                callback.call(new Object());
            }
        },
        // 初始化数据
        initData: function () {
            var self = this;
            //更新数据
            $.getdata('/clientApi/WarehouseBuyOrder/GetBuyOrderDetail?buyOrderId='+buyOrderId, {}, function(res){
                objBuyDetail.setBuyDetailValue(res);
            });
        },
        //添加产品
        addProduct: function(){
            var self = this;
            $("#btnAddProduct").unbind("click").on("click", function(){
                win.iframe({
                    title: "添加产品",
                    url: 'warehouseBuyOrderSelect.html?buyOrderId='+buyOrderId,
                    width: $(window).width()-50,
                    height: $(window).height() - 50,
                    done: function () {
                        
                    },
                    close: function(){
                        self.render();
                        self.initData();
                    }
                });
            });
        },
        // 显示表格
        render: function () {
            var self = this;
            $.tableObject({
                tableId: self.tableId,
                tableOption: {
                    url: "/clientApi/WarehouseBuyOrder/GetBuyOrderDetailProduct?buyOrderId="+buyOrderId,
                    page: false,
                    width: $(window).width()-22,
                    height: "full-" + self.searchBarHeight,
                    where: self.searchKeys,
                    cols: [
                        [{
                            field: "checked",
                            title: '<input type="checkbox" class="selectAll" lay-ignore />',
                            width: 35,
                            fixed: "left",
                            templet: '#SelectItem'
                        },
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
                        listField.imageQty,
                        listField.areaPosition,
                        {
                            field: "Qty",
                            title: "采购数量",
                            width: 65,
                            align: 'center',
                            fixed: "right"
                        },
                        listField.remarkFixed,
                        {
                            field: "InQty",
                            title: "入库数量",
                            width: 65,
                            align: 'center',
                            fixed: "right",
                            templet: '#InQtyEdit'
                        },
                        {
                            field: "InQty",
                            title: "入库备注",
                            fixed: "right",
                            templet: '#InStockRemarkEdit'
                        },
                    ]
                    ],
                    doneOk: function(a){
                        self.initEvent(a.count>0);
                        //显示数量
                        $.showQty(buyOrderId);

                        $('.selectRow').parent().parent().parent().parent().addClass('selectRowNew');
                    }
                }
            });
        },//table操作
        initEvent: function(hasRow){
            var self = this;
            if(hasRow){
                $(".proShow").removeClass("hide");
            } else{
                $(".proShow").addClass("hide");
            }

            //全选
            $(".selectAll").unbind("click").on("click", function(){
                $(".selectRow").prop("checked", $(this).prop("checked"));
            });

            //删除
            $("#btnDeleteProduct").unbind("click").on("click", function(){
                var ids = [];
                $('.layui-table-fixed .selectRow').each(function(){
                    if($(this).prop("checked")){
                        ids.push($(this).data("id"));
                    }
                });

                if(ids.length==0){
                    win.alert("请勾选要删除的行");
                    return;
                }
                
                win.confirm('确定删除选中项吗？', function(){
                    $.postdata('/clientApi/WarehouseBuyOrder/DeleteOrderProduct', {
                        BuyOrderId: buyOrderId,
                        BuyOrderProductIds: ids,
                    }, function(res){
                        if(res){
                            win.msg("删除成功");
                            self.render();
                            self.initData();
                        } else {
                            win.alert("删除失败");
                        }
                    });
                });
            });

            //编辑
            $("#btnEditProduct").unbind("click").on("click", function(){
                $(".proShow").addClass("hide");
                $(".proEdit").removeClass("hide");

                //数据复位
                $(".txtQty,.txtRemark").each(function(){
                    $(this).val($(this).data("defaultvalue"));
                });
            });

            //取消
            $("#btnCancelProduct").unbind("click").on("click", function(){
                $(".proEdit").addClass("hide");
                $(".proShow").removeClass("hide");
                
                //数据复位
                $(".txtQty,.txtRemark").each(function(){
                    $(this).val($(this).data("defaultvalue"));
                });
            });

            //更新：入库数量、入库备注
            $("#btnPostProduct").unbind("click").on("click", function(){
                var ids = [];
                var qtys = [];
                var htmlArea = $('.layui-table-fixed-r').hasClass('layui-hide')? '.layui-table-main': '.layui-table-fixed-r';
                $(htmlArea+" .txtQty").each(function(){
                    ids.push($(this).data('id'));
                    var qty = parseInt($(this).val());
                    qtys.push(qty);
                });

                var remarks = [];
                $(htmlArea+" .txtRemark").each(function(){
                    remarks.push($(this).val());
                });

                var list = [];
                $.each(ids, function(i, item){
                    list.push({
                        BuyOrderProductId: ids[i],
                        InQty: qtys[i],
                        InStockRemark: remarks[i],
                    })
                });
                var param = { BuyOrderId: buyOrderId, BuyOrderProducts: list}

                $.postdata('/clientApi/WarehouseBuyOrder/UpdateOrderProduct', param, function(res){
                    if(res){
                        win.msg("更新成功");
                        $(".proEdit").addClass("hide");
                        $(".proShow").removeClass("hide");
                        self.render();
                        self.initData();
                    } else {
                        win.alert('更新失败');
                    }
                });
            });
        },
        finance: function(){
            var self = this;
            $.getdata('/clientApi/WarehouseBuyOrder/GetBuyPayRrcord?buyOrderId=' + buyOrderId, {}, function (res) {
                objBuyDetail.payList(res);
            });
        },
    };

    //
    var objOpt = {
        init: function(){
            var self = this;
            self.stockIn();
        },
        //入库
        stockIn: function(){
            var self = this;
            $("#btnStockIn")
                .unbind("click")
                .on("click", function () {
                    win.open({
                        id: "stockIn",
                        title: "入库",
                        width: 500,
                        height: 300,
                        done: function () {
                            win.formFocus('stockInFormFilter');

                            form.on("submit(stockInBtnFilter)", function (data) {
                                data.field.BuyOrderId = buyOrderId;
                                win.confirm('确认入库吗？', function(){
                                    $.postdata('/clientApi/WarehouseBuyOrder/StockIn', data.field, function(res){
                                        if(res){
                                            win.msg('操作成功');
                                            tabNum();
                                        } else {
                                            win.alert('操作失败');
                                        }
                                    });
                                });
                
                                return false;
                            });
                        }
                    });
                });
            
        }
    }
    
    $('#orderBody').load('../static/templates/buyOrderStockIn.html', function(){
        objOrder.init(function(){
            objOpt.init();
        });
    });
});
