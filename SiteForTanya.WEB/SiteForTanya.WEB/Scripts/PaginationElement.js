function getPaginationHtml(totalItemsCount, itemsCountOnPage, pageNumber)
{
    var html = '';

    if (totalItemsCount > itemsCountOnPage)
    {
        html += '<div id="prevBtn" class="paginationElement"><i class="prevBtn fa fa-angle-double-left" aria-hidden="true"></i></div>';

        var buttonsCount = getButtonsCount(totalItemsCount, itemsCountOnPage);

        if (buttonsCount <= 5)
        {
            for (var i = 1; i <= buttonsCount; i++) {
                html += '<div id="paginationBtn-' + i + '" class="paginationElement paginationBtn">' + i + '</div>';
            }
        }
        else
        {
            if (pageNumber <= 3) {
                for (var i = 1; i <= 5; i++) {
                    html += '<div id="paginationBtn-' + i + '" class="paginationElement paginationBtn">' + i + '</div>';
                }
            }
            else if (pageNumber >= buttonsCount - 1) {
                for (var i = buttonsCount - 4; i <= buttonsCount; i++) {
                    html += '<div id="paginationBtn-' + i + '" class="paginationElement paginationBtn">' + i + '</div>';
                }
            }
            else {
                for (var i = pageNumber - 2; i <= (parseInt(pageNumber) + parseInt(2)) ; i++) {
                    html += '<div id="paginationBtn-' + i + '" class="paginationElement paginationBtn">' + i + '</div>';
                }
            }
        }

        html += '<div id="nextBtn" class="paginationElement"><i class="fa fa-angle-double-right" aria-hidden="true"></i></div>';
        if (buttonsCount > 5) {
            html += '<div id = "goToPageContainer"><label id="gotoLabel">GO TO</label><input id="inputGoToPage" type="text" class="form-control"><label id="ofLabel">OF ' + buttonsCount + '</label></div>';
        }
    }

    return html;
}

function getButtonsCount(totalItemsCount, itemsCountOnPage)
{
    var buttonsCount = parseInt(totalItemsCount / itemsCountOnPage);
    if (totalItemsCount % itemsCountOnPage > 0)
    {
        buttonsCount++;
    }

    return buttonsCount;
}