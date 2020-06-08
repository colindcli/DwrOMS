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
            $.getdata('/clientApi/WarehouseBuyOrder/GetBuyOrderDetail?buyOrderId='+buyOrderId, {}, function(res){
                objBuyDetail.setBuyDetailValue(res);
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
                        listField.imageQty,
                        listField.areaPosition,
                        listField.remark,
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
                        //显示数量
                        $.showQty(buyOrderId);

                        $('.selectRow').parent().parent().parent().parent().addClass('selectRowNew');
                    }
                }
            });
        },
        finance: function(){
            var self = this;
            $.getdata('/clientApi/WarehouseBuyOrder/GetBuyPayRrcord?buyOrderId=' + buyOrderId, {}, function (res) {
                objBuyDetail.payList(res);
            });
        },
    };

    $('#orderBody').load('../static/templates/buyOrder.html', function(){
        objOrder.init();
    });
});
