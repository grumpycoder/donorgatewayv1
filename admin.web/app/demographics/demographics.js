//mark.lawrence
//demographics.js

(function() {
    'use strict'; 

    var controllerId = 'DemographicsController';

    angular.module('app.demographics').controller(controllerId, UserController);

    UserController.$inject = ['logger'];

    function UserController(logger) {
        var vm = this;
        vm.title = 'Demographic Changes';
        vm.subTitle = 'Demographics';

        activate();

        function activate() {
            logger.log(controllerId + ' activated');
        };

    }

})();