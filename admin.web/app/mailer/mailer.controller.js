//mark.lawrence
//mailer.controller.js

(function() {
    'use strict';

    var controllerId = 'MailerController';

    angular.module('app.mailer').controller(controllerId, mainController);

    mainController.$inject = ['logger', '$uibModal'];

    function mainController(logger, $modal) {
        var vm = this;
        var tableStateRef;
        var pageSizeDefault = 10;

        vm.title = 'Mailer Manager';
        vm.subTitle = 'Mailers';

        vm.mailers = [];
        vm.searchModel = {
            page: 1,
            pageSize: pageSizeDefault,
            orderBy: 'id',
            orderDirection: 'asc'
        };
        
        activate();

        function activate() {
            logger.log(controllerId + ' activated');
        }

        vm.search = function (tableState) {
            tableStateRef = tableState;

            //if (!vm.searchModel.isPriority) vm.searchModel.isPriority = null;

            //if (typeof (tableState.sort.predicate) !== "undefined") {
            //    vm.searchModel.orderBy = tableState.sort.predicate;
            //    vm.searchModel.orderDirection = tableState.sort.reverse ? 'desc' : 'asc';
            //}
            //if (typeof (tableState.search.predicateObject) !== "undefined") {
            //    vm.searchModel.name = tableState.search.predicateObject.name;
            //    vm.searchModel.lookupId = tableState.search.predicateObject.lookupId;
            //    vm.searchModel.finderNumber = tableState.search.predicateObject.finderNumber;
            //    vm.searchModel.zipcode = tableState.search.predicateObject.zipcode;
            //    vm.searchModel.email = tableState.search.predicateObject.email;
            //    vm.searchModel.phone = tableState.search.predicateObject.phone;
            //}

            //vm.isBusy = true;
            //service.query(vm.searchModel)
            //    .then(function (data) {
            //        vm.people = data.items;
            //        vm.searchModel = data;
            //        vm.isBusy = false;
            //    });
        }

        vm.paged = function paged(pageNum) {
            vm.search(tableStateRef);
        };

    }

})();