(function (app) {
    app.controller('menuGroupListController', menuGroupListController);

    menuGroupListController.$inject = ['$scope', 'apiService'];

    function menuGroupListController($scope, apiService) {
        $scope.menuGroup = [];

        $scope.getMenuGroup = getMenuGroup;

        function getMenuGroup() {
            apiService.get('/api/menugroup/getall', null, function (result) {
                $scope.menuGroup = result.data;
            }, function () {
                console.log('Load menugroup failed.');
            });
        }

        $scope.getMenuGroup();
    }
})(angular.module('umc.menugroup'));