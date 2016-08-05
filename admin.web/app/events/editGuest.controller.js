//mark.lawrence
//editGuest.controller.js

(function () {
    'use strict';

    var controllerId = 'EditGuestController';

    angular.module('app.events').controller(controllerId, mainController);

    mainController.$inject = ['$scope', 'logger', '$uibModalInstance', 'eventService', 'guest'];

    function mainController($scope, logger, $modal, service, guest) {
        var vm = this;
        vm.title = 'Edit Guest';

        if (guest.isAttending) {
            vm.showAddTicket = true;
        }

        vm.guest = angular.copy(guest);

        vm.ticketCountList = [];
        for (var i = 1; i < guest.event.ticketAllowance + 1; i++) {
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

            if (vm.showAddTicket) {
                service.addTicket(vm.guest)
                    .then(function (data) {
                        angular.extend(vm.guest, data);
                        $modal.close(vm.guest);
                    });
            } else {
                service.registerGuest(vm.guest)
                    .then(function(data) {
                        angular.extend(vm.guest, data);
                        $modal.close(vm.guest);
                    });
            }
        }
    }


})();