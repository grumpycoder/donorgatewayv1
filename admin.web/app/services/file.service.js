//mark.lawrence
//file.service.js

(function () {

    'use strict';

    var serviceId = 'fileService';
    angular.module('app.service').factory(serviceId, serviceController);

    function serviceController(logger, $http, config) {
        logger.log(serviceId + ' loaded');
        var url = config.apiUrl + config.apiEndPoints.File;

        var service = {
            guest: guest, 
            mailer: mailer,
            tax: tax
        }

        return service;

        function guest(id, datafile) {
            return $http.post(url + '/guest/' + id, formDataObject(datafile), {
                transformRequest: angular.identity,
                headers: { 'Content-Type': undefined }
            }).then(function (response) {
                return response.data;
            });
        }

        function mailer(id, datafile) {
            return $http.post(url + '/mailer/' + id, formDataObject(datafile), {
                transformRequest: angular.identity,
                headers: { 'Content-Type': undefined }
            }).then(function (response) {
                return response.data;
            });
        }
        
        function tax(datafile) {
            console.log('post tax file');
            return $http.post(url + '/tax', formDataObject(datafile), {
                transformRequest: angular.identity,
                headers: { 'Content-Type': undefined }
            }).then(function (response) {
                return response.data;
            }).catch(function(err) {
                console.log('err', err);
            });
        }

        function formDataObject(data) {
            var fd = new FormData();
            fd.append('file', data);
            return fd;
        }
    }
})();