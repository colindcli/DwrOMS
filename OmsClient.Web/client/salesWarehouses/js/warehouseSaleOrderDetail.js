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
            $.getdata('/clientApi/WarehouseSaleOrder/GetSaleOrderDetail?saleOrderId=' + saleOrderId, {}, function (res) {
                objSaleDetail.setSaleDetailValue(res);

                //备货事件
                self.stockingEvent(res.StockInfo);
            });
        },
        // 显示表格
        render: function () {
            var self = this;
            $.tableObject({
                tableId: self.tableId,
                tableOption: {
                    url: "/clientApi/WarehouseSaleOrder/GetSaleOrderDetailProduct?saleOrderId=" + saleOrderId,
                    page: false,
                    width: $(window).width() - 22,
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
                    doneOk: function (a) {
                        //显示数量
                        $.showQty(saleOrderId);
                    }
                }
            });
        },
        finance: function(){
            var self = this;
            $.getdata('/clientApi/WarehouseSaleOrder/GetSaleReceiveRrcord?saleOrderId=' + saleOrderId, {}, function (res) {
                objSaleDetail.receiveList(res);
            });
        },
        track: function(){
            var self = this;
            $.getdata('/clientApi/WarehouseSaleOrder/GetSaleTrackResult?saleOrderId=' + saleOrderId, {}, function (res) {
                objSaleDetail.trackList(res);
            });
        },
        stocking: function () {
            var self = this;
            $.getdata('/clientApi/WarehouseSaleOrder/GetStockingResult?saleOrderId=' + saleOrderId, {}, function (res) {
                $('#StockStartInfo').text(res.StockStartInfo);
                $('#StockEndInfo').text(res.StockEndInfo);

                self.stockingEvent(res);
            });
        },
        stockingEvent: function(res){
            var self = this;
            if($('#btnStartStock').length>0){
                if (res.ShowStartBtn) {
                    $('#btnStartStock').removeClass('hide').unbind('click').on('click', function () {
                        win.confirm('确定开始备货吗？', function () {
                            $.postdata('/clientApi/WarehouseSaleOrder/StartStocking?saleOrderId=' + saleOrderId, {}, function (res) {
                                if (res) {
                                    self.stocking();
                                } else {
                                    win.alert('报错了')
                                }
                            });
                        });
                    });
                } else {
                    $('#btnStartStock').addClass('hide');
                }
            }

            if($('#btnEndStock').length>0){
                if (res.ShowEndBtn) {
                    $('#btnEndStock').removeClass('hide').unbind('click').on('click', function () {
                        win.confirm('确定完成备货吗？', function () {
                            $.postdata('/clientApi/WarehouseSaleOrder/EndStocking?saleOrderId=' + saleOrderId, {}, function (res) {
                                if (res) {
                                    self.stocking();
                                } else {
                                    win.alert('报错了')
                                }
                            });
                        });
                    });
                } else {
                    $('#btnEndStock').addClass('hide');
                }
            }
        },
    };

    //
    var objOpt = {
        init: function(){
            var self = this;
            self.export();
            self.trackOpt();
            self.stockOut();
        },
        export: function () {
            var self = this;
            //导出excel
            $('#btnExcel').unbind('click').on('click', function () {
                var tempForm = document.createElement("form");
                tempForm.id = "exportExcelForm";
                tempForm.action = "/clientApi/WarehouseSaleOrder/ExportStocking?SaleOrderId=" + saleOrderId;
                tempForm.target = "_blank";
                tempForm.method = "post";
                tempForm.style.display = "none";
                document.body.appendChild(tempForm);
                tempForm.submit();

                $("#exportExcelForm").remove();
                return false;
            });
        },
        trackOpt: function(){
            var self = this;
            if($('#btnTrack').length>0){
                $('#btnTrack').unbind('click').on('click', function(){
                    win.iframe({
                        title: "快递物流",
                        url: 'warehouseSaleTrack.html?saleOrderId='+saleOrderId,
                        width: $(window).width()-50,
                        height: 500,
                        close: function(){
                            objOrder.track();
                        }
                    });
                });
            }
        },
        stockOut: function(){
            var self = this;
            $('#btnStockOut').unbind('click').on('click', function(){
                win.open({
                    id: "stockOut",
                    title: "发货出库",
                    width: 500,
                    height: 300,
                    done: function () {
                        form.on("submit(stockOutBtnFilter)", function (data) {
                            data.field.SaleOrderId = saleOrderId;
                            win.confirm('确定出库吗？', function(){
                                $.postdata('/clientApi/WarehouseSaleOrder/StockOut', data.field, function(res){
                                    if(res){
                                        win.msg('出库成功');
                                        objOrder.initData();
                                    } else {
                                        win.alert('出库失败');
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

    //
    $('#orderBody').load('../static/templates/saleOrder.html', function(){
        objOrder.init(function(){
            objOpt.init();
        });
    });
});