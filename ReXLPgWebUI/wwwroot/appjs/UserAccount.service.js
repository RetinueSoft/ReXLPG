(function () {
    'use strict';

    angular.module('app').service('UserAccountService', UserAccountService);
    UserAccountService.$inject = ['server'];

    function UserAccountService(server) {   
        this.createUser = createUser;
        this.login = login;
        this.resetPassword = resetPassword;
        this.logout = logout;
        this.getUserImage = getUserImage;
        this.getUserDetail = getUserDetail;
        this.editUser = editUser;
        this.getUserDocumentImages = getUserDocumentImages;
        this.getMyDetail = getMyDetail;
        this.uploadDocuments = uploadDocuments;

        function createUser(user, sucessFunc, failureFunc) {
            server.Post('/User/Create', user, sucessFunc, failureFunc);
        }
        function login(user, sucessFunc, failureFunc) {
            server.Post('/Auth/Login', { mobile: user.mobile, password: user.password, clientTimeOffset: (new Date()).getTimezoneOffset() }, sucessFunc, failureFunc);
        }
        function resetPassword(user, sucessFunc, failureFunc) {
            server.Post('/UserAccount/ResetPassword', { user: user }, sucessFunc, failureFunc);
        }
        function logout(sucessFunc) {
            server.Get('/Auth/Logout', sucessFunc);
        }
        function getUserImage(sucessFunc) {
            server.Get('/User/LoginUserImage', sucessFunc);
        }
        function getUserDocumentImages(sucessFunc) {
            server.Get('/User/MyDocumentImages', sucessFunc);
        }
        function getMyDetail(sucessFunc) {
            server.Get('/User/MyDetail', sucessFunc);
        }
        function getUserDetail(sucessFunc) {
            server.Get('/User/UserDetail', sucessFunc);
        }
        function editUser(user, sucessFunc) {
            server.Post('/User/Edit', user, sucessFunc);
        }
        function uploadDocuments(documents, sucessFunc) {
            server.Post('/User/UploadDocument', documents, sucessFunc);
        }
    }
})();