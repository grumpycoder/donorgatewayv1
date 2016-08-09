//mark.lawrence
//events.js

(function () {
    'use strict';

    var controllerId = 'EventController';

    angular.module('app.events').controller(controllerId, mainController);

    mainController.$inject = ['logger', '$uibModal', 'eventService', 'guestService', 'templateService'];

    function mainController(logger, $modal, service, guestService, templateService) {
        var vm = this;
        var tableStateRef;
        var pageSizeDefault = 10;

        //TODO: hostLocation needs to be dynamic to environment
        vm.hostLocation = 'localhost:54505/';

        vm.title = 'Event Manager';
        vm.description = "Manage Donor Events";
        vm.currentDate = new Date();

        vm.dateOptions = {
            dateDisabled: false,
            formatYear: 'yy',
            maxDate: new Date(2020, 5, 22),
            minDate: new Date(),
            startingDay: 1
        };

        vm.dateFormat = "MM/DD/YYYY h:mm a";
        vm.events = [];

        vm.searchModel = {
            page: 1,
            pageSize: pageSizeDefault,
            orderBy: 'id',
            orderDirection: 'asc'
        };

        vm.tabs = [
            { title: 'Details', template: 'app/events/views/home.html', active: false, icon: 'fa-info-circle' },
            { title: 'Guests', template: 'app/events/views/guest-list.html', active: true, icon: 'fa-users' },
            //{ title: 'Mail Queue', template: 'app/events/views/mail-queue.html', active: false, icon: '' },
            //{ title: 'Waiting Queue', template: 'app/events/views/wait-queue.html', active: false, icon: '' },
            { title: 'Template', template: 'app/events/views/template.html', active: false, icon: 'fa-file-text' }
        ];

        activate();

        function activate() {
            logger.log(controllerId + ' activated');
            getEvents().then(function () {
                logger.log('loaded events');
            });
        }

        vm.addToMailQueue = function (guest) {
            vm.isBusy = true;
            service.addToMail(guest)
                .then(function (data) {
                    logger.info('Added Guest to Queue: ' + data.name);
                    angular.extend(guest, data);
                    logger.success('Issued ticket to: ' + guest.name);

                    var event = angular.copy(vm.selectedEvent);
                    angular.extend(vm.selectedEvent, guest.event);
                    vm.selectedEvent.guests = event.guests;
                }).finally(function () {
                    //complete();
                    vm.isBusy = false;
                });
        }

        vm.changeEvent = function () {
            if (!vm.selectedEvent) return;
            //RESET VALUES
            vm.searchModel = {
                page: 1,
                pageSize: pageSizeDefault,
                orderBy: 'id',
                orderDirection: 'asc'
            };
            vm.showWaitList = false;
            vm.showMailQueue = false;
            vm.tabs[0].active = true;

            vm.isBusy = true;
            service.getById(vm.selectedEvent.id)
                .then(function (data) {
                    angular.extend(vm.selectedEvent, data);
                }).finally(function () {
                    vm.isBusy = false;
                    vm.searchGuests(tableStateRef);
                });

        }

        vm.refreshGuests = function () {
            vm.searchGuests(tableStateRef);
        }

        vm.deleteEvent = function (id) {
            //TODO: Confirmation on delete
            vm.isBusy = true;
            service.remove(id)
                .then(function () {
                    vm.selectedEvent = undefined;
                    logger.success('Deleted event');
                }).finally(function () {
                    vm.isBusy = false;
                    getEvents();
                });
        }

        vm.exportGuests = function () {
            vm.isBusy = true;
            service.downloadGuests(vm.selectedEvent.id, vm.searchModel)
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

        vm.createGuest = function () {

            $modal.open({
                templateUrl: '/app/events/views/edit-guest.html',
                controller: 'CreateGuestController',
                controllerAs: 'vm', 
                resolve: {
                    event: vm.selectedEvent
                }
            }).result.then(function (result) {
                
                var event = angular.copy(vm.selectedEvent);
                angular.extend(vm.selectedEvent, result.event);
                vm.selectedEvent.guests = event.guests;

                vm.selectedEvent.guests.unshift(result);
            });
        }

        vm.registerGuest = function (guest) {
           
            $modal.open({
                templateUrl: '/app/events/views/edit-guest.html',
                controller: 'EditGuestController',
                controllerAs: 'vm',
                resolve: {
                    guest: guest
                }
            }).result.then(function (result) {
                angular.extend(guest, result);
                var event = angular.copy(vm.selectedEvent);
                angular.extend(vm.selectedEvent, result.event);
                vm.selectedEvent.guests = event.guests;
            });
        }

        vm.toggleWaiting = function () {
            vm.showWaiting = !vm.showWaiting;
            vm.showMail = false;
            vm.showSent = false; 
            vm.searchModel.page = 1;
            vm.searchGuests(tableStateRef);
        }

        vm.toggleMail = function () {
            vm.showMail = !vm.showMail;
            vm.showWaiting = false;
            vm.showSent = false;
            vm.searchModel.page = 1;
            vm.searchGuests(tableStateRef);
        }

        vm.toggleSent = function() {
            vm.showSent = !vm.showSent;
            vm.showWaiting = false;
            vm.showMail = false; 
            vm.searchModel.page = 1;
            vm.searchGuests(tableStateRef);
        }

        vm.mailAllTickets = function () {
            vm.isBusy = true;
            service.mailAllTickets(vm.selectedEvent.id)
                .then(function (data) {
                    angular.extend(vm.selectedEvent, data);
                    vm.searchGuests(tableStateRef);
                    vm.isBusy = false; 
                });
        }

        vm.mailTicket = function (guest) {
            vm.isBusy = true;
            service.mailTicket(guest)
                        .then(function (data) {
                            angular.extend(guest, data);
                            
                            logger.success('Issued ticket to: ' + guest.name);

                            var event = angular.copy(vm.selectedEvent);
                            angular.extend(vm.selectedEvent, guest.event);
                            vm.selectedEvent.guests = event.guests;

                        }).finally(function () {
                            vm.isBusy = false;
                        });
        }

        vm.searchGuests = function (tableState) {
            tableStateRef = tableState;
            if (!vm.selectedEvent) return false;

            if (typeof (tableState.sort.predicate) !== "undefined") {
                vm.searchModel.orderBy = tableState.sort.predicate;
                vm.searchModel.orderDirection = tableState.sort.reverse ? 'desc' : 'asc';
            }
            if (typeof (tableState.search.predicateObject) !== "undefined") {
                vm.searchModel.name = tableState.search.predicateObject.name;
                vm.searchModel.address = tableState.search.predicateObject.address;
                vm.searchModel.city = tableState.search.predicateObject.city;
                vm.searchModel.state = tableState.search.predicateObject.state;
                vm.searchModel.zipcode = tableState.search.predicateObject.zipcode;
                vm.searchModel.guestCount = tableState.search.predicateObject.guestCount;
                vm.searchModel.phone = tableState.search.predicateObject.phone;
                vm.searchModel.email = tableState.search.predicateObject.email;
                vm.searchModel.accountId = tableState.search.predicateObject.accountId;
                vm.searchModel.finderNumber = tableState.search.predicateObject.finderNumber;
                vm.searchModel.isMailed = tableState.search.predicateObject.isMailed;
            }

            vm.searchModel.isAttending = null;
            vm.searchModel.isWaiting = null;
            vm.searchModel.isMailed = null; 

            if (vm.showWaiting) {
                vm.searchModel.isWaiting = vm.showWaiting ? vm.showWaiting : null;
            }

            if (vm.showMail) {
                vm.searchModel.isWaiting = false;
                vm.searchModel.isMailed = false;
                vm.searchModel.isAttending = true;
            }

            if (vm.showSent) {
                vm.searchModel.isWaiting = false;
                vm.searchModel.isMailed = true;
                vm.searchModel.isAttending = true;
            }

            vm.isBusy = true;
            return service.getGuests(vm.selectedEvent.id, vm.searchModel)
                .then(function (data) {
                    vm.selectedEvent.guests = data.items;
                    vm.searchModel = data;
                    vm.isBusy = false;
                });

        }

        vm.paged = function paged() {
            vm.searchGuests(tableStateRef);
        };

        vm.saveEvent = function (form) {
            vm.isBusy = true;
            return service.update(vm.selectedEvent)
                .then(function (data) {
                    var guests = vm.selectedEvent.guests;
                    angular.extend(vm.selectedEvent, data);

                    vm.selectedEvent.guests = guests;
                    logger.success('Saved Event: ' + data.name);
                })
                .finally(function () {
                    form.$setPristine();
                    vm.isBusy = false;
                });
        }

        vm.fileSelected = function ($files, $file) {
            //TODO: Called multiple times. Error on directory selection of file. 
            var reader = new FileReader();
            reader.onloadstart = function () {
                vm.isBusy = true;
            };
            reader.onloadend = function () {
                vm.isBusy = false;
            }
            reader.onload = function () {
                debugger;
                var dataUrl = reader.result;
                vm.selectedEvent.template.image = dataUrl.split(",")[1];
                vm.selectedEvent.template.mimeType = $file.type;
            };
            reader.readAsDataURL($file);

        };

        vm.saveTemplate = function () {
            vm.isBusy = true;
            templateService.update(vm.selectedEvent.template)
                .then(function (data) {
                    vm.selectedEvent.template = angular.extend(vm.selectedEvent.template, data);
                    logger.success('Saved template: ' + data.name);
                }).finally(function () {
                    vm.isBusy = false;
                });
        }

        vm.toggleCancel = function (form) {
            vm.selectedEvent.isCancelled = !vm.selectedEvent.isCancelled;
            vm.saveEvent(form).then(function () {
                if (vm.selectedEvent.isCancelled) {
                    logger.warning('Cancelled event');
                } else {
                    logger.info('Restored event');
                }
            });
        }

        vm.showCreateEvent = function () {
            $modal.open({
                templateUrl: '/app/events/views/create-event.html',
                controller: 'CreateEventController',
                controllerAs: 'vm'
            }).result.then(function (data) {
                vm.selectedEvent = data;
                vm.events.unshift(vm.selectedEvent);
                logger.success('Successfully created ' + data.name);
            });
        }

        vm.showGuestUpload = function () {
            $modal.open({
                keyboard: false,
                backdrop: 'static',
                templateUrl: '/app/events/views/guest-upload.html',
                //controller: ['logger', '$uibModalInstance', 'fileService', 'selectedEvent', UploadGuestController],
                controller: 'UploadGuestController',
                controllerAs: 'vm',
                resolve: {
                    event: vm.selectedEvent
                }
            }).result.then(function (result) {
                if (result.success) {
                    logger.success(result.message);
                } else {
                    logger.error(result.message);
                }
                vm.changeEvent();
            });
        }

        function getEvents() {
            vm.isBusy = true;
            return service.get()
                .then(function (data) {
                    vm.events = data;
                }).finally(function () {
                    vm.isBusy = false;
                });
        }

    };

})();