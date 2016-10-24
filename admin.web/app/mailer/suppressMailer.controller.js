//mark.lawrence
//suppressMailer.controller.js

(function () {

    'use strict';

    var controllerId = 'SuppressMailerController';

    angular.module('app.mailer').controller(controllerId, mainController);

    mainController.$inject = ['logger', '$uibModalInstance', 'mailerService', 'mailer', 'reasons'];

    function mainController(logger, $modal, service, mailer, reasons) {
        var vm = this;
        vm.title = 'Mailer Suppress';
        vm.mailer = mailer;

        vm.reasons = reasons;

        vm.mailer.reasonId = vm.reasons[0].id;

        vm.cancel = function () {
            $modal.dismiss();
        }

        vm.cancel = function() {
            $modal.dismiss();
        }

        vm.save = function () {
            vm.mailer.suppress = true;
            service.save(vm.mailer).then(function (data) {
                angular.extend(mailer, data);
            });
            $modal.close(vm.mailer);
        }

    }

})();