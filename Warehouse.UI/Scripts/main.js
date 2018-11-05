(function (window, $) {
	'use strict';

    $("#modal-crop-image button[data-dismiss=modal]").click(function () {
        if ($("input[name=base64String]").val() != "") {
            $("button[data-target='#modal-crop-image'").text("Đã chọn 1 hình");
        }
        else {
            $("button[data-target='#modal-crop-image'").text("Chọn hình");
        }
    });

	// Cache document for fast access.
	var document = window.document;


	$('a.toggle-menu').click(function(){
        $('ul.menu').fadeToggle("slow");
    });


    var owl = $("#owl-demo");
 
		owl.owlCarousel({
	    	items : 6
		});
	 
		// Custom Navigation Events
		$(".next").click(function(){
	    	owl.trigger('owl.next');
		})
		$(".prev").click(function(){
	    	owl.trigger('owl.prev');
		})



})(window, jQuery);




