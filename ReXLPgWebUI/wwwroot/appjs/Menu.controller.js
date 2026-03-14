var myModule = angular.module('app');

myModule.controller('MenuController', ['$scope', '$window', 'UserAccountService', MenuController]);

function MenuController(binder, window, service) {
    binder.loginClicked = function () { window.location.href = '/Home/Login'; }

    binder.logoutClicked = function () {
        RemoveFromCache("logintoken");
        fetch("/Home/RemoveToken", {
            method: "GET",
            headers: { "Content-Type": "application/json" },
        }).then(response => {
            if (response.ok) {
                window.location.href = "/Home";
            } else {
                alert("Token remove failed.");
            }
        });
    }

    binder.getUserImage = function () {
        service.getUserImage(function (result) {
            $('#profileImg').attr('src', 'data:image/jpeg;base64,' + result);
        });
    }
}
