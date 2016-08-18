//user.service.js
//mark.lawrence

(function () {
    'use strict';

    var serviceId = 'mailerService';
    angular.module('app.service').factory(serviceId, serviceController);

    function serviceController(logger, $http, config) {
        logger.log(serviceId + ' loaded');
        var url = config.apiUrl + config.apiEndPoints.Mailer;

        var service = {
            get: get,
            query: query, 
            campaigns: campaigns
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

        function campaigns() {
            return $http.get(url + '/campaigns').then(_success).catch(error);
        }

        function _success(response) {
            return response.data;
        }

        function error(error) {
            return error.data.message;
        }
    }
})();