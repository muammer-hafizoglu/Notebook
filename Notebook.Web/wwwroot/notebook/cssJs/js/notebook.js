/* ----------------  General Functions ---------------------*/
$("#messageModal").modal();

// Null or Empty Control
var isNotNull = function (data) {
    if (data === undefined) {
        return false;
    }
    else if (data !== '' || data !== undefined || data !== null) {
        return true;
    }
    else {
        return false;
    }
};

// Content Print
var $print = function (contentID, title) {
    var divContents = $("#" + contentID).html();
    var printWindow = window.open('', '', 'height=800,width=1200');
    printWindow.document.write('<html><head><title>' + title + '</title>');
    printWindow.document.write('</head><body >');
    printWindow.document.write(divContents);
    printWindow.document.write('</body></html>');
    printWindow.document.close();
    printWindow.print();
};

// Snipper
var $loading = function (value) {
    var height = ($(window).height() / 2) - 60;
    var content = "<div id='loading'><i style ='margin:auto; font-size:80px; color:white; margin-top:" + height + "px' class='fa fa-spinner fa-pulse fa-3x fa-fw'></i></div>";

    if (value === "true" || value === true) {
        if ($("#loading").index() === -1) {
            $("body").append(content);
        }

        $("#loading").css("display", "block");
    }
    else {
        $("#loading").css("display", "none");
    }
};

// Modal
var $modal = function (id) {
    if (id !== undefined) {
        $("#modal" + id).modal();
    }
    else {
        $("#modal").modal({
            backdrop: "static"
        });
    }
};

var $search = function (ID) {
    $('#filter,#srcTxt').val(ID).html(ID);
}

// Bootstrap Notification
var $ntf = function (content, type = "success", _autoClose = "10000") {
    var _type = "success";
    var _signal = "check";

    switch (type) {
        case "danger":
            _type = "danger";
            _signal = "exclamation-circle";
            break;
        case "info":
            _type = "info";
            _signal = "info-circle";
            break;
    }

    var modal = "<div class='modal modal-" + _type + " fade' id='modal-ntf'><div class='modal-dialog'><div class='modal-content'>" +
        "<div class='alert alert-" + _type + "' style='margin-bottom:0' id='modal-ntf-content'><i class='fa fa-" + _signal + "'></i>   " + content + "</div></div></div></div> ";

    $("#div_modal").html("");
    $("#div_modal").html(modal);
    $("#modal-ntf").modal();

    if (!_autoClose) {
        setTimeout(function () {
            $('#modal-ntf').modal('hide');
        }, _autoClose);
    }
};

// Selected Checked
var $getChecked = function () {
    var list = [];

    for (var i = 0; i < $("[name='Select']:checked").length; i++) {
        list[i] = $("[name='Select']:checked:eq(" + i + ")").attr("data-id");
    }

    if (list !== null && list.length > 0) {
        return list;
    }
    else {
        return null;
    }

};

/* ----------------  Plugins ---------------------*/

// Jquery-Confirm Alert 
var $alert = function (title, content, size = "small", theme = "supervan", callback = null) {
    $.alert({
        title: title,
        content: content,
        theme: theme,
        closeIcon: true,
        columnClass: size,
        buttons: {
            Ok: function () {
                if (callback !== null) {
                    callback();
                }
            }
        }
    });
};

// Jquery-Confirm Dialog
var $dialog = function (title, content) {
    $.dialog({
        title: title,
        content: content,
    });
};

// Jquery-Confirm Confirm
var $confirm = function (content) {
    var defer = $.Deferred();

    $.confirm({
        title: '',
        content: content,
        theme: 'supervan',
        buttons: {
            Evet: function () {
                defer.resolve(true);
            },
            İptal: function () {
                defer.resolve(false);
            }
        }
    });

    return defer.promise();
};


// Notebook Modal
var openModal = function (data) {
    if (isNotNull(data)) {
        $("#" + data).fadeIn();
    }
    else {
        $("#modal").fadeIn();
    }
};

var closeModal = function (value) {
    if (isNotNull(value)) {
        $("#" + value).fadeOut();
    }
    else {
        $("#modal").fadeOut();
    }
};


openModal("modal");
