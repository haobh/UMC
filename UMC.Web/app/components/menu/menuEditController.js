(function (app) {
    app.controller('menuEditController', menuEditController);

    menuEditController.$inject = ['apiService', '$scope', 'notificationService', '$state', '$stateParams'];

    function menuEditController(apiService, $scope, notificationService, $state, $stateParams) {
        //Nhận Parameter từ View lên và gán vào ViewModel
        $scope.menu = {
            //CreatedDate: new Date(),
            //Status: true
        }

        $scope.UpdateMenu = UpdateMenu;

        function loadMenuDetail() {
            apiService.get('/api/menu/getbyid/' + $stateParams.id, null, function (result) {
                $scope.menu = result.data;
            }, function (error) {
                notificationService.displayError(error.data);
            });
        }

        function UpdateMenu() {
            apiService.put('/api/menu/update', $scope.menu,
                function (result) {
                    notificationService.displaySuccess(result.data.Name + ' đã được cập nhật.');
                    $state.go('menu');
                }, function (error) {
                    notificationService.displayError('Cập nhật không thành công.');
                });
        }
        //load GroupId, / để xóa /admin đi
        function loadMenuGroupID() {
            apiService.get('/api/menu/getmenugroupid', null, function (result) {
                $scope.menuGroupID = result.data;
            }, function () {
                console.log('Cannot get list parent');
            });
        }

        loadMenuGroupID();
        loadMenuDetail();
    }

})(angular.module('umc.menu'));