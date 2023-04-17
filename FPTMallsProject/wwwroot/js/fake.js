"use strict";

function ownKeys(object, enumerableOnly) { var keys = Object.keys(object); if (Object.getOwnPropertySymbols) { var symbols = Object.getOwnPropertySymbols(object); if (enumerableOnly) symbols = symbols.filter(function (sym) { return Object.getOwnPropertyDescriptor(object, sym).enumerable; }); keys.push.apply(keys, symbols); } return keys; }

function _objectSpread(target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i] != null ? arguments[i] : {}; if (i % 2) { ownKeys(Object(source), true).forEach(function (key) { _defineProperty(target, key, source[key]); }); } else if (Object.getOwnPropertyDescriptors) { Object.defineProperties(target, Object.getOwnPropertyDescriptors(source)); } else { ownKeys(Object(source)).forEach(function (key) { Object.defineProperty(target, key, Object.getOwnPropertyDescriptor(source, key)); }); } } return target; }

function _defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }

var vw = $(window).width(); //var winPos = 0

var menuOpen = false;
var footerPos = 0;
var sticky = $('.sticky');
var stickyHeight = sticky.height();
var noteInit = false;
var isMobile = /Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent);
var option = {
  infinite: false,
  slidesToShow: 1,
  slidesToScroll: 1,
  dots: true,
  arrows: false,
  autoplay: true,
  autoplaySpeed: 5000,
  speed: 700
};

function slide() {
  $('.banner-slide').slick(_objectSpread(_objectSpread({}, option), {}, {
    arrows: true,
    pauseOnFocus: false,
    responsive: [{
      breakpoint: 480,
      settings: {
        arrows: false
      }
    }]
  }));
  $('.promotion-slide__inner').slick(_objectSpread(_objectSpread({}, option), {}, {
    slidesToShow: 3,
    slidesToScroll: 3,
    arrows: true,
    responsive: [{
      breakpoint: 1001,
      settings: {
        slidesToShow: 2,
        slidesToScroll: 2
      }
    }, {
      breakpoint: 767,
      settings: {
        slidesToShow: 1,
        slidesToScroll: 1,
        arrows: false
      }
    }]
  }));
  $('.service-slide__inner').slick(_objectSpread({}, option));
  $('.discovery-style__slide').slick(_objectSpread({}, option));
  $('.new-list__slide').slick(_objectSpread(_objectSpread({}, option), {}, {
    slidesToShow: 3,
    slidesToScroll: 3,
    responsive: [{
      breakpoint: 1000,
      settings: {
        slidesToShow: 1.3,
        slidesToScroll: 1,
        infinite: false
      }
    }, {
      breakpoint: 767,
      settings: {
        dots: false,
        slidesToShow: 1.2,
        slidesToScroll: 1,
        infinite: false
      }
    }]
  }));
  $('.trademark-slide').slick(_objectSpread(_objectSpread({}, option), {}, {
    slidesToShow: 4,
    slidesToScroll: 4,
    arrows: true,
    responsive: [{
      breakpoint: 1367,
      settings: {
        arrows: true,
        slidesToShow: 3.5,
        slidesToScroll: 3
      }
    }, {
      breakpoint: 1280,
      settings: {
        arrows: false
      }
    }, {
      breakpoint: 1000,
      settings: {
        arrows: false,
        slidesToShow: 2.5,
        slidesToScroll: 2
      }
    }, {
      breakpoint: 767,
      settings: {
        arrows: false,
        slidesToShow: 2.2,
        slidesToScroll: 2,
        dots: false,
        infinite: false
      }
    }]
  })); // $('.branch-floor__note__list').slick({
  //   ...option,
  //   autoplay: false
  // });
}

