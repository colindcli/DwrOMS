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
            $.getdata('/clientApi/UserSaleOrder/GetSaleOrderDetail?saleOrderId='+saleOrderId, {}, function(res){
                objSaleDetail.setSaleDetailValue(res);
            });
        },
        // 显示表格
        render: function () {
            var self = this;
            $.tableObject({
                tableId: self.tableId,
                tableOption: {
                    url: "/clientApi/UserSaleOrder/GetSaleOrderDetailProduct?saleOrderId="+saleOrderId,
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
            $.getdata('/clientApi/UserSaleOrder/GetSaleReceiveRrcord?saleOrderId=' + saleOrderId, {}, function (res) {
                objSaleDetail.receiveList(res);
            });
        },
        track: function(){
            var self = this;
            $.getdata('/clientApi/UserSaleOrder/GetSaleTrackResult?saleOrderId=' + saleOrderId, {}, function (res) {
                objSaleDetail.trackList(res);
            });
        }
    };

    //
    var objOpt = {
        init: function(){
            var self = this;
            self.export();
        },
        export: function(){
            var self = this;
            //导出excel
            $('#btnExcel').unbind('click').on('click', function(){
                var tempForm = document.createElement("form");
                tempForm.id = "exportExcelForm";
                tempForm.action = "/clientApi/UserSaleOrder/ExportProduct?SaleOrderId="+saleOrderId;
                tempForm.target = "_blank";
                tempForm.method = "post";
                tempForm.style.display = "none";
                document.body.appendChild(tempForm);
                tempForm.submit();

                $("#exportExcelForm").remove();
                return false;
            });
        },
    }
    
    $('#orderBody').load('../static/templates/saleOrder.html', function(){
        objOrder.init(function(){
            objOpt.init();
        });
    });
});
