//mark.lawrence
//donortax.js

(function() {
    'use strict';

    var controllerId = 'DonorTaxController';

    angular.module('app.donortax').controller(controllerId, mainController);

    mainController.$inject = ['logger', '$uibModal', 'constituentService'];

    function mainController(logger, $modal, service) {
        var vm = this;
        var tableStateRef;
        var pageSizeDefault = 10;

        vm.title = 'Donor Tax Manager';

        vm.people = [];
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

            if (!vm.searchModel.isPriority) vm.searchModel.isPriority = null;

            if (typeof (tableState.sort.predicate) !== "undefined") {
                vm.searchModel.orderBy = tableState.sort.predicate;
                vm.searchModel.orderDirection = tableState.sort.reverse ? 'desc' : 'asc';
            }
            if (typeof (tableState.search.predicateObject) !== "undefined") {
                vm.searchModel.name = tableState.search.predicateObject.name;
                vm.searchModel.lookupId = tableState.search.predicateObject.lookupId;
                vm.searchModel.finderNumber = tableState.search.predicateObject.finderNumber;
                vm.searchModel.zipcode = tableState.search.predicateObject.zipcode;
                vm.searchModel.email = tableState.search.predicateObject.email;
                vm.searchModel.phone = tableState.search.predicateObject.phone;
                vm.searchModel.updateStatus = tableState.search.predicateObject.updateStatus;
            }

            vm.isBusy = true;
            service.query(vm.searchModel)
                .then(function (data) {
                    vm.people = data.items;
                    vm.searchModel = data;
                    vm.isBusy = false;
                });
        }

        vm.paged = function paged(pageNum) {
            vm.search(tableStateRef);
        };

    }

})();