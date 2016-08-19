//mark.lawrence
//uploadMailer.controller.js

(function () {

    'use strict';

    var controllerId = 'UploadMailerController';

    angular.module('app.mailer').controller(controllerId, mainController);

    //mainController.$inject = ['logger', '$uibModalInstance', 'fileService', 'campaigns'];
    mainController.$inject = ['logger', '$uibModalInstance', 'fileService', 'mailerService', 'campaigns'];

    function mainController(logger, $modal, service, mailerService, campaigns) {
        var vm = this;
        vm.title = 'Mailer Upload';

        vm.createNew = false;

        vm.campaigns = campaigns;

        vm.campaign = {

        };

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

            service.mailer(vm.selectedCampaign.id, vm.file)
                .then(function (data) {
                    vm.result.success = true;
                    vm.result.message = data;
                })
                .catch(function (error) {
                    vm.result.message = error.data.message;
                })
                .finally(function () {
                    vm.file = undefined;
                    vm.isBusy = false;
                    vm.result.campaigns = vm.campaigns; 
                    $modal.close(vm.result);
                });
        }

        vm.createCampaign = function () {
            mailerService.createCampaign(vm.campaign)
                .then(function (data) {
                    vm.campaigns.unshift(data);
                    vm.selectedCampaign = data;
                    vm.createNew = false;
                });
        }
    }

})();