$(function () {

    var NewsDotNet = {};

    NewsDotNet.GridManager = {};
    //************************* ARTICLES GRID
    NewsDotNet.GridManager.articlesGrid = function (gridName, pagerName) {

        var colNames = [
                'Id',
                'Заголовок',
                'Автор',
                'Теги',
                'Состояние',
                'Адрес',
                'Создана',
                'Изменена'
        ];

        var columns = [];

        columns.push({
            name: 'Id',
            hidden: true,
            key: true
        });
        
        columns.push({
            name: 'Title',
            index: 'Title',
            width: 260,
            formatter: function (cellvalue, options, rowObject) {
                return '<a href="/Articles/' + rowObject.AddressName + '"target="_blank">' + cellvalue + '</a>';
            }
        });

        columns.push({
            name: 'AuthorName',
            width: 90,
            align: 'center',
            sortable: false
        });

        columns.push({
            name: 'Tags',
            width: 130,
            sortable: false
            ,
            formatter: function (cellvalue, options, rowObject) {
                var tags = cellvalue;
                var tagStr = "";
                $.each(tags, function (i, t) {
                    if (tagStr) tagStr += ", "
                    tagStr += t.Name;
                })
                return tagStr;
           }
        });

        columns.push({
            name: 'State',
            index: 'State',
            width: 120,
            align: 'center',
            formatter: 'select',
            editable: true,
            edittype:"select",
            editoptions:{value:"0:Черновик;1:Опубликованная"}
        });

        columns.push({
            name: 'AddressName',
            width: 180,
            sortable: false
        });
        

        columns.push({
            name: 'CreatedTime',
            index: 'CreatedTime',
            width: 140,
            align: 'center',
            sorttype: 'date',
            datefmt: 'm/d/Y'
        });

        columns.push({
            name: 'LastChangedTime',
            index: 'LastChangedTime',
            width: 140,
            align: 'center',
            sorttype: 'date',
            datefmt: 'm/d/Y'
        });

        var lastsel2

        $(gridName).jqGrid({
            url: '/AdminArticles/Articles',
            datatype: 'json',
            mtype: 'GET',
            height: 'auto',
            toppager: true,

            colNames: colNames,
            colModel: columns,

            pager: pagerName,
            rownumbers: true,
            rownumWidth: 40,
            rowNum: 10,
            rowList: [10, 20, 30],

            sortname: 'CreatedTime',
            sortorder: 'desc',
            viewrecords: true,

            jsonReader: {
                repeatitems: false
            },

            afterInsertRow: function (rowid) {
                
                var published = $(gridName).jqGrid('getCell', rowid, 'State') == 1;

                if (!published) {
                    $(gridName).setRowData(rowid, [], {
                        color: '#9D9687'
                    });
                    $(gridName + " tr#" + rowid + " a").css({
                        color: '#9D9687'
                    });
                }
            },
            
            onSelectRow: function (rowid) {
                if (rowid && rowid !== lastsel2) {
                    $(gridName).jqGrid('restoreRow', lastsel2);
                    lastsel2 = rowid;
                }
                editparameters = {
                    "keys": true,
                    "aftersavefunc": function (rowid) {
                        var published = $(gridName).jqGrid('getCell', rowid, 'State') == 1;
                        var textColor = published ? '#000000' : '#9D9687';
                        $(gridName).setRowData(rowid, [], {
                            color: textColor
                        });
                        $(gridName + " tr#" + rowid + " a").css({
                            color: textColor
                        });
                    },
                    "errorfunc": null
                }                    
                $(gridName).jqGrid('editRow', rowid, editparameters);
            },

            editurl: '/Admin/AdminArticles/EditArticleState'

        });

        var deleteOptions = {
            url: '/AdminArticles/DeleteArticle',
            caption: 'Удалить статью',
            processData: "Сохранение...",
            msg: "Удалить статью?",
            closeOnEscape: true,
            afterSubmit: NewsDotNet.GridManager.afterSubmitHandler
        };

        var btnAddProperties = {
            caption: "",
            buttonicon:"ui-icon-plus",
            title: "Новая запись",
            onClickButton: function(){location="/Admin/AdminArticles/New";},
            position: "first",
            id: 'addBtn'
        }

        var btnEditProperties = {
            caption: "",
            buttonicon:"ui-icon-pencil",
            title: "Редактировать выбранную запись",
            onClickButton: function() {
                var selectedRow = $(gridName).jqGrid('getGridParam', 'selrow');
                if (!selectedRow)
                {
                    $( "#dialog" ).dialog();
                    return;
                }
                var addrName = $(gridName).jqGrid('getCell', selectedRow, 'AddressName');
                location = "/Admin/AdminArticles/Edit/" + addrName;
            },
            position:"first"
        }

        $(gridName).navGrid(pagerName, { cloneToTop: true, search: false, edit: false, add: false }, {}, {}, deleteOptions)
                    .navButtonAdd(gridName + "_toppager", btnEditProperties)
                    .navButtonAdd(pagerName, btnEditProperties)
                    .navButtonAdd(gridName + "_toppager", btnAddProperties)
                    .navButtonAdd(pagerName, btnAddProperties);
        ApplyPermissions();
    };
    //************************* TAGS GRID
    NewsDotNet.GridManager.tagsGrid = function (gridName, pagerName) {
        var colNames = ['ID', 'Имя', 'Адрес'];

        var columns = [];

        columns.push({
            name: 'ID',
            index: 'ID',
            hidden: true,
            sorttype: 'int',
            key: true,
            editable: false,
            editoptions: {
                readonly: true
            }
        });

        columns.push({
            name: 'Name',
            index: 'Name',
            width: 400,
            editable: true,
            edittype: 'text',
            editoptions: {
                size: 30,
                maxlength: 50
            },
            editrules: {
                required: true
            }
        });

        columns.push({
            name: 'AddressName',
            index: 'AddressName',
            width: 400,
            editable: true,
            edittype: 'text',
            editoptions: {
                size: 30,
                maxlength: 30
            },
            editrules: {
                required: true
            }
        });

        $(gridName).jqGrid({
            url: '/AdminTags/Tags',
            datatype: 'json',
            mtype: 'GET',
            height: 'auto',
            toppager: true,
            colNames: colNames,
            colModel: columns,
            pager: pagerName,
            rownumbers: true,
            rownumWidth: 40,
            rowNum: 500,
            sortname: 'Name',
            loadonce: true,
            jsonReader: {
                repeatitems: false
            }
        });

        var editOptions = {
            url: '/AdminTags/EditTag',
            editCaption: 'Редактировать тег',
            processData: "Сохранение...",
            closeAfterEdit: true,
            closeOnEscape: true,
            width: 400,
            afterSubmit: function (response, ArticleData) {
                var json = $.parseJSON(response.responseText);

                if (json) {
                    $(gridName).jqGrid('setGridParam', { datatype: 'json' });
                    return [json.success, json.message, json.id];
                }

                return [false, "Получить результат от сервера не удалось.", null];
            }
        };

        var addOptions = {
            url: '/AdminTags/AddTag',
            addCaption: 'Добавить тег',
            processData: "Сохранение...",
            closeAfterAdd: true,
            closeOnEscape: true,
            width: 400,
            afterSubmit: function (response, ArticleData) {
                var json = $.parseJSON(response.responseText);

                if (json) {
                    $(gridName).jqGrid('setGridParam', { datatype: 'json' });
                    return [json.success, json.message, json.id];
                }

                return [false, "Получить результат от сервера не удалось.", null];
            }
        };

        var deleteOptions = {
            url: '/AdminTags/DeleteTag',
            caption: 'Удалить тег',
            processData: "Сохранение...",
            width: 500,
            msg: "Удалить тег?",
            closeOnEscape: true,
            afterSubmit: NewsDotNet.GridManager.afterSubmitHandler
        };

        // configuring the navigation toolbar.
        $(gridName).jqGrid('navGrid', pagerName, {
            cloneToTop: true,
            search: false
        },

        editOptions, addOptions, deleteOptions);
    };

    NewsDotNet.GridManager.afterSubmitHandler = function (response, ArticleData) {

        var json = $.parseJSON(response.responseText);

        if (json) return [json.success, json.message, json.id];

        return [false, "Получить результат от сервера не удалось.", null];
    };

    $("#tabs").tabs({
        show: function (event, ui) {

            if (!ui.tab.isLoaded) {

                var gdMgr = NewsDotNet.GridManager,
                        fn, gridName, pagerName;

                switch (ui.index) {
                    case 0:
                        fn = gdMgr.articlesGrid;
                        gridName = "#tableArticles";
                        pagerName = "#pagerArticles";
                        break;
                    case 1:
                        fn = gdMgr.tagsGrid;
                        gridName = "#tableTags";
                        pagerName = "#pagerTags";
                        break;
                };

                fn(gridName, pagerName);
                ui.tab.isLoaded = true;
            }
        }
    });
});
