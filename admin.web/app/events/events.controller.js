﻿//mark.lawrence
//events.js

(function () {
    'use strict';

    var controllerId = 'EventController';

    angular.module('app.events').controller(controllerId, mainController);

    mainController.$inject = ['$scope', 'logger', '$uibModal', 'eventService', 'guestService', 'templateService'];

    function mainController($scope, logger, $modal, service, guestService, templateService) {
        var vm = this;
        vm.title = 'Event Manager';
        var tableStateRef;
        var pageSizeDefault = 10;

        var choices = [
            { id: 1, name: "Register", command: function (e) { vm.registerGuest(e) }, icon: 'icon ion-key', default: true },
            { id: 2, name: "Mail Ticket", command: function (e) { vm.mailTicket(e) }, icon: 'icon ion-android-mail', default: false },
            { id: 3, name: "Cancel", command: function (e) { vm.cancelGuest(e) }, icon: 'icon ion-android-cancel', default: false },
            { id: 4, name: "Guest List", command: function (e) { vm.addToMailQueue(e) }, icon: 'icon ion-android-add-circle', default: false },
            { id: 5, name: "Add Tickets", command: function (e) { vm.registerGuest(e) }, icon: 'icon ion-android-add-circle', default: false }
        ];

        vm.hostLocation = window.__env.rsvpUrl + '/';

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
            { title: 'Guests', template: 'app/events/views/guest-list.html?v1.1', active: true, icon: 'fa-users' },
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
            console.log('add to mail queue');service.addToMail(guest)
                .then(function (data) {
                    logger.info('Moved Guest ' + data.name + 'Send Ticket: ');
                    angular.extend(guest, data);

                    BuildGuestOptions(guest);
                    var event = angular.copy(vm.selectedEvent);
                    angular.extend(vm.selectedEvent, guest.event);
                    vm.selectedEvent.guests = event.guests;
                }).finally(function () {
                    vm.isBusy = false;
                });
        }

        vm.cancelGuest = function (guest) {
            service.cancelGuest(guest)
                .then(function (result) {
                    logger.log('result', result);
                    angular.extend(guest, result);
                    //BUG: Not changin options
                    BuildGuestOptions(guest);
                    var event = angular.copy(vm.selectedEvent);
                    angular.extend(vm.selectedEvent, result.event);
                    vm.selectedEvent.guests = event.guests;
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
            vm.isBusy = true;
            service.remove(id)
                .then(function () {
                    logger.success('Deleted event');
                }).finally(function () {
                    vm.selectedEvent = undefined;
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
                var guest = result;
                BuildGuestOptions(guest);
                var event = angular.copy(vm.selectedEvent);
                angular.extend(vm.selectedEvent, result.event);
                vm.selectedEvent.guests = event.guests;
                vm.selectedEvent.guests.unshift(guest);
            });
        }

        vm.registerGuest = function (guest) {
            guest.event = angular.copy(vm.selectedEvent);
            $modal.open({
                templateUrl: '/app/events/views/edit-guest.html',
                controller: 'EditGuestController',
                controllerAs: 'vm',
                resolve: {
                    guest: guest
                }
            }).result.then(function (result) {
                angular.extend(guest, result);
                BuildGuestOptions(guest);

                var event = angular.copy(vm.selectedEvent);
                angular.extend(vm.selectedEvent, result.event);
                vm.selectedEvent.guests = event.guests;
                guest = null;
            });
        }

        vm.toggleWaiting = function () {
            vm.showWaiting = !vm.showWaiting;
            vm.showWaitingSent = false; 
            vm.showMail = false;
            vm.showSent = false;
            vm.searchModel.page = 1;
            vm.searchGuests(tableStateRef);
        }

        vm.toggleWaitingSent = function () {
            vm.showWaitingSent = !vm.showWaitingSent;
            vm.showMail = false;
            vm.showSent = false;
            vm.showWaiting = false;
            vm.searchModel.page = 1;
            vm.searchGuests(tableStateRef);
        }

        vm.toggleMail = function () {
            vm.showMail = !vm.showMail;
            vm.showWaiting = false;
            vm.showWaitingSent = false; 
            vm.showSent = false;
            vm.searchModel.page = 1;
            vm.searchGuests(tableStateRef);
        }

        vm.toggleSent = function () {
            vm.showSent = !vm.showSent;
            vm.showWaiting = false;
            vm.showWaitingSent = false; 
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

        vm.mailAllWaiting = function () {
            vm.isBusy = true;
            service.mailAllWaiting(vm.selectedEvent.id)
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
                            BuildGuestOptions(guest);

                            var event = angular.copy(vm.selectedEvent);
                            angular.extend(vm.selectedEvent, guest.event);
                            vm.selectedEvent.guests = event.guests;
                            logger.success('Mailed ticket to: ' + guest.name);

                        }).finally(function () {
                            vm.isBusy = false;
                        });
        }

        vm.searchGuests = function (tableState) {
            tableStateRef = tableState;
            if (!vm.selectedEvent) return false;

            if (tableState !== undefined) {
                if (typeof (tableState.sort.predicate) !== "undefined") {
                    vm.searchModel.orderBy = tableState.sort.predicate;
                    vm.searchModel.orderDirection = tableState.sort.reverse ? 'desc' : 'asc';
                }
            }

            vm.searchModel.isAttending = null;
            vm.searchModel.isWaiting = null;
            vm.searchModel.isMailed = null;

            if (vm.showWaiting) {
                vm.searchModel.isWaiting = true; //vm.showWaiting ? vm.showWaiting : null;
                vm.searchModel.isMailed = false; //vm.showWaitingSent ? vm.showWaitingSent : null;
            }

            if (vm.showWaitingSent) {
                vm.searchModel.isWaiting = true;// vm.showWaitingSent ? vm.showWaitingSent : null;
                vm.searchModel.isMailed = true; //vm.showWaitingSent ? vm.showWaitingSent : null;
                vm.searchModel.isAttending = true;
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
                    var guests = data.items.map(function (guest) {
                        BuildGuestOptions(guest);
                        return guest;
                    });
                    vm.selectedEvent.guests = guests;
                   
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
            var file = $file;

            var src = '';
            var reader = new FileReader();

            reader.onloadstart = function () {
                vm.isBusy = true;
            }

            reader.onload = function (e) {
                src = reader.result;
                vm.selectedEvent.template.image = reader.result;
                vm.selectedEvent.template.mimeType = file.type;
            }
            reader.onerror = function (e) {
                logger.log(e);
            }

            reader.onloadend = function (e) {
                vm.isBusy = false;
                //Added due to large images not complete before digest cycle. 
                $scope.$apply();
            };

            reader.readAsDataURL(file);
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

        function BuildGuestOptions(guest) {
            var options = [];

            if (guest.isAttending === null || guest.isAttending === false) options.push(choices[0]); 
            if (guest.isAttending && !guest.isWaiting && !guest.isMailed) {
                options.push(choices[1]);
            }
            if (guest.isWaiting) {
                options.push(choices[3]);
            }
            if (guest.isAttending) {
                options.push(choices[4]);
            }
            if (guest.isAttending === true) options.push(choices[2]);

            guest.primaryChoice = angular.copy(options[0]);
            options.shift();

            angular.extend(guest, { choices: options });
        }

    };

})();