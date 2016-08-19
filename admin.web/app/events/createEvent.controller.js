//mark.lawrence
//createEvent.controller.js

(function() {
	
    'use strict';

    var controllerId = 'CreateEventController';

    angular.module('app.events').controller(controllerId, mainController);

    mainController.$inject = ['logger', '$uibModalInstance', 'eventService'];

    function mainController(logger, $modal, service) {
        var vm = this;
        vm.title = 'Create Event';
        
        vm.dateFormat = "MM/DD/YYYY hh:mm";

        vm.event = {
            startDate: new Date(),
            capacity: 1,
            template: {}
        };

        vm.cancel = function () {
            $modal.dismiss();
        }

        vm.save = function () {
            vm.isSaving = true; 
            vm.event.template.name = vm.event.name;
            service.create(vm.event)
                .then(function (data) {
                    $modal.close(data);
                });
        }
    }

})();