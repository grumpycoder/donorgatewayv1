//mark.lawrence
//guest.service.js

(function () {
    'use strict';

    var serviceId = 'guestService';
    angular.module('app.service').factory(serviceId, serviceController);

    function serviceController(logger, $http, config) {
        logger.log(serviceId + ' loaded');
        var url = config.apiUrl + config.apiEndPoints.Guest;

        var service = {
            create: create,
            remove: remove,
            get: get,
            getById: getById,
            update: update,
            query: query
        }

        return service;

        function create(event) {
            return $http.post(url, event)
                .then(function (response) {
                    return response.data;
                });
        }

        function remove(id) {
            return $http.delete(url + '/' + id).then(_success);
        };

        function get() {
            return $http.get(url)
                .then(function (response) {
                    return response.data;
                });
        }

        function getById(id) {
            return $http.get(url + '/' + id)
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

        function update(event) {
            return $http.put(url, event)
                .then(function (response) {
                    return response.data;
                });
        }

        function _success(response) {
            return response.data;
        };
    }
})();