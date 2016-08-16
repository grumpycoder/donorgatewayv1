//mark.lawrence
//donortax.js

(function() {
    'use strict';

    var controllerId = 'DonorTaxController';

    angular.module('app.donortax').controller(controllerId, mainController);

    mainController.$inject = ['logger', '$uibModal', 'constituentService'];

    function mainController(logger, $modal, service) {
        var vm = this;
        vm.title = 'Donor Tax Manager';

        vm.people = [];

        activate();

        function activate() {
            logger.log(controllerId + ' activated');

            service.get()
                .then(function (data) {
                    vm.people = data; 
                    logger.log('data', data);
                });

        }

    }

})();