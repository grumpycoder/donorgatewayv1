//mark.lawrence
//nav.js

(function () {
    'use strict';

    var controllerId = 'NavController';

    angular.module('app.nav').controller(controllerId, mainController);

    mainController.$inject = ['logger'];

    function mainController(logger) {
        var vm = this;

        activate();

        function activate() {
            logger.log(controllerId + ' activated');
            vm.user = JSON.parse(localStorage.getItem('currentUser'));
        }
    };
})();