//mark.lawrence
//editGuest.controller.js

(function () {
    'use strict';

    var controllerId = 'EditGuestController';

    angular.module('app.events').controller(controllerId, mainController);

    mainController.$inject = ['logger', '$uibModalInstance', 'guestService', 'templateService', 'guest', 'event'];

    function mainController(logger, $modal, service, templateService, guest, event) {
        var vm = this;
        vm.title = 'Edit Guest';
        vm.event = event;
        vm.isFull = false;

        vm.guest = angular.copy(guest);

        vm.cancel = function () {
            $modal.dismiss();
        }

        vm.changeAttending = function () {
            if (vm.event.ticketRemainingCount - vm.guest.guestCount < 0) {
                vm.isFull = true;
            } else {
                vm.isFull = false;
            }
        }

        vm.save = function () {
            //Check if first initialization of attending flag compared to original model

            if (vm.guest.isAttending && !guest.isAttending) vm.guest.responseDate = new Date();
            if (vm.isFull) {
                vm.guest.isWaiting = true;
                vm.guest.waitingDate = new Date();
            }
            service.update(vm.guest)
                .then(function (data) {
                    angular.extend(vm.guest, data);
                    $modal.close(vm.guest, vm.event);
                }).finally(function () {
                });
        }
    }


})();