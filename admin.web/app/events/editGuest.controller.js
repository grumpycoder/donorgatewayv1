//mark.lawrence
//editGuest.controller.js

(function () {
    'use strict';

    var controllerId = 'EditGuestController';

    angular.module('app.events').controller(controllerId, mainController);

    mainController.$inject = ['logger', '$uibModalInstance', 'eventService', 'guest'];

    function mainController(logger, $modal, service, guest) {
        var vm = this;
        vm.title = 'Edit Guest';

        vm.guest = angular.copy(guest);

        vm.cancel = function () {
            $modal.dismiss();
        }

        vm.save = function () {
            service.registerGuest(vm.guest).then(function(data) {
                angular.extend(vm.guest, data);
                $modal.close(vm.guest);
            });
        }
    }


})();