
$(document).ready(function () {
    var submit = $('input[type="submit"]');
    //var hv = $('#hidP1Complete').attr("value");
    var hv = $('#hidP1Complete');

    if ($('#hidP1Complete').val() == "True") {
        //alert('I am in Form 1 complete condition');
        submit.prop('disabled', true);
        //$('#radioSection').attr('disabled', true);
        //$("#radioSection").find("input, button").attr("disabled", true);
        //hv.prop('disabled', true);
        $('input[name=OrdCorrect]').attr("disabled", true);
        $('input[name=ShortTermRentals]').attr("disabled", true);
    } else {
        //alert('I am in Form 1 NOT complete condition')
        submit.prop('disabled', true);
        $('input[name=OrdCorrect]').attr("disabled", false);
        $('input[name=ShortTermRentals]').attr("disabled", false);
    }

    $("#radioSection input[name='OrdCorrect']").click(function () {
        if($("input:radio[name='ShortTermRentals']").is(":checked"))
        {
            submit.prop('disabled', false);
        }
        
    });

    $("#radioSection2 input[name='ShortTermRentals']").click(function () {
        if($("input:radio[name='OrdCorrect']").is(":checked"))
        {
            submit.prop('disabled', false);
        }
    });

    $('[data-toggle="popover"]').popover();
});