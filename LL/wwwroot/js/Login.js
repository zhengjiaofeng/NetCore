$(function () {
    DoLogin.AutoCarousel();
});
var DoLogin = {
   
    //自定义函数
    AutoCarousel: function () {
        $("#carousel_photo").carousel({
            interval: 1000,
            keyboard:false
            });
    }
};
