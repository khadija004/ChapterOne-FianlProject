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
                'backdrop-filter':'blur(10px)',
                'background':'transparent'
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

    // $('#navbar .logo-pages .pages li').each(function(i, elem) {
    //     debugger
    //      let page = $(this).children(0).attr("href");
    //      let url = location.href.split("/");
    //      let urlStr = url[url.length - 1];
    //      if(page == urlStr){
    //          $("#navbar .logo-pages .pages li").removeClass("active-navbar");
    //          $(elem).addClass("active-navbar");
    //      }

    //  });

    // $('#navbar .logo-pages .pages li').click(function (e) {
    //     e.preventDefault()
    //     $("#navbar .logo-pages .pages li").removeClass("active-navbar");
    //     // $(".tab").addClass("active"); // instead of this do the below 
    //     let page = $(this).children(0).attr("href");
    //     let url = location.href.split("/");
    //     let urlStr = url[url.length - 1];
    //     let resultUrl = url.toString().replace(urlStr,page);
    //     let end = resultUrl.replaceAll(",","/")
    //     document.location = end;
    //     $(this).addClass("active-navbar");

    // });



    // $(document).ready(function () {

    //     $('#navbar .logo-pages .pages li')
    //             .click(function (e) {
    //         $('#navbar .logo-pages .pages li')
    //             .removeClass('.active');
    //         $(this).addClass('.active');
    //     });
    // });

    // $('#navbar .logo-pages .pages li a').click(function (e) {
    //     $('#navbar .logo-pages .pages li a').css("color","");
    //     $(this).css("color","red")
    // });


    var menuItems = document.querySelectorAll("#pages ul li a");

    menuItems.forEach(function (item) {
        item.addEventListener("click", function (event) {
            
            menuItems.forEach(function (item) {
                item.classList.remove("active-menu");
            });
            
            event.target.classList.add("active-menu");
        });
    });
})