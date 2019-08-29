//连接signalr
var cookie = new cookiehelep();
var cookie_token = cookie.cookieget("cookie_token");
var userName = $("#userName").html();
const connection = new signalR.HubConnectionBuilder().withUrl("https://localhost:44377/ChatHub?userNmae=" + userName).withHubProtocol(new signalR.protocols.msgpack.MessagePackHubProtocol()).build();
connection.on("revicemsg", function (username, message) {
    var htmlstr = "<li>" + username + ":" + message + "</li>";
    $("#rec_message").append(htmlstr);
});

//添加用户集合
connection.on("addUsers", function (userName) {
    var htmlstr = "<li>" + userName + ":</li>";
    $("#rec_users").append(htmlstr)
});
//移除用户列表
connection.start().catch(err => console.log(err.toString()));



//发送信息事件
function SendMessage() {
    //获取编辑器html内容：详细
    var msg = ue.getContent();
    /* ps  MessagePack区分大小写 */
    connection.invoke("SendAllMessage", { Sender: userName, Message: msg }).catch(err => console.log(err.toString()));
    //情况内容
    ue.setContent('');
    ue.focus();
}

var ue;
$(function () {
     ue = UE.getEditor('msg_ueditor', {
        initialFrameHeight: 200
    });

   
});
