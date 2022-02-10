$(document).ready(function () {

    $(document).on('click', '#increment-basket-item', function () {
        $.ajax({
            type: "GET",
            url: "/Home/AddBasket?id=" + $(this).attr('data-id'),
            success: function (res) {
                $("#basket-list").empty();
                $("#basket-list").append(res);
            }
        });
        $.ajax({
            type: "GET",
            url: "/Home/TotalBasketPrice",
            success: function (res) {
                $("#basket-total-price").text("CART ($" + res + ")");
            }
        });
    })

    $(document).on('click', '#decrement-basket-item', function () {
        $.ajax({
            type: "GET",
            url: "/Home/DecrementBasket?id=" + $(this).attr('data-id'),
            success: function (res) {
                $("#basket-list").empty();
                $("#basket-list").append(res);
            }
        });
        $.ajax({
            type: "GET",
            url: "/Home/TotalBasketPrice",
            success: function (res) {
                $("#basket-total-price").text("CART ($" + res + ")");
            }
        });
    })

    $(document).on('click', '#remove-basket-item', function () {
        $.ajax({
            type: "GET",
            url: "/Home/RemoveBasket?id=" + $(this).attr('data-id'),
            success: function (res) {
                $("#basket-list").empty();
                $("#basket-list").append(res);
            }
        });
        $.ajax({
            type: "GET",
            url: "/Home/DecBasketCount",
            success: function (res) {
                $("#basket-count").text(res);
            }
        });
    })

    $(document).on('click', '#add-to-cart', function () {
        $.ajax({
            type: "GET",
            url: "/Home/AddBasket?id=" + $(this).attr('data-id'),
            success: function (res) {
                $("#basket-list").append(res);
            }
        });

        $.ajax({
            type: "GET",
            url: "/Home/IncBasketCount",
            success: function (res) {
                $("#basket-count").text(res);
            }
        });
        $.ajax({
            type: "GET",
            url: "/Home/TotalBasketPrice",
            success: function (res) {
                $("#basket-total-price").text("CART ($" + res + ")");
            }
        });
    })


    var searchedProduct;
    $(document).on('keyup', '#input-search', function () {
        searchedProduct = $(this).val();

        $("#searchedProducts li").slice(1).remove();

        $.ajax({
            type: "GET",
            url: "/Home/Search?searchedProduct=" + searchedProduct,
            success: function (res) {

                $("#searchedProducts").append(res);
            }
        });
    });

    var skip = 8;
    $(window).scroll(function () {
        var scroll = $(window).scrollTop();
        if (scroll > $("#productRow").height() - $('.product-item').height()) {

            $.ajax({
                type: "GET",
                url: "/Product/Load?skip=" + skip,
                success: function (res) {

                    $("#productRow").append(res);
                    skip += 8;

                    var productsCount = $("#productsCount").val();

                    if (skip >= productsCount) {
                        $(window).unbind('scroll');
                       /* $("#loadMore").remove();*/
                    }
                }
            });
        }
    });
    // HEADER

    $(document).on('click', '#search', function () {
        $(this).next().toggle();
    })

    $(document).on('click', '#mobile-navbar-close', function () {
        $(this).parent().removeClass("active");

    })
    $(document).on('click', '#mobile-navbar-show', function () {
        $('.mobile-navbar').addClass("active");

    })

    $(document).on('click', '.mobile-navbar ul li a', function () {
        if ($(this).children('i').hasClass('fa-caret-right')) {
            $(this).children('i').removeClass('fa-caret-right').addClass('fa-sort-down')
        }
        else {
            $(this).children('i').removeClass('fa-sort-down').addClass('fa-caret-right')
        }
        $(this).parent().next().slideToggle();
    })

    // SLIDER

    $(document).ready(function(){
        $(".slider").owlCarousel(
            {
                items: 1,
                loop: true,
                autoplay: true
            }
        );
      });

    // PRODUCT

    $(document).on('click', '.categories', function(e)
    {
        e.preventDefault();
        $(this).next().next().slideToggle();
    })

    $(document).on('click', '.category li a', function (e) {
        e.preventDefault();
        let category = $(this).attr('data-id');
        let products = $('.product-item');
        
        products.each(function () {
            if(category == $(this).attr('data-id'))
            {
                $(this).parent().fadeIn();
            }
            else
            {
                $(this).parent().hide();
            }
        })
        if(category == 'all')
        {
            products.parent().fadeIn();
        }
    })

    // ACCORDION 

    $(document).on('click', '.question', function()
    {   
       $(this).siblings('.question').children('i').removeClass('fa-minus').addClass('fa-plus');
       $(this).siblings('.answer').not($(this).next()).slideUp();
       $(this).children('i').toggleClass('fa-plus').toggleClass('fa-minus');
       $(this).next().slideToggle();
       $(this).siblings('.active').removeClass('active');
       $(this).toggleClass('active');
    })

    // TAB

    $(document).on('click', 'ul li', function()
    {   
        $(this).siblings('.active').removeClass('active');
        $(this).addClass('active');
        let dataId = $(this).attr('data-id');
        $(this).parent().next().children('p.active').removeClass('active');

        $(this).parent().next().children('p').each(function()
        {
            if(dataId == $(this).attr('data-id'))
            {
                $(this).addClass('active')
            }
        })
    })

    $(document).on('click', '.tab4 ul li', function()
    {   
        $(this).siblings('.active').removeClass('active');
        $(this).addClass('active');
        let dataId = $(this).attr('data-id');
        $(this).parent().parent().next().children().children('p.active').removeClass('active');

        $(this).parent().parent().next().children().children('p').each(function()
        {
            if(dataId == $(this).attr('data-id'))
            {
                $(this).addClass('active')
            }
        })
    })

    // INSTAGRAM

    $(document).ready(function(){
        $(".instagram").owlCarousel(
            {
                items: 4,
                loop: true,
                autoplay: true,
                responsive:{
                    0:{
                        items:1
                    },
                    576:{
                        items:2
                    },
                    768:{
                        items:3
                    },
                    992:{
                        items:4
                    }
                }
            }
        );
      });

      $(document).ready(function(){
        $(".say").owlCarousel(
            {
                items: 1,
                loop: true,
                autoplay: true
            }
        );
      });
})