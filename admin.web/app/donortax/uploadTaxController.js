//uploadTaxController.js
(function () {

    'use strict';

    var controllerId = 'UploadTaxController';

    angular.module('app.donortax').controller(controllerId, mainController);

    mainController.$inject = ['logger', '$uibModalInstance', 'fileService'];

    function mainController(logger, $modal, service) {
        var vm = this;
        vm.title = 'Tax Upload';

        vm.cancel = function () {
            vm.file = undefined;
            $modal.dismiss();
        }

        vm.fileSelected = function ($file, $event) {
            vm.result = null;
        };

        vm.save = function () {
            console.log('save');
            vm.isBusy = true;
            vm.result = {
                success: false
            }

            service.tax(vm.file)
                .then(function (data) {
                    console.log('data', data);
                    vm.result.success = true;
                    vm.result.message = data;
                })
                .catch(function (error) {
                    console.log('err', error);
                    vm.result.message = error.data.message;
                })
                .finally(function () {
                    vm.file = undefined;
                    vm.isBusy = false;
                    $modal.close(vm.result);
                });
        }

    }

})();