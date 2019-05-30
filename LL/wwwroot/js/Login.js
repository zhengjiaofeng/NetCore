$(function () {
    DoLogin.init();
});
var DoLogin = {
    //初始化函数
    init: function () {
        this.AutoCarousel();
    },
    //自定义函数
    AutoCarousel: function () {
        $("#carousel_photo").carousel({
            interval: 1000,
            keyboard: false
        });
    },
    //登录
    DoLogin: function () {

        var userName = $("#UserName").val();
        var passWord = $("#PassWord").val();
        if (userName == "") {
            common.LayerAlert("用户名不能为空！");
            return;
        }
        if (passWord === "") {
            common.LayerAlert("密码不能为空！");
            return;
        }
        var data = {
            UserName: userName,
            PassWord: common.rsaencrypt($("#rsaPublicKey").val(), passWord)
        }
        commonajax.Ajax("POST", "Account/Login", data, function (d) {
            if (d.isSucess == true) {
                window.location.href = d.msg;
            } else {
                common.LayerAlert(d.msg);
            }
        }, null);
    
    }

};
