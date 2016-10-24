//template.service.js
//mark.lawrence

(function () {
    'use strict';

    var serviceId = 'templateService';
    angular.module('app.service').factory(serviceId, serviceController);

    function serviceController(logger, $http, config) {
        logger.log(serviceId + ' loaded');
        var url = config.apiUrl + config.apiEndPoints.Template;

        var service = {
            get: get,
            update: update,
            query: query
            //            remove: remove,
            //            save: save,
            //            search: search,
        }

        return service;

        function get() {
            return $http.get(url)
                .then(function (response) {
                    return response.data;
                });
        }

        function query(name) {
            return $http.get(url + '/' + name)
                .then(function (response) {
                    return response.data;
                });
        }

        function update(template) {
            return $http.put(url, template)
                .then(function (response) {
                    return response.data;
                });
        }
    }
})();