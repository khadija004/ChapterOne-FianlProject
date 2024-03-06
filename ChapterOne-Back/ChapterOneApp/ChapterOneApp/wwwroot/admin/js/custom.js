$(function(){
    $(document).on("click", ".delete-slider", function (e) {
        e.preventDefault();
        let Id = $(this).parent().parent().attr("data-id");
        let deletedElem = $(this).parent().parent();
        let data = { id: Id };

        $.ajax({
            url: "slider/Delete",
            type: "post",
            data: data,
            success: function (res) {

                $(deletedElem).remove();
                $(".tooltip-inner").remove();
                $(".arrow").remove();
                if ($(tbody).length == 1) {
                    $(".table").remove();
                }
            }
        })
    })


    $(document).on("click", ".delete-our", function (e) {
        e.preventDefault();
        let Id = $(this).parent().parent().attr("data-id");
        let deletedElem = $(this).parent().parent();
        let data = { id: Id };

        $.ajax({
            url: "our/Delete",
            type: "post",
            data: data,
            success: function (res) {

                $(deletedElem).remove();
                $(".tooltip-inner").remove();
                $(".arrow").remove();
                if ($(tbody).length == 1) {
                    $(".table").remove();
                }
            }
        })
    })


    $(document).on("click", ".delete-autobiographyone", function (e) {
        e.preventDefault();
        let Id = $(this).parent().parent().attr("data-id");
        let deletedElem = $(this).parent().parent();
        let data = { id: Id };

        $.ajax({
            url: "autobiographyone/Delete",
            type: "post",
            data: data,
            success: function (res) {

                $(deletedElem).remove();
                $(".tooltip-inner").remove();
                $(".arrow").remove();
                if ($(tbody).length == 1) {
                    $(".table").remove();
                }
            }
        })
    })


    $(document).on("click", ".delete-autobiographytwo", function (e) {
        e.preventDefault();
        let Id = $(this).parent().parent().attr("data-id");
        let deletedElem = $(this).parent().parent();
        let data = { id: Id };

        $.ajax({
            url: "autobiographytwo/Delete",
            type: "post",
            data: data,
            success: function (res) {

                $(deletedElem).remove();
                $(".tooltip-inner").remove();
                $(".arrow").remove();
                if ($(tbody).length == 1) {
                    $(".table").remove();
                }
            }
        })
    })


    $(document).on("click", ".delete-brand", function (e) {
        e.preventDefault();
        let Id = $(this).parent().parent().attr("data-id");
        let deletedElem = $(this).parent().parent();
        let data = { id: Id };

        $.ajax({
            url: "brand/Delete",
            type: "post",
            data: data,
            success: function (res) {

                $(deletedElem).remove();
                $(".tooltip-inner").remove();
                $(".arrow").remove();
                if ($(tbody).length == 1) {
                    $(".table").remove();
                }
            }
        })
    })


    $(document).on("click", ".delete-gallery", function (e) {
        e.preventDefault();
        let Id = $(this).parent().parent().attr("data-id");
        let deletedElem = $(this).parent().parent();
        let data = { id: Id };

        $.ajax({
            url: "gallery/Delete",
            type: "post",
            data: data,
            success: function (res) {

                $(deletedElem).remove();
                $(".tooltip-inner").remove();
                $(".arrow").remove();
                if ($(tbody).length == 1) {
                    $(".table").remove();
                }
            }
        })
    })


    $(document).on("click", ".delete-wrapper", function (e) {
        e.preventDefault();
        let Id = $(this).parent().parent().attr("data-id");
        let deletedElem = $(this).parent().parent();
        let data = { id: Id };

        $.ajax({
            url: "wrapper/Delete",
            type: "post",
            data: data,
            success: function (res) {

                $(deletedElem).remove();
                $(".tooltip-inner").remove();
                $(".arrow").remove();
                if ($(tbody).length == 1) {
                    $(".table").remove();
                }
            }
        })
    })


    $(document).on("click", ".delete-autobiographythree", function (e) {
        e.preventDefault();
        let Id = $(this).parent().parent().attr("data-id");
        let deletedElem = $(this).parent().parent();
        let data = { id: Id };

        $.ajax({
            url: "autobiographythree/Delete",
            type: "post",
            data: data,
            success: function (res) {

                $(deletedElem).remove();
                $(".tooltip-inner").remove();
                $(".arrow").remove();
                if ($(tbody).length == 1) {
                    $(".table").remove();
                }
            }
        })
    })



    $(document).on("click", ".delete-autobiographyfour", function (e) {
        e.preventDefault();
        let Id = $(this).parent().parent().attr("data-id");
        let deletedElem = $(this).parent().parent();
        let data = { id: Id };

        $.ajax({
            url: "autobiographyfour/Delete",
            type: "post",
            data: data,
            success: function (res) {

                $(deletedElem).remove();
                $(".tooltip-inner").remove();
                $(".arrow").remove();
                if ($(tbody).length == 1) {
                    $(".table").remove();
                }
            }
        })
    })


    $(document).on("click", ".delete-promo", function (e) {
        e.preventDefault();
        let Id = $(this).parent().parent().attr("data-id");
        let deletedElem = $(this).parent().parent();
        let data = { id: Id };

        $.ajax({
            url: "promo/Delete",
            type: "post",
            data: data,
            success: function (res) {

                $(deletedElem).remove();
                $(".tooltip-inner").remove();
                $(".arrow").remove();
                if ($(tbody).length == 1) {
                    $(".table").remove();
                }
            }
        })
    })


    $(document).on("click", ".delete-store", function (e) {
        e.preventDefault();
        let Id = $(this).parent().parent().attr("data-id");
        let deletedElem = $(this).parent().parent();
        let data = { id: Id };

        $.ajax({
            url: "store/Delete",
            type: "post",
            data: data,
            success: function (res) {

                $(deletedElem).remove();
                $(".tooltip-inner").remove();
                $(".arrow").remove();
                if ($(tbody).length == 1) {
                    $(".table").remove();
                }
            }
        })
    })

    $(document).on("click", ".delete-brand", function (e) {
        e.preventDefault();
        let Id = $(this).parent().parent().attr("data-id");
        let deletedElem = $(this).parent().parent();
        let data = { id: Id };

        $.ajax({
            url: "brandtwo/Delete",
            type: "post",
            data: data,
            success: function (res) {

                $(deletedElem).remove();
                $(".tooltip-inner").remove();
                $(".arrow").remove();
                if ($(tbody).length == 1) {
                    $(".table").remove();
                }
            }
        })
    })


    $(document).on("click", ".delete-team", function (e) {
        e.preventDefault();
        let Id = $(this).parent().parent().attr("data-id");
        let deletedElem = $(this).parent().parent();
        let data = { id: Id };

        $.ajax({
            url: "team/Delete",
            type: "post",
            data: data,
            success: function (res) {

                $(deletedElem).remove();
                $(".tooltip-inner").remove();
                $(".arrow").remove();
                if ($(tbody).length == 1) {
                    $(".table").remove();
                }
            }
        })
    })


    $(document).on("click", ".delete-tag", function (e) {
        e.preventDefault();
        let Id = $(this).parent().parent().attr("data-id");
        let deletedElem = $(this).parent().parent();
        let data = { id: Id };

        $.ajax({
            url: "tag/Delete",
            type: "post",
            data: data,
            success: function (res) {

                $(deletedElem).remove();
                $(".tooltip-inner").remove();
                $(".arrow").remove();
                if ($(tbody).length == 1) {
                    $(".table").remove();
                }
            }
        })
    })


    $(document).on("click", ".delete-author", function (e) {
        e.preventDefault();
        let Id = $(this).parent().parent().attr("data-id");
        let deletedElem = $(this).parent().parent();
        let data = { id: Id };

        $.ajax({
            url: "author/Delete",
            type: "post",
            data: data,
            success: function (res) {

                $(deletedElem).remove();
                $(".tooltip-inner").remove();
                $(".arrow").remove();
                if ($(tbody).length == 1) {
                    $(".table").remove();
                }
            }
        })
    })


    $(document).on("click", ".delete-genre", function (e) {
        e.preventDefault();
        let Id = $(this).parent().parent().attr("data-id");
        let deletedElem = $(this).parent().parent();
        let data = { id: Id };

        $.ajax({
            url: "genre/Delete",
            type: "post",
            data: data,
            success: function (res) {

                $(deletedElem).remove();
                $(".tooltip-inner").remove();
                $(".arrow").remove();
                if ($(tbody).length == 1) {
                    $(".table").remove();
                }
            }
        })
    })


    $(document).on("click", ".delete-product", function (e) {
        e.preventDefault();
        let Id = $(this).parent().parent().attr("data-id");
        let deletedElem = $(this).parent().parent();
        let data = { id: Id };

        $.ajax({
            url: "product/Delete",
            type: "post",
            data: data,
            success: function (res) {

                $(deletedElem).remove();
                $(".tooltip-inner").remove();
                $(".arrow").remove();
                if ($(tbody).length == 1) {
                    $(".table").remove();
                }
            }
        })
    })


    $(document).on("click", ".delete-blog", function (e) {
        e.preventDefault();
        let Id = $(this).parent().parent().attr("data-id");
        let deletedElem = $(this).parent().parent();
        let data = { id: Id };

        $.ajax({
            url: "blog/Delete",
            type: "post",
            data: data,
            success: function (res) {

                $(deletedElem).remove();
                $(".tooltip-inner").remove();
                $(".arrow").remove();
                if ($(tbody).length == 1) {
                    $(".table").remove();
                }
            }
        })
    })


    $(document).on("click", ".delete-productComment", function (e) {
        e.preventDefault();
        let Id = $(this).parent().parent().attr("data-id");
        let deletedElem = $(this).parent().parent();
        let data = { id: Id };

        $.ajax({
            url: "productComment/Delete",
            type: "post",
            data: data,
            success: function (res) {

                $(deletedElem).remove();
                $(".tooltip-inner").remove();
                $(".arrow").remove();
                if ($(tbody).length == 1) {
                    $(".table").remove();
                }
            }
        })
    })


    $(document).on("click", ".delete-blogComment", function (e) {
        e.preventDefault();
        let Id = $(this).parent().parent().attr("data-id");
        let deletedElem = $(this).parent().parent();
        let data = { id: Id };

        $.ajax({
            url: "blogComment/Delete",
            type: "post",
            data: data,
            success: function (res) {

                $(deletedElem).remove();
                $(".tooltip-inner").remove();
                $(".arrow").remove();
                if ($(tbody).length == 1) {
                    $(".table").remove();
                }
            }
        })
    })

})