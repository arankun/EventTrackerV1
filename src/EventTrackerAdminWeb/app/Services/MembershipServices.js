app.factory('MembershipServices', function ($http, $q, $log, $rootScope) {

    var baseUrl = 'http://localhost:38012/api/membership';

    var service = {
        data: {
            members: []
        },
        find: function () {
            var deferred = $q.defer();
            $http.get(baseUrl)
            .success(function (data) {
                service.data.members = data;
                deferred.resolve(data);
            }).error(function () {
                deferred.reject();
            });
            return deferred.promise;
        }

    }
    service.find();
    return service;

});