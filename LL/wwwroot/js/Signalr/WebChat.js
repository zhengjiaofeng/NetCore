//连接signalr
const connection = new signalR.HubConnectionBuilder().withUrl("https://localhost:44377/ChatHub").build();
connection.on("revicemsg", function (username, message) {
    var htmlstr = "<li>" + username + ":" + message + "</li>";
    $("#rec_message").append(htmlstr);
});
connection.start().catch(err => console.log(err.toString()));

//用户名
const userName = Math.random().toString(36).substr(2);

//发送信息事件
function SendMessage() {
    var msg = $("#send_message").val();
    connection.invoke("SendAllMessage", userName, msg).catch(err => console.log(err.toString()));
    $("#send_message").val("");
}