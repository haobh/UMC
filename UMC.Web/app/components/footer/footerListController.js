(function (app) {
    app.controller('footerListController', footerListController);
    //tiem 2 bien vao Controller
    footerListController.$inject = ['$scope', 'apiService'];

    function footerListController($scope, apiService) {
        $scope.footer = [];

        $scope.getFooter = getFooter;

        function getFooter() {
            apiService.get('/api/footer/getall', null, function (result) {
                $scope.footer = result.data; //gan vao scope.footer, giong viewBag(ng-model)
            }, function () {
                console.log('Load footer failed.');
            });
        }

        $scope.getFooter();
    }
})(angular.module('umc.footer')); //goi module footer.module.js