var Search = {
    init: function () {
        Search.resisterEvent();
    },
    resisterEvent: function () {
        $("#txtSearch").autocomplete({
            minLength: 0,
            source: function (request, response) {
                $.ajax({
                    url: '/Guest/SearchSanPham',
                    dataType: "json",
                    data: {
                        term: request.term
                    },
                    success: function (data) {
                        response(data.data);
                    }
                })
            },
            minLength: 2,
            select: function (event, ui) {
                log("Selected: " + ui.item.value + " aka " + ui.item.id);
            },
            focus: function (event, ui) {
                $("#txtSearch").val(ui.item.val);
                return false;
            },
            select: function (event, ui) {
                $("#txtSearch").val(ui.item.value);
                //$("#project-id").val(ui.item.value);
                //$("#project-description").html(ui.item.desc);
                //$("#project-icon").attr("src", "images/" + ui.item.icon);

                return false;
            }
        }).autocomplete("instance")._renderItem = function (ul, item) {
            return $("<li>")
                .append('<a href="Guest/Details?ID=' + item.MaSP + '"><img style="padding-bottom:5px;width:64px;height:64px" class="icon"  + item.Anhbia + '" />' + item.TenSP + '</a>')
                .appendTo(ul);
        };
    }
}
Search.init();