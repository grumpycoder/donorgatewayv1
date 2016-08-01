//mark.lawrence
//event.service.js

(function () {
    'use strict';

    var serviceId = 'eventService';
    angular.module('app.service').factory(serviceId, serviceController);

    function serviceController(logger, $http, config) {
        logger.log(serviceId + ' loaded');
        var url = config.apiUrl + config.apiEndPoints.Event;

        var service = {
            create: create,
            downloadGuests: downloadGuests, 
            remove: remove,
            get: get,
            getById: getById,
            getGuests: getGuests, 
            update: update,
            query: query
        }

        return service;

        function create(event) {
            return $http.post(url, event)
                .then(_success).catch(error);
        }

        function downloadGuests(id, vm) {
            return $http.post(url + '/' + id + '/guests/export', vm)
                .then(function(data) {
                    return data;
                }).catch(error);
        }


        function remove(id) {
            return $http.delete(url + '/' + id).then(_success).catch(error);
        };

        function get() {
            return $http.get(url)
                .then(_success).catch(error);
        }

        function getById(id) {
            return $http.get(url + '/' + id)
                .then(_success).catch(error);
        }

        function getGuests(id, vm) {
            return $http.post(url + '/' + id + '/guests', vm)
                .then(_success).catch(error);
        }

        function query(name) {
            return $http.get(url + '/' + name)
                .then(_success).catch(error);
        }

        function update(event) {
            return $http.put(url, event)
                .then(_success).catch(error);
        }

        function _success(response) {
            return response.data;
        };

        function error(error) {
            return error.data.message; 
        }
    }
})();