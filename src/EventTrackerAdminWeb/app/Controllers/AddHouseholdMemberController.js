app.module('MembershipModule').controller('AddHouseholdMemberController', ['$scope', '$location',
        '$routeParams', '$timeout', 'config', 'dataService', 'modalService',
        function ($scope, $location, $routeParams, $timeout, config,
                  dataService, modalService) {
            $scope.deleteCustomer = function () {

                var custName = $scope.customer.firstName + ' ' + $scope.customer.lastName;


                var modalOptions = {
                    closeButtonText: 'Cancel',
                    actionButtonText: 'Delete Customer',
                    headerText: 'Delete ' + custName + '?',
                    bodyText: 'Are you sure you want to delete this customer?'
                };

                modalService.showModal({}, modalOptions).then(function (result) {
                    dataService.deleteCustomer($scope.customer.id).then(function () {
                        $location.path('/customers');
                    }, processError);
                });
            }
        }]);