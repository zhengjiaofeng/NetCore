$(function () {

});
var common = {
    init: function () {

    },

    //Layer alert
    LayerAlert: function (msg) {
        layer.alert(msg, {
            title: '提示',
            icon: 5,
            time: 2000, //2秒关闭（如果不配置，默认是3秒）
            shade: [0.8, '#393D49'],
            anim: 6
        });
    },
    //加密
    rsaencrypt: function (key,value) {
        var encrypt = new JSEncrypt();
        encrypt.setPublicKey(key);
        var encrypt_data = encrypt.encrypt(value);
        return encrypt_data;
    }
    
}

var commonajax = {
    init: function () {

    },
    //ajax 封装
    Ajax: function (method, url, data, successfn, errorfn) {

        console.log(data);
        $.ajax({
            url: url,
            type: method,
            async: false,
            traditional: true,
            //data: JSON.stringify(data),
            data: data,
            dataType: 'json',
            beforeSend: function () {
                var token = localStorage.getItem("token");
                console.log(token);
                if (token != null) {
                    //request.setRequestHeader("Authorization", 'Bearer '+token);
                }
            },
            error: function () { DiaLog("亲，提交出错了，稍后再试哦……") },
            success: function (d) {
                successfn(d);
            },
            error: function errorCallback(xmlHttpRequest, textStatus, errorThrown) {
                if (errorfn == null) {
                    common.LayerAlert(errorThrown + ":" + textStatus);
                } else {
                    errorfn();
                }
            }
        });

    }
    
}

