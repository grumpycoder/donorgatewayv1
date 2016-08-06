//mark.lawrence
//editGuest.controller.js

(function () {
    'use strict';

    var controllerId = 'CreateGuestController';

    angular.module('app.events').controller(controllerId, mainController);

    mainController.$inject = ['$scope', 'logger', '$uibModalInstance', 'eventService', 'event'];

    function mainController($scope, logger, $modal, service, event) {
        var vm = this;
        vm.title = 'Create Guest';
       
        vm.guest = {
            finderNumber: '00000000000', 
            eventId: event.id
        };

        vm.ticketCountList = [];
        for (var i = 1; i < event.ticketAllowance + 1; i++) {
            vm.ticketCountList.push(i);
        }

        vm.decrementTickets = function () {
            $scope.editGuestForm.$pristine = false;
            vm.guest.additionalTickets--;
        }

        vm.incrementTickets = function (e) {
            $scope.editGuestForm.$pristine = false;
            vm.guest.additionalTickets++;
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
            service.registerGuest(vm.guest)
                            .then(function (data) {
                                angular.extend(vm.guest, data);
                                $modal.close(vm.guest);
                            });
        }
    }


})();