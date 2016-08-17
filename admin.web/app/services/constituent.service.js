//user.service.js
//mark.lawrence

(function () {
    'use strict';

    var serviceId = 'constituentService';
    angular.module('app.service').factory(serviceId, serviceController);

    function serviceController(logger, $http, config) {
        logger.log(serviceId + ' loaded');
        var url = config.apiUrl + config.apiEndPoints.Constituent;

        var service = {
            get: get,
            query: query, 
            save: save
        }

        return service;


        function get() {
            return $http.get(url).then(_success).catch(error);
        }

        function query(vm) {
            return $http.post(url + '/search', vm)
              .then(function (response) {
                  return response.data;
              });
        }

        function save(person) {
            return $http.put(url, person)
              .then(function (response) {
                  return response.data;
              });
        }

        function _success(response) {
            return response.data;
        }

        function error(error) {
            return error.data.message;
        }
    }
})();