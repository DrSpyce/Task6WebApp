var seedAdder = 1;
var numberOfPersons;
var rowNumber;

$(document).ready(function () {
	rowNumber = document.getElementById("rowNumber").textContent;
	$(window).on("scroll", function () {
		var docHeight = $(document).height();
		var winScrolled = $(window).height() + $(window).scrollTop();
		if ((docHeight - winScrolled) < 1) {
			seed = document.getElementById("seed").value;
			seed = seed + seedAdder;
			seedAdder++;
			numberOfPersons = 10;
			sendAjax(numberOfPersons, seed);
		}
	});
});

$(document).ready(function () {
	$('#seed').on('change', function () {
		changeData();
	});
});

$(document).ready(function () {
	$('#locale').on('change', function () {
		changeData();
	});
});

function inputErrorChange() {
	if (document.getElementById('errorsInput').value > 1000)
		document.getElementById('errorsInput').value = 1000;
	if (document.getElementById('errorsInput').value < 0)
		document.getElementById('errorsInput').value = 0;
	changeData();
}


function randomizeSeed() {
	$('#seed').val(Math.floor(Math.random() * 999999));
	changeData();
}

function changeData() {
	$('#tbody').empty();
	rowNumber = 0;
	seedAdder = 1;
	numberOfPersons = 20;
	sendAjax(numberOfPersons);
}

function showItems(result) {
	for (var i = 0; i < result.length; i++) {
		rowNumber++;
		var $tr = $('<tr>').append(
			$('<td>').text(rowNumber),
			$('<td>').attr('id', 'guid' + rowNumber).text(result[i].guid),
			$('<td>').attr('id', 'fullName' + rowNumber).text(result[i].fullName),
			$('<td>').attr('id', 'adress' + rowNumber).text(result[i].adress),
			$('<td>').attr('id', 'phone' + rowNumber).text(result[i].phone)
		).appendTo('#tbody');
	}
}

function changeInputValue() {
	var rangeValue = document.getElementById('errorsRange').value;
	document.getElementById('errorsInput').value = rangeValue;
}


function loadAllData() {
	var obj = [];
	for (var i = 1; i <= rowNumber; i++) {
		obj.push([i, $('#guid' + i).text(), $('#fullName' + i).text(), $('#adress' + i).text(), $('#phone' + i).text()]);
	}
	download_csv_file(obj);
}

function download_csv_file(obj) {
	var csv = 'ID, Guid, Full Name, Street, N., City, Phone\n';
	obj.forEach(function (row) {
		csv += row.join(',');
		csv += "\n";
	});
	var hiddenElement = document.createElement('a');
	hiddenElement.href = 'data:text/csv;charset=utf-8,' + encodeURI(csv);
	hiddenElement.target = '_blank';
	hiddenElement.download = 'file.csv';
	hiddenElement.click();
}  