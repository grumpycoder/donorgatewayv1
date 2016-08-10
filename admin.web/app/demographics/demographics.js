//mark.lawrence
//demographics.js

(function() {
    'use strict'; 

    var controllerId = 'DemographicsController';

    angular.module('app.demographics').controller(controllerId, UserController);

    UserController.$inject = ['logger', 'demographicService'];

    function UserController(logger, service) {
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
            service.get()
                .then(function(data) {
                    vm.demographics = data;
                });
        }

        vm.delete = function(demo) {
            vm.isBusy = true;
            service.remove(demo.id)
                .then(function (data) {
                    logger.success('Deleted ' + demo.name);
                    var idx = vm.demographics.indexOf(demo);
                    vm.demographics.splice(idx, 1);
                }).finally(function() {
                    vm.isBusy = false; 
                });
        }

        vm.deleteAll = function () {
            vm.isBusy = true;
            service.removeAll()
                .then(function (data) {
                    logger.success(data);
                    vm.demographics = []; 
                }).finally(function () {
                    vm.isBusy = false;
                });

        }
    }

})();