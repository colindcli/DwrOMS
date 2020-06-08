//
$(function () {
    var saleOrderId = getQueryValue("saleOrderId");

    var objOrder = {
        tableId: "tableList",
        init: function (callback) {
            var self = this;
            self.initData();
            self.render();
            self.finance();
            self.track();

            if(callback){
                callback.call(new Object());
            }
        },
        // 初始化数据
        initData: function () {
            var self = this;
            //更新数据
            $.getdata('/clientApi/SaleStatistics/GetSaleOrderDetail?saleOrderId='+saleOrderId, {}, function(res){
                objSaleDetail.setSaleDetailValue(res);
            });
        },
        // 显示表格
        render: function () {
            var self = this;
            $.tableObject({
                tableId: self.tableId,
                tableOption: {
                    url: "/clientApi/SaleStatistics/GetSaleOrderDetailProduct?saleOrderId="+saleOrderId,
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
                        listField.qty,
                        listField.price,
                        listField.remark,
                    ]
                    ],
                    doneOk: function(a){
                        //显示数量
                        $.showQty(saleOrderId);
                    }
                }
            });
        },
        finance: function(){
            var self = this;
            $.getdata('/clientApi/SaleStatistics/GetSaleReceiveRrcord?saleOrderId=' + saleOrderId, {}, function (res) {
                objSaleDetail.receiveList(res);
            });
        },
        track: function(){
            var self = this;
            $.getdata('/clientApi/SaleStatistics/GetSaleTrackResult?saleOrderId=' + saleOrderId, {}, function (res) {
                objSaleDetail.trackList(res);
            });
        }
    };

    //
    $('#orderBody').load('../static/templates/saleOrder.html', function(){
        objOrder.init();
    });
});
