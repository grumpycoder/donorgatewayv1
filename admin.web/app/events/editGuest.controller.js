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
        vm.ticketCountList = [];

        for (var i = 1; i < guest.event.ticketAllowance + 1; i++) {
            vm.ticketCountList.push(i);
        }

        vm.cancel = function () {
            $modal.dismiss();
        }

        vm.changeAttending = function () {
            if (vm.guest.isAttending) {
                vm.guest.ticketCount = 1;
            } else {
                vm.guest.ticketCount = null;
            }
        }

        vm.save = function () {
            service.registerGuest(vm.guest).then(function(data) {
                angular.extend(vm.guest, data);
                $modal.close(vm.guest);
            });
        }
    }


})();