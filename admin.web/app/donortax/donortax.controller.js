//mark.lawrence
//donortax.controller.js

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
        vm.subTitle = 'Constituents';

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

            if (tableState !== undefined) {
                if (typeof (tableState.sort.predicate) !== "undefined") {
                    vm.searchModel.orderBy = tableState.sort.predicate;
                    vm.searchModel.orderDirection = tableState.sort.reverse ? 'desc' : 'asc';
                }
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

        vm.edit = function (person) {
            $modal.open({
                templateUrl: '/app/donortax/views/edit-constituent.html',
                controller: 'EditConstituentController',
                controllerAs: 'vm', 
                resolve: {
                    person: person
                }
            }).result.then(function (person) {
                logger.success('Successfully updated ' + person.name);
            });
        }

        vm.viewTaxes = function (person) {

            $modal.open({
                templateUrl: '/app/donortax/views/taxitems.html',
                controller: 'TaxItemsController',
                controllerAs: 'vm',
                resolve: {
                    person: person
                }
            }).result.then(function (person) {
                logger.success('Successfully updated ' + person.name);
            });
        }

        vm.showUpload = function () {
            $modal.open({
                keyboard: false,
                backdrop: 'static',
                templateUrl: '/app/donortax/views/tax-upload.html',
                controller: 'UploadTaxController',
                controllerAs: 'vm'
            })
            .result.then(function (result) {
                if (result.success) {
                    logger.success(result.message);
                } else {
                    logger.error(result.message);
                }

            });
        }
    }

})();