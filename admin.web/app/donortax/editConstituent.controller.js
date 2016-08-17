//mark.lawrence
//editConstituent.controller.js

(function () {
    'use strict';

    var controllerId = 'EditConstituentController';

    angular.module('app.donortax').controller(controllerId, mainController);

    mainController.$inject = ['$scope', 'logger', '$uibModalInstance', 'constituentService', 'person'];

    function mainController($scope, logger, $modal, service, person) {
        var vm = this;
        vm.title = 'Edit Person';
        vm.person = person;

        vm.cancel = function () {
            $modal.dismiss();
        }

        vm.save = function () {
            service.save(person)
                .then(function (data) {
                    angular.extend(vm.person, data);
                    $modal.close(vm.person);
                });
        }
    }

})();