//app.filter.js
//mark.lawrence

angular.module('app.filter').filter('percentage', ['$filter', function ($filter) {
    return function (input, decimals) {
        return $filter('number')(input * 100, decimals) + '%';
    };
}]);

angular.module('app.filter', []).filter('checkmark', function () {
    return function (input) {
        return input ? '\u2713' : '\u2718';
    };
});

angular.module('app.filter').filter('yesNo', function () {
    return function (input) {
        return input ? 'Yes' : 'No';
    }
});

angular.module('app.filter').filter('updateStatus', function () {
    return function (input) {
        if (input === 1) {
            return 'Unchanged';
        }
        if (input === 2) {
            return 'Changed';
        }
        if (input === 6) {
            return 'Reconciled';
        }
        return "";
    };
});