$(document).ready(function () {
    $('#myTable').DataTable(
        {
            "Processing": true, // for show progress bar
            "serverSide": true, // for process server side
            "filter": true, // this is for disable filter (search box)
            "orderMulti": true, // for disable multiple column at once
            "pageLength": 10,
            "bSortable": true,
            "ajax": {
                "url": "/Employee/DataTable",
                "type": "POST",
                "datatype": "json"
            },
            "columnDefs": [{
                "order": [[0]],
                "targets": [0],
                "visible": false,
                "searchable": false,
                "language": {
                    "emptyTable": "No record found. Please click on Add Employee for create new record.",
                    "Processing": '<i class="fa fa- refresh fa- spin">Laoding...</i>'
                },
            }],
            columns: [
                { data: "empId", "autoWidth": true },
                { data: "employeeName", "autoWidth": true },
                { data: "designation", "autoWidth": true },
                { data: "department", "autoWidth": true },
                {
                    data: "empId",
                    "render": function (data, type, row) { return '<a class="btn btn-primary" data-toggle="modal" data-target="#exampleModal" href="" data-url="/Employee/UpdateEmployee?id=' + data + '" ><i class="fa fa-edit"></i> Edit</a>'; }
                },
                {
                    data: "empId",
                    render: function (data, type, row) {
                        return '<a href="#" class="btn btn-danger" onclick=DeleteData("' + data + '")><i class="fa fa-trash"></i> Delete</a>';
                    }
                },
            ],
            "dom": "Bfrtip",
            "buttons": [
                //{
                //    extend: 'copy',
                //    className: 'copy Button',
                //    text: '<style="background-color:lightgreen" ><i class="fa fa-clone"></i> Copy'
                //},
                {
                    extend: 'excel',
                    text: '<i class="fa fa-file-excel-o"></i> Excel'
                },
                {
                    extend: 'pdf',
                    text: '<i class="fa fa-file-pdf-o"></i> Pdf'
                },
                //{
                //    extend: 'csv',
                //    text: '<i class="fa fa-file-excel-o"></i> CSV'
                //},
                {
                    extend: 'print',
                    text: '<i class="fa fa-print"></i> Print',
                    customize: function (win) {
                        $(win.document.body)
                            .css('font-size', '10pt')
                            .prepend(
                                '<img src="http://datatables.net/media/images/logo-fade.png" style="position:absolute; top:0; left:0;" />'
                            );

                        $(win.document.body).find('table')
                            .addClass('compact')
                            .css('font-size', 'inherit');
                    }
                },
                //{
                //    text: 'Custom Button',
                //    action: function () {
                //        alert('hi');
                //    }
                //}
            ],
        });
});
$('#exampleModal').on('show.bs.modal', function (event) {
    '<img src="~/Images/Icon/Loading_Icon.gif" />'

});
$('#exampleModal').on('shown.bs.modal', function (event) {
    var url = '';
    url = event.relatedTarget.getAttribute('data-url');
    $.get(url)
        .done(function (response) {
            $('#empPopup').html(response);
        });
});




if ($.validator) {
    $.validator.setDefaults({
        ignore: [] // DON'T IGNORE PLUGIN HIDDEN SELECTS, CHECKBOXES AND RADIOS!!!
        ,
        highlight: function (element, errorClass) {
            $(element).removeClass(errorClass);
            $(element).closest('.form-group').removeClass('has-success').addClass('has-error');
        },
        unhighlight: function (element, errorClass) {
            $(element).closest('.form-group').addClass('has-success').removeClass('has-error');
        },
        errorPlacement: function (error, element) {
            if (element.data().chosen) {
                {
                    element.next().after(error);
                }
            } else {
                if (element.parent().hasClass("input-group")) {
                    element.parent().next().after(error);
                } else {
                    element.after(error);
                }
            }
        }
    });

}
function validateEmpForm() {
    $('#myform').validate({
        rules: {
            EmployeeName: { required: true },
            Designation: { required: true },
            Department: { required: true }
        },
        messages: {
            EmployeeName: { required: 'Please provide Employee Name' },
            Designation: { required: 'Please provide Employee Designation' },
            Department: { required: 'Please provide Employee Department' },

        },
        submitHandler: function (form) {
            var form = $('#myform')[0];
            var formData = new FormData(form);
            $.ajax({
                url: form.action,
                data: formData,
                type: 'POST',
                contentType: false, // NEEDED, DON'T OMIT THIS (requires jQuery 1.6+)
                processData: false, // NEEDED, DON'T OMIT THIS
                success: function (response) {
                    if (response.status == 200) {
                        $("#exampleModal").modal("hide");
                        $.notify({
                            // options
                            title: '<strong>Success</strong>',
                            message: "<br>Record has been saved successfully",
                            icon: 'glyphicon glyphicon-ok'
                        },
                            {
                                // settings
                                element: 'body',
                                //position: null,
                                type: "success",
                                //allow_dismiss: true,
                                //newest_on_top: false,
                                showProgressbar: false,
                                placement: {
                                    from: "top",
                                    align: "center"
                                },
                                offset: 20,
                                spacing: 10,
                                z_index: 1051,
                                delay: 3300,
                                timer: 1000,

                            });
                        $('#myTable').DataTable().ajax.reload();
                        //notify("saved", "App Design Pattern Mapping Updated Successfully !");
                    }
                    else {
                        $.notify({
                            // options
                            title: '<strong>Error</strong>',
                            message: "<br>There was error an occured.",
                            icon: 'glyphicon glyphicon-remove-sign',

                        }, {
                            // settings
                            element: 'body',
                            //position: null,
                            type: "danger",
                            //allow_dismiss: true,
                            //newest_on_top: false,
                            showProgressbar: false,
                            placement: {
                                from: "top",
                                align: "center"
                            },
                            offset: 20,
                            spacing: 10,
                            z_index: 1051,
                            delay: 3300,
                            timer: 1000,

                        });
                    }
                }, error: function () {
                }
            });
            return false;
        }
    });
}



function DeleteData(id) {
    if (confirm("Are you sure you want to delete ...?")) {
        Delete(id);
    } else {
        return false;
    }
}


function Delete(id) {
    var url = '@Url.Content("~/")' + "Employee/DeleteEmployee";

    $.post(url, { ID: id }, function (data) {
        if (data) {
            oTable = $('#myTable').DataTable();
            oTable.draw();
        } else {
            alert("Something Went Wrong!");
        }
    });
}