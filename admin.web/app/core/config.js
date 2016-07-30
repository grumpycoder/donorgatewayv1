﻿//config.js
//mark.lawrence
(function () {
    function toastrConfig(toastr) {
        toastr.options.timeOut = 4000;
        toastr.options.positionClass = 'toast-bottom-right';
    };

    var keyCodes = {
        backspace: 8,
        tab: 9,
        enter: 13,
        esc: 27,
        space: 32,
        pageup: 33,
        pagedown: 34,
        end: 35,
        home: 36,
        left: 37,
        up: 38,
        right: 39,
        down: 40,
        insert: 45,
        del: 46
    };

    var apiEndPoints = {
        User: 'users',
        Constituent: 'constituent',
        Tax: 'tax',
        Template: 'template',
        Campaign: 'campaign',
        Mailer: 'mailer',
        Reason: 'reason',
        Event: 'event',
        Guest: 'guest',
        File: 'file'
    };

    var config = {
        appErrorPrefix: '[DG Error] ', //Configure the exceptionHandler decorator
        appTitle: 'DonorGateway',
        version: '1.0.0',
        apiUrl: 'http://' + window.location.host + '/api/',
        keyCodes: keyCodes,
        apiEndPoints: apiEndPoints
    };

    var defaults = {
        EMAIL_SUFFIX: '@splcenter.org',
        GENERIC_PASSWORD: '1P@ssword'
    };

    angular.module('app.core')
        .config(toastrConfig)
        .constant('config', config)
        .value('defaults', defaults);
})();