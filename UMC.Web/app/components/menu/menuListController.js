﻿(function (app) {
    app.controller('menuListController', menuListController);
    //tiem 2 bien vao Controller
    menuListController.$inject = ['$scope', 'apiService'];

    function menuListController($scope, apiService) {
        $scope.menu = [];
        //Doan nay dung phan trang, bien scope giong nhu ViewBag de hien thi tu Controller -> View
        $scope.page = 0;
        $scope.pagesCount = 0;
        //--------------------
        $scope.getMenu = getMenu;
        $scope.keyword = '';
        $scope.search = search;

        function search() {
            getMenu();
        }

        function getMenu(page) {
            //Truyen vao Controller de phan trang
            page = page || 0;  //Mac dinh truyen pahe = 0 vao
            var config = { //Cai cho Config nay dung de truyen so ban ghi trong 1 trang vao Controller
                params: {
                    keyword: $scope.keyword,
                    page: page, //Mac dinh la trang so 0
                    pageSize: 2  //So ban ghi la 2
                }
            }
            //--------------
            apiService.get('/api/menu/getall', config, function (result) {
                //Phan trang, neu config la null thi no khong nhan tham so truyen vao
                $scope.menu = result.data.Items; //gan vao scope.menuGroup, giong viewBag(ng-model)
                $scope.page = result.data.Page;
                $scope.pagesCount = result.data.TotalPages;
                $scope.totalCount = result.data.TotalCount;
                //-----------------
            }, function () {
                console.log('Load menu failed.');
            });
        }

        $scope.getMenu();
    }
})(angular.module('umc.menu')); //goi module menu.module.js