$(document).ready(function() {

    var endDate = getEndDate();
    $('#calendar').fullCalendar({
        buttonText: {
            today: '今天',
            month: '月視'
        },
        monthNames: ["1月", "2月", "3月", "4月", "5月", "6月", "7月", "8月", "9月", "10月", "11月", "12月"],
        dayNames: ["星期天", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六"],
        header: {
            left: 'today',
            center: 'title',
            right: 'prev,next'
        },
        defaultDate: defaultDate,
        navLinks: false, // can click day/week names to navigate views
        editable: false,
        eventClick: function(calEvent, jsEvent, view) {
            /*alert('Event: ' + calEvent.title);
            alert('Coordinates: ' + jsEvent.pageX + ',' + jsEvent.pageY);
            alert('View: ' + view.name);*/
            showMessageContent(calEvent);
        },
        dayClick: function(date, jsEvent, view) {
            if(checkDate(date._d))
                showMesseageModal(date._d);
        },
        //viewRender: function (view, element) {
        //    alert(view.title);
        //},
        events: eventsData,
        dayRender: function (date, cell) {
            if(date._d < today || date._d > endDate)
                cell.css("background-color", "#f5f5f5");
        }
    });

    function getEndDate() {
        var enddate = new Date(today);
        enddate.setMonth(enddate.getMonth()+4);
        enddate.setDate(1);
        enddate.setDate(enddate.getDate()-1);
        return enddate;
    }

    function checkDate(date) {
        if (date > today && date <= endDate)
            return true;
        else
            return false;
    }

    function showMesseageModal(date) {
        $('#createDate').val(date.getFullYear() + '-' + (date.getMonth() + 1) + '-' + date.getDate());
        $('#nickname').val('');
        $('#content').val('');
        $('#nickname').prop('disabled', false);
        $('#content').prop('disabled', false);
        $('#select-color').prop('disabled', false);
        $('#btn-send, #btn-cancel, .text-select-color').show();
        $('#modal-message').modal('show');
    }

    $('.dropdown-option').on('click', function() {
        $('#select-color').css('background-color', $(this).css('background-color'));
    });

    $('#btn-cancel').on('click', function() {
        $('#content').val('');
        $('#modal-message').modal('hide');
    });

    $('#btn-send').on('click', function() {
        $('#modal-message').modal('hide');
        alertMessage('處理中...', 'success');

        $.ajax({
            type: "POST",
            url: createUrl,
            data: { "nickname": $('#nickname').val(), "content": $('#content').val(), "color": hexc($('#select-color').css('background-color')), "createDate": $('#createDate').val() },
            dataType: "json",
            headers: {
                'RequestVerificationToken': requestVerificationToken
            },
            success: function(result) {
                if (checkTimeout(result)) {
                    if (result.Result == true) {
                        setTimeout(function () { location.reload(); }, 500);
                    } else {
                        alertMessage('資料錯誤，請確認！', 'danger');
                    }
                } else {
                    alertMessage('伺服器忙碌中，請稍後再試！', 'danger');
                }
            },
            error: function() {
                alertMessage('伺服器忙碌中，請稍後再試！', 'danger');
            }
        });
    });

    function hexc(colorval) {
        var parts = colorval.match(/^rgb\((\d+),\s*(\d+),\s*(\d+)\)$/);
        delete(parts[0]);
        for (var i = 1; i <= 3; ++i) {
            parts[i] = parseInt(parts[i]).toString(16);
            if (parts[i].length == 1) parts[i] = '0' + parts[i];
        }
        return '#' + parts.join('');
    }

    function alertMessage(message, type) {
        $('#alert-message').html(message);

        if (type === 'success')
            $('.modal-alert').css('background-color', '#dff0d8');
        else if (type === 'danger')
            $('.modal-alert').css('background-color', '#f2dede');
        else
            $('.modal-alert').css('background-color', '#ffffff');

        $('#modal-alert').modal('show');
        setTimeout(function () { $('#modal-alert').modal('hide'); }, 3000);
    }

    function showMessageContent(calEvent){
        $('#nickname').val(calEvent.title);
        $('#content').val(calEvent.content);
        $('#select-color').css('background-color', calEvent.color);
        $('#createDate').val(calEvent.start._i);
        $('#nickname').prop('disabled', true);
        $('#content').prop('disabled', true);
        $('#select-color').prop('disabled', true);
        $('#btn-send, #btn-cancel, .text-select-color').hide();
        $('#modal-message').modal('show');
    }
});