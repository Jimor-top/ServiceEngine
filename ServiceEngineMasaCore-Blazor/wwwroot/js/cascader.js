function addMenuOptionClickHandlers() {
    var menuOptions = document.querySelectorAll('.m-list-item');

    for (var i = 0; i < menuOptions.length; i++) {
        menuOptions[i].addEventListener("click", function () {
            // 处理菜单选项的点击事件逻辑
            // 这里可以写入你希望执行的代码
            console.log("adasdasdadsadad");
        });
    }
}