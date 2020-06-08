//
$(function () {
    var saleOrderId = getQueryValue("saleOrderId");

    var objTable = {
        tableId: "tableList",
        init: function () {
            var self = this;
            self.initData();
            self.updateRate();
            self.updateEdit();
            self.addProduct();
            self.postOrder();
            self.render();
            // self.operator();
            self.initEvent();
            self.import();
            self.export();
        },
        // 初始化数据
        initData: function () {
            var self = this;
            //更新数据
            $.getdata('/clientApi/UserSaleOrder/GetSaleOrderDetail?saleOrderId='+saleOrderId, {}, function(res){
                var arr = [];
                $.each(res.Currencies, function(i, item){
                    arr.push('<option value="'+item.CurrencyId+'">'+item.CurrencyName+'（汇率'+item.CurrencyRate+'）</option>')
                });
                $('[name="CurrencyId"]').html(arr.join(''));

                //
                $("#CurrencyRate").text(res.SaleOrder.CurrencyRate);

                //
                form.val("orderEditFormFilter", $.formData(res.SaleOrder));

                //统计
                $("#CountProductWeights").text(res.Count.ProductWeights+'克');
                $("#CountProductQtys").text(res.Count.ProductQtys);
                $("#CountProductAmount").text(res.Count.CurrencySymbol+res.Count.ProductAmount);
                $("#CountShipFeight").text(res.Count.CurrencySymbol+res.Count.ShipFeight);
                $("#CountShipFee").text(res.Count.CurrencySymbol+res.Count.ShipFee);
                $("#CountDiscount").text(res.Count.CurrencySymbol+res.Count.Discount);
                $("#CountTotal").text(res.Count.CurrencySymbol+res.Count.Total);
                $("#CountTotalRmb").text('￥'+res.Count.TotalRmb);
            });
        },
        //更新汇率
        updateRate: function(){
            var self = this;
            $("#updateRate").unbind("click").on("click", function(){
                win.confirm('确定更新为当前汇率吗？', function(){
                    $.postdata('/clientApi/UserSaleOrder/UpdateSaleOrderRate', { SaleOrderId: saleOrderId }, function(res){
                        $("#CurrencyRate").text(res);
                        win.msg("更新成功");
                    });
                });
            });
        },
        //更新编辑
        updateEdit: function(){
            var self = this;
            form.on("submit(orderEditBtnFilter)", function (data) {
                $.postdata("/clientApi/UserSaleOrder/UpdateOrder", data.field, function (res) {
                    if(res){
                        win.close();
                        win.msg("更新成功");
                        self.initData();
                        self.render();
                    }else{
                        win.alert("更新失败");
                    }
                });
                return false;
            });
        },
        //添加产品
        addProduct: function(){
            var self = this;
            $("#btnAddProduct").unbind("click").on("click", function(){
                win.iframe({
                    title: "添加产品",
                    url: 'mySaleProductSelect.html?saleOrderId='+saleOrderId,
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
        //提交订单
        postOrder: function(){
            var self = this;
            form.on("submit(orderPostBtnFilter)", function (data) {
                win.confirm('确定提交订单吗？', function(){
                    $.postdata("/clientApi/UserSaleOrder/PostOrder", data.field, function (res) {
                        if(res){
                            win.close();
                            win.msg("提交成功");
                        }else{
                            win.alert("提交失败");
                        }
                    });
                    return false;
                });
                return false;
            });
            $('#btnPost').unbind('click').on('click', function(){
                $('[lay-filter="orderPostBtnFilter"]').click();
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
                        [{
                            field: "checked",
                            title: '<input type="checkbox" class="selectAll" lay-ignore />',
                            width: 35,
                            fixed: "left",
                            templet: '<div><input type="checkbox" class="selectRow" data-id="{{ d.SaleOrderProductId }}" lay-ignore /></div>'
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
                        listField.priceA,
                        listField.priceB,
                        listField.priceC,
                        listField.imageQty,
                        listField.areaPosition,
                        {
                            field: "Qty",
                            title: "数量",
                            width: 65,
                            templet: QtyEdit,
                            fixed: "right",
                        },
                        listField.priceFixedEdit,
                        {
                            field: "Remark",
                            title: "备注",
                            width: 125,
                            templet: RemarkEdit,
                            fixed: "right",
                        },
                    ]
                    ],
                    doneOk: function(a){
                        self.initEvent(a.count>0);
                        //显示数量
                        $.showQty(saleOrderId);
                    }
                }
            });
        },
        //table操作
        initEvent: function(hasRow){
            var self = this;
            if(hasRow){
                $(".proShow").removeClass("hide");
                
                //数量提醒
                $(".txtQty").unbind("click").on("click", function(){
                    var qty = $(this).data("minqty");
                    win.tips({
                        el: $(this),
                        txt: '起订量：'+qty
                    });
                    
                    //选择
                    $(this).select();
                });

                //价格提醒
                $(".txtPrice").unbind("click").on("click", function(){
                    var qty = $(this).data("minprice");
                    win.tips({
                        el: $(this),
                        txt: '最低价格：'+qty
                    });

                    //选择
                    $(this).select();
                });
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
                    $.postdata('/clientApi/UserSaleOrder/DeleteOrderProduct', {
                        SaleOrderId: saleOrderId,
                        SaleOrderProductIds: ids,
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
                $(".txtQty,.txtPrice,.txtRemark").each(function(){
                    $(this).val($(this).data("defaultvalue"));
                });
            });

            //取消
            $("#btnCancelProduct").unbind("click").on("click", function(){
                $(".proEdit").addClass("hide");
                $(".proShow").removeClass("hide");
                
                //数据复位
                $(".txtQty,.txtPrice,.txtRemark").each(function(){
                    $(this).val($(this).data("defaultvalue"));
                });
            });

            //更新：产品数量、价格、备注
            $("#btnPostProduct").unbind("click").on("click", function(){
                var ids = [];
                var qtys = [];
                var flag = true;
                var htmlArea = $('.layui-table-fixed-r').hasClass('layui-hide')? '.layui-table-main': '.layui-table-fixed-r';
                $(htmlArea+" .txtQty").each(function(){
                    ids.push($(this).data('id'));
                    var qty = parseInt($(this).val());
                    var defaultQty = parseInt($(this).data('defaultvalue'));
                    if(qty<defaultQty){
                        win.tips({
                            el: $(this),
                            txt: '起订量：'+defaultQty,
                        })
                        flag = false;
                    }
                    qtys.push(qty);
                });
                if(!flag){
                    return false;
                }

                var prices = [];
                $(htmlArea+" .txtPrice").each(function(){
                    prices.push($(this).val());
                });
                var remarks = [];
                $(htmlArea+" .txtRemark").each(function(){
                    remarks.push($(this).val());
                });

                var list = [];
                $.each(ids, function(i, item){
                    list.push({
                        SaleOrderProductId: ids[i],
                        Qty: qtys[i],
                        Price: prices[i],
                        Remark: remarks[i],
                    })
                });
                var param = { SaleOrderId: saleOrderId, SaleOrderProducts: list}

                $.postdata('/clientApi/UserSaleOrder/UpdateOrderProduct', param, function(res){
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
        import: function () {
            var self = this;
            $("#btnImportProduct")
                .unbind("click")
                .on("click", function () {
                    win.open({
                        id: "import",
                        title: "批量导入",
                        width: 600,
                        height: 380,
                        done: function () {
                            form.on("submit(importBtnFilter)", function (data) {
                                if (data.field.CategoryId == 0) {
                                    win.msg("请选择分类");
                                    return false;
                                }

                                var fileObj = document.getElementById("FileUpload").files[0]; // js 获取文件对象
                                if (typeof (fileObj) == "undefined" || fileObj.size <= 0) {
                                    alert("请选择Excel文件");
                                    return false;
                                }

                                var formFile = new FormData();
                                formFile.append("file", fileObj);
                                formFile.append("SaleOrderId", saleOrderId);

                                $.ajax({
                                    url: "/clientApi/UserSaleOrder/ImportProduct",
                                    data: formFile,
                                    type: "Post",
                                    dataType: "json",
                                    cache: false,
                                    processData: false, //用于对data参数进行序列化处理 这里必须false
                                    contentType: false, //必须
                                    success: function (result) {
                                        if (result.code != 0) {
                                            win.alert(result.msg);
                                        } else {
                                            win.close();
                                            win.msg("导入成功");
                                            self.render();
                                        }
                                    },
                                });

                                return false;
                            });
                        }
                    });
                });
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
    };
    objTable.init();
});
