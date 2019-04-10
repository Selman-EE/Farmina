///======================================================================================
// Document  ready
//=======================================================================================
$(document).ready(function () {
	//
	//
	//check required field if has no attribute than it will be add
	//$('div.required-field').find('input,select,textarea').each(function (index, element) {
	//    var that = $(element);
	//    var required = that.attr('required');
	//    if (!required) {
	//        that.attr('required', 'required').attr('aria-required', true);
	//    }
	//});
});



//
//Side bar nav activitaion 
var pageUrl = window.location.pathname.split('/')[1].toLowerCase();
$('#menu li').removeClass('nav-active');
if (pageUrl) {
	$('#menu li.' + pageUrl + '-menu').addClass('nav-active');
}
else {
	$('#menu li.home-menu').addClass('nav-active');
}

///======================================================================================
// Notifications PNotify
//=======================================================================================

function showNotify(status, message) {
	new PNotify({
		//title: status ? 'Success' : 'Error',
		title: status ? 'İşlem başarılı' : 'Hata',
		text: message || "",
		type: status ? 'success' : 'error'
	});
}

///======================================================================================
// General delete method
//=======================================================================================
function deleteEntity(id, url) {
	if (confirm('Are you sure ?')) {
		$.ajax({
			url: url,
			type: 'POST',
			data: { id: id },
			dataType: 'json',
			success: function (data) {
				//show notify and remove entity from table                
				$("tr#" + data.EntityId).remove();
				showNotify(data.Status, data.Message);
			},
			error: function (e) {
				showNotify(data.Status, data.Message);
			},
			beforeSend: function (e) {
				showLoading();
			},
			complete: function (e) {
				hideLoading();
			}
		});
	}
}

///======================================================================================
// Loading Gif
//=======================================================================================

/* Ali Evdi */
function showLoading() {
	$("div#divLoading").addClass('show');
}
function hideLoading() {
	$("div#divLoading").removeClass('show');
}

//$( "#open" ).click(function() {
//	showLoading();

//	setTimeout(() => {
//		hideLoading();
//	}, 2000);
//});


///======================================================================================
// Options selected values 
//=======================================================================================
function setSelectedValuesOnSelectElement(id, val) {
	//country
	$('#' + id).val(val).trigger("change");
}


function showFullDate(fullDate) {

	var dt = new Date(fullDate);
	var day = parseInt(dt.getDate());
	var month = parseInt(dt.getMonth() + 1); // jan = 0
	var year = parseInt(dt.getFullYear());

	var hour = parseInt(dt.getHours());
	var min = parseInt(dt.getMinutes());
	var sec = parseInt(dt.getSeconds());


	if (day < 10) { day = "0" + day; }
	if (month < 10) { month = "0" + month; }
	if (hour < 10) { hour = "0" + hour; }
	if (min < 10) { min = "0" + min; }
	if (sec < 10) { sec = "0" + sec; }

	var date = day + '.' + month + '.' + year + ' ' + hour + ':' + min + ':' + sec;
	return date;

}

function showDate(fullDate) {

	var dt = new Date(fullDate);
	var day = parseInt(dt.getDate());
	var month = parseInt(dt.getMonth() + 1); // jan = 0
	var year = parseInt(dt.getFullYear());

	if (day < 10) { day = "0" + day; }
	if (month < 10) { month = "0" + month; }

	var date = day + '.' + month + '.' + year;
	return date;

}

function showHour(fullDate) {

	var dt = new Date(fullDate);
	var hour = parseInt(dt.getHours());
	var min = parseInt(dt.getMinutes());

	if (hour < 10) { hour = "0" + hour; }
	if (min < 10) { min = "0" + min; }

	return hour + ':' + min;
}

//
// Add days method make customize   
Date.prototype.addDays = function (days) {
	var date = new Date(this.valueOf());
	date.setDate(date.getDate() + days);
	return date;
};


//
//get first day of the week by any date
Date.prototype.getMonday = function () {
	var day = this.getDay() || 7;
	if (day !== 1)
		this.setHours(-24 * (day - 1));
	return this;
};
//
//Special chars encoding (ß,ü,ö,ç,ğ,ş) something like that
// Create a in-memory div, set its inner text (which jQuery automatically encodes)
function htmlEncode(value) {
	// Then grab the encoded contents back out. The div never exists on the page.
	var $div = $('<div/>');
	var html = $div.text(value).html();
	$div.remove();
	return html;
}

function htmlDecode(value) {
	var $div = $('<div/>');
	var text = $div.html(value).text();
	$div.remove();
	return text;
}

function select2Search(className, searchUrl) {
	//
	$('.' + className).select2({
		"language": {
			"noResults": function () {
				return "Hiçbir sonuç bulanamadı.";
			}
		},
		minimumInputLength: 2,
		maximumSelectionLength: 7,
		formatSelectionTooBig: function (limit) {
			return 'Bir kere en fazla (' + limit + ') adet ürün seçilebilir';
		},
		delay: 250,
		allowClear: true,
		placeholder: '--Arama yapınız--',
		ajax: {
			url: searchUrl,
			dataType: 'json',
			type: "GET",
			quietMillis: 50,
			data: function (params) {
				return {
					term: params.term
				};
			},
			processResults: function (data) {
				data = jQuery.parseJSON(data);
				return {
					results: $.map(data, function (item) {
						return {
							text: item.text,
							id: item.id,
							//children: item.children
						};
					})
				};
			},
		},
		//escapeMarkup: function (markup) { return markup; }, // let our custom formatter work
		//templateResult: formatState,
		//templateSelection: formatRepoSelection
	});
}

//
//select2 element repo selection
function formatRepoSelection(repo) {
	//console.log(repo);
	$(repo.element).attr('data-test', 'test-' + repo.id);
	return repo.text;
}