function branch() {
  var mall = 0;
  var tab = 0;
  var branchNum = 1;
  $('[data-handle="floor-0"]').addClass('active');
  $('[data-handle]').on('click', function () {
    var data = $(this).data('handle');
    var group = $(this).data('tab'); // add class active to click item

    $('[data-tab="' + group + '"]').removeClass('active');
    $(this).addClass('active'); // check click into tab

    if (group === 1) {
      mall = $(this).index();
      branchNum = $(this).index() + 1; //console.log(mall)

      $('.branch-content').eq(mall).find('.tab-header__item').eq(tab).addClass('active');
      noteInit = false; // return floor G when change mall and floor not G

      changeMall(4, 'floor-0');
      changeMall(3, 'floor-map'); // $('[data-tab="4"]').removeClass('active')
      // $('[data-content="4"]').hide()
      // $('[data-handle="floor-0"]').addClass('active')
      // $('[data-toggle="floor-0"]').show()
    } else if (group === 2) {
      tab = $(this).index();
    } else if (group === 3) {
      $('[data-handle=' + data + ']').addClass('active');
    } // delete padding if tab is map


    tab === 1 ? $('.branch-inner').addClass('padding') : $('.branch-inner').removeClass('padding'); // change tab content

    $('[data-content="' + group + '"]').hide();
    $('[data-toggle="' + data + '"]').show(); // note slide init first slick

    if (data === 'floor-note' && !noteInit) {
      console.log(branchNum);
      $('[data-toggle="branch' + branchNum + '"]').find('.branch-floor__note__list').not('.slick-initialized').slick(_objectSpread(_objectSpread({}, option), {}, {
        autoplay: false
      }));
      noteInit = true;
    }
  });
}

function changeMall(tab, handle) {
  $('[data-tab="' + tab + '"]').removeClass('active');
  $('[data-content="' + tab + '"]').hide();
  $('[data-handle="' + handle + '"]').addClass('active');
  $('[data-toggle="' + handle + '"]').show();
}

function branchSelect() {
  var $list = $('.branch-list');
  var $select = $('.branch-list__select');
  var $drop = $('.branch-list__drop');
  var $item = $('.branch-list__item');
  var $name = $('.branch-list__name');
  $select.on('click', function () {
    $list.toggleClass('open');
  });
  $item.on('click', function () {
    // select in pc
    $item.removeClass('active');
    $(this).addClass('active'); // select in sp 

    var value = $(this).find($name).text();
    $list.removeClass('open');
    $select.text(value);
  });
}

function magnifier() {
  $('.thumb').magnify({
    speed: 200
  });
} // function menu() {
//   $('.btn-menu__icon').on('click tap', function() {
//     $(this).find('.line').removeClass('disableAnimation');
//     $(this).toggleClass('active');
//     $('html').toggleClass('menu-open');
//     if (menuOpen) {
//       $('.submenu-header').removeClass('open')
//     }
//     menuOpen = !menuOpen
//     //console.log(menuOpen)
//   });
// }
// function openSubmenu() {
//   let $el = $('[data-toggles="collapse"]')
//   if (!isMobile) {
//     $('.has-sub-menu').hover (
//       function() {
//         $(this).addClass('hover')
//       },
//       function() {
//         $(this).removeClass('hover')
//       }
//     )
//   } 
//   $el.on('click', function(e) {
//     e.preventDefault()
//     if (vw < 1200) {
//       var target = $(this).attr('href')
//       $(this).toggleClass('expanded')
//       $(target).toggleClass('open')
//       $(target).find('.sub-menu').removeClass('open')
//       $(target).find('.icon-down-arrow').removeClass('expanded')
//     } 
//   });
// }
// function menuScroll() {
//   winPos >= 57
//     ? $('html').addClass('menu-fixed')
//     : $('html').removeClass('menu-fixed') 
// }


function smoothScroll() {
  var offset = 73;
  $('a[href^="#"]').on("click", function (e) {
    e.preventDefault();
    vw <= 1199 ? offset = 58 : offset = 73;
    var h = $(this).attr("href");
    var t = $(h == "#" || h === "" ? 'body' : h);
    var p = t.offset().top - offset;
    setTimeout(function () {
      $('html,body').animate({
        scrollTop: p
      }, 500);
    }, 10);
    return false;
  });
}

function stickyScroll() {
  //var stickyPos = sticky.offset().top; //console.log(stickyPos)

  //if (stickyHeight + stickyPos >= footerPos) {
  //  sticky.css({
  //    opacity: 0,
  //    zIndex: -1
  //  });
  //} else {
  //  sticky.css({
  //    opacity: 1,
  //    zIndex: 11
  //  });
  //}
}

function update() {
 /* vw = $(window).width();
  footerPos = $('.footer').offset().top;*/
} // update variable when resize


$(function () {
  slide();
  branch();
  magnifier(); //menu()

  branchSelect();
  smoothScroll(); //openSubmenu()
});
$(window).on('load', function () {
  //footerPos = $('.footer').offset().top; comment tạm 
});
$(window).on('resize', function () {
  update();
});
$(window).on('scroll', function () {
  //winPos = window.pageYOffset
  //menuScroll()
  stickyScroll();
});