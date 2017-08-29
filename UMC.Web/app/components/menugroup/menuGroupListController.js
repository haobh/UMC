(function (app) {
    app.controller('menuGroupListController', menuGroupListController);
    //tiem 2 bien vao Controller
    menuGroupListController.$inject = ['$scope', 'apiService'];

    function menuGroupListController($scope, apiService) {
        $scope.menuGroup = [];

        $scope.getMenuGroup = getMenuGroup;

        function getMenuGroup() {
            apiService.get('/api/menugroup/getall', null, function (result) {
                $scope.menuGroup = result.data; //gan vao scope.menuGroup, giong viewBag(ng-model)
            }, function () {
                console.log('Load menugroup failed.');
            });
        }

        $scope.getMenuGroup();
    }
})(angular.module('umc.menugroup')); //goi module menugroup.module.js