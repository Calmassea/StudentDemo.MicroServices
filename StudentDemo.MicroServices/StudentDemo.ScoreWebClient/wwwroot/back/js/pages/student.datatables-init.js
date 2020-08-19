$(document).ready(function () {
	var a = $("#student-datatable").DataTable({
		select: {
			style: "multi"
		},
		//lengthChange: !1,
		buttons: ["copy", "print"],
		language: {
			paginate: {
				previous: "<i class='mdi mdi-chevron-left'>",
				next: "<i class='mdi mdi-chevron-right'>"
			}
		},
		"columns": [
			{ "data": "id" },
			{ "data": "studentid" },
			{ "data": "chinese" },
			{ "data": "maths" },
			{ "data": "english" },
			{ "data":"func"}
		],
		drawCallback: function () {
			$(".dataTables_paginate > .pagination").addClass("pagination-rounded")
		}
	});
});