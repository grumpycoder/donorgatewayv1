//mark.lawrence
//taxItems.controller.js

(function () {
    'use strict';

    var controllerId = 'TaxItemsController';

    angular.module('app.donortax').controller(controllerId, mainController);

    mainController.$inject = ['$scope', 'logger', '$uibModalInstance', 'constituentService', 'person'];

    function mainController($scope, logger, $modal, service, person) {
        var vm = this;
        var currentYear = parseInt(moment().get('Year'));

        vm.selectedYear = currentYear - 1;
        logger.log('selectedYear', vm.selectedYear);

        vm.dateOptions = {
            formatYear: 'yyyy',
            maxDate: new Date('12/30/' + vm.selectedYear),
            minDate: new Date('1/1/' + vm.selectedYear),
            startingDay: 1
        };
        vm.altInputFormats = ['M!/d!/yyyy'];

        vm.years = [];

        vm.minDate = new Date('1/1/' + vm.selectedYear);
        vm.maxDate = new Date('12/30/' + vm.selectedYear);

        vm.addItem = addItem;
        vm.cancelEdit = cancelEdit;
        vm.constituent = constituent;
        vm.deleteItem = deleteItem;
        vm.editItem = editItem;
        vm.itemToEdit = {};
        vm.newItem = {};
        vm.saveItem = saveItem;
        vm.taxItems = constituent.taxItems;
        vm.yearChanged = yearChanged;

        activate();

        function activate() {
            getYears();
        }

        function getYears() {
            for (var i = 0; i < 5; i++) {
                vm.years.push(currentYear - i);
            }
        }

        function addItem() {
            vm.newItem.constituentId = constituent.id;
            vm.newItem.taxYear = moment(vm.newItem.donationDate).year();

            service.create(vm.newItem)
                .then(function (data) {
                    vm.taxItems.push(data);
                    vm.newItem = {};
                    logger.success('Added tax item');
                });
        }

        function cancelEdit() {
            vm.currentEdit = {};
        }

        function deleteItem(item) {
            var idx = vm.taxItems.indexOf(item);
            service.remove(item.id)
                .then(function (data) {
                    vm.taxItems.splice(idx, 1);
                    logger.warning('deleted ' + item.donationDate);
                });
        }

        function editItem(item) {
            vm.currentEdit = {};
            vm.currentEdit[item.id] = true;
            vm.itemToEdit = angular.copy(item);
            vm.itemToEdit.donationDate = moment(vm.itemToEdit.donationDate).toDate();
        }

        function saveItem(item) {
            service.update(vm.itemToEdit)
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
    }

})();