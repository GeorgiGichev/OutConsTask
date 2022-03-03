// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

const fBtn = $("#FBtn");
const tBtn = $("#TBtn");
const currentPageBtn = $("#SBtn");
const prevContainer = $("#prevContainer");
const nextContainer = $("#nextContainer");
const usersTable = $("#usersTable");
const usersRadio = $("#usersRadio");
const projectsRadio = $("#projectsRadio");
const startDateElement = $("#startDate");
const lastDateElement = $("#endDate");
const filterButton = $("#filterButton");

const topUsersBaseURL = "/Home/GetTop10Users";
const topProjectsBaseURL = "/Project/GetTop10Projects";

$(document).ready(function () {
    if (fBtn.text() == 0) {
        fBtn.hide();
    }

    getTop10(topUsersBaseURL, "Users");

    $(function () {
        let radios = $('input:radio[name=inlineRadioOptions]');
        if (radios.is(':checked') === false) {
            radios.filter('[value=user]').prop('checked', true);
        }
    });

    $(function () {
        let radios = $('input:radio[name=sortInlineRadioOptions]');
        if (radios.is(':checked') === false) {
            radios.filter('[value=byId]').prop('checked', true);
        }
    });

    $('input:radio[name=sortInlineRadioOptions]').change(function () {
        var value = $(this).val();
        sortTable(value);
    })

    $("#clearButton").click(function () {
        startDateElement.val("");
        lastDateElement.val("");
        if ($(usersRadio).prop("checked")) {
            getTop10(topUsersBaseURL, "Users");
        }
        else if ($(projectsRadio).prop("checked")) {
            getTop10(topProjectsBaseURL, "Projets");
        }

        getUsersByPage(1);
    })

    $('input:radio[name=inlineRadioOptions]').change(function () {
        loadTop10();
    })

    $("#filterButton").click(function () {
        loadTop10();
        getUsersByPage(1);
    })

    $(".hoursBtn").click(showHours);

    $("#PrevBtn").click(function () {
        const prevPage = parseInt(currentPageBtn.text()) - 1;
        getUsersByPage(prevPage);
    })

    $("#NextBtn").click(function () {
        const nextPage = tBtn.text();
        getUsersByPage(nextPage);
    })

    $("#FBtn").click(function () {
        const value = $(this).text();
        getUsersByPage(value);
    })

    $("#TBtn").click(function () {
        const value = $(this).text();
        getUsersByPage(value);
    })
})

function loadTop10() {
    let startDate = startDateElement.val();
    let lastDate = lastDateElement.val();
    const minDate = '0001-01-01';
    let postUrl = "";
    if (startDate == "" && lastDate != "") {
        postUrl = `?startDate=${minDate}&lastDate=${lastDate}`;
    }
    else if (startDate != "" && lastDate == "") {
        postUrl = `?startDate=${startDate}`;
    }
    else if (startDate != "" && lastDate != "") {
        postUrl = `?startDate=${startDate}&lastDate=${lastDate}`;
    }
    
    let url = "";
    if ($(usersRadio).prop("checked")) {
        url = topUsersBaseURL + postUrl;
        getTop10(url, "Users");
    }
    else if ($(projectsRadio).prop("checked")) {
        url = topProjectsBaseURL + postUrl;
        getTop10(url, "Projets");
    }
}

function showHours() {
    const idElement = $(this).attr("userid");
    $.ajax({
        url: "/Project/HoursByUser?userId=" + idElement,
        type: 'GET',
        success: function (res) {
            let user;
            let projects = {};
            $(res).each(function (key, value) {
                user = value.userNames
                projects[value.projectName] = value.hours
            })
            let output = "";
            $.each(projects, function (key, value) {
                let formatedHours = parseFloat(value).toFixed(2);
                output += `<p>${key + " : " + formatedHours + "h"}</p>`
            })
            let pop = new popup([{ text: "Close" }], user, output);
            pop.open();
        }
    });
}

