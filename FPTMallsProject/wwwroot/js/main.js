!(function($) {
    "use strict";
  
    // Smooth scroll for the navigation menu and links with .scrollto classes
    $(document).on('click', '.nav-menu a, .mobile-nav a, .scrollto', function(e) {
      if (location.pathname.replace(/^\//, '') == this.pathname.replace(/^\//, '') && location.hostname == this.hostname) {
        e.preventDefault();
        var target = $(this.hash);
        if (target.length) {
  
          var scrollto = target.offset().top;
  
          if ($('#header').length) {
            scrollto -= $('#header').outerHeight();
          }
  
          if ($(this).attr("href") == '#header') {
            scrollto = 0;
          }
  
          $('html, body').animate({
            scrollTop: scrollto
          }, 1500, 'easeInOutExpo');
  
          if ($(this).parents('.nav-menu, .mobile-nav').length) {
            $('.nav-menu .active, .mobile-nav .active').removeClass('active');
            $(this).closest('li').addClass('active');
          }
  
          if ($('body').hasClass('mobile-nav-active')) {
            $('body').removeClass('mobile-nav-active');
            $('.mobile-nav-toggle i').toggleClass('icofont-navigation-menu icofont-close');
            $('.mobile-nav-overly').fadeOut();
          }
          return false;
        }
      }
    });

    //check time
      var objDate = new Date();
      var hours = objDate.getHours();
      if(hours >= 10 && hours <= 22){
        document.getElementById("title-time").innerHTML = "We are Open";;
      }
      else{
        document.getElementById("title-time").innerHTML = "We Are Closed";
      }
  
    // Mobile Navigation
    if ($('.nav-menu').length) {
      let is_body = $('body').data("home") == true ? 'homepage' : '';
      let navSearch = $(".nav-search");
  
      var $mobile_nav = $('.nav-menu').clone().prop({
        class: 'mobile-nav d-lg-none'
      });
      $('body').append($mobile_nav);
      $('body').prepend('<button type="button" class="mobile-nav-toggle d-lg-none ' + is_body + '"><i class="icofont-navigation-menu"></i></button>');
      $('body').append('<div class="mobile-nav-overly"></div>');
  
      if (navSearch.length > 0) 
        $('body').prepend(`<div class="nav-search mobile d-block d-lg-none">${navSearch.html()}</div>`);
  
      $(document).on('click', '.mobile-nav-toggle', function(e) {
        $('body').toggleClass('mobile-nav-active');
        $('.mobile-nav-toggle i').toggleClass('icofont-navigation-menu icofont-close');
        $('.mobile-nav-overly').toggle();
      });
  
      $(".mobile-nav ul li").each((key, value) => {
        let dropDown = $(value);
  
        if (dropDown.hasClass("drop-down")) {
          let parent = dropDown.children("a");
          parent.wrap('<div class="parent-drop-down">');
          
          let parentDropdown = $(value).find(".parent-drop-down");
          let parentDropdownLink = parentDropdown.children("a");
  
          parentDropdownLink.addClass('d-inline-block');
          $('<i class="drop-down-icon d-inline-block float-right icofont-rounded-down h5 py-1 px-2 mt-1 mr-1"></i>').insertAfter(parentDropdownLink);
        }
      });
  
      $(document).on('click', '.mobile-nav .drop-down > div .drop-down-icon', function(e) {
        e.preventDefault();
        $(this).parent().next().slideToggle(300);
        $(this).parent().parent().toggleClass('active');
      });
  
      $(document).click(function(e) {
        var container = $(".mobile-nav, .mobile-nav-toggle");
        if (!container.is(e.target) && container.has(e.target).length === 0) {
          if ($('body').hasClass('mobile-nav-active')) {
            $('body').removeClass('mobile-nav-active');
            $('.mobile-nav-toggle i').toggleClass('icofont-navigation-menu icofont-close');
            $('.mobile-nav-overly').fadeOut();
          }
        }
      });
    } else if ($(".mobile-nav, .mobile-nav-toggle").length) {
      $(".mobile-nav, .mobile-nav-toggle").hide();
    }
  
    // Clients carousel (uses the Owl Carousel library)
    $(".banner-carousel").owlCarousel({
        navigation : true, // Show next and prev buttons
        dots: false, 
        slideSpeed : 600,
        paginationSpeed : 600,
        lazyLoad: true,
        autoplay: true,
        autoplayTimeout: 8000,
        loop: true,
        items : 1, 
    });
  
    // Initi AOS
    AOS.init({
      duration: 800,
      easing: "ease-in-out"
    });
  
    // initiate lazy load
    $('.lazy').lazy({
      // called whenever an element could not be handled
      onError: function(element) {
        var imagePlaceHolder = element.data('placeholder');
        $(element).attr("src", imagePlaceHolder);
      }
    });
  
    $("#backtotop").on("click", function(e){
      e.preventDefault();
  
      $("html, body").animate({ scrollTop: 0 }, 400);
    });
  
    // image placeholder
    $('.article-thumbnail, .promotional').on("error", function(){
      $(this).attr("src", "/themes/revamp2022/assets/img/placeholder.jpg")
      $(this).parent("a").css({"background-image": "url(/themes/revamp2022/assets/img/placeholder.jpg)"});
      $(this).parent("div").css({"background-image": "url(/themes/revamp2022/assets/img/placeholder.jpg)"});
    });
  
    // check cookie dialog box
    if (! parseCookie("__smsmc")) {
      $("#cookie-dialog-box").removeClass("d-none");
      $("#footer").addClass("mb-5");
    }
  
    $(document).on("click", "#cookie-dialog-close", function(e){
      e.preventDefault();
      var date = new Date();
      var year = date.getFullYear();
      var month = date.getMonth();
      var day = date.getDate();
      var d = new Date(year, month, day + 1);
      var n = d.toUTCString();
      document.cookie = "__smsmc=1; expires=" + n + "; path=/";
  
      $('#cookie-dialog-box').animate({ opacity: 0 }, 400, function(){ 
        $("#cookie-dialog-box").addClass("d-none");
      });
      $('#survey-dialog').animate({ bottom: 20 }, 400); 
      $("#footer").removeClass("mb-5");
  
      // close survey dialog
    }).on("scroll", function(e){
      let scroll = $(window).scrollTop();
      let backToTop = $(".backtotop");
  
      if (scroll > 500) backToTop.fadeIn();
      else backToTop.fadeOut();
    });
  
    /********************************************
     * YouTube - WatchSM
     *******************************************/
    $("#ytplayer a").on("click", function(e){
      e.preventDefault();
      let watchSMVideo = $(this).attr("href");
      let watchSMImage = $(this).children("img");
  
      let player = `<iframe src="${watchSMVideo}" frameborder="0" allowfullscreen="true" width="${watchSMImage.width()}" height="${watchSMImage.height()}" />`;
      $(this).parent("div#ytplayer").html(player);
    });
  
    /********************************************
     * Malls Filter
     *******************************************/
    // function myFunction() {
    //   var urlv0 = ""; //insert responding url
    //   var urlv1 = urlv0.substring(8);
    //   var urlv2 = urlv1.replace("/", "/%2F" );
    //   var urlv3 = urlv2.replace("/", "");
    //   var shareUrl = "https://www.facebook.com/sharer/sharer.php?u=https%3A%2F%2F" + urlv3 + "&amp;src=sdkpreparse" + "&amp";
    //   var link = document.getElementById("urlLink");
    //   link.setAttribute("href", shareUrl);
    // }
  
    /********************************************
     * parse cookie
     *******************************************/
     function parseCookie(cname) {
      var name = cname + "=";
      var decodedCookie = decodeURIComponent(document.cookie);
      var ca = decodedCookie.split(';');
      for (var i = 0; i < ca.length; i++) {
          var c = ca[i];
          while (c.charAt(0) == ' ') {
              c = c.substring(1);
          }
          if (c.indexOf(name) == 0) {
              return c.substring(name.length, c.length);
          }
      }
      return null;
    }
  
    /********************************************
     * Malls Dropdown
     *******************************************/
    let mallRegionButton = $("#dropdownMallRegionButton");
    let mallRegionList = $("#dropdownMallRegionList");
    let activeMallRegion = mallRegionList.children("a.active");
    mallRegionButton.html(activeMallRegion.html());
  
    /********************************************
     * Site Search
     *******************************************/
    $(".site-search").on("click", function(){
      let $this = $(this);
      let parent = $(this).parent(".nav-search");
  
      $this.addClass("d-none");
      parent.addClass("active");
      parent.find("input").focus();
    });
  
    $(".search-form input[type='text']").on("blur focusout", function(){
      let parent = $(this).parent("form").parent(".nav-search");
      parent.removeClass("active");
      $(".site-search").removeClass('d-none');
    });
  
    /********************************************
     * Leasing form validation
     *******************************************/
    $(".validate-field").on("focusout blur", function(e){
      e.preventDefault();
  
      let field = $(this);
  
      field.css({ "border-color": "#999999" });
      if (field.val() == "") {
        field.css({ "border-color": "#ff0000" });
        return false;
      }
    });
  
    /********************************************
     * Government Service Select Box
     *******************************************/
    $("#business_centers").on("change", function(e){
      e.preventDefault();
  
      let tab = $(this).val();
      let tablist = $(".tab-pane");
  
      tablist.removeClass("show active");
      $(`#${tab}`).addClass("show");
      $(`#${tab}`).addClass("active");
  
    });
  
   })(jQuery);