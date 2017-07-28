kvite.services.contacts = kvite.services.contacts || {};

kvite.services.contacts.insert = function (data, onSuccess, onError) {
    var url = "/api/contacts/create";

    var settings = {
        cache: false
        , contentType: "application/x-www-form-urlencoded; charset=UTF-8"
        , data: data
        , dataType: "json"
        , success: onSuccess
        , error: onError
        , type: "POST"
    };
    $.ajax(url, settings);
}

kvite.services.contacts.update = function (id, data, onSuccess, onError) {

    var url = "/api/contacts/" + id;
    var settings = {
        cache: false
    , contentType: "application/x-www-form-urlencoded; charset=UTF-8"
    , data: data
    , dataType: "json"
    , success: onSuccess
    , error: onError
    , type: "PUT"
    };
    $.ajax(url, settings);
}

kvite.services.contacts.get = function (onSuccess, onError) {

    var url = "/api/contacts";
    var settings = {
        cache: false
    , contentType: "application/x-www-form-urlencoded; charset=UTF-8"
    , dataType: "json"
    , success: onSuccess
    , error: onError
    , type: "GET"
    };
    $.ajax(url, settings);
}

kvite.services.contacts.getById = function (id, onSuccess, onError) {

    var url = "/api/contacts/" + id;
    var settings = {
        cache: false
    , contentType: "application/x-www-form-urlencoded; charset=UTF-8"
    , dataType: "json"
    , success: onSuccess
    , error: onError
    , type: "GET"
    };
    $.ajax(url, settings);
}

kvite.services.contacts.delete = function (id, onSuccess, onError) {

    var url = "/api/contacts/" + id;
    var settings = {
        cache: false
    , contentType: "application/x-www-form-urlencoded; charset=UTF-8"
    , dataType: "json"
    , success: onSuccess
    , error: onError
    , type: "DELETE"
    };
    $.ajax(url, settings);
};

kvite.services.contacts.getAll = function (onSuccess, onError) {
    var url = "/googlecontacts/index";
    var settings = {
        cache: false
    , contentType: "application/x-www-form-urlencoded; charset=UTF-8"
    , dataType: "json"
    , success: onSuccess
    , error: onError
    , type: "GET"
    };
    $.ajax(url, settings);
};

kvite.services.contacts.importToDatabase = function (contacts, onSuccess, onError) {
    var url = "/api/contacts/google";
    var settings = {
        cache: false
    , contentType: "application/x-www-form-urlencoded; charset-UTF-8"
    , dataType: "json"
    , data: contacts
    , success: onSuccess
    , error: onError
    , type: "POST"
    };
    $.ajax(url, settings);
};

kvite.services.contacts.getContactsByEventOwnerUserId = function (userId, onSuccess, onError) {
    var url = "/api/contacts?userId=" + userId;
    var settings = {
        cache: false
    , contentType: "application/x-www-form-urlencoded; charset=UTF-8"
    , dataType: "json"
    , success: onSuccess
    , error: onError
    , type: "GET"
    };
    $.ajax(url, settings);
}

; (function () {
    if (angular) {
        angular.module(APPNAME).factory("contactsService", GoogleContacts);

        GoogleContacts.$inject = ["$baseService", "$kvite"];
        function GoogleContacts($baseService, $kvite) {
            var serviceObject = kvite.services.contacts;
            var service = $baseService.merge(true, {}, serviceObject, $baseService);
            return service;
        }
    }
})();