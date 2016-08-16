//mark.lawrence
//donortax.js

(function() {
    'use strict';

    var controllerId = 'DonorTaxController';

    angular.module('app.donortax').controller(controllerId, mainController);

    mainController.$inject = ['logger', '$uibModal' ];

    function mainController(logger, $modal) {
        var vm = this;
        vm.title = 'Donor Tax Manager';
    }

})();