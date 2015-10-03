
var app = angular.module('app', ['ngRoute', 'ui.bootstrap']);

app.config(
    function ($routeProvider, $httpProvider, $locationProvider) {

        $routeProvider
            .when('/members', { templateUrl: 'app/views/members/index.html', controller: 'MembershipController' })

            .otherwise({ redirectTo: '/members' });
    }
);