//mark.lawrence
//demographics.js

(function() {
    'use strict'; 

    var controllerId = 'DemographicsController';

    angular.module('app.demographics').controller(controllerId, UserController);

    UserController.$inject = ['logger', '$http'];

    function UserController(logger, $http) {
        var vm = this;
        vm.title = 'Demographic Updates';
        vm.description = 'Updates made to constituent data'; 
        vm.subTitle = 'Demographics';


        vm.demographics = [];

        activate();

        function activate() {
            logger.log(controllerId + ' activated');
            getDemographics();
        };


        function getDemographics() {
            $http.get('api/demographicchange')
                .then(function (response) {
                    logger.log('data', response.data);
                    vm.demographics = response.data;
                });
        }
    }

})();