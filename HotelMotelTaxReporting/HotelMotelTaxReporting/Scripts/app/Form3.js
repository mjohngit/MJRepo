
$(document).ready(function () {
    var hv = $('#hidCertificationComplete').val();
    var docCt = $('#hidDocCount').val();

    var hiddoc = $('#item_PcisDocId');
    var hidPcis = $('#hidPcisDocNeeded').val();
 
    if ($(hiddoc.val() === undefined)) {
        $("#updSubmit").prop("disabled", false);
       
        $("#spnUpdWarning").hide();
    }

    if (docCt >= "5") {
        $("#updSubmit").prop("disabled", true);
    }
    else {
        $("#updSubmit").prop("disabled", false);
    }

    if (hidPcis == 'True') {
        $('#msgFileNotNeeded').hide();
        $("#btnSubmit").prop("disabled", true);
    } else {
        $('#msgFileNotNeeded').show();
        $("#btnSubmit").prop("disabled", false);
    }

    $('[data-toggle="popover"]').popover();
    
    if (hiddoc.val() > 0) {
        //$("#updSubmit").prop("disabled", true);
        $('#btnSubmit').prop("disabled", false);
        $("#spnUpdWarning").show();
    }    

    $("#frmDelete0").click(function () {
        if (!confirm("Do you want to delete this file?")) {
            return false;
        }
    });

    $("#frmDelete1").click(function () {
        if (!confirm("Do you want to delete this file?")) {
            return false;
        }
    });

    $("#frmDelete2").click(function () {
        if (!confirm("Do you want to delete this file?")) {
            return false;
        }
    });

    $("#frmDelete3").click(function () {
        if (!confirm("Do you want to delete this file?")) {
            return false;
        }
    });

    $("#frmDelete4").click(function () {
        if (!confirm("Do you want to delete this file?")) {
            return false;
        }
    });


    //$("#btnDelete").click(function () {
    //    $("#FileDelete").slideDown();
    //});
    //$("#cancel").click(function () {
    //    $("#FileDelete").slideUp();
    //});

    //$('button[name="remove_levels"]').on('click', function (e) {
    //    var $form = $(this).closest('form');
    //    e.preventDefault();
    //    $('#confirm').modal({ backdrop: 'static', keyboard: false })
    //        .one('click', '#delete', function (e) {
    //            $form.trigger('submit');
    //        });
    //});

    //$('#confirm-delete').on('show.bs.modal', function (e) {
    //    $(this).find('.btn-ok').attr('href', $(e.relatedTarget).data('href'));

    //    $('.debug-url').html('Delete URL: <strong>' + $(this).find('.btn-ok').attr('href') + '</strong>');
    //});

    //$('#confirm-delete').on('click', '.btn-ok', function (e) {
    //    var $modalDiv = $(e.delegateTarget);
    //    var id = $(this).data('recordId');
    //    // $.ajax({url: '/api/record/' + id, type: 'DELETE'})
    //    // $.post('/api/record/' + id).then()
    //    $modalDiv.addClass('loading');
    //    setTimeout(function() {
    //        $modalDiv.modal('hide').removeClass('loading');
    //    }, 1000);
    //});
    //$('#confirm-delete').on('show.bs.modal', function (e) {
    //    var data = $(e.relatedTarget).data();
    //    $('.title', this).text(data.recordTitle);
    //    $('.btn-ok', this).data('recordId', data.recordId);
    //});
    //$('#confirmDelete').on('show.bs.modal', function (e) {
    //    $message = $(e.relatedTarget).attr('data-message');
    //    $(this).find('.modal-body p').text($message);
    //    $title = $(e.relatedTarget).attr('data-title');
    //    $(this).find('.modal-title').text($title);
        
    //     Pass form reference to modal for submission on yes/ok
    //    var form = $(e.relatedTarget).closest('form');
    //    $(this).find('.modal-footer #confirm').data('form', form);
    //    });
    
    //<!-- Form confirm (yes/ok) handler, submits form -->
    //$('#confirmDelete').find('.modal-footer #confirm').on('click', function(){
    //    $(this).data('form').submit();
    //});
});
