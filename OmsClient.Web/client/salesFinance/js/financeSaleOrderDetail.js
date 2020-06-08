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
            $.getdata('/clientApi/FinanceSaleOrder/GetSaleOrderDetail?saleOrderId='+saleOrderId, {}, function(res){
                objSaleDetail.setSaleDetailValue(res);
            });
        },
        // 显示表格
        render: function () {
            var self = this;
            $.tableObject({
                tableId: self.tableId,
                tableOption: {
                    url: "/clientApi/FinanceSaleOrder/GetSaleOrderDetailProduct?saleOrderId="+saleOrderId,
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
            $.getdata('/clientApi/FinanceSaleOrder/GetSaleReceiveRrcord?saleOrderId=' + saleOrderId, {}, function (res) {
                objSaleDetail.receiveList(res);
            });
        },
        track: function(){
            var self = this;
            $.getdata('/clientApi/FinanceSaleOrder/GetSaleTrackResult?saleOrderId=' + saleOrderId, {}, function (res) {
                objSaleDetail.trackList(res);
            });
        }
    };

    //
    var objOpt = {
        init: function(){
            var self = this;
            self.addRecord();
            self.postUnpay();
            self.postAfterPay();
        },
        addRecord: function(){
            var self = this;
            if($('#btnAddPay').length>0){
                $('#btnAddPay').unbind('click').on('click', function(){
                    win.iframe({
                        title: "收款记账",
                        url: 'financeSaleReceive.html?saleOrderId='+saleOrderId,
                        width: $(window).width()-50,
                        height: 500,
                        close: function(){
                            objOrder.finance();
                        }
                    });
                });
            }
        },
        postUnpay: function(){
            var self = this;
            $('[name="SelectPay"]').unbind('change').on('change', function(){
                var v = parseInt($(this).val());
                if(v==1){
                    $('#btnPostUnpay').text('标志为后收款');
                } else if(v==2){
                    $('#btnPostUnpay').text('完成收款');
                } else{
                    $('#btnPostUnpay').text('确认提交');
                }
            });
            form.on("submit(postUnpayBtnFilter)", function (data) {
                data.field.SaleOrderId = saleOrderId;
                var s = "";
                var v = parseInt(data.field.SelectPay);
                if(v === 1){
                    s = "确认标志为后收款订单吗？";
                } else if(v === 2){
                    s = "确认完成收款吗？"
                } else{
                    win.msg('请选择');
                    return false;
                }
                win.confirm(s, function(){
                    $.postdata('/clientApi/FinanceSaleOrder/PostUnpayReceive', data.field, function(res){
                        if(res){
                            win.msg('提交成功');
                        } else {
                            win.alert('提交失败');
                        }
                    });
                });

                return false;
            });
        },
        postAfterPay: function(){
            var self = this;
            form.on("submit(postAfterPayFormFilter)", function (data) {
                data.field.SaleOrderId = saleOrderId;
                win.confirm('确认完成收款吗？', function(){
                    $.postdata('/clientApi/FinanceSaleOrder/PostPayReceive', data.field, function(res){
                        if(res){
                            win.msg('提交成功');
                        } else {
                            win.alert('提交失败');
                        }
                    });
                });

                return false;
            });
        }
    }

    //
    $('#orderBody').load('../static/templates/saleOrder.html', function(){
        objOrder.init(function(){
            objOpt.init();
        });
    });
});
