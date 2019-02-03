//$.get("/Shared/_LanguagePartial", function (result) {
//    $("#language_selector").html(result);
//});

$.ajax({
  //  cache: false,
    url: "/Shared/_LanguagePartial",
    method: "Get",
    success: function (result) {
        $("#language_selector").html(result);
    },
    error: function () {
        console.log("Lỗi ajax");
    }
})
$.ajax({
    //  cache: false,
    url: "/Shared/_HeaderMenuPartial",
    method: "Get",
    success: function (result) {
        $("#col-header-menu").html(result);
    },
    error: function () {
        console.log("Lỗi ajax");
    },
    complete: function () {
        var cbpHorizontalMenu, cbpVerticalmenu;

        cbpHorizontalMenu = (function () {
            var menuId = '#cbp-hrmenu',
                $listItems = $(menuId + '> ul > li'),
                $menuItems = $listItems.children('a, .cbp-main-link'),
                $innerTabs = $(menuId + ' .cbp-hrsub-tabs-names li > a'),
                $body = $('body'),
                $header = $('#desktop-header-container'),
                current = -1;
            currentlevel = -1;
            $listItems.has('ul').find(' > a').doubleTapToGo();

            function init() {
                var isTouchDevice = 'ontouchstart' in document.documentElement;
                if (isTouchDevice) {
                    $menuItems.on('mouseover', open)
                } else {
                    $menuItems.hoverIntent({
                        over: open,
                        out: dnthing,
                        interval: 30
                    })
                }
                $listItems.on('mouseover', function (event) {
                    event.stopPropagation()
                });
                $innerTabs.hover(function () {
                    $innerTabs.removeClass('active');
                    $(this).tab('show')
                })
            }
            var setCurrent = function (strName) {
                current = strName
            };

            function dnthing(event) { }

            function open(event) {
                $othemenuitem = $('#cbp-hrmenu1').find('.cbp-hropen');
                $othemenuitem.find('.cbp-hrsub').removeClass('cbp-show');
                $othemenuitem.removeClass('cbp-hropen');
                cbpVerticalmenu.setCurrent(-1);
                var $item = $(event.currentTarget).parent('li'),
                    idx = $item.index();
                $submenu = $item.find('.cbp-hrsub');
                if (current == idx)
                    return;
                $submenu.removeClass('cbp-notfit');
                $submenu.removeClass('cbp-show');
                if (current !== -1) {
                    $listItems.eq(current).removeClass('cbp-hropen')
                }
                if (current === idx) {
                    $item.removeClass('cbp-hropen');
                    current = -1
                } else {
                    $submenu.addClass('cbp-show');
                    iqitmenuwidth = $header.width();
                    iqititemposition = $item.offset().left - $header.offset().left;
                    if ((iqitmenuwidth - iqititemposition) <= $submenu.width()) {
                        $submenu.addClass('cbp-notfit')
                    }
                    $item.addClass('cbp-hropen');
                    current = idx;
                    $body.off('mouseover').on('mouseover', close)
                }
                return !1
            }

            function close(event) {
                $listItems.eq(current).removeClass('cbp-hropen');
                current = -1
            }
            return {
                init: init,
                setCurrent: setCurrent
            }
        })();
        cbpHorizontalMenu.init()

        $('.cbp-vertical-on-top').on('mouseover', function () {
            $(this).addClass('cbp-vert-expanded')
        });
        $('.cbp-vertical-on-top').on('mouseleave', function () {
            $(this).removeClass('cbp-vert-expanded')
        });
        cbpVerticalmenu = (function (test) {
            var menuId = '#cbp-hrmenu1-ul',
                $listItems = $(menuId + ' > li'),
                $menuItems = $listItems.children('a'),
                $innerTabs = $(menuId + ' .cbp-hrsub-tabs-names li > a'),
                $body = $('body'),
                current = -1,
                currentlevel = -1;
            $listItems.has('ul').find(' > a').doubleTapToGo();

            function init() {
                var isTouchDevice = 'ontouchstart' in document.documentElement;
                if (isTouchDevice) {
                    $menuItems.on('mouseover', open)
                } else {
                    $menuItems.hoverIntent({
                        over: open,
                        out: dnthing,
                        interval: 30
                    })
                }
                $listItems.on('mouseover', function (event) {
                    event.stopPropagation()
                });
                $innerTabs.hover(function () {
                    $innerTabs.removeClass('active');
                    $(this).tab('show')
                });
                $(window).resize(function () {
                    $('cbp-hrmenu-tab').not('.cbp-hropen').find('.cbp-hrsub-wrapper').removeAttr('style')
                })
            }

            function dnthing(event) { }
            var setCurrent = function (strName) {
                current = strName
            };

            function open(event) {
                $othemenuitem = $('#cbp-hrmenu').find('.cbp-hropen');
                $othemenuitem.find('.cbp-hrsub').removeClass('cbp-show');
                closeElement($othemenuitem);
                cbpHorizontalMenu.setCurrent(-1);
                var $item = $(event.currentTarget).parent('li'),
                    idx = $item.index();
                if (current == idx)
                    return;
                $submenu = $item.find('.cbp-hrsub');
                $submenu.removeClass('cbp-show');
                if (current !== -1) {
                    closeElement($listItems.eq(current))
                }
                if (current === idx) {
                    closeElement($item);
                    current = -1
                } else {
                    $submenu.parent().width($(iqitmegamenu.containerSelector).width() - $(menuId).width());
                    callerHeight = $item.height();
                    $submenu.parent().css({
                        marginLeft: $item.innerWidth() + "px",
                        marginRight: $item.innerWidth() + "px",
                        marginTop: -callerHeight + "px"
                    });
                    $submenu.addClass('cbp-show');
                    $item.addClass('cbp-hropen');
                    current = idx;
                    $body.off('mouseover').on('mouseover', close)
                }
                return !1
            }

            function close(event) {
                closeElement($listItems.eq(current));
                current = -1
            }

            function closeElement($element) {
                $element.removeClass('cbp-hropen')
            }
            return {
                init: init,
                setCurrent: setCurrent
            }
        })();
        cbpVerticalmenu.init()
        var iqitMobileMenu = (function () {
            var $menu = $('#iqitmegamenu-mobile');
            var $expander = $menu.find('.mm-expand');
            $menu.on('click', function (e) {
                e.stopPropagation()
            });
            $expander.on('click', function () {
                $(this).parent().toggleClass('show')
            })
        });
        iqitMobileMenu()
    }
});