function getUsersByPage(pageNumber) {
    let startDate = startDateElement.val();
    let lastDate = lastDateElement.val();
    const minDate = '0001-01-01';
    let postUrl = "";
    if (startDate == "" && lastDate != "") {
        postUrl = `&startDate=${minDate}&lastDate=${lastDate}`;
    }
    else if (startDate != "" && lastDate == "") {
        postUrl = `&startDate=${startDate}`;
    }
    else if (startDate != "" && lastDate != "") {
        postUrl = `&startDate=${startDate}&lastDate=${lastDate}`;
    }
    $.ajax({
        url: "/Home/AllUsers?id=" + pageNumber + postUrl,
        type: 'GET',
        success: function (res) {
            movePage(res);
            populateTable(res);
        }
    });
}

function populateTable(res) {
    usersTable.html("");
    const users = res.usersForListing;
    $(users).each(function () {
        const row = document.createElement("tr");
        const idElement = document.createElement("td");
        const fNameElement = document.createElement("td");
        const surnameElement = document.createElement("td");
        const emailElement = document.createElement("td");
        const btnElementContainer = document.createElement("td");
        const btn = document.createElement("input");
        $(btn).addClass("btn btn-primary btn-block hoursBtn");
        $(btn).attr("type", "button");
        $(btn).attr("userid", this.id);
        $(btn).val("View Project Hours");
        $(btn).click(showHours);
        $(btnElementContainer).html(btn);
        $(idElement).text(this.id);
        $(fNameElement).text(this.name);
        $(surnameElement).text(this.surname);
        $(emailElement).text(this.email);
        $(row).append(idElement);
        $(row).append(fNameElement);
        $(row).append(surnameElement);
        $(row).append(emailElement);
        $(row).append(btnElementContainer);
        $(usersTable).append(row);
    });
}

function movePage(res) {
    if (!res.hasPreviousPage) {
        prevContainer.addClass("disabled");
        fBtn.hide();
    }
    else {
        prevContainer.removeClass("disabled");
        fBtn.show();
    }

    if (!res.hasNextPage) {
        nextContainer.addClass("disabled");
        tBtn.hide();
        console.log("nqma")
    }
    else {
        nextContainer.removeClass("disabled");
        tBtn.show();
        console.log("ima")
    }

    currentPageBtn.text(res.pageNumber);
    fBtn.text(res.previousPageNumber);
    tBtn.text(res.nextPageNumber);
}

function popup(buttons, title, html) {
    var popup_html = "<div class=\"popup_wrapper\"><form class=\"popup\">";
    if (title) {
        popup_html += "<h2 class=\"title\">" + title + "</h2>";
    }
    if (html) {
        popup_html += html;
    }
    if (buttons) {
        popup_html += "<div class=\"buttons\">";
        for (var x = 0; x < buttons.length; x++) {
            var bClass = buttons[x]["class"] ? " class=" + buttons[x]["class"] : "";
            var bCheckForm = buttons[x]["checkForm"] ? " data-checkForm=" + buttons[x]["checkForm"] : "";
            var bClose = buttons[x]["close"] === false ? " data-close=" + buttons[x]["close"] : "";
            var bValue = buttons[x]["value"] !== undefined ? " data-value=" + buttons[x]["value"] : "";
            var bText = buttons[x]["text"] || "";
            popup_html += "<button" + bClass + bClose + bCheckForm + bValue + ">" + bText + "</button>";
        }
        popup_html += "</div>";
    }
    popup_html += "</form></div>";
    var popup = $(popup_html);
    var form = popup.children("form");
    var top;
    function open() {
        $("body").append(popup);
        popup.fadeIn(500);
        top = $("body").scrollTop();
        $("html").css({ "position": "fixed", "top": -top });
    }
    function close() {
        popup.fadeOut(200, function () {
            popup.remove();
            $("html").css({ "position": "static", "top": 0 });
            $("html, body").scrollTop(top);
        });
    }
    this.open = function (f) {
        var r = $.Deferred();
        open();
        
        popup.on("click", ".buttons button", function () {
            close();
        });
        return r.progress(f);
    };
    this.close = function () {
        close();
    };
    this.addClass = function (fClass) {
        $(popup).addClass(fClass);
    };
    this.removeClass = function (fClass) {
        $(popup).removeClass(fClass);
    };
}

