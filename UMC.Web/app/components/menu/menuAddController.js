(function (app) {
    app.controller('menuAddController', menuAddController);
    //tiem 2 bien vao Controller
    menuAddController.$inject = ['$scope', 'apiService', 'notificationService', '$state'];
    function menuAddController($scope, apiService, notificationService, $state) {
        $scope.menu = {
            //Name: 'Danh mục 1', //Mặc định những thuộc tính bên View sẽ set mặc định   
            Status: true      //Mặc định những thuộc tính bên View sẽ set mặc định
        }

        $scope.AddMenu = AddMenu; //Bên View sử dụng ng-submit

        function AddMenu() {
            apiService.post('/api/menu/create', $scope.menu, //Bên View sẽ sử dụng menu.Name
                function (result) {
                    notificationService.displaySuccess($scope.menu.Name + ' đã được thêm mới.');
                    $state.go('menu');  //Redirect lại về trang,(state trong module)
                }, function (error) {
                    notificationService.displayError('Thêm mới không thành công.');
                });
        }

        function loadMenuGroupID() {
            apiService.get('/api/menu/getmenugroupid', null, function (result) {
                $scope.menuGroupID = result.data;
            }, function () {
                console.log('Cannot get list parent');
            });
        }
        loadMenuGroupID();
    }
})(angular.module('umc.menu')); //controller add nay nam trong module menu