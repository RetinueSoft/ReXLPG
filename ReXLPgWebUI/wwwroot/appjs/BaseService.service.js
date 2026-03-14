(function () {
    'use strict';

    angular.module('app').service('server', server);

    function server() {
        this.Get = Get;
        this.Post = Post;
        this.PostDownload = PostDownload;
        this.UploadFile = UploadFile;
        this.GetDownload = GetDownload;

        function Get(url, sucessFunc) {
            $('.loader_bg').fadeIn();
            
            $.ajax({
                async: false,
                type: "GET",
                contentType: "application/json",
                url: apiBaseUrl + url,
                headers: {
                    "Authorization": "Bearer " + GetValueFromCache("logintoken")
                },
                success: function (response) {
                    setTimeout(function () {
                        $('.loader_bg').fadeOut();
                    }, 10);
                    if (response.success) {
                        sucessFunc(response.sucessValue);
                    }
                    else {
                        const str = response.errorMessage.map(e => {
                            if (e.field && e.fieldValue) {
                                return `${e.field}: ${e.message} (wrong value: ${e.fieldValue})`;
                            } else if (!e.field && e.fieldValue) {
                                return `${e.message} (wrong value: ${e.fieldValue})`;
                            } else if (e.field && !e.fieldValue) {
                                return `${e.field}: ${e.message}`;
                            } else {
                                return `${e.message}`;
                            }
                        }).join('\n');
                        alert(str);
                    }
                },
                error: function (response, exception) {
                    setTimeout(function () {
                        $('.loader_bg').fadeOut();
                    }, 10);
                    var msg = '';
                    if (response.status === 0) {
                        msg = 'Not connect.\n Verify Network.';
                    } else if (response.status == 401) {
                        msg = 'Not authorized request, Please login.';
                        //window.location.href = "/Home/Index";
                    } else if (response.status == 404) {
                        msg = 'Requested page not found. [404]';
                    } else if (response.status == 500) {
                        msg = 'Internal Server Error [500].';
                    } else {
                        msg = 'Uncaught Error.\n' + response.responseText;
                    }
                    alert(msg);
                }
            });
        }
        function Post(url, data, sucessFunc, failureFunc) {
            $('.loader_bg').fadeIn();
            console.log(JSON.stringify(data));
            $.ajax({
                async: false,
                type: "POST",
                contentType: "application/json",
                url: apiBaseUrl + url,
                data: JSON.stringify(data),
                headers: {
                    "Authorization": "Bearer " + GetValueFromCache("logintoken")
                },
                success: function (response) {
                    setTimeout(function () {
                        $('.loader_bg').fadeOut();
                    }, 10);

                    if (response.success) {
                        sucessFunc(response.sucessValue);
                    }
                    else {
                        if (failureFunc != null) {
                            failureFunc(response.errorMessage);
                        }
                        else {   
                            const str = response.errorMessage.map(e => {
                                if (e.field && e.fieldValue) {
                                    return `${e.field}: ${e.message} (wrong value: ${e.fieldValue})`;
                                } else if (!e.field && e.fieldValue) {
                                    return `${e.message} (wrong value: ${e.fieldValue})`;
                                } else if (e.field && !e.fieldValue) {
                                    return `${e.field}: ${e.message}`;
                                } else {
                                    return `${e.message}`;
                                }
                            }).join('\n');
                            alert(str);
                        }
                    }
                },
                error: function (response, exception) {
                    console.log(response);
                    console.log(exception);
                    setTimeout(function () {
                        $('.loader_bg').fadeOut();
                    }, 10);

                    var msg = '';
                    if (response.status === 0) {
                        msg = 'Not connect.\n Verify Network.';
                    } else if (response.status == 401) {
                        msg = 'Not authorized Logging out.';
                        if (window.location.pathname !== "/Home/Login")
                            window.location.href = "/Home/Login";
                    } else if (response.status == 404) {
                        msg = 'Requested page not found. [404]';
                    } else if (response.status == 500) {
                        msg = 'Internal Server Error [500].';
                    } else {
                        msg = 'Uncaught Error.\n' + response.responseText;
                    }
                    console.log(msg);
                    console.log(exception);
                    alert(msg);
                }
            });
        }
        function PostDownload(url, data, fileName) {
            $.ajax({
                'async': false,
                'type': "POST",
                'global': false,
                'url': url,
                'data': data,
                'success': function (response) {
                    var blob = new Blob([response], { type: 'application/csv' });
                    var a = document.createElement('a');
                    a.style.display = 'none';
                    var url = window.URL.createObjectURL(blob);
                    a.href = url;
                    a.download = fileName;
                    document.body.appendChild(a);
                    a.click();
                    document.body.removeChild(a);
                    window.URL.revokeObjectURL(url);
                },
                'error': function (response, exception) {
                    setTimeout(function () {
                        //console.log("Hide");
                        //$('#loader').modal('hide');
                    }, 1000);
                    var msg = '';
                    if (response.status === 0) {
                        msg = 'Not connect.\n Verify Network.';
                    } else if (response.status == 401) {
                        msg = 'Not authorized Logging out.';
                        window.location.href = "/Home/Login";
                    } else if (response.status == 404) {
                        msg = 'Requested page not found. [404]';
                    } else if (response.status == 500) {
                        msg = 'Internal Server Error [500].';
                    } else {
                        msg = 'Uncaught Error.\n' + response.responseText;
                    }
                    alert(msg);
                }
            });
        }
        function GetDownload(url, fileName) {
            $.ajax({
                url: url, // Your API endpoint
                method: 'GET',
                xhrFields: {
                    responseType: 'blob' // Important!
                },
                success: function (data, status, xhr) {
                    const blob = new Blob([data], {
                        type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet'
                    });

                    const link = document.createElement('a');
                    link.href = window.URL.createObjectURL(blob);
                    link.download = fileName;
                    document.body.appendChild(link);
                    link.click();
                    document.body.removeChild(link);
                },
                error: function (response, exception) {
                    setTimeout(function () {
                        //console.log("Hide");
                        //$('#loader').modal('hide');
                    }, 1000);
                    var msg = '';
                    if (response.status === 0) {
                        msg = 'Not connect.\n Verify Network.';
                    } else if (response.status == 401) {
                        msg = 'Not authorized Logging out.';
                        window.location.href = "/Home/Login";
                    } else if (response.status == 404) {
                        msg = 'Requested page not found. [404]';
                    } else if (response.status == 500) {
                        msg = 'Internal Server Error [500].';
                    } else {
                        msg = 'Uncaught Error.\n' + response.responseText;
                    }
                    alert(msg);
                }
            });
        }
        function UploadFile(url, data, fileName, sucessFunc, failureFunc) {
            $('.loader_bg').fadeIn();
            $.ajax({
                'async': false,
                'type': "POST",
                'global': false,
                'url': url,
                'contentType': false,
                'processData': false,
                'data': data,
                'success': function (response) {
                    setTimeout(function () {
                        $('.loader_bg').fadeOut();
                    }, 10);
                    if (response.success) {
                        sucessFunc(response.sucessValue);
                    }
                    else if (response.errorMessage) {
                        if (failureFunc != null) {
                            failureFunc(response.errorMessage);
                        }
                        else {
                            var str = "";
                            $.when(
                                $.each(response.errorMessage, function (key, value) {
                                    str += key + ": " + value + "\n\n";
                                })
                            ).then(function () { alert(str); });
                        }
                    }
                    else {
                        fileName += "_error.csv";
                        var blob = new Blob([response], { type: 'application/csv' });
                        var a = document.createElement('a');
                        a.style.display = 'none';
                        var url = window.URL.createObjectURL(blob);
                        a.href = url;
                        a.download = fileName;
                        document.body.appendChild(a);
                        a.click();
                        document.body.removeChild(a);
                        window.URL.revokeObjectURL(url);
                        alert("Error file exported " + fileName);

                        if (failureFunc != null) {
                            failureFunc(response.errorMessage);
                        }
                    }
                },
                'error': function (response, exception) {
                    setTimeout(function () {
                        $('.loader_bg').fadeOut();
                    }, 10);

                    var msg = '';
                    if (response.status === 0) {
                        msg = 'Not connect.\n Verify Network.';
                    } else if (response.status == 401) {
                        msg = 'Not authorized Logging out.';
                        window.location.href = "/Home/Login";
                    } else if (response.status == 404) {
                        msg = 'Requested page not found. [404]';
                    } else if (response.status == 500) {
                        msg = 'Internal Server Error [500].';
                    } else {
                        msg = 'Uncaught Error.\n' + response.responseText;
                    }
                    alert(msg);
                }
            });
        }
    }
})();