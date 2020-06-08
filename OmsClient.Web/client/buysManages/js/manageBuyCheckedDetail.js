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
            $.getdata('/clientApi/ManageBuyOrder/GetBuyOrderDetail?buyOrderId='+buyOrderId, {}, function(res){
                objBuyDetail.setBuyDetailValue(res);
            });
        },
        // 显示表格
        render: function () {
            var self = this;
            $.tableObject({
                tableId: self.tableId,
                tableOption: {
                    url: "/clientApi/ManageBuyOrder/GetBuyOrderDetailProduct?buyOrderId="+buyOrderId,
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
                        listField.priceAvg,
                        listField.imageQty,
                        listField.areaPosition,
                        listField.qty,
                        listField.price,
                        listField.remark,
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
            $.getdata('/clientApi/ManageBuyOrder/GetBuyPayRrcord?buyOrderId=' + buyOrderId, {}, function (res) {
                objBuyDetail.payList(res);
            });
        },
    };

    //
    var objOpt = {
        init: function(){
            var self = this;
            self.check();
        },
        check: function(){
            var self = this;
            
        },
    }
    
    $('#orderBody').load('../static/templates/buyOrder.html', function(){
        objOrder.init(function(){
            objOpt.init();
        });
    });
});
