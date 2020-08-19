//
// CONFIG IZIToast
//
iziToast.settings({
    timeout: 3000,
    // position: 'center',
    // imageWidth: 50,
    pauseOnHover: true,
    // resetOnHover: true,
    close: true,
    progressBar: true,
    // layout: 1,
    // balloon: true,
    // target: '.target',
    // icon: 'material-icons',
    // iconText: 'face',
    // animateInside: false,
    // transitionIn: 'flipInX',
    // transitionOut: 'flipOutX',
});
//操作成功弹窗
function OperationSuccess(context) {
    iziToast.success({
        title: '成功',
        message: context,
        position: 'bottomRight',
        transitionIn: 'bounceInLeft',
        // iconText: 'star',
        onOpen: function () {
            console.log('提示框成功弹出！');
        },
        onClose: function () {
            console.log("提示框关闭！");
            //document.getElementById("add-pj-form").reset();
        }
    });
};
//操作失败弹窗
function OperationFailed(context) {
    iziToast.error({
        title: '失败',
        message: context,
        position: 'topRight',
        transitionIn: 'fadeInDown',
        // iconText: 'star',
        onOpen: function () {
            console.log('提示框成功弹出！');
        },
        onClose: function () {
            console.log("提示框关闭！");
            //document.getElementById("add-pj-form").reset();
        }
    });
};