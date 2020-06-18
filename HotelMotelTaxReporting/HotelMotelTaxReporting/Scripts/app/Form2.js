
$(document).ready(function () {
    
    var hv = $('#hidCertificationComplete');
   
    if ($(hv).val() == "True") {
        $("#btnSavePurposeAmts").attr('disabled', 'disabled');
    }
    $('[data-toggle="popover"]').popover();
  

   $(':text').focus(function () {
       $(this).one('mouseup', function (event) {
           event.preventDefault();
       }).select();
   });

    function postFormDmo() {
        $.ajax({
            type: "POST",
            url: '@Url.Action("AddPartialNewContractDmo")',
            data: $('form').serialize(),
            success: function(response) {
                console.log(response);
            }
        });
    }
});

