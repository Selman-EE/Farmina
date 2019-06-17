
$(document).ready(function () {
	select2Search('ddlProduct', '/home/searchproducts');
	select2Search('ddlCustomers', '/home/searchcustomers');
	select2Search('ddlSuppliers', '/home/searchsuppliers');
	//select2Search('ddlDiscount', '/home/searchdiscount');
	//
	//
	saveVoucher();
	//
	//add products
	addNewProducts();
	//
	getCustomerById();
	//
	getSupplierById();
	//
	removeProduct();
	//
	downloadVouchers();
	//
	//$("body").on('DOMSubtreeModified', ".eklenen-urunler", function () {
	//	//console.log(true);
	//});

	//$('.eklenen-urunler').bind('DOMNodeInserted DOMNodeRemoved', function () {
	//	console.log(false);
	//});


});



function getCustomerById() {

	$('#btnAddCustomer').click(function (e) {
		e.preventDefault();
		showLoading();
		//
		var customerId = $('.ddlCustomers').val();
		//
		$.get('/home/getcustomer', { id: customerId }, function (data) {
			$('#txtTax').val(data.TaxNumber);
			$('#txtCountryCode').val(data.Country);
			$('#txtRegionCode').val(data.Region);
			$('#txtZipCode').val(data.ZipCode);
			$('#txtCity').val(data.City);
			$('#txtAddress').val(data.Address);
		}).done(function (response) {
			hideLoading();

		});
	});
}
//
function getSupplierById() {

	$('#btnAddSupplier').click(function (e) {
		e.preventDefault();
		showLoading();
		//
		var supplierId = $('.ddlSuppliers').val();
		//
		$.get('/home/getsupplier', { id: supplierId }, function (data) {
			//console.log(data);
			$('#txtSupplierName').val(data.Name);
			$('#txtSupplierCode').val(data.Code);
		}).done(function (response) {
			hideLoading();
		});
	});
}
//
function addNewProducts() {
	$('#btnAddProduct').click(function (e) {
		e.preventDefault();

		//
		var listIndex = $('.products-list-index').last().val() || 0;
		var productIds = $('.ddlProduct').val();

		if (!productIds) {
			showNotify(false, "Ürün seçiniz önce lütfen");
			return false;
		}
		//
		//check it if products already added
		var pIds = [];
		$('.eklenen-urunler div[class*=urun-liste]').each(function (index, element) {
			var elementId = $(element).find('#product-elementId').val();
			var productId = $(element).find('input[name=productid-' + elementId + ']').val();
			pIds.push(productId);
		});

		var inList = productIds.filter(x => pIds.includes(x));
		if (inList.length > 0) {
			showNotify(false, "Eklenmiş ürünleri tekrar ekleyemezseniz. Lütfen adetlerini değiştirin.");
			for (var i = 0; i < inList.length; i++) {
				productIds.splice(productIds.indexOf(inList[i]), 1);
			}
		}

		if (productIds.length <= 0) {
			showNotify(false, "Yeni eklenecek ürün bulunamadı");
			return false;
		}

		/* url and querystring change */
		var url = '/home/addproducts';
		url = updateQueryStringParameter("productIds", productIds, url);
		url = updateQueryStringParameter("listIndex", listIndex, url);
		//
		showLoading();
		$.get(url, function (data) {
			//
			$('.eklenen-urunler').append(data);
		}).done(function (response) {
			hideLoading();
			$('.ddlDiscount').select2();
			calculateTotalPriceOfTheVoucher();
		});
	});
}
//
function removeProduct() {
	$(document).on('click', '.delete', function (e) {
		e.preventDefault();
		var elemetenId = $(this).attr('deleteproduct');
		$('.urun-liste-' + elemetenId).remove();
	});
}

//
function calculateTotalPriceOfTheVoucher() {

	if ($('.eklenen-urunler').find('.priceWithTax').length <= 0) {
		return false;
	}
	//
	var price = 0;
	$('.eklenen-urunler').find('.priceWithTax').each(function (index, element) {
		price += parseFloat($(element).text().replace(',', '.'));
	});
	//
	var $totalPrice = $('#totalPriceOfTheVoucher');
	$totalPrice.html("");
	$totalPrice.html(price.toFixed(2));
}
//

