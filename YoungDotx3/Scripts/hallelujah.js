$(document).ready(function() {

    var date = '';
    var lastId;
    var page = 1;
    var isQuerying = false;
    getMessages();

    $(window).scroll(function() {
        if (!isQuerying && lastId > 1) {
            // 內容剩餘高度 = 內容高度 - (螢幕高度 + 距離頂部高度)，低於300時去要資料
            var contentHeight = $(".container").height() - ($(window).height() + $(window).scrollTop());
            if (!isQuerying && contentHeight < 300) {
                isQuerying = true;
                getMessages();
            }
        }
    });

    function getMessages() {
        $.ajax({
            type: "POST",
            url: getMessagesUrl,
            data: { "id": lastId },
            dataType: "json",
            headers: {
                'RequestVerificationToken': requestVerificationToken
            },
            success: function(result) {
                if (checkTimeout(result)) {
                    page++;
                    var models = result.messages;
                    for (var i = 0; i < models.length > 0; i++) {
                        $('#initial').before(bindMessageHtml(models[i], false));
                    }
                } else {
                    alertMessage('伺服器忙碌中，請稍後再試！', 'danger');
                }
            },
            error: function() {
                alertMessage('伺服器忙碌中，請稍後再試！', 'danger');
            },
            complete: function() {
                isQuerying = false;
            }
        });
    }

    function createMessage() {
        alertMessage('處理中...', 'success');

        $.ajax({
            type: "POST",
            url: createMessageUrl,
            data: { "nickname": $('#nickname').val(), "content": $('#content').val() },
            dataType: "json",
            headers: {
                'RequestVerificationToken': requestVerificationToken
            },
            success: function(result) {
                if (checkTimeout(result)) {

                    if (result.isSuccess) {
                        $('#content').val('');
                        var message = result.messageModel;
                        if ($('.' + message.CreateDate).length > 0) {
                            $('.' + message.CreateDate).after(bindMessageHtml(message, true));
                        } else {
                            $('#leave-message').after(bindMessageHtml(message, true));
                        }
                        $('#modal-alert').modal('hide');
                    } else {
                        $('#modal-alert').modal('hide');
                        alertMessage('新增失敗，請稍後再試！', 'danger');
                    }
                }
            },
            error: function(result) {
                alertMessage('伺服器忙碌中，請稍後再試！', 'danger');
            }
        });
    }

    function bindMessageHtml(message, isNewCreate) {

        var html = '';
        if (isNewCreate) {
            if ($('.' + message.CreateDate).length == 0)
                html += bindDateIconHtml(message.CreateDate);
        } else {
            lastId = message.Id;
            if (date != message.CreateDate) {
                date = message.CreateDate;
                html += bindDateIconHtml(date);
            }
        }

        html += '<li class="message">';
        html += getIconHtml(message.Id);
        html += '   <div class="timeline-item">';
        html += '       <span class="time"><i class="fa fa-clock-o"></i>' + message.CreateTime + '</span>';
        html += '       <h3 class="timeline-header">' + message.Nickname + '</h3>';
        html += '       <div class="timeline-body">';
        html += getMessageHtml(message.Content);
        html += '       </div>';
        html += '       <div class="timeline-footer"></div>';
        html += '   </div>';
        html += '</li>';

        return html;
    }

    function bindDateIconHtml(date) {
        var html = '';
        html += '   <li class="time-label ' + date + '">';
        html += '       <span class="tag is-danger">' + date + '</span>';
        html += '   </li>';
        return html;
    }

    function getIconHtml(seq) {
        var base = 3;
        if (seq % base === 0) {
            return '<span class="fa fa-envelope tag is-info"></span>';
        } else if (seq % base === 1) {
            return '<span class="fa fa-commenting-o tag is-danger"></span>';
        } else {
            return '<span class="fa fa-comments-o  tag is-primary"></span>';
        }
    }

    function getMessageHtml(content) {
        var result = '';
        var lines = content.split('\n');
        for (var i = 0; i < lines.length; i++) {
            result += '       <p>' + lines[i] + '</p>';
        }
        return result;
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
        setTimeout(function() { $('#modal-alert').modal('hide'); }, 3000);
    }

    $('#btn-cancel').on('click', function() {
        $('#nickname, #content').val('');
    });

    $('#btn-send').on('click', function() {
        if ($('#nickname').val() === '') {
            alertMessage('請輸入暱稱！', 'danger');
            $('#nickname').trigger('focus');
        } else if ($('#content').val() === '') {
            alertMessage('請輸入內容！', 'danger');
            $('#content').trigger('focus');
        } else {
            createMessage();
        }
    });
});