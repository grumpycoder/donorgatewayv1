//mark.lawrence
//mailer.controller.js

(function() {
    'use strict';

    var controllerId = 'MailerController';

    angular.module('app.mailer').controller(controllerId, mainController);

    mainController.$inject = ['logger', '$uibModal', 'mailerService'];

    function mainController(logger, $modal, service) {
        var vm = this;
        var tableStateRef;
        var pageSizeDefault = 10;

        vm.title = 'Mailer Manager';
        vm.subTitle = 'Mailers';

        vm.campaigns = [];
        vm.reasons = [];

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
            service.campaigns()
                .then(function(data) {
                    vm.campaigns = data;
                });

            service.reasons()
                .then(function (data) {
                    vm.reasons = data;
                });

        }

        vm.search = function (tableState) {
            tableStateRef = tableState;

            if (typeof (tableState.sort.predicate) !== "undefined") {
                vm.searchModel.orderBy = tableState.sort.predicate;
                vm.searchModel.orderDirection = tableState.sort.reverse ? 'desc' : 'asc';
            }
            if (typeof (tableState.search.predicateObject) !== "undefined") {
                vm.searchModel.firstName = tableState.search.predicateObject.firstName;
                vm.searchModel.lastName = tableState.search.predicateObject.lastName;
                vm.searchModel.address = tableState.search.predicateObject.address;
                vm.searchModel.city = tableState.search.predicateObject.city;
                vm.searchModel.state = tableState.search.predicateObject.state;
                vm.searchModel.zipCode = tableState.search.predicateObject.zipCode;
                vm.searchModel.finderNumber = tableState.search.predicateObject.finderNumber;
                vm.searchModel.sourceCode = tableState.search.predicateObject.sourceCode;
                vm.searchModel.campaignId = tableState.search.predicateObject.campaignId;
                vm.searchModel.reasonId = tableState.search.predicateObject.reasonId;
            }

            vm.isBusy = true;
            service.query(vm.searchModel)
                .then(function (data) {
                    vm.mailers = data.items;
                    logger.log('mailers', vm.mailers);
                    vm.searchModel = data;
                    vm.isBusy = false;
                });
        }

        vm.paged = function paged(pageNum) {
            vm.search(tableStateRef);
        };

    }

})();