//
function saveVoucher() {
	$('#saveVoucher').submit(function (e) {
		e.preventDefault();

		var platformCode = $('#txtPlatformCode').val();
		if (!platformCode) {
			showNotify(false, "Platfrom kodu boş bırakalamaz");
			return false;
		}
		var voucherDate = $('#voucherDate').val();
		if (!voucherDate) {
			showNotify(false, "Belge tarihi boş bırakalamaz");
			return false;
		}
		var voucherType = $('#txtVoucherType').val();
		if (!voucherType) {
			showNotify(false, "Belge tipi boş bırakalamaz");
			return false;
		}
		var voucherNo = $('#txtVoucherNo').val();
		if (!voucherNo) {
			showNotify(false, "Belge numarası boş bırakalamaz");
			return false;
		}
		var customerId = $('.ddlCustomers').val();
		if (!customerId) {
			showNotify(false, "Lütfen müşteri seçiniz ");
			return false;
		}
		var supplierId = $('.ddlSuppliers').val();
		if (!supplierId) {
			showNotify(false, "Lütfen satıcı seçiniz");
			return false;
		}
		//
		if ($('.eklenen-urunler div[class*=urun-liste]').length <= 0) {
			showNotify(false, "Lütfen ürün ekleyin");
			return false;
		}

		var taxPercent = $('input[name=txtTax]').val() || 0;
		//
		showLoading();
		//
		var products = [];
		//
		$('.eklenen-urunler div[class*=urun-liste]').each(function (index, element) {
			var elementId = $(element).find('#product-elementId').val();
			var productName = $(element).find('input[name=productName-' + elementId + ']').val();
			if (!productName) {
				showNotify(false, "Ürün ismi boş olamaz");
				return false;
			}

			var productPrice = $(element).find('input[name=productPrice-' + elementId + ']').val();
			if (!productPrice) {
				showNotify(false, "Ürün fiyatı boş olamaz");
				return false;
			} else if (parseFloat(productPrice.replace(',', '.')) <= 0) {
				showNotify(false, "Ürün fiyatı sıfırdan büyük olmalı");
				return false;
			}

			var productQuantity = $(element).find('input[name=productQuantity-' + elementId + ']').val();
			if (!productQuantity) {
				showNotify(false, "Ürün adeti boş olamaz");
				return false;
			} else if (parseInt(productQuantity) <= 0) {
				showNotify(false, "Ürün adeti sıfırdan büyük olmalı");
				return false;
			}

			var productId = $(element).find('input[name=productid-' + elementId + ']').val();

			var pd = $(element).find('.productDiscount-' + elementId).find('.ddlDiscount').val().split('|');
			var productDiscount = pd[1];
			var productDiscountName = pd[0];
			//
			var productBarcode = $(element).find('input[name=productBarcode-' + elementId + ']').val();
			if (!productBarcode) {
				showNotify(false, "Ürün barcode boş olamaz");
				return false;
			}
			//
			var product = {};
			product.ProductId = productId;
			product.ProductName = productName;
			product.ProductPrice = productPrice;
			product.ProductQuantity = productQuantity;
			product.ProductDiscount = productDiscount;
			product.ProductDiscountName = productDiscountName;
			product.Barcode = productBarcode;
			products.push(product);
		});

		//
		var parameters = {};
		parameters.CustomerId = customerId;
		parameters.SupplierId = supplierId;
		parameters.PlatformCode = platformCode;
		parameters.VoucherDate = voucherDate;
		parameters.VoucherNo = voucherNo;
		parameters.TaxPercent = taxPercent;
		parameters.Products = products;
		//
		var params = JSON.stringify({ model: parameters });
		//
		$.ajax({
			url: '/home/savevoucher',
			type: 'POST',
			data: params,
			dataType: 'json',
			contentType: "application/json; charset=utf-8",
			success: function (data) {
				if (data.Status) {
					clearForm();
					$('#txtVoucherNo').val(data.EntityId);
				}
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
	});
}
//
//reset all form
function clearForm() {
	//reset form
	$('#saveVoucher')[0].reset();
	//clear added products
	$('.eklenen-urunler').html('');
	//clear select2
	$('.ddlProduct,.ddlCustomers,.ddlSuppliers').val('').trigger('change');
}
//
//download voucher
function downloadVouchers() {

	$('#btnGetAllDailyInvoices').click(function (e) {
		e.preventDefault();
		//
		//
		var voucherDate = $('#reportVoucherDate').val();
		if (!voucherDate) {
			showNotify(false, "Lütfen tarih seçiniz...");
			return false;
		}
		//
		showLoading();
		//
		var url = '/voucher/createvoucher';
		$.get(url, { date: voucherDate }, function (data) {
			//
			if (data.Status) {
				/* url and querystring change */
				url = '/download/vouchers';
				url = updateQueryStringParameter("filePath", data.Message, url);

				//onclick="(function(_this){_this.parentNode.removeChild(_this);})(this);"
				$('#dailyInvoices').find('.download-voucher').remove();
				$('#dailyInvoices').append('<a href="' + url + '" class="btn btn-default btn-edit download-voucher" onclick="(function(_this){_this.parentNode.removeChild(_this);})(this);">İndir</a>');
			} else {
				showNotify(data.Status, data.Message);
			}

		}).done(function (response) {
			hideLoading();
		});

	});

}
