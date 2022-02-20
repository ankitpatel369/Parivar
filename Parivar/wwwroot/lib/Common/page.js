function ListPagesItem(sequence, pageNum, isCurrent, html) {
    this.sequence = sequence;
    this.pageNum = pageNum;
    this.isCurrent = isCurrent;
    this.html = html;
}

function listPages(allCount, numPerPage, currPage, pagerCount, allCurrStr, allLnkStr, currStr, lnkStr, prevStr, nextStr, mainPrevStr, mainNextStr) {
    var items = [];
    if (numPerPage <= 0) numPerPage = 1;
    var pagesCount = 1;
    while (allCount > numPerPage) {
        pagesCount++;
        allCount -= numPerPage;
    }
    if (pagesCount <= pagerCount) {
        if (currPage > 1)
            items.push(new window.ListPagesItem(items.Count, 0, 0 === currPage, mainPrevStr.replace(/#0#/g, (currPage - 1)).replace("cls", "prevEnable")));
        else
            items.push(new window.ListPagesItem(items.Count, 0, 0 === currPage, mainPrevStr.replace(/#0#/g, "0").replace("cls", "prevDisable")));


        for (var i = 1; i <= pagesCount; i++) {
            //items.push(new ListPagesItem(items.Count, i, i === currPage, i === currPage ? string.Format(currStr, i) : string.Format(lnkStr, i)));
            items.push(new window.ListPagesItem(items.Count, i, i === currPage, i === currPage ? currStr.replace(/#0#/g, i) : lnkStr.replace(/#0#/g, i)));
        }
        if (pagesCount > 1) {
            //items.push(new ListPagesItem(items.Count, 0, 0 === currPage, 0 === currPage ? string.Format(allCurrStr, 0) : string.Format(allLnkStr, 0)));
            //items.push(new ListPagesItem(items.Count, 0, 0 === currPage, 0 === currPage ? allCurrStr.replace(/#0#/g, 0) : allLnkStr.replace(/#0#/g, 0)));  //removed all
        }

        if (currPage < pagesCount)
            items.push(new window.ListPagesItem(items.Count, 0, 0 === currPage, mainNextStr.replace(/#0#/g, (currPage + 1)).replace("cls", "nextEnable")));
        else
            items.push(new window.ListPagesItem(items.Count, 0, 0 === currPage, mainNextStr.replace(/#0#/g, 0).replace("cls", "nextDisable")));
        return items;
    }
    var grCount = 0;
    var tmpPage = currPage === 0 ? 1 : currPage;
    while (tmpPage > grCount * pagerCount) {
        grCount++;
    }

    if (currPage > 1)
        items.push(new window.ListPagesItem(items.Count, 0, 0 === currPage, mainPrevStr.replace(/#0#/g, (currPage - 1)).replace("cls", "prevEnable")));
    else
        items.push(new window.ListPagesItem(items.Count, 0, 0 === currPage, mainPrevStr.replace(/#0#/g, "0").replace("cls", "prevDisable")));

    var tmpCurrPage = (grCount - 1) * pagerCount;
    if (grCount > 1) {
        //items.push(new ListPagesItem(items.Count, 1, false, string.Format(lnkStr, "1")));
        items.push(new ListPagesItem(items.Count, 1, false, lnkStr.replace(/#0#/g, "1")));

        //items.push(new ListPagesItem(items.Count, (tmpCurrPage - 1), false, string.Format(prevStr, "" + (tmpCurrPage - 1))));
        items.push(new ListPagesItem(items.Count, (tmpCurrPage - 1), false, prevStr.replace(/#0#/g, "" + (tmpCurrPage - 1))));
    }
    for (var i = (grCount - 1) * pagerCount; i <= grCount * pagerCount && i <= pagesCount; i++) {
        if (i !== 0)
            //items.push(new ListPagesItem(items.Count, i, i === currPage, i === currPage ? string.Format(currStr, i) : string.Format(lnkStr, i)));
            items.push(new ListPagesItem(items.Count, i, i === currPage, i === currPage ? currStr.replace(/#0#/g, i) : lnkStr.replace(/#0#/g, i)));
        tmpCurrPage++;
    }
    if (tmpCurrPage <= pagesCount) {
        //items.push(new ListPagesItem(items.Count, tmpCurrPage, false, string.Format(nextStr, tmpCurrPage)));
        items.push(new ListPagesItem(items.Count, tmpCurrPage, false, nextStr.replace(/#0#/g, tmpCurrPage)));

        //items.push(new ListPagesItem(items.Count, pagesCount, false, string.Format(lnkStr, "" + pagesCount)));
        items.push(new ListPagesItem(items.Count, pagesCount, false, lnkStr.replace(/#0#/g, "" + pagesCount)));
    }
    //items.push(new ListPagesItem(items.Count, 0, 0 === currPage, 0 === currPage ? string.Format(allCurrStr, 0) : string.Format(allLnkStr, 0)));
    //items.push(new ListPagesItem(items.Count, 0, 0 === currPage, 0 === currPage ? allCurrStr.replace(/#0#/g, 0) : allLnkStr.replace(/#0#/g, 0))); //removed all


    if (currPage < pagesCount)
        items.push(new ListPagesItem(items.Count, 0, 0 === currPage, mainNextStr.replace(/#0#/g, (currPage + 1)).replace("cls", "nextEnable")));
    else
        items.push(new ListPagesItem(items.Count, 0, 0 === currPage, mainNextStr.replace(/#0#/g, 0).replace("cls", "nextDisable")));
    return items;
}

function RenderPaginationResult(data) {
    var element = `<div class="col-md-6 col-xl-4" data-orgId="{0}">
            <div class="card card-border {1}">
                <div class="card-header">
                    <div class="card-title pull-right btn-group-sm">
                        <a href="{0}" class="btn btn-success waves-effect waves-light tooltips" data-placement="top" data-toggle="tooltip" data-original-title="Edit">
                            <i class="fa fa-pencil"></i>
                        </a>
                        <a href="#" class="btn btn-primary waves-effect waves-light tooltips" data-placement="top" data-toggle="tooltip" data-original-title="View">
                            <i class="fa fa-eye"></i>
                        </a>
                    </div>
                    <h3 class="card-title">{2}</h3>
                </div>
                <div class="card-body">
                    <div class="media-main">
                        <a class="pull-left" href="#">
                            <img class="thumb-lg rounded-circle" src="~/adminTheme/assets/images/users/avatar-4.jpg" alt="">
                        </a>
                        <div class="info">
                            <h4>{3}</h4>
                            <p class="text-muted">{4}</p>
                        </div>
                    </div>
                    <div class="clearfix"></div>
                    <hr>
                    <ul class="social-links list-inline m-0">
                        <li class="list-inline-item">
                            <a class="action-menu tooltips" data-toggle="dropdown" data-toggle-second="tooltip" title="Social Links" aria-haspopup="true" aria-expanded="false">
                                <i class="fa fa-internet-explorer"></i>
                            </a>
                            <ul class="dropdown-menu">
                                <li><a class="inlite-menu" href="{5}" target="_blank"><i class="fa fa-internet-explorer"></i> Website</a></li>
                                <li><a class="inlite-menu" href="javascript:;" target="_blank"><i class="fa fa-facebook"></i> Facebook</a></li>
                                <li><a class="inlite-menu" href="{6}" target="_blank"><i class="fa fa-twitter"></i> Twitter</a></li>
                            </ul>
                        </li>
                        <li class="list-inline-item">
                            <a href="javascript:;" target="_blank" onclick="updateLink(this,'{7}')" data-toggle="tooltip" class="tooltips" title="Location">
                                <i class="fa fa-location-arrow"></i>
                            </a>
                        </li>
                        <li class="list-inline-item pull-right">
                            <a class="action-menu" data-toggle="dropdown" data-toggle-second="tooltip" title="Options" aria-haspopup="true" aria-expanded="false">
                                <i class="fa fa-bars"></i>
                            </a>
                            <ul class="text-center dropdown-menu">
                                <li><a class="inlite-menu" href="#">Action</a></li>
                                <li><a class="inlite-menu" href="#">Another action</a></li>
                                <li><a class="inlite-menu" href="#">Something else here</a></li>
                                <li class="dropdown-divider"></li>
                                <li><a class="inlite-menu" href="#">Separated link</a></li>
                            </ul>
                        </li>
                    </ul>
                </div>
            </div>
        </div>`;
    var content = "";
    if (data.length === 0) {
        content = `<center><h3 class="text-center">No Medicine Found !</h3></center>`;
    } else {

        var ElementData = "";
        if (data.length !== 0) {
            $.each(data, function (i, item) {
                ElementData += element.SetFormat(
                    item.Id, // {0}
                    (item.IsApproved ? "" : "card-danger"), // {1}
                    item.OrganisationName, // {2}
                    item.FullName, // {3}
                    item.Email, // {4}
                    item.Website, // {5}
                    item.Twitter, // {6}
                    item.FullAddress); // {7}
            });
            content = ElementData;
        }
    }
    return content;
}