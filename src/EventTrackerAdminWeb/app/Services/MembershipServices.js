app.factory('MembershipService', function ($http, $q, $log, $rootScope) {

    //var baseUrl = '/api/member';
    var baseUrl = 'http://localhost:38012/api/membership/getmembers';

    var service = {
        data: {
            currentmember: {},
            members: [],
            selected: [],
            totalPages: 0,

            filterOptions: {
                filterText: '',
                externalFilter: 'searchText',
                useExternalFilter: true
            },
            sortOptions: {
                fields: ["MemberId"],
                directions: ["desc"]
            },
            pagingOptions: {
                pageSizes: [10, 20, 50, 100],
                pageSize: 10,
                currentPage: 1
            }
        },

        find: function () {
            var params = {
                searchtext: service.data.filterOptions.filterText,
                page: service.data.pagingOptions.currentPage,
                pageSize: service.data.pagingOptions.pageSize,
                sortBy: service.data.sortOptions.fields[0],
                sortDirection: service.data.sortOptions.directions[0]
            };

            var deferred = $q.defer();
            $http.get(baseUrl, { params: params })
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