function getTop10(url, title) {
    $.ajax({
        url: url,
        type: 'GET',
        success: function (res) {
            google.charts.load('current', { packages: ['corechart', 'bar'] });
            google.charts.setOnLoadCallback(drawBarColors);
            function drawBarColors() {
                let arr = []
                $(res).each(function (i, item) {
                    let innerArr = ['', item.totalHours, 'blue', item.fullName];
                    arr.push(innerArr)
                })

                var data = google.visualization.arrayToDataTable([
                    ['Element', 'Hours', { role: 'style' }, { role: 'annotation' }],
                    ['', populateEmptyFields(arr[0], "number"), 'green', populateEmptyFields(arr[0], "string")],
                    ['', populateEmptyFields(arr[1], "number"), 'red', populateEmptyFields(arr[1], "string")],
                    ['', populateEmptyFields(arr[2], "number"), 'blue', populateEmptyFields(arr[2], "string")],
                    ['', populateEmptyFields(arr[3], "number"), 'orangered', populateEmptyFields(arr[3], "string")],
                    ['', populateEmptyFields(arr[4], "number"), 'turquoise', populateEmptyFields(arr[4], "string")],
                    ['', populateEmptyFields(arr[5], "number"), 'deeppink', populateEmptyFields(arr[5], "string")],
                    ['', populateEmptyFields(arr[6], "number"), 'darkorange', populateEmptyFields(arr[6], "string")],
                    ['', populateEmptyFields(arr[7], "number"), 'coral', populateEmptyFields(arr[7], "string")],
                    ['', populateEmptyFields(arr[8], "number"), 'blanchedalmond', populateEmptyFields(arr[8], "string")],
                    ['', populateEmptyFields(arr[9], "number"), 'aquamarine', populateEmptyFields(arr[9], "string")],
                ]);
                var options = {
                    title: 'Top 10 ' + title,
                    chartArea: { width: '100%' },
                    colors: ['#b0120a', '#ffab91'],
                    hAxis: {
                        title: 'Total Hours',
                        minValue: 0
                    },
                   
                };
                var chart = new google.visualization.BarChart(document.getElementById('chart_div'));
                chart.draw(data, options);
            }
        }
    });
}

function populateEmptyFields(value, type) {
    if (value == undefined && type == "number") {
        return 0;
    }
    else if (value == undefined && type == "string") {
        return "";
    }
    else if (value != undefined && type == "number") {
        return value[1];
    }
    else if (value != undefined && type == "string") {
        return value[3];
    }
}

function sortTable(sortCriteria) {
    let index = 0;
    if (sortCriteria == "byName") {
        index = 1;
    }
    var table, rows, switching, i, x, y, shouldSwitch;
    table = $("#usersTable");
    switching = true;
    while (switching) {
        switching = false;
        rows = table.find("tr");
     
        for (i = 0; i < (rows.length - 1); i++) {
            shouldSwitch = false;
            x = $(rows[i]).find("td")[index];
            y = $(rows[i + 1]).find("td")[index];
            if (index == 0) {
                if (parseInt($(x).text()) > parseInt($(y).text())) {
                    shouldSwitch = true;
                    break;
                }
            }
            else if (index == 1) {
                if ($(x).text().toLowerCase() > $(y).text().toLowerCase()) {
                    shouldSwitch = true;
                    break;
                }
            }
            
        }
        if (shouldSwitch) {
            rows[i].parentNode.insertBefore(rows[i + 1], rows[i]);
            switching = true;
        }
    }
}