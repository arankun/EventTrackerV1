app.controller('MembershipController', function ($scope, MembershipService, $routeParams, $log) {

    $scope.data = MembershipService.data;
    $scope.hardCodedMembers = [
               { id: 1, name: "Ajay Kumar", age: 34 },
               { id: 2, name: "Vijay Kumar", age: 23 },
               { id: 3, name: "Salman Khan", age: 45 },
               { id: 4, name: "Sanjay Dutt", age: 53 },
               { id: 5, name: "John Abraham", age: 40 },
               { id: 6, name: "John6 Abraham", age: 40 },
               { id: 7, name: "John7 Abraham", age: 40 },
               { id: 8, name: "John8 Abraham", age: 40 },
               { id: 9, name: "John9 Abraham", age: 40 },
               { id: 10, name: "John10 Abraham", age: 40 },
               { id: 11, name: "John11 Abraham", age: 40 }];

    $scope.gridOptionsTest = { data: 'hardCodedMembers' };

    $scope.$watch('data.sortOptions', function (newVal, oldVal) {
        $log.log("sortOptions changed: " + newVal);
        if (newVal !== oldVal) {
            $scope.data.pagingOptions.currentPage = 1;
            MembershipService.find();
        }
    }, true);

    $scope.$watch('data.filterOptions', function (newVal, oldVal) {
        $log.log("filters changed: " + newVal);
        if (newVal !== oldVal) {
            $scope.data.pagingOptions.currentPage = 1;
            MembershipService.find();
        }
    }, true);

    $scope.$watch('data.pagingOptions', function (newVal, oldVal) {
        $log.log("page changed: " + newVal);
        if (newVal !== oldVal) {
            MembershipService.find();
        }
    }, true);

    $scope.gridOptions = {
        data: 'data.members.Content',
        showFilter: false,
        multiSelect: false,
        selectedItems: $scope.data.selected,
        enablePaging: true,
        showFooter: true,
        totalServerItems: 'data.members.totalRecords',
        pagingOptions: $scope.data.pagingOptions,
        filterOptions: $scope.data.filterOptions,
        useExternalSorting: true,
        sortInfo: $scope.data.sortOptions,
        plugins: [new ngGridFlexibleHeightPlugin()],
        columnDefs: [
                    { field: 'FullName', displayName: 'Name' },
                    { field: 'Email', displayName: 'Email' },
                    { field: 'Phone', displayName: 'Phone' },
                    { field: 'DateOfBirth', displayName: 'DOB' },
                    { field: 'Gender', displayName: 'Gender' },
                    { field: 'MemberOf', displayName: 'Member' },
                    { field: 'Household', displayName: 'HH' }
        ],
        afterSelectionChange: function (selection, event) {
            $log.log("selection: " + selection.entity.MemberId);
            // $location.path("comments/" + selection.entity.commentId);
        }
    };
});
