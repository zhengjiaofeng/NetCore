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
        var url ="Account/Login"
          $.ajax({
            url: url,
              type: "POST",
            async: false,
            data: data,
            dataType: 'json',
            error: function () { DiaLog("亲，提交出错了，稍后再试哦……") },
            success: function (d) {
                if (d.isSucess == true) {
                    var cookie = new cookiehelep();
                    cookie.cookieremove("cookie_token")

                    var expiresDate = new Date();
                    expiresDate.setTime(expiresDate.getTime() + (1 * 60 * 1000)); 
                    cookie.cookieset("cookie_token", d.token, expiresDate)
                    window.location.href = d.msg;
                } else {
                    common.LayerAlert(d.msg);
                }
            },
            error: function errorCallback(xmlHttpRequest, textStatus, errorThrown) {
                common.LayerAlert(errorThrown + ":" + textStatus);
            }
        });
    
    }

};


