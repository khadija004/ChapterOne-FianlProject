$(document).ready(function () {

    //Navbar fixed
    $(window).scroll(function () {
        var header = $('#navbar'),
            scroll = $(window).scrollTop();
        let logoImg = $(".logo img")
        let loginRegister = $(".login-register")
        if (scroll >= 150) {
            header.css({
                'position': 'fixed',
                'top': '0',
                'left': '0',
                'right': '0',
                'z-index': '99999',
                'background-color': 'white',
                'box-shadow': 'rgba(149, 157, 165, 0.2) 0px 8px 24px',
                'backdrop-filter': 'blur(10px)',
                'background': 'transparent'
            });
            logoImg.css({
                'margin-top': '26px'
            })
            loginRegister.css({
                'background-color': 'white'
            })
        } else {
            header.css({
                'position': 'relative',
                'box-shadow': 'none'
            });
            logoImg.css({
                'margin-top': '40px'
            })
        }
    });


    let search = document.querySelector(".search i")
    search.addEventListener("click", function () {
        document.querySelector(".search-input").classList.toggle("d-none")
    })

    let login = document.querySelector(".user i")
    login.addEventListener("click", function () {
        document.querySelector(".login-register").classList.toggle("d-none")
    })

    //login-registerdə body-ə vuranda işləməsi üçün js
    document.addEventListener("click", function (e) {
        if (!!!e.target.closest(".user")) {
            if (!$(".login-register").hasClass("d-none")) {
                $(".login-register").addClass("d-none")
            }
        }
    })

    //Bir-başa headerə qaytarn icon
    $('#topbtn').click(function () {
        $('html').animate({
            scrollTop: 0
        }, 100)

    })

    //Phone-tablet navbars search
    let searchPhone = document.querySelector("#navbar-phone .icons ul li .search")
    searchPhone.addEventListener("click", function () {
        document.querySelector("#navbar-phone .search-input").classList.toggle("d-none")
    })

    //Phone-tablet navbars menu
    let hamburgerMenu = document.querySelector("#navbar-phone .nav .hamburger-icon i")
    hamburgerMenu.addEventListener("click", function () {
        document.querySelector("#navbar-phone .hamburger-menu").classList.toggle("d-none")
    })


    //Active menu navbar
    var menuItems = document.querySelectorAll("#pages ul li a");

    menuItems.forEach(function (item) {
        item.addEventListener("click", function (event) {

            menuItems.forEach(function (item) {
                item.classList.remove("active-menu");
            });

            event.target.classList.add("active-menu");
        });
    });


    //MAIN SEARCH

    //$(document).on("submit", ".hm-searchbox", function (e) {
    //    e.preventDefault();
    //    let value = $(".input-search").val();
    //    let url = `/shop/MainSearch?searchText=${value}`;

    //    window.location.assign(url);

    //})



    $(function () {

        //add cart

        $(document).on("click", ".cart-add", function (e) {
            console.log(this)
            let id = $(this).parent().attr("data-id");
            let data = { id: id };
            let count = (".basket-count");
            $.ajax({
                type: "Post",
                url: "/Shop/AddToCart",
                data: data,
                success: function (res) {
                    $(count).text(res);
                }
            })
            return false;
        })

        //phone
        $(document).on("click", ".cart-add", function (e) {
            console.log(this)
            let id = $(this).parent().attr("data-id");
            let data = { id: id };
            let count = (".basketNumber");
            $.ajax({
                type: "Post",
                url: "/Shop/AddToCart",
                data: data,
                success: function (res) {
                    $(count).text(res);
                }
            })
            return false;
        })




        //delete product from basket
        $(document).on("click", ".delete-cart", function () {
            let id = $(this).parent().parent().attr("data-id");
            let prod = $(this).parent().parent();
            let tbody = $(".table-body").children();
            let data = { id: id };

            $.ajax({
                type: "Post",
                url: `Cart/DeleteDataFromBasket`,
                data: data,
                success: function (res) {
                    if ($(tbody).length == 1) {
                        $(".basket-products").addClass("d-none");
                        $(".show-alert").removeClass("d-none")
                    }
                    $(prod).remove();
                    res--;
                    $(".basket-count").text(res)
                    grandTotal();
                    //$(".show-alert").removeClass("d-none")

                }

            })
            return false;
        })


        //delete product from basket
        $(document).on("click", ".delete-cart", function () {
            let id = $(this).parent().parent().attr("data-id");
            let prod = $(this).parent().parent();
            let tbody = $(".table-body").children();
            let data = { id: id };

            $.ajax({
                type: "Post",
                url: `Cart/DeleteDataFromBasket`,
                data: data,
                success: function (res) {
                    if ($(tbody).length == 1) {
                        $(".basket-products").addClass("d-none");
                        $(".show-alert").removeClass("d-none")
                    }
                    $(prod).remove();
                    res--;
                    $(".basketNumber").text(res)
                    grandTotal();
                    //$(".show-alert").removeClass("d-none")

                }

            })
            return false;
        })

        function grandTotal() {
            let tbody = $(".table-body").children()

            let sum = 0;
            for (var prod of tbody) {
                let price = parseFloat($(prod).children().eq(4).children().eq(1).text())
                console.log(price)
                sum += price
            }
            $(".grand-total").text(sum + ".00");
        }

    })


    //change product count
    $(document).on("click", ".inc", function () {
        let id = $(this).parent().parent().parent().attr("data-id");
        let nativePrice = parseFloat($(this).parent().parent().prev().children().eq(1).text());
        let total = $(this).parent().parent().next().children().eq(1);
        let count = $(this).parent().prev().children().eq(0);

        $.ajax({
            type: "Post",
            url: `Cart/IncrementProductCount?id=${id}`,
            success: function (res) {
                res++;
                subTotal(res, nativePrice, total, count)
                grandTotal();
            }
        })
    })

    $(document).on("click", ".dec", function () {
        let id = $(this).parent().parent().parent().attr("data-id");
        let nativePrice = parseFloat($(this).parent().parent().prev().children().eq(1).text());
        let total = $(this).parent().parent().next().children().eq(1);
        let count = $(this).parent().next().children().eq(0);

        $.ajax({
            type: "Post",
            url: `Cart/DecrementProductCount?id=${id}`,
            success: function (res) {
                if ($(count).val() == 1) {
                    return;
                }
                res--;
                subTotal(res, nativePrice, total, count)
                grandTotal();
            }
        })
    })


    function grandTotal() {
        let tbody = $(".table-body").children()

        let sum = 0;
        for (var prod of tbody) {
            let price = parseFloat($(prod).children().eq(4).children().eq(1).text())
            console.log(price)
            sum += price
        }
        $(".grand-total").text(sum + ".00");
    }

    function subTotal(res, nativePrice, total, count) {
        $(count).val(res);
        let subtotal = parseFloat(nativePrice * $(count).val());
        $(total).text(subtotal + ".00");
    }



    $(function () {

        //add wishlist

        $(document).on("click", ".wishlist-add", function (e) {

            let id = $(this).parent().attr("data-id");
            let data = { id: id };
            let count = (".wishlist-count");
            $.ajax({
                type: "Post",
                url: "/Shop/AddToWishlist",
                data: data,
                success: function (res) {
                    $(count).text(res);
                }
            })
            return false;
        })


        $(document).on("click", ".wishlist-add", function (e) {

            let id = $(this).parent().attr("data-id");
            let data = { id: id };
            let count = (".wishlistNumber");
            $.ajax({
                type: "Post",
                url: "/Shop/AddToWishlist",
                data: data,
                success: function (res) {
                    $(count).text(res);
                }
            })
            return false;
        })

        //delete product from wishlist
        $(document).on("click", ".delete-wishlist", function () {

            let id = $(this).parent().parent().attr("data-id");

            let product = $(this).parent().parent();
            let tablebody = $(".wishlist-table-body").children();
            let data = { id: id };
            $.ajax({
                type: "Post",
                url: `wishlist/DeleteDataFromWishlist`,
                data: data,
                success: function (res) {
                    if ($(tablebody).length == 1) {
                        $(".wishlist-products").addClass("d-none");
                        $(".show-alert").removeClass("d-none")
                    }
                    $(product).remove();
                    res--;
                    $(".wishlist-count").text(res)
                }
            })
            return false;
        })


        //delete product from wishlist
        $(document).on("click", ".delete-wishlist", function () {

            let id = $(this).parent().parent().attr("data-id");

            let product = $(this).parent().parent();
            let tablebody = $(".wishlist-table-body").children();
            let data = { id: id };
            $.ajax({
                type: "Post",
                url: `wishlist/DeleteDataFromWishlist`,
                data: data,
                success: function (res) {
                    if ($(tablebody).length == 1) {
                        $(".wishlist-products").addClass("d-none");
                        $(".show-alert").removeClass("d-none")
                    }
                    $(product).remove();
                    res--;
                    $(".wishlistNumber").text(res)
                }
            })
            return false;
        })

    })






    $(function () {
        //add to wishlist detail
        $(document).on("click", ".add-to-wishlist-detail", function (e) {

            let id = $(this).attr("data-id");
            let data = { id: id };
            let count = (".wishlist-count");
            $.ajax({
                type: "Post",
                url: "/Shop/AddToWishlist",
                data: data,
                success: function (res) {
                    $(count).text(res);
                }
            })
            return false;
        })

    })


    $(function () {
        //add cart (product detail)
        $(document).on("click", ".addCart", function (e) {
            let id = $(this).attr("data-id");
            console.log(id)
            let data = { id: id };
            let count = (".basket-count");
            $.ajax({
                type: "Post",
                url: "/Shop/AddToCart",
                data: data,
                success: function (res) {
                    $(count).text(res);
                }
            })
            return false;
        })

    })


    //search
    /*  let searchInput = $(".search-input");*/
    let rightIcons = $(".right-icons");
    let navMenu = $(".nav-main-menu");
    let social = $(".social-icons");


    $(".search-icon").on("click", function (e) {
        $(rightIcons).css({ 'opacity': '0' });
        $(navMenu).css({ 'opacity': '0' });
        $(social).css({ 'opacity': '0' });
        $(searchInput).css({ 'opacity': '1', 'z-index': '5' });
    })

    $(".close-icon").on("click", function () {
        $(rightIcons).css({ 'opacity': '1' });
        $(navMenu).css({ 'opacity': '1' });
        $(social).css({ 'opacity': '1' });
        $(searchInput).css({ 'opacity': '0', 'z-index': '-5' });
        $(".search-input input").val("");
        $(".not-found").css({ 'display': 'd-none' });

    })




})

let phoneLogin = document.querySelector("#navbar-phone .icons .phone-login")
phoneLogin.addEventListener("click", function () {
    document.querySelector("#navbar-phone .phone-login-register").classList.toggle("d-none")
})


