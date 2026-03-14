var myModule = angular.module('app');

myModule.controller('UserProfileAccountController', ['$scope', 'UserAccountService', UserProfileAccountController]);

function UserProfileAccountController(binder, service) {
    emptyDocObj = { userImagePreview:'', userImage:'', aadharFrontPreview: '', aadharFront: '', aadharBackPreview: '', aadharBack: '', workIdPreview: '', workId: '' };
    serverDocImages = [];
    binder.user = { };
    binder.document = JSON.parse(JSON.stringify(emptyDocObj));
    binder.tab = { profileTab: true, passwordTab: false, documentTab: false };

    binder.triggerPageLoaded = triggerPageLoaded;
    binder.triggerUpload = triggerUpload;
    binder.toggleProfileTab = toggleProfileTab;
    binder.togglePasswordTab = togglePasswordTab;
    binder.toggleDocumentTab = toggleDocumentTab;
    binder.triggerSave = triggerSave;
    binder.triggerCancel = triggerCancel;

    function triggerPageLoaded() {
        initializeDatepickers();
        getUserDetail();
        getUserImage();
    }
    function getUserImage() {
        service.getUserImage(function (result) {
            binder.document.userImagePreview = 'data:image/jpeg;base64,' + result;
        });
    }
    function getUserDetail() {
        service.getMyDetail(function (result) {
            binder.user = result;
            binder.user.password = "";
            binder.user.userType = 'Student';
            setToDatePicker("dob", result.dateOfBirth);
        });
    }
    function getDocumentImages() {
        service.getUserDocumentImages(function (result) {
            serverDocImages = result;
            clearImage();
        });
    }
    function triggerUpload (controlId) {
        document.getElementById(controlId).click();
    };
    function toggleProfileTab() {
        binder.tab.profileTab = true; binder.tab.passwordTab = false; binder.tab.documentTab = false;
    };
    function togglePasswordTab() {
        binder.tab.profileTab = false; binder.tab.passwordTab = true; binder.tab.documentTab = false;
    };
    function toggleDocumentTab() {
        binder.tab.profileTab = false; binder.tab.passwordTab = false; binder.tab.documentTab = true;
        getDocumentImages();
    };
    function triggerSave() {
        var validate = { status: true };

        if (binder.tab.profileTab) {
            ValidateEmptyAndSetError(binder.user, "name", "userNameGroupProfile", validate);
            ValidateEmptyAndSetError(binder.user, "mobile", "userMobileGroupProfile", validate);
            ValidateEmptyAndSetError(binder.user, "aadharNumber", "userAadharGroupProfile", validate);
            ValidateEmptyAndSetError(binder.user, "dob", "userDOBGroupProfile", validate);
            ValidateEmptyAndSetError(binder.user, "address", "userAddressGroupProfile", validate);
            ValidateEmptyAndSetError(binder.user, "emergencyContact1Name", "userEmConName1GroupProfile", validate);
            ValidateEmptyAndSetError(binder.user, "emergencyContact1Mobile", "userEmConMobile1GroupProfile", validate);
            ValidateEmptyAndSetError(binder.user, "emergencyContact2Name", "userEmConName2GroupProfile", validate);
            ValidateEmptyAndSetError(binder.user, "emergencyContact2Mobile", "userEmConMobile2GroupProfile", validate);
            ValidateEmptyAndSetError(binder.user, "purposeOfStay", "purposeOfStayGroupProfile", validate);
            ValidateEmptyAndSetError(binder.user, "workPlaceName", "workingTypeGroupProfile", validate);
            ValidateEmptyAndSetError(binder.user, "workingPlaceId", "workingIdGroupProfile", validate);

        }
        else if (binder.tab.passwordTab) {
            ValidateEmptyAndSetError(binder.user, "oldpassword", "userOldPasswordGroupLogin", validate);
            ValidateEmptyAndSetError(binder.user, "password", "userPasswordGroupLogin", validate);
            ValidateEmptyAndSetError(binder.user, "cpassword", "userConfirmPasswordGroupLogin", validate);
            if (binder.user.password !== binder.user.cpassword) {
                SetControlError("userConfirmPasswordGroupLogin", "Confirm password incorrect"); validate = false;
            }
        }
        else if (binder.tab.documentTab) {
            binder.user.aadharFrontImage = binder.document.aadharFront;
            binder.user.aadharBackImage = binder.document.aadharBack;
            binder.user.WorkIdFrontImage = binder.document.workId;
            binder.user.userImage = binder.document.userImage;
        }
        
        
        if (!validate.status) {
            return;
        }

        if (binder.tab.profileTab) {
            binder.user.dateOfBirth = (new Date(binder.user.dob)).toISOString();
            service.editUser(binder.user, function () {
                alert("Stored sucessfully!");
                toggleProfileTab();
            });
        }
        else if (binder.tab.passwordTab) { }
        else if (binder.tab.documentTab) {
            var documents = JSON.parse(JSON.stringify(serverDocImages));
            documents.userId = binder.user.guid;
            documents.userImage = binder.document.userImagePreview.split(',')[1];
            documents.aadharFrontImage = binder.document.aadharFrontPreview.split(',')[1];
            documents.aadharBackImage = binder.document.aadharBackPreview.split(',')[1];
            documents.workIdFrontImage = binder.document.workIdPreview.split(',')[1];
            console.log(documents);
            service.uploadDocuments(documents, function () {
                alert("Stored sucessfully!");
                toggleProfileTab();
            });
        }
                
    };
    function triggerCancel() {
        toggleProfileTab();
        clearImage();
    };
    function clearImage() {
        binder.document.aadharFrontPreview = 'data:image/jpeg;base64,' + serverDocImages["aadharFrontImage"];
        binder.document.aadharBackPreview = 'data:image/jpeg;base64,' + serverDocImages["aadharBackImage"];
        binder.document.workIdPreview = 'data:image/jpeg;base64,' + serverDocImages["workIdFrontImage"];
    };

    function ValidateEmptyAndSetError(obj, prop, controlName, validate) {
        RemoveControlError(controlName);
        if (isNullOrEmpty(obj, prop)) {
            SetControlError(controlName, prop + " shouldn't be empty"); validate.status = false;
        }
    }
}

myModule.directive('fileModel', ['$parse', function ($parse) {
    return {
        restrict: 'A',
        scope: {
            fileModel: '=',
            previewModel: '='
        },
        link: function (scope, element) {
            element.bind('change', function () {
                var file = element[0].files[0];
                scope.fileModel = file;

                if (file && file.type.startsWith("image/")) {
                    var reader = new FileReader();
                    reader.onload = function (e) {
                        scope.$apply(function () {
                            scope.previewModel = e.target.result;
                        });
                    };
                    reader.readAsDataURL(file);
                } else {
                    scope.$apply();
                }
            });
        }
    };
}]);


