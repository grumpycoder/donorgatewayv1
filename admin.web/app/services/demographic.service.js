//template.service.js
//mark.lawrence

(function () {
    'use strict';

    var serviceId = 'demographicService';
    angular.module('app.service').factory(serviceId, serviceController);

    function serviceController(logger, $http, config) {
        logger.log(serviceId + ' loaded');
        var url = config.apiUrl + config.apiEndPoints.Demographic;

        var service = {
            get: get,
            remove: remove,
            removeAll: removeAll
        }

        return service;

        function get() {
            return $http.get(url)
                .then(function (response) {
                    return response.data;
                });
        }

        function remove(id) {
            return $http.delete(url + '/' + id).then(_success).catch(error);
        }

        function removeAll() {
            return $http.delete(url).then(_success).catch(error);
        }

        function _success(response) {
            return response.data;
        };

        function error(error) {
            return error.data.message;
        }
    }
})();