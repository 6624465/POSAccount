function SearchBox(SearchTable) {
    sessionStorage.setItem('SsnSearchTbl', SearchTable);
    var Obj = {
        SearchTable: SearchTable,
        whereclause: '',
        txtSearch: '',
        SearchBy: ''
    };

    SearchData(Obj);
}

function SearchData(Obj) {
    $('#modalBody').empty().css('display', 'none');
    $('#modalAni').html('<i class="fa fa-refresh fa-spin"></i>');

    $('#mySearch').modal('show');

    if (Obj.SearchTable == 'VesselSchedule') {
        var VoyageObj = {
            BranchId: '240',
            AgentCode: $('#bookingHeader_AgentCode').val(),
            BookingType: $('#bookingHeader_BookingType').val(),
            VesselCode: $('#bookingHeader_VesselCode').val(),
            ETA: '',
            searchVm: Obj
        };

        VoyageSearch(VoyageObj);
    }
    else
        ajaxCall(Obj);
}

function ajaxCall(Obj) {
    $.ajax({
        type: 'POST',
        url: searchBoxUrl,
        dataType: 'html',
        data: JSON.stringify(Obj),
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            $('#modalAni').empty();
            $('#modalBody').html(data).slideDown(700);
        },
        error: function (err) {
            debugger;
        }
    });
}

function btnSearch() {
    var whereClause = $('#searchVm_whereclause option:selected').val();
    var searchBy = $('#searchVm_SearchBy option:selected').val();
    if (whereClause != null && whereClause != '' && searchBy != null && searchBy != '') {
        var Obj = {
            SearchTable: sessionStorage.getItem('SsnSearchTbl'),
            whereclause: $('#searchVm_whereclause option:selected').val(),
            txtSearch: $('#searchVm_txtSearch').val(),
            SearchBy: $('#searchVm_SearchBy option:selected').val()
        };
        if (Obj.SearchTable == 'VesselSchedule') {
            var VoyageObj = {
                BranchId: '240',
                AgentCode: $('#bookingHeader_AgentCode').val(),
                BookingType: $('#bookingHeader_BookingType').val(),
                VesselCode: $('#bookingHeader_VesselCode').val(),
                ETA: '',
                searchVm: Obj
            };
            VoyageSearch(VoyageObj);
        }
        else
            ajaxCall(Obj);
    }
    else {
        var data = '<ul><li>Please enter all fields</li></ul>';
        $('#errMsg').html(data).slideDown(700);
    }

}