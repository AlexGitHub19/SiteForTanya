function getPaginationHtml(totalItemsCount, itemsCountOnPage)
{
    var html = '';

    if (totalItemsCount > itemsCountOnPage)
    {
        html += '<button id="prevBtn" class="my-btn btn"><i class="fa fa-angle-double-left" aria-hidden="true"></i></button>';

        var buttonsCount = getButtonsCount(totalItemsCount, itemsCountOnPage);

        if (buttonsCount <= 5)
        {
            for (var i = 1; i <= buttonsCount; i++) {
                html += '<button id="paginationBtn-' + i + '" class="my-btn btn paginationBtn">' + i + '</button>';
            }
        }
        else
        {
            if (pageNumber <= 3) {
                for (var i = 1; i <= 5; i++) {
                    html += '<button id="paginationBtn-' + i + '" class="my-btn btn paginationBtn">' + i + '</button>';
                }
            }
            else if (pageNumber >= buttonsCount - 1) {
                for (var i = buttonsCount - 4; i <= buttonsCount; i++) {
                    html += '<button id="paginationBtn-' + i + '" class="my-btn btn paginationBtn">' + i + '</button>';
                }
            }
            else {
                for (var i = pageNumber - 2; i <= (parseInt(pageNumber) + parseInt(2)) ; i++) {
                    html += '<button id="paginationBtn-' + i + '" class="my-btn btn paginationBtn">' + i + '</button>';
                }
            }
        }

        html += '<button id="nextBtn" class="my-btn btn"><i class="fa fa-angle-double-right" aria-hidden="true"></i></button>';
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