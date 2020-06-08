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
            $('#btnCheck').unbind('click').on('click', function(){
                win.open({
                    id: "checking",
                    title: "审核订单",
                    width: 500,
                    height: 300,
                    done: function () {
                        form.on("submit(checkingBtnFilter)", function (data) {
                            if(data.field.Pass === undefined){
                                win.msg('请选择审核');
                                return false;
                            }
                            if(data.field.Pass !== "true" && data.field.Remark === ""){
                                win.msg('请在备注输入驳回原因');
                                $('#checking [name="Remark"]').focus();
                                return false;
                            }

                            var s = data.field.Pass==="true"?'确认审核通过吗？':'确认审核驳回吗？';
                            data.field.BuyOrderId = buyOrderId;
                            win.confirm(s, function(){
                                $.postdata('/clientApi/ManageBuyOrder/CheckBuyOrder', data.field, function(res){
                                    if(res){
                                        win.msg('操作成功');
                                        objOrder.initData();
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
        },
    }
    
    $('#orderBody').load('../static/templates/buyOrder.html', function(){
        objOrder.init(function(){
            objOpt.init();
        });
    });
});
