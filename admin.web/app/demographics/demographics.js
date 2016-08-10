//mark.lawrence
//demographics.js

(function() {
    'use strict'; 

    var controllerId = 'DemographicsController';

    angular.module('app.demographics').controller(controllerId, UserController);

    UserController.$inject = ['logger', 'demographicService'];

    function UserController(logger, service) {
        var vm = this;
        vm.title = 'Demographic Updates';
        vm.description = 'Updates made to constituent data'; 
        vm.subTitle = 'Demographics';

        vm.demographics = [];

        activate();

        function activate() {
            logger.log(controllerId + ' activated');
            getDemographics();
        };


        function getDemographics() {
            service.get()
                .then(function(data) {
                    vm.demographics = data;
                });
        }

        vm.delete = function(demo) {
            vm.isBusy = true;
            service.remove(demo.id)
                .then(function (data) {
                    logger.success('Deleted ' + demo.name);
                    var idx = vm.demographics.indexOf(demo);
                    vm.demographics.splice(idx, 1);
                }).finally(function() {
                    vm.isBusy = false; 
                });
        }

        vm.deleteAll = function () {
            vm.isBusy = true;
            service.removeAll()
                .then(function (data) {
                    logger.success(data);
                    vm.demographics = []; 
                }).finally(function () {
                    vm.isBusy = false;
                });

        }

        vm.export = function () {
            vm.isBusy = true;
            service.exportList()
                .then(function (data) {
                    var contentType = data.headers()['content-type'];
                    var filename = data.headers()['x-filename'];

                    var linkElement = document.createElement('a');
                    try {
                        var blob = new Blob([data.data], { type: contentType });
                        var url = window.URL.createObjectURL(blob);

                        linkElement.setAttribute('href', url);
                        linkElement.setAttribute("download", filename);

                        var clickEvent = new MouseEvent("click", {
                            "view": window,
                            "bubbles": true,
                            "cancelable": false
                        });
                        linkElement.dispatchEvent(clickEvent);
                    } catch (ex) {
                        logger.log(ex);
                    }
                }).finally(function () {
                    vm.isBusy = false;
                });
        }
    }

})();