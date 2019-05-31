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

        $.ajax({
            url: url,
            type: method,
            async: false,
            //traditional: true,
            data: data,
            dataType: 'json',
            beforeSend: function (XMLHttpRequest) {
                var cookie = new cookiehelep();
                var cookie_token = cookie.cookieget("cookie_token");
                if (cookie_token != null) {
                    //头部添加Bearer token
                    XMLHttpRequest.setRequestHeader("Authorization", 'Bearer ' + cookie_token);
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


class cookiehelep {
    init() {

    }
    //设置缓存
    cookieset(key, val, expiresTime) {
        /*
         *expires：（Number|Date）有效期；设置一个整数时，单位是天；也可以设置一个日期对象作为Cookie的过期日期；
         *path：（String）创建该Cookie的页面路径；
         *domain：（String）创建该Cookie的页面域名；
         *secure：（Booblean）如果设为true，那么此Cookie的传输会要求一个安全协议，例如：HTTPS；
         */
        $.cookie(key, val, { expires: expiresTime });
    }
    //读取cookei
    cookieget(key) {
        return $.cookie(key);
    }
    //删除
    cookieremove(key) {
        $.cookie(key, null);   //通过传递null作为cookie的值即可

    }
}


