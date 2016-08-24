//mark.lawrence
//demographics.js

(function() {
    'use strict'; 

    var controllerId = 'DemographicsController';

    angular.module('app.demographics').controller(controllerId, UserController);

    UserController.$inject = ['logger', 'demographicService'];

    function UserController(logger, service) {
        var vm = this;
        var tableStateRef;
        var pageSizeDefault = 10;

        vm.title = 'Demographic Updates';
        vm.description = 'Updates made to constituent data'; 
        vm.subTitle = 'Demographics';

        vm.searchModel = {
            page: 1,
            pageSize: pageSizeDefault,
            orderBy: 'id',
            orderDirection: 'asc'
        };

        vm.demographics = [];

        activate();

        function activate() {
            logger.log(controllerId + ' activated');
            vm.manual = false; 
        };

        vm.search = function (tableState) {
            tableStateRef = tableState;

            if (typeof (tableState.sort.predicate) !== "undefined") {
                vm.searchModel.orderBy = tableState.sort.predicate;
                vm.searchModel.orderDirection = tableState.sort.reverse ? 'desc' : 'asc';
            }
            if (typeof (tableState.search.predicateObject) !== "undefined") {
                vm.searchModel.name = tableState.search.predicateObject.name;
                vm.searchModel.lookupId = tableState.search.predicateObject.lookupId;
                vm.searchModel.finderNumber = tableState.search.predicateObject.finderNumber;
                vm.searchModel.city = tableState.search.predicateObject.city;
                vm.searchModel.state = tableState.search.predicateObject.state;
                vm.searchModel.zipcode = tableState.search.predicateObject.zipcode;
                vm.searchModel.email = tableState.search.predicateObject.email;
                vm.searchModel.phone = tableState.search.predicateObject.phone;
                vm.searchModel.updatedBy = tableState.search.predicateObject.updatedBy;
                vm.searchModel.source = tableState.search.predicateObject.source;
            }
            vm.isBusy = true;
            service.query(vm.searchModel)
                .then(function (data) {
                    vm.demographics = data.items;
                    vm.searchModel = data;
                    vm.isBusy = false;
                    vm.manual = true;
                    logger.log(data.items);
                });
        }
        
        vm.manualSearch = function () {
            logger.log('manual');
            vm.search(tableStateRef);
        }

        vm.paged = function paged(pageNum) {
            vm.search(tableStateRef);
        };

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