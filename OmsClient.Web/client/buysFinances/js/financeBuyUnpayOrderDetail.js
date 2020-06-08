//
$(function () {
    var buyOrderId = getQueryValue("buyOrderId");
    
    var objOrder = {
        tableId: "tableList",
        init: function (callback) {
            var self = this;
            self.initData();
            self.render();
            
            self.finance();

            if(callback){
                callback.call(new Object());
            }
        },
        // 初始化数据
        initData: function () {
            var self = this;
            //更新数据
            $.getdata('/clientApi/FinanceBuyOrder/GetBuyOrderDetail?buyOrderId='+buyOrderId, {}, function(res){
                objBuyDetail.setBuyDetailValue(res);
            });
        },
        // 显示表格
        render: function () {
            var self = this;
            $.tableObject({
                tableId: self.tableId,
                tableOption: {
                    url: "/clientApi/FinanceBuyOrder/GetBuyOrderDetailProduct?buyOrderId="+buyOrderId,
                    page: false,
                    width: $(window).width()-22,
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
                        listField.areaPosition,
                        listField.priceAvgFixedTem,
                        listField.qty,
                        listField.inqty,
                        listField.priceFixedEdit,
                        listField.remarkFixed,
                    ]
                    ],
                    doneOk: function(a){
                        //显示数量
                        $.showQty(buyOrderId);
                    }
                }
            });
        },
        finance: function(){
            var self = this;
            $.getdata('/clientApi/FinanceBuyOrder/GetBuyPayRrcord?buyOrderId=' + buyOrderId, {}, function (res) {
                objBuyDetail.payList(res);
            });
        },
    };

    //
    var objOpt = {
        init: function(){
            var self = this;
            self.addRecord();
            self.postFinishedPay();
        },
        addRecord: function(){
            var self = this;
            if($('#btnAddPay').length>0){
                $('#btnAddPay').unbind('click').on('click', function(){
                    win.iframe({
                        title: "付款记账",
                        url: 'financeBuyPay.html?buyOrderId='+buyOrderId,
                        width: $(window).width()-50,
                        height: 500,
                        close: function(){
                            objOrder.finance();
                        }
                    });
                });
            }
        },
        //完成付款
        postFinishedPay: function(){
            var self = this;
            form.on("submit(postPayFormFilter)", function (data) {
                data.field.BuyOrderId = buyOrderId;
                win.confirm('确认完成收款吗？', function(){
                    $.postdata('/clientApi/FinanceBuyOrder/PostFininshPay', data.field, function(res){
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
    }
    
    $('#orderBody').load('../static/templates/buyOrder.html', function(){
        objOrder.init(function(){
            objOpt.init();
        });
    });
});
