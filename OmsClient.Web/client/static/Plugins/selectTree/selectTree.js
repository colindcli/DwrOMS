$(function(){
    $.extend({
        toTree: function(opt){
            // 生成树形结构
            var objToTree = {
                option: {
                    data: [],
                    //根节点Id值
                    rootIdValue: null,
                    //ParentId名
                    parentId: 'parentId',
                    //Id名
                    id: 'id',
                    //排除子树
                    expId: null,
                    sort: 1
                },
                init: function(opt){
                    var self = this;
                    $.extend(self.option, opt);
                    return self.getTree(self.option.data, self.option.rootIdValue, self.option.expId);
                },
                child: function (list, id) { 
                    var self = this;
                    var temp = [];
                    $.each(list,function (i, item) { 
                        if(item[self.option.parentId] == id){
                            temp.push(item);
                        }
                    });
                    return temp;
                },
                toTree: function(list, rootId, depth, expId, rows){
                    var self = this;
                    if(rootId != expId){
                        var items = self.child(list, rootId);
                        $.each(items, function(i, item){
                            if(item[self.option.id] != expId){
                                item.Depth = depth;
                                item.Sort = self.option.sort;
                                self.option.sort = self.option.sort + 1;
                                rows.push(item);
                                self.toTree(list, item[self.option.id], depth+1, expId, rows);
                            }
                        });
                    }
                },
                getTree: function(data, rootId, expId){
                    var self = this;
                    var rows = [];
                    self.toTree(data, rootId, 1, expId, rows);
                    return rows;
                }
            };
            return objToTree.init(opt);
        }
    });
});