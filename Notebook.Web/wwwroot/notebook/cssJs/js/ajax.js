var AjaxOperations = (function () {
    var parameters = {
        url: "",
        methodType: "get",
        dataType: "html",
        returnUrl: "",
        data: ""
    };

    AjaxOperations.prototype.run = function () {
        var _result = null;

        $.ajax({
            url: this.parameters.url,
            type: this.parameters.methodType,
            dataType: this.parameters.dataType,
            data: this.parameters.data,
            async: false,
            success: function (result) {
                _result = result;
            },
            error: function () {
                $alert("", "Hata meydana geldi!");
            }
        });

        return _result;
    };
});

$(function () {
    $("[data-action]").on("click", function () {
        var _url = "/" + $(this).attr("data-action");
        var _id = $(this).attr("data-id");
        var _returnType = $(this).attr("data-type");
        var _confirm = $(this).attr("data-confirm");
        var _methodType = $(this).attr("data-method");
        var _returnUrl = $(this).attr("data-url");
        var _data = null;

        if (isNotNull(_id)) {
            _url = _url + "?ID=" + _id;
        }

        if (isNotNull(_confirm)) {
            $confirm(_confirm).then(function (result) {
                if (result) {
                    run();
                }
            });
        }
        else {
            run();
        }

        function run() {
            var _ajaxOperations = new AjaxOperations();
            _ajaxOperations.parameters = {
                url: _url,
                methodType: _methodType,
                data: _data
            };

            var _result = _ajaxOperations.run();

            if (_result != null) {
                switch (_returnType) {
                    case "modal": {
                        $("#modalContent").html("");
                        $("#modalContent").html(_result);
                        openModal();
                        break;
                    }
                    case "content": {
                        $("#content").html(_result);
                        break;
                    }
                    case "reload": {
                        location.reload();
                        break;
                    }
                    case "redirect": {
                        location.href = _returnUrl;
                        break;
                    }
                }
            }
        }
    });
})