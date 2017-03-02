var gviewtype = "";
$("#printTabletileview").show();
$("#printTable").hide();
$('#search-criteria').val("");
////Load all MyRequet items
$('#divLoading').show();
var Active = true;
var Refresh = false;
//Load Roles table
$.ajax({
    url: '@Url.Action("PartialAnnouncement", "Announcement")',
    data: JSON.stringify({ "Active": Active, "Refresh": Refresh }),
    contentType: 'application/json; charset=utf-8',
    type: 'Post',
    async: false,
    success: function (data) {
        $("#DivPartialRequest").empty().append(data);
        setTimeout(function () { $('#divLoading').hide(); }, 1000);
        divTileView_click();
    },
    error: function () {
        $('#divLoading').hide();

    }
});
function Active() {

    $('#search-criteria').val("");
    ////Load all MyRequet items
    $('#divLoading_Refresh').show();
    var Active = true;
    var Refresh = false;
    //Load Roles table
    $.ajax({
        url: '@Url.Action("PartialAnnouncement", "Announcement")',
        data: JSON.stringify({ "Active": Active, "Refresh": Refresh }),
        contentType: 'application/json; charset=utf-8',
        type: 'Post',
        async: false,
        success: function (data) {
            $("#DivPartialRequest").empty().append(data);
            setTimeout(function () { $('#divLoading_Refresh').hide(); }, 1000);

            if (gviewtype === "ClassicView") {
                setTimeout(function () {
                    divClassicView_click();
                }, 100);
            }
            if (gviewtype === "TileView") {
                divTileView_click();
            }
        },
        error: function () {
            $('#divLoading_Refresh').hide();

        }
    });
}
function InActive() {
    $('#search-criteria').val("");
    ////Load all MyRequet items
    $('#divLoading_Refresh').show();
    var Active = false;
    var Refresh = false;
    //Load Roles table
    $.ajax({
        url: '@Url.Action("PartialAnnouncement", "Announcement")',
        data: JSON.stringify({ "Active": Active, "Refresh": Refresh }),
        contentType: 'application/json; charset=utf-8',
        type: 'Post',
        async: false,
        success: function (data) {
            $("#DivPartialRequest").empty().append(data);
            setTimeout(function () { $('#divLoading_Refresh').hide(); }, 1000);

            if (gviewtype === "ClassicView") {
                setTimeout(function () {
                    divClassicView_click();
                }, 100);
            }
            if (gviewtype === "TileView") {
                divTileView_click();
            }
        },
        error: function () {
            $('#divLoading_Refresh').hide();

        }
    });
}

function SearchDiv() {
    $('.col-sm-4').hide();
    var txt = $('#search-criteria').val();
    $('.col-sm-4:contains("' + txt + '")').show();
    $('.col-sm-4').each(function () {
        if ($(this).text().toUpperCase().indexOf(txt.toUpperCase()) != -1) {
            $(this).show();
        }
    });
}
// For display Classic view
function divClassicView_click() {
    try {
        document.getElementById("DivClassicView").setAttribute("style", "display:block;");
        document.getElementById("DivTileView").setAttribute("style", "display:none;");
        document.getElementById("DivTileSearch").setAttribute("style", "display:none;");
        //document.getElementById("printTable").setAttribute("style", "display:block;");
        document.getElementById("ddlExportdata").setAttribute("style", "display:block;");
        document.getElementById("printTabletileview").setAttribute("style", "display:none;");
        document.getElementById("printTable").setAttribute("style", "display:block;");
        //document.getElementById("DivMain").setAttribute("style", "margin-top:0px;");
        gviewtype = "ClassicView";
    } catch (e) {

    }
}
// For display tile view
function divTileView_click() {
    try {
        document.getElementById("DivClassicView").setAttribute("style", "display:none;");
        document.getElementById("DivTileView").setAttribute("style", "display:block;");
        document.getElementById("DivTileSearch").setAttribute("style", "display:block;");
        //document.getElementById("printTable").setAttribute("style", "display:none;");
        document.getElementById("ddlExportdata").setAttribute("style", "display:none;");
        document.getElementById("DivMain").setAttribute("style", "top:-45px;position:relative;");
        //document.getElementById("DivMain").setAttribute("style", "position:relative;");
        document.getElementById("printTabletileview").setAttribute("style", "display:block;");
        document.getElementById("printTable").setAttribute("style", "display:none;");
        gviewtype = "TileView";
    } catch (e) {

    }
}
// For export data.
function printTable() {
    var printContent = document.getElementById("example");
    var num;
    var uniqueName = new Date();
    var windowName = 'Print' + uniqueName.getTime();
    var printWindow = window.open(num, windowName, 'left=50000,top=50000,width=0,height=0');
    printWindow.document.write(printContent.outerHTML);
    printWindow.document.close();
    printWindow.focus();
    printWindow.print();
    printWindow.close();
}

function printTabletileview() {
    var divContents = $("#exampletile").html();
    var printWindow = window.open('', '', 'height=400,width=800');
    printWindow.document.write('<html><head> <style type="text/css">body { font: 12px "Helvetica Neue",Helvetica,Arial,sans-serif;} .col-sm-4 {width: 30%;float: left;padding-right: 15px; }.boxdiv {   color: #fff;    float: left;    font-size: 17px;    margin: 0 0 20px;    min-height: 120px;    padding: 10px;    position: relative;    width: 100%;}.boxdiv span.display-name {  left: 10px;    position: absolute;    width: 80%;}#lblCatergoryCount {    font-size: 30px;    line-height: initial;    position: absolute;    right: 30px;}ul li{list-style:none;}ul#itemContainer li a {background: none repeat scroll 0 0 #5ebd5e; text-decoration:none; float: left; margin-bottom: 25px; width: 100%;} </style> <title>DIV Contents</title>');
    printWindow.document.write('</head><body >');
    //printWindow.document.write('<ul id="itemContainer" style="min-height: 0px; padding: 0 20px; margin:0;"><li style="display: list-item; opacity: 1;list-style: outside none none;"><div class="col-sm-4"><div><a title="Edit"><div class="boxdiv"><i class="fa icon-bubbles bg-icon"></i><span class="display-name"> Other problems </span><label style="float:right;" id="lblCatergoryCount">1</label></div></a></div></div></li>');
    printWindow.document.write(divContents);
    printWindow.document.write('</body></html>');
    printWindow.document.close();
    printWindow.print();
}

// For Refresh the page
function Refresh() {

    //  location.reload();
    $('#search-criteria').val("");
    ////Load all MyRequet items
    $('#divLoading_Refresh').show();

    var Active = true;
    var Refresh = true;
    //Load Roles table
    $.ajax({
        url: '@Url.Action("PartialAnnouncement", "Announcement")',
        data: JSON.stringify({ "Active": Active, "Refresh": Refresh }),
        contentType: 'application/json; charset=utf-8',
        type: 'Post',
        async: false,
        success: function (data) {
            $("#DivPartialRequest").empty().append(data);
            setTimeout(function () { $('#divLoading_Refresh').hide(); }, 1000);

            if (gviewtype === "ClassicView") {
                setTimeout(function () {
                    divClassicView_click();
                }, 100);
            }
            if (gviewtype === "TileView") {
                divTileView_click();
            }
        },
        error: function () {
            $('#divLoading_Refresh').hide();

        }
    });
}