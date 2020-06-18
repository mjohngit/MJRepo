
$(document).ready(function () {

    var surveyYr;

    $("#btnPrevYear2Fl").click(function () {
        surveyYr = $("#hidPrevYear2").val();
        $("#hidSurveyYear").val(surveyYr);
    });

    $("#btnPrevYear2Pr").click(function () {
        surveyYr = $("#hidPrevYear2").val();
        $("#hidSurveyYear").val(surveyYr);

    });

    $("#btnPrevYearFl").click(function () {
        surveyYr = $("#hidPrevYear").val();
        $("#hidSurveyYear").val(surveyYr);
    });

    $("#btnPrevYearPr").click(function () {
        surveyYr = $("#hidPrevYear").val();
        $("#hidSurveyYear").val(surveyYr);
    });

    $("#btnCurrYearFl").click(function () {
        surveyYr = $("#hidCurrYear").val();
        $("#hidSurveyYear").val(surveyYr);
    });

    $("#btnCurrYearPr").click(function () {
        surveyYr = $("#hidCurrYear").val();
        $("#hidSurveyYear").val(surveyYr);
    });
});