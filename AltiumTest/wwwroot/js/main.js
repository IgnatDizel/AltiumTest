(function (altiumTest) {
    altiumTest.HubConnection = altiumTest.HubConnection || {};
    altiumTest.UserName = altiumTest.UserName || {};

    altiumTest.InitHub = function () {


        altiumTest.HubConnection = new signalR.HubConnectionBuilder()
            .withUrl("/chat?userName=" + altiumTest.UserName)
            .build();

        altiumTest.HubConnection.on("UpdateCount", function (count) {
            let counter = document.getElementById("counter");
            counter.innerHTML = count;
        });

        altiumTest.HubConnection.on("Send", function (data) {
            altiumTest.AddMessageDown(data);
            altiumTest.ScrolDownIfNeeded();
            $("#message").val("");
        });

        $("#sendBtn").click(function () {
            let message = {};
            message.text = $("#message").val();
            message.userName = altiumTest.UserName;

            if (message.text && message.userName) {
                altiumTest.HubConnection.invoke("Send", message);
            }
        });

        $("#messagebox").scroll(function () {
            if ($("#messagebox").scrollTop() == 0) {
                let $firstMessage = $("#messagebox").children().first();

                altiumTest.GetMessages(function (data) {
                    let topBeforeInsert = $firstMessage.offset().top;

                    for (let i = data.length - 1; i >= 0; i--) {
                        altiumTest.AddMessageUp(data[i]);
                    }

                    let topAfterInsert = $firstMessage.offset().top;
                    $("#messagebox").scrollTop(topAfterInsert - topBeforeInsert);

                }, $firstMessage.attr("created"))
            }
        });

        altiumTest.HubConnection.start();
    };

    altiumTest.AddMessageDown = function (message) {
        $('<p></p>')
            .text(message.userName + ": " + message.text)
            .attr({ created: message.created })
            .attr({ id: 'm' + message.id })
            .appendTo("#messagebox");
    };

    altiumTest.AddMessageUp = function (message) {
        $('<p></p>')
            .text(message.userName + ": " + message.text)
            .attr({ created: message.created })
            .attr({ id: 'm' + message.id })
            .prependTo("#messagebox");
    };

    altiumTest.ScrolDownIfNeeded = function (duration = 1000) {
        let messageBoxScrollHeight = $("#messagebox")[0].scrollHeight;
        let messageBoxScrollTop = $("#messagebox").scrollTop();
        let messageBoxHeight = $("#messagebox").height();
        let lengthToBottomInPixels = messageBoxScrollHeight - messageBoxScrollTop - messageBoxHeight;

        if (lengthToBottomInPixels < 500) {
            altiumTest.ScrolDown(1000);
        }
    };

    altiumTest.ScrolDown = function (duration) {
        $("#messagebox").animate({ scrollTop: $("#messagebox")[0].scrollHeight }, duration);
    };

    altiumTest.Login = function () {
        altiumTest.UserName = $("#login").val();


        altiumTest.InitHub();

        altiumTest.GetMessages(function (data) {
            for (let i = 0; i < data.length; i++) {
                altiumTest.AddMessageDown(data[i]);
            }
            $("#loginForm").hide();
            $("#chatbox").show();
            altiumTest.ScrolDown(0);

        });

    }

    altiumTest.GetMessages = function (callback, lastMessageTime) {

        let url = "/api/v1/messages";

        if (lastMessageTime) {
            url = url + "?lastMessageTime=" + lastMessageTime;
        }

        $.get(url, callback);
    }

})(window.altiumTest = window.altiumTest || {});