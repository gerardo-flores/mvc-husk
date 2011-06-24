//load the Seasonal Products grid..js will only be loaded on pages that have a grid
jQuery(document).ready(function () {
    jQuery("#grid").jqGrid({
        url: '/SeasonalProducts/GridData',
        datatype: "json",
        colNames: ['ID', 'Week', 'Category', 'SeasonalityIndex'],
        colModel: [
            { name: 'ID', index: 'ID', width: 55 },
   		    { name: 'Week', index: 'Week', width: 55 },
   		    { name: 'Category', index: 'Category', width: 90 },
   		    { name: 'SeasonalityIndex', index: 'SeasonalityIndex', width: 100 }
   	        ],
        rowNum: 10,
        rowList: [10, 20, 30],
        autowidth: true,
        height: 'auto',
        pager: jQuery('#pager'),
        sortname: 'ID',
        viewrecords: true,
        sortorder: "desc",
        caption: "Season Products",
        jsonReader: {
            root: "rows",
            page: "page",
            total: "total",
            records: "records",
            repeatitems: false,
            cell: "cell",
            id: "id",
            userdata: "userdata",
            subgrid: { root: "rows",
                repeatitems: true,
                cell: "cell"
            }
        }
    });
}).navGrid('#pager', { edit: false, add: false, del: false });