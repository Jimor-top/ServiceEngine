window.downloadFile = function (fileName, fileData) {
    // 创建一个新的<a>标签
    var link = document.createElement('a');

    // 设置<a>标签的属性
    link.href = 'data:application/octet-stream;base64,' + fileData;
    link.download = fileName;

    // 模拟单击链接以下载文件
    link.click();
}
