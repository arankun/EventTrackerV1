
var app = angular.module('app', ['ngRoute', 'ui.bootstrap', 'ngGrid']);

app.config(
    function ($routeProvider, $httpProvider, $locationProvider) {
        $routeProvider.when('/', { controller: 'MembershipController', templateUrl: 'app/Views/Members/index.html' })
                      .when('/members', { templateUrl: 'app/views/members/index.html', controller: 'MembershipController' })
            .otherwise({ redirectTo: '/members' });
    }
);