function AddTimeByTimeZone() {

    var timeZoneId = $("#cboTimeZone :selected").text();

    $.ajax({
        url: "/Home/AddTimeByTimeZone?timeZoneId=" + timeZoneId,
        type: "Get",
        async: false,
        beforeSend: function (xhr) {

           //do cosmetic stuffs here!
            //hideProgressBar(false);
            //generateProgressBar();
        }
    })
        .done(function (data) {

            //hideProgressBar(true);

            if (data != undefined && data.length > 0) {

                $('#tblCurrentTimeQuery > tbody').empty();

                for (i = 0; i < data.length; i++) {

                    if (i % 2 != 0) {
                        $('#tblCurrentTimeQuery tbody').append(
                            '<tr class=\"table-active\">' +
                            '<td>' + data[i].CurrentTimeQueryId + '</td>' +
                            '<td>' + data[i].ClientIp + '</td>' +
                            '<td>' + formatAMPM(data[i].Time) + '</td>' +
                            '<td>' + formatAMPM(data[i].UTCTime) + '</td>' +
                            '<td>' + data[i].Timezone + '</td>' +
                            '</tr>'
                        );
                    }
                    else {
                        $('#tblCurrentTimeQuery tbody').append(
                            '<tr>' +
                            '<td>' + data[i].CurrentTimeQueryId + '</td>' +
                            '<td>' + data[i].ClientIp + '</td>' +
                            '<td>' + formatAMPM(data[i].Time) + '</td>' +
                            '<td>' + formatAMPM(data[i].UTCTime) + '</td>' +
                            '<td>' + data[i].Timezone + '</td>' +
                            '</tr>'
                        );
                    }
                    
                }
            }
        });
}

function formatAMPM(queryTime) {

    var mydate = new Date(queryTime);

    var hours = mydate.getHours();
    var minutes = mydate.getMinutes();
    var seconds = mydate.getSeconds();

    var day = mydate.getDate();
    var month = mydate.getMonth() + 1;
    var year = mydate.getFullYear();

    var ampm = hours >= 12 ? 'PM' : 'AM';

    hours = hours % 12;
    hours = hours ? hours : 12; // the hour '0' should be '12'
    minutes = minutes < 10 ? '0' + minutes : minutes;
    var time = day + '/' + month + '/' + year + ' ' + hours + ':' + minutes + ':' + seconds + ' ' + ampm;

    return time;
}