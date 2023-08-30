document.addEventListener("DOMContentLoaded", async function () {
    const app = document.createElement('div');
    const loader = document.getElementById('blazor-loader-ui')?.outerHTML || `<div id='blazor-loader-ui'>loading</div>`;
    const mode = document.head.querySelector('#blazor-spa-booter')?.dataset.mode || 'Server';
    const iframe = document.createElement('iframe');
    const appName = mode != 'Server' ? 'webAssembly' : 'server';
    const initBlazorSource = (callback) => {
        let script = document.createElement('script');
        script.id = appName;
        script.onload = callback;
        script.setAttribute('autostart', 'false');
        script.src = `_framework/blazor.${appName}.js`;
        document.head.appendChild(script);
    };
    const addRootComponent = (delay, count) => {
        return new Promise((resolve, reject) => {
            if (count < 1) {
                reject();
                return;
            }
            Blazor.rootComponents.add(app, appName, {}).then(() => console.info(`${appName} initialization completed.`))
                .catch(error => {
                    console.info(error);
                    window.setTimeout(() => addRootComponent(delay, count - 1).then(() => resolve()), delay);
                });
            resolve();
        });
    };
    window.showAppOverlay = function () {
        if (!document.body.dataset.loading) {
            document.body.dataset.loading = 'true';
            document.body.insertAdjacentHTML('afterbegin', loader);
        }
    };
    window.hideAppOverlay = function () {
        var handler = window.setTimeout(function () {
            var overlay = document.getElementById('blazor-loader-ui');
            if (overlay) {
                overlay.remove();
                delete document.body.dataset.loading;
            }
            if (self != top && top.hideAppOverlay) top.hideAppOverlay();
            window.clearTimeout(handler);
        }, 300);
    };
    window.startBlazorApp = function () { Blazor.start().then(() => addRootComponent(300, 20)).catch(error => console.info(error)); };
    window.notifyAppIsReady =function () {
        console.info(`${appName} is ready.`);
        if (mode == 'Server' && self != top)
            top.startBlazorApp();//启动主页面的WebAssembly
        else if (mode == 'Automatic' && self == top)
            iframe.contentWindow.Blazor._internal.navigationManager
                .listenForNavigationEvents((uri, state, intercepted) => {
                    window.showAppOverlay();
                    app.style.display = 'block';//显示加载完成的WebAssembly
                    Blazor.navigateTo(uri, false, true);//拦截server的路由，指向切换WebAssembly
                    iframe.style.display = 'none'; //清除并释放server模式iframe
                    iframe.src = 'about:blank';
                    try { iframe.contentWindow.document.write(''); } catch { }
                    iframe.remove();
                    window.hideAppOverlay();
                    return;
                });
    }
    document.body.insertAdjacentElement('afterbegin', app);
    if (mode == 'Automatic') {
        initBlazorSource(() => { console.info("Automatic render start."); });
        iframe.style = 'width:100%;height:100%;position:absolute;z-index:99999;';
        iframe.setAttribute('src', '/_host/server');
        iframe.setAttribute('frameborder', '0');
        iframe.setAttribute('scrolling', 'no');
        iframe.setAttribute('allowfullscreen', 'true');
        iframe.setAttribute('webkitallowfullscreen', 'true');
        iframe.setAttribute('mozallowfullscreen', 'true');
        app.insertAdjacentElement('beforebegin', iframe);
    }
    else initBlazorSource(() => window.startBlazorApp());
});