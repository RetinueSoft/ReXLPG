var myModule = angular.module('app');

myModule.controller('UserLoginAccountController', ['$scope', 'UserAccountService', UserLoginAccountController]);

function UserLoginAccountController(binder, service) {
    binder.user = { name: '', mobile: '', password: '', cPassword: '', aadharNumber: '', subscription: 1, termAndCondition: false, userType:'Student' };
    binder.login = login;
    binder.signup = signup;
    binder.retrivePassword = retrivePassword;

    $("#password").on("keydown", function (event) {
        if (event.keyCode === 13) {
            event.preventDefault();
            $("#userLoginBtn").click();
        }
    });
    function modalClose() {
        $('.modal').modal('hide');
    }

    function login() {
        var validationStatus = true;

        RemoveControlError("userMobileGroupLogin");
        RemoveControlError("userPasswordGroupLogin");
        if (isNullOrEmpty(binder.user, 'mobile')) {
            SetControlError("userMobileGroupLogin", "Mobile shouldn't be empty"); validationStatus = false;
        }
        if (isNullOrEmpty(binder.user, 'password')) {
            SetControlError("userPasswordGroupLogin", "Password shouldn't be empty"); validationStatus = false;
        }

        if (!validationStatus) {
            return;
        }

        var returnURL = getUrlQueryString("ReturnUrl");
        service.login(binder.user, function (result) {
            console.log(result);
            AddIntoCache("logintoken", result);
            fetch("/Home/StoreToken", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(result)
            }).then(response => {
                if (response.ok) {
                    if (returnURL && returnURL !== "") {
                        window.location.href = returnURL;
                    } else {
                        window.location.href = "/Dashboard";
                    }
                } else {
                    alert("Token store failed.");
                }
            });
        });
    }
    function signup() {
        var validationStatus = true;
        RemoveControlError("userNameGroupLogin");
        RemoveControlError("userMobileGroupLogin");
        RemoveControlError("userPasswordGroupLogin");
        RemoveControlError("userConfirmPasswordGroupLogin");

        if (isNullOrEmpty(binder.user, 'name')) {
            SetControlError("userNameGroupLogin", "Name shouldn't be empty"); validationStatus = false;
        }
        if (isNullOrEmpty(binder.user, 'mobile')) {
            SetControlError("userMobileGroupLogin", "Mobile shouldn't be empty"); validationStatus = false;
        }
        if (isNullOrEmpty(binder.user, 'password')) {
            SetControlError("userPasswordGroupLogin", "Password shouldn't be empty"); validationStatus = false;
        }
        if (isNullOrEmpty(binder.user, 'cpassword')) {
            SetControlError("userConfirmPasswordGroupLogin", "Password shouldn't be empty"); validationStatus = false;
        }
        if (binder.user.password !== binder.user.cpassword) {
            SetControlError("userConfirmPasswordGroupLogin", "Confirm password incorrect"); validationStatus = false;
        }
        if (!validationStatus) {
            return;
        }

        service.createUser(binder.user, function () {
            alert("Account created sucessfully!");
            $("#loginBtn")[0].click();
        });
    }
    function retrivePassword() {
        var validationStatus = true;

        RemoveControlError("userMobileGroupLogin");
        RemoveControlError("userAadharGroupLogin");

        if (isNullOrEmpty(binder.user, 'mobile')) {
            SetControlError("userMobileGroupLogin", "Mobile shouldn't be empty"); validationStatus = false;
        }
        if (isNullOrEmpty(binder.user, 'aadharNumber')) {
            SetControlError("userAadharGroupLogin", "Aadhar shouldn't be empty"); validationStatus = false;
        }
        if (!validationStatus) {
            return;
        }

        service.resetPassword(user, function (result) {
            modalClose();
        }, function (result) {
            $.each(result, function (key, value) {
                value = value.replace(/\n/g, "<br />");
                SetControlError("userMobileGroupReset", value);
            });
        });
    }
}
