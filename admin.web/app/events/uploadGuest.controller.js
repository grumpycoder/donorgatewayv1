//mark.lawrence
//uploadGuest.controller.js

(function() {
	
    'use strict';

    var controllerId = 'UploadGuestController';

    angular.module('app.events').controller(controllerId, mainController);

    mainController.$inject = ['logger', '$uibModalInstance', 'fileService', 'event'];

    function mainController(logger, $modal, service, event) {
        var vm = this;
        vm.title = 'Edit Guest';

        vm.event = event;

        vm.cancel = function () {
            vm.file = undefined;
            $modal.dismiss();
        }

        vm.fileSelected = function ($file, $event) {
            vm.result = null;
        };
        
        vm.save = function () {
            vm.isBusy = true;
            vm.result = {
                success: false
            }

            service.guest(vm.event.id, vm.file)
                    .then(function (data) {
                        vm.result.success = true;
                        vm.result.message = data;
                    }).catch(function (error) {
                        vm.result.message = error.data.message;
                    }).finally(function () {
                        vm.file = undefined;
                        vm.isBusy = false;
                        $modal.close(vm.result);
                    });
        }
    }

})();