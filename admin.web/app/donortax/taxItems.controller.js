//mark.lawrence
//taxItems.controller.js

(function () {
    'use strict';

    var controllerId = 'TaxItemsController';

    angular.module('app.donortax').controller(controllerId, mainController);

    mainController.$inject = ['$scope', 'logger', '$uibModalInstance', 'constituentService', 'person'];

    function mainController($scope, logger, $modal, service, constituent) {
        var vm = this;
        var currentYear = parseInt(moment().get('Year'));

        vm.selectedYear = currentYear - 1;
        vm.dateOptions = {
            formatYear: 'yyyy',
            maxDate: new Date('12/30/' + vm.selectedYear),
            minDate: new Date('1/1/' + vm.selectedYear),
            startingDay: 1
        };
        vm.altInputFormats = ['M!/d!/yyyy'];
        vm.years = [];

        vm.yearChanged = yearChanged;

        vm.constituent = constituent;
        vm.itemToEdit = {};
        vm.newItem = {};
        vm.taxItems = constituent.taxItems;
        activate();

        function activate() {
            getYears();
        }


        vm.addItem = function() {
            vm.newItem.constituentId = constituent.id;
            vm.newItem.taxYear = moment(vm.newItem.donationDate).year();

            service.addTax(vm.newItem)
                .then(function (data) {
                    vm.taxItems.push(data);
                    vm.newItem = {};
                    logger.success('Added tax item');
                });
        }

        vm.cancelEdit = function() {
            vm.currentEdit = {};
        }

        vm.deleteItem = function(item) {
            var idx = vm.taxItems.indexOf(item);
            service.removeTax(item.id)
                .then(function (data) {
                    vm.taxItems.splice(idx, 1);
                    logger.warning('deleted ' + item.donationDate);
                });
        }

        vm.editItem = function(item) {
            vm.currentEdit = {};
            vm.currentEdit[item.id] = true;
            vm.itemToEdit = angular.copy(item);
            vm.itemToEdit.donationDate = moment(vm.itemToEdit.donationDate).toDate();
        }

        vm.saveItem = function(item) {
            service.updateTax(vm.itemToEdit)
                .then(function (response) {
                    angular.extend(item, vm.itemToEdit);
                    vm.currentEdit = {};
                    logger.success('Updated tax item ' + moment(item.donationDate).format('MM/dd/yyyy'));
                });
        }

        function yearChanged() {
            vm.dateOptions = {
                maxDate: new Date('12/30/' + vm.selectedYear),
                minDate: new Date('1/1/' + vm.selectedYear)
            };
            vm.newItem.donationDate = vm.dateOptions.minDate;
        }

        function getYears() {
            for (var i = 0; i < 5; i++) {
                vm.years.push(currentYear - i);
            }
            vm.yearChanged();
        }

        vm.cancel = function () {
            $modal.dismiss();
        }

    }

})();