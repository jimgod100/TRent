// timeoutView.js

// 定义计时器
var timeout;

// 启动计时器函数
function startTimer() {
    timeout = setTimeout(showTimeoutConfirmation, 1800000); // 60秒后触发确认框
}

// 显示超时确认框函数
function showTimeoutConfirmation() {
    var confirmed = confirm('停留過久，請重新登入'); // 显示确认框
    if (confirmed) {
        // 确认后的操作
        // 重定向到登录页面
        window.location.href = '/Login/Login'; // 将网页重定向到登录页面的路径
    } else {
        // 如果用户取消了确认框，这里可以添加相应的操作
    }
}

// 清除计时器函数
function clearTimeoutFunction() {
    clearTimeout(timeout); // 清除计时器
}

// 向服务器发出登出请求的函数
function logoutServerRequest() {
    // 使用 AJAX 或其他方法向服务器发送登出请求
    // 这里是一个示例使用 AJAX 的方法
    var xhr = new XMLHttpRequest();
    xhr.open('GET', '/YourController/Logout'); // 更改为你的 Controller 和 Logout 方法的路径
    xhr.onload = function () {
        if (xhr.status === 200) {
            // 登出成功后可以执行其他操作，比如重定向到登录页面
            window.location.href = '/Login/Login'; // 重定向到登录页面
        }
    };
    xhr.send();
}
